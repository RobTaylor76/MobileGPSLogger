/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 08/10/2006
 * Time: 09:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;


namespace Gps
{
	
	public delegate void GPSFixHandler(object source , GPSCoordUpdateEventArgs args);
	public delegate void SpeedChangeHandler(object source , EventArgs args);
	
	public class GPSCoordUpdateEventArgs : EventArgs
	{
		public GPSData m_coCurrentGPSData ;
		
		public GPSCoordUpdateEventArgs(GPSData p_coCurrentData )
		{
			m_coCurrentGPSData = p_coCurrentData; //new Coordinate(p_coCurrentPosition);
		}
		
		public Coordinate CurrentPosition 
		{
			get
			{
				return m_coCurrentGPSData.LatestFixPosition;
			}
		}
	
		
	}
	
	
	public struct GPSData
	{
		
		private bool m_blnHaveGPSFix;
		
		private double m_dbCurrentSpeedKPH ;

		private double m_dbCurrentBearing ;
		
		private int m_iNoOfSatellitesBeingTracked ;
		
		private string 		m_strGPSTimeOfLastGPSFix;
		private DateTime 	m_dtTimeOfLastGPSFix ;
		
		private string []m_strSatellitesUsedForFix;
		
		private string m_strGPSTypeFix;
		
		private Coordinate m_coLatestFixPosition;
		
		public void Initialise()
		{
			
			m_blnHaveGPSFix = false;
			
			m_dbCurrentSpeedKPH = 0;
	
			m_dbCurrentBearing = 0;
			
			m_iNoOfSatellitesBeingTracked = 0;
			
			m_strGPSTimeOfLastGPSFix = "";
			
			m_strGPSTypeFix = "None";
			
			m_coLatestFixPosition = new Coordinate(0,0);
						
				
			//initialise the array od satellites use for fix..
			m_strSatellitesUsedForFix= new string [12];
			for (int i = 0; i< m_strSatellitesUsedForFix.Length ; i ++)
			{
				m_strSatellitesUsedForFix[i] = ""; 
			}			
			
		}
		

		public int NoOfTrackedSatellites
		{
			get
			{
				return m_iNoOfSatellitesBeingTracked;
			}
			set { m_iNoOfSatellitesBeingTracked = value; }			
		}	
		/*
		 * TimeOfLastFix - retuns the time returned on the Lestening Device (PC/PDA)
		 * 					  when the last GPS fix occured
		 * 
		 */		
		public DateTime TimeOfLastFix
		{
			get
			{
				return m_dtTimeOfLastGPSFix;
			}
			set { m_dtTimeOfLastGPSFix = value; }
			
		}
		/*
		 * GPSTimeOfLastFix - retuns the UTC time returned by the GPs receiver
		 * 					  when the last GPS fix occured
		 * 
		 */
		public string TimeOfLastFixGPS
		{
			get
			{
				string strTime ="00:00:00";
				if (m_strGPSTimeOfLastGPSFix.Length >= 6)
				{
					strTime = m_strGPSTimeOfLastGPSFix.Substring(0,2) + ":" + 
								m_strGPSTimeOfLastGPSFix.Substring(2,2) + ":" + 
								m_strGPSTimeOfLastGPSFix.Substring(4,2);
					
				}
				return strTime;
			}			
			set { m_strGPSTimeOfLastGPSFix = value; }
			
		}
		
		public string FixType
		{
			get
			{
				return m_strGPSTypeFix;
			}			
			set { m_strGPSTypeFix = value; }
			
		}
	
		
		public bool HaveGPSFix
		{
			get
			{
				return m_blnHaveGPSFix;
			}
			set { m_blnHaveGPSFix = value; }
		}
		
		public double CurrentSpeed(UnitOfMeasure unitofMeasure)
		{
 //  	1 kilometer = 0.539956803 nautical miles
 //  	1 kilometer = 0.621371192 miles
 //		1 miles = 5 280 feet
 			if (unitofMeasure == UnitOfMeasure.Mile) return (m_dbCurrentSpeedKPH*0.621371192d);
          	if (unitofMeasure == UnitOfMeasure.Kilometer) return (m_dbCurrentSpeedKPH);
 			if (unitofMeasure == UnitOfMeasure.NauticalMile) return (m_dbCurrentSpeedKPH*0.539956803d );
          	return 0;
		
		}

