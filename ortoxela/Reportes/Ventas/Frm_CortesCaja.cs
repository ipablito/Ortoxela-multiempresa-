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

namespace ortoxela.Reportes.Ventas
{
    public partial class Frm_CortesCaja : Form
    {
        public Frm_CortesCaja()
        {
            InitializeComponent();
        }

        string ListaSeries = "";
        string ListaNombresSeries = "";
        string ListaBodegas = "";
        /* Mensaje de error al validar Fechas. */
        string datoMensajeError = "FECHA(S) INVALIDA(S)";
        /* Mensaje de error al validar Series. */
        string datoMensajeErrorSeries = "DEBE SELECCIONAR AL MENOS UNA SERIE ";

        classortoxela logicaxela = new classortoxela();

        private Boolean validarFechas()
        {
            if ((FechaInicio.DateTime.ToString("yyyy-MM-dd") == "0001-01-01") || (FechaFin.DateTime.ToString("yyyy-MM-dd") == "0001-01-01"))
            {
                clases.ClassMensajes.customessage(this, datoMensajeError);
                return false;
            }
            else
                return true;
        }

        private Boolean validarSeries()
        {
            if (this.listBoxSeries.SelectedItems.Count > 0)
            {
                return true;
            }
            else
            {
                clases.ClassMensajes.customessage(this, datoMensajeErrorSeries);
                return false;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.validarFechas())
            {
                if (validarSeries())
                {
                    ListaSeries = "0";
                    ListaNombresSeries = "";
                    for (int cnt = 0; cnt < listBoxSeries.SelectedItems.Count; cnt++)
                    {
                        DataRowView srs = listBoxSeries.SelectedItems[cnt] as DataRowView;
                        ListaSeries += "," + srs["codigo_serie"].ToString();
                        ListaNombresSeries += srs["documento"].ToString() + " ,";
                    }
                    /**/

                    string QueryVtas = "SELECT        fecha, Tipo_Pago, nombre_cliente, descuentoPct, DescuentoQ, total_iva, Total_sin_iva, no_documento, refer_documento, nombre_tipo_pago, documento,  " +
                             " nombre_estado, fecha_anula, usuario_anula, Socio_Comercial, tipo_cliente, nitCliente, codigo_cliente, codigo_serie FROM            v_ventas_general  v " +
                             " where codigo_serie in (" + ListaSeries + ") " +
                             " and fecha between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" +
                            FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59'";

                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryVtas, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_ventas_general");
                   
                    XtraReport_Corte_Caja reportec = new XtraReport_Corte_Caja();
                    reportec.DataSource = datasetx;
                    reportec.DataMember = datasetx.Tables["v_ventas_general"].TableName;
                    reportec.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reportec.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reportec.Parameters["Series"].Value = ListaNombresSeries;
                    reportec.Parameters["NombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                    
                    reportec.RequestParameters = false;
                    reportec.ShowPreview();
                }
            }
            // else MessageBox.Show("Debe ingresar un Rango de Fechas!", "Advertencia");

            Cursor.Current = Cursors.Default;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.validarFechas())
            {
                if (validarSeries())
                {
                    ListaSeries = "0";
                    ListaNombresSeries = "";
                    for (int cnt = 0; cnt < listBoxSeries.SelectedItems.Count; cnt++)
                    {
                        DataRowView srs = listBoxSeries.SelectedItems[cnt] as DataRowView;
                        ListaSeries += "," + srs["codigo_serie"].ToString();
                        ListaNombresSeries += srs["documento"].ToString() + " ,";
                    }
                    /**/

                    string QueryVtas = "SELECT        r.no_recibo, r.fecha_creacion, r.monto_recibo, r.vale, r.factura, r.nombre_documento, r.serie_documento, r.nombre_cliente, r.estadoid, p.nombre_tipo_pago "+
                            /*",r.socio_comercial*/ "FROM            v_recibos r INNER JOIN  tipo_pago p ON r.tipo_pago = p.tipo_pago "+
                             " where fecha_creacion between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" +
                            FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59'";

                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryVtas, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_recibos");

                    XtraReport_CorteCajaRecibos reportec = new XtraReport_CorteCajaRecibos ();
                    reportec.DataSource = datasetx;
                    reportec.DataMember = datasetx.Tables["v_recibos"].TableName;
                    reportec.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reportec.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reportec.Parameters["Series"].Value = ListaNombresSeries;
                    reportec.Parameters["NombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                    
                    reportec.RequestParameters = false;
                    reportec.ShowPreview();
                }
            }
            
        }

        private void Frm_CortesCaja_Load(object sender, EventArgs e)
        {
            this.Text = "Cortes de Caja - " + clases.ClassVariables.nombreEmpresa; 
            /* BODEGAS */
            try
            {
                string ssql;

                /* jramirez 2013.07.24 */
                ssql = "SELECT distinct codigo_bodega, nombre_bodega FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario;

                listBoxBodegas.DataSource = logicaxela.Tabla(ssql);
                listBoxBodegas.DisplayMember = "nombre_bodega";
                listBoxBodegas.ValueMember = "codigo_bodega";

            }
            catch
            { }

            /* FECHAS */
            try
            {

                /* jramirez 2014.01.20 */

                DateTime now = DateTime.Now;

                string date = now.GetDateTimeFormats('d')[0];
                this.FechaFin.EditValue = date;

                DateTime now2 = DateTime.Now.AddMonths(-6);

                string date2 = now2.ToShortDateString();
                this.FechaInicio.EditValue = date2;

            }
            catch
            { }
        }

        private void listBoxBodegas_MouseUp(object sender, MouseEventArgs e)
        {
            ListaBodegas = "0";
            if (listBoxBodegas.SelectedItems.Count > 0)
            {

                for (int cnt = 0; cnt < listBoxBodegas.SelectedItems.Count; cnt++)
                {
                    DataRowView bdgs = listBoxBodegas.SelectedItems[cnt] as DataRowView;
                    ListaBodegas += "," + bdgs["codigo_bodega"].ToString();

                }
            }
            string ssql = " SELECT distinct codigo_serie,CONCAT(nombre_documento,'[',serie_documento,']',' Bod: ',nombre_bodega) AS documento FROM v_bodegas_series_usuarios b " +
                        " WHERE codigo_tipo=1 AND codigo_bodega IN (" + ListaBodegas + " )";
            this.listBoxSeries.DataSource = logicaxela.Tabla(ssql);
            this.listBoxSeries.DisplayMember = "documento";
            this.listBoxSeries.ValueMember = "codigo_serie";
        }

       
    }
}
