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
			this.button1 = new System.Windows.Forms.Button();
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
			this.chkWriteLog = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.txtCurrSpeed = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.chkWriteTrackLog = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(24, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(85, 23);
			this.button1.TabIndex = 13;
			this.button1.Text = "Start";
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// commport
			// 
			this.commport.Location = new System.Drawing.Point(115, 4);
			this.commport.Name = "commport";
			this.commport.Size = new System.Drawing.Size(53, 21);
			this.commport.TabIndex = 12;
			this.commport.Text = m_strGPSCOMPort;
			// 
			// txtLatitude
			// 
			this.txtLatitude.Location = new System.Drawing.Point(80, 31);
			this.txtLatitude.Name = "txtLatitude";
			this.txtLatitude.ReadOnly = true;
			this.txtLatitude.Size = new System.Drawing.Size(112, 21);
			this.txtLatitude.TabIndex = 11;
			// 
			// txtLongitude
			// 
			this.txtLongitude.Location = new System.Drawing.Point(80, 58);
			this.txtLongitude.Name = "txtLongitude";
			this.txtLongitude.ReadOnly = true;
			this.txtLongitude.Size = new System.Drawing.Size(112, 21);
			this.txtLongitude.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 20);
			this.label1.TabIndex = 9;
			this.label1.Text = "Latitude:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 18);
			this.label2.TabIndex = 8;
			this.label2.Text = "Longitude:";
			// 
			// txtFixTime
			// 
			this.txtFixTime.Location = new System.Drawing.Point(90, 86);
			this.txtFixTime.Name = "txtFixTime";
			this.txtFixTime.ReadOnly = true;
			this.txtFixTime.Size = new System.Drawing.Size(102, 21);
			this.txtFixTime.TabIndex = 5;
			// 
			// txtFixType
			// 
			this.txtFixType.Location = new System.Drawing.Point(90, 113);
			this.txtFixType.Name = "txtFixType";
			this.txtFixType.ReadOnly = true;
			this.txtFixType.Size = new System.Drawing.Size(102, 21);
			this.txtFixType.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 89);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 18);
			this.label3.TabIndex = 4;
			this.label3.Text = "Fix Time:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 116);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 18);
			this.label4.TabIndex = 2;
			this.label4.Text = "Fix Type:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(11, 143);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(73, 18);
			this.label5.TabIndex = 1;
			this.label5.Text = "No Of Sats:";
			// 
			// txtNoOfSatellites
			// 
			this.txtNoOfSatellites.Location = new System.Drawing.Point(90, 140);
			this.txtNoOfSatellites.Name = "txtNoOfSatellites";
			this.txtNoOfSatellites.ReadOnly = true;
			this.txtNoOfSatellites.Size = new System.Drawing.Size(102, 21);
			this.txtNoOfSatellites.TabIndex = 0;
			// 
			// chkWriteLog
			// 
			this.chkWriteLog.Location = new System.Drawing.Point(36, 194);
			this.chkWriteLog.Name = "chkWriteLog";
			this.chkWriteLog.Size = new System.Drawing.Size(156, 18);
			this.chkWriteLog.TabIndex = 15;
			this.chkWriteLog.Text = "Write NMEA Log";
			this.chkWriteLog.CheckStateChanged += new System.EventHandler(this.ChkWriteLogCheckedChanged);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(174, 5);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(25, 20);
			this.button2.TabIndex = 16;
			this.button2.Text = "R";
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// txtCurrSpeed
			// 
			this.txtCurrSpeed.Location = new System.Drawing.Point(90, 167);
			this.txtCurrSpeed.Name = "txtCurrSpeed";
			this.txtCurrSpeed.ReadOnly = true;
			this.txtCurrSpeed.Size = new System.Drawing.Size(102, 21);
			this.txtCurrSpeed.TabIndex = 17;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(11, 170);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(73, 18);
			this.label6.TabIndex = 18;
			this.label6.Text = "Speed MPH:";
			// 
			// chkWriteTrackLog
			// 
			this.chkWriteTrackLog.Location = new System.Drawing.Point(36, 212);
			this.chkWriteTrackLog.Name = "chkWriteTrackLog";
			this.chkWriteTrackLog.Size = new System.Drawing.Size(157, 21);
			this.chkWriteTrackLog.TabIndex = 19;
			this.chkWriteTrackLog.Text = "Write GPS Track";
			this.chkWriteTrackLog.CheckStateChanged += new System.EventHandler(this.ChkWriteTrackLogChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(204, 245);
			this.Controls.Add(this.chkWriteTrackLog);
			this.Controls.Add(this.txtCurrSpeed);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.chkWriteLog);
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
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "GPS Tracker";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.CheckBox chkWriteTrackLog;
		private System.Windows.Forms.TextBox txtCurrSpeed;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button2;
		
		private System.Windows.Forms.CheckBox chkWriteLog;
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
		private System.Windows.Forms.Button button1;
		

		

	
	}
}
