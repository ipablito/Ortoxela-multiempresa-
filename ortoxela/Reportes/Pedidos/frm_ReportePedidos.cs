using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using DevExpress.XtraReports.UI;

namespace ortoxela.Reportes.Pedidos
{
    public partial class frm_ReportePedidos : DevExpress.XtraEditors.XtraForm
    {
        public frm_ReportePedidos()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraReport_x_SocioComercial reportesc = new XtraReport_x_SocioComercial();
            reportesc.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;

            reportesc.RequestParameters = false;
            reportesc.ShowPreview();
        }

        private void frm_ReportePedidos_Load(object sender, EventArgs e)
        {
            this.Text = "Reportes Pedidos/Envios - " + clases.ClassVariables.nombreEmpresa; 
        }

      
          }
}