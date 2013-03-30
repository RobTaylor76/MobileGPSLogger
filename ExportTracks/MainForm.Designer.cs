/*
 * Created by SharpDevelop.
 * User: u771666
 * Date: 15/07/2010
 * Time: 15:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ExportTracks
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
			this.export = new System.Windows.Forms.Button();
			this.cbTrackIds = new System.Windows.Forms.ComboBox();
			this.exportSelectedTrack = new System.Windows.Forms.Button();
			this.cbPlacemarkDistances = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// export
			// 
			this.export.Location = new System.Drawing.Point(38, 12);
			this.export.Name = "export";
			this.export.Size = new System.Drawing.Size(121, 23);
			this.export.TabIndex = 0;
			this.export.Text = "Export LastTrack";
			this.export.Click += new System.EventHandler(this.ExportClick);
			// 
			// cbTrackIds
			// 
			this.cbTrackIds.Location = new System.Drawing.Point(4, 71);
			this.cbTrackIds.Name = "cbTrackIds";
			this.cbTrackIds.Size = new System.Drawing.Size(199, 21);
			this.cbTrackIds.TabIndex = 1;
			// 
			// exportSelectedTrack
			// 
			this.exportSelectedTrack.Location = new System.Drawing.Point(38, 99);
			this.exportSelectedTrack.Name = "exportSelectedTrack";
			this.exportSelectedTrack.Size = new System.Drawing.Size(121, 23);
			this.exportSelectedTrack.TabIndex = 2;
			this.exportSelectedTrack.Text = "Export Selected";
			this.exportSelectedTrack.Click += new System.EventHandler(this.ExportSelectedTrackClick);
			// 
			// cbPlacemarkDistances
			// 
			this.cbPlacemarkDistances.Location = new System.Drawing.Point(60, 170);
			this.cbPlacemarkDistances.Name = "cbPlacemarkDistances";
			this.cbPlacemarkDistances.Size = new System.Drawing.Size(74, 21);
			this.cbPlacemarkDistances.TabIndex = 3;
			this.cbPlacemarkDistances.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 149);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(180, 18);
			this.label1.Text = "Placemark Seperation (metres)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(204, 262);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbPlacemarkDistances);
			this.Controls.Add(this.exportSelectedTrack);
			this.Controls.Add(this.cbTrackIds);
			this.Controls.Add(this.export);
			this.Name = "MainForm";
			this.Text = "ExportTracks";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbPlacemarkDistances;
		private System.Windows.Forms.ComboBox cbTrackIds;
		private System.Windows.Forms.Button exportSelectedTrack;
		private System.Windows.Forms.Button export;
		

		

		

	}
}
