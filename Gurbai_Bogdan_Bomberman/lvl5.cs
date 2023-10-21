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
    public partial class lvl5 : Form
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

        public lvl5(int d, int nrb, int lvl, int vit, int scr, int vieti)
        {
            InitializeComponent();
            distanta = d;
            nr_bombe = nrb;
            level = lvl;
            speed = vit;
            scor = scr;
            lives = vieti;
            distanta_i = d;
            nr_bombe_i = nrb;
            scor_i = scr;
            speed_i = vit;
            lives_i = vieti - 1;
            MakeOrange();
            MakePurple();
            MakeBlue();
            MakeBrown();
            MakeLevel();
            MakeBombs();
            if (speed == 0) timer_player.Interval = 25;
            if (speed == 1) timer_player.Interval = 16;

            //afisare vieti

            lbl_vieti.Text = lives.ToString();
        }

        //general
        int viteza = 5;
        //timp si scor
        int s = 30, m = 3;
        //player
        int distanta, nr_bombe, count_bombe = 0, scor, level, speed, lives; //power-ups
        int distanta_i, nr_bombe_i, scor_i = 0, lives_i, speed_i; //power-ups
        int mijTop, mijLeft; //coordonate
        int t = 0, playerT = 0; //timere // bombT; 
        bool sus = false, jos = false, stanga = false, dreapta = false; //directii player
        bool bomba = false; //verificarea plasarea bombei
        bool player_dead = false, player_finsih = false; //verifcare daca playerul este mort
        //bombe
        PictureBox[,] explozii = new PictureBox[4, 18]; //imaginile bombelor
        bool[] folosire_bombe = new bool[4]; //verificare daca in folosinta
        int[] timer_bombe = new int[4]; //timerul
        bool[,] directii = new bool[4, 4]; //verficare directii, sus, jos, dreapta, stanga
        //inamic orange
        PictureBox[] inamici_orange = new PictureBox[5];//imaginile
        int[] directii_orange = new int[5]; //directiile 1sus 2jos 3dreapta 4stanga 
        int[] timere_orange = new int[5]; //timer
        bool[] dead_orange = new bool[5];
        int nr_orange = 5;
        //inamic purple
        PictureBox[] inamici_purple = new PictureBox[5];//imaginile
        int[] directii_purple = new int[5]; //directiile 1sus 2jos 3dreapta 4stanga 
        int[] timere_purple = new int[5]; //timer
        bool[] dead_purple = new bool[5];
        int nr_purple = 3;
        //inamic blue
        PictureBox[] inamici_blue = new PictureBox[5];//imaginile
        int[] directii_blue = new int[5]; //directiile 1sus 2jos 3dreapta 4stanga 
        int[] timere_blue = new int[5]; //timer
        bool[] dead_blue = new bool[5];
        int nr_blue = 3;
        //inamic brown
        PictureBox[] inamici_brown = new PictureBox[5];//imaginile
        int[] directii_brown = new int[5]; //directiile 1sus 2jos 3dreapta 4stanga 
        int[] timere_brown = new int[5]; //timer
        bool[] dead_brown = new bool[5];
        int nr_brown = 2;
        //pereti
        PictureBox[] pereti = new PictureBox[100];
        int contor_pereti = 0;
        //poarta
        bool poarta = false;
        bool poarta_dead = false;
        //powerup
        int powerupx, powerupy;
        bool pwu = false;
        bool pwu_dead = false;

        //harta
        int nr_pereti = 85;

        public int[,] harta = new int[11, 29] { {1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,1,0,1,0,0,0,0},
                                               {1,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
                                               {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                               {0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
                                               {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                               {0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
                                               {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                               {0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
                                               {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                               {0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0},
                                               {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
                                            };

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

        //verificare poarta()
        bool verificare_poarta()
        {
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "inamic")
                    if (x.Visible == true)
                        return false;
            }
            return true;
        }

        //crearea niveluli

        void MakeLevel()
        {
            //notarea inamicilor
            for (int i = 0; i < nr_orange; i++)
                harta[(inamici_orange[i].Top - 5 - 122) / 50, (inamici_orange[i].Left - 5 - 74) / 50] = 1;
            for (int i = 0; i < nr_purple; i++)
                harta[(inamici_purple[i].Top - 5 - 122) / 50, (inamici_purple[i].Left - 5 - 74) / 50] = 1;
            for (int i = 0; i < nr_blue; i++)
                harta[(inamici_blue[i].Top - 5 - 122) / 50, (inamici_blue[i].Left - 5 - 74) / 50] = 1;
            for (int i = 0; i < nr_brown; i++)
                harta[(inamici_brown[i].Top - 5 - 122) / 50, (inamici_brown[i].Left - 5 - 74) / 50] = 1;
            int count_pereti = 0;
            //crearea matricei
            Random r = new Random();
            do
            {
                count_pereti++;
                int x, y;
                do
                {
                    x = r.Next(0, 11);
                    y = r.Next(0, 29);
                } while (harta[x, y] != 0);
                harta[x, y] = 2;
            } while (count_pereti <= nr_pereti);
            //crearea peretilor
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 29; j++)
                {
                    if (harta[i, j] == 2)
                    {
                        pereti[contor_pereti] = new PictureBox();
                        pereti[contor_pereti].Height = 50;
                        pereti[contor_pereti].Width = 50;
                        pereti[contor_pereti].Image = Properties.Resources.block;
                        pereti[contor_pereti].SizeMode = PictureBoxSizeMode.StretchImage;
                        pereti[contor_pereti].Tag = "zid_destructibil";
                        pereti[contor_pereti].BackColor = Color.Transparent;
                        pereti[contor_pereti].BringToFront();
                        pereti[contor_pereti].Visible = true;
                        this.Controls.Add(pereti[contor_pereti]);
                        pereti[contor_pereti].Top = i * 50 + 122;
                        pereti[contor_pereti].Left = j * 50 + 74;
                        contor_pereti++;
                    }
                }
            }
            //crearea portii
            int poartax, poartay;
            do
            {
                poartax = r.Next(0, 11);
                poartay = r.Next(0, 29);
            } while (harta[poartax, poartay] != 2);
            exit.Top = poartax * 50 + 122;
            exit.Left = poartay * 50 + 74;
            harta[poartax, poartay] = 3;
            exit.SendToBack();
            //crearea powerup-ului
            do
            {
                powerupx = r.Next(0, 11);
                powerupy = r.Next(0, 29);
            } while (harta[powerupx, powerupy] != 2);
            powerup.Top = powerupx * 50 + 122;
            powerup.Left = powerupy * 50 + 74;
            harta[powerupx, powerupy] = 4;
            powerup.SendToBack();
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 29; j++)
                    Console.Write(harta[i, j] + " ");
                Console.WriteLine(" ");
            }
        }

        //crearea inamic orange

        void MakeOrange()
        {
            //initiere
            for (int i = 0; i < nr_orange; i++) { inamici_orange[i] = new PictureBox(); }
            //alocarea atributiilor
            Random r = new Random();
            for (int i = 0; i < nr_orange; i++)
            {
                inamici_orange[i].Height = 40;
                inamici_orange[i].Width = 40;
                inamici_orange[i].Tag = "inamic";
                inamici_orange[i].Image = Properties.Resources.orange1;
                inamici_orange[i].SizeMode = PictureBoxSizeMode.StretchImage;
                inamici_orange[i].BackColor = Color.Transparent;
                inamici_orange[i].BringToFront();
                inamici_orange[i].Visible = true;
                this.Controls.Add(inamici_orange[i]);
                //timer
                timere_orange[i] = 0;
                //initializarea directiei
                directii_orange[i] = r.Next(1, 4);
                //verificare mort
                dead_orange[i] = false;
            }
            inamici_orange[0].Top = 422 + 5;
            inamici_orange[0].Left = 224 + 5;
            inamici_orange[1].Top = 572 + 5;
            inamici_orange[1].Left = 474 + 5;
            inamici_orange[2].Top = 522 + 5;
            inamici_orange[2].Left = 1074 + 5;
            inamici_orange[3].Top = 272 + 5;
            inamici_orange[3].Left = 974 + 5;
            inamici_orange[4].Top = 422 + 5;
            inamici_orange[4].Left = 1374 + 5;
        }

        //crearea inamic purple

        void MakePurple()
        {
            //initiere
            for (int i = 0; i < nr_purple; i++) { inamici_purple[i] = new PictureBox(); }
            //alocarea atributiilor
            Random r = new Random();
            for (int i = 0; i < nr_purple; i++)
            {
                inamici_purple[i].Height = 40;
                inamici_purple[i].Width = 40;
                inamici_purple[i].Tag = "inamic";
                inamici_purple[i].Image = Properties.Resources.orange1;
                inamici_purple[i].SizeMode = PictureBoxSizeMode.StretchImage;
                inamici_purple[i].BackColor = Color.Transparent;
                inamici_purple[i].BringToFront();
                inamici_purple[i].Visible = true;
                this.Controls.Add(inamici_purple[i]);
                //timer
                timere_purple[i] = 0;
                //initializarea directiei
                directii_purple[i] = r.Next(1, 4);
                //verificare mort
                dead_purple[i] = false;
            }
            inamici_purple[0].Top = 222 + 5;
            inamici_purple[0].Left = 174 + 5;
            inamici_purple[1].Top = 322 + 5;
            inamici_purple[1].Left = 974 + 5;
            inamici_purple[2].Top = 272 + 5;
            inamici_purple[2].Left = 1174 + 5;
        }

        //crearea inamic blue

        void MakeBlue()
        {
            //initiere
            for (int i = 0; i < nr_blue; i++) { inamici_blue[i] = new PictureBox(); }
            //alocarea atributiilor
            Random r = new Random();
            for (int i = 0; i < nr_blue; i++)
            {
                inamici_blue[i].Height = 40;
                inamici_blue[i].Width = 40;
                inamici_blue[i].Tag = "inamic";
                inamici_blue[i].Image = Properties.Resources.blue1;
                inamici_blue[i].SizeMode = PictureBoxSizeMode.StretchImage;
                inamici_blue[i].BackColor = Color.Transparent;
                inamici_blue[i].BringToFront();
                inamici_blue[i].Visible = true;
                this.Controls.Add(inamici_blue[i]);
                //timer
                timere_blue[i] = 0;
                //initializarea directiei
                directii_blue[i] = r.Next(1, 4);
                //verificare mort
                dead_blue[i] = false;
            }
            inamici_blue[0].Top = 222 + 5;
            inamici_blue[0].Left = 574 + 5;
            inamici_blue[1].Top = 472 + 5;
            inamici_blue[1].Left = 1174 + 5;
            inamici_blue[2].Top = 372 + 5;
            inamici_blue[2].Left = 474 + 5;
        }

        //crearea inamic brown

        void MakeBrown()
        {
            //initiere
            for (int i = 0; i < nr_brown; i++) { inamici_brown[i] = new PictureBox(); }
            //alocarea atributiilor
            Random r = new Random();
            for (int i = 0; i < nr_brown; i++)
            {
                inamici_brown[i].Height = 40;
                inamici_brown[i].Width = 40;
                inamici_brown[i].Tag = "inamic";
                inamici_brown[i].Image = Properties.Resources.brown1;
                inamici_brown[i].SizeMode = PictureBoxSizeMode.StretchImage;
                inamici_brown[i].BackColor = Color.Transparent;
                inamici_brown[i].BringToFront();
                inamici_brown[i].Visible = true;
                this.Controls.Add(inamici_brown[i]);
                //timer
                timere_brown[i] = 0;
                //initializarea directiei
                directii_brown[i] = r.Next(1, 4);
                //verificare mort
                dead_brown[i] = false;
            }
            inamici_brown[0].Top = 472 + 5;
            inamici_brown[0].Left = 374 + 5;
            inamici_brown[1].Top = 172 + 5;
            inamici_brown[1].Left = 1374 + 5;
        }

        //crearea bombelor

        void MakeBombs()
        {
            for (int i = 0; i < 4; i++)
            {
                //initierea
                for (int j = 0; j < 18; j++) { explozii[i, j] = new PictureBox(); }
                folosire_bombe[i] = new bool();
                timer_bombe[i] = new int();
                for (int j = 0; j < 4; j++) { directii[i, j] = new bool(); }
                //alocarea imaginilor
                explozii[i, 0].Image = Properties.Resources.bomb1;
                explozii[i, 1].Image = Properties.Resources.explozie;
                for (int j = 2; j < 6; j++) { explozii[i, j].Image = Properties.Resources.explozieInaltime; }
                for (int j = 6; j < 10; j++) { explozii[i, j].Image = Properties.Resources.explozieInaltime; }
                for (int j = 10; j < 14; j++) { explozii[i, j].Image = Properties.Resources.explosieLatime; }
                for (int j = 14; j < 18; j++) { explozii[i, j].Image = Properties.Resources.explosieLatime; }
                //alocarea atributiilor
                for (int j = 0; j < 18; j++)
                {
                    explozii[i, j].Visible = false;
                    explozii[i, j].BackColor = Color.Transparent;
                    explozii[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    explozii[i, j].Tag = "explozie";
                    explozii[i, j].Height = 50;
                    explozii[i, j].Width = 50;
                    explozii[i, j].BringToFront();
                    this.Controls.Add(explozii[i, j]);
                }
                //alocarea datelor auxiliare
                explozii[i, 0].Tag = "bomba";
                explozii[i, 0].Width = 45;
                explozii[i, 0].Height = 45;
                explozii[i, 0].Top = 0;
                explozii[i, 0].Left = 0;
                folosire_bombe[i] = false;
                timer_bombe[i] = 0;
                for (int j = 0; j < 4; j++) { directii[i, j] = true; }
            }

        }

        //apasarea tastelor

        private void lvl5_KeyUp(object sender, KeyEventArgs e)
        {
            if (!player_dead && !player_finsih)
            {
                if (e.KeyCode == Keys.W) { player.Image = Properties.Resources.playeridleup; sus = false; }
                if (e.KeyCode == Keys.S) { player.Image = Properties.Resources.playeridledown; jos = false; }
                if (e.KeyCode == Keys.D) { player.Image = Properties.Resources.playeridleright; dreapta = false; }
                if (e.KeyCode == Keys.A) { player.Image = Properties.Resources.playeridleleft; stanga = false; }
                if (e.KeyCode == Keys.Space && bomba == true && folosire_bombe[count_bombe] == false)
                {
                    bool ok = true;
                    for (int i = 0; i < nr_bombe; i++)
                    {
                        if ((explozii[i, 0].Top == ((mijTop - 122) / 50) * 50 + 122 + 5) && (explozii[i, 0].Left == ((mijLeft - 74) / 50) * 50 + 74 + 5))
                            ok = false;
                    }
                    if (ok == true)
                    {
                        folosire_bombe[count_bombe] = true;
                        timer_bombe[count_bombe] = t;
                        explozii[count_bombe, 0].Top = ((mijTop - 122) / 50) * 50 + 122 + 5;
                        explozii[count_bombe, 0].Left = ((mijLeft - 74) / 50) * 50 + 74 + 5;
                        explozii[count_bombe, 0].BringToFront();
                        count_bombe++;
                        if (count_bombe >= nr_bombe)
                            count_bombe = 0;
                    }
                }
            }
        }

        private void lvl5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) sus = true;
            if (e.KeyCode == Keys.S) jos = true;
            if (e.KeyCode == Keys.D) dreapta = true;
            if (e.KeyCode == Keys.A) stanga = true;
            if (e.KeyCode == Keys.Escape)
            {
                { (new Lobby(distanta_i, nr_bombe_i, level, speed_i, scor_i, lives_i)).Show(); this.Close(); }
            }
            if (e.KeyCode == Keys.Space)
            {
                bomba = true;
            }
        }

        //timer player

        private void timer_player_Tick(object sender, EventArgs e)
        {
            //miscarea playerului

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
                if (verificare_dreapta(player) == true && player.Left + player.Width < 1527)
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

        //timer inamic orange

        private void timer_orange_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < nr_orange; i++)
            {
                //miscare orange
                if (directii_orange[i] == 1)
                {
                    if (verificare_sus(inamici_orange[i]) == true && inamici_orange[i].Top > 123)
                        inamici_orange[i].Top = inamici_orange[i].Top - viteza;
                }
                if (directii_orange[i] == 2)
                {
                    if (verificare_jos(inamici_orange[i]) == true && inamici_orange[i].Top + inamici_orange[i].Height < 670)
                        inamici_orange[i].Top = inamici_orange[i].Top + viteza;
                }
                if (directii_orange[i] == 3)
                {
                    if (verificare_dreapta(inamici_orange[i]) == true && inamici_orange[i].Left + inamici_orange[i].Width < 1527)
                        inamici_orange[i].Left = inamici_orange[i].Left + viteza;
                }
                if (directii_orange[i] == 4)
                {
                    if (verificare_stanga(inamici_orange[i]) == true && inamici_orange[i].Left > 75)
                        inamici_orange[i].Left = inamici_orange[i].Left - viteza;
                }
                //punerea bombelor peste inamici
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "bomba" || (string)x.Tag == "zid")
                        if (x.Bounds.IntersectsWith(inamici_orange[i].Bounds))
                        {
                            if (directii_orange[i] == 1) directii_orange[i] = 2;
                            if (directii_orange[i] == 2) directii_orange[i] = 1;
                            if (directii_orange[i] == 3) directii_orange[i] = 4;
                            if (directii_orange[i] == 4) directii_orange[i] = 3;
                        }
                }
                //schimbarea directiilor
                if (directii_orange[i] == 1 && (verificare_sus(inamici_orange[i]) == false || inamici_orange[i].Top <= 123)) directii_orange[i] = r.Next(1, 5);
                if (directii_orange[i] == 2 && (verificare_jos(inamici_orange[i]) == false || inamici_orange[i].Top + inamici_orange[i].Height >= 670)) directii_orange[i] = r.Next(1, 5);
                if (directii_orange[i] == 3 && (verificare_dreapta(inamici_orange[i]) == false || inamici_orange[i].Left + inamici_orange[i].Width >= 1527)) directii_orange[i] = r.Next(1, 5);
                if (directii_orange[i] == 4 && (verificare_stanga(inamici_orange[i]) == false || inamici_orange[i].Left <= 75)) directii_orange[i] = r.Next(1, 5);
                //setarea tiemrelor
                if (!dead_orange[i])
                    timere_orange[i] = t;
            }
        }

        //timer purple

        private void timer_purple_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < nr_purple; i++)
            {
                //miscare orange
                if (directii_purple[i] == 1)
                {
                    if (verificare_sus(inamici_purple[i]) == true && inamici_purple[i].Top > 123)
                        inamici_purple[i].Top = inamici_purple[i].Top - viteza;
                }
                if (directii_purple[i] == 2)
                {
                    if (verificare_jos(inamici_purple[i]) == true && inamici_purple[i].Top + inamici_purple[i].Height < 670)
                        inamici_purple[i].Top = inamici_purple[i].Top + viteza;
                }
                if (directii_purple[i] == 3)
                {
                    if (verificare_dreapta(inamici_purple[i]) == true && inamici_purple[i].Left + inamici_purple[i].Width < 1527)
                        inamici_purple[i].Left = inamici_purple[i].Left + viteza;
                }
                if (directii_purple[i] == 4)
                {
                    if (verificare_stanga(inamici_purple[i]) == true && inamici_purple[i].Left > 75)
                        inamici_purple[i].Left = inamici_purple[i].Left - viteza;
                }
                //punerea bombelor peste inamici
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "bomba" || (string)x.Tag == "zid")
                        if (x.Bounds.IntersectsWith(inamici_purple[i].Bounds))
                        {
                            if (directii_purple[i] == 1) directii_purple[i] = 2;
                            if (directii_purple[i] == 2) directii_purple[i] = 1;
                            if (directii_purple[i] == 3) directii_purple[i] = 4;
                            if (directii_purple[i] == 4) directii_purple[i] = 3;
                        }
                }
                //schimbarea directiilor
                if (directii_purple[i] == 1 && (verificare_sus(inamici_purple[i]) == false || inamici_purple[i].Top <= 123)) directii_purple[i] = r.Next(1, 5);
                if (directii_purple[i] == 2 && (verificare_jos(inamici_purple[i]) == false || inamici_purple[i].Top + inamici_purple[i].Height >= 670)) directii_purple[i] = r.Next(1, 5);
                if (directii_purple[i] == 3 && (verificare_dreapta(inamici_purple[i]) == false || inamici_purple[i].Left + inamici_purple[i].Width >= 1527)) directii_purple[i] = r.Next(1, 5);
                if (directii_purple[i] == 4 && (verificare_stanga(inamici_purple[i]) == false || inamici_purple[i].Left <= 75)) directii_purple[i] = r.Next(1, 5);
                //setarea tiemrelor
                if (!dead_purple[i])
                    timere_purple[i] = t;
            }
        }

        //timer blue

        private void timer_blue_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < nr_blue; i++)
            {
                //miscare orange
                if (directii_blue[i] == 1)
                {
                    if (verificare_sus(inamici_blue[i]) == true && inamici_blue[i].Top > 123)
                        inamici_blue[i].Top = inamici_blue[i].Top - viteza;
                }
                if (directii_blue[i] == 2)
                {
                    if (verificare_jos(inamici_blue[i]) == true && inamici_blue[i].Top + inamici_blue[i].Height < 670)
                        inamici_blue[i].Top = inamici_blue[i].Top + viteza;
                }
                if (directii_blue[i] == 3)
                {
                    if (verificare_dreapta(inamici_blue[i]) == true && inamici_blue[i].Left + inamici_blue[i].Width < 1527)
                        inamici_blue[i].Left = inamici_blue[i].Left + viteza;
                }
                if (directii_blue[i] == 4)
                {
                    if (verificare_stanga(inamici_blue[i]) == true && inamici_blue[i].Left > 75)
                        inamici_blue[i].Left = inamici_blue[i].Left - viteza;
                }
                //punerea bombelor peste inamici
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "bomba" || (string)x.Tag == "zid")
                        if (x.Bounds.IntersectsWith(inamici_blue[i].Bounds))
                        {
                            if (directii_blue[i] == 1) directii_blue[i] = 2;
                            if (directii_blue[i] == 2) directii_blue[i] = 1;
                            if (directii_blue[i] == 3) directii_blue[i] = 4;
                            if (directii_blue[i] == 4) directii_blue[i] = 3;
                        }
                }
                //schimbarea directiilor
                if (directii_blue[i] == 1 && (verificare_sus(inamici_blue[i]) == false || inamici_blue[i].Top <= 123)) directii_blue[i] = r.Next(1, 5);
                if (directii_blue[i] == 2 && (verificare_jos(inamici_blue[i]) == false || inamici_blue[i].Top + inamici_blue[i].Height >= 670)) directii_blue[i] = r.Next(1, 5);
                if (directii_blue[i] == 3 && (verificare_dreapta(inamici_blue[i]) == false || inamici_blue[i].Left + inamici_blue[i].Width >= 1527)) directii_blue[i] = r.Next(1, 5);
                if (directii_blue[i] == 4 && (verificare_stanga(inamici_blue[i]) == false || inamici_blue[i].Left <= 75)) directii_blue[i] = r.Next(1, 5);
                //setarea tiemrelor
                if (!dead_blue[i])
                    timere_blue[i] = t;
            }
        }

        //timer brown

        private void timer_brown_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < nr_brown; i++)
            {
                //miscare orange
                if (directii_brown[i] == 1)
                {
                    if (verificare_sus(inamici_brown[i]) == true && inamici_brown[i].Top > 123)
                        inamici_brown[i].Top = inamici_brown[i].Top - viteza;
                }
                if (directii_brown[i] == 2)
                {
                    if (verificare_jos(inamici_brown[i]) == true && inamici_brown[i].Top + inamici_brown[i].Height < 670)
                        inamici_brown[i].Top = inamici_brown[i].Top + viteza;
                }
                if (directii_brown[i] == 3)
                {
                    if (verificare_dreapta(inamici_brown[i]) == true && inamici_brown[i].Left + inamici_brown[i].Width < 1527)
                        inamici_brown[i].Left = inamici_brown[i].Left + viteza;
                }
                if (directii_brown[i] == 4)
                {
                    if (verificare_stanga(inamici_brown[i]) == true && inamici_brown[i].Left > 75)
                        inamici_brown[i].Left = inamici_brown[i].Left - viteza;
                }
                //punerea bombelor peste inamici
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "bomba" || (string)x.Tag == "zid")
                        if (x.Bounds.IntersectsWith(inamici_brown[i].Bounds))
                        {
                            if (directii_brown[i] == 1) directii_brown[i] = 2;
                            if (directii_brown[i] == 2) directii_brown[i] = 1;
                            if (directii_brown[i] == 3) directii_brown[i] = 4;
                            if (directii_brown[i] == 4) directii_brown[i] = 3;
                        }
                }
                //schimbarea directiilor
                if (directii_brown[i] == 1 && (verificare_sus(inamici_brown[i]) == false || inamici_brown[i].Top <= 123)) directii_brown[i] = r.Next(1, 5);
                if (directii_brown[i] == 2 && (verificare_jos(inamici_brown[i]) == false || inamici_brown[i].Top + inamici_brown[i].Height >= 670)) directii_brown[i] = r.Next(1, 5);
                if (directii_brown[i] == 3 && (verificare_dreapta(inamici_brown[i]) == false || inamici_brown[i].Left + inamici_brown[i].Width >= 1527)) directii_brown[i] = r.Next(1, 5);
                if (directii_brown[i] == 4 && (verificare_stanga(inamici_brown[i]) == false || inamici_brown[i].Left <= 75)) directii_brown[i] = r.Next(1, 5);
                //setarea tiemrelor
                if (!dead_brown[i])
                    timere_brown[i] = t;
            }
        }

        //bomb and animatii timer 

        private void timer1_Tick(object sender, EventArgs e)
        {
            //numararea ticurilor

            t++;

            //afisarea timpului  
            if (!player_dead && !player_finsih)
            {
                if (t % 40 == 0) { s--; }
                if (s == -1) { m--; s = 59; }
                if (s < 10) lbl_secunde.Text = "0" + s.ToString();
                else lbl_secunde.Text = s.ToString();
                lbl_minute.Text = m.ToString();
                if (m == -1) player_dead = true;
            }

            //animatie orange

            Random r = new Random();
            for (int i = 0; i < nr_orange; i++)
            {
                if (!dead_orange[i])
                {
                    if (t % 40 == 0)
                        inamici_orange[i].Image = Properties.Resources.orange1;
                    if (t % 40 == 10)
                        inamici_orange[i].Image = Properties.Resources.orange2;
                    if (t % 40 == 20)
                        inamici_orange[i].Image = Properties.Resources.orange3;
                    if (t % 40 == 30)
                        inamici_orange[i].Image = Properties.Resources.orange2;
                    //directie random la un interval de timp
                    if (t % 100 == 0)
                    {
                        int directie_noua = r.Next(1, 5);
                        while (directii_orange[i] == directie_noua) { directie_noua = r.Next(1, 4); }
                        directii_orange[i] = directie_noua;
                    }
                    //omorarea playerului
                    if (mijTop >= inamici_orange[i].Top && mijTop <= inamici_orange[i].Top + inamici_orange[i].Height && mijLeft >= inamici_orange[i].Left && mijLeft <= inamici_orange[i].Left + inamici_orange[i].Width) player_dead = true;
                }
            }

            //animatie purple

            for (int i = 0; i < nr_purple; i++)
            {
                if (!dead_purple[i])
                {
                    if (t % 40 == 0)
                        inamici_purple[i].Image = Properties.Resources.purple1;
                    if (t % 40 == 10)
                        inamici_purple[i].Image = Properties.Resources.purple2;
                    if (t % 40 == 20)
                        inamici_purple[i].Image = Properties.Resources.purple3;
                    if (t % 40 == 30)
                        inamici_purple[i].Image = Properties.Resources.purple2;
                    //directie random la un interval de timp
                    if (t % 80 == 0)
                    {
                        int directie_noua = r.Next(1, 5);
                        while (directii_purple[i] == directie_noua) { directie_noua = r.Next(1, 4); }
                        directii_purple[i] = directie_noua;
                    }
                    //omorarea playerului
                    if (mijTop >= inamici_purple[i].Top && mijTop <= inamici_purple[i].Top + inamici_purple[i].Height && mijLeft >= inamici_purple[i].Left && mijLeft <= inamici_purple[i].Left + inamici_purple[i].Width) player_dead = true;
                }
            }

            //animatie blue

            for (int i = 0; i < nr_blue; i++)
            {
                if (!dead_blue[i])
                {
                    if (t % 40 == 0)
                        inamici_blue[i].Image = Properties.Resources.blue1;
                    if (t % 40 == 10)
                        inamici_blue[i].Image = Properties.Resources.blue2;
                    if (t % 40 == 20)
                        inamici_blue[i].Image = Properties.Resources.blue3;
                    if (t % 40 == 30)
                        inamici_blue[i].Image = Properties.Resources.blue2;
                    //directie random la un interval de timp
                    if (t % 80 == 0)
                    {
                        int directie_noua = r.Next(1, 5);
                        while (directii_blue[i] == directie_noua) { directie_noua = r.Next(1, 4); }
                        directii_blue[i] = directie_noua;
                    }
                    //omorarea playerului
                    if (mijTop >= inamici_blue[i].Top && mijTop <= inamici_blue[i].Top + inamici_blue[i].Height && mijLeft >= inamici_blue[i].Left && mijLeft <= inamici_blue[i].Left + inamici_blue[i].Width) player_dead = true;
                }
            }

            //animatie brown

            for (int i = 0; i < nr_brown; i++)
            {
                if (!dead_brown[i])
                {
                    if (t % 40 == 0)
                        inamici_brown[i].Image = Properties.Resources.brown1;
                    if (t % 40 == 10)
                        inamici_brown[i].Image = Properties.Resources.brown2;
                    if (t % 40 == 20)
                        inamici_brown[i].Image = Properties.Resources.brown3;
                    if (t % 40 == 30)
                        inamici_brown[i].Image = Properties.Resources.brown2;
                    //directie random la un interval de timp
                    if (t % 40 == 0)
                    {
                        int directie_noua = r.Next(1, 5);
                        while (directii_brown[i] == directie_noua) { directie_noua = r.Next(1, 4); }
                        directii_brown[i] = directie_noua;
                    }
                    //omorarea playerului
                    if (mijTop >= inamici_brown[i].Top && mijTop <= inamici_brown[i].Top + inamici_brown[i].Height && mijLeft >= inamici_brown[i].Left && mijLeft <= inamici_brown[i].Left + inamici_brown[i].Width) player_dead = true;
                }
            }

            //animatie powerup
            if (t % 20 == 0)
                powerup.Image = Properties.Resources.powerup_distanta1;
            if (t % 20 == 10)
                powerup.Image = Properties.Resources.powerup_distanta2;

            //updatarea coordonatelor playerului

            mijTop = player.Top + player.Height / 2;
            mijLeft = player.Left + player.Width / 2;

            //explozie bomba1

            for (int i = 0; i < 4; i++)
            {
                if (folosire_bombe[i] == true)
                {
                    //transfomarea bombei in solid
                    if (!player.Bounds.IntersectsWith(explozii[i, 0].Bounds))
                        explozii[i, 0].Tag = "zid";
                    //animatia bombei
                    for (int j = 2; j < 10; j++)
                    {
                        if (t % 5 == 0 && t % 10 != 0) explozii[i, j].Image = Properties.Resources.explozieInaltime;
                        if (t % 10 == 0) explozii[i, j].Image = Properties.Resources.explozieInaltimeInvers;
                    }
                    for (int j = 10; j < 18; j++)
                    {
                        if (t % 5 == 0 && t % 10 != 0) explozii[i, j].Image = Properties.Resources.explosieLatimeInvers;
                        if (t % 10 == 0) explozii[i, j].Image = Properties.Resources.explosieLatime;
                    }
                    if (t % 5 == 0 && t % 10 != 0) explozii[i, 0].Image = Properties.Resources.bomb1;
                    if (t % 10 == 0) explozii[i, 0].Image = Properties.Resources.bomb2;
                    if (timer_bombe[i] == t - 1) //bomba initiala
                        explozii[i, 0].Visible = true;
                    //daca o bomba este activata de explozia altei bomba
                    foreach (Control x in this.Controls)
                    {
                        if ((string)x.Tag == "explozie")
                            if (x.Bounds.IntersectsWith(explozii[i, 0].Bounds) && explozii[i, 0].Visible == true)
                                timer_bombe[i] = t - 85;
                    }
                    if (timer_bombe[i] == t - 85) //explozia initiala
                    {
                        explozii[i, 0].Visible = false;
                        explozii[i, 1].Visible = true;
                        explozii[i, 1].Top = explozii[i, 0].Top - 5;
                        explozii[i, 1].Left = explozii[i, 0].Left - 5;
                        //exploziile laterale
                        for (int j = 2; j < 2 + distanta; j++) //sus
                        {
                            if (explozii[i, 1].Top - (j - 1) * 50 >= 122 && directii[i, 0] == true)
                            {
                                explozii[i, j].Top = explozii[i, 1].Top - (j - 1) * 50;
                                explozii[i, j].Left = explozii[i, 1].Left;
                                explozii[i, j].Visible = true;
                            }
                            foreach (Control x in this.Controls) //fier
                            {
                                if ((string)x.Tag == "zid" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds)) { directii[i, 0] = false; explozii[i, j].Visible = false; }
                                }
                            }
                            foreach (Control x in this.Controls) //caramida
                            {
                                if ((string)x.Tag == "zid_destructibil" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds))
                                    { x.Tag = "zid_distrus"; explozii[i, j].Visible = false; directii[i, 0] = false; }
                                }
                            }
                        }
                        for (int j = 6; j < 6 + distanta; j++) //jos
                        {
                            if (explozii[i, 1].Top + (j - 5) * 50 <= 622 && directii[i, 1] == true)
                            {
                                explozii[i, j].Top = explozii[i, 1].Top + (j - 5) * 50;
                                explozii[i, j].Left = explozii[i, 1].Left;
                                explozii[i, j].Visible = true;
                            }
                            foreach (Control x in this.Controls) //fier
                            {
                                if ((string)x.Tag == "zid" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds)) { directii[i, 1] = false; explozii[i, j].Visible = false; }
                                }
                            }
                            foreach (Control x in this.Controls) //caramida
                            {
                                if ((string)x.Tag == "zid_destructibil" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds))
                                    { x.Tag = "zid_distrus"; explozii[i, j].Visible = false; directii[i, 1] = false; }
                                }
                            }
                        }
                        for (int j = 10; j < 10 + distanta; j++) //dreapta
                        {
                            if (explozii[i, 1].Left + (j - 9) * 50 <= 1474 && directii[i, 2] == true)
                            {
                                explozii[i, j].Top = explozii[i, 1].Top;
                                explozii[i, j].Left = explozii[i, 1].Left + (j - 9) * 50;
                                explozii[i, j].Visible = true;
                            }
                            foreach (Control x in this.Controls) //fier
                            {
                                if ((string)x.Tag == "zid" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds)) { directii[i, 2] = false; explozii[i, j].Visible = false; }
                                }
                            }
                            foreach (Control x in this.Controls) //caramida
                            {
                                if ((string)x.Tag == "zid_destructibil" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds))
                                    { x.Tag = "zid_distrus"; explozii[i, j].Visible = false; directii[i, 2] = false; }
                                }
                            }
                        }
                        for (int j = 14; j < 14 + distanta; j++) //stanga
                        {
                            if (explozii[i, 1].Left - (j - 13) * 50 >= 74 && directii[i, 3] == true)
                            {
                                explozii[i, j].Top = explozii[i, 1].Top;
                                explozii[i, j].Left = explozii[i, 1].Left - (j - 13) * 50;
                                explozii[i, j].Visible = true;
                            }
                            foreach (Control x in this.Controls) //fier
                            {
                                if ((string)x.Tag == "zid" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds)) { directii[i, 3] = false; explozii[i, j].Visible = false; }
                                }
                            }
                            foreach (Control x in this.Controls) //caramida
                            {
                                if ((string)x.Tag == "zid_destructibil" && x.Visible == true)
                                {
                                    if (x.Bounds.IntersectsWith(explozii[i, j].Bounds))
                                    { x.Tag = "zid_distrus"; explozii[i, j].Visible = false; directii[i, 3] = false; }
                                }
                            }
                        }
                        for (int k = 0; k < nr_pereti; k++)
                            if ((string)pereti[k].Tag == "zid_distrus") pereti[k].Image = Properties.Resources.block_spart;
                    }
                    //iesire si eliberarea parametrilor pentru urmatoarea explozie
                    if (timer_bombe[i] == t - 100)
                    {
                        explozii[i, 0].Tag = "bomba";
                        for (int j = 0; j < 18; j++)
                        {
                            explozii[i, j].Visible = false;
                            explozii[i, j].Top = 0;
                            explozii[i, j].Left = 0;
                        }
                        for (int j = 0; j < 4; j++) { directii[i, j] = true; }
                        foreach (Control x in this.Controls)
                        {
                            if ((string)x.Tag == "zid_distrus" && x.Visible == true)
                            {
                                x.Visible = false;
                            }
                        }
                        folosire_bombe[i] = false;
                    }
                }
            }
            //colectare powerup
            if (mijTop >= powerup.Top && mijTop <= powerup.Top + powerup.Height && mijLeft >= powerup.Left && mijLeft <= powerup.Left + powerup.Width && powerup.Visible == true && !pwu_dead)
            {
                scor += 50;
                powerup.Visible = false;
                if (distanta < 4)
                    distanta++;
            }

            //explozie player

            if (player_dead == true)
            {
                timer_player.Stop(); //oprirea miscarii
                if (playerT > t - 45)
                {
                    if (t % 5 == 0 && t % 10 != 0)
                        player.Image = Properties.Resources.playerdead1;
                    if (t % 10 == 0)
                        player.Image = Properties.Resources.playerdead2;
                }
                if (playerT == t - 45) player.Image = Properties.Resources.playerdespawn1;
                if (playerT == t - 55) player.Image = Properties.Resources.playerdespawn2;
                if (playerT == t - 65) player.Visible = false;
                if (playerT == t - 75) { (new Lobby(distanta_i, nr_bombe_i, level, speed_i, scor_i, lives_i)).Show(); this.Close(); }
            }

            //moarte player powerup si poarta

            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "explozie" && x.Visible == true)
                {
                    if (x.Bounds.IntersectsWith(player.Bounds)) { player_dead = true; x.BringToFront(); }

                    if (x.Bounds.IntersectsWith(exit.Bounds) && poarta == false) { poarta = true; }
                    if (x.Bounds.IntersectsWith(exit.Bounds) && poarta == true) { poarta_dead = true; exit.SendToBack(); }
                    if (x.Bounds.IntersectsWith(powerup.Bounds) && pwu == false) { pwu = true; }
                    if (x.Bounds.IntersectsWith(powerup.Bounds) && pwu == true && pwu_dead == false) { pwu_dead = true; powerup.SendToBack(); scor -= 50; }
                }
            }
            if (poarta_dead) exit.Image = Properties.Resources.poarta_dead;
            if (pwu_dead) powerup.Image = Properties.Resources.powerup_distanta_dead;

            //explozie orange

            for (int i = 0; i < nr_orange; i++)
            {
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "explozie" && x.Visible == true)
                    {
                        if (x.Bounds.IntersectsWith(inamici_orange[i].Bounds)) dead_orange[i] = true;
                    }
                }
                if (dead_orange[i] == true)
                {
                    directii_orange[i] = 0; //oprirea miscarii
                    if (timere_orange[i] > t - 35)
                    {
                        inamici_orange[i].Image = Properties.Resources.orangedead;
                    }
                    if (timere_orange[i] == t - 35) inamici_orange[i].Image = Properties.Resources.inamicdespawn1;
                    if (timere_orange[i] == t - 45) inamici_orange[i].Image = Properties.Resources.inamicdespawn2;
                    if (timere_orange[i] == t - 55) { inamici_orange[i].Visible = false; scor += 100; }
                }
            }

            //explozie purple

            for (int i = 0; i < nr_purple; i++)
            {
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "explozie" && x.Visible == true)
                    {
                        if (x.Bounds.IntersectsWith(inamici_purple[i].Bounds)) dead_purple[i] = true;
                    }
                }
                if (dead_purple[i] == true)
                {
                    directii_purple[i] = 0; //oprirea miscarii
                    if (timere_purple[i] > t - 35)
                    {
                        inamici_purple[i].Image = Properties.Resources.purpledead;
                    }
                    if (timere_purple[i] == t - 35) inamici_purple[i].Image = Properties.Resources.inamicdespawn1;
                    if (timere_purple[i] == t - 45) inamici_purple[i].Image = Properties.Resources.inamicdespawn2;
                    if (timere_purple[i] == t - 55) { inamici_purple[i].Visible = false; scor += 200; }
                }
            }

            //explozie blue

            for (int i = 0; i < nr_blue; i++)
            {
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "explozie" && x.Visible == true)
                    {
                        if (x.Bounds.IntersectsWith(inamici_blue[i].Bounds)) dead_blue[i] = true;
                    }
                }
                if (dead_blue[i] == true)
                {
                    directii_blue[i] = 0; //oprirea miscarii
                    if (timere_blue[i] > t - 35)
                    {
                        inamici_blue[i].Image = Properties.Resources.bluedead;
                    }
                    if (timere_blue[i] == t - 35) inamici_blue[i].Image = Properties.Resources.inamicdespawn1;
                    if (timere_blue[i] == t - 45) inamici_blue[i].Image = Properties.Resources.inamicdespawn2;
                    if (timere_blue[i] == t - 55) { inamici_blue[i].Visible = false; scor += 250; }
                }
            }

            //explozie brown

            for (int i = 0; i < nr_brown; i++)
            {
                foreach (Control x in this.Controls)
                {
                    if ((string)x.Tag == "explozie" && x.Visible == true)
                    {
                        if (x.Bounds.IntersectsWith(inamici_brown[i].Bounds)) dead_brown[i] = true;
                    }
                }
                if (dead_brown[i] == true)
                {
                    directii_brown[i] = 0; //oprirea miscarii
                    if (timere_brown[i] > t - 35)
                    {
                        inamici_brown[i].Image = Properties.Resources.browndead;
                    }
                    if (timere_brown[i] == t - 35) inamici_brown[i].Image = Properties.Resources.inamicdespawn1;
                    if (timere_brown[i] == t - 45) inamici_brown[i].Image = Properties.Resources.inamicdespawn2;
                    if (timere_brown[i] == t - 55) { inamici_brown[i].Visible = false; scor += 400; }
                }
            }

            lbl_score.Text = scor.ToString();

            //iesire

            if (verificare_poarta())
            {
                if (!poarta_dead)
                {
                    if (t % 20 == 0) { exit.Image = Properties.Resources.poarta1; }
                    if (t % 20 == 10) { exit.Image = Properties.Resources.poarta2; }
                    for (int k = 0; k < contor_pereti; k++)
                    {
                        if ((string)pereti[k].Tag == "zid_destructibil" && pereti[k].Top == powerup.Top && pereti[k].Left == powerup.Left)
                        {
                            if (t % 20 == 0) { pereti[k].Image = Properties.Resources.block; }
                            if (t % 20 == 10) { pereti[k].Image = Properties.Resources.block_spart; }
                        }
                    }
                    if (mijTop >= exit.Top && mijTop <= exit.Top + exit.Height && mijLeft >= exit.Left && mijLeft <= exit.Left + exit.Width)
                    {
                        player_finsih = true;
                    }
                }
            }

            if (player_finsih == true)
            {
                timer_player.Stop();
                player.Top = exit.Top + 5;
                player.Left = exit.Left + 5;
                player.BringToFront();
                if (playerT == t - 10) scor += m * 60 + (s / 10) * 10;
                if (t % 40 == 0) { player.Image = Properties.Resources.playeridledown; }
                if (t % 40 == 10) { player.Image = Properties.Resources.playeridleleft; }
                if (t % 40 == 20) { player.Image = Properties.Resources.playeridleup; }
                if (t % 40 == 30) { player.Image = Properties.Resources.playeridleright; }
                if (playerT == t - 100) { (new Lobby(distanta, nr_bombe, level + 1, speed, scor, lives)).Show(); this.Close(); }
            }
        }
    }
}
