/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 13/10/2006
 * Time: 21:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using Gps;
using Db4objects.Db4o;

namespace Gps
{
	/// <summary>
	/// Description of GPSTrackDBWriter.
	/// </summary>
	public class GPSTrackDBWriter
	{
		//	GPSException is used to inform clients of exceptions via Event Notifications		
		public event ExceptionHandler DBWriterException;
		
		private GPSWrapper m_GPSWrapper = null;
					
//		private StreamWriter m_swOutputFile = null;
				
		private IObjectContainer m_dbTrack= null;
				
		private string m_strTrackDBName = null;
		
		private double m_dbGPSPositionalAccuracy = 0.01d; // 10 metres for 2d lock...
		private long m_lTrackWriterLogFrequency = 10 * 10000000; //every 10 seconds...
		
		private Coordinate m_cdLastLoggedPosition = new Coordinate(0,0);
		private DateTime m_dtTimeOfLastWrite = new DateTime(0);
		
		private bool m_blnLoggingStarted = false;

		private GPSTrackID m_trTrackId; // = null;
		
		
		public GPSTrackDBWriter( string p_strTrackDBName)
		{
				
//			m_GPSWrapper.GPSException += new ExceptionHandler(ExceptionHandler);	
			
			m_strTrackDBName = p_strTrackDBName;

		}
		
		public GPSTrackDBWriter(GPSWrapper p_gpsWrapper , string p_strTrackDBName)
		{

		 	m_GPSWrapper = p_gpsWrapper ; //new GPSWrapper();
			
						
//			m_GPSWrapper.GPSException += new ExceptionHandler(ExceptionHandler);
			
			m_GPSWrapper.GPSCoordUpdateEvent += new GPSFixHandler(GPSCoordUpdateEventHandler);
			
			m_strTrackDBName = p_strTrackDBName;

		}
		
		~GPSTrackDBWriter()
		{
			// remove event handlers.....
			
			if (m_GPSWrapper != null)
			{
				m_GPSWrapper.GPSCoordUpdateEvent -= new GPSFixHandler(GPSCoordUpdateEventHandler);
			}
			CloseTrackDB();
		}
		
		public double PositionalAccuracy
		{
			get
			{
				return m_dbGPSPositionalAccuracy;
			}
			
		}
		/*
		 * LogFrequency - the miniumm time between tracks being written to log...
		 * */
		public long LogFrequency
		{
			get
			{
				return m_lTrackWriterLogFrequency/10000000; // stored as ticks for optimised comparison
			}
			set 
			{
				m_lTrackWriterLogFrequency = value*10000000;// stored as ticks for optimised comparison
			}
		}
		
		private bool OpenTrackDB()
		{
			if (m_dbTrack == null)
			{
				m_dbTrack = Db4oFactory.OpenFile(m_strTrackDBName);
			}
			
			return (m_dbTrack != null);
			
//			
//			m_swOutputFile = File.AppendText(p_strTrackFileName);
//			
//			m_swOutputFile.WriteLine("");
//			m_swOutputFile.WriteLine("H  COORDINATE SYSTEM");
//			m_swOutputFile.WriteLine("U  LAT LON DEG");
//			m_swOutputFile.WriteLine("");
//			m_swOutputFile.WriteLine("H  LATITUDE    LONGITUDE    DATE      TIME     ALT    ;track");
//			m_swOutputFile.Flush();

			// accessDb4o
			
//			try
//			{
//			    // do something with db4o
//			}
//			finally
//			{
//			    db.Close();
//			} 
			
			
			
		}
		
		
		
//		public void ProcessNMEAFile(string p_strInputFileName )
//		{
//			
//			string buffer;
//			
//			StreamReader inputFile = File.OpenText(p_strInputFileName);
//			
//			while ((buffer = inputFile.ReadLine() ) != null)
//			{					
//				m_GPSData.ParseNMEASentence(buffer);
//			}
//
//			inputFile.Close();
//			
//		}
		
		public void StartLogging()
		{
			// Get new Track Id...
			try
			{
				bool blnOpenOk = OpenTrackDB();

				if (blnOpenOk)
				{
				//m_strCurrentTrackID
				
					m_trTrackId = new GPSTrackID();
					m_trTrackId.CreateTrackID();
					
					//store the new track id in Db 
					// we now store a track ID for every track in Database...
					m_dbTrack.Set(m_trTrackId);
					
					m_dbTrack.Commit();
					
					m_blnLoggingStarted = true;
				}
			}
			catch (Exception ex)
			{
				//Console.WriteLine(e.Message);
				// what to do?
					RaiseExceptionEvent("Error Occured In GPSTrackDBWriter.StartLogging()",ex);
			}
			
		}
		
