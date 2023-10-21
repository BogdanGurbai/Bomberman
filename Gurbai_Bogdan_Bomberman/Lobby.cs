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
    public partial class Lobby : Form
    {
        //reduce lag
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public Lobby(int d, int b, int lvl, int vit, int scr, int vieti)
        {
            InitializeComponent();
            distanta = d;
            nr_bombe = b;
            nivel = lvl;
            speed = vit;
            scor = scr;
            lives = vieti;
            lbl_score.Text = scor.ToString();
            lbl_vieti.Text = lives.ToString();
            gameover.Visible = false;

            if (speed == 0) timer_player.Interval = 25;
            if (speed == 1) timer_player.Interval = 16;
        }

        //general
        int viteza = 5, scor = 0;
        //player
        int mijTop, mijLeft;
        int distanta, nr_bombe, lives,speed; //power-ups
        int t = 0, playerT = 0; //timere // bombT; 
        bool sus = false, jos = false, stanga = false, dreapta = false; //directii player
        bool intrat = false;

        //poweups
        int nivel;

        //verificare directii

        bool verificare_sus(PictureBox player)
        {
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "zid")
                    if (player.Top - viteza <= x.Top + x.Height && player.Top > x.Top + x.Height)
                        if ((player.Left + player.Width >= x.Left) && (player.Left <= x.Left + x.Width))
                            return false;
            }
            foreach (Control x in this.Controls)
            {
                if (((string)x.Tag == "zid_destructibil" || (string)x.Tag == "zid_distrus") && x.Visible == true)
                    if (player.Top - viteza <= x.Top + x.Height && player.Top > x.Top + x.Height)
                        if ((player.Left + player.Width >= x.Left) && (player.Left <= x.Left + x.Width))
                            return false;
            }
            return true;
        }

        bool verificare_jos(PictureBox player)
        {
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "zid")
                    if (player.Top + player.Height + viteza >= x.Top && player.Top < x.Top)
                        if ((player.Left + player.Width >= x.Left) && (player.Left <= x.Left + x.Width))
                            return false;
            }
            foreach (Control x in this.Controls)
            {
                if (((string)x.Tag == "zid_destructibil" || (string)x.Tag == "zid_distrus") && x.Visible == true)
                    if (player.Top + player.Height + viteza >= x.Top && player.Top < x.Top)
                        if ((player.Left + player.Width >= x.Left) && (player.Left <= x.Left + x.Width))
                            return false;
            }
            return true;
        }

        bool verificare_dreapta(PictureBox player)
        {
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "zid")
                    if (player.Left + player.Width + viteza >= x.Left && player.Left < x.Left)
                        if ((player.Top + player.Height >= x.Top) && (player.Top <= x.Top + x.Height))
                            return false;
            }
            foreach (Control x in this.Controls)
            {
                if (((string)x.Tag == "zid_destructibil" || (string)x.Tag == "zid_distrus") && x.Visible == true)
                    if (player.Left + player.Width + viteza >= x.Left && player.Left < x.Left)
                        if ((player.Top + player.Height >= x.Top) && (player.Top <= x.Top + x.Height - 23))
                            return false;
            }
            return true;
        }

        bool verificare_stanga(PictureBox player)
        {
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "zid")
                    if (player.Left - viteza <= x.Left + x.Width && player.Left > x.Left + x.Width)
                        if ((player.Top + player.Height >= x.Top) && (player.Top <= x.Top + x.Height))
                            return false;
            }
            foreach (Control x in this.Controls)
            {
                if (((string)x.Tag == "zid_destructibil" || (string)x.Tag == "zid_distrus") && x.Visible == true)
                    if (player.Left - viteza <= x.Left + x.Width && player.Left > x.Left + x.Width)
                        if ((player.Top + player.Height >= x.Top) && (player.Top <= x.Top + x.Height - 23))
                            return false;
            }
            return true;
        }

        //apasarea tastelor

        private void Lobby_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) sus = true;
            if (e.KeyCode == Keys.S) jos = true;
            if (e.KeyCode == Keys.D) dreapta = true;
            if (e.KeyCode == Keys.A) stanga = true;
            if (e.KeyCode == Keys.Escape)
            {
                (new Form1()).Show(); this.Close();
            }
        }

        private void Lobby_KeyUp(object sender, KeyEventArgs e)
        {
            if (!intrat)
            {
                if (e.KeyCode == Keys.W) { player.Image = Properties.Resources.playeridleup; sus = false; }
                if (e.KeyCode == Keys.S) { player.Image = Properties.Resources.playeridledown; jos = false; }
                if (e.KeyCode == Keys.D) { player.Image = Properties.Resources.playeridleright; dreapta = false; }
                if (e.KeyCode == Keys.A) { player.Image = Properties.Resources.playeridleleft; stanga = false; }
            }
        }


        private void timer_player_Tick(object sender, EventArgs e)
        {
            if (sus == true)
            {
                player.Width = 40;
                if (t % 3 == 0 && t % 6 != 0)
                    player.Image = Properties.Resources.playerup1;
                if (t % 6 == 0)
                    player.Image = Properties.Resources.playerup2;
                if (verificare_sus(player) == true && player.Top > 123)
                    player.Top = player.Top - viteza;
            }
            if (jos == true)
            {
                player.Width = 40;
                if (t % 3 == 0 && t % 6 != 0)
                    player.Image = Properties.Resources.playerdown1;
                if (t % 6 == 0)
                    player.Image = Properties.Resources.playerdown2;
                if (verificare_jos(player) == true && player.Top + player.Height < 670)
                    player.Top = player.Top + viteza;
            }
            if (dreapta == true)
            {
                player.Width = 40;
                if (t % 3 == 0 && t % 6 != 0)
                    player.Image = Properties.Resources.playerright1;
                if (t % 6 == 0)
                    player.Image = Properties.Resources.playerright2;
                if (verificare_dreapta(player) == true && player.Left + player.Width < 727)
                    player.Left = player.Left + viteza;
            }
            if (stanga == true)
            {
                player.Width = 40;
                if (t % 3 == 0 && t % 6 != 0)
                    player.Image = Properties.Resources.playerleft1;
                if (t % 6 == 0)
                    player.Image = Properties.Resources.playerleft2;
                if (verificare_stanga(player) == true && player.Left > 75)
                    player.Left = player.Left - viteza;
            }
            playerT = t;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            //numararea tick-urilor

            t++;

            //updatarea coordonatelor playerului

            mijTop = player.Top + player.Height / 2;
            mijLeft = player.Left + player.Width / 2;

            //verificarea nivelului urmator
            if (lives > 0)
            {
                if (nivel == 1 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta1.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta1.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta1.Top && mijTop <= poarta1.Top + poarta1.Height && mijLeft >= poarta1.Left && mijLeft <= poarta1.Left + poarta1.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta1.Top + 5;
                        player.Left = poarta1.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl1 f = new lvl1(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
                if (nivel == 2 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta2.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta2.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta2.Top && mijTop <= poarta2.Top + poarta2.Height && mijLeft >= poarta2.Left && mijLeft <= poarta2.Left + poarta2.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta2.Top + 5;
                        player.Left = poarta2.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl2 f = new lvl2(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
                if (nivel == 3 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta3.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta3.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta3.Top && mijTop <= poarta3.Top + poarta3.Height && mijLeft >= poarta3.Left && mijLeft <= poarta3.Left + poarta3.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta3.Top + 5;
                        player.Left = poarta3.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl3 f = new lvl3(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
                if (nivel == 4 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta4.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta4.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta4.Top && mijTop <= poarta4.Top + poarta4.Height && mijLeft >= poarta4.Left && mijLeft <= poarta4.Left + poarta4.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta4.Top + 5;
                        player.Left = poarta4.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl4 f = new lvl4(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
                if (nivel == 5 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta5.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta5.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta5.Top && mijTop <= poarta5.Top + poarta5.Height && mijLeft >= poarta5.Left && mijLeft <= poarta5.Left + poarta5.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta5.Top + 5;
                        player.Left = poarta5.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl5 f = new lvl5(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
                if (nivel == 6 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta6.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta6.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta6.Top && mijTop <= poarta6.Top + poarta6.Height && mijLeft >= poarta6.Left && mijLeft <= poarta6.Left + poarta6.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta6.Top + 5;
                        player.Left = poarta6.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl6 f = new lvl6(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
                if (nivel == 7 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta7.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta7.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta7.Top && mijTop <= poarta7.Top + poarta7.Height && mijLeft >= poarta7.Left && mijLeft <= poarta7.Left + poarta7.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta7.Top + 5;
                        player.Left = poarta7.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl7 f = new lvl7(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
                if (nivel == 8 || nivel >= 9)
                {
                    if (t % 20 == 0) { poarta8.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { poarta8.Image = Properties.Resources.poarta2; }
                    if (mijTop >= poarta8.Top && mijTop <= poarta8.Top + poarta8.Height && mijLeft >= poarta8.Left && mijLeft <= poarta8.Left + poarta8.Width)
                    {
                        intrat = true;
                        timer_player.Stop();
                        player.Top = poarta8.Top + 5;
                        player.Left = poarta8.Left + 5;
                        player.BringToFront();
                        if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                        if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                        if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                        if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                        if (playerT == t - 100)
                        {
                            lvl8 f = new lvl8(distanta, nr_bombe, nivel, speed, scor, lives);
                            f.Show();
                            this.Hide();
                        }
                    }
                }
            }
            else gameover.Visible = true;
        }
    }
}
