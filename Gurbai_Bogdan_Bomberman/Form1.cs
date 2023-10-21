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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lbl_highscore.Text = Properties.Settings.Default.h_score;
        }

        private void btn_campaign_Click(object sender, EventArgs e)
        {
            Lobby f = new Lobby(1,1,1,0,0,3);
            f.Show();
            this.Hide();
        }

        private void btn_training_Click(object sender, EventArgs e)
        {
            Lobby f = new Lobby(3,3,9,1,0,9);
            f.Show();
            this.Hide();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void btn_iesire_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
