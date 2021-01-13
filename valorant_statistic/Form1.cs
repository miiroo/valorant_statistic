using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace valorant_statistic
{
    public partial class Form1 : Form
    {

        public String fileName = @"datas.txt";
        public String seasonName = "";

        public Form1() {
            InitializeComponent();
        }

        //line 0 - settings and datas 
        //current act-season 
        //lines contain:
        //act-season*combat score*k-d*roundT-roundE*data
        private void Form1_Load(object sender, EventArgs e) {
            if (!File.Exists(fileName)) {
                MessageBox.Show("HINTS\nDon't use * in datas.\nTo divide K-D/Round WIN-LOSE use any symbol\nWrite your team rounds before enemy team rounds.");
                StreamWriter sw = File.CreateText(fileName);
                sw.Close();
                season.Text = "Please add new season.";
            }
            else {
                string[] lines = File.ReadAllLines(fileName);
                if (lines.Length == 0) season.Text = "Please add new season.";
                else {
                    string currentSeason = lines[0];
                    currentSeason = currentSeason.Remove(0, 8);
                    season.Text = currentSeason;
                    seasonName = currentSeason;
                    if (lines.Length == 1) {
                        ToolStripMenuItem item = new ToolStripMenuItem(currentSeason);
                        item.Click += new EventHandler(item_Click);
                        chooseSeasonToolStripMenuItem.DropDownItems.Add(item);
                    }
                    else {
                        List<String> seasons = new List<String>();
                        seasons = getAllSeasons();
                        foreach (string line in seasons) {
                            ToolStripMenuItem item = new ToolStripMenuItem(line);
                            item.Click += new EventHandler(item_Click);
                            chooseSeasonToolStripMenuItem.DropDownItems.Add(item);
                        }
                    }
                }
                updateSeasonStat();
            }
        }

        private void item_Click(object sender, EventArgs e) {
            var menuItem = sender as ToolStripMenuItem;
            var menuText = menuItem.Text;
            seasonName = menuText;
            seasonAdded();
        }


        //lines contain:
        //act-season*combat score*k-d*roundT-roundE*data
        private List<String> getAllSeasons() {
            string[] lines = File.ReadAllLines(fileName);
            List<String> seasons = new List<String>();
            seasons.Add(lines[0].Remove(0, 8));
            foreach (string line in lines) {
                if (!line.Contains("current")) {
                    string getSeason = line.Remove(line.IndexOf('*'), line.Length- line.IndexOf('*'));
                    if (!seasons.Contains(getSeason)) seasons.Add(getSeason);
                }
            }
            return seasons;
        }

        private void addSeasonToolStripMenuItem_Click(object sender, EventArgs e) {
            Form seasonForm = new addSeasonForm(this);
            seasonForm.ShowDialog();
            
        }

        public void seasonAdded() {
            string[] lines = File.ReadAllLines(fileName);
            if (lines.Length != 0) {
                lines[0] = "current " + seasonName;
            }
            else {
                lines = new string[1];
                lines[0] = "current " + seasonName;
            }
            try {
                StreamWriter sw = new StreamWriter(fileName, false);
                foreach (string line in lines) {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
                this.Close();
            }
            bool addSeason = true;
            foreach (ToolStripMenuItem item in chooseSeasonToolStripMenuItem.DropDownItems) {
                if (item.Text == seasonName) {
                    addSeason = false;
                }
            }
            if (addSeason) {
                ToolStripMenuItem item2 = new ToolStripMenuItem(seasonName);
                item2.Click += new EventHandler(item_Click);
                chooseSeasonToolStripMenuItem.DropDownItems.Add(item2);
            }
            season.Text = seasonName;
            updateSeasonStat();
        }


        //act-season*combat score*k-d*roundT-roundE*data
        private void updateSeasonStat() {
            string[] lines = File.ReadAllLines(fileName);
            int total = 0;
            int lose = 0;
            int win = 0;
            int killsI = 0;
            int deathsI = 0;
            int roundTeam = 0;
            int roundEnemy = 0;
            float kdaratio = (float)0;
            int averagecs = 0;

            if (lines.Length > 1) {
                for (int i = 1; i<lines.Length; i++) {
                    string kills = ""; //roundTeam
                    string deaths = ""; //roundEnemy
                    string combatscore = "";
                    string kda = ""; //match


                    string line = lines[i];
                    if(line.Contains(seasonName)) {
                        total++;
                        //get combat score
                        line = line.Remove(0, line.IndexOf('*') + 1);
                        for (int j=0; j<line.IndexOf('*');j++) {
                            combatscore += line[j];
                            
                        }
                        if (combatscore != "")
                            averagecs += Int32.Parse(combatscore);

                        //get k/d
                        line = line.Remove(0, line.IndexOf('*') + 1);
                        for (int j = 0; j < line.IndexOf('*'); j++) {
                            kda += line[j];
                        }
                        if (kda != "") {
                            int k = 0;
                            while (k < kda.Length && Char.IsDigit(kda[k])) {
                                kills += kda[k];
                                k++;
                            }
                            k++;
                            killsI += Int32.Parse(kills);
                            while (k< kda.Length) {
                                deaths += kda[k];
                                k++;
                            }
                            if (deaths != "")
                            deathsI = Int32.Parse(deaths);
                            
                        }

                        //get rounds win lose
                        kills = "";
                        deaths = "";
                        kda = "";
                        line = line.Remove(0, line.IndexOf('*') + 1);
                        for (int j = 0; j < line.IndexOf('*'); j++) {
                            kda += line[j];
                        }
                        if (kda != "") {
                            int k = 0;
                            while (k < kda.Length && Char.IsDigit(kda[k])) {
                                kills += kda[k];
                                k++;
                            }
                            k++;
                            roundTeam = Int32.Parse(kills);
                            while (k < kda.Length) {
                                deaths += kda[k];
                                k++;
                            }
                            if (deaths != "")
                                roundEnemy = Int32.Parse(deaths);

                            if (roundTeam > roundEnemy) win++;
                            else lose++;
                        }
                    }
                }

                if (total != 0) {
                    averagecs = averagecs / total;
                    if (deathsI == 0) deathsI = 1;
                    kdaratio = killsI / deathsI;
                }

            }
            totalmatches.Text = total.ToString();
            wincount.Text = win.ToString();
            losecount.Text = lose.ToString();
            avrcs.Text = averagecs.ToString();
            if (kdaratio < 1) {
                kdratio.ForeColor = Color.FromArgb(192, 0, 0);
                kdratio.Text = String.Format("{0:0.00}", kdaratio);
            }
            else {
                kdratio.ForeColor = Color.Lime;
                kdratio.Text = String.Format("{0:0.00}", kdaratio);
            }
        }

        private void showAllMatchesToolStripMenuItem_Click(object sender, EventArgs e) {
            Form allMathcesForm = new matchesForm(this);
            allMathcesForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (seasonName != "") {
                StreamWriter sw = new StreamWriter(fileName, true);
                sw.WriteLine(seasonName + "*" + textBox1.Text + "*" + textBox2.Text + "*" + textBox3.Text + "*" + DateTime.Today.ToShortDateString());
                sw.Close();
                updateSeasonStat();
            }
            else {
                MessageBox.Show("Add season first.");
            }
        }
    }
}
