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

namespace valorant_statistic
{
    public partial class matchesForm : Form
    {
        private Form1 _owner;
        public matchesForm(Form1 owner) {
            InitializeComponent();
            _owner = owner;
            this.BackgroundImage = owner.BackgroundImage;
            this.BackgroundImageLayout = owner.BackgroundImageLayout;
            this.FormBorderStyle = owner.FormBorderStyle;
            this.Icon = owner.Icon;
        }

        private void matchesForm_Load(object sender, EventArgs e) {
            dataGridView1.ForeColor = Color.Black;
            string[] lines = File.ReadAllLines(_owner.fileName);
            if (lines.Length > 1) {
                for (int i = 1; i < lines.Length; i++) {
                    string data = "";
                    string combatscore = "";
                    string kda = ""; //match
                    string mathces = "";

                    string line = lines[i];
                    if (line.Contains(_owner.seasonName)) {
                        //get combat score
                        line = line.Remove(0, line.IndexOf('*') + 1);
                        for (int j = 0; j < line.IndexOf('*'); j++) {
                            combatscore += line[j];
                        }

                        //get k/d
                        line = line.Remove(0, line.IndexOf('*') + 1);
                        for (int j = 0; j < line.IndexOf('*'); j++) {
                            if (Char.IsDigit(line[j]) )
                                kda += line[j];
                            else kda += " / ";
                        }

                        //get rounds win lose
                        string kills = "";
                        string deaths = "";
                        int roundTeam = 0;
                        int roundEnemy = 0;

                        line = line.Remove(0, line.IndexOf('*') + 1);
                        for (int j = 0; j < line.IndexOf('*'); j++) {
                            if (Char.IsDigit(line[j]) )
                                mathces += line[j];
                            else mathces += " / ";
                        }
                        if (mathces != "") {
                            int k = 0;
                            while (k < mathces.Length && (Char.IsDigit(mathces[k]) || mathces[k] == ' ')) {
                                if(mathces[k] != ' ')
                                    kills += mathces[k];
                                k++;
                            }
                            k++;
                            roundTeam = Int32.Parse(kills);
                            while (k < mathces.Length) {
                                if (mathces[k] != ' ')
                                    deaths += mathces[k];
                                k++;
                            }
                            if (deaths != "")
                            roundEnemy = Int32.Parse(deaths);
                        }


                        //get data
                        line = line.Remove(0, line.IndexOf('*') + 1);
                        for (int j = 0; j < line.Length; j++) {
                            data += line[j];
                        }


                        dataGridView1.Rows.Add(_owner.seasonName,combatscore,kda,mathces,data);

                        if (roundTeam > roundEnemy) {
                            dataGridView1.Rows[dataGridView1.RowCount - 2].DefaultCellStyle.BackColor = Color.Lime;
                        }
                        else {
                            dataGridView1.Rows[dataGridView1.RowCount - 2].DefaultCellStyle.BackColor = Color.FromArgb(192, 0, 0);
                        }
                    }
                }

            }
        }
    }
}