		public void StopLogging()
		{
			m_blnLoggingStarted = false;
			CloseTrackDB();
		}
		
		private void CloseTrackDB()
		{
			if (m_dbTrack != null)
			{

				try
				{
					m_dbTrack.Close();
					m_dbTrack = null;
				}
				catch (Exception ex)
				{
					RaiseExceptionEvent("Error Occured In GPSTrackDBWriter.CloseTrackDB()",ex);

				}
				
			}
		}
		
		private  void GPSCoordUpdateEventHandler(object source , GPSCoordUpdateEventArgs args)
		{

			LogPosition(ref args.m_coCurrentGPSData);
										
		}
				
		public void LogPosition (ref GPSData p_gpsData)
		{
				
			bool blnLogPosition = false;
				
			
			if ((m_dbTrack != null) && m_blnLoggingStarted)
			{
				//T N54.85655 W1.56742 09-OCT-00 12:01:01

				DateTime dtTimeOfEvent = p_gpsData.TimeOfLastFix; 
								
				TimeSpan tsDifference =	dtTimeOfEvent.Subtract(m_dtTimeOfLastWrite);
									
				if (tsDifference.Ticks >= m_lTrackWriterLogFrequency )
				{
					if ((m_cdLastLoggedPosition.Latitude == 0) ||
						(m_cdLastLoggedPosition.Longitude == 0))
					{			
						blnLogPosition = true;
					}
					else
					{
						// if 2d lock then accuracy -> 10m
						// else 2.5m?
						// get distance between two points...
	
						if (p_gpsData.FixType == "3D" )
						{
							m_dbGPSPositionalAccuracy = 0.0025d; //2.5 m
							
						}
						else
						{
							m_dbGPSPositionalAccuracy = 0.01d; //10 m
							
						}
						
						double dbDistanceBetweenPoints = Coordinate.DistanceBetweenPoints(m_cdLastLoggedPosition,p_gpsData.LatestFixPosition, UnitOfMeasure.Kilometer);
						
						if (dbDistanceBetweenPoints > m_dbGPSPositionalAccuracy)
						{
							blnLogPosition = true;
							
						}
					}
						
					if (blnLogPosition)
					{
						
						GPSTrackInfo newPosition = new GPSTrackInfo(m_trTrackId.GetTrackID(),p_gpsData.LatestFixPosition,dtTimeOfEvent);
						newPosition.SpeedKPH = p_gpsData.CurrentSpeedKPH;
						try
						{
							m_dbTrack.Set(newPosition);
							m_dbTrack.Commit();
							
							m_dtTimeOfLastWrite =  dtTimeOfEvent;
						    m_cdLastLoggedPosition = p_gpsData.LatestFixPosition;
	
						}
						catch (Exception ex)
						{
									RaiseExceptionEvent("Error Occured In GPSTrackDBWriter.GPSCoordUpdateEventHandler(...)",ex);
	
						}
					}			
				}
			
		

			}
			
//			Console.WriteLine("{0}" , test.GetLatitude(GPSData.GPSCoordFormat.DegreesMinutesSeconds));
//			Console.WriteLine("{0}" , test.GetLongitude(GPSData.GPSCoordFormat.DegreesMinutesSeconds));
//			Console.WriteLine("{0}" ,  test.TimeOfLastGPSFix);
			
		}	
				
//	      public  void adjustClassNames()
//	      {
//            IObjectContainer db = Db4oFactory.OpenFile(m_strTrackDBName);
//         
//            IStoredClass[] classes = db.Ext().StoredClasses();
//            for (int i = 0; i < classes.Length; i++) {
//                IStoredClass storedClass = classes[i];
//                String name = storedClass.getName();
//                
//                Console.WriteLine(name);
//                
//   //             String newName = null;
//   //             int pos = name.IndexOf(",");
////                if(pos == -1){
////                    for(int j = 0; j < ASSEMBLY_MAP.Length; j += 2){
////                        pos = name.IndexOf(ASSEMBLY_MAP[j]);
////                        if(pos == 0){
////                            newName = name + ", " + ASSEMBLY_MAP[j + 1];
////                            break;
////                        }
////                    }
////                }
////  //              if(newName != null){
//   //                 storedClass.rename(newName);
//   //             }
//            }
//            objectContainer.close();
//        }
				/*
		 * RaiseExceptionEvent - this will be single point of call to inform clients
		 * 						 of exceptions...
		 */
		private void RaiseExceptionEvent(string p_strMessage, Exception p_eException)
		{
			if (DBWriterException != null)
			{
	
				DBWriterException(this,new ExceptionEventArgs(p_strMessage,p_eException));

			}
		}
		
		
		
	}
}
