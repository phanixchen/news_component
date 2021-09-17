using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NewsComponentMining
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bProcessNewsData_Click(object sender, EventArgs e)
        {
            ProcessNewsData f = new ProcessNewsData();

            f.ShowDialog();
        }

        private void bExportArff_Click(object sender, EventArgs e)
        {
            ExportArff f = new ExportArff();

            f.ShowDialog();
        }
    }
}