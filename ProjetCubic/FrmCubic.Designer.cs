namespace ProjetCubic
{
    partial class FrmCubic
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
            this.opfParcourirChanson = new System.Windows.Forms.OpenFileDialog();
            this.btnParcourir = new System.Windows.Forms.Button();
            this.lblPathChanson = new System.Windows.Forms.Label();
            this.lblTempschanson = new System.Windows.Forms.Label();
            this.ssBarreStatut = new System.Windows.Forms.StatusStrip();
            this.tslblStatut = new System.Windows.Forms.ToolStripStatusLabel();
            this.opfParcourirDossierChanson = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.ssBarreStatut.SuspendLayout();
            this.SuspendLayout();
            // 
            // opfParcourirChanson
            // 
            this.opfParcourirChanson.DefaultExt = "osu";
            this.opfParcourirChanson.FileName = "fichier osu";
            this.opfParcourirChanson.Filter = "FICHIER OSU|*.osu";
            this.opfParcourirChanson.Title = "Sélectionner le fichier de chanson";
            // 
            // btnParcourir
            // 
            this.btnParcourir.Location = new System.Drawing.Point(401, 60);
            this.btnParcourir.Name = "btnParcourir";
            this.btnParcourir.Size = new System.Drawing.Size(75, 23);
            this.btnParcourir.TabIndex = 0;
            this.btnParcourir.Text = "Parcourir...";
            this.btnParcourir.UseVisualStyleBackColor = true;
            this.btnParcourir.Click += new System.EventHandler(this.btnParcourir_Click);
            // 
            // lblPathChanson
            // 
            this.lblPathChanson.AutoSize = true;
            this.lblPathChanson.Location = new System.Drawing.Point(26, 33);
            this.lblPathChanson.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblPathChanson.Name = "lblPathChanson";
            this.lblPathChanson.Size = new System.Drawing.Size(0, 13);
            this.lblPathChanson.TabIndex = 1;
            // 
            // lblTempschanson
            // 
            this.lblTempschanson.AutoSize = true;
            this.lblTempschanson.Location = new System.Drawing.Point(26, 101);
            this.lblTempschanson.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblTempschanson.Name = "lblTempschanson";
            this.lblTempschanson.Size = new System.Drawing.Size(0, 13);
            this.lblTempschanson.TabIndex = 2;
            // 
            // ssBarreStatut
            // 
            this.ssBarreStatut.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblStatut});
            this.ssBarreStatut.Location = new System.Drawing.Point(0, 101);
            this.ssBarreStatut.Name = "ssBarreStatut";
            this.ssBarreStatut.Size = new System.Drawing.Size(518, 22);
            this.ssBarreStatut.TabIndex = 3;
            // 
            // tslblStatut
            // 
            this.tslblStatut.Name = "tslblStatut";
            this.tslblStatut.Size = new System.Drawing.Size(52, 17);
            this.tslblStatut.Text = "Bonjour,";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(401, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Parcourir...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmCubic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 123);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ssBarreStatut);
            this.Controls.Add(this.lblTempschanson);
            this.Controls.Add(this.lblPathChanson);
            this.Controls.Add(this.btnParcourir);
            this.Name = "FrmCubic";
            this.ShowIcon = false;
            this.Text = "Cubic Project";
            this.ssBarreStatut.ResumeLayout(false);
            this.ssBarreStatut.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog opfParcourirChanson;
        private System.Windows.Forms.Button btnParcourir;
        private System.Windows.Forms.Label lblPathChanson;
        private System.Windows.Forms.Label lblTempschanson;
        private System.Windows.Forms.StatusStrip ssBarreStatut;
        private System.Windows.Forms.ToolStripStatusLabel tslblStatut;
        private System.Windows.Forms.FolderBrowserDialog opfParcourirDossierChanson;
        private System.Windows.Forms.Button button1;
    }
}

