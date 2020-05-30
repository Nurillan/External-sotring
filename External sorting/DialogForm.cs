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
    public partial class DialogForm : Form
    {
        public int Amount;

        public DialogForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox.Text, out Amount) || Amount < 1 || Amount > 10000)
            {
                MessageBox.Show("Enter approptiate amount of numbers (1-10000)");
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}
