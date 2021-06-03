
namespace VectorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonCursor = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelThickness = new System.Windows.Forms.Label();
            this.thicknessBar = new System.Windows.Forms.TrackBar();
            this.colorButton2 = new System.Windows.Forms.Button();
            this.colorButton1 = new System.Windows.Forms.Button();
            this.buttonRotate = new System.Windows.Forms.Button();
            this.buttonMirror = new System.Windows.Forms.Button();
            this.buttonPolygon = new System.Windows.Forms.Button();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonRectangle = new System.Windows.Forms.Button();
            this.buttonLine = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Location = new System.Drawing.Point(3, 85);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1057, 828);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // buttonCursor
            // 
            this.buttonCursor.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonCursor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCursor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCursor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCursor.ForeColor = System.Drawing.Color.Brown;
            this.buttonCursor.Image = ((System.Drawing.Image)(resources.GetObject("buttonCursor.Image")));
            this.buttonCursor.Location = new System.Drawing.Point(10, 15);
            this.buttonCursor.Name = "buttonCursor";
            this.buttonCursor.Size = new System.Drawing.Size(51, 41);
            this.buttonCursor.TabIndex = 8;
            this.buttonCursor.Tag = "Курсор";
            this.buttonCursor.UseVisualStyleBackColor = false;
            this.buttonCursor.Click += new System.EventHandler(this.buttonCursor_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelThickness);
            this.panel1.Controls.Add(this.thicknessBar);
            this.panel1.Controls.Add(this.colorButton2);
            this.panel1.Controls.Add(this.colorButton1);
            this.panel1.Controls.Add(this.buttonRotate);
            this.panel1.Controls.Add(this.buttonMirror);
            this.panel1.Controls.Add(this.buttonPolygon);
            this.panel1.Controls.Add(this.buttonCircle);
            this.panel1.Controls.Add(this.buttonRectangle);
            this.panel1.Controls.Add(this.buttonLine);
            this.panel1.Controls.Add(this.buttonCursor);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1040, 67);
            this.panel1.TabIndex = 9;
            // 
            // labelThickness
            // 
            this.labelThickness.AutoSize = true;
            this.labelThickness.Location = new System.Drawing.Point(656, 38);
            this.labelThickness.Name = "labelThickness";
            this.labelThickness.Size = new System.Drawing.Size(79, 20);
            this.labelThickness.TabIndex = 17;
            this.labelThickness.Text = "Толщина: ";
            this.labelThickness.Visible = false;
            // 
            // thicknessBar
            // 
            this.thicknessBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.thicknessBar.LargeChange = 500;
            this.thicknessBar.Location = new System.Drawing.Point(544, 3);
            this.thicknessBar.Maximum = 1000;
            this.thicknessBar.Minimum = 50;
            this.thicknessBar.Name = "thicknessBar";
            this.thicknessBar.Size = new System.Drawing.Size(290, 56);
            this.thicknessBar.SmallChange = 100;
            this.thicknessBar.TabIndex = 16;
            this.thicknessBar.TickFrequency = 100;
            this.thicknessBar.Value = 50;
            this.thicknessBar.Visible = false;
            this.thicknessBar.Scroll += new System.EventHandler(this.thicknessBar_Scroll);
            // 
            // colorButton2
            // 
            this.colorButton2.BackColor = System.Drawing.Color.Black;
            this.colorButton2.Location = new System.Drawing.Point(475, 19);
            this.colorButton2.Name = "colorButton2";
            this.colorButton2.Size = new System.Drawing.Size(40, 41);
            this.colorButton2.TabIndex = 15;
            this.colorButton2.UseVisualStyleBackColor = false;
            this.colorButton2.Click += new System.EventHandler(this.colorButton2_Click);
            // 
            // colorButton1
            // 
            this.colorButton1.BackColor = System.Drawing.Color.White;
            this.colorButton1.Location = new System.Drawing.Point(455, 7);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(40, 41);
            this.colorButton1.TabIndex = 14;
            this.colorButton1.UseVisualStyleBackColor = false;
            this.colorButton1.Click += new System.EventHandler(this.colorButton1_Click);
            // 
            // buttonRotate
            // 
            this.buttonRotate.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonRotate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonRotate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonRotate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRotate.ForeColor = System.Drawing.Color.Brown;
            this.buttonRotate.Image = ((System.Drawing.Image)(resources.GetObject("buttonRotate.Image")));
            this.buttonRotate.Location = new System.Drawing.Point(381, 15);
            this.buttonRotate.Name = "buttonRotate";
            this.buttonRotate.Size = new System.Drawing.Size(54, 41);
            this.buttonRotate.TabIndex = 13;
            this.buttonRotate.Tag = "Курсор";
            this.buttonRotate.UseVisualStyleBackColor = false;
            // 
            // buttonMirror
            // 
            this.buttonMirror.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonMirror.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonMirror.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonMirror.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonMirror.ForeColor = System.Drawing.Color.Brown;
            this.buttonMirror.Image = ((System.Drawing.Image)(resources.GetObject("buttonMirror.Image")));
            this.buttonMirror.Location = new System.Drawing.Point(320, 15);
            this.buttonMirror.Name = "buttonMirror";
            this.buttonMirror.Size = new System.Drawing.Size(50, 41);
            this.buttonMirror.TabIndex = 11;
            this.buttonMirror.Tag = "Курсор";
            this.buttonMirror.UseVisualStyleBackColor = false;
            // 
            // buttonPolygon
            // 
            this.buttonPolygon.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonPolygon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonPolygon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonPolygon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonPolygon.ForeColor = System.Drawing.Color.Brown;
            this.buttonPolygon.Image = ((System.Drawing.Image)(resources.GetObject("buttonPolygon.Image")));
            this.buttonPolygon.Location = new System.Drawing.Point(253, 15);
            this.buttonPolygon.Name = "buttonPolygon";
            this.buttonPolygon.Size = new System.Drawing.Size(55, 41);
            this.buttonPolygon.TabIndex = 12;
            this.buttonPolygon.Tag = "Курсор";
            this.buttonPolygon.UseVisualStyleBackColor = false;
            // 
            // buttonCircle
            // 
            this.buttonCircle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonCircle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCircle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCircle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCircle.ForeColor = System.Drawing.Color.Brown;
            this.buttonCircle.Image = ((System.Drawing.Image)(resources.GetObject("buttonCircle.Image")));
            this.buttonCircle.Location = new System.Drawing.Point(192, 15);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(50, 41);
            this.buttonCircle.TabIndex = 11;
            this.buttonCircle.Tag = "Курсор";
            this.buttonCircle.UseVisualStyleBackColor = false;
            this.buttonCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // buttonRectangle
            // 
            this.buttonRectangle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonRectangle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonRectangle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonRectangle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRectangle.ForeColor = System.Drawing.Color.Brown;
            this.buttonRectangle.Image = ((System.Drawing.Image)(resources.GetObject("buttonRectangle.Image")));
            this.buttonRectangle.Location = new System.Drawing.Point(131, 15);
            this.buttonRectangle.Name = "buttonRectangle";
            this.buttonRectangle.Size = new System.Drawing.Size(49, 41);
            this.buttonRectangle.TabIndex = 10;
            this.buttonRectangle.Tag = "Курсор";
            this.buttonRectangle.UseVisualStyleBackColor = false;
            this.buttonRectangle.Click += new System.EventHandler(this.buttonRectangle_Click);
            // 
            // buttonLine
            // 
            this.buttonLine.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonLine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonLine.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonLine.ForeColor = System.Drawing.Color.Brown;
            this.buttonLine.Image = ((System.Drawing.Image)(resources.GetObject("buttonLine.Image")));
            this.buttonLine.Location = new System.Drawing.Point(72, 15);
            this.buttonLine.Name = "buttonLine";
            this.buttonLine.Size = new System.Drawing.Size(48, 41);
            this.buttonLine.TabIndex = 9;
            this.buttonLine.Tag = "Курсор";
            this.buttonLine.UseVisualStyleBackColor = false;
            this.buttonLine.Click += new System.EventHandler(this.buttonLine_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(1064, 845);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "VectorEditor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonCursor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonRotate;
        private System.Windows.Forms.Button buttonMirror;
        private System.Windows.Forms.Button buttonPolygon;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button buttonRectangle;
        private System.Windows.Forms.Button buttonLine;
        private System.Windows.Forms.Button colorButton1;
        private System.Windows.Forms.Button colorButton2;
        private System.Windows.Forms.TrackBar thicknessBar;
        private System.Windows.Forms.Label labelThickness;
    }
}

