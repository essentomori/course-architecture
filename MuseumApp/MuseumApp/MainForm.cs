using System;
using System.Windows.Forms;

namespace MuseumApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnMuseums_Click(object sender, EventArgs e)
        {
            new MuseumForm().ShowDialog();
        }

        private void btnExhibits_Click(object sender, EventArgs e)
        {
            new ExhibitForm().ShowDialog();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            new ReportForm().ShowDialog();
        }
    }
}