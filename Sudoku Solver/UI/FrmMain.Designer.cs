namespace SudokuSolver.UI
{
	using SudokuSolver.Sudoku;

	partial class FrmMain
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
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
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
			SudokuSolver.Sudoku.Sudoku sudoku1 = new SudokuSolver.Sudoku.Sudoku();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			this.btnSolve = new System.Windows.Forms.Button();
			this.sudokuField1 = new SudokuSolver.UI.SudokuField();
			this.customMenuStrip1 = new SudokuSolver.UI.CustomMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsTextfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteNonPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.solvingMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.humanSolvingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.backTrackingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.animatedSolvingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showCandidatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.changeColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.germanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSolve
			// 
			this.btnSolve.Location = new System.Drawing.Point(113, 492);
			this.btnSolve.Name = "btnSolve";
			this.btnSolve.Size = new System.Drawing.Size(197, 34);
			this.btnSolve.TabIndex = 11;
			this.btnSolve.Text = "Solve - Human Solving";
			this.btnSolve.UseVisualStyleBackColor = true;
			this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
			// 
			// sudokuField1
			// 
			this.sudokuField1.EditingEnabled = true;
			this.sudokuField1.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold);
			this.sudokuField1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
			this.sudokuField1.GridBorderWidth = 5;
			this.sudokuField1.GridColor = System.Drawing.Color.Orange;
			this.sudokuField1.GridInnerBorderWidth = 3;
			this.sudokuField1.GridWidth = 1;
			this.sudokuField1.HoverActivated = true;
			this.sudokuField1.HoveredCellColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.sudokuField1.IsAlwaysFocused = true;
			this.sudokuField1.Location = new System.Drawing.Point(12, 86);
			this.sudokuField1.Name = "sudokuField1";
			this.sudokuField1.NonPresetCellForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.sudokuField1.PresetCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.sudokuField1.PresetCellForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.sudokuField1.SelectedCellColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(80)))));
			this.sudokuField1.ShowCandidates = false;
			this.sudokuField1.Size = new System.Drawing.Size(400, 400);
			sudoku1.Name = "Sudoku96322692";
			sudoku1.RaiseCellChangedEvent = false;
			this.sudokuField1.Sudoku = sudoku1;
			this.sudokuField1.TabIndex = 0;
			this.sudokuField1.TabStop = false;
			// 
			// customMenuStrip1
			// 
			this.customMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.customMenuStrip1.Location = new System.Drawing.Point(0, 0);
			this.customMenuStrip1.Name = "customMenuStrip1";
			this.customMenuStrip1.Size = new System.Drawing.Size(424, 24);
			this.customMenuStrip1.TabIndex = 12;
			this.customMenuStrip1.Text = "customMenuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
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
            this.saveAsBitmapToolStripMenuItem,
            this.saveAsXMLToolStripMenuItem});
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
			// saveAsXMLToolStripMenuItem
			// 
			this.saveAsXMLToolStripMenuItem.Name = "saveAsXMLToolStripMenuItem";
			this.saveAsXMLToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.saveAsXMLToolStripMenuItem.Text = "Save As XML";
			this.saveAsXMLToolStripMenuItem.Click += new System.EventHandler(this.saveAsXMLToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(100, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteNonPresetToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// deleteNonPresetToolStripMenuItem
			// 
			this.deleteNonPresetToolStripMenuItem.Name = "deleteNonPresetToolStripMenuItem";
			this.deleteNonPresetToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.deleteNonPresetToolStripMenuItem.Text = "Delete NonPreset";
			this.deleteNonPresetToolStripMenuItem.Click += new System.EventHandler(this.deleteNonPresetToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solvingMethodToolStripMenuItem,
            this.animatedSolvingToolStripMenuItem,
            this.showCandidatesToolStripMenuItem,
            this.changeColorsToolStripMenuItem,
            this.languageToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// solvingMethodToolStripMenuItem
			// 
			this.solvingMethodToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.humanSolvingToolStripMenuItem,
            this.backTrackingToolStripMenuItem});
			this.solvingMethodToolStripMenuItem.Name = "solvingMethodToolStripMenuItem";
			this.solvingMethodToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.solvingMethodToolStripMenuItem.Text = "Solving Method";
			// 
			// humanSolvingToolStripMenuItem
			// 
			this.humanSolvingToolStripMenuItem.Checked = true;
			this.humanSolvingToolStripMenuItem.CheckOnClick = true;
			this.humanSolvingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.humanSolvingToolStripMenuItem.Name = "humanSolvingToolStripMenuItem";
			this.humanSolvingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.humanSolvingToolStripMenuItem.Text = "Human Solving";
			this.humanSolvingToolStripMenuItem.Click += new System.EventHandler(this.humanSolvingToolStripMenuItem_Click);
			// 
			// backTrackingToolStripMenuItem
			// 
			this.backTrackingToolStripMenuItem.CheckOnClick = true;
			this.backTrackingToolStripMenuItem.Name = "backTrackingToolStripMenuItem";
			this.backTrackingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.backTrackingToolStripMenuItem.Text = "BackTracking";
			this.backTrackingToolStripMenuItem.Click += new System.EventHandler(this.backTrackingToolStripMenuItem_Click);
			// 
			// animatedSolvingToolStripMenuItem
			// 
			this.animatedSolvingToolStripMenuItem.Checked = true;
			this.animatedSolvingToolStripMenuItem.CheckOnClick = true;
			this.animatedSolvingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.animatedSolvingToolStripMenuItem.Name = "animatedSolvingToolStripMenuItem";
			this.animatedSolvingToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.animatedSolvingToolStripMenuItem.Text = "Animated Solving";
			this.animatedSolvingToolStripMenuItem.Click += new System.EventHandler(this.animatedSolvingToolStripMenuItem_Click);
			// 
			// showCandidatesToolStripMenuItem
			// 
			this.showCandidatesToolStripMenuItem.CheckOnClick = true;
			this.showCandidatesToolStripMenuItem.Name = "showCandidatesToolStripMenuItem";
			this.showCandidatesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.showCandidatesToolStripMenuItem.Text = "Show Candidates";
			this.showCandidatesToolStripMenuItem.Click += new System.EventHandler(this.showCandidatesToolStripMenuItem_Click);
			// 
			// changeColorsToolStripMenuItem
			// 
			this.changeColorsToolStripMenuItem.Enabled = false;
			this.changeColorsToolStripMenuItem.Name = "changeColorsToolStripMenuItem";
			this.changeColorsToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.changeColorsToolStripMenuItem.Text = "Change Colors";
			// 
			// languageToolStripMenuItem
			// 
			this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.germanToolStripMenuItem,
            this.englishToolStripMenuItem});
			this.languageToolStripMenuItem.Enabled = false;
			this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
			this.languageToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.languageToolStripMenuItem.Text = "Language";
			// 
			// germanToolStripMenuItem
			// 
			this.germanToolStripMenuItem.Name = "germanToolStripMenuItem";
			this.germanToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.germanToolStripMenuItem.Text = "German";
			// 
			// englishToolStripMenuItem
			// 
			this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
			this.englishToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.englishToolStripMenuItem.Text = "English";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Enabled = false;
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(424, 538);
			this.Controls.Add(this.btnSolve);
			this.Controls.Add(this.sudokuField1);
			this.Controls.Add(this.customMenuStrip1);
			this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.customMenuStrip1;
			this.MaximizeBox = false;
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sudoku Solver - Plexian";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Shown += new System.EventHandler(this.frmMain_Shown);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
			this.customMenuStrip1.ResumeLayout(false);
			this.customMenuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SudokuField sudokuField1;
		private System.Windows.Forms.Button btnSolve;
		private CustomMenuStrip customMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsTextfileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsBitmapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteNonPresetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showCandidatesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem solvingMethodToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem humanSolvingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem changeColorsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem germanToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem backTrackingToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsXMLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem animatedSolvingToolStripMenuItem;

	}
}

