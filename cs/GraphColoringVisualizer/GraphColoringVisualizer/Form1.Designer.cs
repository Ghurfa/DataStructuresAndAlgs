namespace GraphColoringVisualizer
{
    partial class Form1
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
            this.colorButton = new System.Windows.Forms.Button();
            this.colorCountInput = new System.Windows.Forms.NumericUpDown();
            this.display = new System.Windows.Forms.PictureBox();
            this.colorCountLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.undoButton = new System.Windows.Forms.Button();
            this.redoButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.colorCountInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.display)).BeginInit();
            this.SuspendLayout();
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(110, 371);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(75, 23);
            this.colorButton.TabIndex = 0;
            this.colorButton.Text = "Color";
            this.colorButton.UseVisualStyleBackColor = true;
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            // 
            // colorCountInput
            // 
            this.colorCountInput.Location = new System.Drawing.Point(310, 371);
            this.colorCountInput.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.colorCountInput.Name = "colorCountInput";
            this.colorCountInput.Size = new System.Drawing.Size(120, 23);
            this.colorCountInput.TabIndex = 1;
            this.colorCountInput.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // display
            // 
            this.display.Location = new System.Drawing.Point(128, 12);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(531, 316);
            this.display.TabIndex = 2;
            this.display.TabStop = false;
            this.display.MouseClick += new System.Windows.Forms.MouseEventHandler(this.display_MouseClick);
            // 
            // colorCountLabel
            // 
            this.colorCountLabel.AutoSize = true;
            this.colorCountLabel.Location = new System.Drawing.Point(239, 375);
            this.colorCountLabel.Name = "colorCountLabel";
            this.colorCountLabel.Size = new System.Drawing.Size(65, 15);
            this.colorCountLabel.TabIndex = 3;
            this.colorCountLabel.Text = "# of Colors";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(531, 377);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 15);
            this.statusLabel.TabIndex = 4;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(20, 22);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 5;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // undoButton
            // 
            this.undoButton.Location = new System.Drawing.Point(19, 256);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(75, 23);
            this.undoButton.TabIndex = 6;
            this.undoButton.Text = "Undo";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.undoButton_Click);
            // 
            // redoButton
            // 
            this.redoButton.Location = new System.Drawing.Point(20, 285);
            this.redoButton.Name = "redoButton";
            this.redoButton.Size = new System.Drawing.Size(75, 23);
            this.redoButton.TabIndex = 7;
            this.redoButton.Text = "Redo";
            this.redoButton.UseVisualStyleBackColor = true;
            this.redoButton.Click += new System.EventHandler(this.redoButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.redoButton);
            this.Controls.Add(this.undoButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.colorCountLabel);
            this.Controls.Add(this.display);
            this.Controls.Add(this.colorCountInput);
            this.Controls.Add(this.colorButton);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.colorCountInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.display)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button colorButton;
        private NumericUpDown colorCountInput;
        private PictureBox display;
        private Label colorCountLabel;
        private Label statusLabel;
        private Button clearButton;
        private Button undoButton;
        private Button redoButton;
    }
}