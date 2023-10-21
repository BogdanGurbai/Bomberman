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
    public partial class lvl8 : Form
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

        public lvl8(int d, int nrb, int lvl, int vit, int scr, int vieti)
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
            MakeBombs();
            exit.Top = 10000;
            exit.Left = 10000;
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
        //inamic boss
        int directie_boss = 1, timer_boss;
        bool dead_boss = false;
        int mijTopB;
        int mijLeftB;
        int stage = 1;
        //inamic brown
        PictureBox[] inamici_brown = new PictureBox[100];//imaginile
        int[] directii_brown = new int[100]; //directiile 1sus 2jos 3dreapta 4stanga 
        int[] timere_brown = new int[100]; //timer
        bool[] dead_brown = new bool[100];
        int nr_brown = 0;
        //poarta
        bool poarta = false;
        bool poarta_dead = false;

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

        //apasarea tastelor

        private void lvl8_KeyUp(object sender, KeyEventArgs e)
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

        private void lvl8_KeyDown(object sender, KeyEventArgs e)
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

        //make brown

        void MakeBrown()
        {
            //initiere
            inamici_brown[nr_brown - 1] = new PictureBox();
            //alocarea atributiilor
            Random r = new Random();
            inamici_brown[nr_brown - 1].Height = 40;
            inamici_brown[nr_brown - 1].Width = 40;
            inamici_brown[nr_brown - 1].Tag = "inamic";
            inamici_brown[nr_brown - 1].Image = Properties.Resources.brown1;
            inamici_brown[nr_brown - 1].SizeMode = PictureBoxSizeMode.StretchImage;
            inamici_brown[nr_brown - 1].BackColor = Color.Transparent;
            inamici_brown[nr_brown - 1].BringToFront();
            inamici_brown[nr_brown - 1].Visible = true;
            this.Controls.Add(inamici_brown[nr_brown - 1]);
            //timer
            timere_brown[nr_brown - 1] = t;
            //initializarea directiei
            directii_brown[nr_brown - 1] = r.Next(1, 4);
            //verificare mort
            dead_brown[nr_brown - 1] = false;
            inamici_brown[nr_brown - 1].Top = ((mijTopB - 122) / 50) * 50 + 122 + 5;
            inamici_brown[nr_brown - 1].Left = ((mijLeftB - 74) / 50) * 50 + 74 + 5;
        }

        //crearea bombelor

        void MakeBombs()
        {
            for (int i = 0; i < nr_bombe; i++)
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

        //timer inamic brown

        private void timer_orange_Tick(object sender, EventArgs e)
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
                    if (verificare_dreapta(inamici_brown[i]) == true && inamici_brown[i].Left + inamici_brown[i].Width < 727)
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
                if (directii_brown[i] == 3 && (verificare_dreapta(inamici_brown[i]) == false || inamici_brown[i].Left + inamici_brown[i].Width >= 727)) directii_brown[i] = r.Next(1, 5);
                if (directii_brown[i] == 4 && (verificare_stanga(inamici_brown[i]) == false || inamici_brown[i].Left <= 75)) directii_brown[i] = r.Next(1, 5);
                //setarea tiemrelor
                if (!dead_brown[i])
                    timere_brown[i] = t;
            }
        }

        //timer boss

        private void timer_corp_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            //miscare orange
            if (directie_boss == 1)
            {
                if (verificare_sus(boss) == true && boss.Top > 123)
                    boss.Top = boss.Top - viteza;
            }
            if (directie_boss == 2)
            {
                if (verificare_jos(boss) == true && boss.Top + boss.Height < 670)
                    boss.Top = boss.Top + viteza;
            }
            if (directie_boss == 3)
            {
                if (verificare_dreapta(boss) == true && boss.Left + boss.Width < 727)
                    boss.Left = boss.Left + viteza;
            }
            if (directie_boss == 4)
            {
                if (verificare_stanga(boss) == true && boss.Left > 75)
                    boss.Left = boss.Left - viteza;
            }
            //punerea bombelor peste inamici
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "bomba" || (string)x.Tag == "zid")
                    if (x.Bounds.IntersectsWith(boss.Bounds))
                    {
                        if (directie_boss == 1) directie_boss = 2;
                        if (directie_boss == 2) directie_boss = 1;
                        if (directie_boss == 3) directie_boss = 4;
                        if (directie_boss == 4) directie_boss = 3;
                    }
            }
            //schimbarea directiilor
            if (directie_boss == 1 && (verificare_sus(boss) == false || boss.Top <= 123)) directie_boss = r.Next(1, 5);
            if (directie_boss == 2 && (verificare_jos(boss) == false || boss.Top + boss.Height >= 670)) directie_boss = r.Next(1, 5);
            if (directie_boss == 3 && (verificare_dreapta(boss) == false || boss.Left + boss.Width >= 727)) directie_boss = r.Next(1, 5);
            if (directie_boss == 4 && (verificare_stanga(boss) == false || boss.Left <= 75)) directie_boss = r.Next(1, 5);
            //setarea tiemrelor
            if (!dead_boss)
                timer_boss = t;
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
                if (s == 0) { m--; s = 59; }
                if (s < 10) lbl_secunde.Text = "0" + s.ToString();
                else lbl_secunde.Text = s.ToString();
                lbl_minute.Text = m.ToString();
                if (m == -1) player_dead = true;
            }

            //animatie brown

            Random r = new Random();

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

            //animatie boss
            if (!dead_boss)
            {
                if (stage == 1)
                {
                    if (t % 40 == 0)
                        boss.Image = Properties.Resources.boss11;
                    if (t % 40 == 10)
                        boss.Image = Properties.Resources.boss12;
                    if (t % 40 == 20)
                        boss.Image = Properties.Resources.boss13;
                    if (t % 40 == 30)
                        boss.Image = Properties.Resources.boss12;
                }
                if (stage == 2)
                {
                    if (t % 40 == 0)
                        boss.Image = Properties.Resources.boss21;
                    if (t % 40 == 10)
                        boss.Image = Properties.Resources.boss22;
                    if (t % 40 == 20)
                        boss.Image = Properties.Resources.boss23;
                    if (t % 40 == 30)
                        boss.Image = Properties.Resources.boss22;
                }
                if (stage == 3)
                {
                    if (t % 40 == 0)
                        boss.Image = Properties.Resources.boss31;
                    if (t % 40 == 10)
                        boss.Image = Properties.Resources.boss32;
                    if (t % 40 == 20)
                        boss.Image = Properties.Resources.boss33;
                    if (t % 40 == 30)
                        boss.Image = Properties.Resources.boss32;
                }
                if (stage == 4)
                {
                    if (t % 40 == 0)
                        boss.Image = Properties.Resources.boss41;
                    if (t % 40 == 10)
                        boss.Image = Properties.Resources.boss42;
                    if (t % 40 == 20)
                        boss.Image = Properties.Resources.boss43;
                    if (t % 40 == 30)
                        boss.Image = Properties.Resources.boss42;
                }
                if (stage == 5)
                {
                    if (t % 40 == 0)
                        boss.Image = Properties.Resources.boss51;
                    if (t % 40 == 10)
                        boss.Image = Properties.Resources.boss52;
                    if (t % 40 == 20)
                        boss.Image = Properties.Resources.boss53;
                    if (t % 40 == 30)
                        boss.Image = Properties.Resources.boss52;
                }
                //directie random la un interval de timp
                if (t % 120 == 0)
                {
                    int directie_noua = r.Next(1, 5);
                    while (directie_boss == directie_noua) { directie_noua = r.Next(1, 4); }
                    directie_boss = directie_noua;
                }
                //omorarea playerului
                if (mijTop >= boss.Top && mijTop <= boss.Top + boss.Height && mijLeft >= boss.Left && mijLeft <= boss.Left + boss.Width) player_dead = true;
            }

            //updatarea coordonatelor playerului si a bossului

            mijTop = player.Top + player.Height / 2;
            mijLeft = player.Left + player.Width / 2;

            mijTopB = boss.Top + boss.Height / 2;
            mijLeftB = boss.Left + boss.Width / 2;

            //spawn brown
            if (!player_dead && !dead_boss)
            {
                if (t % 100 == 0)
                {
                    nr_brown++;
                    MakeBrown();
                }
            }

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
                        }
                        for (int j = 10; j < 10 + distanta; j++) //dreapta
                        {
                            if (explozii[i, 1].Left + (j - 9) * 50 <= 674 && directii[i, 2] == true)
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
                        }
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
                        folosire_bombe[i] = false;
                    }
                }
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
                }
            }
            if (poarta_dead) exit.Image = Properties.Resources.poarta_dead;

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

            //explozie boss
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "explozie" && x.Visible == true)
                {
                    if (x.Bounds.IntersectsWith(boss.Bounds) && !dead_boss) dead_boss = true;
                }
            }
            if (dead_boss == true)
            {
                if (stage == 1)
                {
                    if (timer_boss > t - 35)
                    {
                        if (t % 5 == 0 && t % 10 != 0)
                            boss.Image = Properties.Resources.bossdead;
                        if (t % 10 == 0)
                            boss.Image = Properties.Resources.bosswhite;
                    }
                    if (timer_boss <= t - 35) { dead_boss = false; stage++; }
                }
            }
            if (dead_boss == true)
            {
                if (stage == 2)
                {
                    if (timer_boss > t - 35)
                    {
                        if (t % 5 == 0 && t % 10 != 0)
                            boss.Image = Properties.Resources.bossdead;
                        if (t % 10 == 0)
                            boss.Image = Properties.Resources.bosswhite;
                    }
                    if (timer_boss <= t - 35) { dead_boss = false; stage++; }
                }
            }
            if (dead_boss == true)
            {
                if (stage == 3)
                {
                    if (timer_boss > t - 35)
                    {
                        if (t % 5 == 0 && t % 10 != 0)
                            boss.Image = Properties.Resources.bossdead;
                        if (t % 10 == 0)
                            boss.Image = Properties.Resources.bosswhite;
                    }
                    if (timer_boss <= t - 35) { dead_boss = false; stage++; }
                }
            }
            if (dead_boss == true)
            {
                if (stage == 4)
                {
                    if (timer_boss > t - 35)
                    {
                        if (t % 5 == 0 && t % 10 != 0)
                            boss.Image = Properties.Resources.bossdead;
                        if (t % 10 == 0)
                            boss.Image = Properties.Resources.bosswhite;
                    }
                    if (timer_boss <= t - 35) { dead_boss = false; stage++; }
                }
            }
            if (dead_boss)
            {
                if (stage == 5)
                {
                    directie_boss = 0;
                    if (timer_boss > t - 35)
                    {
                        boss.Image = Properties.Resources.bossdead;
                    }
                    if (timer_boss == t - 35) boss.Image = Properties.Resources.inamicdespawn1;
                    if (timer_boss == t - 45) boss.Image = Properties.Resources.inamicdespawn2;
                    if (timer_boss == t - 55) { boss.Visible = false; scor += 1000; }
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
                    exit.Top = 372;
                    exit.Left = 374;
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
                if (playerT == t - 100)
                {
                    if (level >= 9)
                    {
                        (new Lobby(distanta, nr_bombe, level + 1, speed, scor, lives)).Show();
                        this.Close();
                    }
                    else
                    {
                        End final = new End(scor);
                        final.Show();
                        this.Hide();
                    }
                }

            }
        }
    }
}
