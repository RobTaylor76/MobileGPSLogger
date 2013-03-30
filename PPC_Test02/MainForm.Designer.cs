/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 06/10/2006
 * Time: 20:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ppc_test01
{
	partial class MainForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		


		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.StartStockTrackBtn = new System.Windows.Forms.Button();
			this.commport = new System.Windows.Forms.TextBox();
			this.txtLatitude = new System.Windows.Forms.TextBox();
			this.txtLongitude = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtFixTime = new System.Windows.Forms.TextBox();
			this.txtFixType = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtNoOfSatellites = new System.Windows.Forms.TextBox();
			this.chkWriteNMEALog = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.txtCurrSpeed = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.chkWriteTrackLogFile = new System.Windows.Forms.CheckBox();
			this.chkWriteToTrackingDB = new System.Windows.Forms.CheckBox();
			this.chkSupressErrors = new System.Windows.Forms.CheckBox();
			this.txtErrorCount = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// StartStockTrackBtn
			// 
			this.StartStockTrackBtn.Location = new System.Drawing.Point(12, 2);
			this.StartStockTrackBtn.Name = "StartStockTrackBtn";
			this.StartStockTrackBtn.Size = new System.Drawing.Size(97, 23);
			this.StartStockTrackBtn.TabIndex = 16;
			this.StartStockTrackBtn.Text = "Start";
			this.StartStockTrackBtn.Click += new System.EventHandler(this.StartStockTrackBtnClick);
			// 
			// commport
			// 
			this.commport.Location = new System.Drawing.Point(115, 4);
			this.commport.Name = "commport";
			this.commport.Size = new System.Drawing.Size(53, 20);
			this.commport.TabIndex = 15;
			// 
			// txtLatitude
			// 
			this.txtLatitude.Location = new System.Drawing.Point(80, 28);
			this.txtLatitude.Name = "txtLatitude";
			this.txtLatitude.ReadOnly = true;
			this.txtLatitude.Size = new System.Drawing.Size(112, 20);
			this.txtLatitude.TabIndex = 14;
			// 
			// txtLongitude
			// 
			this.txtLongitude.Location = new System.Drawing.Point(80, 51);
			this.txtLongitude.Name = "txtLongitude";
			this.txtLongitude.ReadOnly = true;
			this.txtLongitude.Size = new System.Drawing.Size(112, 20);
			this.txtLongitude.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 12);
			this.label1.Text = "Latitude:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 14);
			this.label2.Text = "Longitude:";
			// 
			// txtFixTime
			// 
			this.txtFixTime.Location = new System.Drawing.Point(89, 75);
			this.txtFixTime.Name = "txtFixTime";
			this.txtFixTime.ReadOnly = true;
			this.txtFixTime.Size = new System.Drawing.Size(103, 20);
			this.txtFixTime.TabIndex = 10;
			// 
			// txtFixType
			// 
			this.txtFixType.Location = new System.Drawing.Point(89, 98);
			this.txtFixType.Name = "txtFixType";
			this.txtFixType.ReadOnly = true;
			this.txtFixType.Size = new System.Drawing.Size(103, 20);
			this.txtFixType.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 78);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 13);
			this.label3.Text = "Fix Time:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(10, 101);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 18);
			this.label4.Text = "Fix Type:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(10, 124);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(73, 17);
			this.label5.Text = "No Of Sats:";
			// 
			// txtNoOfSatellites
			// 
			this.txtNoOfSatellites.Location = new System.Drawing.Point(89, 121);
			this.txtNoOfSatellites.Name = "txtNoOfSatellites";
			this.txtNoOfSatellites.ReadOnly = true;
			this.txtNoOfSatellites.Size = new System.Drawing.Size(103, 20);
			this.txtNoOfSatellites.TabIndex = 5;
			// 
			// chkWriteNMEALog
			// 
			this.chkWriteNMEALog.Location = new System.Drawing.Point(12, 193);
			this.chkWriteNMEALog.Name = "chkWriteNMEALog";
			this.chkWriteNMEALog.Size = new System.Drawing.Size(77, 32);
			this.chkWriteNMEALog.TabIndex = 4;
			this.chkWriteNMEALog.Text = "Write NMEA";
			this.chkWriteNMEALog.CheckStateChanged += new System.EventHandler(this.chkWriteNMEALogCheckedChanged);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(174, 1);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(25, 24);
			this.button2.TabIndex = 3;
			this.button2.Text = "R";
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// txtCurrSpeed
			// 
			this.txtCurrSpeed.Location = new System.Drawing.Point(89, 145);
			this.txtCurrSpeed.Name = "txtCurrSpeed";
			this.txtCurrSpeed.ReadOnly = true;
			this.txtCurrSpeed.Size = new System.Drawing.Size(103, 20);
			this.txtCurrSpeed.TabIndex = 1;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(10, 148);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(73, 13);
			this.label6.Text = "Speed MPH:";
			// 
			// chkWriteTrackLogFile
			// 
			this.chkWriteTrackLogFile.Location = new System.Drawing.Point(95, 193);
			this.chkWriteTrackLogFile.Name = "chkWriteTrackLogFile";
			this.chkWriteTrackLogFile.Size = new System.Drawing.Size(88, 35);
			this.chkWriteTrackLogFile.TabIndex = 0;
			this.chkWriteTrackLogFile.Text = "GPS Track File";
			this.chkWriteTrackLogFile.CheckStateChanged += new System.EventHandler(this.ChkWriteTrackLogFileChanged);
			// 
			// chkWriteToTrackingDB
			// 
			this.chkWriteToTrackingDB.Location = new System.Drawing.Point(95, 229);
			this.chkWriteToTrackingDB.Name = "chkWriteToTrackingDB";
			this.chkWriteToTrackingDB.Size = new System.Drawing.Size(97, 21);
			this.chkWriteToTrackingDB.TabIndex = 17;
			this.chkWriteToTrackingDB.Text = "GPS Track BD";
			this.chkWriteToTrackingDB.CheckStateChanged += new System.EventHandler(this.ChkWriteToTrackingDBChanged);
			// 
			// chkSupressErrors
			// 
			this.chkSupressErrors.Checked = true;
			this.chkSupressErrors.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSupressErrors.Location = new System.Drawing.Point(12, 223);
			this.chkSupressErrors.Name = "chkSupressErrors";
			this.chkSupressErrors.Size = new System.Drawing.Size(77, 32);
			this.chkSupressErrors.TabIndex = 24;
			this.chkSupressErrors.Text = "Supress Errors";
			this.chkSupressErrors.CheckStateChanged += new System.EventHandler(this.ChkSupressErrorsChanged);
			// 
			// txtErrorCount
			// 
			this.txtErrorCount.Location = new System.Drawing.Point(89, 167);
			this.txtErrorCount.Name = "txtErrorCount";
			this.txtErrorCount.ReadOnly = true;
			this.txtErrorCount.Size = new System.Drawing.Size(103, 20);
			this.txtErrorCount.TabIndex = 25;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(10, 170);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(74, 13);
			this.label7.Text = "Error Count:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(204, 262);
			this.Controls.Add(this.txtErrorCount);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.chkSupressErrors);
			this.Controls.Add(this.chkWriteToTrackingDB);
			this.Controls.Add(this.chkWriteTrackLogFile);
			this.Controls.Add(this.txtCurrSpeed);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.chkWriteNMEALog);
			this.Controls.Add(this.txtNoOfSatellites);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtFixType);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtFixTime);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtLongitude);
			this.Controls.Add(this.txtLatitude);
			this.Controls.Add(this.commport);
			this.Controls.Add(this.StartStockTrackBtn);
			this.Location = new System.Drawing.Point(10, 0);
			this.Name = "MainForm";
			this.Text = "GPS Tracker";
			this.ResumeLayout(false);
		//	this.PerformLayout();
		}
		private System.Windows.Forms.Button StartStockTrackBtn;
		private System.Windows.Forms.CheckBox chkWriteTrackLogFile;
		private System.Windows.Forms.TextBox txtCurrSpeed;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button2;
		
		private System.Windows.Forms.CheckBox chkWriteToTrackingDB;
		private System.Windows.Forms.CheckBox chkWriteNMEALog;
		private System.Windows.Forms.TextBox txtNoOfSatellites;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtFixTime;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtFixType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLongitude;
		private System.Windows.Forms.TextBox txtLatitude;
		private System.Windows.Forms.TextBox commport;
		
		private System.Windows.Forms.CheckBox chkSupressErrors;

		private System.Windows.Forms.TextBox txtErrorCount;
		
		private System.Windows.Forms.Label label7;

	
		

		

		

	}
}
