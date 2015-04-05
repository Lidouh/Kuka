namespace Mapping
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
            this.renderControl1 = new Mapping.RenderControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(610, 457);
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
            this.pictureBox1.Location = new System.Drawing.Point(459, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(406, 397);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(892, 44);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(125, 394);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(234, 457);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(75, 23);
            this.buttonPreview.TabIndex = 4;
            this.buttonPreview.Text = "Aperçu";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonCoordinates
            // 
            this.buttonCoordinates.Location = new System.Drawing.Point(892, 457);
            this.buttonCoordinates.Name = "buttonCoordinates";
            this.buttonCoordinates.Size = new System.Drawing.Size(125, 23);
            this.buttonCoordinates.TabIndex = 5;
            this.buttonCoordinates.Text = "Coordonnées";
            this.buttonCoordinates.UseVisualStyleBackColor = true;
            this.buttonCoordinates.Click += new System.EventHandler(this.buttonCoordinates_Click);
            // 
            // buttonReinit
            // 
            this.buttonReinit.Location = new System.Drawing.Point(87, 457);
            this.buttonReinit.Name = "buttonReinit";
            this.buttonReinit.Size = new System.Drawing.Size(75, 23);
            this.buttonReinit.TabIndex = 6;
            this.buttonReinit.Text = "Réinitialiser";
            this.buttonReinit.UseVisualStyleBackColor = true;
            this.buttonReinit.Click += new System.EventHandler(this.buttonReinit_Click);
            // 
            // renderControl1
            // 
            this.renderControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.renderControl1.Location = new System.Drawing.Point(12, 44);
            this.renderControl1.Name = "renderControl1";
            this.renderControl1.Size = new System.Drawing.Size(408, 397);
            this.renderControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 522);
            this.Controls.Add(this.buttonReinit);
            this.Controls.Add(this.buttonCoordinates);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.renderControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RenderControl renderControl1;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCoordinates;
        private System.Windows.Forms.Button buttonReinit;
    }
}

