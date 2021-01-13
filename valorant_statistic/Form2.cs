using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace valorant_statistic
{
    public partial class addSeasonForm : Form
    {
        private Form1 _owner;
        public addSeasonForm(Form1 owner) {
            InitializeComponent();
            _owner = owner;
            this.BackgroundImage = owner.BackgroundImage;
            this.BackgroundImageLayout = owner.BackgroundImageLayout;
            this.FormBorderStyle = owner.FormBorderStyle;
            this.Icon = owner.Icon;

        }

        private void button1_Click(object sender, EventArgs e) {
            _owner.seasonName = textBox1.Text;
            this.Close();
        }

        private void addSeasonForm_FormClosed(object sender, FormClosedEventArgs e) {
            _owner.seasonAdded();
        }
    }
}
