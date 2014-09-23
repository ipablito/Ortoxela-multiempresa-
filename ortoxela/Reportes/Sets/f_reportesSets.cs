using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ortoxela.Reportes.Sets
{
    public partial class f_reportesSets : DevExpress.XtraEditors.XtraForm
    {
        public f_reportesSets()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                f_imp nf = new f_imp();
                nf.ReporteSetEnPedido();
                nf.ShowDialog();
            }
            catch
            { }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (dxValidationProvider1.Validate() == true)
                {
                    string f1 = dateEdit1.Text;
                    string f2 = dateEdit2.Text;
                    f1+= " 00:00:00";
                    f2 += " 23:59:59";
                    f_imp nf = new f_imp();
                    nf.ReporteEntreFechas(f1, f2);
                    nf.ShowDialog();
                }
            }
            catch { }
            this.Cursor = Cursors.Default;
        }
    }
}