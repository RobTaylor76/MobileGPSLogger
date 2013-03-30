/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 05/10/2007
 * Time: 20:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Db4objects.Db4o;
using xnatest;
using System.IO;
using Gps;


namespace ExportGPSInfo
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string m_strTrackingDBFileName = null;
		private string m_strGPSExportFile= null;
		private string m_strLogMyPositionURL = null;
		private string m_strErrorLogFile = null;
		
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			XmlParser xp = new XmlParser();
		
			xp.OnElementStart += new OnElementStartD(xp_OnElementStart);
			//xp.OnElementEnd += new OnElementEndD(xp_OnElementEnd);
			//xp.OnElementData += new OnElementDataD(xp_OnElementData);			
			
			xp.Load("gpstracker.xml");
			
			xp.Parse();

			
		}
		void xp_OnElementStart(string name, string ns, int numAttribs, Array attribs)
		{
		   // throw new Exception("The method or operation is not implemented.");
		    
		   	if (numAttribs == 0) return;
		    	
		   	attr valueAttribute =  (attr)attribs.GetValue(0);
		   	

		   	
		    if (name == "DatabaseExportFile")
		    {
		    	m_strGPSExportFile = valueAttribute.attrVal ;
		    }
		    else if (name == "TrackingDatabase")
		    {
		    	m_strTrackingDBFileName = valueAttribute.attrVal;
		    }		    
		   	else if (name == "LogMyPositionURL")
		    {
		    	m_strLogMyPositionURL = valueAttribute.attrVal;
		    }		     
		   	else if (name == "ErrorLogFile")
			{
			    	m_strErrorLogFile = valueAttribute.attrVal ;
		    }
		    	
		}			
		
		void BtnExportClick(object sender, EventArgs eventa)
		{
			
			
		
//create the alias before opening the file!!!		
//Db4objects.Db4o.Config.WildcardAlias alias = new Db4objects.Db4o.Config.WildcardAlias( "Gps.*, PPC_Test02","Gps.*, GPSAssembly");
//
//Db4oFactory.Configure().AddAlias(alias);
//
// alias = new Db4objects.Db4o.Config.WildcardAlias( "Gps.*, GPSAssembly","Gps.*, PPC_Test02");
//
//Db4oFactory.Configure().AddAlias(alias);
		
			IObjectContainer dbTrack =  Db4oFactory.OpenFile(m_strTrackingDBFileName);
	//		IObjectContainer dbTrack =  Db4oFactory.OpenFile("D:\\DevelopmentWork\\ScratchPad\\Output\\test.db");
			
	//Argument '1': cannot convert from 'WildcardAlias' to 'Db4objects.Db4o.Config.IAlias' (CS1503) - D:\DevelopmentWork\c#\ExportGPSInfo\MainForm.cs:89,3		
	
//	Db4objects.Db4o.Reflect.IReflectClass[] classes =  dbTrack.Ext().KnownClasses();
//            for (int i = 0; i < classes.Length; i++) 
//            {
//                Db4objects.Db4o.Reflect.IReflectClass storedClass = classes[i];
//                storedClass.GetType().ToString();
//                String name = storedClass.GetType().ToString();
//                
////                String newName = null;
////                int pos = name.IndexOf(",");
////                if(pos == -1){
////                    for(int j = 0; j < ASSEMBLY_MAP.Length; j += 2){
////                        pos = name.IndexOf(ASSEMBLY_MAP[j]);
////                        if(pos == 0){
////                            newName = name + ", " + ASSEMBLY_MAP[j + 1];
////                            break;
////                        }
////                    }
////                }
////                if(newName != null){
////                    storedClass.rename(newName);
////                }
//            }
//          //  objectContainer.close();
//       // }		

//Db4objects.Db4o.Config.TypeAlias alias = new Db4objects.Db4o.Config.TypeAlias( "Gps.GPSTrackInfo, PPC_Test02","Gps.GPSTrackInfo, GPSAssembly");
//Db4oFactory.Configure().AddAlias(alias);
//alias = new Db4objects.Db4o.Config.TypeAlias( "Gps.Coordinate, PPC_Test02","Gps.Coordinate, GPSAssembly");
//Db4oFactory.Configure().AddAlias(alias);
//alias = new Db4objects.Db4o.Config.TypeAlias( "Gps.GPSTrackID, PPC_Test02","Gps.GPSTrackID, GPSAssembly");
//#
//Db4oFactory.Configure().AddAlias(alias);
//		int iCount = 0;
 		StreamWriter swOutputFile = null;
		string strCurrTrackId = "############";
		
		try{
				 
		  	Comparison<GPSTrackInfo> trackCmp = new   
		    Comparison<GPSTrackInfo>(delegate(GPSTrackInfo s1, GPSTrackInfo s2)
		    {
		      return s1.When.CompareTo(s2.When);
		    });
		    // Native Query using Comparison
		
		    Predicate<GPSTrackInfo> trackPred = new Predicate<GPSTrackInfo>(delegate(GPSTrackInfo item)
		     {
		      return true; //item.Exported == false;
		     });
		    
		     IList<GPSTrackInfo>   results = dbTrack.Query<GPSTrackInfo>(trackPred, trackCmp);

		 	  swOutputFile = File.AppendText(m_strGPSExportFile);
//			

    			foreach (GPSTrackInfo item in results)
		    	{
					GPSTrackInfo trackInfo = item;
					
				//	m_swOutputFile.WriteLine(trackInfo.TrackId);
					
//		    	}

/////Db4oFactory.Configure().AddAlias(
///// new Db4objects.Db4o.Config.Alias(
/////    "*,ExportGPSInfo",
/////   "*, ASPAssemblyNameJunk456CurrentSession"));			
//			
//			try{
//				 
//				IObjectSet result = dbTrack.Get(typeof(GPSTrackID));
//
//				foreach (object item in result)
//		    	{
//					GPSTrackID trackID = (GPSTrackID)item;
//					
//					m_swOutputFile.WriteLine(trackID.GetNextTrackID());
//					
//		    	}
//				
//			 	result = dbTrack.Get(typeof(GPSTrackInfo));
				
//				GPSTrackInfo trackInfo = null;
//				// should only be at most one...
//				foreach (object item in result)
//		    	{
//					trackInfo = (GPSTrackInfo)item;
					
//					m_swOutputFile.WriteLine(trackInfo.TrackId + " " + trackInfo.When);
//					m_cdLastLoggedPosition = args.CurrentPosition;
					
					if (strCurrTrackId !=	trackInfo.TrackId)
    				{
						strCurrTrackId =trackInfo.TrackId; 
		     			writeTrackHeader(swOutputFile);
	
    				}
					
					
					string strDate = trackInfo.When.ToString("dd-MMM-yy ") + trackInfo.When.ToString("T");

					string strOutput = "T "+ trackInfo.Where.GetLatitude(CoordinateFormat.DecimalDegrees) +
										  " " +trackInfo.Where.GetLongitude(CoordinateFormat.DecimalDegrees) + 
										  " " +strDate;
					swOutputFile.WriteLine(strOutput);
					
					//iCount++;
					
					//if(iCount < 514)
					//{
					trackInfo.Exported = true;
					dbTrack.Set(trackInfo);
					//}
					
		    	}
				
				//m_swOutputFile.WriteLine(result.Count);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.Message);
			}
			finally
			{
				dbTrack.Close();
				swOutputFile.Flush();
				swOutputFile.Close();

			}
		
		

			
			
		}
		
		private void writeTrackHeader(StreamWriter swOutputFile)
		{
			swOutputFile.WriteLine("");
			swOutputFile.WriteLine("H  COORDINATE SYSTEM");
			swOutputFile.WriteLine("U  LAT LON DEG");
			swOutputFile.WriteLine("");
			swOutputFile.WriteLine("H  LATITUDE    LONGITUDE    DATE      TIME     ALT    ;track");
			swOutputFile.Flush();
		 }	
