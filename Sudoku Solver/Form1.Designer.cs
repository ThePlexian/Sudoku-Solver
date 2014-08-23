namespace SudokuSolver
{
	partial class frmMain
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.btnSolve = new System.Windows.Forms.Button();
			this.sudokuField1 = new SudokuSolver.SudokuField();
			this.customMenuStrip1 = new SudokuSolver.CustomMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsTextfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetToInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showCandidatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allowEditingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(544, 331);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 8;
			// 
			// btnSolve
			// 
			this.btnSolve.Location = new System.Drawing.Point(109, 443);
			this.btnSolve.Name = "btnSolve";
			this.btnSolve.Size = new System.Drawing.Size(197, 32);
			this.btnSolve.TabIndex = 11;
			this.btnSolve.Text = "Solve";
			this.btnSolve.UseVisualStyleBackColor = true;
			this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
			// 
			// sudokuField1
			// 
			this.sudokuField1.EditingEnabled = true;
			this.sudokuField1.Font = new System.Drawing.Font("Tahoma", 13.25F);
			this.sudokuField1.GridBorderWidth = 5;
			this.sudokuField1.GridColor = System.Drawing.Color.Orange;
			this.sudokuField1.GridInnerBorderWidth = 3;
			this.sudokuField1.GridWidth = 1;
			this.sudokuField1.HoverActivated = true;
			this.sudokuField1.HoveredCellColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.sudokuField1.Location = new System.Drawing.Point(12, 46);
			this.sudokuField1.Name = "sudokuField1";
			this.sudokuField1.NonPresetCellForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.sudokuField1.PresetCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.sudokuField1.PresetCellForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.sudokuField1.SelectedCellColor = System.Drawing.Color.Red;
			this.sudokuField1.ShowCandidates = true;
			this.sudokuField1.Size = new System.Drawing.Size(391, 391);
			this.sudokuField1.Sudoku = null;
			this.sudokuField1.TabIndex = 0;
			this.sudokuField1.TabStop = false;
			// 
			// customMenuStrip1
			// 
			this.customMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.customMenuStrip1.Location = new System.Drawing.Point(0, 0);
			this.customMenuStrip1.Name = "customMenuStrip1";
			this.customMenuStrip1.Size = new System.Drawing.Size(415, 24);
			this.customMenuStrip1.TabIndex = 12;
			this.customMenuStrip1.Text = "customMenuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsTextfileToolStripMenuItem,
            this.saveAsBitmapToolStripMenuItem});
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.saveToolStripMenuItem.Text = "Save";
			// 
			// saveAsTextfileToolStripMenuItem
			// 
			this.saveAsTextfileToolStripMenuItem.Name = "saveAsTextfileToolStripMenuItem";
			this.saveAsTextfileToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.saveAsTextfileToolStripMenuItem.Text = "Save As Textfile";
			this.saveAsTextfileToolStripMenuItem.Click += new System.EventHandler(this.saveAsTextfileToolStripMenuItem_Click);
			// 
			// saveAsBitmapToolStripMenuItem
			// 
			this.saveAsBitmapToolStripMenuItem.Name = "saveAsBitmapToolStripMenuItem";
			this.saveAsBitmapToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.saveAsBitmapToolStripMenuItem.Text = "Save As Bitmap";
			this.saveAsBitmapToolStripMenuItem.Click += new System.EventHandler(this.saveAsBitmapToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToInputToolStripMenuItem,
            this.showCandidatesToolStripMenuItem,
            this.allowEditingToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// resetToInputToolStripMenuItem
			// 
			this.resetToInputToolStripMenuItem.Name = "resetToInputToolStripMenuItem";
			this.resetToInputToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.resetToInputToolStripMenuItem.Text = "Reset To Input";
			this.resetToInputToolStripMenuItem.Click += new System.EventHandler(this.resetToInputToolStripMenuItem_Click);
			// 
			// showCandidatesToolStripMenuItem
			// 
			this.showCandidatesToolStripMenuItem.Name = "showCandidatesToolStripMenuItem";
			this.showCandidatesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.showCandidatesToolStripMenuItem.Text = "Show Candidates";
			this.showCandidatesToolStripMenuItem.Click += new System.EventHandler(this.showCandidatesToolStripMenuItem_Click);
			// 
			// allowEditingToolStripMenuItem
			// 
			this.allowEditingToolStripMenuItem.Checked = true;
			this.allowEditingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.allowEditingToolStripMenuItem.Name = "allowEditingToolStripMenuItem";
			this.allowEditingToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.allowEditingToolStripMenuItem.Text = "Allow Editing";
			this.allowEditingToolStripMenuItem.Click += new System.EventHandler(this.allowEditingToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Enabled = false;
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(415, 482);
			this.Controls.Add(this.btnSolve);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.sudokuField1);
			this.Controls.Add(this.customMenuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.customMenuStrip1;
			this.MaximizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sudoku Solver - ThePlexian - ";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.customMenuStrip1.ResumeLayout(false);
			this.customMenuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SudokuField sudokuField1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSolve;
		private CustomMenuStrip customMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsTextfileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsBitmapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetToInputToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showCandidatesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allowEditingToolStripMenuItem;

	}
}

