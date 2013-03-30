/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 06/10/2006
 * Time: 20:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using Gps;
using System.Configuration;
using xnatest;

namespace ppc_test01
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm
	{
		
		
		//create an Serial Port object
       // private System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort();
        
        // allow socket to be controlled...
       // private bool	m_blnSocketOpen = false;
        
      //  private System.IO.StreamWriter logFile = null;
        
        private GPSWrapper m_GPSWrapper = null;
        
        private GPSTrackFileWriter m_GPSTrackeWriter = null;
        
        private Coordinate m_coLastGPSFix;// = new Coordinate(0,0);
        
        private string m_strNEMALogFile = "nmea.txt";
        private string m_strGPSTrackFile = "gpstrack.trk";
        private string m_strErrorLogFile = "gpserrors.txt";
        
        private long m_lGPSTrackTimeInterval = 0 ;
		private string m_strGPSCOMPort = "COM8" ;        
        
		[STAThread]
		public static void Main(string[] args)
		{
//			Application.EnableVisualStyles();
//			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		public MainForm()
		{

			
     //   private string m_strNEMALogFile = null;
     //   private string m_strGPSTrackFile = null;			
     
     		XmlParser xp = new XmlParser();
		
			xp.OnElementStart += new OnElementStartD(xp_OnElementStart);
			//xp.OnElementEnd += new OnElementEndD(xp_OnElementEnd);
			//xp.OnElementData += new OnElementDataD(xp_OnElementData);			
			
			xp.Load("gpstracker.xml");
			
			xp.Parse();

			//m_strNEMALogFile = System.Configuration.ConfigurationSettings.AppSettings["NMEALogFile"];
     		//m_strGPSTrackFile = ConfigurationSettings.AppSettings["GPSTrackFileName"];
     		//m_strErrorLogFile  = ConfigurationSettings.AppSettings["ErrorLogFile"];
     		
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			m_GPSWrapper = new GPSWrapper();
			
			m_GPSWrapper.NMEALogFileLocation = 	m_strNEMALogFile;
						
			m_GPSWrapper.GPSData.CoordinateChange += new CoordinateChangeHandler(GPSCoordsUpdated);
			m_GPSWrapper.GPSException +=  new ExceptionHandler(GPSException);
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();			
		}
		
		void xp_OnElementStart(string name, string ns, int numAttribs, Array attribs)
		{
		   // throw new Exception("The method or operation is not implemented.");
		    
		   	if (numAttribs == 0) return;
		    	
		   	attr valueAttribute =  (attr)attribs.GetValue(0);
		   	
		    if (name == "GPSTrackFileName")
		    {
		    	m_strGPSTrackFile = valueAttribute.attrVal ;
		    }
		    else if (name == "NMEALogFile")
		    {
		    	m_strNEMALogFile = valueAttribute.attrVal ;
		    }		    
		    else if (name == "ErrorLogFile")
		    {
		    	m_strErrorLogFile = valueAttribute.attrVal ;
		    }
		    else if (name == "GPSTrackTimeInterval")
		    {
		    	m_lGPSTrackTimeInterval = long.Parse(valueAttribute.attrVal) ;
		    }	
		    else if (name == "GPSCOMPort")
		    {
		    	m_strGPSCOMPort = valueAttribute.attrVal ;
		    }		    
		}			
		
		void Button1Click(object sender, System.EventArgs e)
		{
          	
			if (m_GPSWrapper.GPSConnected())
          	{
          		m_GPSWrapper.StopGPSReceiver();
          		this.button1.Text = "Start";

          	}
          	else
          	{
          		m_GPSWrapper.CommPort = this.commport.Text;
           		m_GPSWrapper.StartGPSReceiver();
         			
           		if (m_GPSWrapper.GPSConnected())
          		{
           			this.button1.Text = "Stop";
           		}
          	}
  			
		}
		
		private void GPSException (object sender , ExceptionEventArgs args)
		{
			MessageBox.Show(args.Exception.Message, args.ExceptionMessage);

		//	string strMessage =  DateTime.Now.ToString("G") + " - Message: " + args.ExceptionMessage + "\n\n";
		//	strMessage += "Exception Message: " + args.Exception.Message + "\n\n";
		//	strMessage += "Stack Trace: " + args.Exception.StackTrace + "\n\n";

			System.IO.StreamWriter errorLog = File.AppendText(m_strErrorLogFile);
			
			errorLog.WriteLine(DateTime.Now.ToString("G") + " - Message: " + args.ExceptionMessage );
			errorLog.WriteLine( "Exception Message: " + args.Exception.Message );
			errorLog.WriteLine( "Stack Trace: " + args.Exception.StackTrace );
			errorLog.WriteLine();
			
			errorLog.Close();
			//File.AppendAllText("gpstracker.err.txt" , strMessage);
			
			
		}
		private void GPSCoordsUpdated(object sender, CoordinateChangeEventArgs p_event)

		{// Event for receiving data
			
			// Read the buffer to text box.
			m_coLastGPSFix = p_event.CurrentPosition;
			
			this.Invoke(new EventHandler(DoUpdate));
		
		}
		
		private void DoUpdate(object s, EventArgs e)
		
		{
 			RefreshScreen();
			
		}
		void ChkWriteLogCheckedChanged(object sender, System.EventArgs e)
		{
//			MessageBox.Show("Write Log Clicked", "Write Log Clicked");

			if (this.chkWriteLog.Checked)
			{
				m_GPSWrapper.StartLoggingNMEA();
			}
			else
			{
				m_GPSWrapper.StopLoggingNMEA();
			}
		}
	
		
		void Button2Click(object sender, System.EventArgs e)
		{
			 RefreshScreen();
		}
		
		private void RefreshScreen()
		{
			this.txtNoOfSatellites.Text = m_GPSWrapper.GPSData.NoOfTrackedSatellites;
			this.txtFixType.Text = m_GPSWrapper.GPSData.FixType;
			this.txtFixTime.Text = m_GPSWrapper.GPSData.GPSTimeOfLastFix;
			
			// latest tatitude and longitude stored in object
			this.txtLongitude.Text = m_coLastGPSFix.GetLongitude(CoordinateFormat.DegreesMinutesSeconds);
			this.txtLatitude.Text = m_coLastGPSFix.GetLatitude(CoordinateFormat.DegreesMinutesSeconds);			
			this.txtCurrSpeed.Text = m_GPSWrapper.GPSData.CurrentSpeed(UnitOfMeasure.Mile).ToString();
		}
		

		void ChkWriteTrackLogChanged(object sender, System.EventArgs e)
		{
			if (this.chkWriteTrackLog.Checked)
			{
			
				m_GPSTrackeWriter = new GPSTrackFileWriter(m_GPSWrapper,m_strGPSTrackFile);
				m_GPSTrackeWriter.LogFrequency = m_lGPSTrackTimeInterval;

			}
			else
			{
				m_GPSTrackeWriter.CloseTrackFile();
				m_GPSTrackeWriter = null;
			}
		}	
	}
}
