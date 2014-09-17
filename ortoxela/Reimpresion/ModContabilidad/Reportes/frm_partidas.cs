using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraReports.UI;

namespace ortoxela.ModContabilidad.Reportes
{
    public partial class frm_partidas : Form
    {
        public frm_partidas()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraReportPartidas reporte = new XtraReportPartidas();            
            reporte.Parameters["FechaInicio"].Value = dateEditInicial.DateTime.ToString("yyyy-MM-dd");
            reporte.Parameters["FechaFin"].Value = dateEditFinal.DateTime.ToString("yyyy-MM-dd");
            reporte.RequestParameters = false;
            reporte.ShowPreviewDialog();
        }
    }
}
