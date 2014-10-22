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

namespace ortoxela.Reportes.Importaciones
{
    public partial class f_importacion : DevExpress.XtraEditors.XtraForm
    {
        public f_importacion()
        {
            InitializeComponent();
        }
        classortoxela logicaxela = new classortoxela();
        //variables
        string ListaSeries = "";
        string ListaNombresSeries = "";
        string ListaBodegas = "";
        //

        private void f_importacion_Load(object sender, EventArgs e)
        {
            try
            {
                string ssql = "SELECT codigo_proveedor AS CODIGO,nombre_proveedor AS NOMBRE FROM proveedores WHERE estadoid<>2";
                gridLookProveedor.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookProveedor.Properties.DisplayMember = "NOMBRE";
                gridLookProveedor.Properties.ValueMember = "CODIGO";

            }
            catch
            { }

            

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

            //Listando bodegas
            string ListaBods = "0";
            string ListaNombBods = "";

            for (int cnt = 0; cnt < listBoxBodegas.Items.Count; cnt++)
            {
                DataRowView srs = listBoxBodegas.Items[cnt] as DataRowView;
                ListaBods += "," + srs["codigo_bodega"].ToString();
                ListaNombBods += srs["nombre_bodega"].ToString() + " ,";
            }

            try
            {
                string getsers = "SELECT distinct codigo_serie, CONCAT(nombre_documento,' [', serie_documento,']')  AS DOCUMENTO  FROM v_bodegas_series_usuarios  WHERE codigo_tipo=16 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_bodega in (" + ListaBods + ")";
                listBoxSeries.DataSource = logicaxela.Tabla(getsers);
                listBoxSeries.DisplayMember = "DOCUMENTO";
                listBoxSeries.ValueMember = "codigo_serie";
            }
            catch
            { }

            /* FECHAS */
            try
            {
                DateTime now = DateTime.Now;

                //fecha final
                string date = now.GetDateTimeFormats('d')[0];
                this.dateEdit2.EditValue = date;


                DateTime now2 = DateTime.Now.AddMonths(-1);
                //fecha inicial
                string date2 = now2.ToShortDateString();
                this.dateEdit1.EditValue = date2;

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
                        " WHERE codigo_tipo=16 AND codigo_bodega IN (" + ListaBodegas + " )";
            this.listBoxSeries.DataSource = logicaxela.Tabla(ssql);
            this.listBoxSeries.DisplayMember = "documento";
            this.listBoxSeries.ValueMember = "codigo_serie";
        }


        private string getSelCodSers()
        {
            string ListaSers = "0";
            string ListaNombSers = "";

            for (int cnt = 0; cnt < listBoxSeries.SelectedItems.Count; cnt++)
            {
                DataRowView srs = listBoxSeries.SelectedItems[cnt] as DataRowView;
                ListaSers += "," + srs["codigo_serie"].ToString();
                ListaNombSers += srs["Documento"].ToString() + " ,";
            }
            return (ListaSers);
        }
        private string getSelCodBods()
        {
            string ListaBods = "0";
            string ListaNombBods = "";

            for (int cnt = 0; cnt < listBoxBodegas.SelectedItems.Count; cnt++)
            {
                DataRowView srs = listBoxBodegas.SelectedItems[cnt] as DataRowView;
                ListaBods += "," + srs["codigo_bodega"].ToString();
                ListaNombBods += srs["nombre_bodega"].ToString() + " ,";
            }
            return (ListaBods);
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region reporte1
                if (getSelCodBods() == "0" || getSelCodSers() == "0")
                {
                    MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
                }

                else
                {
                    string QueryCompras = "select * from v_importaciones_detalle_proveedor_categoria " +
                        "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                        "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";



                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_compras_detalle_proveedor_categoria");

                    if (datasetx.Tables[0].Rows.Count < 1)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Reporte vacio");
                        return;
                    }

                    Reportes.Compras.XtraReport_Compras_por_Categoria reportecat = new Reportes.Compras.XtraReport_Compras_por_Categoria();
                    reportecat.DataSource = datasetx;
                    reportecat.DataMember = datasetx.Tables["v_compras_detalle_proveedor_categoria"].TableName;
                    reportecat.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reportecat.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reportecat.Parameters["tipo"].Value = "Reporte de Importaciones por Linea (Categoria)";
                    reportecat.Parameters["Empresa"].Value = clases.ClassVariables.nombreEmpresa;



                    string LiNomSe = "";

                    for (int cnt = 0; cnt < listBoxSeries.SelectedItems.Count; cnt++)
                    {
                        DataRowView srs = listBoxSeries.SelectedItems[cnt] as DataRowView;
                        LiNomSe += srs["Documento"].ToString() + " ,";
                    }

                    reportecat.Parameters["dseries"].Value = LiNomSe;

                    reportecat.RequestParameters = false;
                    reportecat.ShowPreview();
                }
                #endregion
            }
            catch
            { }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region reporte2
                if (getSelCodBods() == "0" || getSelCodSers() == "0")
                {
                    MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
                }

                else
                {
                    string QueryCompras = "select * from v_importaciones " +
                        "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                        "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
                    //
                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_compras");


                    if (datasetx.Tables[0].Rows.Count < 1)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Reporte vacio");
                        return;
                    }

