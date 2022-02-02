namespace SudokuSolver.View
{
	using SudokuSolver.View;

	partial class FrmSudokuSelection
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
			SudokuSolver.Model.Data.Sudoku sudoku1 = new SudokuSolver.Model.Data.Sudoku();
			this.lblInfo = new System.Windows.Forms.Label();
			this.pbIcon = new System.Windows.Forms.PictureBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblPreview = new System.Windows.Forms.Label();
			this.cbSudokus = new System.Windows.Forms.ComboBox();
			this.sfPreview = new SudokuSolver.View.SudokuField();
			((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// lblInfo
			// 
			this.lblInfo.Location = new System.Drawing.Point(12, 63);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(192, 48);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "You have selected a file which contains several sudokus. Please select one out of" +
    " the list below.";
			// 
			// pbIcon
			// 
			this.pbIcon.Location = new System.Drawing.Point(15, 12);
			this.pbIcon.Name = "pbIcon";
			this.pbIcon.Size = new System.Drawing.Size(189, 43);
			this.pbIcon.TabIndex = 1;
			this.pbIcon.TabStop = false;
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(129, 238);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(75, 23);
			this.btnSelect.TabIndex = 4;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(15, 238);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// lblPreview
			// 
			this.lblPreview.AutoSize = true;
			this.lblPreview.Location = new System.Drawing.Point(257, 12);
			this.lblPreview.Name = "lblPreview";
			this.lblPreview.Size = new System.Drawing.Size(55, 14);
			this.lblPreview.TabIndex = 7;
			this.lblPreview.Text = "Preview: ";
			// 
			// cbSudokus
			// 
			this.cbSudokus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSudokus.FormattingEnabled = true;
			this.cbSudokus.Location = new System.Drawing.Point(15, 126);
			this.cbSudokus.Name = "cbSudokus";
			this.cbSudokus.Size = new System.Drawing.Size(189, 22);
			this.cbSudokus.TabIndex = 8;
			this.cbSudokus.SelectedIndexChanged += new System.EventHandler(this.CbSudokus_SelectedIndexChanged);
			// 
			// sfPreview
			// 
			this.sfPreview.EditingEnabled = false;
			this.sfPreview.GridBorderWidth = 3;
			this.sfPreview.GridColor = System.Drawing.Color.Orange;
			this.sfPreview.GridInnerBorderWidth = 2;
			this.sfPreview.GridWidth = 1;
			this.sfPreview.HoverActivated = false;
			this.sfPreview.HoveredCellColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.sfPreview.IsAlwaysFocused = false;
			this.sfPreview.Location = new System.Drawing.Point(260, 29);
			this.sfPreview.Name = "sfPreview";
			this.sfPreview.NonPresetCellForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.sfPreview.PresetCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.sfPreview.PresetCellForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.sfPreview.SelectedCellColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(120)))), ((int)(((byte)(80)))));
			this.sfPreview.ShowCandidates = false;
			this.sfPreview.Size = new System.Drawing.Size(232, 232);
			sudoku1.Name = "Sudoku38537316";
			this.sfPreview.Sudoku = sudoku1;
			this.sfPreview.TabIndex = 2;
			this.sfPreview.Text = "sudokuField1";
			// 
			// FrmSudokuSelection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(504, 273);
			this.Controls.Add(this.cbSudokus);
			this.Controls.Add(this.lblPreview);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.sfPreview);
			this.Controls.Add(this.pbIcon);
			this.Controls.Add(this.lblInfo);
			this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FrmSudokuSelection";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Sudoku";
			this.Load += new System.EventHandler(this.FrmSudokuSelection_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.PictureBox pbIcon;
		private SudokuField sfPreview;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblPreview;
		private System.Windows.Forms.ComboBox cbSudokus;
	}
}