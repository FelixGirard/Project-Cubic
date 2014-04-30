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
            this.SuspendLayout();
            // 
            // opfParcourirChanson
            // 
            this.opfParcourirChanson.FileName = "fichier osu";
            // 
            // btnParcourir
            // 
            this.btnParcourir.Location = new System.Drawing.Point(162, 87);
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
            this.lblPathChanson.Location = new System.Drawing.Point(35, 92);
            this.lblPathChanson.Name = "lblPathChanson";
            this.lblPathChanson.Size = new System.Drawing.Size(0, 13);
            this.lblPathChanson.TabIndex = 1;
            // 
            // FrmCubic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lblPathChanson);
            this.Controls.Add(this.btnParcourir);
            this.Name = "FrmCubic";
            this.ShowIcon = false;
            this.Text = "Cubic Project";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog opfParcourirChanson;
        private System.Windows.Forms.Button btnParcourir;
        private System.Windows.Forms.Label lblPathChanson;
    }
}