//		public class ArbitraryQuery : Predicate<GPSTrackInfo>
//		{
//			private bool m_blnExported;
//			private bool m_blnBeingExported;
//			
//			public ArbitraryQuery(bool blnExported , bool blnBeingExported)
//			{
//				_points=points;
//			}
//			
//			public bool Match(GPSTrackInfo trackInfo)
//			{
//				
//				return ((trackInfo.Exported == m_blnExported) && (trackInfo.Exporting==m_blnBeingExported));
//			}
//		}
////
////	 Comparison<StockItem> stockCmp = new
//		//    Comparison<StockItem>(delegate(StockItem s1, StockItem s2)
//		//    {
//		//      return s2.Title.CompareTo(s1.Title);
//		//    });
//
//		
//		public class QuerySorter extends Comparison<GPSTrackInfo>
//		{
//			private bool m_blnExported;
//			private bool m_blnBeingExported;
//			
//			public int compare(GPSTrackInfo t1, GPSTrackInfo t2)
//			{
//				if (t1. < t2.getDateExec()) return -1;
//				if (t1.getDateExec() > t2.getDateExec()) return 1;
//				return 0;
//			}
//		}
		void BtnLogLocationWSClick(object sender, System.EventArgs e)
		{
			try
			{			
				LogLocationService service = new LogLocationService();
				
				service.Url = m_strLogMyPositionURL;
			
				PositionalLogEntryRequest request = new PositionalLogEntryRequest();
				
				PositionalLogEntry[] logEntries = new PositionalLogEntry[10]; //TODO: need to know how many positions...
				
				Credentials credentials = new Credentials();
				credentials.UserID = "u771666";
				credentials.AccessToken = "[1212121]";
				credentials.Password = "Password";
				
				request.Credentials = credentials;
				
				request.LogEntry = getLogEntries();
				
				PositionalLogEntryResponse response = service.LogLocation(request);
				MessageBox.Show(response.ReturnCode,"PositionalLogEntryResponse" );
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, ex.Message);
				LogError("An error occurred in -> getLogEntries()...", ex);


			}
		}
		
		private PositionalLogEntry[] getLogEntries()
		{
			IObjectContainer dbTrack =  Db4oFactory.OpenFile(m_strTrackingDBFileName);

		PositionalLogEntry[] logEntries = null;

		try{
				
// RT - there is no need to sort the results for the WS?			
//		  	Comparison<GPSTrackInfo> trackCmp = new   
//		    Comparison<GPSTrackInfo>(delegate(GPSTrackInfo s1, GPSTrackInfo s2)
//		    {
//		      return s1.When.CompareTo(s2.When);
//		    });
//		    // Native Query using Comparison
//		
		    Predicate<GPSTrackInfo> trackPred = new Predicate<GPSTrackInfo>(delegate(GPSTrackInfo item)
		     {
		      return true; //item.Exported == false;
		     });
//		    
//		     IList<GPSTrackInfo>   results = dbTrack.Query<GPSTrackInfo>(trackPred, trackCmp);
		     IList<GPSTrackInfo>   results = dbTrack.Query<GPSTrackInfo>(trackPred);

			//iCount = results.Count;

			logEntries = new PositionalLogEntry[results.Count]; //TODO: need to know how many positions...
	     
			int iCurrentPosition = 0;
    			foreach (GPSTrackInfo item in results)
		    	{
					GPSTrackInfo trackInfo = item;
					
					ExportGPSInfo.Coordinate coordinate = new Coordinate();

					coordinate.Latitude = trackInfo.Where.GetLatitude(CoordinateFormat.DecimalDegrees);
					coordinate.Longitude = trackInfo.Where.GetLongitude(CoordinateFormat.DecimalDegrees);
					coordinate.Format = CoordinateFormats.DecimalDegrees;
					
					PositionalLogEntry  entry = new PositionalLogEntry();
				
					entry.Position = coordinate;
					entry.Date = trackInfo.When;
					entry.Time = trackInfo.When;
					entry.TrackID = trackInfo.TrackId;
					logEntries[iCurrentPosition] = entry;
//					string strDate = trackInfo.When.ToString("dd-MMM-yy ") + trackInfo.When.ToString("T");

//					string strOutput = "T "+ trackInfo.Where.GetLatitude(CoordinateFormat.DecimalDegrees) +
//										  " " +trackInfo.Where.GetLongitude(CoordinateFormat.DecimalDegrees) + 
//										  " " +strDate;
//					m_swOutputFile.WriteLine(strOutput);
					
					//iCount++;
					
					//if(iCount < 514)
					//{
					trackInfo.Exported = true;
					dbTrack.Set(trackInfo);
					//}
					iCurrentPosition++;
		    	}
				
				//m_swOutputFile.WriteLine(result.Count);
			}
			catch (Exception ex)
			{
	//			MessageBox.Show(ex.Message, ex.Message);
				LogError("An error occurred in -> getLogEntries()...", ex);
			}
			finally
			{
				dbTrack.Close();
			}
			
			
			//PositionalLogEntry[] logEntries = new PositionalLogEntry[10]; //TODO: need to know how many positions...
			
			return logEntries;
		}
		
	private void LogError(string ExceptionMessage,Exception ex)
	{
			System.IO.StreamWriter errorLog = File.AppendText(m_strErrorLogFile);
			
			errorLog.WriteLine(DateTime.Now.ToString("G") + " - Message: " + ExceptionMessage );
			errorLog.WriteLine( "Exception Message: " + ex.Message );
			errorLog.WriteLine( "Stack Trace: " + ex.StackTrace );
			errorLog.WriteLine();
			
			errorLog.Close();		
	}
	}

}