		public double CurrentSpeedKPH
		{
			get { return m_dbCurrentSpeedKPH; }
			set { m_dbCurrentSpeedKPH = value; }

		}
		public double CurrentBearing
		{
			get
			{
				return m_dbCurrentBearing;
			}	
			set { m_dbCurrentBearing = value; }

		}		
		
		public Coordinate LatestFixPosition
		{			
			get
			{				
				//return  m_coLatestFixPosition;  // Coordinate is now struct... a copy is returned...
				// now a class so need to copy it...
				//return new Coordinate(m_coLatestFixPosition);
				//now a struct again... i must document my thought processes...
				return  m_coLatestFixPosition;
			}

		}
		
		public double LatestFixPositionLatitude
		{
			get
			{
				return m_coLatestFixPosition.Latitude;
			}
			set
			{
				m_coLatestFixPosition.Latitude = value;
			}
			
		}
		
		public double LatestFixPositionLongitude
		{
			get
			{
				return m_coLatestFixPosition.Longitude;
			}
			set
			{
				m_coLatestFixPosition.Longitude = value;
			}
						
		}		
		
		public string [] SatellitesUsedForFix
		{
			get { return m_strSatellitesUsedForFix;}

		}
		
	}
	/// <summary>
	/// Description of GPSData.
	/// </summary>
	public class GPSDataParser
	{

		public event GPSFixHandler GPSCoordUpdateEvent;
		
		private GPSData m_CurrentGPSInfo= new GPSData();
		
		public GPSDataParser()
		{
			m_CurrentGPSInfo.Initialise();
		}
		
		public GPSData GetCurrentGPSData()
		{
			lock( this)
			{
				return m_CurrentGPSInfo;  // will return a copy of GPS Data...
			}
				
			
		}
		
