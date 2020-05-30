using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace External_sorting
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Amount of elements"; //текст в шапке
            column1.Name = "amoubt"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Random file";
            column2.Name = "random";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Reverse file";
            column3.Name = "reverse";
            column3.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView.Columns.Add(column1);
            dataGridView.Columns.Add(column2);
            dataGridView.Columns.Add(column3);
        }

        private void addDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogForm form = new DialogForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                Handler.NumberCount = form.Amount;
                string random = "rand";
                string reverse = "rev";
                Handler.MakeRandomFile(random);
                Handler.MakeReverseFile(random, reverse);
                Handler.SortFile(random);
                string randPas = Handler.time.TotalMilliseconds.ToString() + " msec";
                Handler.SortFile(reverse);
                string revPas = Handler.time.TotalMilliseconds.ToString() + " msec";
                dataGridView.Rows.Add(Handler.NumberCount, randPas, revPas);
            }
        }
    }
}
