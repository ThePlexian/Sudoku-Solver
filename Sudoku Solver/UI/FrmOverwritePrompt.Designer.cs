namespace SudokuSolver.UI
{
	partial class FrmOverwritePrompt
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnOverwrite = new System.Windows.Forms.Button();
			this.btnAppend = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblText = new System.Windows.Forms.Label();
			this.pbIcon = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOverwrite
			// 
			this.btnOverwrite.Location = new System.Drawing.Point(74, 74);
			this.btnOverwrite.Name = "btnOverwrite";
			this.btnOverwrite.Size = new System.Drawing.Size(75, 25);
			this.btnOverwrite.TabIndex = 0;
			this.btnOverwrite.Text = "Overwrite";
			this.btnOverwrite.UseVisualStyleBackColor = true;
			this.btnOverwrite.Click += new System.EventHandler(this.btnOverwrite_Click);
			// 
			// btnAppend
			// 
			this.btnAppend.Location = new System.Drawing.Point(155, 74);
			this.btnAppend.Name = "btnAppend";
			this.btnAppend.Size = new System.Drawing.Size(75, 25);
			this.btnAppend.TabIndex = 1;
			this.btnAppend.Text = "Append";
			this.btnAppend.UseVisualStyleBackColor = true;
			this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(236, 74);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 25);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblText
			// 
			this.lblText.Location = new System.Drawing.Point(71, 13);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(240, 47);
			this.lblText.TabIndex = 3;
			this.lblText.Text = "[Info text]";
			this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbIcon
			// 
			this.pbIcon.Location = new System.Drawing.Point(12, 13);
			this.pbIcon.Name = "pbIcon";
			this.pbIcon.Size = new System.Drawing.Size(53, 47);
			this.pbIcon.TabIndex = 4;
			this.pbIcon.TabStop = false;
			// 
			// FrmOverwritePrompt
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(323, 112);
			this.Controls.Add(this.pbIcon);
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAppend);
			this.Controls.Add(this.btnOverwrite);
			this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FrmOverwritePrompt";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Confirm saving";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmOverwritePrompt_FormClosing);
			this.Load += new System.EventHandler(this.frmSavePrompt_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOverwrite;
		private System.Windows.Forms.Button btnAppend;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblText;
		private System.Windows.Forms.PictureBox pbIcon;
	}
}