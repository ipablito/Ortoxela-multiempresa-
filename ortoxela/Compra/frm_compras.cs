using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Controls;

using integracion_ii;
using Sisnova.Invex.BL;
using DevExpress.XtraReports.UI;

namespace ortoxela.Compra
{
    public partial class frm_compras : DevExpress.XtraEditors.XtraForm
    {
        public frm_compras()
        {
            InitializeComponent();
        }
        private void CargaBodega(int serie)
        {
            try
            {
                /* ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid=1"; */
                /* jramirez 2013.07.24 */
                ssql = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega=1 AND userid=" + clases.ClassVariables.id_usuario;
                if (serie != 0)
                    ssql = ssql + " and codigo_serie=" + serie.ToString();
                ssql = ssql + " order by codigo_bodega asc ";
                DataTable tempTabla = new DataTable();
                tempTabla = logicaxela.Tabla(ssql);
                gridLookBodega.Properties.DataSource = tempTabla;
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
                gridLookBodega.EditValue = int.Parse(tempTabla.Rows[0]["CODIGO"].ToString());
                // 
            }
            catch
            { }
        }

        classortoxela logicaxela = new classortoxela();
        string ssql;
        double TotalIngresoCosto = 0;
        double TotalIngresoVenta = 0;
        private void CargaDatos()
        {
            try
            {
                ssql = "SELECT codigo_proveedor AS CODIGO,nombre_proveedor AS NOMBRE FROM proveedores WHERE estadoid<>2";
                gridLookProveedor.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookProveedor.Properties.DisplayMember = "NOMBRE";
                gridLookProveedor.Properties.ValueMember = "CODIGO";         
                
            }
            catch
            { }
            /*Carga Combo Bodegas*/
            CargaBodega(0);
            
            try
            {
                if (clases.ClassVariables.id_rol == "1")
                {
                    /* ssql = "SELECT s.codigo_serie,CONCAT(t.nombre_documento,' [', s.serie_documento,']') AS DOCUMENTO FROM tipos_documento AS t , series_documentos AS s WHERE s.codigo_tipo = t.codigo_tipo and t.signo>0"; */
                    /* jramirez 2013.07.24 */
                    ssql = "SELECT distinct codigo_serie,CONCAT(nombre_documento,' [', serie_documento,']') AS DOCUMENTO FROM v_bodegas_series_usuarios  WHERE signo >0 AND userid=" + clases.ClassVariables.id_usuario;

                }
                else
                {
                    /* jramirez 2013.07.24 */
                    ssql = "SELECT distinct codigo_serie,CONCAT(nombre_documento,' [', serie_documento,']') AS DOCUMENTO FROM v_bodegas_series_usuarios  WHERE signo >0 AND codigo_serie NOT IN(18,19) AND userid=" + clases.ClassVariables.id_usuario;
                    /* ssql = "SELECT s.codigo_serie,CONCAT(t.nombre_documento,' [', s.serie_documento,']') AS DOCUMENTO FROM tipos_documento AS t , series_documentos AS s WHERE s.codigo_tipo = t.codigo_tipo and t.signo>0 AND s.codigo_serie NOT IN(18,19)"; */
                }
                gridLookTipoDocumento.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookTipoDocumento.Properties.DisplayMember = "DOCUMENTO";
                gridLookTipoDocumento.Properties.ValueMember = "codigo_serie";
                gridLookTipoDocumento.Properties.View.Columns["codigo_serie"].Visible = false;
                
            }
            catch
            { }

        }
        private void CreaColumnas()
        {
            DataTable temporal = new DataTable();
            temporal.Columns.Add("IDBODEGA");
            temporal.Columns.Add("BODEGA");
            temporal.Columns.Add("CODIGO");
            temporal.Columns.Add("DESCRIPCION");
            temporal.Columns.Add("CANTIDAD");
            temporal.Columns.Add("PRECIO");
            temporal.Columns.Add("SUBTOTAL");
            temporal.Columns.Add("DESCUENTO");            
            temporal.Columns.Add("Ultimo Costo");
            temporal.Columns.Add("ACTUALIZA_PRECIO");
            temporal.Columns.Add("INGRESO_EGRESO");
            temporal.Columns.Add("EXISTENCIA");
            temporal.Columns.Add("PrecioPonderadoHistorico");

            gridControl1.DataSource = temporal;
            gridView1.Columns["DESCRIPCION"].Width=200;
            gridView1.Columns["IDBODEGA"].Visible = false;
            gridView1.Columns["ACTUALIZA_PRECIO"].Visible = false;
            gridView1.Columns["INGRESO_EGRESO"].Visible = false;
            gridView1.Columns["EXISTENCIA"].Visible = false;
            gridView1.Columns["BODEGA"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["DESCRIPCION"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["SUBTOTAL"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["DESCUENTO"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["Ultimo Costo"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["PrecioPonderadoHistorico"].Visible = false;
        }   
        private void frm_compras_Load(object sender, EventArgs e)
        {            
            CargaDatos();
            id_proveedor = 0;
            id_usuario_descuento = "0";
            CreaColumnas();
            dateEdit1.DateTime = DateTime.Now;            
        }
        
        private void gridControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
        DataTable tempTabla = new DataTable();
        string id_articulo;
        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(dxValidationEncabezado.Validate())
            {
                if (e.KeyChar == 13)
                {
                    id_articulo = textCodigoArt.Text;
                    if (bandera_ingreso_egreso == "1")
                        cadena = "SELECT codigo_articulo as CODIGO, descripcion AS 'NOMBRE ARTICULO',numero_serie AS 'No SERIE',costo,precio_venta FROM articulos where codigo_articulo='" + id_articulo + "'";
                    else
                        cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',costo,articulos.precio_venta FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo where articulos.codigo_articulo='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                    tempTabla = logicaxela.Tabla(cadena);
                    if (tempTabla.Rows.Count > 0)
                    {
                        textCodigoArt.Text = tempTabla.Rows[0]["CODIGO"].ToString();
                        textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString();
                        textCosto.Text = tempTabla.Rows[0]["costo"].ToString();
                        //textVenta.Text = tempTabla.Rows[0]["precio_venta"].ToString();
                        if (bandera_ingreso_egreso == "2")
                            cant_existencia = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);

                        #region ultimo precio al que se compro
                        try
                        {
                            cadena = "SELECT d.precio_unitario FROM detalle_doctos_inv d JOIN header_doctos_inv h ON d.id_documento=h.id_documento WHERE d.codigo_articulo='" + id_articulo + "' AND h.codigo_serie in (7,39) ORDER BY h.id_documento DESC LIMIT 1";
                            DataTable nt1t = new DataTable();
                            nt1t = logicaxela.Tabla(cadena);
                            textVenta.Text = nt1t.Rows[0]["precio_unitario"].ToString();
                        }
                        catch
                        { }
                        #endregion


                        textCantidadArt.Focus();
                    }
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            Form nuevo = new Proveedores.Proveedor();
            nuevo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            nuevo.ShowDialog();
            try
            {
                ssql = "SELECT codigo_proveedor as CODIGO,nombre_proveedor AS NOMBRE FROM proveedores where estadoid<>2";
                gridLookProveedor.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookProveedor.Properties.DisplayMember = "NOMBRE";
                gridLookProveedor.Properties.ValueMember = "CODIGO";
                gridLookProveedor.Text = "";
                gridLookProveedor.EditValue = clases.ClassVariables.idnuevo; ;
            }
            catch
            { }
        }
        int ExistenciaHijo;
        int ExistenciaFija;
        private void sbAgregaArt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (dxValidationProvider1.Validate())
                {

                    if (bandera_ingreso_egreso == "2")
                    {

                        DataTable TempoPadre = new DataTable();
                        /* cadena = "SELECT articulos.compuesto FROM articulos WHERE articulos.codigo_articulo='" + id_articulo + "'";
                         jramirez 2013.07.04
                         */
                        cadena = "select f_es_compuesto('" + id_articulo + "') AS compuesto;";
                        string compuesto = logicaxela.Tabla(cadena).Rows[0]["compuesto"].ToString();
                        if (Convert.ToBoolean(logicaxela.Tabla(cadena).Rows[0]["compuesto"]))
                        {
                            /* cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',articulos.precio_venta,articulos.costo FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND articulos.codigo_padre='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;*/
                            cadena = "CALL sp_devuelve_sistema ('" + id_articulo + "'," + gridLookBodega.EditValue + ")";
                            TempoPadre = logicaxela.Tabla(cadena);                            
                            for (int x = 0; x < TempoPadre.Rows.Count; x++)
                            {
                                banderaRepetido = true;
                                for (int y = 0; y < gridView1.DataRowCount; y++)
                                {
                                    if (gridView1.GetRowCellValue(y, "CODIGO").ToString() == TempoPadre.Rows[x]["CODIGO"].ToString() & gridView1.GetRowCellValue(y, "IDBODEGA").ToString() == gridLookBodega.EditValue.ToString())
                                        banderaRepetido = false;
                                }

                                if (banderaRepetido)
                                {
                                    ExistenciaHijo = Convert.ToInt32(TempoPadre.Rows[x]["EXISTENCIA"]);
                                    if (ExistenciaHijo != 0)
                                    {
                                        if (Convert.ToInt32(textCantidadArt.Text) <= ExistenciaHijo)
                                        {
                                            ExistenciaFija = Convert.ToInt32(textCantidadArt.Text);
                                        }
                                        else
                                        {
                                            ExistenciaFija = ExistenciaHijo;
                                        }
                                        double descuento;
                                        if (textDescuentoPorce.Text.Contains("%"))
                                        {
                                            descuento = Convert.ToDouble(TempoPadre.Rows[x]["costo"]) * (Convert.ToDouble(textDescuentoPorce.Text.Replace("%", "")) / 100);
                                        }
                                        else
                                        {
                                            descuento = Convert.ToDouble(textDescuentoPorce.Text.Replace("Q", ""));
                                        }
                                        gridView1.AddNewRow();
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text + "[" + gridLookBodega.EditValue + "]");
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", TempoPadre.Rows[x]["CODIGO"]);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", TempoPadre.Rows[x]["NOMBRE ARTICULO"]);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", ExistenciaFija);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO", Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCUENTO", descuento);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento) * (ExistenciaFija));
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", bandera_actualiza_precio);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", bandera_ingreso_egreso);
                                        //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Ultimo Costo", TempoPadre.Rows[x]["precio_venta"]);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Ultimo Costo", textVenta.Text);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIA", ExistenciaHijo);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PrecioPonderadoHistorico", PrecioPonderadoHistoria);
                                        TotalIngresoCosto = TotalIngresoCosto + (ExistenciaFija * (Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento));
                                        TotalIngresoVenta = TotalIngresoVenta + (ExistenciaFija * Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"]));
                                        totalIva = totalIva + (((Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento) * (.12)) * ExistenciaFija);
                                        gridView1.UpdateCurrentRow();
                                        CalculaDescuento();
                                        //textTotalVenta.Text = TotalIngresoVenta.ToString("C");
                                        
                                    }
                                    
                                }
                                else
                                    clases.ClassMensajes.ProdYaExisteEnListado(this);
                            }
                            textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text = "";
                            textCodigoArt.Focus();
                        }
                        else
                            if (Convert.ToInt32(textCantidadArt.Text) <= cant_existencia)
                            {
                                banderaRepetido = true;
                                for (int y = 0; y < gridView1.DataRowCount; y++)
                                {
                                    if (gridView1.GetRowCellValue(y, "CODIGO").ToString() == textCodigoArt.Text & gridView1.GetRowCellValue(y, "IDBODEGA").ToString() == gridLookBodega.EditValue.ToString())
                                        banderaRepetido = false;
                                }
                                if (banderaRepetido)
                                {
                                    double descuento;
                                    if (textDescuentoPorce.Text.Contains("%"))
                                    {
                                        descuento = Convert.ToDouble(textCosto.Text) * (Convert.ToDouble(textDescuentoPorce.Text.Replace("%", "")) / 100);
                                    }
                                    else
                                    {
                                        descuento = Convert.ToDouble(textDescuentoPorce.Text.Replace("Q", ""));
                                    }
                                    gridView1.AddNewRow();
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text + "[" + gridLookBodega.EditValue + "]");
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", textCodigoArt.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", textNombreArti.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", textCantidadArt.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO", Convert.ToDouble(textCosto.Text) - descuento);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCUENTO", descuento);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(textCosto.Text) - descuento) * (Convert.ToDouble(textCantidadArt.Text)));
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", bandera_actualiza_precio);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", bandera_ingreso_egreso);
                                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Ultimo Costo", textVenta.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Ultimo Costo", textVenta.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIA", cant_existencia);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PrecioPonderadoHistorico", PrecioPonderadoHistoria);
                                    TotalIngresoCosto = TotalIngresoCosto + (Convert.ToDouble(textCantidadArt.Text) * (Convert.ToDouble(textCosto.Text) - descuento));
                                    TotalIngresoVenta = TotalIngresoVenta + (Convert.ToDouble(textCantidadArt.Text) * Convert.ToDouble(textVenta.Text));
                                    totalIva = totalIva + (((Convert.ToDouble(textCosto.Text) - descuento) * (.12)) * Convert.ToDouble(textCantidadArt.Text));
                                    gridView1.UpdateCurrentRow();
                                    CalculaDescuento();
                                    //textTotalVenta.Text = TotalIngresoVenta.ToString("C");
                                    textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textCosto.Text = textVenta.Text = textVenta.Text = "";
                                    textCodigoArt.Focus();
                                }
                            }
                            else
                                clases.ClassMensajes.NoHayExistenciaProd(this);
                    }
                    else
                    {
                        DataTable TempoPadre = new DataTable();
                        cadena = "SELECT articulos.compuesto FROM articulos WHERE articulos.codigo_articulo='" + id_articulo + "'";
                        string compuesto = logicaxela.Tabla(cadena).Rows[0]["compuesto"].ToString();
                        if (Convert.ToBoolean(logicaxela.Tabla(cadena).Rows[0]["compuesto"]))
                        {
                            cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',articulos.precio_venta,articulos.costo FROM articulos WHERE articulos.estadoid=1 AND articulos.codigo_padre='" + id_articulo + "'";
                            TempoPadre = logicaxela.Tabla(cadena);
                            
                            for (int x = 0; x < TempoPadre.Rows.Count; x++)
                            {
                                banderaRepetido = true;
                                for (int y = 0; y < gridView1.DataRowCount; y++)
                                {
                                    if (gridView1.GetRowCellValue(y, "CODIGO").ToString() == TempoPadre.Rows[x]["CODIGO"].ToString() & gridView1.GetRowCellValue(y, "IDBODEGA").ToString() == gridLookBodega.EditValue.ToString())
                                        banderaRepetido = false;
                                }

                                if (banderaRepetido)
                                {
                                    ExistenciaFija = Convert.ToInt32(textCantidadArt.Text);

                                    double descuento;
                                    if (textDescuentoPorce.Text.Contains("%"))
                                    {
                                        descuento = Convert.ToDouble(TempoPadre.Rows[x]["costo"]) * (Convert.ToDouble(textDescuentoPorce.Text.Replace("%", "")) / 100);
                                    }
                                    else
                                    {
                                        descuento = Convert.ToDouble(textDescuentoPorce.Text.Replace("Q", ""));
                                    }
                                    gridView1.AddNewRow();
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text + "[" + gridLookBodega.EditValue + "]");
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", TempoPadre.Rows[x]["CODIGO"]);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", TempoPadre.Rows[x]["NOMBRE ARTICULO"]);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", ExistenciaFija);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO", Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCUENTO", descuento);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento) * (ExistenciaFija));
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", bandera_actualiza_precio);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", bandera_ingreso_egreso);
                                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "VENTA", TempoPadre.Rows[x]["precio_venta"]);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Ultimo Costo", textVenta.Text);
                                    TotalIngresoCosto = TotalIngresoCosto + (ExistenciaFija * (Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento));
                                    TotalIngresoVenta = TotalIngresoVenta + (ExistenciaFija * Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"]));
                                    totalIva = totalIva + (((Convert.ToDouble(TempoPadre.Rows[x]["costo"]) - descuento) * (.12)) * ExistenciaFija);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PrecioPonderadoHistorico", PrecioPonderadoHistoria);
                                    gridView1.UpdateCurrentRow();
                                    CalculaDescuento();
                                    //textTotalVenta.Text = TotalIngresoVenta.ToString("C");                                  
                                }
                                else
                                    clases.ClassMensajes.ProdYaExisteEnListado(this);
                            }
                            gridView1.AddNewRow();
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text + "[" + gridLookBodega.EditValue + "]");
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", textCodigoArt.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", textNombreArti.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", 0);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO", 0);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCUENTO", 0);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL",0);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", bandera_actualiza_precio);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", bandera_ingreso_egreso);
                            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "VENTA", 0);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Ultimo Costo", textVenta.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PrecioPonderadoHistorico", PrecioPonderadoHistoria);
                            gridView1.UpdateCurrentRow();                            
                            textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text = "";
                            textCodigoArt.Focus();
                        }
                        else
                        {
                            banderaRepetido = true;
                            for (int y = 0; y < gridView1.DataRowCount; y++)
                            {
                                if (gridView1.GetRowCellValue(y, "CODIGO").ToString() == textCantidadArt.Text & gridView1.GetRowCellValue(y, "IDBODEGA").ToString() == gridLookBodega.EditValue.ToString())
                                    banderaRepetido = false;
                            }
                            if (banderaRepetido)
                            {
                                double descuento;
                                if (textDescuentoPorce.Text.Contains("%"))
                                {
                                    descuento = Convert.ToDouble(textCosto.Text) * (Convert.ToDouble(textDescuentoPorce.Text.Replace("%", "")) / 100);
                                }
                                else
                                {
                                    descuento = Convert.ToDouble(textDescuentoPorce.Text.Replace("Q", ""));
                                }
                                gridView1.AddNewRow();
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text + "[" + gridLookBodega.EditValue + "]");
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", textCodigoArt.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", textNombreArti.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", textCantidadArt.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO", Convert.ToDouble(textCosto.Text) - descuento);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCUENTO", descuento);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(textCosto.Text) - descuento) * (Convert.ToDouble(textCantidadArt.Text)));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", bandera_actualiza_precio);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", bandera_ingreso_egreso);
                                //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "VENTA", textVenta.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Ultimo Costo", textVenta.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PrecioPonderadoHistorico", PrecioPonderadoHistoria);
                                TotalIngresoCosto = TotalIngresoCosto + (Convert.ToDouble(textCantidadArt.Text) * (Convert.ToDouble(textCosto.Text) - descuento));
                                TotalIngresoVenta = TotalIngresoVenta + (Convert.ToDouble(textCantidadArt.Text) * Convert.ToDouble(textVenta.Text));
                                totalIva = totalIva + (((Convert.ToDouble(textCosto.Text) - descuento) * (.12)) * Convert.ToDouble(textCantidadArt.Text));
                                gridView1.UpdateCurrentRow();
                                CalculaDescuento();
                                //textTotalVenta.Text = TotalIngresoVenta.ToString("C");                                
                            }
                        }
                        textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textCosto.Text = textVenta.Text = textVenta.Text = "";
                        textCodigoArt.Focus();
                    }
                }
                else
                    clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
            catch
            { 
            
            }
            Cursor.Current = Cursors.Default;
        }
        
        int cant_existencia;
        string PrecioPonderadoHistoria = "0";
        private void textNombreArti_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dxValidationEncabezado.Validate())
            {
                if (bandera_ingreso_egreso == "1")
                    cadena = "SELECT codigo_articulo as CODIGO, descripcion AS 'NOMBRE ARTICULO',costo as 'PRECIO COSTO', precio_venta AS 'PRECIO VENTA' FROM articulos where estadoid=1 ";
                else
                    cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.precio_venta AS 'PRECIO VENTA',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid=1 AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                clases.ClassVariables.cadenabusca = cadena;
                Form nuevo = new Buscador.Buscador();
                nuevo.ShowDialog();
                if (Buscador.Buscador.SeleccionSiNo)
                {
                    id_articulo = clases.ClassVariables.id_busca;
                    if (bandera_ingreso_egreso == "1")
                        cadena = "SELECT codigo_articulo as CODIGO, descripcion AS 'NOMBRE ARTICULO',numero_serie AS 'No SERIE',costo,precio_venta FROM articulos where codigo_articulo='" + id_articulo + "'";
                    else
                        cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',costo,articulos.precio_venta FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo where articulos.codigo_articulo='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                    tempTabla = logicaxela.Tabla(cadena);
                    if (tempTabla.Rows.Count > 0)
                    {
                        textCodigoArt.Text = tempTabla.Rows[0]["CODIGO"].ToString();
                        textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString();
                        textCosto.Text = tempTabla.Rows[0]["costo"].ToString();
                        PrecioPonderadoHistoria = tempTabla.Rows[0]["costo"].ToString();
                        //textVenta.Text = tempTabla.Rows[0]["precio_venta"].ToString();
                        if (bandera_ingreso_egreso == "2")
                            cant_existencia = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);


                        #region ultimo precio al que se compro
                        try
                        {
                            cadena = "SELECT d.precio_unitario FROM detalle_doctos_inv d JOIN header_doctos_inv h ON d.id_documento=h.id_documento WHERE d.codigo_articulo='" + id_articulo + "' AND h.codigo_serie in (7,39) ORDER BY h.id_documento DESC LIMIT 1";
                            DataTable nt1t = new DataTable();
                            nt1t = logicaxela.Tabla(cadena);
                            textVenta.Text = nt1t.Rows[0]["precio_unitario"].ToString();
                        }
                        catch
                        { }
                        #endregion
                        
                        textCantidadArt.Focus();
                    }
                }
            }
        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transac;
        string id_nuevoIngreso;
        int id_proveedor;   
        private void registraIngreso()
        {
            
            try
            {
                    conexion.Open();
                    transac = conexion.BeginTransaction();
                    comando.Transaction = transac;

                    

                    ssql = "INSERT into header_doctos_inv(codigo_serie,no_documento,codigo_proveedor, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento, razon_ajuste, descripcion, estadoid,contado_credito,refer_documento) " +
                        "VALUES ("+gridLookTipoDocumento.EditValue+", '"+textNoDocumento.Text+"', "+id_proveedor+", '"+dateEdit1.DateTime.ToString("yyyy-MM-dd")+"', "+TotalIngresoCosto+", "+TotalDescuento+", "+TotalIngresoVenta+", "+clases.ClassVariables.id_usuario+", "+id_usuario_descuento+", '"+memoRazonAjuste.Text+"', '"+memoDescripcion.Text+"', 4,"+radioGroup2.SelectedIndex+",'"+comboBox_seriesComprasINVEX.Text+"-"+ textNoFacturaCompra.Text+"');SELECT LAST_INSERT_ID();";
                
                comando = new MySqlCommand(ssql, conexion);
                comando.Transaction=transac;                
                id_nuevoIngreso=comando.ExecuteScalar().ToString();

                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    #region hayar el precio de venta
                    string cn = "select precio_venta from articulos where codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "';";
                    DataTable ntt = logicaxela.Tabla(cn);

                    string pv = "0";
                    try
                    {
                        pv = ntt.Rows[0]["precio_venta"].ToString();
                    }
                    catch
                    { }
                    #endregion

                    ssql = "INSERT into detalle_doctos_inv(id_documento, cantidad_enviada, precio_unitario, precio_total,codigo_articulo, codigo_bodega, precio_venta,precioPonderado_Historico) " +
                               "VALUES (" + id_nuevoIngreso + ", " + gridView1.GetRowCellValue(x, "CANTIDAD") + ", " + gridView1.GetRowCellValue(x, "PRECIO") + ", " + gridView1.GetRowCellValue(x, "SUBTOTAL") + ",'" + gridView1.GetRowCellValue(x, "CODIGO") + "', " + gridView1.GetRowCellValue(x, "IDBODEGA") + ", " + pv+","+gridView1.GetRowCellValue(x,"PrecioPonderadoHistorico")+ ");";
                    comando = new MySqlCommand(ssql, conexion);
                    comando.Transaction = transac;
                    comando.ExecuteNonQuery();
                    if (bandera_actualiza_precio == "1")
                    {
                        ssql = "UPDATE articulos SET articulos.costo= (select f_costo_iva_articulo('" + gridView1.GetRowCellValue(x, "CODIGO") + "')) ,articulos.precio_venta=" + gridView1.GetRowCellValue(x, "VENTA") + " WHERE articulos.codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                        comando = new MySqlCommand(ssql, conexion);
                        comando.Transaction = transac;
                        comando.ExecuteNonQuery();
                    }
                    if (bandera_ingreso_egreso == "2")
                    {
                        ssql = "update bodegas SET  existencia_articulo = existencia_articulo -" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA") + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                        comando = new MySqlCommand(ssql, conexion);
                        comando.Transaction = transac;
                        comando.ExecuteNonQuery();
                    }
                    else
                    {
                        ssql = "SELECT * FROM bodegas WHERE bodegas.codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "' AND bodegas.codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA");
                        if (logicaxela.ExisteRegistro(ssql))
                        {
                            ssql = "update bodegas SET  existencia_articulo = existencia_articulo +" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA") + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                            comando = new MySqlCommand(ssql, conexion);
                            comando.Transaction = transac;
                            comando.ExecuteNonQuery();
                        }
                        else
                        {
                            ssql = "INSERT into bodegas(codigo_bodega, codigo_articulo, existencia_articulo) " +
                                  "VALUES (" + gridView1.GetRowCellValue(x, "IDBODEGA") + ", '" + gridView1.GetRowCellValue(x, "CODIGO") + "', '" + gridView1.GetRowCellValue(x, "CANTIDAD") + "')";
                            comando = new MySqlCommand(ssql, conexion);
                            comando.Transaction = transac;
                            comando.ExecuteNonQuery();
                        }
                    }
                }
                transac.Commit();
                simplePrinter.Enabled = true;
                sbAceptar.Enabled = false;
                groupControl1.Enabled = false;
                groupControl2.Enabled = false;
                panelControl1.Enabled = false;

                int v = Convert.ToInt16(gridLookTipoDocumento.EditValue);

                if (Class_integracion.logeado == true)
                {
                    agregaraInvex(v);
                }


                clases.ClassMensajes.INSERTO(this);
            }
            catch
            {
                transac.Rollback();
                clases.ClassMensajes.NoINSERTO(this);
            }
            finally
            {
                conexion.Close();
            }
        }
        private void sbAceptar_Click(object sender, EventArgs e)
        {
            if (Class_integracion.logeado == true)
            {
                DataTable dt = new DataTable();
                int v = Convert.ToInt16(gridLookTipoDocumento.EditValue);
                string cns = "SELECT codigo_tipo FROM series_documentos WHERE series_documentos.codigo_serie=" + v.ToString();
                dt = logicaxela.Tabla(cns);
                cns = dt.Rows[0][0].ToString();
                if (cns == "6")
                {
                    if (comboBox_seriesComprasINVEX.Text != "")
                    {
                        if (dxValidationProvider3.Validate() == false)
                        {
                            clases.ClassMensajes.FaltanDatosEnCampos(this);
                            return;
                        }
                    }
                    else
                    {
                        clases.ClassMensajes.FaltanDatosEnCamposNombre(this, "Serie de factura del proveedor");
                        return;
                    }
                }
            }



            if(dxValidationProvider2.Validate() & gridView1.DataRowCount>0)
            {
                
                //cadena = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE header_doctos_inv.no_documento="+textNoDocumento.Text+" AND series_documentos.codigo_serie="+gridLookTipoDocumento.EditValue;
                if(gridLookProveedor.EditValue!=null && (textNoFacturaCompra.Text!="" && textNoFacturaCompra.Text!="0"))
                    cadena = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE (header_doctos_inv.no_documento=" + textNoDocumento.Text + " AND series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue + ") OR (refer_documento='" + comboBox_seriesComprasINVEX.Text + "-" + textNoFacturaCompra.Text + "' AND codigo_proveedor="+gridLookProveedor.EditValue.ToString()+")";
                else
                    cadena = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE header_doctos_inv.no_documento=" + textNoDocumento.Text + " AND series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue;

                if (logicaxela.ExisteRegistro(cadena) == false)
                {
                    if (gridLookProveedor.EditValue != null)
                    {
                        registraIngreso();
                        //if (clases.ClassVariables.llamadoDentroForm)
                        //{
                        //    clases.ClassVariables.llamadoDentroForm = false;
                        //    this.Close();
                        //}
                    }
                    else
                    {
                        if (MessageBox.Show("¿Desea seleccionar un proveedor?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            registraIngreso();
                            if (clases.ClassVariables.llamadoDentroForm)
                            {
                                clases.ClassVariables.llamadoDentroForm = false;
                                this.Close();
                            }
                        }
                    }
                }
                else
                {
                    alertControl1.Show(this, "INFORMACION", "EL NUMERO DE DOCUMENTO YA EXISTE", Properties.Resources.Advertencia64);
                }
            }
            else
            clases.ClassMensajes.FaltanDatosEnCampos(this);
        }
        private void Limpia()
        {
            //gridLookBodega.EditValue = null;
            //gridLookProveedor.EditValue = null;
            //gridLookTipoDocumento.EditValue = null;
            textNoDocumento.Text = "";
            textCodigoArt.Text = "";
            textCantidadArt.Text = "";
            textNombreArti.Text = "";
            textCosto.Text = "";
            textVenta.Text = "";
            memoDescripcion.Text = "";
            memoRazonAjuste.Text="";
            TotalIngresoCosto = 0;
            TotalDescuento = 0;
            textTotalIva.Text = TotalDescuento.ToString("C");
            TotalIngresoVenta = 0;
            textTotalSinIva.Text = TotalIngresoVenta.ToString("C");
            textPrecioTotal.Text=TotalIngresoVenta.ToString("C");
            CreaColumnas();
            gridLookBodega.Focus();
            simplePrinter.Enabled = false;
            sbAceptar.Enabled = true;
            groupControl1.Enabled = true;
            groupControl2.Enabled = true;
            panelControl1.Enabled = true;



            #region nuevo correlativo

            //gridLookTipoDocumento_EditValueChanged
            try
            {
                cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC' FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE  series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue + " ORDER BY header_doctos_inv.id_documento DESC LIMIT 1";
                textNoDocumento.Text = logicaxela.Tabla(cadena).Rows[0][0].ToString();
            }
            catch
            {
                textNoDocumento.Text = "1";
            }

            #endregion
        }
        private void sbnuevo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿DESEA BORRAR LO DATOS?", "INFORMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            Limpia();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
            TotalIngresoCosto = TotalIngresoCosto - (Convert.ToDouble(gridView1.GetFocusedRowCellValue("CANTIDAD")) * Convert.ToDouble(gridView1.GetFocusedRowCellValue("PRECIO")));
            TotalIngresoVenta = TotalIngresoCosto - (Convert.ToDouble(gridView1.GetFocusedRowCellValue("CANTIDAD")) * Convert.ToDouble(gridView1.GetFocusedRowCellValue("VENTA")));
            gridView1.DeleteSelectedRows();
            gridView1.UpdateCurrentRow();
            textTotalSinIva.Text = TotalIngresoCosto.ToString("C");
            textPrecioTotal.Text = TotalIngresoVenta.ToString("C");
            //gridLookProveedor.Text = "";
            //gridLookBodega.Text = "";
            //gridLookTipoDocumento.Text = "";
            }
            catch
            {}
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simplePrinter_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                DataTable nt = new DataTable();
                ad.Fill(nt);

                PrintIngresoProd.XtraReportIngresoProd reporte = new PrintIngresoProd.XtraReportIngresoProd();
                reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                reporte.Parameters["ID"].Value=id_nuevoIngreso;
                reporte.RequestParameters = false;
                reporte.ShowPreviewDialog();
            }
            catch
            {
            
            }
            this.Cursor = Cursors.Default;
        }

        private void simpleButtonEstado_Click(object sender, EventArgs e)
        {
            try
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = true;
                Form nuevo = new Bodega.Tipobodega();
                nuevo.WindowState = System.Windows.Forms.FormWindowState.Normal;
                nuevo.ShowDialog();
                ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid<>2";
                gridLookBodega.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
                gridLookBodega.Text = "";
                gridLookBodega.EditValue = clases.ClassVariables.idnuevo;
            }
            catch
            { }
        }

        public void agregaraInvex(int iddoc)
        {
            DataTable dt = new DataTable();
            string cns = "SELECT codigo_tipo FROM series_documentos WHERE series_documentos.codigo_serie=" + iddoc.ToString();
            dt = logicaxela.Tabla(cns);
            cns = dt.Rows[0][0].ToString();
            string formapago = "";
            if (cns == "6")
            {
                string nitp = "0";
                string nombrep = "Sin Proveedor";
                    try
                    {
                cns ="SELECT nit,nombre_proveedor FROM proveedores WHERE codigo_proveedor="+gridLookProveedor.EditValue;
                dt=logicaxela.Tabla(cns);
                nitp=dt.Rows[0][0].ToString();
                nombrep=dt.Rows[0][1].ToString();
                    }
                catch{}
                string fp ="";
                int nodoc = Convert.ToInt32(textNoFacturaCompra.Text);
                string sf = comboBox_seriesComprasINVEX.Text;
                
                if(radioGroup2.SelectedIndex==0)
                    fp="Contado";
                else
                    fp="Crédito";

                //formapago = fp;
                formapago = "Crédito";

                Sisnova.Invex.BL.Compra encabezadodecompra = integracion_ii.Class_compra.EncabezadoCompra(nombrep, nitp, dateEdit1.DateTime,formapago, nodoc,sf);

                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    cns = "SELECT categorias.`codigo_categoria`,categorias.`nombre_categoria` FROM categorias INNER JOIN sub_categorias ON sub_categorias.`codigo_categoria`=categorias.`codigo_categoria` INNER JOIN articulos ON articulos.`codigo_categoria`=sub_categorias.`codigo_subcat` WHERE articulos.codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                    dt = logicaxela.Tabla(cns);
                    string codigoA = dt.Rows[0][0].ToString();
                    string nombreA = dt.Rows[0][1].ToString();
                    int cantidadA = Convert.ToInt16(gridView1.GetRowCellValue(x, "CANTIDAD"));
                    decimal precioA = Convert.ToDecimal(gridView1.GetRowCellValue(x, "PRECIO"));
                    integracion_ii.Class_compra.DetalleCompra(encabezadodecompra, cantidadA, precioA, codigoA, nombreA);
                }

                integracion_ii.Class_compra.ValidarCompra(encabezadodecompra,formapago);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            Form hijo = new Series.SerieDoc();
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
            DataTable dt = new DataTable();
            try
            {
                ssql = "SELECT codigo_serie CODIGO,serie_documento as SERIE FROM series_documentos INNER JOIn tipos_documento on series_documentos.codigo_tipo = tipos_documento.codigo_tipo WHERE tipos_documento.codigo_tipo=6";
                gridLookTipoDocumento.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookTipoDocumento.Properties.DisplayMember = "SERIE";
                gridLookTipoDocumento.Properties.ValueMember = "CODIGO";
                gridLookTipoDocumento.Text = "";
                gridLookTipoDocumento.EditValue = clases.ClassVariables.idnuevo;
               
            }
            catch
            { }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            Articulos.Articulos.BanderaLlamada = false;
            Form nuevo = new Articulos.Articulos();
            nuevo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            nuevo.ShowDialog();
            if(Articulos.Articulos.BanderaLlamada )
            {
                textCodigoArt.Text = Articulos.Articulos.id_articulo;
                textNombreArti.Text = Articulos.Articulos.nombre_articulo;
                textCosto.Text = Articulos.Articulos.precio_costo;
                textVenta.Text = Articulos.Articulos.precio_venta;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            textDescuentoPorce.Enabled = false;
            simpleButton4.Visible = true;
            simpleButton5.Visible = false;
        }
        string id_usuario_descuento;
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            MiniLogin.LoginMini.id_UsuarioModifica = "";
            Form logins = new MiniLogin.LoginMini();
            logins.ShowDialog();
            if (MiniLogin.LoginMini.id_UsuarioModifica != "")
            {
                id_usuario_descuento = MiniLogin.LoginMini.id_UsuarioModifica;
                textDescuentoPorce.Enabled = true;
                simpleButton4.Visible = false;
                simpleButton5.Visible = true;
            }
        }
        bool banderaRepetido;
        
        double TempTotalPedido;//esta variable me sirve para pasar el total del pedido y podes sacar el descuento
        double TotalDescuento;
        double totalIva;
        private void CalculaDescuento()
        {
            try
            {
                if (Convert.ToDouble(textDescuentoPorce.Text.Replace("%", "")) >= 0)
                {
                    if (textDescuentoPorce.Text.Contains("%"))
                    {

                        TotalDescuento = TotalIngresoCosto * ((Convert.ToDouble(textDescuentoPorce.Text.Replace("%", ""))) / 100);
                        TempTotalPedido = TotalIngresoCosto - TotalDescuento;
                        textTotalSinIva.Text = (TempTotalPedido/1.12).ToString("C");
                        TotalIngresoVenta = TempTotalPedido;
                        textTotalDescuento.Text = TotalDescuento.ToString("C");
                        textPrecioTotal.Text = (TotalIngresoVenta).ToString("C");
                        totalIva = TempTotalPedido - (TempTotalPedido / 1.12);
                        textTotalIva.Text = totalIva.ToString("C");
                        double descuento;
                        for (int x = 0; x < gridView1.DataRowCount; x++)
                        {
                            descuento = Math.Round(Convert.ToDouble(gridView1.GetRowCellValue(x,"PRECIO"))*(Convert.ToDouble(textDescuentoPorce.Text.Replace("%",""))/100),2);
                            gridView1.SetRowCellValue(x,"PRECIO",Convert.ToDouble(gridView1.GetRowCellValue(x,"PRECIO"))-descuento);
                            gridView1.SetRowCellValue(x, "SUBTOTAL", Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO")));
                            gridView1.SetRowCellValue(x, "DESCUENTO", descuento);
                        }
                    }
                    else
                    {
                        TotalDescuento = Convert.ToDouble(textDescuentoPorce.Text);
                        TempTotalPedido = TotalIngresoCosto - TotalDescuento;
                        textTotalSinIva.Text = (TempTotalPedido / 1.12).ToString("C");
                        TotalIngresoVenta = TempTotalPedido;
                        textTotalDescuento.Text = TotalDescuento.ToString("C");
                        textPrecioTotal.Text = (TotalIngresoVenta).ToString("C");
                        totalIva = TempTotalPedido - (TempTotalPedido / 1.12);
                        textTotalIva.Text = totalIva.ToString("C");
                    }
                }
                else
                {
                    textDescuentoPorce.Text = "0";
                    TotalDescuento = TotalIngresoCosto * ((Convert.ToDouble(textDescuentoPorce.Text)) / 100);
                    TempTotalPedido = TotalIngresoCosto - TotalDescuento;
                    textTotalSinIva.Text = (TempTotalPedido / 1.12).ToString("C");
                    TotalIngresoVenta=TempTotalPedido;
                    textTotalDescuento.Text = TotalDescuento.ToString("C");
                    textPrecioTotal.Text = (TotalIngresoVenta).ToString("C");
                    totalIva = TempTotalPedido - (TempTotalPedido / 1.12);
                    textTotalIva.Text = totalIva.ToString("C");
                }

            }
            catch { }
        }
        private void textDescuentoPorce_EditValueChanged(object sender, EventArgs e)
        {
            CalculaDescuento();
        }
        string bandera_actualiza_precio,bandera_ingreso_egreso;
        string cadena;
        private void gridLookTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {           
            cadena = "SELECT tipos_documento.actualiza_precios,tipos_documento.signo FROM tipos_documento INNER JOIN series_documentos ON tipos_documento.codigo_tipo=series_documentos.codigo_tipo WHERE series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue;
            tempTabla = logicaxela.Tabla(cadena);
            bandera_actualiza_precio=tempTabla.Rows[0][0].ToString();
            bandera_ingreso_egreso=tempTabla.Rows[0][1].ToString();
            CreaColumnas();
            TotalIngresoCosto = 0;
            TotalDescuento = 0;
            textTotalIva.Text = TotalDescuento.ToString("C");
            TotalIngresoVenta = 0;
            textTotalSinIva.Text = TotalIngresoVenta.ToString("C");
            textPrecioTotal.Text = TotalIngresoVenta.ToString("C");

            if (Class_integracion.logeado == true)
            {
                if (gridLookTipoDocumento.EditValue.ToString() == "7")
                {
                    comboBox_seriesComprasINVEX.Enabled = true;
                    comboBox_seriesComprasINVEX.DataSource = integracion_ii.Class_SeriesDocumentos.ListaSeriesFacturasEnCompras();
                    comboBox_seriesComprasINVEX.DisplayMember = "Serie";
                    comboBox_seriesComprasINVEX.ValueMember = "Serie";
                    gridLookProveedor.Text = "";
                }
                else
                {
                    comboBox_seriesComprasINVEX.Text = "";
                    comboBox_seriesComprasINVEX.Enabled = false;
                }
            }
            else
            {
                comboBox_seriesComprasINVEX.Text = "";
                comboBox_seriesComprasINVEX.Enabled = false;
            }


            try
            {
                cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC' FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE  series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue + " ORDER BY header_doctos_inv.id_documento DESC LIMIT 1";
                textNoDocumento.Text = logicaxela.Tabla(cadena).Rows[0][0].ToString();
                
            }
            catch
            {
                textNoDocumento.Text = "1";
            }


            if (Convert.ToInt32(gridLookTipoDocumento.EditValue) == 7)
            {
                sb_solicitud_compra.Enabled = true;
            }
            else
            {
                sb_solicitud_compra.Enabled = false;
            }
            CargaBodega(int.Parse(gridLookTipoDocumento.EditValue.ToString()));

            
        }

        private void gridLookProveedor_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                id_proveedor = Convert.ToInt32(gridLookProveedor.EditValue);
                cadena = "SELECT proveedores.dias_credito FROM proveedores WHERE proveedores.codigo_proveedor=" + id_proveedor;
                labelCreditoProveedor.Text = logicaxela.Tabla(cadena).Rows[0][0].ToString() + " DIAS DE CREDITO";
            }
            catch { }
        }
        double cant_devolucion, preciounitario, total_devuelta;// solo me sirve para calcular el total de devolucion
        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            total_devuelta = 0;
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(gridView1.GetFocusedRowCellValue("CANTIDAD")) * Convert.ToDouble(gridView1.GetFocusedRowCellValue("PRECIO"))));
            for (int x = 0; x < gridView1.DataRowCount; x++)
            {
                cant_devolucion = Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD"));
                preciounitario = Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO"));
                total_devuelta += cant_devolucion * preciounitario;
            }
            TotalIngresoCosto = total_devuelta;
            CalculaDescuento();
        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (bandera_ingreso_egreso == "2")
            {
                ColumnView view = sender as ColumnView;
                if((Convert.ToInt32(view.GetRowCellValue(e.RowHandle,view.Columns["CANTIDAD"]))>(Convert.ToInt32(view.GetRowCellValue(e.RowHandle,view.Columns["EXISTENCIA"])))))
                {
                    e.Valid=false;
                    view.SetColumnError(gridView1.Columns["CANTIDAD"],"NO HAY PRODUCTO EN EXISTENCIA SOLO HAY "+gridView1.GetFocusedRowCellValue("EXISTENCIA"));
                }
            }

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridLookBodega_EditValueChanged(object sender, EventArgs e)
        {
            textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textCosto.Text = textVenta.Text = "";            
        }

        string id_cotizacion;
        bool bandera_carga_cotizacion = false;
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            cadena = "SELECT header_doctos_inv.id_documento AS 'CODIGO',header_doctos_inv.fecha AS 'FECHA PEDIDO',header_doctos_inv.no_documento 'NUMERO DOCUMENTO',proveedores.nombre_proveedor AS 'NOMBRE PROVEEDOR',proveedores.email AS 'EMAIL',proveedores.telefono_principal AS 'TELEFONO',header_doctos_inv.monto_neto AS 'TOTAL SOLICITUD' FROM header_doctos_inv INNER JOIN series_documentos ON series_documentos.codigo_serie=header_doctos_inv.codigo_serie INNER JOIN proveedores ON header_doctos_inv.codigo_proveedor=proveedores.codigo_proveedor WHERE series_documentos.codigo_tipo=14 AND (header_doctos_inv.estadoid=4 ) AND header_doctos_inv.codigo_proveedor=" + id_proveedor;
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                Cursor = Cursors.WaitCursor;
                id_cotizacion = clases.ClassVariables.id_busca;
                cadena = "SELECT bh.codigo_bodega AS IDBODEGA,bh.nombre_bodega AS BODEGA,ar.codigo_articulo AS CODIGO,ar.descripcion AS DESCRIPCION,ddi.cantidad_enviada AS CANTIDAD,ddi.precio_unitario AS 'PRECIO',ddi.precio_total AS 'SUBTOTAL',0 AS DESCUENTO,ar.precio_venta AS VENTA,1 ACTUALIZA_PRECIO,1 AS INGRESO_EGRESO, 0 AS EXISTENCIA FROM detalle_doctos_inv ddi INNER JOIN articulos ar ON ddi.codigo_articulo=ar.codigo_articulo INNER JOIN bodegas_header bh ON ddi.codigo_bodega=bh.codigo_bodega WHERE id_documento=" + id_cotizacion;
                gridControl1.DataSource = logicaxela.Tabla(cadena);
                int existencia_tempora;
                bandera_carga_cotizacion = true;
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    cadena = "select existencia_articulo from bodegas where codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA").ToString() + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO").ToString() + "'";
                    existencia_tempora = Convert.ToInt32(logicaxela.Tabla(cadena).Rows[0][0]);
                    gridView1.SetRowCellValue(x, "EXISTENCIA", existencia_tempora);
                    //if (existencia_tempora == 0)
                    //    gridView1.SetRowCellValue(x, "CANTIDAD", existencia_tempora);
                    //else
                    //    if (Convert.ToInt32(gridView1.GetRowCellValue(x, "IDBODEGA").ToString()) > existencia_tempora)
                    //        gridView1.SetRowCellValue(x, "CANTIDAD", existencia_tempora);
                    gridView1.UpdateCurrentRow();
                }
                bandera_carga_cotizacion = false;
               
                CalculaDescuento();
                Cursor = Cursors.Default;

            }
        }
    }
}