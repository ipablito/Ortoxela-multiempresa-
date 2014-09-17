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
    public partial class frm_RepVueltos : DevExpress.XtraEditors.XtraForm
    {
        public frm_RepVueltos()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if(radioGroup1.SelectedIndex==0)
            {
                xtr_vueltos vueltos = new xtr_vueltos();
                vueltos.Parameters["Estado"].Value = 4;
                vueltos.Parameters["Nombre"].Value = "Vueltos pendientes de dar";
                //vueltos.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                vueltos.RequestParameters = false;
                
                vueltos.ShowPreviewDialog();
            }
            else
            {
                xtr_vueltos vueltos = new xtr_vueltos();
                vueltos.Parameters["Estado"].Value = 5;
                vueltos.Parameters["Nombre"].Value = "Vueltos Operados";
                vueltos.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                vueltos.RequestParameters = false;                
                vueltos.ShowPreviewDialog();
            }
        }

        private void frm_RepVueltos_Load(object sender, EventArgs e)
        {
            this.Text = "Reportes de Vueltos - " + clases.ClassVariables.nombreEmpresa; 
        }
    }
}