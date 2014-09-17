using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

using DevExpress.XtraReports.UI;

namespace ortoxela.ModCobranza.Reporte.Proveedores
{
    public partial class frm_reportes_pagos_a_proveedores : Form
    {
        public frm_reportes_pagos_a_proveedores()
        {
            InitializeComponent();
        }

        //Variables Globales
        string consulta;
        int id_proveedor;
        classortoxela logicaorto = new classortoxela();

        private void textNombreProveedor_KeyPress(object sender, KeyPressEventArgs e)
        {
            consulta = "SELECT codigo_proveedor AS 'CODIGO',nombre_proveedor AS 'NOMBRE DE PROVEEDOR',nit AS 'NIT',telefono_principal AS 'TELEFONO DE OFICINA' FROM proveedores WHERE estadoid<>2";
            clases.ClassVariables.cadenabusca = consulta;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                DataTable tempCliente = new DataTable();
                id_proveedor = int.Parse(clases.ClassVariables.id_busca);
                consulta = "SELECT codigo_proveedor AS 'CODIGO', nombre_proveedor AS 'NOMBRE PROVEEDOR', nit, telefono_principal, telefono_celular,email FROM proveedores WHERE codigo_proveedor=" + id_proveedor;
                tempCliente = logicaorto.Tabla(consulta);
                textNombreProveedor.Text = tempCliente.Rows[0]["NOMBRE PROVEEDOR"].ToString();
            }
            e.KeyChar = Convert.ToChar(13);
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 1)
            {
                textNombreProveedor.Enabled = true;
            }
            else
            {
                textNombreProveedor.Enabled = false;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Boolean faltandatos = false;

            consulta = "SELECT * from v_factura_credito_pendiente_proveedores where FECHA BETWEEN '" + dateEditInicial.DateTime.ToString("yyyy-MM-dd") + "' and '" + dateEditFinal.DateTime.ToString("yyyy-MM-dd") + "'";

            if (radioGroup1.SelectedIndex == 1)
            {
                if (textNombreProveedor.Text != "")
                {
                    consulta += " AND codigo_proveedor=" + id_proveedor;
                }
                else
                {
                    clases.ClassMensajes.FaltanDatosEnCampos(this);
                    faltandatos = true;
                }
            }

            MySqlDataAdapter da = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
            da.Fill(dt);

            if (faltandatos == false)
            {

                if (dt.Rows.Count > 0)
                {
                    DataSet datset = new DataSet();
                    da.Fill(datset, "v_factura_credito_pendiente_proveedores");
                    XtraReport_Facturas_Pendientes_Proveedores reporte = new XtraReport_Facturas_Pendientes_Proveedores();
                    reporte.DataSource = datset;
                    reporte.DataMember = datset.Tables["v_factura_credito_pendiente_proveedores"].TableName;
                    reporte.Parameters["Fecha_inicio"].Value = dateEditInicial.DateTime.ToString("yyyy-MM-dd");
                    reporte.Parameters["Fecha_fin"].Value = dateEditFinal.DateTime.ToString("yyyy-MM-dd");
                    reporte.RequestParameters = false;
                    reporte.ShowPreviewDialog();
                }
                else
                {
                    clases.ClassMensajes.NoHayInformacionCriterio(this);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Boolean faltandatos=false;

            consulta = "SELECT        codigo_proveedor, nitProveedor, nombre_proveedor, Factura, no_factura, monto_neto, saldo_factura, fecha_operacion, docto_abono, no_recibo, monto_abono, refer_documento " +
                          "FROM v_abonos_a_proveedores where fecha_operacion >= '" + dateEditInicial.DateTime.ToString("yyyy-MM-dd") + " 00:00:00 ' and  fecha_operacion <='" + dateEditFinal.DateTime.ToString("yyyy-MM-dd") + " 23:59:59'  ";

            if (radioGroup1.SelectedIndex == 1)
            {
                if (textNombreProveedor.Text != "")
                {
                    consulta += " AND codigo_proveedor=" + id_proveedor;
                }
                else
                {
                    clases.ClassMensajes.FaltanDatosEnCampos(this);
                    faltandatos = true;
                }           
            }

            MySqlDataAdapter da = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
            da.Fill(dt);

            if (faltandatos == false)
            {

                if (dt.Rows.Count > 0)
                {
                    DataSet dataset1 = new DataSet();
                    da.Fill(dataset1, "v_abonos_a_proveedores");
                    XtraReport_Abonos_Proveedores reported = new XtraReport_Abonos_Proveedores();
                    reported.DataSource = dataset1;
                    reported.DataMember = dataset1.Tables["v_abonos_a_proveedores"].TableName;
                    reported.Parameters["Fecha_inicio"].Value = dateEditInicial.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reported.Parameters["Fecha_fin"].Value = dateEditFinal.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reported.RequestParameters = false;
                    reported.ShowPreview();
                }
                else
                {
                    clases.ClassMensajes.NoHayInformacionCriterio(this);
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Boolean faltandatos = false;

            consulta = "SELECT * from v_factura_credito_cancelada_proveedores where FECHA BETWEEN '" + dateEditInicial.DateTime.ToString("yyyy-MM-dd") + "' and '" + dateEditFinal.DateTime.ToString("yyyy-MM-dd") + "'";

            if (radioGroup1.SelectedIndex == 1)
            {
                if (textNombreProveedor.Text != "")
                {
                    consulta += " AND codigo_proveedor=" + id_proveedor;
                }
                else
                {
                    clases.ClassMensajes.FaltanDatosEnCampos(this);
                    faltandatos = true;
                }
            }


            MySqlDataAdapter da = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
            da.Fill(dt);

            if (faltandatos == false)
            {

                if (dt.Rows.Count > 0)
                {
                    DataSet datset = new DataSet();
                    da.Fill(datset, "v_factura_credito_cancelada_proveedores");
                    XtraReport_Facturas_Canceladas_Proveedores reporte = new XtraReport_Facturas_Canceladas_Proveedores();
                    reporte.DataSource = datset;
                    reporte.DataMember = datset.Tables["v_factura_credito_cancelada_proveedores"].TableName;
                    reporte.Parameters["Fecha_inicio"].Value = dateEditInicial.DateTime.ToString("yyyy-MM-dd");
                    reporte.Parameters["Fecha_fin"].Value = dateEditFinal.DateTime.ToString("yyyy-MM-dd");
                    reporte.RequestParameters = false;
                    reporte.ShowPreviewDialog();
                }
                else
                {
                    clases.ClassMensajes.NoHayInformacionCriterio(this);
                }
            }
        }

        private void dateEditInicial_EditValueChanged(object sender, EventArgs e)
        {
            if ((dateEditInicial.EditValue != null) && (dateEditFinal.EditValue != null))
            {
                simpleButton1.Enabled = true;
                simpleButton2.Enabled = true;
                simpleButton3.Enabled = true;
            }
            else
            {
                simpleButton1.Enabled = false;
                simpleButton2.Enabled = false;
                simpleButton3.Enabled = false;
            }
        }

        private void dateEditFinal_EditValueChanged(object sender, EventArgs e)
        {
            if ((dateEditInicial.EditValue != null) && (dateEditFinal.EditValue != null))
            {
                simpleButton1.Enabled = true;
                simpleButton2.Enabled = true;
                simpleButton3.Enabled = true;
            }
            else
            {
                simpleButton1.Enabled = false;
                simpleButton2.Enabled = false;
                simpleButton3.Enabled = false;
            }
        }

        private void frm_reportes_pagos_a_proveedores_Load(object sender, EventArgs e)
        {
            if (clases.ClassVariables.op_reporte == 1)
            {
                panelControl1.Visible = true;
                panelControl2.Visible = false;
                panelControl3.Visible = false;
            }
            else if (clases.ClassVariables.op_reporte == 2)
            {
                panelControl1.Visible = false;
                panelControl2.Visible = true;
                panelControl3.Visible = false;
                panelControl2.Location = new Point(12, 129);
            }
            else if (clases.ClassVariables.op_reporte == 3)
            {
                panelControl1.Visible = false;
                panelControl2.Visible = false;
                panelControl3.Visible = true;
                panelControl3.Location = new Point(12, 129);
            }
        }

    
    }
}
