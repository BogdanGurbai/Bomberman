namespace Gurbai_Bogdan_Bomberman
{
    partial class End
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_score = new System.Windows.Forms.Label();
            this.lbl_highscore = new System.Windows.Forms.Label();
            this.pct_highscore = new System.Windows.Forms.PictureBox();
            this.btn_iesire = new System.Windows.Forms.Button();
            this.Restart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pct_highscore)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_score
            // 
            this.lbl_score.BackColor = System.Drawing.Color.Transparent;
            this.lbl_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_score.ForeColor = System.Drawing.Color.White;
            this.lbl_score.Location = new System.Drawing.Point(104, 21);
            this.lbl_score.Name = "lbl_score";
            this.lbl_score.Size = new System.Drawing.Size(168, 29);
            this.lbl_score.TabIndex = 211;
            this.lbl_score.Text = "0";
            // 
            // lbl_highscore
            // 
            this.lbl_highscore.BackColor = System.Drawing.Color.Transparent;
            this.lbl_highscore.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_highscore.ForeColor = System.Drawing.Color.White;
            this.lbl_highscore.Location = new System.Drawing.Point(605, 21);
            this.lbl_highscore.Name = "lbl_highscore";
            this.lbl_highscore.Size = new System.Drawing.Size(168, 29);
            this.lbl_highscore.TabIndex = 212;
            this.lbl_highscore.Text = "0";
            // 
            // pct_highscore
            // 
            this.pct_highscore.BackColor = System.Drawing.Color.Transparent;
            this.pct_highscore.Image = global::Gurbai_Bogdan_Bomberman.Properties.Resources.HIGHSCORE;
            this.pct_highscore.Location = new System.Drawing.Point(70, 93);
            this.pct_highscore.Name = "pct_highscore";
            this.pct_highscore.Size = new System.Drawing.Size(659, 235);
            this.pct_highscore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pct_highscore.TabIndex = 213;
            this.pct_highscore.TabStop = false;
            this.pct_highscore.Visible = false;
            // 
            // btn_iesire
            // 
            this.btn_iesire.BackColor = System.Drawing.Color.Black;
            this.btn_iesire.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_iesire.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_iesire.ForeColor = System.Drawing.Color.White;
            this.btn_iesire.Location = new System.Drawing.Point(756, 677);
            this.btn_iesire.Margin = new System.Windows.Forms.Padding(0);
            this.btn_iesire.Name = "btn_iesire";
            this.btn_iesire.Size = new System.Drawing.Size(42, 42);
            this.btn_iesire.TabIndex = 214;
            this.btn_iesire.Text = "X";
            this.btn_iesire.UseVisualStyleBackColor = false;
            this.btn_iesire.Click += new System.EventHandler(this.btn_iesire_Click);
            // 
            // Restart
            // 
            this.Restart.BackColor = System.Drawing.Color.Black;
            this.Restart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Restart.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Restart.ForeColor = System.Drawing.Color.White;
            this.Restart.Location = new System.Drawing.Point(593, 677);
            this.Restart.Margin = new System.Windows.Forms.Padding(0);
            this.Restart.Name = "Restart";
            this.Restart.Size = new System.Drawing.Size(163, 42);
            this.Restart.TabIndex = 215;
            this.Restart.Text = "Main Menu";
            this.Restart.UseVisualStyleBackColor = false;
            this.Restart.Click += new System.EventHandler(this.Restart_Click);
            // 
            // End
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Gurbai_Bogdan_Bomberman.Properties.Resources.fundal_sfarsit;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(802, 723);
            this.Controls.Add(this.Restart);
            this.Controls.Add(this.btn_iesire);
            this.Controls.Add(this.pct_highscore);
            this.Controls.Add(this.lbl_highscore);
            this.Controls.Add(this.lbl_score);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "End";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "End";
            ((System.ComponentModel.ISupportInitialize)(this.pct_highscore)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_score;
        private System.Windows.Forms.Label lbl_highscore;
        private System.Windows.Forms.PictureBox pct_highscore;
        private System.Windows.Forms.Button btn_iesire;
        private System.Windows.Forms.Button Restart;
    }
}