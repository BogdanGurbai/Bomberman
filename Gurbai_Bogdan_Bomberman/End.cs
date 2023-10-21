using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gurbai_Bogdan_Bomberman
{
    public partial class End : Form
    {
        public End(int scor)
        {
            InitializeComponent();
            lbl_score.Text = scor.ToString();
            lbl_highscore.Text = Properties.Settings.Default.h_score;
            int a = Int32.Parse(lbl_highscore.Text);
            if(scor>a)
            {
                lbl_highscore.Text = scor.ToString();
                Properties.Settings.Default.h_score = lbl_highscore.Text;
                Properties.Settings.Default.Save();
                pct_highscore.Visible = true;
            }
        }

        private void btn_iesire_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }
    }
}
