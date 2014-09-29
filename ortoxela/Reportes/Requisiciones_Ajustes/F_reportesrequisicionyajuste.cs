using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraReports.UI;

namespace ortoxela.Reportes.Requisiciones_Ajustes
{
    public partial class F_reportesrequisicionyajuste : Form
    {
        public F_reportesrequisicionyajuste()
        {
            InitializeComponent();
        }

        private void F_reportesrequisicionyajuste_Load(object sender, EventArgs e)
        {
            DataSet_req_ajuTableAdapters.seriesreqyajuTableAdapter lg = new DataSet_req_ajuTableAdapters.seriesreqyajuTableAdapter();
            lookUpEditTipoDocumento.Properties.ValueMember = "codigo_serie";
            lookUpEditTipoDocumento.Properties.DisplayMember = "nombre_documento";
            lookUpEditTipoDocumento.Properties.DataSource = lg.GetData_tiposseriesamostrar();

            lookUpEdit_tipodocumento2.Properties.ValueMember = "codigo_serie";
            lookUpEdit_tipodocumento2.Properties.DisplayMember = "nombre_documento";
            lookUpEdit_tipodocumento2.Properties.DataSource = lg.GetData_tiposseriesamostrar();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(dateTimePicker1.Value.ToShortDateString());
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if ((lookUpEditTipoDocumento.Text != "") && (lookUpEditTipoDocumento.Text != "Seleccione un tipo de documento"))
            {
                F_impresion nf = new F_impresion();
                nf.impresionreqyaju(Convert.ToInt16(lookUpEditTipoDocumento.EditValue), dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString());
                nf.ShowDialog();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if ((lookUpEdit_tipodocumento2.Text != "") && (lookUpEdit_tipodocumento2.Text != "Seleccione un tipo de documento") && (textEdit1.Text != ""))
                {
                    DataSet_req_ajuTableAdapters.header_doctos_invTableAdapter lg = new DataSet_req_ajuTableAdapters.header_doctos_invTableAdapter();
                    int val = Convert.ToInt16(lg.GetData_hayaridheader(Convert.ToInt16(lookUpEdit_tipodocumento2.EditValue), Convert.ToInt16(textEdit1.Text)).Rows[0][0]);

                    Compra.PrintIngresoProd.XtraReportIngresoProd reporte = new Compra.PrintIngresoProd.XtraReportIngresoProd();
                    reporte.Parameters["ID"].Value = val;
                    reporte.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                    reporte.RequestParameters = false;
                    reporte.ShowPreviewDialog();
                }
            }
            catch
            {

            }
            this.Cursor = Cursors.Default;
        }
    }
}
