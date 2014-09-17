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

namespace ortoxela.Reportes.Ventas
{
    public partial class Frm_RepVentas : DevExpress.XtraEditors.XtraForm
    {
        public Frm_RepVentas()
        {
            InitializeComponent();
        }
         classortoxela bandera = new classortoxela();

         string ListaSeries = "";
         string ListaNombresSeries = "";
         string ListaBodegas = "";
        /* Mensaje de error al validar Fechas. */
         string datoMensajeError = "FECHA(S) INVALIDA(S)";
         /* Mensaje de error al validar Series. */
         string datoMensajeErrorSeries = "DEBE SELECCIONAR AL MENOS UNA SERIE ";

         classortoxela logicaxela = new classortoxela();
         private void Frm_RepVentas_Load(object sender, EventArgs e)
         {
             this.Text = "Reportes de Ventas - " + clases.ClassVariables.nombreEmpresa; 
             /* ESTADOS FACTURAS */
             try
             {
                 string ssql = "select 0 as estadoid,'Todas' as ESTADO from dual union select 1 as estadoid,'Activas' as ESTADO from dual union select 2 as estadoid,'Anuladas' as ESTADO from dual";
                 comboBox3.DataSource = logicaxela.Tabla(ssql);
                 comboBox3.DisplayMember = "ESTADO";
                 comboBox3.ValueMember = "estadoid";

             }
             catch
             { }
            

             /* CATEGORIAS */
             try
             {
                 string ssql = "(select 0 as codigo,'Todas' as categoria from dual) union all (select  codigo_subcat as codigo,nombre_subcategoria as categoria from sub_categorias where estadoid=1 order by nombre_subcategoria asc)";
                 comboBox5.DataSource = logicaxela.Tabla(ssql);
                 comboBox5.DisplayMember = "categoria";
                 comboBox5.ValueMember = "codigo";

             }
             catch
             { }

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
                 this.FechaFin.EditValue = date ;

                 DateTime now2 = DateTime.Now.AddMonths(-1);

                 string date2 = now2.ToShortDateString();
                 this.FechaInicio.EditValue = date2;

             }
             catch
             { }
         }
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

