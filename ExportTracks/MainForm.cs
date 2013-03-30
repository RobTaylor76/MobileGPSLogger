/*
 * Created by SharpDevelop.
 * User: u771666
 * Date: 15/07/2010
 * Time: 15:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Gps;
using System.Configuration;
using xnatest;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using System.Xml.Serialization;

namespace ExportTracks
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
      	private string m_strTrackingDBFileName = null;
      	private string m_strGPSExportFile = null;
        private string m_strErrorLogFile = "gpserrors.txt";
        
        private double m_dblPlacemarkSeparation = 0;
        
        private List<PlacemarkType> m_lPlaceMarks;
        
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		
		public MainForm()
		{
			
     		XmlParser xp = new XmlParser();
		
			xp.OnElementStart += new OnElementStartD(xp_OnElementStart);
			
			xp.Load("gpstracker.xml");
			
			xp.Parse();			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
						
			PopulateTrackIDCombo();
			PopulatePlacemarkDistancesCombo();
			
		}
		
		void xp_OnElementStart(string name, string ns, int numAttribs, Array attribs)
		{
		   // throw new Exception("The method or operation is not implemented.");
		    
		   	if (numAttribs == 0) return;
		    	
		   	attr valueAttribute =  (attr)attribs.GetValue(0);
		   	
			if (name == "TrackingDatabase")
		    {
		    	m_strTrackingDBFileName = valueAttribute.attrVal;
		    }
		    else if (name == "DatabaseExportFile")
		    {
		    	m_strGPSExportFile = valueAttribute.attrVal ;
		    }
		    else if (name == "ErrorLogFile")
		    {
		    	m_strErrorLogFile = valueAttribute.attrVal ;
		    }		    

		}
		void ExportClick(object sender, System.EventArgs e)
		{
			ExportLatestTrack();
			
		}	
		
		private String CreateKMLMessage (String strTrackName )
		{
			
			KmlType kml = new KmlType();
			
			DocumentType kmldoc = new DocumentType();
			
			kml.Item= kmldoc;
			
			
			int iStyleCount = 1;
			
			//count number of Placemarks..
		
			int itemCount = m_lPlaceMarks.Count;
			
			kmldoc.Items1 = new FeatureType[m_lPlaceMarks.Count];
			
			
			StyleType pointStyle = new StyleType();
			
			pointStyle.id = "PointStyle";
			
			pointStyle.IconStyle = new IconStyleType();
			
			pointStyle.IconStyle.Icon = new IconStyleIconType();
			
			pointStyle.IconStyle.Icon.href = "root://icons/palette-4.png";
					
			kmldoc.Items = new StyleSelectorType[iStyleCount];
			
			kmldoc.Items[0] = pointStyle;
			
	
			
			
			int iArrayPlaceHolder =0;
			
			foreach (PlacemarkType item in m_lPlaceMarks)
		    {
				kmldoc.Items1[iArrayPlaceHolder++] = item;
			}
			
			
		
			String strSerialise = SerializeToString (kml) ;
					
			//Console.WriteLine(strSerialise);
			
			//Console.Write("Press any key to continue . . . ");
			//Console.ReadKey(true);
				
			return strSerialise;
		}
		
		private string ConvertCoordTostring( Coordinate  p_Coord)
		{
			return p_Coord.GetLongitude (CoordinateFormat.DecimalDegreesNumeric) +","+
								p_Coord.GetLatitude (CoordinateFormat.DecimalDegreesNumeric);
		}
		
		private void AddPlaceMark (string p_strPlaceMarkName, string p_strPlaceMarkDesc, string p_strCoordinates , bool p_blnLine)
		{
			
				/*
  <Placemark>
    <name>Location 3</name>
    <description>This is location 3</description>
    <Point>
      <coordinates>-122.063,37.4063228</coordinates>
    </Point>
  </Placemark>

  <Placemark>
    <name>Yahoo! Inc.</name>
    <description><![CDATA[
      Yahoo! Inc.<br />
      701 First Avenue<br />
      Sunnyvale, CA 94089<br />
      Tel: (408) 349-3300<br />
      Fax: (408) 349-3301<br />
      <p>Home page: <a href="http://yahoo.com">http://yahoo.com</a></p>
      ]]>
    </description>
    <Point>
      <coordinates>-122.0250403,37.4163228</coordinates>
    </Point>
  </Placemark>

  <Placemark>
      <name />
      <description>[15-Aug-10 16:26:39] [264.26 km] [36.5 mph] [END]</description>
      <styleUrl>#PointStyle</styleUrl>
      <Point>
        <coordinates>-2.15337666666667,55.5678933333333</coordinates>
      </Point>
    </Placemark>
    
    <Placemark>
      <name>GPSTEST01 15-Aug-10 09:48:20</name>
      <description />
      <LineString>
        <coordinates>-1.63345833333333,55.0660283333333 -1.63354,55.06615 -1.633455,55.0660683333333 -1.633465,55.0661516666667 -1.63344833333333,55.0662333333333 -1.63348333333333,55.066285 -1.63337,55.0662833333333 -1.633315,55.0663133333333 -1.633295,55.066355 -1.63326833333333,55.06632 </coordinates>
      </LineString>
    </Placemark>				 
				 
				 
				 */			
			PlacemarkType pm = new PlacemarkType();
			
			pm.name = p_strPlaceMarkName;
			pm.description= p_strPlaceMarkDesc;
			
			
			if (p_blnLine)
			{
				LineStringType line =  new LineStringType();

				pm.Item1 = line;
				
				line.coordinates = p_strCoordinates;

//				pm.styleUrl ="#LineStyle";
				
			}
			else 
			{
				PointType point = new PointType();
				
				pm.Item1 = point;
				
				point.coordinates = p_strCoordinates;
				
				pm.styleUrl ="#PointStyle";
			}
			
			
			m_lPlaceMarks.Add(pm);
		}
		
		private void AddPositionalPlaceMark(ref GPSTrackInfo p_TrackInfo , double dbldistance , string strDesc )
		{
			
			String strPlaceMarkName =  "" ;
			String strPlaceMarkDesc = "[" + p_TrackInfo.When.ToString("dd-MMM-yy ") +
				p_TrackInfo.When.ToString("T") + "] [" +  String.Format("{0:0.##}", dbldistance) + " km] [" +  
				String.Format("{0:0.#}",p_TrackInfo.Speed(UnitOfMeasure.Mile)) + " mph] [" + strDesc +"]";
				
			AddPlaceMark( strPlaceMarkName , strPlaceMarkDesc , ConvertCoordTostring(p_TrackInfo.Where) ,false );
		}
		
		public static string SerializeToString(object obj)
  		{
			   XmlSerializer serializer = new XmlSerializer(obj.GetType());
			 
			   using (StringWriter writer = new StringWriter())
			   {
			    	serializer.Serialize(writer, obj);
			 
			    	return writer.ToString();
			   }
		  }
		
		  public static T SerializeFromString<T>(string xml)
		  {
		   XmlSerializer serializer = new XmlSerializer(typeof(T));
		  
		   using (StringReader reader = new StringReader(xml))
		   {
		    return (T)serializer.Deserialize(reader);
		   }
		  }
		void ExportSelectedTrackClick(object sender, System.EventArgs e)
		{
 			int selectedIndex = cbTrackIds.SelectedIndex;
 			
 			if ( selectedIndex != -1)
 			{
	    		Object selectedItem = cbTrackIds.SelectedItem;
	
	    		String selectedTrack = (String)selectedItem; //this.cbTrackIds.SelectedValue(this.cbTrackIds.SelectedIndex);
				
	    		ExportSelectedTrack( selectedTrack);
 			}
		}
		
		
		void ExportSelectedTrack(String strTrackId)
		{
			if (File.Exists(m_strGPSExportFile))
			{
				File.Delete(m_strGPSExportFile);
				
			}				
			
			 m_lPlaceMarks = new List<PlacemarkType>();
			
			String strKMLMessage = "";
			
			StreamWriter swOutputFile =  File.AppendText(m_strGPSExportFile);
			
			IObjectContainer dbTrack =  Db4oFactory.OpenFile(m_strTrackingDBFileName);
			try{
				 
				//Coordinate referencePoint = new Coordinate(0,0);
				Coordinate lastPoint = new Coordinate(0,0);
				
				IQuery query = dbTrack.Query();
				
				//only get Objects for the latest track IDs points...
				
				query = dbTrack.Query();
				query.Constrain(typeof(GPSTrackInfo));
				query.Descend("m_strTrackID").Constrain(strTrackId);
				query.Descend("m_dtTimeAtPosition").OrderAscending();
				//query.Descend("m_dtTimeAtPosition").OrderDescending();				
				
			    IObjectSet result = query.Execute();								 
		 	
			    GPSTrackInfo trackInfo = new GPSTrackInfo(); // = null;
				// should only be at most one...
				
				String strCoordinates = "";

				double dblDistanceBetweenPoints = 0;
				double dblCulmativeDistance = 0;
		
				double dblCulmativeDistanceFromLastPoint = 0;
				
				bool blnFirstPoint = true;
				
				foreach (object item in result)
		    	{
					trackInfo = (GPSTrackInfo)item;
					
					if (m_dblPlacemarkSeparation > 0)
					{
						if (blnFirstPoint)
						{
							//referencePoint = trackInfo.Where;
							blnFirstPoint = false;
							// add Placemark for start and end of track...
							AddPositionalPlaceMark(ref trackInfo,dblCulmativeDistance,"START");
						}
						else
						{
							//dblDistanceBetweenPoints = Coordinate.DistanceBetweenPoints(trackInfo.Where,referencePoint,UnitOfMeasure.Kilometer);
							
								
							dblDistanceBetweenPoints= Coordinate.DistanceBetweenPoints(trackInfo.Where,lastPoint,UnitOfMeasure.Kilometer);
							dblCulmativeDistance += dblDistanceBetweenPoints;
							dblCulmativeDistanceFromLastPoint += dblDistanceBetweenPoints;
							
							//at placemark every 100m
							if(dblCulmativeDistanceFromLastPoint >= m_dblPlacemarkSeparation ) 
							{
								// at Placemark for start and end of track...	
								//referencePoint = trackInfo.Where;
								
								dblCulmativeDistanceFromLastPoint = 0;
								AddPositionalPlaceMark(ref trackInfo,dblCulmativeDistance,"");
	
							}
								
						}
					}
					lastPoint = trackInfo.Where;
					strCoordinates += ConvertCoordTostring( trackInfo.Where) + " ";
		    	}
	
				//add placemark for endPoint
				AddPositionalPlaceMark(ref trackInfo,dblCulmativeDistance,"END");
				
				//add the path...
				AddPlaceMark( trackInfo.TrackId , "" , strCoordinates , true);
				
				strKMLMessage = CreateKMLMessage(strTrackId);

				 m_lPlaceMarks = null;

				
				
			}
			catch (Exception ex)
			{
							LogException( ex , "ExportSelectedTrack");
			}
			finally
			{
				dbTrack.Close();
			}
		
			
			swOutputFile.Write(strKMLMessage );
			
			try
			{
				swOutputFile.Flush();
				swOutputFile.Close();	
			}
			catch (Exception ex)
			{
							LogException( ex , "ExportSelectedTrack");

			}
			finally
			{
				swOutputFile.Close();
			}		
		
		
		}
		
		void ExportLatestTrack()
		{
			String strTrackId= "";
			
			IObjectContainer dbTrack =  Db4oFactory.OpenFile(m_strTrackingDBFileName);
			try{
				 
	
				GPSTrackID trackID = new GPSTrackID();
				trackID.CreateTrackID(); // need dummy track id
				
				IQuery query = dbTrack.Query();
				
				query.Constrain(typeof(GPSTrackID));
				
				query.Descend("m_dtCreationDate").OrderAscending();
				
			    IObjectSet result = query.Execute();			 	
			
				// should  be one for each track...
				foreach (object item in result)
		    	{
					trackID = (GPSTrackID)item;					
		    	}
				strTrackId = trackID.GetTrackID();
				
			}
			catch (Exception ex)
			{
				LogException( ex , "Error: ExportClick ");
			}
			finally
			{
				dbTrack.Close();
			}				
				
			
			ExportSelectedTrack(strTrackId);
		
		}		
		
		void PopulateTrackIDCombo()
		{
					
			IObjectContainer dbTrack =  Db4oFactory.OpenFile(m_strTrackingDBFileName);
			try{
				 
	
				GPSTrackID trackID = new GPSTrackID();
				trackID.CreateTrackID(); // need dummy track id
				
				IQuery query = dbTrack.Query();
				
				query.Constrain(typeof(GPSTrackID));
				
				query.Descend("m_dtCreationDate").OrderAscending();
				
			    IObjectSet result = query.Execute();			 	
			
				// should  be one for each track...
				foreach (object item in result)
		    	{
					trackID = (GPSTrackID)item;	
					
					this.cbTrackIds.Items.Add(trackID.GetTrackID());
		    	}
				
			}
			catch (Exception ex)
			{
				LogException( ex , "PopulateTrackIDCombo");
			}
			finally
			{
				dbTrack.Close();
			}	
		
			
	}

		void LogException(Exception ex , String strMessage)
		{
			
			System.IO.StreamWriter errorLog = File.AppendText(m_strErrorLogFile);
			
			errorLog.WriteLine(DateTime.Now.ToString("G") + " - Message: " + strMessage );
			errorLog.WriteLine( "Exception Message: " + ex.Message );
			errorLog.WriteLine( "Stack Trace: " + ex.StackTrace );
			errorLog.WriteLine();
		
			errorLog.Close();
			
			MessageBox.Show(ex.Message, strMessage);
			
		}
			
		void PopulatePlacemarkDistancesCombo()
		{
				
			this.cbPlacemarkDistances.Items.Add("0");
			this.cbPlacemarkDistances.Items.Add("10");
			this.cbPlacemarkDistances.Items.Add("100");			
			this.cbPlacemarkDistances.Items.Add("1000");			
			this.cbPlacemarkDistances.Items.Add("10000");	
			
			this.cbPlacemarkDistances.SelectedIndex = 2;
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = cbPlacemarkDistances.SelectedIndex;
 			
 			if ( selectedIndex != -1)
 			{
	    		Object selectedItem = cbPlacemarkDistances.SelectedItem;
	
	    	    String strSelectedDistance = (String)selectedItem; //this.cbTrackIds.SelectedValue(this.cbTrackIds.SelectedIndex);
				
	    	    m_dblPlacemarkSeparation = Convert.ToDouble(strSelectedDistance) / 1000;
 			}

		}
		
	}
}
