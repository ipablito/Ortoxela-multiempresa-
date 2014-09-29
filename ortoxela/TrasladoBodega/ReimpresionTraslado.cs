using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraReports.UI;
using MySql.Data.MySqlClient;

namespace ortoxela.TrasladoBodega
{
    public partial class ReimpresionTraslado : Form
    {
        public ReimpresionTraslado()
        {
            InitializeComponent();
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                DataTable nt = new DataTable();
                ad.Fill(nt);

                int fila = dataGridView_traslados.CurrentRow.Index;
                PrintTraslado.XtraReportTraslado reporte = new PrintTraslado.XtraReportTraslado();
                reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                reporte.Parameters["ID"].Value = Convert.ToInt16(dataGridView_traslados[0, fila].Value);
                reporte.RequestParameters = false;
                reporte.ShowPreviewDialog();
            }
            catch
            { }
            this.Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintTraslado.DataSet_ReimpresionTrasladoTableAdapters.ListaTrasladosTableAdapter lg = new PrintTraslado.DataSet_ReimpresionTrasladoTableAdapters.ListaTrasladosTableAdapter();
            DateTime t1, t2;
            t1 = Convert.ToDateTime(dateEdit1.DateTime.ToString("yyyy-MM-dd"));
            t2 = Convert.ToDateTime(dateEdit2.DateTime.ToString("yyyy-MM-dd"));
            dataGridView_traslados.DataSource = lg.GetData_listadetraslados(t1, t2);
        }

        private void ReimpresionTraslado_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime now = DateTime.Now;

                //fecha final
                string date = now.GetDateTimeFormats('d')[0];
                this.dateEdit2.EditValue = date;
                this.dateEdit1.EditValue = date;

            }
            catch
            { }
        }
    }
}