                    Reportes.Compras.XtraReport_x_NoCompra reportecat = new Compras.XtraReport_x_NoCompra();
                    reportecat.DataSource = datasetx;
                    reportecat.DataMember = datasetx.Tables["v_compras"].TableName;
                    reportecat.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reportecat.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reportecat.Parameters["Empresa"].Value = clases.ClassVariables.nombreEmpresa;
                    reportecat.Parameters["tipo"].Value = "REPORTE DE IMPORTACIONES DETALLADO";



                    string LiNomSe = "";

                    for (int cnt = 0; cnt < listBoxSeries.SelectedItems.Count; cnt++)
                    {
                        DataRowView srs = listBoxSeries.SelectedItems[cnt] as DataRowView;
                        LiNomSe += srs["Documento"].ToString() + " ,";
                    }

                    reportecat.Parameters["dseries"].Value = LiNomSe;

                    reportecat.RequestParameters = false;
                    reportecat.ShowPreview();
                    //

                }
                #endregion
            }
            catch { }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region reporte3

                if (getSelCodBods() == "0" || getSelCodSers() == "0")
                {
                    MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
                }

                else
                {
                    string QueryCompras = "select * from v_importaciones_general " +
                        "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                        " AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
                    //
                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_compras_general");


                    if (datasetx.Tables[0].Rows.Count < 1)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Reporte vacio");
                        return;
                    }

                    Reportes.Compras.XtraReport_x_Compra_X_Arti reportecat = new Compras.XtraReport_x_Compra_X_Arti();
                    reportecat.DataSource = datasetx;
                    reportecat.DataMember = datasetx.Tables["v_compras_general"].TableName;
                    reportecat.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reportecat.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reportecat.Parameters["Empresa"].Value = clases.ClassVariables.nombreEmpresa;
                    reportecat.Parameters["tipo"].Value = "REPORTE DE IMPORTACIONES GENERAL";


                    string LiNomSe = "";

                    for (int cnt = 0; cnt < listBoxSeries.SelectedItems.Count; cnt++)
                    {
                        DataRowView srs = listBoxSeries.SelectedItems[cnt] as DataRowView;
                        LiNomSe += srs["Documento"].ToString() + " ,";
                    }

                    reportecat.Parameters["dseries"].Value = LiNomSe;

                    reportecat.RequestParameters = false;
                    reportecat.ShowPreview();

                }
                #endregion
            }
            catch
            { }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region reporte4

                if (getSelCodBods() == "0" || getSelCodSers() == "0")
                {
                    MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
                }

                else
                {
                    string QueryCompras = "select * from v_importaciones_proveedor " +
                        "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                        "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
                    //
                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_compras_proveedor");

                    MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                    DataTable nt = new DataTable();
                    ad.Fill(nt);

                    if (datasetx.Tables[0].Rows.Count < 1)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Reporte vacio");
                        return;
                    }

                    ortoxela.Reportes.Proveedores.XtraReport_RepProveedores reporteP = new ortoxela.Reportes.Proveedores.XtraReport_RepProveedores();
                    reporteP.DataSource = datasetx;
                    reporteP.DataMember = datasetx.Tables["v_compras_proveedor"].TableName;
                    reporteP.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reporteP.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reporteP.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                    reporteP.Parameters["tipo"].Value = "REPORTE DE IMPORTACIONES TODOS LOS PROVEEDORES";
                    reporteP.RequestParameters = false;
                    reporteP.ShowPreview();
                }
                #endregion
            }
            catch { }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region reporte5


                if (getSelCodBods() == "0" || getSelCodSers() == "0" || gridLookProveedor.EditValue.ToString() == "SELECCIONE PROVEEDOR")
                {
                    MessageBox.Show("Por favor seleccione la bodega, la serie y el proveedor para generar el reporte");
                }

                else
                {
                    string QueryCompras = "select * from v_importaciones_proveedor " +
                        "where codigo_proveedor=" + gridLookProveedor.EditValue.ToString() + " and codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                        "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";

                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_compras_proveedor");

                    if (datasetx.Tables[0].Rows.Count == 0)
                    {
                        clases.ClassMensajes.customessage(this, "El reporte esta vacio");
                        Cursor = Cursors.Default;
                        return;
                    }


                    MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                    DataTable nt = new DataTable();
                    ad.Fill(nt);

                    if (datasetx.Tables[0].Rows.Count < 1)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Reporte vacio");
                        return;
                    }

                    ortoxela.Reportes.Proveedores.XtraReport_RepUnProveedor reporteP = new ortoxela.Reportes.Proveedores.XtraReport_RepUnProveedor();
                    reporteP.DataSource = datasetx;
                    reporteP.DataMember = datasetx.Tables["v_compras_proveedor"].TableName;
                    reporteP.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reporteP.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reporteP.Parameters["Codigo_proveedor"].Value = gridLookProveedor.EditValue;
                    reporteP.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                    reporteP.Parameters["tipo"].Value = "REPORTE DE IMPORTACIONES PROVEEDOR:";

                    reporteP.RequestParameters = false;
                    reporteP.ShowPreview();


                }
                #endregion
            }
            catch { }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
        }
    }
}