         /* ********************************************** */
         /* ********* REPORTE DE VENTAS TOTALES ********** */
         /* ********************************************** */
         private void simpleButton4_Click(object sender, EventArgs e)
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
                         ListaNombresSeries += srs["documento"].ToString()+ " ,";
                     }

                     string QueryVtas = "SELECT        fecha, Tipo_Pago, nombre_cliente, descuentoPct, DescuentoQ, total_iva, Total_sin_iva, no_documento, refer_documento, nombre_tipo_pago, documento,  " +
                              " nombre_estado, fecha_anula, usuario_anula, Socio_Comercial, tipo_cliente, nitCliente, codigo_cliente, codigo_serie FROM            v_ventas_general  v " +
                              " where codigo_serie in (" + ListaSeries + ") " +
                              " and fecha between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00'  and '"  +
                             FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59'";

                     MySqlDataAdapter adaptadorx = new MySqlDataAdapter(QueryVtas, Properties.Settings.Default.ortoxelaConnectionString);
                     DataSet datasetx = new DataSet();
                     adaptadorx.Fill(datasetx, "v_ventas_general");
                     //
                     XtraReport_VentasTotalesPorMes reportef = new XtraReport_VentasTotalesPorMes();
                     reportef.DataSource = datasetx;
                     reportef.DataMember = datasetx.Tables["v_ventas_general"].TableName;
                     reportef.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                     reportef.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                     reportef.Parameters["Series"].Value = ListaNombresSeries;
                     reportef.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 
                     

                     reportef.RequestParameters = false;
                     reportef.ShowPreview();
                 }
             }
            
             Cursor.Current = Cursors.Default;

         }
         /* ********************************************** */
         /* ********* REPORTE DE ESTADISTICA ************* */
         /* ********************************************** */
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (this.validarFechas())
            {
                if (validarSeries())
                {
                    string consulta;
                    ListaSeries = "0";
                    ListaNombresSeries = "";
                    for (int cnt = 0; cnt < listBoxSeries.SelectedItems.Count; cnt++)
                    {
                        DataRowView srs = listBoxSeries.SelectedItems[cnt] as DataRowView;
                        ListaSeries += "," + srs["codigo_serie"].ToString();
                        ListaNombresSeries += srs["documento"].ToString() + " ,";
                    }

                    if (ListaSeries == "0")
                        ListaSeries = "";

                    //consulta = "CALL sp_estadistica_mes_fechas2('" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00','" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59','" + ListaSeries + "'); ";
                    consulta = "call sp_estadistica_mes_k1('" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00','" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59','" + ListaSeries + "');";
                    DataTable dt = new DataTable();
                    dt = logicaxela.Tabla(consulta);
                    //dataGridView1.Visible = true;
                    //dataGridView1.DataSource = dt;
                    Form_dataGridView nf = new Form_dataGridView();
                    nf.CargarGrid(dt);
                    nf.ShowDialog();
                    
                    


                    /* da error porq el reporte no es dinamico
                    MySqlDataAdapter adaptador1 = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet dataset1 = new DataSet();
                    DataTable dat = new DataTable();
                    dat = logicaxela.Tabla(consulta);
                    adaptador1.Fill(dat);
                    
                    adaptador1.Fill(dataset1, "v_estadistica_mes");
                    
                    XtraReport_Estadistica_Mes reporteem = new XtraReport_Estadistica_Mes();
                    reporteem.DataSource = dataset1;
                    
                    reporteem.DataMember = dataset1.Tables["v_estadistica_mes"].TableName;
                    
                    reporteem.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reporteem.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reporteem.Parameters["Bodega"].Value = ListaNombresSeries; //bodegas.Text;
                    reporteem.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;

                    reporteem.RequestParameters = false;
                    reporteem.ShowPreview();
                     */
                }
             }
            

             Cursor.Current = Cursors.Default;
        }

        /* ********************************************** */
        /* ********* REPORTE VENTAS GENERAL ************* */
        /* ********************************************** */
        private void simpleButton2_Click(object sender, EventArgs e)
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



                     string consulta = "select  fecha, Tipo_Pago, nombre_cliente, descuentoPct, DescuentoQ, total_iva, Total_sin_iva, no_documento, refer_documento, nombre_tipo_pago, documento,  " +
                                  "nombre_estado, fecha_anula, usuario_anula, tipo_cliente, nitCliente, codigo_cliente FROM v_ventas_general where 1=1 " +
                                 " and fecha between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59" + "' " +
                                 " and codigo_serie in (" + ListaSeries + ") ";

                     string estadoid = comboBox3.Text;
                     string estado = "";
                     if (estadoid == "Activas")
                     {
                         consulta = consulta + " and nombre_estado='Activo' ";
                         estado = "Activo";
                     }
                     else
                     {
                         if (estadoid == "Anuladas")
                         {
                             consulta = consulta + " and nombre_estado = 'Anulado' ";
                             estado = "Anulado";
                         }
                     }

                     MySqlDataAdapter adaptador1 = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);

                     DataSet dataset1 = new DataSet();
                     adaptador1.Fill(dataset1, "v_ventas_general");
                     XtraReport_VentasGeneral reportev = new XtraReport_VentasGeneral();
                     reportev.DataSource = dataset1;
                     reportev.DataMember = dataset1.Tables["v_ventas_general"].TableName;
                     reportev.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                     reportev.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                     reportev.Parameters["nombre_estado"].Value = estado;
                     reportev.Parameters["Series"].Value = ListaNombresSeries;
                     reportev.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 


                     reportev.RequestParameters = false;
                     reportev.ShowPreviewDialog();
                 }

            }
            
            Cursor.Current = Cursors.Default;
        }

        /* ********************************************** */
        /* ********* REPORTE VENTAS DETALLADO *********** */
        /* ********************************************** */
        private void simpleButton3_Click(object sender, EventArgs e)
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

                    string consultax = "select codigo_articulo, Articulo, cantidad_enviada, precio_sin_iva, precio_iva, nombre_bodega, fecha_compra, Tipo_Pago, nombre_cliente, no_documento, descuentoPct, " +
                            "DescuentoQ, total_iva, Total_sin_iva, refer_documento, documento, costo_iva, costo_sin_iva, forma_pago,codigo_serie from v_ventas_de_costo " +
                            " where  fecha_compra between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59" + "' " +
                            " and codigo_serie in (" + ListaSeries + ") ";

                    if (textEditNo.Text.Trim() != "")
                    {
                        consultax += " and no_documento =" + textEditNo.EditValue.ToString();
                    }

                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(consultax, Properties.Settings.Default.ortoxelaConnectionString);

                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_ventas_de_costo");
                    XtraReport_Ventas_Detalle reported = new XtraReport_Ventas_Detalle();
                    reported.DataSource = datasetx;
                    reported.DataMember = datasetx.Tables["v_ventas_de_costo"].TableName;
                    reported.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reported.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reported.Parameters["Series"].Value = ListaNombresSeries;
                    reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 

                    reported.RequestParameters = false;
                    reported.ShowPreview();
                }
            }
            
            Cursor.Current = Cursors.Default;
            
        }

        /* ********************************************** */
        /* ********* REPORTE ARTICULOS MAS VENDIDOS ***** */
        /* ********************************************** */
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (this.validarFechas())
            {

                //string consultax = "SELECT        codigo_articulo, Articulo, nombre_bodega, cantidad_enviada, precio_maximo_sin_iva, precio_maximo_iva, precio_minimo_sin_iva, precio_minimo_iva, " +
                // " precio_promedio_iva, precio_promedio_sin_iva, total_facturado_iva, Total_facturado_sin_iva, descuentoPct, DescuentoQ, documento, costo_iva, costo_sin_iva, " +
                // " categoria, fecha, fecha_1era_venta, fecha_ultima_venta,codigo_serie  " +
                // " FROM    v_ventas_articulo_mas where fecha between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";

                string consultax = "SELECT "+
  "d.codigo_articulo     AS codigo_articulo, "+
  "a.descripcion         AS Articulo, "+
  "b.nombre_bodega       AS nombre_bodega, "+
  "h.fecha               AS fecha, "+
  "MIN(h.fecha)          AS fecha_1era_venta, "+
  "MAX(h.fecha)          AS fecha_ultima_venta, "+
  "SUM(d.cantidad_enviada) AS cantidad_enviada, "+
  "(MAX(d.precio_unitario) / 1.12) AS precio_maximo_sin_iva, "+
  "MAX(d.precio_unitario) AS precio_maximo_iva, "+
  "(MIN(d.precio_unitario) / 1.12) AS precio_minimo_sin_iva, "+
  "MIN(d.precio_unitario) AS precio_minimo_iva, "+
  "(SUM(d.precio_total) / SUM(d.cantidad_enviada)) AS precio_promedio_iva, "+
  "(SUM((d.precio_total / 1.12)) / SUM(d.cantidad_enviada)) AS precio_promedio_sin_iva, "+
  "SUM(d.precio_total)   AS total_facturado_iva, "+
  "SUM((d.precio_total / 1.12)) AS Total_facturado_sin_iva, "+
  "(h.descuento / h.monto_neto) AS descuentoPct, "+
  "h.descuento           AS DescuentoQ, "+
  "CONCAT(CONVERT(t.nombre_documento USING utf8),'[',CONVERT(t.serie_documento USING utf8),']') AS documento, "+
  "AVG(a.costo)          AS costo_iva, "+
  "AVG((a.costo / 1.12)) AS costo_sin_iva, "+
  "+s.nombre_subcategoria AS categoria, "+
  "h.codigo_serie        AS codigo_serie "+
"FROM (((((header_doctos_inv h "+
       "JOIN detalle_doctos_inv d "+
         "ON ((h.id_documento = d.id_documento))) "+
      "JOIN v_tipos_documentos t "+
        "ON ((h.codigo_serie = t.codigo_serie))) "+
     "JOIN articulos a "+
       "ON ((d.codigo_articulo = a.codigo_articulo))) "+
    "JOIN sub_categorias s "+
      "ON ((a.codigo_categoria = s.codigo_subcat))) "+
   "JOIN bodegas_header b "+
     "ON ((d.codigo_bodega = b.codigo_bodega))) "+
"WHERE ((t.codigo_tipo = 1) "+
       "AND (h.estadoid NOT IN(3,6))) "+
"AND h.fecha between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' "+
"GROUP BY d.codigo_articulo "+
"ORDER BY SUM(d.cantidad_enviada) DESC ";
                        

                        if (textEdit1.Text.Trim() != "")
                        {
                                       consultax += " limit " + textEdit1.Text;
                        }
                        MySqlDataAdapter adaptadorx = new MySqlDataAdapter(consultax, Properties.Settings.Default.ortoxelaConnectionString);

                        DataSet datasetx = new DataSet();
                        //DataTable dss = new DataTable();
                        adaptadorx.Fill(datasetx, "v_ventas_articulo_mas");
                        //adaptadorx.Fill(dss);
                        XtraReport_ProductosMasVendidos reported = new XtraReport_ProductosMasVendidos();
                        reported.DataSource = datasetx;
                        //reported.DataSource = dss;
                        reported.DataMember = datasetx.Tables["v_ventas_articulo_mas"].TableName;
                        reported.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
                        reported.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59"; ;
                        reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 

                        reported.RequestParameters = false;
                        reported.ShowPreview();
                    //}
                    //else
                    //{

                    //    XtraReport_ProductosMasVendidos reported = new XtraReport_ProductosMasVendidos();
                    //    reported.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
                    //    reported.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59"; ;
                    //    reported.RequestParameters = false;
                    //    reported.ShowPreview();
                    //}
            }
          
            Cursor.Current = Cursors.Default;
        }

        /* ********************************************** */
        /* ****** REPORTE VENTAS POR TIPO DE CLIENTE **** */
        /* ********************************************** */
        private void simpleButton7_Click(object sender, EventArgs e)
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

                    string consultax = "SELECT        fecha, tipo_cliente, Total_sin_iva " +
                            " FROM         v_ventas_general " +
                            " where  fecha between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59" + "' " +
                            " and codigo_serie in (" + ListaSeries + ") ";

                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(consultax, Properties.Settings.Default.ortoxelaConnectionString);

                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_ventas_general");
                    XtraReport_VentasPorTipoCliente reported = new XtraReport_VentasPorTipoCliente();
                    reported.DataSource = datasetx;
                    reported.DataMember = datasetx.Tables["v_ventas_general"].TableName;

                    reported.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                    reported.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reported.Parameters["Series"].Value = ListaNombresSeries;
                    reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 

                    reported.RequestParameters = false;
                    reported.ShowPreview();
                }
            }
            
            Cursor.Current = Cursors.Default;
        }

        /* ********************************************** */
        /* ****** REPORTE VENTAS POR CATEGORIA  ********* */
        /* ********************************************** */
        private void simpleButton8_Click(object sender, EventArgs e)
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

                    string consulta = "SELECT codigo_articulo, Articulo, cantidad_enviada, precio_sin_iva, precio_iva, nombre_bodega, fecha_compra, Tipo_Pago, nombre_cliente, codigo_tipoc, nombre_cliente1,  " +
                            " no_documento, descuentoPct, DescuentoQ, total_iva, Total_sin_iva, refer_documento, documento, costo_iva, costo_sin_iva, forma_pago, codigo_subcat,  " +
                            " nombre_subcategoria,codigo_serie " +
                            " FROM v_ventas_detalle_socio_categoria where fecha_compra between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' " +
                            " and codigo_serie in (" + ListaSeries + ") ";
                    if (comboBox5.SelectedValue.ToString() != "0")
                    {
                        consulta += " and codigo_subcat = " + comboBox5.SelectedValue;
                    }

                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_ventas_detalle_cliente_categoria");
                    XtraReport_Ventas_por_cliente_sc reported = new XtraReport_Ventas_por_cliente_sc();
                    reported.DataSource = datasetx;
                    reported.DataMember = datasetx.Tables["v_ventas_detalle_cliente_categoria"].TableName;



                    reported.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
                    reported.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59"; ;
                    if (comboBox5.SelectedValue.ToString() == "0")
                    {
                        reported.Parameters["codigo_subcat"].Value = 1;
                        reported.Parameters["codigo_subcat2"].Value = 1000;
                    }
                    else
                    {
                        reported.Parameters["codigo_subcat"].Value = comboBox5.SelectedValue;
                        reported.Parameters["codigo_subcat2"].Value = comboBox5.SelectedValue;
                    }
                    reported.Parameters["Series"].Value = ListaNombresSeries;
                    reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 

                    reported.RequestParameters = false;
                    reported.ShowPreview();

                }
            }
           
            Cursor.Current = Cursors.Default;
        }


        
        /* ********************************************** */
        /* **** REPORTE VENTAS POR SOCIO COMERCIAL ****** */
        /* ********************************************** */
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

                    string consulta = "SELECT        codigo_articulo, Articulo, cantidad_enviada, precio_sin_iva, precio_iva, nombre_bodega, fecha_compra, Tipo_Pago, nombre_cliente, codigo_tipoc, " +
                   " nombre_cliente1, no_documento, descuentoPct, DescuentoQ, total_iva, Total_sin_iva, refer_documento, documento, costo_iva, costo_sin_iva, forma_pago, " +
                   " codigo_subcat, nombre_subcategoria, codigo_serie FROM            v_ventas_detalle_socio_categoria " +
                   " where fecha_compra between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' " +
                   " and codigo_serie in (" + ListaSeries + ") ";
