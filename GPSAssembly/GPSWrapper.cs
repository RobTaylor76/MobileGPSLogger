/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 12/10/2006
 * Time: 12:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Gps
{
	
//	public delegate void GPSCoordinateChangeHandler(object source , EventArgs args);
	

	public delegate void ExceptionHandler(object source , ExceptionEventArgs args);
	
	public class ExceptionEventArgs : EventArgs
	{
		private string m_strExceptionMessage;
		private Exception m_eException;
		public ExceptionEventArgs(string p_strExceptionMessage , Exception p_eException)
		{
			m_strExceptionMessage = p_strExceptionMessage;
			m_eException = p_eException;
		}
		
		public string ExceptionMessage
		{
			get
			{
				return m_strExceptionMessage;
			}
			
		}
		public Exception Exception
		{
			get
			{
				return m_eException;
			}			
		}
		
	}
	/// <summary>
	/// Description of GPSWrapper.
	/// </summary>
	public class GPSWrapper
	{
		
		//	GPSException is used to inform clients of exceptions via Event Notifications		
		public event ExceptionHandler GPSException;
		public event GPSFixHandler GPSCoordUpdateEvent;

		//create an Serial Port object
        private System.IO.Ports.SerialPort m_spGPS= null;
        
        // allow socket to be controlled...
        private bool	m_blnSocketOpen = false;
        private bool    m_blnLoggingNMEAMessages = false;
        
        private System.IO.StreamWriter m_swLogFile = null;
        
        private GPSDataParser m_gpsDataParser = new GPSDataParser();
		
		private string m_strNMEALogFileLocation = "gps_nema.log";
		private string m_strCommPort = "COM8";
		private int m_iCommPortBaudeRate=4800;
		private int m_iSerialPortTimeOut = 1000;
		//public event GPSCoordinateChangeHandler GPSCoordinateChange;
		
		private bool m_blnUseReaderThread = true;
		private Thread m_tPollingThread = null;
		private Mutex  m_mPollingMutex = null;
	
		
		char[]  chMessageBuffer; // used by polling thread
		
		
		public bool UseReaderThread {
			get { return m_blnUseReaderThread; }
			set { m_blnUseReaderThread = value; }
		}
		
		public GPSWrapper()
		{
			
			//catch all teh events raised by the data parser...
			m_gpsDataParser.GPSCoordUpdateEvent += new GPSFixHandler(GPSCoordUpdateEventHandler);
		}
		
		~GPSWrapper()
		{
			m_gpsDataParser.GPSCoordUpdateEvent -= new GPSFixHandler(GPSCoordUpdateEventHandler);

			
		}
		
		private  void GPSCoordUpdateEventHandler(object source , GPSCoordUpdateEventArgs args)
		{
			//propogate the event onwards...
			if (GPSCoordUpdateEvent != null)
			{
				GPSCoordUpdateEvent(this,args);
			}			
			
		}
		
		public string CommPort
		{
			get
			{
				return m_strCommPort;
			}
			set
			{
				if (!m_strCommPort.Equals(value))
				{
					// if this is for a different comm port thenn close the existing one..
					StopGPSReceiver(); 
					m_strCommPort = value; 
					
				}

			}
		}

		public int SerialPortTimeOut
		{
			get
			{
				return m_iSerialPortTimeOut;
			}
			set
			{
				if (m_iSerialPortTimeOut!=value)
				{
					// if this is for a different comm port thenn close the existing one..
					StopGPSReceiver(); 
					m_iSerialPortTimeOut = value; 
					
				}

			}
		}
		
		public int CommPortBaudRate
		{
			get
			{
				return m_iCommPortBaudeRate;
			}
			set
			{
				if (m_iCommPortBaudeRate!=value)
				{
					// if this is for a different comm port thenn close the existing one..
					StopGPSReceiver(); 
					m_iCommPortBaudeRate = value; 
					
				}

			}
		}
		public GPSDataParser GPSDataParser
		{
			get
			{
				return m_gpsDataParser;
			}
		}
		
		public string NMEALogFileLocation
		{
			get
			{
				return m_strNMEALogFileLocation;
			}
			set
			{
				if (!m_strNMEALogFileLocation.Equals(value))
				{
					m_strNMEALogFileLocation = value;
					if (LoggingNMEAMessages())
					{
						StopLoggingNMEA();
						StartLoggingNMEA();			
					}
		
				}
			}
		}
		public bool StartGPSReceiver()
		{			
			bool blnStartedOk = false;
			
			if (GPSConnected())
			{
				//close the existing comm port...
				StopGPSReceiver();
			}
          	try
            {

          		bool blnUseEvent = !UseReaderThread;
          		OpenSerialPort(blnUseEvent);         		
         		
          		if (UseReaderThread)
          		{
          			m_tPollingThread = new Thread(this.PollingThread);
          			
          			m_mPollingMutex = new Mutex(true);  // initally owned... will release to end thread...
          			
          			m_tPollingThread.Start();
          			
         				
          		}
//          		else
//          		{
//          			OpenSerialPort(true);
//          			          			
//					blnStartedOk = true;
//
//          		}

	                
	           //     logFile  = File.CreateText("gps.nema.txt");	
				blnStartedOk = true;
           	
                
            }
            catch (System.Exception ex)
            {
               // this.result.Text = ex.Message;
               m_spGPS = null;
               m_blnSocketOpen = false;
               
               RaiseExceptionEvent("Error Occured In StartGPSReceiver()",ex);

            }

            return blnStartedOk;
		}
		
		// StopGPSReceiver - this will close any active Serial Port sessions...
		public void StopGPSReceiver()
		{
			if (GPSConnected())
			{
				try
				{
					bool blnUseEvent = !UseReaderThread;
					
					if (UseReaderThread)
					{	
						m_mPollingMutex.ReleaseMutex();
						
						m_tPollingThread.Join();
					}
//					else
//					{
//						CloseSerialPort(true);
//					}
					CloseSerialPort(blnUseEvent);
					
				}
				catch (Exception ex)
				{
					RaiseExceptionEvent("Error Occured In StopGPSReceiver()",ex);
				}
	          //		m_spGPS.Close();
			}
		}

		private void OpenSerialPort(bool useEventHandler)
		{
	 					
			m_spGPS = new System.IO.Ports.SerialPort();
			m_spGPS.PortName = m_strCommPort;
			//open serial port
			//	          		m_spGPS.BaudRate = 38400;
			m_spGPS.BaudRate = m_iCommPortBaudeRate;
			m_spGPS.DataBits =8;
			m_spGPS.StopBits = StopBits.One;
			m_spGPS.Parity = Parity.None;
			
			//set read time out to 500 ms
			m_spGPS.DtrEnable = true;
			m_spGPS.ReadTimeout = m_iSerialPortTimeOut;
			
			if (useEventHandler)
			{			
				m_spGPS.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
			}
			m_spGPS.Open();
			
			m_blnSocketOpen = true;

		}
		private void CloseSerialPort(bool useEventHandler)
		{
			
			if (useEventHandler)
			{
				m_spGPS.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
			}
			m_spGPS.Close();
			
		    m_blnSocketOpen = false;

			m_spGPS.Dispose();
			
			m_spGPS = null;
			
		}
		
		private void PollingThread()
		{
			
			chMessageBuffer = new char[1024];
			
//			try
//			{
//				OpenSerialPort(false);

	            try
	            {
					// open an close the serial port in here...
					while( !m_mPollingMutex.WaitOne(0,false))  //while 1000ms and continue if mutex is not signalled... 
					{
						try
						{
							// Read the buffer to text box.
							
							//string newLine = m_spGPS.ReadLine();
							
							string newLine = ReadLineFromGPS();
							
							//string newLine = m_spGPS.ReadExisting();
							m_gpsDataParser.ParseNMEASentence(newLine);
							
							
							if (LoggingNMEAMessages())
							{
								m_swLogFile.WriteLine(newLine);
								m_swLogFile.Flush();				
							}
							
							Thread.Sleep(0);
						}
						catch (OutOfMemoryException )
						{
							//ignore...  http://social.msdn.microsoft.com/Forums/en-US/vssmartdevicesvbcs/thread/75451515-ba72-4a96-95e7-3ec801b70260
						}
						catch (Exception ex)
						{
							RaiseExceptionEvent("Error Occured In PollingThread while(!m_mPollingMutex.WaitOne(0,false))",ex);
						}				
						
					}
	            }
	            catch (System.Exception ex)
	            {
	    			RaiseExceptionEvent("Error Occured In PollingThread while(true)",ex);
	        	
	            }
//	            finally
//	            {
//	            	CloseSerialPort(false);
//	            }
			
//			}
//            catch (System.Exception ex)
//            {
//               // this.result.Text = ex.Message;
//               m_spGPS = null;
//               m_blnSocketOpen = false;
//               
//               RaiseExceptionEvent("Error Occured In PollingThread.OpenSerialPort()",ex);
//
//            }
            
		}
		
		private string ReadLineFromGPS()
		{
			string strNMEAString = "";
			//char[]  chMessageBuffer= new char[1024];
			//Array.Clear(chMessageBuffer, 0, chMessageBuffer.Length);

			int iCurrentChar;
			for (int iNumberOfChars=0; iNumberOfChars < chMessageBuffer.Length ;)
			{
				iCurrentChar = m_spGPS.ReadChar();		
				
				chMessageBuffer[iNumberOfChars] = Convert.ToChar(iCurrentChar);

				
				if (iCurrentChar == 10 //LF 
				    || iCurrentChar == 11 //VT 
				    || iCurrentChar == 12 //FF 
				    || iCurrentChar == 13) //CR
				{
					//end of line indicator...
					if (iNumberOfChars == 0) //have blank line
					{
						continue; // ignore the blank line...
					}
					strNMEAString = new string(chMessageBuffer,0,iNumberOfChars); // ignores the trailing char
					break;
				}
				
				iNumberOfChars++;
			}
			
			return strNMEAString;
			
		}
		
		public bool GPSConnected()
		{
			return m_blnSocketOpen;

		}
		
		public bool LoggingNMEAMessages()
		{
			return m_blnLoggingNMEAMessages;
		}
			
			
		// StartLogging - start logging NMEA messages
		public void StartLoggingNMEA()
		{
			if (m_swLogFile != null)
			{
				StopLoggingNMEA();
			}	
			try
			{
				if(m_strNMEALogFileLocation != null)
				{
					m_swLogFile  = File.CreateText(m_strNMEALogFileLocation);
					m_blnLoggingNMEAMessages = true;
				}
				else
				{
					Exception ex = new Exception("Cannot Log NMEA Message -> NMEALogFileLocation is null");
					RaiseExceptionEvent("Error Occured In StartLoggingNMEA()",ex);
				}
			}
			catch (Exception ex)
			{
			    RaiseExceptionEvent("Error Occured In StartLoggingNMEA()",ex);
			}
		}
		
		// StopLoging - - start logging NMEA messages
		public void StopLoggingNMEA()
		{
			if (LoggingNMEAMessages())
			{
	          	m_swLogFile.Close();
	          	m_blnLoggingNMEAMessages = false;
	          	m_swLogFile = null;
	          //		m_spGPS.Close();
			}
		}				
		
		private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{// Event for receiving data
			
			// should we loop to get all data or just work line to line..
			// we could catch the read timeout exception and carry on as normal...
			try
			{
				if (m_blnSocketOpen)
				{
					// Read the buffer to text box.
					string newLine = m_spGPS.ReadLine();
					//string newLine = m_spGPS.ReadExisting();
					m_gpsDataParser.ParseNMEASentence(newLine);
					
					if (LoggingNMEAMessages())
					{
						m_swLogFile.WriteLine(newLine);
						m_swLogFile.Flush();				
					}
				}
				//GPSCoordinateChange(this,new EventArgs());
				// need to raise a event to signify that data has been received..
			}
			catch (Exception ex)
			{
				RaiseExceptionEvent("Error Occured In port_DataReceived(...)",ex);
			}
		}
		
		public string ReadNMEAStringFromGPS()
		{
			String strNMEAString = "";
			
			try
			{
				if (m_blnSocketOpen)
				{
					// Read the buffer to text box.
					 strNMEAString = m_spGPS.ReadLine();
				}
				//GPSCoordinateChange(this,new EventArgs());
				// need to raise a event to signify that data has been received..
			}
			catch (Exception ex)
			{
				RaiseExceptionEvent("Error Occured In port_DataReceived(...)",ex);
			}	
			
			return strNMEAString;
		}
		
		/*
		 * RaiseExceptionEvent - this will be single point of call to inform clients
		 * 						 of exceptions...
		 */
		private void RaiseExceptionEvent(string p_strMessage, Exception p_eException)
		{
			if (GPSException != null)
			{
	
				GPSException(this,new ExceptionEventArgs(p_strMessage,p_eException));

			}
		}
		
		public GPSData GetCurrentGPSData()
		{
			return m_gpsDataParser.GetCurrentGPSData();
		}
	}
}