		public void ParseNMEASentence(string strSentence)
		{
			if (!strSentence.StartsWith("$"))
			{
				return; // not a nmea sentence...
			}
			
			try
			{
				lock( this) // ensure GPS data in consistent state by using lock to control access...
				{
					string[] strSentenceComponents = strSentence.Split(',','*'); // split by * and ,
					
					if (strSentenceComponents[0].Length != 6)
					{
						return;// not a expected nmea sentence...
					}
					string strSentenceType = strSentenceComponents[0].Substring(strSentenceComponents[0].Length-3);
								
					if (strSentenceType.Equals("GGA")) //GGA Global Positioning System Fix Data
					{
					    	ParseGGASentence( strSentenceComponents);
					    	
					}
					else if (strSentenceType.Equals("VTG"))// VTG - Track made good and ground speed
					{
					    	ParseVTGSentence( strSentenceComponents);
					    	
					}
					else if (strSentenceType.Equals("GLL"))// GLL - Geographic position, Latitude and Longitude
					{
					    	ParseGLLSentence( strSentenceComponents);
					    	
					}			
					else if (strSentenceType.Equals("GSA"))//  GSA - to find out if 2 or 3d fix...
					{
					    	ParseGSASentence( strSentenceComponents);
					    	
					}	
					else if (strSentenceType.Equals("RMC"))//   RMC - Recommended minimum specific GPS/Transit data...
					{
					    	ParseRMCSentence( strSentenceComponents);
					    	
					}	
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Error Parsing GPS Sentence in GPSData.ParseNMEASentence(...)\n "+strSentence +"\n"+ex.Message,ex.InnerException);
			}
			
		}
		
		private void ParseGSASentence(  string []  p_strSentenceComponents)
		{		
/*
         GSA - GPS DOP and active satellites
        GSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*39
           A            Auto selection of 2D or 3D fix (M = manual)
           3            3D fix
           04,05...     PRNs of satellites used for fix (space for 12)
           2.5          PDOP (dilution of precision)
           1.3          Horizontal dilution of precision (HDOP)
           2.1          Vertical dilution of precision (VDOP)
             DOP is an indication of the effect of satellite geometry on
             the accuracy of the fix.
             
$GPGSA,A,3,22,19,03,11,,,,,,,,,4.4,2.4,3.7*3B
             
*/

			if(p_strSentenceComponents[2].Length>0 )
			{
				if (p_strSentenceComponents[2].Equals("3"))
				{
					m_CurrentGPSInfo.FixType = "3D";
				} 
				else if (p_strSentenceComponents[2].Equals("2"))
				{
					m_CurrentGPSInfo.FixType = "2D";
				} 
				else
				{
					m_CurrentGPSInfo.FixType = "None";
				} 

				//satelites used for fix...
				
				for (int i = 0; i< m_CurrentGPSInfo.SatellitesUsedForFix.Length ; i ++)
				{
				
					m_CurrentGPSInfo.SatellitesUsedForFix[i] = p_strSentenceComponents[i+3]; 
				}

			}
			
		}
		private void ParseGLLSentence(  string []  p_strSentenceComponents)
		{	
/*
         GLL - Geographic position, Latitude and Longitude
        GLL,4916.45,N,12311.12,W,225444,A
           4916.46,N    Latitude 49 deg. 16.45 min. North
           12311.12,W   Longitude 123 deg. 11.12 min. West
           225444       Fix taken at 22:54:44 UTC
           A            Data valid
             (Garmin 65 does not include time and status)
             
$GPGLL,5451.0090,N,00134.9302,W,205428.734,A*2D
             
*/
	
			bool blnDataValid = p_strSentenceComponents[6].StartsWith("A"); // has check sum info as well...
	
			if (blnDataValid)
			{
				double dbGPSFixLatitude = Coordinate.ConvertDegreesDecimalMinutesToDecimalDegrees( p_strSentenceComponents[2],
			                                                              	 p_strSentenceComponents[1]);
			
				double dbGPSFixLongitude = Coordinate.ConvertDegreesDecimalMinutesToDecimalDegrees( p_strSentenceComponents[4],
			                                                              	 p_strSentenceComponents[3]);
				
				//m_coLatestFixPosition = new Coordinate(dbGPSFixLatitude,dbGPSFixLongitude);
				
				m_CurrentGPSInfo.LatestFixPositionLatitude = dbGPSFixLatitude;
				m_CurrentGPSInfo.LatestFixPositionLongitude = dbGPSFixLongitude;
				
				m_CurrentGPSInfo.TimeOfLastFixGPS = p_strSentenceComponents[5];
				
				m_CurrentGPSInfo.TimeOfLastFix = DateTime.Now;
				
				if (GPSCoordUpdateEvent != null)
				{
					//CoordinateChange(this,new CoordinateChangeEventArgs(new Coordinate(m_coLatestFixPosition),m_dtTimeOfLastGPSFix));
//					CoordinateChange(this,new CoordinateChangeEventArgs(m_coLatestFixPosition,m_dtTimeOfLastGPSFix));
					GPSCoordUpdateEvent(this,new GPSCoordUpdateEventArgs(m_CurrentGPSInfo));

				}
			}
	
		}
		
		private void ParseVTGSentence(  string []  p_strSentenceComponents)
		{		
/*
 * 		$GPVTG	111.78	T		M	0.15	N	0.3	K*69
        VTG - Track made good and ground speed
        VTG,054.7,T,034.4,M,005.5,N,010.2,K
           054.7,T      True track made good
           034.4,M      Magnetic track made good
           005.5,N      Ground speed, knots
           010.2,K      Ground speed, Kilometers per hour

*/
			// if there is a bearing... extract it...
			if (p_strSentenceComponents[1].Length > 0)
			{
				m_CurrentGPSInfo.CurrentBearing =double.Parse(p_strSentenceComponents[1]);
//				Console.WriteLine("Travelling at bearing of {0} degrees",m_dbCurrentBearing );

			}
			// if there is a speed ... extract it...
			if (p_strSentenceComponents[7].Length > 0)
			{			
//				double dbNewSpeed = double.Parse(p_strSentenceComponents[7]);
//				
//				double dbSpeedChange = Math.Abs(dbNewSpeed - m_dbCurrentSpeedKPH);
//					
//				m_dbCurrentSpeedKPH  = dbNewSpeed;
//				m_dbCurrentSpeedMPH = m_dbCurrentSpeedKPH*0.621d;
//				
//				if (dbSpeedChange >	m_dbSpeedChangeEventTriggerKMPH)
//				{
//					if (SpeedChange != null)
//					{
//						SpeedChange(this,new EventArgs());
//						
//					}
//					
//				}
				m_CurrentGPSInfo.CurrentSpeedKPH=double.Parse(p_strSentenceComponents[7]);
//				m_dbCurrentSpeedMPH = m_dbCurrentSpeedKPH*0.621d;
				
//				Console.WriteLine("Travelling at {0} Kmph",m_dbCurrentSpeedKPH );
			}
	
		}
		private void ParseRMCSentence(  string []  p_strSentenceComponents)
		{
/*
        RMC - Recommended minimum specific GPS/Transit data
        RMC,225446,A,4916.45,N,12311.12,W,000.5,054.7,191194,020.3,E*68
           225446       Time of fix 22:54:46 UTC
           A            Navigation receiver warning A = OK, V = warning
           4916.45,N    Latitude 49 deg. 16.45 min North
           12311.12,W   Longitude 123 deg. 11.12 min West
           000.5        Speed over ground, Knots
           054.7        Course Made Good, True
           191194       Date of fix  19 November 1994
           020.3,E      Magnetic variation 20.3 deg East
           *68          mandatory checksum
 
			
//			p_strSentenceComponents[0];  // sentence type....
//			p_strSentenceComponents[1];  // Fix taken at 12:35:19 UTC - can have doubles?
//			p_strSentenceComponents[2];  // Navigation receiver warning A = OK, V = warning
//			p_strSentenceComponents[3];  // latitude 
//			p_strSentenceComponents[4];  // latitude direction (N / S)
//			p_strSentenceComponents[5];  // Longitude
//			p_strSentenceComponents[6];  // Longitude direction (E / W)
//			p_strSentenceComponents[7];  // Speed over ground, Knots 
//			p_strSentenceComponents[8];  // Course Made Good, True
//			p_strSentenceComponents[9];  // Date of fix  19 November 1994
//			p_strSentenceComponents[9];  // Magnetic variation 20.3 deg East
			
 */
 
 
 			// if there is a bearing... extract it...
			if (p_strSentenceComponents[8].Length > 0)
			{
				m_CurrentGPSInfo.CurrentBearing =double.Parse(p_strSentenceComponents[1]);
			}
			// if there is a speed ... extract it...
			if (p_strSentenceComponents[7].Length > 0)
			{			
				double dbSpeedInKnotts  = double.Parse(p_strSentenceComponents[7]);
				//1 knots = 1.85200 kilometers
				m_CurrentGPSInfo.CurrentSpeedKPH=dbSpeedInKnotts *1.85200d ;	
			}
 
 
			
		}
		private void ParseGGASentence(  string []  p_strSentenceComponents)
		{
			/*
GGA - Global Positioning System Fix Data
        GGA,123519,4807.038,N,01131.324,E,1,08,0.9,545.4,M,46.9,M, , *42
           123519       Fix taken at 12:35:19 UTC
           4807.038,N   Latitude 48 deg 07.038' N
           01131.324,E  Longitude 11 deg 31.324' E
           1            Fix quality: 0 = invalid
                                     1 = GPS fix
                                     2 = DGPS fix
           08           Number of satellites being tracked
           0.9          Horizontal dilution of position
           545.4,M      Altitude, Metres, above mean sea level
           46.9,M       Height of geoid (mean sea level) above WGS84
                        ellipsoid
           (empty field) time in seconds since last DGPS update
           (empty field) DGPS station ID number
           
$GPGGA,205429.734,5451.0088,N,00134.9305,W,1,05,1.6,101.7,M,47.7,M,0.0,0000*65
           
*/
//			p_strSentenceComponents[0];  // sentence type....
//			p_strSentenceComponents[1];  // Fix taken at 12:35:19 UTC - can have doubles?
//			p_strSentenceComponents[2]; // latitude 
//			p_strSentenceComponents[3]; // latitude direction (N / S)
//			p_strSentenceComponents[4]; // Longitude
//			p_strSentenceComponents[5]; // Longitude direction (E / W)
//			p_strSentenceComponents[6]; // Fix Quality - 
//			p_strSentenceComponents[7]; // no of satellites being tracked 
//			p_strSentenceComponents[8]; // Horizontal dilution of position ???
//			p_strSentenceComponents[9]; // Altitude, Metres, above mean sea level
//			p_strSentenceComponents[9]; // height units.. M -> metres
//			p_strSentenceComponents[10] // height units.. M -> metres
//			p_strSentenceComponents[11]; // Height of geoid (mean sea level) above WGS84 ellipsoid
//			p_strSentenceComponents[12] // Height of geoid units.. M -> metres



//			string [] strComponentDescriptions = {"Sentence Type" ,
//													"Fix taken at(UTC)",
//													"latitude ",
//													"latitude direction (N / S)",
//													"Longitude",
//													"Longitude direction (E / W)",
//													"Fix Quality - ",
//													"no of satellites being tracked ",
//													"Horizontal dilution of position ???",
//													"Altitude, Metres, above mean sea level",
//													"height units.. M -> metres",
//													"Height of geoid (mean sea level) above WGS84 ellipsoid",
//													"Height of geoid units.. M -> metres"};
//													
//													
//			for (int i = 1 ; i < strComponentDescriptions.Length ; i++)
//			{
//				Console.WriteLine("{0} -> {1}" ,strComponentDescriptions[i] ,p_strSentenceComponents[i] );
//			}
			
			m_CurrentGPSInfo.HaveGPSFix = !p_strSentenceComponents[6].Equals("0"); // 0 = invalid...
			
			// if there is a GPs fix.. use the info to get GPS fix co-ordinates...
			if (m_CurrentGPSInfo.HaveGPSFix)
			{
				double dbGPSFixLatitude = Coordinate.ConvertDegreesDecimalMinutesToDecimalDegrees( p_strSentenceComponents[3],
			                                                              	 p_strSentenceComponents[2]);
			
				double dbGPSFixLongitude = Coordinate.ConvertDegreesDecimalMinutesToDecimalDegrees( p_strSentenceComponents[5],
			                                                              	 p_strSentenceComponents[4]);
				
				//m_coLatestFixPosition = new Coordinate(dbGPSFixLatitude,dbGPSFixLongitude);
	
				
				m_CurrentGPSInfo.LatestFixPositionLatitude = dbGPSFixLatitude;
				m_CurrentGPSInfo.LatestFixPositionLongitude = dbGPSFixLongitude;
				
				

				m_CurrentGPSInfo.TimeOfLastFix = DateTime.Now;

				m_CurrentGPSInfo.TimeOfLastFixGPS = p_strSentenceComponents[1];
				
				if (GPSCoordUpdateEvent != null)
				{
					//CoordinateChange(this,new CoordinateChangeEventArgs(new Coordinate(m_coLatestFixPosition),m_dtTimeOfLastGPSFix));
					GPSCoordUpdateEvent(this,new GPSCoordUpdateEventArgs(m_CurrentGPSInfo));
					
				}

				
			}
			   
			
			m_CurrentGPSInfo.NoOfTrackedSatellites = int.Parse(p_strSentenceComponents[7]);
			    
		}
		

		
//Degrees and double Minutes to double Degrees..
//=IF(UPPER(G18)="S",(H18+(I18/60))*-1,(H18+(I18/60)))
// H18 = degrees...
// I18 = decmal minutes...
// If N/E (H18+(I18/60)))
// IF S,W (H18+(I18/60))*-1

//Degrees, Minutes & Seconds to double Degrees 								
// mins/secs to double degrees 
// E5 = seconds
// D5 = minutes 
// C5 = degrees
 // If North/East ->((((E5/60)+D5)/60)+C5)
 // If South/West -> (((E5/60)+D5)/60)+C5)*-1
			
//Degrees, Minutes & Seconds to Degrees & double Minutes
//  degress , double minutes = minutes + (seconds/60)
//

//double Degrees to  Degrees & double Minutes =
//double minutes =ABS(F12-(TRUNC(F12)))*60
// double part of number * 60 - absoluted...
// directional indiactors - >=IF(F12<0,"S","N")
//								=IF(F13<0,"W","E")

//double Degrees to Degrees, Minutes & Seconds 
// F12 = 52.657570305556	
// C12 =ABS(TRUNC(F12))								-> degrees
// D12  =TRUNC((ABS(F12)-C12)*60)					->minutes
//seconds = ((ABS(F12))*3600)-(D12*60)-(C12*3600) 	->seconds
//
// 

			
			
			//latitude -> 4807.038,N   Latitude 48 deg 07.038' N
			
			//Degrees & double Minutes	
			
		

		
	}
}
