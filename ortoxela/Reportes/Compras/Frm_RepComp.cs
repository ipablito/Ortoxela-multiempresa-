using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql;
using MySql.Data.MySqlClient;

using DevExpress.XtraReports.UI;

namespace ortoxela.Reportes.Compras
{
    public partial class Frm_RepComp : DevExpress.XtraEditors.XtraForm
    {
        public Frm_RepComp()
        {
            InitializeComponent();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }


        //variables
        string ListaSeries = "";
        string ListaNombresSeries = "";
        string ListaBodegas = "";
        //

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (getSelCodBods() == "0" || getSelCodSers() == "0")
            {
                MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
            }

            else
            {
                string QueryCompras = "select * from v_compras " +
                    "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                    "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
                //
                MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                DataSet datasetx = new DataSet();
                adaptadorx.Fill(datasetx, "v_compras");

                
                XtraReport_x_NoCompra reportecat = new XtraReport_x_NoCompra();
                reportecat.DataSource = datasetx;
                reportecat.DataMember = datasetx.Tables["v_compras"].TableName;
                reportecat.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                reportecat.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
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
                //

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

            if (getSelCodBods() == "0" || getSelCodSers() == "0")
            {
                MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
            }

            else
            {
                string QueryCompras = "select * from v_compras_general " +
                    "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                    " AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
                //
                MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                DataSet datasetx = new DataSet();
                adaptadorx.Fill(datasetx, "v_compras_general");



                XtraReport_x_Compra_X_Arti reportecat = new XtraReport_x_Compra_X_Arti();
                reportecat.DataSource = datasetx;
                reportecat.DataMember = datasetx.Tables["v_compras_general"].TableName;
                reportecat.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                reportecat.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
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
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

            if (getSelCodBods() == "0" || getSelCodSers() == "0")
            {
                MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
            }

            else
            {
                string QueryCompras = "select * from v_compras_proveedor " +
                    "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                    "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
                //
                MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                DataSet datasetx = new DataSet();
                adaptadorx.Fill(datasetx, "v_compras_proveedor");
                


                ortoxela.Reportes.Proveedores.XtraReport_RepProveedores reporteP = new ortoxela.Reportes.Proveedores.XtraReport_RepProveedores();
                reporteP.DataSource = datasetx;
                reporteP.DataMember = datasetx.Tables["v_compras_proveedor"].TableName;
                reporteP.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                reporteP.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                
                reporteP.RequestParameters = false;
                reporteP.ShowPreview();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (getSelCodBods() == "0" || getSelCodSers() == "0" || gridLookProveedor.EditValue.ToString()== "SELECCIONE PROVEEDOR")
            {
                MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
            }

            else
            {
                string QueryCompras = "select * from v_compras_proveedor " +
                    "where codigo_proveedor=" + gridLookProveedor.EditValue.ToString() + " and codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                    "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";

                MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                DataSet datasetx = new DataSet();
                adaptadorx.Fill(datasetx, "v_compras_proveedor");

                

                    ortoxela.Reportes.Proveedores.XtraReport_RepUnProveedor reporteP = new ortoxela.Reportes.Proveedores.XtraReport_RepUnProveedor();
                    reporteP.DataSource = datasetx;
                    reporteP.DataMember = datasetx.Tables["v_compras_proveedor"].TableName;
                    reporteP.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reporteP.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reporteP.Parameters["Codigo_proveedor"].Value = gridLookProveedor.EditValue;

                    reporteP.RequestParameters = false;
                    reporteP.ShowPreview();
                
                
            }
        }
        
        classortoxela logicaxela = new classortoxela();
        
        private void Frm_RepComp_Load(object sender, EventArgs e)
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
                string getsers = "SELECT distinct codigo_serie, CONCAT(nombre_documento,' [', serie_documento,']')  AS DOCUMENTO  FROM v_bodegas_series_usuarios  WHERE codigo_tipo=6 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_bodega in (" + ListaBods + ")";
                listBoxSeries.DataSource = logicaxela.Tabla(getsers);
                listBoxSeries.DisplayMember = "DOCUMENTO";
                listBoxSeries.ValueMember = "codigo_serie";
            }
            catch
            {}

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

        private void simpleButton5_Click(object sender, EventArgs e)
        {



            if (getSelCodBods() == "0" || getSelCodSers() == "0")
            {
                MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
            }

            else
            {
                string QueryCompras = "select * from v_doctos_detalle " +
                    "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                    "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
                //
                MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                DataSet datasetx = new DataSet();
                adaptadorx.Fill(datasetx, "v_doctos_detalle");


                XtraReport_Requisiciones_Detalle reporteP = new XtraReport_Requisiciones_Detalle();
                reporteP.DataSource = datasetx;
                reporteP.DataMember = datasetx.Tables["v_doctos_detalle"].TableName;
                reporteP.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                reporteP.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                reporteP.Parameters["Empresa"].Value = clases.ClassVariables.nombreEmpresa;

                reporteP.RequestParameters = false;
                reporteP.ShowPreview();
            }
            
            
            
            
            //XtraReport_Requisiciones_Detalle reporteRQ = new XtraReport_Requisiciones_Detalle();
            //reporteRQ.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
            //reporteRQ.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
            //reporteRQ.Parameters["Empresa"].Value = clases.ClassVariables.nombreEmpresa;
            
            //reporteRQ.RequestParameters = false;
            //reporteRQ.ShowPreview();
        }

        private void labelControl17_Click(object sender, EventArgs e)
        {
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

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (getSelCodBods() == "0" || getSelCodSers() == "0")
            {
                MessageBox.Show("Por favor seleccione la bodega y la serie para generar el reporte");
            }

            else
            {
                string QueryCompras = "select * from v_compras_detalle_proveedor_categoria " +
                    "where codigo_serie in (" + getSelCodSers() + ") and codigo_bodega in (" + getSelCodBods() + ")" +
                    "AND fecha_compra between'" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";



                MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryCompras, Properties.Settings.Default.ortoxelaConnectionString);
                DataSet datasetx = new DataSet();
                adaptadorx.Fill(datasetx, "v_compras_detalle_proveedor_categoria");

                    XtraReport_Compras_por_Categoria reportecat = new XtraReport_Compras_por_Categoria();
                    reportecat.DataSource = datasetx;
                    reportecat.DataMember = datasetx.Tables["v_compras_detalle_proveedor_categoria"].TableName;
                    reportecat.Parameters["Fecha_inicio"].Value = dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reportecat.Parameters["Fecha_fin"].Value = dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
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
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void panelControl6_Paint(object sender, PaintEventArgs e)
        {

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
                        " WHERE codigo_tipo=6 AND codigo_bodega IN (" + ListaBodegas + " )";
            this.listBoxSeries.DataSource = logicaxela.Tabla(ssql);
            this.listBoxSeries.DisplayMember = "documento";
            this.listBoxSeries.ValueMember = "codigo_serie";
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridLookProveedor_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}