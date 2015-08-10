using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public partial class DlgFileAssociationCheck : Form
    {
        public DlgFileAssociationCheck()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            Properties.Settings.Default.CheckFileAssociation = !chkDontAsk.Checked;
            Properties.Settings.Default.Save();
            base.OnClosed(e);
        }

        private void cmdYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void cmdNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }
    }
}
