using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;


using DevExpress.XtraReports.UI;

namespace ortoxela.ModCobranza.Reporte
{
    public partial class frm_reportes : DevExpress.XtraEditors.XtraForm
    {
        public frm_reportes()
        {
            InitializeComponent();
        }

        string consulta;
        int id_cliente;
        classortoxela logicaorto = new classortoxela();
        private void textNombreCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            consulta = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL',clientes.nit AS 'NIT',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes where estadoid<>2";
            clases.ClassVariables.cadenabusca = consulta;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                DataTable tempCliente = new DataTable();
                id_cliente = int.Parse(clases.ClassVariables.id_busca);
                consulta = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_casa,contacto FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                tempCliente = logicaorto.Tabla(consulta);
                textNombreCliente.Text= tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 1)
            {
                textNombreCliente.Enabled = true;
            }
            else
            {
                textNombreCliente.Enabled = false;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Boolean faltandatos = false;

            consulta = "SELECT * from v_factura_credito_pendiente where FECHA BETWEEN '" + dateEditInicial.DateTime.ToString("yyyy-MM-dd") + "' and '" + dateEditFinal.DateTime.ToString("yyyy-MM-dd") + "'";
            
            if (radioGroup1.SelectedIndex == 1)
            {
                if (textNombreCliente.Text != "")
                {
                    consulta += " AND codigo_cliente=" + id_cliente;
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
                    da.Fill(datset, "v_factura_credito_pendiente");
                    XtraReport_Facturas_Pendientes reporte = new XtraReport_Facturas_Pendientes();
                    reporte.DataSource = datset;
                    reporte.DataMember = datset.Tables["v_factura_credito_pendiente"].TableName;
                    reporte.Parameters["Fecha_inicio"].Value = dateEditInicial.DateTime.ToString("yyyy-MM-dd");
                    reporte.Parameters["Fecha_fin"].Value = dateEditFinal.DateTime.ToString("yyyy-MM-dd");
                    reporte.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
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
            Boolean faltandatos = false;

            consulta = "SELECT        codigo_cliente, nitCliente, nombre_cliente, Factura, no_factura, monto_neto, saldo_factura, fecha_operacion, docto_abono, no_recibo, monto_abono " +
                          "FROM v_abonos_clientes where fecha_operacion >= '" + dateEditInicial.DateTime.ToString("yyyy-MM-dd") + " 00:00:00 ' and  fecha_operacion <='" + dateEditFinal.DateTime.ToString("yyyy-MM-dd") + " 23:59:59'  ";
            
            if (radioGroup1.SelectedIndex == 1)
            {
                if (textNombreCliente.Text != "")
                {
                    consulta += " AND codigo_cliente=" + id_cliente;
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
                    da.Fill(dataset1, "v_abonos_clientes");
                    XtraReport_ReporteAbonosClientes reported = new XtraReport_ReporteAbonosClientes();
                    reported.DataSource = dataset1;
                    reported.DataMember = dataset1.Tables["v_abonos_clientes"].TableName;
                    reported.Parameters["Fecha_inicio"].Value = dateEditInicial.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reported.Parameters["Fecha_fin"].Value = dateEditFinal.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
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

            consulta = "SELECT * from v_factura_credito_cancelada where FECHA BETWEEN '" + dateEditInicial.DateTime.ToString("yyyy-MM-dd") + "' and '" + dateEditFinal.DateTime.ToString("yyyy-MM-dd") + "'";
            
            if (radioGroup1.SelectedIndex == 1)
            {
                if (textNombreCliente.Text != "")
                {
                    consulta += " AND codigo_cliente=" + id_cliente;
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
                    da.Fill(datset, "v_factura_credito_cancelada");
                    XtraReport_facturas_credito_canceladas reporte = new XtraReport_facturas_credito_canceladas();
                    reporte.DataSource = datset;
                    reporte.DataMember = datset.Tables["v_factura_credito_cancelada"].TableName;
                    reporte.Parameters["Fecha_inicio"].Value = dateEditInicial.DateTime.ToString("yyyy-MM-dd");
                    reporte.Parameters["Fecha_fin"].Value = dateEditFinal.DateTime.ToString("yyyy-MM-dd");
                    reporte.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
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

        private void frm_reportes_Load(object sender, EventArgs e)
        {
            /* FECHAS */
            try
            {
                /* jramirez  */
                DateTime now = DateTime.Now;

                string date = now.GetDateTimeFormats('d')[0];
                this.dateEditFinal.EditValue = date;

                DateTime now2 = DateTime.Now.AddMonths(-6);

                string date2 = now2.ToShortDateString();
                this.dateEditInicial.EditValue = date2;

            }
            catch
            { }
            
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