//                    string consulta = "SELECT clientes.`nombre_cliente` AS nombre_cliente1,1 AS cantidad_enviada,SUM(ROUND(header_doctos_inv.`monto`,2)) AS precio_iva,SUM(ROUND(header_doctos_inv.`monto`/1.12,2)) AS precio_sin_iva  FROM header_doctos_inv " +
//"INNER JOIN clientes " +
//"ON header_doctos_inv.`socio_comercial`=clientes.`codigo_cliente` " +
//"WHERE header_doctos_inv.`estadoid`=4 " +
//"AND header_doctos_inv.`fecha` BETWEEN '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' " +
//"AND header_doctos_inv.`codigo_serie` IN (" + ListaSeries + ")" +
//"GROUP BY clientes.`nombre_cliente`";


                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_ventas_detalle_socio_categoria");
                    XtraReport_Ventas_por_SocioComercial reported = new XtraReport_Ventas_por_SocioComercial();
                    reported.DataSource = datasetx;
                    reported.DataMember = datasetx.Tables["v_ventas_detalle_socio_categoria"].TableName;

                    //XtraReport_Ventas_por_SocioComercial reported = new XtraReport_Ventas_por_SocioComercial();
                    //DataTable dt = new DataTable();
                    //dt = logicaxela.Tabla(consulta);
                    //reported.DataSource = dt;

                    reported.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
                    reported.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59"; ;
                    reported.Parameters["Series"].Value = ListaNombresSeries;
                    reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 

                    reported.RequestParameters = false;
                    reported.ShowPreview();
                }
            }
           
            Cursor.Current = Cursors.Default;
        }

        /* ********************************************** */
        /* **** REPORTE VENTAS LINEA (CATEGORIA) ******** */
        /* ********************************************** */
        private void simpleButton9_Click(object sender, EventArgs e)
        {
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

                    string consulta = "SELECT        codigo_articulo, Articulo, cantidad_enviada, precio_sin_iva, precio_iva, nombre_bodega, fecha_compra, Tipo_Pago, nombre_cliente, codigo_tipoc, " +
                   " nombre_cliente1, no_documento, descuentoPct, DescuentoQ, total_iva, Total_sin_iva, refer_documento, documento, costo_iva, costo_sin_iva, forma_pago, " +
                   " codigo_subcat, nombre_subcategoria, codigo_serie FROM            v_ventas_detalle_socio_categoria " +
                   " where fecha_compra between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' " +
                   " and codigo_serie in (" + ListaSeries + ") ";

                    MySqlDataAdapter adaptadorx = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
                    DataSet datasetx = new DataSet();
                    adaptadorx.Fill(datasetx, "v_ventas_detalle_cliente_categoria");
                    XtraReport_Ventas_categoria reported = new XtraReport_Ventas_categoria();
                    reported.DataSource = datasetx;
                    reported.DataMember = datasetx.Tables["v_ventas_detalle_cliente_categoria"].TableName;


                    reported.Parameters["Fecha_inicio"].Value = FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
                    reported.Parameters["Fecha_fin"].Value = FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                    reported.Parameters["Series"].Value = ListaNombresSeries;
                    reported.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 

                    reported.RequestParameters = false;
                    reported.ShowPreview();
                }
            }
           
            Cursor.Current = Cursors.Default;
        }


        /* ********************************************** */
        /* *********** EVENTO LISTBOX BODEGAS *********** */
        /* ********************************************** */
             
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