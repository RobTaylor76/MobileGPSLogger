/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 13/10/2006
 * Time: 19:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Gps
{
	public enum CoordinateFormat 
	{
			DecimalDegrees,
			DegreesMinutesSeconds,
			DegreesDecimalMinutes,
			DecimalDegreesNumeric
	}
		
	
	public enum UnitOfMeasure
	{
		Mile,
		Kilometer,
		NauticalMile
	}
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public struct Coordinate
	{
		private double m_dbGPSLatitude;
		private double m_dbGPSLongitude;
		
		public double Latitude
		{
			get
			{
				return m_dbGPSLatitude;
			}
			set
			{
				m_dbGPSLatitude = value;
			}
			
		}
		
		public double Longitude
		{
			get
			{
				return m_dbGPSLongitude;
			}
			set
			{
				m_dbGPSLongitude = value;
			}
						
		}
				
		public Coordinate(double p_dbLatitude , double p_dbLongitude)
		{
			m_dbGPSLatitude = p_dbLatitude;
			m_dbGPSLongitude = p_dbLongitude;
		}
		
		public Coordinate( Coordinate p_coToCopy)
		{
			m_dbGPSLatitude = p_coToCopy.m_dbGPSLatitude;
			m_dbGPSLongitude = p_coToCopy.m_dbGPSLongitude;
		
		}
		
		public string GetLatitude(CoordinateFormat p_gpsFormat)
		{
			return ConvertCoordinateToString(m_dbGPSLatitude,p_gpsFormat,true);
			
		}
		
		public string GetLongitude(CoordinateFormat p_gpsFormat)
		{
			return ConvertCoordinateToString(m_dbGPSLongitude,p_gpsFormat,false);
			
		}		

		static private string ConvertCoordinateToString(double p_dbCoordinate ,
		                                         		CoordinateFormat p_gpsFormat,
		                                         		bool blnLatitude) 
		{
			string strCoordinateAsString= "";
			
			// Need to know if N/S E/W - use blnLatitude to distinguish between two types
			//
			switch (p_gpsFormat)
			{
				case CoordinateFormat.DecimalDegreesNumeric:
					strCoordinateAsString = p_dbCoordinate.ToString();
					
					break;
					
				case CoordinateFormat.DecimalDegrees:
					// simply convert to String...
					strCoordinateAsString = Math.Abs(p_dbCoordinate ).ToString();
					break;
				case CoordinateFormat.DegreesMinutesSeconds:
					
//	Degrees (°)	Minutes (')	Seconds (")
//N	54	51	0.546000
//W	1	34	55.800000
					
//double Degrees to Degrees, Minutes & Seconds 
// F12 = 52.657570305556	
// C12 =ABS(TRUNC(F12))								-> degrees
// D12  =TRUNC((ABS(F12)-C12)*60)					->minutes
//seconds = ((ABS(F12))*3600)-(D12*60)-(C12*3600) 	->seconds

					double dbAbsValue  = Math.Abs(p_dbCoordinate );
					
					double dbDegrees =Math.Round(dbAbsValue);
					
					if (dbDegrees > dbAbsValue)
					{
						dbDegrees -= 1.0;
					}
					
					double dbMinutes = (dbAbsValue - dbDegrees)* 60; 
					
					if ( dbMinutes  < Math.Round((dbAbsValue - dbDegrees)* 60))
					{
						dbMinutes = Math.Round(dbMinutes) -1;
					}
					else
					{
						dbMinutes = Math.Round(dbMinutes) -1;
					}
					
					double dbSecond  = ((dbAbsValue - dbDegrees)*3600) - (dbMinutes*60) ;
					
					if (dbSecond > 60)
					{
						dbSecond-=60;
						dbMinutes++;
					}
					
					string strDegrees =  dbDegrees.ToString();
					string strMinutes =  dbMinutes.ToString();
					string strSecond =  dbSecond.ToString();


					
					strCoordinateAsString =  strDegrees + " " + strMinutes + " " + strSecond;

					break;
				case CoordinateFormat.DegreesDecimalMinutes:
//Degrees & Decimal Minutes		
//N	54	51.009100000
//W	1	34.930000000
					
//double Degrees to  Degrees & double Minutes =
//double minutes =ABS(F12-(TRUNC(F12)))*60
// double part of number * 60 - absoluted...
// directional indiactors - >=IF(F12<0,"S","N")
//								=IF(F13<0,"W","E")					break;
				default:
					//??? assume GPSCoordFormat.DecimalDegrees
					strCoordinateAsString = p_dbCoordinate.ToString();	
					break;
			}	
			
			string strCompassID = "";
			
			if (p_gpsFormat != CoordinateFormat.DecimalDegreesNumeric)
			{
			
				if ( blnLatitude)
				{
					//N or S
					if (p_dbCoordinate < 0)
					{
						strCompassID="S";
					}
					else
					{
						strCompassID="N";							
					}					
				}
				else
				{
					//E or W
					if (p_dbCoordinate < 0)
					{
						strCompassID="W";
					}
					else
					{
						strCompassID="E";							
					}
							
				}		
			}
			return strCompassID + strCoordinateAsString;
		}
		
		
		static public double ConvertDegreesDecimalMinutesToDecimalDegrees( string p_strCompassDirection,
		                                                           string p_strCoordinate)
		{
			
			double dbtDecimalDegrees =0;
			
			int iIndexOfDot = p_strCoordinate.IndexOf('.');
			
			string strDegrees = p_strCoordinate.Substring(0,iIndexOfDot-2);

			string strMinutes = p_strCoordinate.Substring(iIndexOfDot-2);

			
			dbtDecimalDegrees = double.Parse(strDegrees) + (double.Parse(strMinutes)/60);
			
			if ( p_strCompassDirection.Equals("S") || p_strCompassDirection.Equals("W"))
			{
				dbtDecimalDegrees*=-1;
			}
	
//			Console.WriteLine("{0} {1} {2} ---> {3}" ,p_strCompassDirection ,strDegrees ,strMinutes,dbtDecimalDegrees );
			return dbtDecimalDegrees;
		}
		
		public static double DistanceBetweenPoints(Coordinate a, 
		                                           Coordinate  b , 
		                                           UnitOfMeasure unitofMeasure)
		{
			
//Explanation of terms
//L1 	= 	latitude at the first point (degrees)
//L2 	= 	latitude at the second point (degrees)
//G1 	= 	longitude at the first point (degrees)
//G2 	= 	longitude at the second point (degrees)
//DG 	= 	longitude of the second point minus longitude of the first point (degrees)
//DL 	= 	latitude of the second point minus latitude of the first point (degrees)
//D 	= 	computed distance (km)
//
//Definitions
//
//    * South latitudes are negative.
//    * East longitudes are positive.
//    * Great circle distance is the shortest distance between two points on a sphere. 
//		This coincides with the circumference of a circle which passes through both points and the centre of the sphere.
//    * Geodesic distance is the shortest distance between two points on a spheroid.
//    * Normal section distance is formed by a plane on a spheroid containing a point at one end of the line and the normal
//		of the point at the other end. For all practical purposes, the difference between a normal section and a geodesic distance is insignificant.
//
			
//Great Circle Distance (Based on Spherical trigonometry)
//
//This method calculates the great circle distance, and is based on spherical trigonometry, and assumes that:
//
//    * 1 minute of arc is 1 nautical mile
//    * 1 nautical mile is 1.852 km
//
//D = 1.852 * 60 * ARCOS ( SIN(L1) * SIN(L2) + COS(L1) * COS(L2) * COS(DG))
//
//Note: If your calculator returns the ARCOS result as radians you will have to convert the radians to degrees before multiplying by 60 and 1.852 degrees = (radians/PI)*180, where PI=3.141592654...
			      // Dim angle As Double = Math.PI * degrees / 180.0
 
			double L1 =  (Math.PI * a.m_dbGPSLatitude)/180d; // -> radians
			double L2 =  (Math.PI * b.m_dbGPSLatitude)/180d; // -> radians
				
			
			double DG = (Math.PI *(a.m_dbGPSLongitude - b.m_dbGPSLongitude)) /180d;// -> radians
			
			double radians = Math.Acos( (Math.Sin(L1) * Math.Sin(L2)) +
			                           (Math.Cos(L1) * Math.Cos(L2) *  Math.Cos(DG)));
          		
          	double degrees = (radians / Math.PI) * 180d;
          	
          	//degrees = (radians/PI)*180
          	double nauticalMiles = 60d * degrees; //1 nautical miles = 1.15077945 miles
          	double miles  = 60d * degrees * 1.15077945d;
          	double kilometers = 1.852d * 60d * degrees; 
          	
//          	if (unitofMeasure == UnitOfMeasure.Mile) return (60d * degrees * 1.15077945d);
//          	if (unitofMeasure == UnitOfMeasure.Kilometer) return (1.852d * 60d * degrees);
//          	if (unitofMeasure == UnitOfMeasure.NauticalMile) return (60d * degrees );
          	if (unitofMeasure == UnitOfMeasure.Mile) return (miles);
          	if (unitofMeasure == UnitOfMeasure.Kilometer) return (kilometers);
          	if (unitofMeasure == UnitOfMeasure.NauticalMile) return (nauticalMiles );
          	return 0;

			
		}
	}
}
