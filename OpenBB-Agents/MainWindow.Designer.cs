namespace Window
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			tabControl1 = new TabControl();
			tabPage1 = new TabPage();
			flowLayoutPanel1 = new FlowLayoutPanel();
			inputLabel = new Label();
			milissecondsSelector = new NumericUpDown();
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)milissecondsSelector).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Dock = DockStyle.Fill;
			tabControl1.Location = new Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(333, 77);
			tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(flowLayoutPanel1);
			tabPage1.Location = new Point(4, 29);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new Padding(3);
			tabPage1.Size = new Size(325, 44);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "Click";
			tabPage1.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel1
			// 
			flowLayoutPanel1.Controls.Add(inputLabel);
			flowLayoutPanel1.Controls.Add(milissecondsSelector);
			flowLayoutPanel1.Dock = DockStyle.Fill;
			flowLayoutPanel1.Location = new Point(3, 3);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new Size(319, 38);
			flowLayoutPanel1.TabIndex = 0;
			// 
			// inputLabel
			// 
			inputLabel.AutoSize = true;
			inputLabel.Dock = DockStyle.Fill;
			inputLabel.Location = new Point(3, 0);
			inputLabel.Name = "inputLabel";
			inputLabel.Size = new Size(28, 20);
			inputLabel.TabIndex = 1;
			inputLabel.Text = "ms";
			// 
			// milissecondsSelector
			// 
			milissecondsSelector.Dock = DockStyle.Fill;
			milissecondsSelector.Location = new Point(37, 3);
			milissecondsSelector.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			milissecondsSelector.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			milissecondsSelector.Name = "milissecondsSelector";
			milissecondsSelector.Size = new Size(150, 27);
			milissecondsSelector.TabIndex = 0;
			milissecondsSelector.Value = new decimal(new int[] { 20, 0, 0, 0 });
			milissecondsSelector.ValueChanged += this.milissecondsSelector_ValueChanged;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new SizeF(8F, 20F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(333, 77);
			this.Controls.Add(tabControl1);
			this.Name = "MainForm";
			this.Text = "OpenBBot";
			tabControl1.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)milissecondsSelector).EndInit();
			this.ResumeLayout(false);
		}

		#endregion

		private TabControl tabControl1;
		private TabPage tabPage1;
		private FlowLayoutPanel flowLayoutPanel1;
		private Label inputLabel;
		private NumericUpDown milissecondsSelector;
	}
}