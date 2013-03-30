/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 05/10/2007
 * Time: 20:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ExportGPSInfo
{
	partial class MainForm
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
			this.btnTrackFile = new System.Windows.Forms.Button();
			this.btnLogLocationWS = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnTrackFile
			// 
			this.btnTrackFile.Location = new System.Drawing.Point(12, 12);
			this.btnTrackFile.Name = "btnTrackFile";
			this.btnTrackFile.Size = new System.Drawing.Size(172, 23);
			this.btnTrackFile.TabIndex = 0;
			this.btnTrackFile.Text = "CreateTrackfile";
			this.btnTrackFile.Click += new System.EventHandler(this.BtnExportClick);
			// 
			// btnLogLocationWS
			// 
			this.btnLogLocationWS.Location = new System.Drawing.Point(12, 41);
			this.btnLogLocationWS.Name = "btnLogLocationWS";
			this.btnLogLocationWS.Size = new System.Drawing.Size(172, 23);
			this.btnLogLocationWS.TabIndex = 1;
			this.btnLogLocationWS.Text = "LogLocationWS";
			this.btnLogLocationWS.Click += new System.EventHandler(this.BtnLogLocationWSClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(238, 295);
			this.Controls.Add(this.btnLogLocationWS);
			this.Controls.Add(this.btnTrackFile);
			this.Name = "MainForm";
			this.Text = "ExportGPSInfo";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnLogLocationWS;
		private System.Windows.Forms.Button btnTrackFile;
		

	}
}
