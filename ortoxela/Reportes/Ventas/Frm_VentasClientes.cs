using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraReports.UI;

namespace ortoxela.Reportes.Ventas
{
    public partial class Frm_VentasClientes : Form
    {
        public Frm_VentasClientes()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraReport_Ventas_por_Cliente reported = new XtraReport_Ventas_por_Cliente();
            reported.Parameters["Fecha_inicio"].Value = deFechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
            reported.Parameters["Fecha_fin"].Value = deFechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59"; ;
            reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
            reported.RequestParameters = false;
            reported.ShowPreview();
        }
    }
}
