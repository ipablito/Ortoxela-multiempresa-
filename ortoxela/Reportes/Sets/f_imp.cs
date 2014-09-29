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
    public partial class f_imp : DevExpress.XtraEditors.XtraForm
    {
        public f_imp()
        {
            InitializeComponent();
        }

        private void f_imp_Load(object sender, EventArgs e)
        {
        }


        public void ReporteSetEnPedido()
        {
            DataTable res = new DataTable();
            DataSet_ReportesTableAdapters.ReporteSetsEnPedidoTableAdapter lg = new DataSet_ReportesTableAdapters.ReporteSetsEnPedidoTableAdapter();
            r_EnPedidos reporte = new r_EnPedidos();
            res = lg.GetData();
            reporte.SetDataSource(res);
            reporte.SetParameterValue("nombreEmpresa", clases.ClassVariables.nombreEmpresa);
            crystalReportViewer1.ReportSource = reporte;
        }

        public void ReporteEntreFechas(string d1, string d2)
        {
            DateTime v1 = Convert.ToDateTime(d1);
            DateTime v2 = Convert.ToDateTime(d2);
            DataTable res = new DataTable();
            DataSet_ReportesTableAdapters.Reporte_2TableAdapter lg = new DataSet_ReportesTableAdapters.Reporte_2TableAdapter();
            r_Reporte2 reporte = new r_Reporte2();
            res = lg.GetData(v1, v2);
            reporte.SetDataSource(res);
            reporte.SetParameterValue("nombreEmpresa", clases.ClassVariables.nombreEmpresa);
            crystalReportViewer1.ReportSource = reporte;
        }
    }
}