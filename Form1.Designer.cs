
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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PasteMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyDeleteMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonCursor = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelThickness = new System.Windows.Forms.Label();
            this.thicknessBar = new System.Windows.Forms.TrackBar();
            this.colorButton2 = new System.Windows.Forms.Button();
            this.colorButton1 = new System.Windows.Forms.Button();
            this.buttonPolygon = new System.Windows.Forms.Button();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonRectangle = new System.Windows.Forms.Button();
            this.buttonLine = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.PasteMenu.SuspendLayout();
            this.CopyDeleteMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.ContextMenuStrip = this.PasteMenu;
            this.pictureBox1.Location = new System.Drawing.Point(3, 65);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(2557, 1436);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(925, 565);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(925, 565);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // PasteMenu
            // 
            this.PasteMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Paste});
            this.PasteMenu.Name = "contextMenuStrip2";
            this.PasteMenu.Size = new System.Drawing.Size(164, 26);
            // 
            // Paste
            // 
            this.Paste.Name = "Paste";
            this.Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.Paste.Size = new System.Drawing.Size(163, 22);
            this.Paste.Text = "Вставить";
            this.Paste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // CopyDeleteMenu
            // 
            this.CopyDeleteMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Copy,
            this.Delete});
            this.CopyDeleteMenu.Name = "contextMenuStrip1";
            this.CopyDeleteMenu.Size = new System.Drawing.Size(182, 48);
            // 
            // Copy
            // 
            this.Copy.Name = "Copy";
            this.Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Copy.Size = new System.Drawing.Size(181, 22);
            this.Copy.Text = "Копировать";
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.Delete.Size = new System.Drawing.Size(181, 22);
            this.Delete.Text = "Удалить";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // buttonCursor
            // 
            this.buttonCursor.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonCursor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCursor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCursor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCursor.ForeColor = System.Drawing.Color.Brown;
            this.buttonCursor.Image = global::VectorEditor.Properties.Resources.resource__7_;
            this.buttonCursor.Location = new System.Drawing.Point(5, 3);
            this.buttonCursor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCursor.Name = "buttonCursor";
            this.buttonCursor.Size = new System.Drawing.Size(45, 42);
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
            this.panel1.Controls.Add(this.buttonPolygon);
            this.panel1.Controls.Add(this.buttonCircle);
            this.panel1.Controls.Add(this.buttonRectangle);
            this.panel1.Controls.Add(this.buttonLine);
            this.panel1.Controls.Add(this.buttonCursor);
            this.panel1.Location = new System.Drawing.Point(9, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.MaximumSize = new System.Drawing.Size(2553, 52);
            this.panel1.MinimumSize = new System.Drawing.Size(910, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(910, 52);
            this.panel1.TabIndex = 9;
            // 
            // labelThickness
            // 
            this.labelThickness.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelThickness.Location = new System.Drawing.Point(650, 3);
            this.labelThickness.Name = "labelThickness";
            this.labelThickness.Size = new System.Drawing.Size(184, 41);
            this.labelThickness.TabIndex = 17;
            this.labelThickness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelThickness.Visible = false;
            // 
            // thicknessBar
            // 
            this.thicknessBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.thicknessBar.LargeChange = 500;
            this.thicknessBar.Location = new System.Drawing.Point(391, 3);
            this.thicknessBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.thicknessBar.Maximum = 1000;
            this.thicknessBar.Minimum = 100;
            this.thicknessBar.Name = "thicknessBar";
            this.thicknessBar.Size = new System.Drawing.Size(254, 45);
            this.thicknessBar.SmallChange = 100;
            this.thicknessBar.TabIndex = 16;
            this.thicknessBar.TickFrequency = 100;
            this.thicknessBar.Value = 100;
            this.thicknessBar.Visible = false;
            this.thicknessBar.Scroll += new System.EventHandler(this.thicknessBar_Scroll);
            // 
            // colorButton2
            // 
            this.colorButton2.BackColor = System.Drawing.Color.Black;
            this.colorButton2.Location = new System.Drawing.Point(351, 11);
            this.colorButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.colorButton2.Name = "colorButton2";
            this.colorButton2.Size = new System.Drawing.Size(35, 35);
            this.colorButton2.TabIndex = 15;
            this.colorButton2.UseVisualStyleBackColor = false;
            this.colorButton2.Click += new System.EventHandler(this.colorButton2_Click);
            this.colorButton2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorButton2_MouseDown);
            // 
            // colorButton1
            // 
            this.colorButton1.BackColor = System.Drawing.Color.White;
            this.colorButton1.Image = global::VectorEditor.Properties.Resources.resource__9_;
            this.colorButton1.Location = new System.Drawing.Point(332, 5);
            this.colorButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(35, 35);
            this.colorButton1.TabIndex = 14;
            this.colorButton1.UseVisualStyleBackColor = false;
            this.colorButton1.Click += new System.EventHandler(this.colorButton1_Click);
            this.colorButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorButton1_MouseDown);
            // 
            // buttonPolygon
            // 
            this.buttonPolygon.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonPolygon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonPolygon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonPolygon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonPolygon.ForeColor = System.Drawing.Color.Brown;
            this.buttonPolygon.Image = global::VectorEditor.Properties.Resources.resource__2_;
            this.buttonPolygon.Location = new System.Drawing.Point(217, 3);
            this.buttonPolygon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPolygon.Name = "buttonPolygon";
            this.buttonPolygon.Size = new System.Drawing.Size(48, 42);
            this.buttonPolygon.TabIndex = 12;
            this.buttonPolygon.Tag = "Курсор";
            this.buttonPolygon.UseVisualStyleBackColor = false;
            this.buttonPolygon.Click += new System.EventHandler(this.buttonPolygon_Click);
            // 
            // buttonCircle
            // 
            this.buttonCircle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonCircle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCircle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCircle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCircle.ForeColor = System.Drawing.Color.Brown;
            this.buttonCircle.Image = global::VectorEditor.Properties.Resources.resource__1_;
            this.buttonCircle.Location = new System.Drawing.Point(164, 3);
            this.buttonCircle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(44, 42);
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
            this.buttonRectangle.Image = global::VectorEditor.Properties.Resources.resource__8_;
            this.buttonRectangle.Location = new System.Drawing.Point(111, 3);
            this.buttonRectangle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRectangle.Name = "buttonRectangle";
            this.buttonRectangle.Size = new System.Drawing.Size(43, 42);
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
            this.buttonLine.Image = global::VectorEditor.Properties.Resources.resource__3_;
            this.buttonLine.Location = new System.Drawing.Point(59, 3);
            this.buttonLine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLine.Name = "buttonLine";
            this.buttonLine.Size = new System.Drawing.Size(42, 42);
            this.buttonLine.TabIndex = 9;
            this.buttonLine.Tag = "Курсор";
            this.buttonLine.UseVisualStyleBackColor = false;
            this.buttonLine.Click += new System.EventHandler(this.buttonLine_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(931, 634);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "VectorEditor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.PasteMenu.ResumeLayout(false);
            this.CopyDeleteMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonCursor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonPolygon;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button buttonRectangle;
        private System.Windows.Forms.Button buttonLine;
        private System.Windows.Forms.Button colorButton1;
        private System.Windows.Forms.Button colorButton2;
        private System.Windows.Forms.TrackBar thicknessBar;
        private System.Windows.Forms.Label labelThickness;
        private System.Windows.Forms.ContextMenuStrip CopyDeleteMenu;
        private System.Windows.Forms.ToolStripMenuItem Copy;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ContextMenuStrip PasteMenu;
        private System.Windows.Forms.ToolStripMenuItem Paste;
    }
}

