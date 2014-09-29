using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ortoxela.Reportes.Requisiciones_Ajustes
{
    public partial class F_impresion : Form
    {
        public F_impresion()
        {
            InitializeComponent();
        }

        public void impresionreqyaju(int id_se, string f_i, string f_f)
        {
            /*
             * DataTable res = new DataTable();
            DataSet1TableAdapters.ListadeProductosenBodegaTableAdapter lg = new DataSet1TableAdapters.ListadeProductosenBodegaTableAdapter();
            R_productosenbodega reporte = new R_productosenbodega();
            res = lg.GetData_listadeproductosenbodega(nomBo);
            reporte.SetDataSource(res);
            crystalReportViewer_reportes.ReportSource = reporte;
             */
            DateTime dfi =Convert.ToDateTime(f_i);
            DateTime dff= Convert.ToDateTime(f_f);
            DataTable res = new DataTable();
            DataSet_req_ajuTableAdapters.datosTableAdapter lg = new DataSet_req_ajuTableAdapters.datosTableAdapter();
            R_req_aju reporte = new R_req_aju();
            res = lg.GetData_reqajuentrefechas(id_se, dfi, dff);
            reporte.SetDataSource(res);
            reporte.SetParameterValue("nombreEmpresa", clases.ClassVariables.nombreEmpresa);
            crystalReportViewer1.ReportSource = reporte;

        }



        private void F_impresion_Load(object sender, EventArgs e)
        {

        }
    }
}
