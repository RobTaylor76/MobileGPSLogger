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

namespace Gps
{
	/// <summary>
	/// Description of GPSTrackFileWriter.
	/// </summary>
	public class GPSTrackFileWriter
	{
		//	GPSException is used to inform clients of exceptions via Event Notifications		
		public event ExceptionHandler FileWriterException;
		
		private GPSWrapper m_GPSWrapper = null;
					
		private StreamWriter m_swOutputFile = null;
				
		private double m_dbGPSPositionalAccuracy = 0.01d; // 10 metres for 2d lock...
		private long m_lTrackWriterLogFrequency = 10 * 10000000; //every 10 seconds...
		
		private Coordinate m_cdLastLoggedPosition = new Coordinate(0,0);
		private DateTime m_dtTimeOfLastWrite = new DateTime(0);

		public GPSTrackFileWriter( string p_strTrackFileName)
		{	
			OpenTrackFile(p_strTrackFileName);
		}
		
		public GPSTrackFileWriter(GPSWrapper p_gpsWrapper , string p_strTrackFileName)
		{

		 	m_GPSWrapper = p_gpsWrapper ; //new GPSWrapper();
			
//			m_GPSData = m_GPSWrapper.GPSDataParser;
						
//			m_GPSWrapper.GPSException += new ExceptionHandler(ExceptionHandler);
			
			m_GPSWrapper.GPSCoordUpdateEvent += new GPSFixHandler(GPSCoordUpdateEventHandler);
			
			
			OpenTrackFile(p_strTrackFileName);

		}
		
		~GPSTrackFileWriter()
		{
			// remove event handlers.....
			if( m_GPSWrapper != null)
			{

				m_GPSWrapper.GPSCoordUpdateEvent -= new GPSFixHandler(GPSCoordUpdateEventHandler);
			}
			CloseTrackFile();
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
		
		private void OpenTrackFile(string p_strTrackFileName)
		{
			try
			{
				m_swOutputFile = File.AppendText(p_strTrackFileName);
//			
				m_swOutputFile.WriteLine("");
				m_swOutputFile.WriteLine("H  COORDINATE SYSTEM");
				m_swOutputFile.WriteLine("U  LAT LON DEG");
				m_swOutputFile.WriteLine("");
				m_swOutputFile.WriteLine("H  LATITUDE    LONGITUDE    DATE      TIME     ALT    ;track");
				m_swOutputFile.Flush();
			}
			catch (Exception ex)
			{

				RaiseExceptionEvent("Error Occured In GPSTrackFileWriter.OpenTrackFile(...)",ex);
			}
			
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
		
		public void CloseTrackFile()
		{
			if (m_swOutputFile != null)
			{
				try
				{
					m_swOutputFile.Close();
				}
				catch (Exception ex)
				{

					RaiseExceptionEvent("Error Occured In GPSTrackFileWriter.CloseTrackFile()",ex);
				}
				m_swOutputFile = null;
			}
		}
		
		private  void GPSCoordUpdateEventHandler(object source , GPSCoordUpdateEventArgs args)
		{

			LogPosition(ref args.m_coCurrentGPSData);
										
		}
				
		public void LogPosition (ref GPSData p_gpsData)
		{

			bool blnLogPosition = false;
				
			
			if (m_swOutputFile != null)
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
						
						
						string strDate = dtTimeOfEvent.ToString("dd-MMM-yy ") + dtTimeOfEvent.ToString("T");
	
						string strOutput = "T "+ p_gpsData.LatestFixPosition.GetLatitude(CoordinateFormat.DecimalDegrees) +
											  " " +p_gpsData.LatestFixPosition.GetLongitude(CoordinateFormat.DecimalDegrees) + 
											  " " +strDate;
			
						try
						{
							m_swOutputFile.WriteLine(strOutput);
							m_swOutputFile.Flush();
												
							m_dtTimeOfLastWrite =  dtTimeOfEvent;
				            m_cdLastLoggedPosition = p_gpsData.LatestFixPosition;		
	
						}
						catch (Exception ex)
						{
					//Console.WriteLine(e.Message);
					// what to do?
							RaiseExceptionEvent("Error Occured In GPSTrackFileWriter.GPSCoordUpdateEventHandler(...)",ex);
						}
	
					}
				}
			
			}
				
//			Console.WriteLine("{0}" , test.GetLatitude(GPSData.GPSCoordFormat.DegreesMinutesSeconds));
//			Console.WriteLine("{0}" , test.GetLongitude(GPSData.GPSCoordFormat.DegreesMinutesSeconds));
//			Console.WriteLine("{0}" ,  test.TimeOfLastGPSFix);
			
		}	
				
		/*
		 * RaiseExceptionEvent - this will be single point of call to inform clients
		 * 						 of exceptions...
		 */
		private void RaiseExceptionEvent(string p_strMessage, Exception p_eException)
		{
			if (FileWriterException != null)
			{
	
				FileWriterException(this,new ExceptionEventArgs(p_strMessage,p_eException));

			}
		}	
		
		
		
		
	}
}
