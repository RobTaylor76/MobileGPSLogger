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
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

using Win32;
using Microsoft.Win32;


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
        
        private GPSTrackFileWriter m_GPSTrackWriter = null;
        private GPSTrackDBWriter m_GPSTrackDBWriter = null;
        
          
        private string m_strNEMALogFile = null;
        private string m_strGPSTrackFile = null;
        private string m_strErrorLogFile = "gpserrors.txt";
  		private string m_strGPSCOMPort = "COM8" ;        
      	private string m_strTrackingDBFileName = null;
      	private string m_strGPSExportFile = null;
  		
        private long m_lGPSTrackTimeInterval = 0 ;
		private int m_iGPSCOMPortBaudRate = 4800 ;
		private int m_iSerialPortTimeOut = 500;
		
		private bool m_blnSuppressErrors = true;
		private int m_iErrorCount=0;
		
		private bool m_bUseGPSUpdateEvents =false;
		private bool m_bUseFormTimer = false;
		private int  m_iFormTimerInterval = 5000; // 5secs 

		private bool m_bUseLogTimer = false;
		private int  m_iLogTimerInterval = 5000; // 5secs 

		private Timer m_UpdateTimer;
		private Timer m_LogTimer;
		
		private Timer m_PowerResetTimer;

        
		//[STAThread]
		public static void Main(string[] args)
		{
	//		Application.EnableVisualStyles();
	//		Application.SetCompatibleTextRenderingDefault(false);
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
								//public event GPSFixHandler GPSCoordUpdateEvent;

			if (m_bUseGPSUpdateEvents)
			{
				m_GPSWrapper.GPSCoordUpdateEvent += new GPSFixHandler(GPSCoordsUpdated);
			}
			
			m_GPSWrapper.GPSException +=  new ExceptionHandler(ExceptionHandler);
			
			
			if (m_bUseFormTimer)
			{
				m_UpdateTimer = new Timer();
				m_UpdateTimer.Interval = m_iFormTimerInterval;
				m_UpdateTimer.Enabled = true;
				m_UpdateTimer.Tick += new System.EventHandler (OnFormTimerEvent);
	
			}
			if (m_bUseLogTimer)
			{
				m_LogTimer = new Timer();
				m_LogTimer.Interval = m_iLogTimerInterval;
				m_LogTimer.Enabled = true;
				m_LogTimer.Tick += new System.EventHandler (OnLogTimerEvent);
	
			}
			
			            // Set the interval on our timer and start the
            // timer. It will run for the duration of the
            // program
            m_PowerResetTimer = new Timer();
            int interval = ShortestTimeoutInterval();
            m_PowerResetTimer.Interval = interval;
            m_PowerResetTimer.Enabled = true;
            m_LogTimer.Tick += new System.EventHandler (PowerResetTimer_Tick);
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this.commport.Text = m_strGPSCOMPort;
			
			this.txtErrorCount.Text = m_iErrorCount.ToString();
			
			this.chkSupressErrors.Checked = m_blnSuppressErrors; // take value of control...
			
			
			
		}
		
		void xp_OnElementStart(string name, string ns, int numAttribs, Array attribs)
		{
		   // throw new Exception("The method or operation is not implemented.");
		    
		   	if (numAttribs == 0) return;
		    	
		   	attr valueAttribute =  (attr)attribs.GetValue(0);
		   	
		    if (name == "TrackingFileName")
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
		    else if (name == "TrackingTimeInterval")
		    {
		    	m_lGPSTrackTimeInterval = long.Parse(valueAttribute.attrVal) ;
		    }	
		    else if (name == "COMPort")
		    {
		    	m_strGPSCOMPort = valueAttribute.attrVal ;
		    }	
		    else if (name == "COMPortBaudRate")
		    {
		    	
		    	m_iGPSCOMPortBaudRate = int.Parse(valueAttribute.attrVal);
		    }
		    else if (name == "COMPortTimeOut")
		    {
		    	m_iSerialPortTimeOut = int.Parse(valueAttribute.attrVal);
		    }
		    else if (name == "TrackingDatabase")
		    {
		    	m_strTrackingDBFileName = valueAttribute.attrVal;
		    }
		    else if (name == "DatabaseExportFile")
		    {
		    	m_strGPSExportFile = valueAttribute.attrVal ;
		    }
		    else if (name == "UseGPSUpdateEvents")
		    {
		    	m_bUseGPSUpdateEvents  = (valueAttribute.attrVal == "True");
		    	
		    	if (m_bUseGPSUpdateEvents) 
		    	{
		    		m_bUseFormTimer = false;
		    	}
		    }
		    else if (name == "UseFormTimer")
		    {
		    	m_bUseFormTimer  = (valueAttribute.attrVal == "True");
		    	
		    	if (m_bUseFormTimer) 
		    	{
		    		m_bUseGPSUpdateEvents = false;
		    	}
		    }	
		    else if (name == "FormTimerInterval")
		    {
		    	m_iFormTimerInterval  = Convert.ToInt32 (valueAttribute.attrVal);
		    	
		    }
		    	
		    else if (name == "UseLogTimer")
		    {
		    	m_bUseLogTimer  = (valueAttribute.attrVal == "True");
		    	
		    }		
		    else if (name == "LogTimerInterval")
		    {
		    	m_iLogTimerInterval  = Convert.ToInt32 (valueAttribute.attrVal);
		    	
		    }		    
		    
		    
		    
		}			
		
		void StartStockTrackBtnClick(object sender, System.EventArgs e)
		{
          	
			if (m_GPSWrapper.GPSConnected())
          	{
          		m_GPSWrapper.StopGPSReceiver();
          		this.StartStockTrackBtn.Text = "Start";

          	}
          	else
          	{
          		m_GPSWrapper.CommPort = this.commport.Text;
          		m_GPSWrapper.CommPortBaudRate = m_iGPSCOMPortBaudRate;
           		m_GPSWrapper.StartGPSReceiver();
         			
           		if (m_GPSWrapper.GPSConnected())
          		{
           			this.StartStockTrackBtn.Text = "Stop";
           		}
          	}
  			
		}
		
		private void ExceptionHandler (object sender , ExceptionEventArgs args)
		{
			
			if (!m_blnSuppressErrors)
			{
				MessageBox.Show(args.Exception.Message, args.ExceptionMessage);
			}
			
			m_iErrorCount++;
			
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
			
			////RefreshScreen();  
			//// have to do the invoke rather update directly due to threading issues!
			////this.Invoke(new EventHandler(DoUpdate));
			// thi.Ivoke was causing the program to hang!!!!
			
		}
		private void GPSCoordsUpdated(object sender, GPSCoordUpdateEventArgs p_event)

		{// Event for receiving data
			
			// Read the buffer to text box.
//			m_coLastGPSFix = p_event.CurrentPosition;
			
			this.Invoke(new EventHandler(DoUpdate));
		
		}

		public  void OnFormTimerEvent(object source, EventArgs e)
		{
			// refresh the screen...
			this.Invoke(new EventHandler(DoUpdate));
		}		
			
		public  void OnLogTimerEvent(object source, EventArgs e)
		{		
			if ((m_GPSTrackDBWriter != null) || (m_GPSTrackWriter != null))
			{
				GPSData currData =  m_GPSWrapper.GetCurrentGPSData();
				if (m_GPSTrackDBWriter != null)
				{
					m_GPSTrackDBWriter.LogPosition( ref currData);
				}
				if (m_GPSTrackWriter != null)
				{
					m_GPSTrackWriter.LogPosition( ref currData);
				}			
			}
		}		
		
		private void DoUpdate(object s, EventArgs e)
		
		{
 			RefreshScreen();
			

		}
		void chkWriteNMEALogCheckedChanged(object sender, System.EventArgs e)
		{
//			MessageBox.Show("Write Log Clicked", "Write Log Clicked");

			if (m_strNEMALogFile == null )
			{
				if ( this.chkWriteNMEALog.Checked)
				{
					MessageBox.Show("No Log file specified", "Cannot Log NMEA Message");
				
					this.chkWriteNMEALog.Checked = false;
				}
			}
			else
			{
				if (this.chkWriteNMEALog.Checked)
				{
					m_GPSWrapper.StartLoggingNMEA();
				}
				else
				{
					m_GPSWrapper.StopLoggingNMEA();
				}
			}
		}
	
		
		void Button2Click(object sender, System.EventArgs e)
		{
			 RefreshScreen();
		}
		
		private void RefreshScreen()
		{
			GPSData m_currentData = m_GPSWrapper.GetCurrentGPSData();
			
			this.txtNoOfSatellites.Text =  Convert.ToString(m_currentData.NoOfTrackedSatellites);
			this.txtFixType.Text = m_currentData.FixType;
			this.txtFixTime.Text = m_currentData.TimeOfLastFixGPS;
			
			// latest tatitude and longitude stored in object
			this.txtLongitude.Text = m_currentData.LatestFixPosition.GetLongitude(CoordinateFormat.DegreesMinutesSeconds);
			this.txtLatitude.Text = m_currentData.LatestFixPosition.GetLatitude(CoordinateFormat.DegreesMinutesSeconds);			
			this.txtCurrSpeed.Text = m_currentData.CurrentSpeed(UnitOfMeasure.Mile).ToString();
			
			this.txtErrorCount.Text = m_iErrorCount.ToString();;

		}
		

		void ChkWriteToTrackingDBChanged(object sender, System.EventArgs e)
		{
			if (m_strTrackingDBFileName == null)
			{
				if (this.chkWriteToTrackingDB.Checked)
				{
					MessageBox.Show("No Track DB File file specified", "Cannot Create Track DB");
					this.chkWriteToTrackingDB.Checked = false;
					
				}
				
			}
			else
			{
				if (this.chkWriteToTrackingDB.Checked)
				{
					if (m_GPSTrackDBWriter == null)
					{
						if (m_bUseLogTimer) 
						{
							m_GPSTrackDBWriter = new GPSTrackDBWriter(m_strTrackingDBFileName);
						}
						else
						{
							m_GPSTrackDBWriter = new GPSTrackDBWriter(m_GPSWrapper,m_strTrackingDBFileName);
						}
						m_GPSTrackDBWriter.DBWriterException +=  new ExceptionHandler(ExceptionHandler);
			
					}
					m_GPSTrackDBWriter.LogFrequency = m_lGPSTrackTimeInterval;
					m_GPSTrackDBWriter.StartLogging();
				
				}
				else
				{
					m_GPSTrackDBWriter.StopLogging();
					
				}
				
			}
		}
		                           
		void ChkWriteTrackLogFileChanged(object sender, System.EventArgs e)
		{
			if (m_strGPSTrackFile == null )
			{
				if ( this.chkWriteTrackLogFile.Checked)
				{	
					MessageBox.Show("No Track File file specified", "Cannot Create Track File");
					this.chkWriteTrackLogFile.Checked = false;
				}
			}
			else
			{
				if (this.chkWriteTrackLogFile.Checked)
				{
				
					if (m_bUseLogTimer) 
					{
						m_GPSTrackWriter = new GPSTrackFileWriter(m_strGPSTrackFile);
					}
					else
					{
						m_GPSTrackWriter = new GPSTrackFileWriter(m_GPSWrapper,m_strGPSTrackFile);
						
					}
					
					m_GPSTrackWriter.FileWriterException +=  new ExceptionHandler(ExceptionHandler);

					m_GPSTrackWriter.LogFrequency = m_lGPSTrackTimeInterval;
	
				}
				else
				{
					m_GPSTrackWriter.CloseTrackFile();
					m_GPSTrackWriter = null;
				}
			}
		}	
		
		void ChkSupressErrorsChanged(object sender, System.EventArgs e)
		{
			m_blnSuppressErrors = this.chkSupressErrors.Checked;
		}



	
	
	        // Look in the registry to see what the shortest timeout
        // period is. Note that Zero is a special value with respect
        // to timeouts. It indicates that a timeout will not occur.
        // As long as SystemIdleTimeerReset is called on intervals
        // that are shorter than the smallest non-zero timeout value
        // then the device will not sleep from idleness. This does
        // not prevent the device from sleeping due to the power
        // button being pressed.
        private int ShortestTimeoutInterval()
        {
            int retVal = 1000;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(
                        @"\SYSTEM\CurrentControlSet\Control\Power");
            object oBatteryTimeout = key.GetValue("BattPowerOff");
            object oACTimeOut = key.GetValue("ExtPowerOff");
            object oScreenPowerOff = key.GetValue("ScreenPowerOff");

            if (oBatteryTimeout is int)
            {
                int v =  (int)oBatteryTimeout;
                if(v>0)
                    retVal = Math.Min(retVal,v);
             }
            if (oACTimeOut is int)
            {
                int v = (int)oACTimeOut;
                if(v>0)
                    retVal = Math.Min(retVal, v);
             }
            if (oScreenPowerOff is int)
            {
                int v = (int)oScreenPowerOff;
                if(v>0)
                    retVal = Math.Min(retVal, v);
           }

	//Since the interval is in seconds and out timer
	//operates in milliseconds the value needs to be multiplied
	//by 1000 to get the appropriate millisecond value. I've
	//multiplied by 900 instead so that I ensure that I call
	//SystemIdleTimerReset before the timeout is reached.
            return retVal*900;
        }


        // Call the SystemIdleTimerReset method to prevent the
        // device from sleeping
        private void PowerResetTimer_Tick(object sender, EventArgs e)
        {
        	if (m_GPSWrapper.GPSConnected())  // only reset timer if the GPS is connected
        	{
            	CoreDLL.SystemIdleTimerReset();
        	}
        }

}
	}
