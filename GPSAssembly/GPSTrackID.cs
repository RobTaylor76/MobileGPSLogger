/*
 * Created by SharpDevelop.
 * User: u771666
 * Date: 05/10/2007
 * Time: 15:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Gps
{
	/// <summary>
	/// Description of GPSTrackID.
	/// </summary>
	public struct GPSTrackID
	{
		private string m_strDeviceID ;//= null;
		private string m_strTrackID ;//= null;
				
		private DateTime m_dtCreationDate;
		
//		public GPSTrackID()
//		{
//			m_strDeviceID = System.Net.Dns.GetHostName();
//		}
		
		public void CreateTrackID()
		{
			m_dtCreationDate = DateTime.Now;
			m_strDeviceID = System.Net.Dns.GetHostName() ;
			m_strTrackID = m_strDeviceID + m_dtCreationDate.ToString(" dd-MMM-yy ") + m_dtCreationDate.ToString("T");
		}
		
		
		public string GetTrackID()
		{
			return m_strTrackID;
		}		
		
//		public string GetNextTrackID()
//		{
//			m_iTrackId++;
//			
//			return m_strDeviceID + "_" + m_iTrackId.ToString();
//		}
//		public static void Main(string[] args)
//		{
//			GPSTrackID id = new GPSTrackID();
//			
//			string trackID = id.GetNextTrackID();
//			
//			Console.WriteLine(trackID);
//			Console.ReadLine();
//		}		
		
	}
	
	public struct GPSTrackInfo
	{
		private Coordinate 	m_coPosition ;
		private string 		m_strTrackID ; //= null;
		private DateTime 	m_dtTimeAtPosition;
		private double 		m_dbSpeedKPH ;

		private bool		m_blnExported; // = false;
		private bool		m_blnExporting; // = false;
		
		public GPSTrackInfo(string strTrackId, Coordinate position, DateTime when)
		{
			m_coPosition = position;
			m_strTrackID = strTrackId;
			m_dtTimeAtPosition = when;
			m_blnExported=false;
			m_blnExporting = false;
			m_dbSpeedKPH= 0;
		}
		
		public Coordinate Where
		{
			get
			{
				return m_coPosition;
			}
		}
		public DateTime When
		{
			get
			{
				return m_dtTimeAtPosition;
			}
		}	
		
		public string TrackId
		{
			get
			{
				return m_strTrackID;
			}
		}	
		
		public bool Exported
		{
			get
			{
				return m_blnExported;
			}
			set
			{
				m_blnExported = value;
			}
		}
		
		public bool Exporting
		{
			get
			{
				return m_blnExporting;
			}
			set
			{
				m_blnExporting = value;
			}
		}
	
	
		public double Speed(UnitOfMeasure unitofMeasure)
		{
 //  	1 kilometer = 0.539956803 nautical miles
 //  	1 kilometer = 0.621371192 miles
 //		1 miles = 5 280 feet
 			if (unitofMeasure == UnitOfMeasure.Mile) return (m_dbSpeedKPH*0.621371192d);
          	if (unitofMeasure == UnitOfMeasure.Kilometer) return (m_dbSpeedKPH);
 			if (unitofMeasure == UnitOfMeasure.NauticalMile) return (m_dbSpeedKPH*0.539956803d );
          	return 0;
		
		}

		public double SpeedKPH
		{
			get { return m_dbSpeedKPH; }
			set { m_dbSpeedKPH = value; }

		}
}
}
