﻿namespace Mapping
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonCoordinates = new System.Windows.Forms.Button();
            this.buttonReinit = new System.Windows.Forms.Button();
            this.pictureBoxRight = new System.Windows.Forms.PictureBox();
            this.pictureBoxBack = new System.Windows.Forms.PictureBox();
            this.pictureBoxTop = new System.Windows.Forms.PictureBox();
            this.labelTop = new System.Windows.Forms.Label();
            this.labelRight = new System.Windows.Forms.Label();
            this.labelBack = new System.Windows.Forms.Label();
            this.listBoxRight = new System.Windows.Forms.ListBox();
            this.listBoxBack = new System.Windows.Forms.ListBox();
            this.listBoxTop = new System.Windows.Forms.ListBox();
            this.buttonCoordBack = new System.Windows.Forms.Button();
            this.buttonCoordRight = new System.Windows.Forms.Button();
            this.buttonCoordTop = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.renderControl1 = new Mapping.RenderControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTop)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(561, 380);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Parcourir";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(419, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(340, 340);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(1147, 504);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(125, 134);
            this.listBox1.TabIndex = 3;
            this.listBox1.Visible = false;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(277, 380);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(75, 23);
            this.buttonPreview.TabIndex = 4;
            this.buttonPreview.Text = "Aperçu";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonCoordinates
            // 
            this.buttonCoordinates.Location = new System.Drawing.Point(1147, 644);
            this.buttonCoordinates.Name = "buttonCoordinates";
            this.buttonCoordinates.Size = new System.Drawing.Size(125, 23);
            this.buttonCoordinates.TabIndex = 5;
            this.buttonCoordinates.Text = "Coordonnées";
            this.buttonCoordinates.UseVisualStyleBackColor = true;
            this.buttonCoordinates.Visible = false;
            this.buttonCoordinates.Click += new System.EventHandler(this.buttonCoordinates_Click);
            // 
            // buttonReinit
            // 
            this.buttonReinit.Location = new System.Drawing.Point(12, 380);
            this.buttonReinit.Name = "buttonReinit";
            this.buttonReinit.Size = new System.Drawing.Size(75, 23);
            this.buttonReinit.TabIndex = 6;
            this.buttonReinit.Text = "Réinitialiser";
            this.buttonReinit.UseVisualStyleBackColor = true;
            this.buttonReinit.Click += new System.EventHandler(this.buttonReinit_Click);
            // 
            // pictureBoxRight
            // 
            this.pictureBoxRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxRight.Location = new System.Drawing.Point(224, 504);
            this.pictureBoxRight.Name = "pictureBoxRight";
            this.pictureBoxRight.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxRight.TabIndex = 7;
            this.pictureBoxRight.TabStop = false;
            this.pictureBoxRight.Visible = false;
            // 
            // pictureBoxBack
            // 
            this.pictureBoxBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxBack.Location = new System.Drawing.Point(430, 504);
            this.pictureBoxBack.Name = "pictureBoxBack";
            this.pictureBoxBack.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxBack.TabIndex = 8;
            this.pictureBoxBack.TabStop = false;
            this.pictureBoxBack.Visible = false;
            // 
            // pictureBoxTop
            // 
            this.pictureBoxTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxTop.Location = new System.Drawing.Point(18, 504);
            this.pictureBoxTop.Name = "pictureBoxTop";
            this.pictureBoxTop.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxTop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTop.TabIndex = 9;
            this.pictureBoxTop.TabStop = false;
            this.pictureBoxTop.Visible = false;
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.Location = new System.Drawing.Point(15, 488);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(26, 13);
            this.labelTop.TabIndex = 10;
            this.labelTop.Text = "Top";
            this.labelTop.Visible = false;
            // 
            // labelRight
            // 
            this.labelRight.AutoSize = true;
            this.labelRight.Location = new System.Drawing.Point(221, 488);
            this.labelRight.Name = "labelRight";
            this.labelRight.Size = new System.Drawing.Size(32, 13);
            this.labelRight.TabIndex = 11;
            this.labelRight.Text = "Right";
            this.labelRight.Visible = false;
            // 
            // labelBack
            // 
            this.labelBack.AutoSize = true;
            this.labelBack.Location = new System.Drawing.Point(427, 488);
            this.labelBack.Name = "labelBack";
            this.labelBack.Size = new System.Drawing.Size(32, 13);
            this.labelBack.TabIndex = 12;
            this.labelBack.Text = "Back";
            this.labelBack.Visible = false;
            // 
            // listBoxRight
            // 
            this.listBoxRight.FormattingEnabled = true;
            this.listBoxRight.Location = new System.Drawing.Point(832, 504);
            this.listBoxRight.Name = "listBoxRight";
            this.listBoxRight.Size = new System.Drawing.Size(120, 147);
            this.listBoxRight.TabIndex = 13;
            this.listBoxRight.Visible = false;
            this.listBoxRight.SelectedIndexChanged += new System.EventHandler(this.listBoxRight_SelectedIndexChanged);
            // 
            // listBoxBack
            // 
            this.listBoxBack.FormattingEnabled = true;
            this.listBoxBack.Location = new System.Drawing.Point(991, 504);
            this.listBoxBack.Name = "listBoxBack";
            this.listBoxBack.Size = new System.Drawing.Size(120, 147);
            this.listBoxBack.TabIndex = 14;
            this.listBoxBack.Visible = false;
            this.listBoxBack.SelectedIndexChanged += new System.EventHandler(this.listBoxBack_SelectedIndexChanged);
            // 
            // listBoxTop
            // 
            this.listBoxTop.FormattingEnabled = true;
            this.listBoxTop.Location = new System.Drawing.Point(659, 504);
            this.listBoxTop.Name = "listBoxTop";
            this.listBoxTop.Size = new System.Drawing.Size(120, 147);
            this.listBoxTop.TabIndex = 15;
            this.listBoxTop.Visible = false;
            this.listBoxTop.SelectedIndexChanged += new System.EventHandler(this.listBoxTop_SelectedIndexChanged);
            // 
            // buttonCoordBack
            // 
            this.buttonCoordBack.Location = new System.Drawing.Point(1009, 657);
            this.buttonCoordBack.Name = "buttonCoordBack";
            this.buttonCoordBack.Size = new System.Drawing.Size(75, 23);
            this.buttonCoordBack.TabIndex = 16;
            this.buttonCoordBack.Text = "CoordBack";
            this.buttonCoordBack.UseVisualStyleBackColor = true;
            this.buttonCoordBack.Visible = false;
            this.buttonCoordBack.Click += new System.EventHandler(this.buttonCoordBack_Click);
            // 
            // buttonCoordRight
            // 
            this.buttonCoordRight.Location = new System.Drawing.Point(855, 657);
            this.buttonCoordRight.Name = "buttonCoordRight";
            this.buttonCoordRight.Size = new System.Drawing.Size(75, 23);
            this.buttonCoordRight.TabIndex = 17;
            this.buttonCoordRight.Text = "CoordRight";
            this.buttonCoordRight.UseVisualStyleBackColor = true;
            this.buttonCoordRight.Visible = false;
            this.buttonCoordRight.Click += new System.EventHandler(this.buttonCoordRight_Click);
            // 
            // buttonCoordTop
            // 
            this.buttonCoordTop.Location = new System.Drawing.Point(684, 657);
            this.buttonCoordTop.Name = "buttonCoordTop";
            this.buttonCoordTop.Size = new System.Drawing.Size(75, 23);
            this.buttonCoordTop.TabIndex = 18;
            this.buttonCoordTop.Text = "CoordTop";
            this.buttonCoordTop.UseVisualStyleBackColor = true;
            this.buttonCoordTop.Visible = false;
            this.buttonCoordTop.Click += new System.EventHandler(this.buttonCoordTop_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(143, 383);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(75, 23);
            this.buttonPrint.TabIndex = 19;
            this.buttonPrint.Text = "Imprimer";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // renderControl1
            // 
            this.renderControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.renderControl1.Location = new System.Drawing.Point(12, 34);
            this.renderControl1.Name = "renderControl1";
            this.renderControl1.Size = new System.Drawing.Size(340, 340);
            this.renderControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 438);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonCoordTop);
            this.Controls.Add(this.buttonCoordRight);
            this.Controls.Add(this.buttonCoordBack);
            this.Controls.Add(this.listBoxTop);
            this.Controls.Add(this.listBoxBack);
            this.Controls.Add(this.listBoxRight);
            this.Controls.Add(this.labelBack);
            this.Controls.Add(this.labelRight);
            this.Controls.Add(this.labelTop);
            this.Controls.Add(this.pictureBoxTop);
            this.Controls.Add(this.pictureBoxBack);
            this.Controls.Add(this.pictureBoxRight);
            this.Controls.Add(this.buttonReinit);
            this.Controls.Add(this.buttonCoordinates);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.renderControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RenderControl renderControl1;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCoordinates;
        private System.Windows.Forms.Button buttonReinit;
        private System.Windows.Forms.PictureBox pictureBoxRight;
        private System.Windows.Forms.PictureBox pictureBoxBack;
        private System.Windows.Forms.PictureBox pictureBoxTop;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.Label labelRight;
        private System.Windows.Forms.Label labelBack;
        private System.Windows.Forms.ListBox listBoxRight;
        private System.Windows.Forms.ListBox listBoxBack;
        private System.Windows.Forms.ListBox listBoxTop;
        private System.Windows.Forms.Button buttonCoordBack;
        private System.Windows.Forms.Button buttonCoordRight;
        private System.Windows.Forms.Button buttonCoordTop;
        private System.Windows.Forms.Button buttonPrint;
    }
}

