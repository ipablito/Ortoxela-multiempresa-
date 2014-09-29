using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;

using DevExpress.XtraReports.UI;

using integracion_ii;
using Sisnova.Invex.BL;


namespace ortoxela.Pedido
{
    public partial class frm_pedido : DevExpress.XtraEditors.XtraForm
    {
        public frm_pedido()
        {
            InitializeComponent();
        }

        private void labelControl16_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }
        private void CargaBodega(int serie)
        {
            try
            {
                /* ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid=1"; */
                /* jramirez 2013.07.24 */
                cadena = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega=1 AND userid=" + clases.ClassVariables.id_usuario;
                if (serie != 0)
                    cadena = cadena + " and codigo_serie=" + serie.ToString();
                cadena = cadena + " order by codigo_bodega asc ";
                DataTable tempTabla = new DataTable();
                tempTabla = logicaorto.Tabla(cadena);
                gridLookBodega.Properties.DataSource = tempTabla;
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
                gridLookBodega.EditValue = int.Parse(tempTabla.Rows[0]["CODIGO"].ToString());
                gridLookBodega.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
                // 
            }
            catch
            { }
        }
        private void CargaDatosCombos()
        {
            try
            {
                cadena = "SELECT tipo_pago as CODIGO, nombre_tipo_pago AS 'TIPO PAGO' FROM tipo_pago where estadoid<>2";
                gridLookTipoPago.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoPago.Properties.DisplayMember = "TIPO PAGO";
                gridLookTipoPago.Properties.ValueMember = "CODIGO";                
                gridLookTipoPago.EditValue = 1;
                gridLookTipoPago.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
                
            }
            catch
            { }

            try
            {
                cadena = "SELECT distinct codigo_serie AS CODIGO, serie_documento AS 'SERIE DOCUMENTO'  FROM v_bodegas_series_usuarios  WHERE codigo_tipo=5 AND userid=" + clases.ClassVariables.id_usuario ;
                gridLookTipoDocumento.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoDocumento.Properties.DisplayMember = "SERIE DOCUMENTO";
                gridLookTipoDocumento.Properties.ValueMember = "CODIGO";

                gridLookTipoDocumento.EditValue = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                gridLookTipoDocumento.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            }
            catch
            {  //MessageBox.Show("Error"); 
            }
            try
            {
                /* cadena = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid<>2"; */
                /*jramirez 2013.07.24 */
                cadena = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE  FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_serie= " + gridLookTipoDocumento.EditValue;
                gridLookBodega.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
                gridLookBodega.EditValue = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                gridLookBodega.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            }
            catch
            { }
            
            //cadena = "SELECT distinct codigo_serie AS CODIGO, serie_documento AS 'SERIE DOCUMENTO'  FROM v_bodegas_series_usuarios  WHERE codigo_tipo=5 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_bodega = " + gridLookBodega.EditValue.ToString() ;

            
            try
            {
                /* RECIBO - No afecta inventario*/
                cadena = "SELECT codigo_serie CODIGO,CONCAT(tipos_documento.nombre_documento,' - ',serie_documento) AS DOCUMENTO FROM series_documentos INNER JOIN tipos_documento ON series_documentos.codigo_tipo = tipos_documento.codigo_tipo WHERE tipos_documento.codigo_tipo=2";
                gridLookSerieRecibo.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookSerieRecibo.Properties.DisplayMember = "DOCUMENTO";
                gridLookSerieRecibo.Properties.ValueMember = "CODIGO";
                gridLookSerieRecibo.EditValue = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                gridLookSerieRecibo.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            }
            catch
            { }

            try {
                cadena = "SELECT codigo_cliente AS codigodoc,nombre_cliente AS nombreDoctor FROM clientes WHERE clientes.`codigo_tipoc` =7 and estadoid=1 ";
                gridLookDoctores.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookDoctores.Properties.DisplayMember = "nombreDoctor";
                gridLookDoctores.Properties.ValueMember = "codigodoc";
                gridLookDoctores.Properties.NullText = "SELECCIONE DOCTOR";
                gridLookDoctores.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            }
            catch
            {}

        }

        private void NumeroDocu()
        {
            cadena = "SELECT (MAX(header_doctos_inv.id_documento)+1) as NumeroDocs FROM header_doctos_inv";
            textNoDocumento.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
        }
        private void frm_pedido_Load(object sender, EventArgs e)
        {
            tieneDeposito = 0;
            cadena = "SELECT (COALESCE(MAX(recibos.no_recibo),0)+1)AS 'NORECIBO' FROM recibos";
            textNoReciboVale.Text= logicaorto.Tabla(cadena).Rows[0]["NORECIBO"].ToString();
            //HijoPadre();
            NumeroDocu();
            textDescuentoPorce.Text = "0";
            xtraTabPage2.PageEnabled = false;
            xtraTabPage1.PageEnabled = false;
            llenaVale();
            CargaDatosCombos();
            
            CreaColumnas();
            TotalDescuento = TempTotalPedido = TotalPedido = 0;
            textTotalDescuento.Text = TotalPedido.ToString("C");
            textTotalPedido.Text = TotalPedido.ToString("C");
            textPor.Text = TotalPedido.ToString("C");
            id_usuario_descuento = "0";
            id_socioComercial = "";
            gridLookSerieRecibo.EditValue = 3;
        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transa;
        string id_nuevo_pedido;

        string valorreporte = "";
        private void RegistraPedido()
        {
            try
            {
                cadena = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE header_doctos_inv.no_documento=" + textNoDocumento.Text + " AND series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue;
                if (logicaorto.ExisteRegistro(cadena))
                    textNoDocumento.Text = (Convert.ToInt32(textNoDocumento.Text) + 1).ToString();
                conexion.Open();                
                transa = conexion.BeginTransaction();
                if (textDeposito.Text != "")
                {
                    /* jramirez 2013.07.04 se agregó a la inserción el número de depósito del pedido (vale) */
                    cadena = "INSERT into header_doctos_inv(codigo_serie,tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento, socio_comercial, descripcion, estadoid,contado_credito,refer_documento,doctor) " +
                        "VALUES (" + gridLookTipoDocumento.EditValue + ", " + gridLookTipoPago.EditValue + ", '" + textNoDocumento.Text + "', " + id_cliente + ", '" + dateFechaPedido.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', " + TotalPedido + ", " + TotalDescuento + ", " + TempTotalPedido + ", " + clases.ClassVariables.id_usuario + ", " + id_usuario_descuento + ",'" + id_socioComercial + "', '" + memoDescripcion.Text + "', 8," + radioGroup2.SelectedIndex + ",'" + textDeposito.Text + "',"+ gridLookDoctores.EditValue +");select last_insert_id();";
                }
                else
                {
                    cadena = "INSERT into header_doctos_inv(codigo_serie,tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento, socio_comercial, descripcion, estadoid,contado_credito) " +
                        "VALUES (" + gridLookTipoDocumento.EditValue + ", " + gridLookTipoPago.EditValue + ", '" + textNoDocumento.Text + "', " + id_cliente + ", '" + dateFechaPedido.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', " + TotalPedido + ", " + TotalDescuento + ", " + TempTotalPedido + ", " + clases.ClassVariables.id_usuario + ", " + id_usuario_descuento + ",'" + id_socioComercial + "', '" + memoDescripcion.Text + "', 8," + radioGroup2.SelectedIndex + ");select last_insert_id();";
                }
                comando = new MySqlCommand(cadena,conexion);
                comando.Transaction = transa;
                id_nuevo_pedido = comando.ExecuteScalar().ToString();
                valorreporte = textNoDocumento.Text;
                for (int x = 0; x < gridView1.DataRowCount;x++)
                {
                    cadena = "INSERT into detalle_doctos_inv(id_documento, cantidad_enviada, precio_unitario, precio_total, codigo_articulo, codigo_bodega) "+
                                "VALUES (" + id_nuevo_pedido + ", " + gridView1.GetRowCellValue(x, "CANTIDAD") + ", " + gridView1.GetRowCellValue(x, "PRECIO UNITARIO") + ", " + gridView1.GetRowCellValue(x, "SUB TOTAL") + ",'" + gridView1.GetRowCellValue(x, "CODIGO") + "', " + gridView1.GetRowCellValue(x, "IDBODEGA") + ")";
                    comando = new MySqlCommand(cadena,conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                    cadena = "update bodegas SET  existencia_articulo = existencia_articulo -" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA") + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                    comando = new MySqlCommand(cadena, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                }
                cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                            "VALUES (" + id_cliente + ", " + id_nuevo_vale + ","+id_nuevo_pedido+",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                cadena = "update header_doctos_inv SET estadoid = 4 WHERE header_doctos_inv.id_documento="+id_nuevo_vale;
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                transa.Commit();
                clases.ClassMensajes.INSERTO(this);
                sbAceptar.Enabled = false;
                if(radioGroup2.SelectedIndex==0)
                //xtraTabPage2.PageEnabled = true;
                //LlenaDatosRecibo();
                simplePrinter.Enabled = true;
                groupControl1.Enabled = false;
                groupControl2.Enabled = false;
                panelControl1.Enabled = false;               
            }
            catch
            {
                transa.Rollback();
                clases.ClassMensajes.NoINSERTO(this);
            }
            finally
            {
                conexion.Close();
            }

        }
        
        private void LlenaDatosRecibo()
        {
            textRecibimosDe.Text = textNombreCliente.Text;
            dateFechaRecibo.DateTime = DateTime.Now;
            textPor.Text = totalVale.ToString();
            textCodigoCobrador.Text = clases.ClassVariables.id_usuario;
            gridLookSerieRecibo.EditValue = null;
            gridLookSerieRecibo.EditValue = 3;
            //cadena = "SELECT (MAX(recibos.no_recibo)+1)AS 'NORECIBO' FROM recibos";
            //textNoRecibo.Text = logicaorto.Tabla(cadena).Rows[0]["NORECIBO"].ToString();

            if (Convert.ToInt16(gridLookTipoPago.EditValue) == 1)
            {
                textEfectivo.Enabled=true;
                textEfectivo.Text = textTotalVale.Text;
                textCheque.Enabled = false;
                textBanco.Enabled = false;
                // textValor.Text = textTotalPedido.Text;
                textValor.Text = textTotalVale.Text;
            }
            else
            {
                textEfectivo.Enabled = false;
                textEfectivo.Text = "";
                textCheque.Enabled = true;
                textBanco.Enabled = true;
                textBanco.Text =  textTotalVale.Text;
                // textValor.Text = textTotalPedido.Text;
                textValor.Text =  textTotalVale.Text;
            }
        }
        private void sbAceptar_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(gridLookTipoDocumento.Text);
            if(dxValidationPedido.Validate() & gridView1.DataRowCount>0)
            {
               /* Envio */
                RegistraPedido();
           
            }
            else
            {
                clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
        }
        string cadena;
        string id_articulo;
        string id_socioComercial;
        int tieneDeposito = 0;
        classortoxela logicaorto = new classortoxela();
        private void textSocioComercial_KeyPress(object sender, KeyPressEventArgs e)
        {
            cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes WHERE clientes.estadoid<>2 and clientes.socio_comercial=1";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                id_socioComercial = clases.ClassVariables.id_busca;
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes WHERE clientes.codigo_cliente="+id_socioComercial;
                textSocioComercial.Text = logicaorto.Tabla(cadena).Rows[0]["NOMBRE CLIENTE"].ToString();
            }
            e.KeyChar = Convert.ToChar(13);
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        DataTable tempTabla=new DataTable();
        int ExistenciaProd,existencia_minima;
        private void textNombreArti_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dxValidationProvider3.Validate())
            {
                cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.precio_venta AS 'PRECIO VENTA',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                clases.ClassVariables.cadenabusca =cadena;
                Form nuevo = new Buscador.Buscador();
                nuevo.ShowDialog();
                if (Buscador.Buscador.SeleccionSiNo)
                {
                    id_articulo = clases.ClassVariables.id_busca;                    
                        cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',articulos.precio_venta,articulos.minimo FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo where articulos.estadoid<>2 and articulos.codigo_articulo='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                        tempTabla = logicaorto.Tabla(cadena);
                    if (tempTabla.Rows.Count > 0)
                    {
                        textCodigoArt.Text = tempTabla.Rows[0]["CODIGO"].ToString();
                        textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString();
                        ExistenciaProd = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);
                        existencia_minima = Convert.ToInt32(tempTabla.Rows[0]["minimo"]);
                        textVenta.Text = tempTabla.Rows[0]["precio_venta"].ToString();
                        textCantidadArt.Focus();
                    }
                }
            }
        }

        private void textCodigoArt_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (e.KeyChar == 13)
            {
                if (dxValidationProvider3.Validate())
                {
                    cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',articulos.precio_venta,articulos.minimo FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND articulos.codigo_articulo='" + textCodigoArt.Text + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                    tempTabla = logicaorto.Tabla(cadena);
                    if (tempTabla.Rows.Count > 0)
                    {
                        textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString();
                        ExistenciaProd = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);
                        existencia_minima = Convert.ToInt32(tempTabla.Rows[0]["minimo"]);
                        textVenta.Text = tempTabla.Rows[0]["precio_venta"].ToString();
                        textCantidadArt.Focus();
                    }
                }
            }
        }
        private void CreaColumnas()
        {
            DataTable temporal = new DataTable();
            temporal.Columns.Add("IDBODEGA");
            temporal.Columns.Add("BODEGA");
            temporal.Columns.Add("CODIGO");
            temporal.Columns.Add("DESCRIPCION");
            temporal.Columns.Add("CANTIDAD");
            temporal.Columns.Add("PRECIO UNITARIO");
            temporal.Columns.Add("SUB TOTAL");
            temporal.Columns.Add("EXISTENCIABODEGA");
            temporal.Columns.Add("MINIMO");
            gridControl1.DataSource = temporal;
            gridView1.Columns["DESCRIPCION"].Width = 200;
           gridView1.Columns["MINIMO"].Visible = false;

            
        }
        bool banderaRepetido;
        double TotalPedido;
        double TempTotalPedido;//esta variable me sirve para pasar el total del pedido y podes sacar el descuento
        double TotalDescuento;
        private void HijoPadre()
        {
            DataTable padre = new DataTable("Padre");
            padre.Columns.Add("IDBODEGA");
            padre.Columns.Add("BODEGA");
            padre.Columns.Add("CODIGO");
            padre.Columns.Add("DESCRIPCION");
            padre.Columns.Add("CANTIDAD");
            padre.Columns.Add("PRECIO UNITARIO");
            padre.Columns.Add("SUB TOTAL");
            DataTable hijo = new DataTable("Hijo");
            hijo.Columns.Add("CODIGOPADRE");
            hijo.Columns.Add("IDBODEGA");
            hijo.Columns.Add("BODEGA");
            hijo.Columns.Add("CODIGO");
            hijo.Columns.Add("DESCRIPCION");
            hijo.Columns.Add("CANTIDAD");
            hijo.Columns.Add("PRECIO UNITARIO");
            hijo.Columns.Add("SUB TOTAL");

            padre.Rows.Add("1","CENTRAL","ART-001","PRODUCTOS ALMACENADO","2","125","250");
            hijo.Rows.Add("ART-001","1", "CENTRAL", "BRS-001", "SSSS ALMACENADO", "4", "125", "250");
            hijo.Rows.Add("ART-001", "1", "CENTRAL", "BRS-050", "CLAVOS", "3", "125", "250");
            hijo.Rows.Add("ART-002", "1", "CENTRAL", "DDR-001", "TORNILLOS", "5", "425", "850");
            DataSet dataset = new DataSet();
            dataset.Tables.Add(padre);
            dataset.Tables.Add(hijo);
            dataset.Relations.Add("Detalle", dataset.Tables["Padre"].Columns["CODIGO"], dataset.Tables["Hijo"].Columns["CODIGOPADRE"], false);
            gridControl1.DataSource = padre;
            gridView1.AddNewRow();            
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", "1");
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", "CENTRAL");
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", "ART-002");
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", "5");
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO UNITARIO", "100");
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", "500");
            gridView1.UpdateCurrentRow();
            //gridView2.AddNewRow();
            //gridView2.SetRowCellValue(gridView2.FocusedRowHandle,"CODIGOPADRE","ART-002");
            //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "IDBODEGA", "1");
            //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "BODEGA", "CENTRAL");
            //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CODIGO", "ART-012");
            //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CANTIDAD", "5");
            //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "PRECIO UNITARIO", "100");
            //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "SUB TOTAL", "500");
            //gridView2.UpdateCurrentRow();
           
            
        }
        private void sbAgregaArt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (dxValidationProvider1.Validate())
                {
                    DataTable TempoPadre = new DataTable();
                    /* cadena = "SELECT articulos.compuesto FROM articulos WHERE articulos.codigo_articulo='"+id_articulo+"'";
                    jramirez 2013.07.04 */                    
                    cadena = "select f_es_compuesto('" + id_articulo + "') AS compuesto;";
                    string compuesto = logicaorto.Tabla(cadena).Rows[0]["compuesto"].ToString();
                    if (Convert.ToBoolean(logicaorto.Tabla(cadena).Rows[0]["compuesto"]))
                    {
                        /* cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',articulos.precio_venta,articulos.costo,articulos.minimo FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND articulos.codigo_padre='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue; */
                        cadena = "CALL sp_devuelve_sistema_k ('" + id_articulo + "'," + gridLookBodega.EditValue + ")";
                        TempoPadre = logicaorto.Tabla(cadena);
                        int ExistenciaHijo;
                        int ExistenciaFija;


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

                                if (x == 0)
                                {
                                    ExistenciaHijo = Convert.ToInt32(TempoPadre.Rows[x]["EXISTENCIA"]);
                                    if (ExistenciaHijo == 0)
                                    {
                                        clases.ClassMensajes.NoHayExistenciaProd(this);
                                        return;
                                    }
                                }
                                //if(ExistenciaHijo!=0)
                                //{
                                //if (Convert.ToInt32(textCantidadArt.Text) <= ExistenciaHijo)

                                if (Convert.ToInt32(TempoPadre.Rows[x]["cantidad"]) <= ExistenciaHijo)
                                {
                                    //ExistenciaFija = Convert.ToInt32(textCantidadArt.Text);
                                    ExistenciaFija = Convert.ToInt32(TempoPadre.Rows[x]["cantidad"]);
                                }
                                else
                                {
                                    ExistenciaFija = ExistenciaHijo;
                                }
                                gridView1.AddNewRow();
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", TempoPadre.Rows[x]["CODIGO"]);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", TempoPadre.Rows[x]["NOMBRE ARTICULO"]);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", ExistenciaFija);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO UNITARIO", TempoPadre.Rows[x]["precio_venta"]);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", (Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"]) * ExistenciaFija));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIABODEGA", ExistenciaHijo);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "MINIMO", TempoPadre.Rows[x]["minimo"]);
                                gridView1.UpdateCurrentRow();
                                TotalPedido = TotalPedido + (ExistenciaFija * Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"]));
                                CalculaDescuento(); 
                            //}

                            }
                            else
                                clases.ClassMensajes.ProdYaExisteEnListado(this);
                        }
                        
                        textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text="";
                        textCodigoArt.Focus();
                    }
                    else
                    {
                        if (Convert.ToInt32(textCantidadArt.Text) <= ExistenciaProd)
                        {
                            banderaRepetido = true;
                            for (int x = 0; x < gridView1.DataRowCount; x++)
                            {
                                if (gridView1.GetRowCellValue(x, "CODIGO").ToString() == textCodigoArt.Text & gridView1.GetRowCellValue(x, "IDBODEGA").ToString() == gridLookBodega.EditValue.ToString())
                                    banderaRepetido = false;
                            }
                            if (banderaRepetido)
                            {
                                gridView1.AddNewRow();
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", textCodigoArt.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", textNombreArti.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(textCantidadArt.Text));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO UNITARIO", textVenta.Text);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", (Convert.ToDouble(textVenta.Text) * Convert.ToDouble(textCantidadArt.Text)));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIABODEGA",ExistenciaProd);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "MINIMO", existencia_minima);
                                gridView1.UpdateCurrentRow();
                                TotalPedido = TotalPedido + (Convert.ToDouble(textCantidadArt.Text) * Convert.ToDouble(textVenta.Text));
                                CalculaDescuento();
                                textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text =textVenta.Text= "";
                                textCodigoArt.Focus();
                            }
                            else
                                clases.ClassMensajes.ProdYaExisteEnListado(this);
                        }
                        else
                            clases.ClassMensajes.NoHayExistenciaProd(this);
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
        bool Band_permite_borrar;// esta me sirve para poder borrar del listado si no hay error de validadcion en el gridview....
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (Band_permite_borrar)
                {
                    TotalPedido = TotalPedido - (Convert.ToDouble(gridView1.GetFocusedRowCellValue("CANTIDAD")) * Convert.ToDouble(gridView1.GetFocusedRowCellValue("PRECIO UNITARIO")));
                    gridView1.DeleteSelectedRows();
                    gridView1.UpdateCurrentRow();
                    textTotalPedido.Text = TotalPedido.ToString("C");
                }
            }
            catch
            { }
        }
        string id_cliente;
        string id_medico;
        string id_SocioComercialCompara;
        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL',clientes.nit AS 'NIT',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes where estadoid<>2";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                id_cliente = clases.ClassVariables.id_busca;
                DataTable tempCliente=new DataTable();
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_casa,contacto FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                tempCliente=logicaorto.Tabla(cadena);
                textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                textNitCliente.Text = tempCliente.Rows[0]["nit"].ToString();
                id_socioComercial = tempCliente.Rows[0]["referido_por"].ToString();
                textUtilizadoPaciente.Text = tempCliente.Rows[0]["nombre_paciente"].ToString();
                textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_casa"].ToString();
                textDoctorPedido.Text = tempCliente.Rows[0]["contacto"].ToString(); 
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                try
                {
                    tempCliente = logicaorto.Tabla(cadena);
                    id_SocioComercialCompara = id_socioComercial;
                    textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();                    
                }
                catch
                { }
            }
            e.KeyChar = Convert.ToChar(13);
        }
        string id_usuario_descuento;
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            MiniLogin.LoginMini.id_UsuarioModifica = "";
            Form logins = new MiniLogin.LoginMini();
            logins.ShowDialog();
            if(MiniLogin.LoginMini.id_UsuarioModifica!="")
            {
                id_usuario_descuento = MiniLogin.LoginMini.id_UsuarioModifica;
                textDescuentoPorce.Enabled = true;
                simpleButton4.Visible = false;
                simpleButton5.Visible = true;
            }
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            textDescuentoPorce.Enabled = false;
            simpleButton4.Visible =true;
            simpleButton5.Visible = false;
        }

        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {
            
        }
        private void limpia()
        {
            textHospital.Text=textPacientePedido.Text=textDoctorPedido.Text=textTelefonoCliente.Text= textNitCliente.Text= textSocioComercial.Text = textTotalDescuento.Text = textTotalPedido.Text = textVenta.Text = textNoDocumento.Text = textNombreCliente.Text = textNombreArti.Text = memoDescripcion.Text ="";
            gridLookBodega.Text = gridLookTipoPago.Text = gridLookTipoDocumento.Text = "";
            gridLookTipoDocumento.EditValue = 6;
            TotalDescuento = 0;
            TotalPedido = 0;
            totalVale = 0;
            TempTotalPedido = 0;
            textDescuentoPorce.Text = "0";
            textTotalDescuento.Text = TotalPedido.ToString("C");
            textTotalPedido.Text = TotalPedido.ToString("C");
            sbSaveReciboCaja.Enabled = true;
            llenaVale();
            CreaColumnas();
        }
        private void sbnuevo_Click(object sender, EventArgs e)
        {
            if (textNombreCliente.Text != "")
            {
                if (MessageBox.Show("¿ESTA SEGURO DE HABER TERMINADO EL PEDIDO ANTERIO?", "INFORMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    id_socioComercial = "";
                    sbAceptar.Enabled = true;
                    simplePrinter.Enabled = false;
                    simpleButton9.Enabled = true;
                    groupControl1.Enabled = true;
                    groupControl2.Enabled = true;
                    panelControl1.Enabled = true;
                    xtraTabPage2.PageEnabled = false;
                    xtraTabPage1.PageEnabled = false;
                    limpia();                    
                }
            }
        }

        private void simplePrinter_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //EnvioDetallado.DataSet_EnvioDetallado2TableAdapters.DataTable1TableAdapter nt = new EnvioDetallado.DataSet_EnvioDetallado2TableAdapters.DataTable1TableAdapter();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                DataTable nt = new DataTable();
                ad.Fill(nt);

                EnvioDetallado.XtraReportEnvioDetallado reporte = new EnvioDetallado.XtraReportEnvioDetallado();

                reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                reporte.Parameters["telefonos"].Value = "Telefono: " + nt.Rows[0]["telefono"].ToString() + ", Telefono auxiliar: " + nt.Rows[0]["telefono2"].ToString() + ".";
                
                reporte.Parameters["ID"].Value = valorreporte;
                reporte.RequestParameters = false;
                reporte.ShowPreviewDialog();
            }
            catch
            {
            }
            this.Cursor = Cursors.Default;
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textNombreArti_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void llenaVale()
        {
            DataTable temporal = new DataTable();            
            temporal.Columns.Add("DESCRIPCION");
            temporal.Columns.Add("CANTIDAD");
            temporal.Columns.Add("PRECIO UNITARIO");
            temporal.Columns.Add("TOTAL");
            gridControl2.DataSource = temporal;
            gridView3.Columns["DESCRIPCION"].Width = 500;
            gridView3.Columns["CANTIDAD"].Width = 60;
            gridView3.Columns["PRECIO UNITARIO"].Width = 70;
            gridView3.Columns["TOTAL"].Width = 70;
            /* Editable */
            gridView3.Columns["DESCRIPCION"].OptionsColumn.ReadOnly=true;
            gridView3.Columns["CANTIDAD"].OptionsColumn.ReadOnly = false;
            gridView3.Columns["PRECIO UNITARIO"].OptionsColumn.ReadOnly = false;
            gridView3.Columns["TOTAL"].OptionsColumn.ReadOnly = true;


            /* Por default Se usa la fecha del dia */
            dateEdit1.EditValue = DateTime.Now;
            textNoRecibo.Text = textNoReciboVale.Text;            
            textTotalVale.Text = textPor.Text;
            textHospital.Text = textSocioComercial.Text;           
            /* Se llena Combo de Series*/
            try
            {
                cadena = "SELECT codigo_serie CODIGO,serie_documento AS 'SERIE DOCUMENTO' FROM series_documentos INNER JOIN tipos_documento ON series_documentos.codigo_tipo = tipos_documento.codigo_tipo WHERE tipos_documento.codigo_tipo=3";
                gridLookSerieVale.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookSerieVale.Properties.DisplayMember = "SERIE DOCUMENTO";
                gridLookSerieVale.Properties.ValueMember = "CODIGO";
                gridLookSerieVale.EditValue = 4;
                //gridLookTipoDocumento.Text = "";
            }
            catch
            { }
        }
        string abono, cancelacion, otro;//solo me sirven para poder generar el recibo
        int id_recibos;//ehernandez cambios en la progra 28/05/2012
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (dxValidationRecibo.Validate())
            {
                cadena="SELECT * FROM recibos WHERE recibos.codigo_serie="+gridLookSerieRecibo.EditValue+" and recibos.no_recibo="+textNoRecibo.Text;
                if (logicaorto.ExisteRegistro(cadena) == false)
                {
                    try
                    {

                        if (radioGroup1.SelectedIndex == 0)
                        {
                            abono = "X";
                            cancelacion = "";
                            otro = "";
                        }
                        else
                            if (radioGroup1.SelectedIndex == 1)
                            {
                                abono = "";
                                cancelacion = "X";
                                otro = "";
                            }
                            else
                                if (radioGroup1.SelectedIndex == 2)
                                {
                                    abono = "";
                                    cancelacion = "";
                                    otro = "X";
                                }
                        conexion.Open();
                        transa = conexion.BeginTransaction();

                        string tempValor = textPor.Text.Replace(",", "");
                        cadena = "INSERT into recibos(no_recibo, codigo_serie, codigo_cliente, fecha_creacion, no_pedido, tipo_pago, monto, usuario_creador, estadoid) " +
                                "VALUES (" + textNoRecibo.Text + ", 3, " + id_cliente + ", '" + Convert.ToDateTime(dateFechaRecibo.EditValue).ToString("yyyy-MM-dd HH:mm:ss") + "', " + id_nuevo_vale + ", 1, " + tempValor.Replace("Q", "") + ", " + clases.ClassVariables.id_usuario + ", 4);SELECT LAST_INSERT_ID();";
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        id_recibos=Convert.ToInt32(comando.ExecuteScalar());
                        cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                                "VALUES (" + id_cliente + ", " + id_nuevo_vale + "," + id_recibos.ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        comando.ExecuteNonQuery();
                        transa.Commit();
                        //ReciboCaja.DataSetReciboCaja dataset = new ReciboCaja.DataSetReciboCaja();
                        //dataset.Tables["recibos"].Rows.Add(textNoRecibo.Text, dateFechaRecibo.DateTime, 1, textPor.Text.Replace("Q", ""), clases.ClassVariables.id_usuario, 1, textFacturas.Text, textCheque.Text, textBanco.Text, textValor.Text, abono, cancelacion, otro, textCantidadDe.Text, textRecibimosDe.Text, id_cliente, textEfectivo.Text);
                        //ReciboCaja.XtraReportReciboCaja reporte = new ReciboCaja.XtraReportReciboCaja();
                        //reporte.DataSource = dataset;
                        //reporte.DataMember = dataset.Tables["recibos"].TableName;
                        //reporte.RequestParameters = false;
                        //reporte.ShowPreviewDialog();
                        xtraTabPage1.PageEnabled = true;
                        llenaPedido();
                        sbSaveReciboCaja.Enabled = false;
                    }
                    catch
                    {
                        clases.ClassMensajes.NoINSERTO(this);
                        transa.Rollback();
                    }
                    finally
                    {
                        conexion.Close();
                    }
                }
                else
                {
                    alertControl1.Show(this, "INFORMACION", "EL NUMERO DE DOCUMENTO YA EXISTE", Properties.Resources.Advertencia64);
                }
            }
            else
            {
                clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
        }
        private void llenaPedido()
        {
            textPacientePedido.Text = textUtilizadoPaciente.Text;
            textDoctorPedido.Text = gridLookDoctores.Text;
            textHospital.Text = textSocioComercial.Text;
            dateFechaPedido.DateTime = DateTime.Now;
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.idnuevo = "";
            clases.ClassVariables.llamadoDentroForm = true;
            Form hijo = new Clientes.form_cliente();
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
            if (clases.ClassVariables.idnuevo != "")
            {
                id_cliente = clases.ClassVariables.idnuevo;
                DataTable tempCliente = new DataTable();
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.telefono_casa,clientes.nombre_paciente FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                tempCliente = logicaorto.Tabla(cadena);
                textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                textNitCliente.Text = tempCliente.Rows[0]["nit"].ToString();
                textUtilizadoPaciente.Text = tempCliente.Rows[0]["nombre_paciente"].ToString();
                textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_casa"].ToString();
                
            }

        }
        private void CalculaDescuento()
        {
            try
            {
                if (Convert.ToDouble(textDescuentoPorce.Text.Replace("%", "")) >= 0)
                {
                    if (textDescuentoPorce.Text.Contains("%"))
                    {
                        TotalDescuento = TotalPedido * ((Convert.ToDouble(textDescuentoPorce.Text.Replace("%", ""))) / 100);
                        TempTotalPedido = TotalPedido - TotalDescuento;
                        textTotalPedido.Text = TempTotalPedido.ToString("C");
                        textTotalDescuento.Text = TotalDescuento.ToString("C");
                    }
                    else
                    {
                        TotalDescuento = Convert.ToDouble(textDescuentoPorce.Text);
                        TempTotalPedido = TotalPedido - TotalDescuento;
                        textTotalPedido.Text = TempTotalPedido.ToString("C");
                        textTotalDescuento.Text = TotalDescuento.ToString("C");
                    }
                }
                else
                {
                    textDescuentoPorce.Text = "0";
                    TotalDescuento = TotalPedido * ((Convert.ToDouble(textDescuentoPorce.Text)) / 100);
                    TempTotalPedido = TotalPedido - TotalDescuento;
                    textTotalPedido.Text = TempTotalPedido.ToString("C");
                    textTotalDescuento.Text = TotalDescuento.ToString("C");
                }

            }
            catch { }
        }
        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            CalculaDescuento();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            Form hijo = new Series.SerieDoc();
            clases.ClassVariables.idnuevo = "";
            clases.ClassVariables.llamadoDentroForm = true;
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
             DataTable dt = new DataTable();
            try
            {
                
                cadena = "SELECT codigo_serie CODIGO,serie_documento AS 'SERIE DOCUMENTO' FROM series_documentos INNER JOIN tipos_documento ON series_documentos.codigo_tipo = tipos_documento.codigo_tipo WHERE tipos_documento.codigo_tipo=5";
                gridLookTipoDocumento.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoDocumento.Properties.DisplayMember = "SERIE DOCUMENTO";
                gridLookTipoDocumento.Properties.ValueMember = "CODIGO";
               gridLookTipoDocumento.EditValue = clases.ClassVariables.idnuevo;
            }
            catch
            { }
                        

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            clases.ClassVariables.idnuevo = "";
            Form hijo = new TipoPago.TipPago();
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
            DataTable dt = new DataTable();
            try
            {
                cadena = "SELECT tipo_pago as CODIGO, nombre_tipo_pago AS 'TIPO PAGO' FROM tipo_pago where estadoid<>2";
                gridLookTipoPago.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoPago.Properties.DisplayMember = "TIPO PAGO";
                gridLookTipoPago.Properties.ValueMember = "CODIGO";
                gridLookTipoPago.Text = "";
                gridLookTipoPago.EditValue = clases.ClassVariables.idnuevo;
            }
            catch
            { }
            

        }

        private void simpleButton6_Click_1(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.sociocomercial = true;
            clases.ClassVariables.llamadoDentroForm = true;
            clases.ClassVariables.idnuevo = "";
            Form hijo = new Clientes.form_cliente();
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
            if(clases.ClassVariables.idnuevo!="")
            {
            id_cliente = clases.ClassVariables.idnuevo;
            DataTable tempCliente = new DataTable();
            cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE' FROM clientes "+
                        "WHERE socio_comercial=1 AND clientes.codigo_cliente=" + id_cliente;
            tempCliente = logicaorto.Tabla(cadena);
            textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        int cant1, cant2;
        bool bandera_carga_cotizacion = false;
        private void gridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            ColumnView ver = sender as ColumnView;
            cant1 = Convert.ToInt32(ver.GetFocusedRowCellValue("CANTIDAD"));
            cant2 = Convert.ToInt32(ver.GetFocusedRowCellValue("EXISTENCIABODEGA"));
            if (bandera_carga_cotizacion == false)
            {
                if (cant1 > cant2)
                {
                    e.Valid = false;
                    ver.SetColumnError(gridView1.Columns["CANTIDAD"], "NO HAY EXISTENCIA DE PRODUCTO SOLO HAY " + cant2.ToString());
                    Band_permite_borrar = false;
                }
                else
                {
                    Band_permite_borrar = true;
                    TotalPedido = 0;
                    for (int x = 0; x < gridView1.DataRowCount; x++)
                    {
                        TotalPedido = TotalPedido + (Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO UNITARIO")));
                        gridView1.SetRowCellValue(x, "SUB TOTAL", Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO UNITARIO")));
                    }
                    CalculaDescuento();
                }
            }
        }
        double totalVale;
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            try
            {
                if (dxValidationDetalleVale.Validate())
                {
                    gridView3.AddNewRow();
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "DESCRIPCION", textDetalleVale.Text);
                    
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "CANTIDAD", textCantidadVale.Text);
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "PRECIO UNITARIO", textUnitarioVale.Text);
                    /* Validacion para evitar error si el precio unitario es 0 jramirez 20130409 */
                    if ((Convert.ToDouble(textUnitarioVale.Text) != 0) && (textUnitarioVale.Text.Length > 0))
                    {
                        gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "TOTAL", (Convert.ToDouble(textCantidadVale.Text) * Convert.ToDouble(textUnitarioVale.Text)));
                        totalVale = totalVale + (Convert.ToDouble(textCantidadVale.Text) * Convert.ToDouble(textUnitarioVale.Text));
                        textTotalVale.Text = totalVale.ToString("C");
                    }
                    else
                    {
                        gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "TOTAL", 0);
                    }
                    gridView3.UpdateCurrentRow();
                    textDetalleVale.Text = textCantidadVale.Text = ""; 
                    textUnitarioVale.Text = "0";
                    textDetalleVale.Focus();
                }
                else
                    clases.ClassMensajes.FaltanDatosEnCampos(this);
                
            }
            catch (Exception y)
            {
                MessageBox.Show(y.ToString(), "Error");
            }
        }
        string id_nuevo_vale;
        private void insertaVale()
        {
            DataTable DatoTemp = new DataTable();
            try
            {
                //cadena = "SELECT tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento, fecha_modificacion, fecha_retorno, usuario_modifica, socio_comercial, razon_ajuste, descripcion, estadoid, contado_credito FROM header_doctos_inv where id_documento=" + id_nuevo_pedido ;
                //DatoTemp = logicaorto.Tabla(cadena);
                conexion.Open();
                transa = conexion.BeginTransaction();
                if (id_socioComercial != id_SocioComercialCompara & textSocioComercial.Text != "")
                {
                    cadena = "UPDATE clientes SET clientes.referido_por=" + id_socioComercial + " WHERE clientes.codigo_cliente=" + id_cliente;
                    comando = new MySqlCommand(cadena, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                }
                int tipo_pago= Convert.ToInt32(gridLookTipoPago.EditValue);
                string desc="";
                string depo = textDeposito.Text;
                if (depo.Length > 0 )
                        tipo_pago = 3;
                cadena = "INSERT into header_doctos_inv(codigo_serie,tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, socio_comercial, estadoid, contado_credito,refer_documento,doctor) "+
                         "VALUES (" + gridLookSerieVale.EditValue + ","+tipo_pago+", '" + textNumeroDocVale.Text+ "', " + id_cliente+ ", '" +dateEdit1.DateTime.ToString("yyyy-MM-dd") + "', " + totalVale+ ",0, " + totalVale+ ", " + clases.ClassVariables.id_usuario+ ",'" +id_socioComercial+ "',8,"+radioGroup2.SelectedIndex+",'"+textDeposito.Text+"',"+gridLookDoctores.EditValue.ToString()+");SELECT LAST_INSERT_ID();";
                comando = new MySqlCommand(cadena,conexion);
                comando.Transaction = transa;
                id_nuevo_vale = comando.ExecuteScalar().ToString();                
                cadena = "";
                for (int x = 0; x < gridView3.DataRowCount; x++)
                {
                    desc = gridView3.GetRowCellValue(x, "DESCRIPCION").ToString().Replace("'", " "); ;                    
                    cadena =cadena+ "INSERT into detalle_manual(id_documento, descripcion, cantidad, precio_unitario, precio_total) " +
                                "VALUES (" + id_nuevo_vale + ",'" + desc + "', " + gridView3.GetRowCellValue(x, "CANTIDAD") + ", " + gridView3.GetRowCellValue(x, "PRECIO UNITARIO") + ", " + gridView3.GetRowCellValue(x, "TOTAL") + ");";
                    desc="" ;
                }
                
                comando = new MySqlCommand(cadena,conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                            "VALUES (" + id_cliente + ", " + id_nuevo_vale + ","+id_nuevo_vale+", '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                transa.Commit();
                simpleButton9.Enabled = false;
                groupControl5.Enabled = false;

                if (Class_integracion.logeado == true)
                {
                    GuardarValeEnINVEX();
                }



                clases.ClassMensajes.INSERTO(this);
                textValor.Text = totalVale.ToString();
                simpleButton9.Enabled = true;                
                if (radioGroup2.SelectedIndex == 0)
                {
                    LlenaDatosRecibo();
                    xtraTabPage2.PageEnabled = true;
                }
                else
                {
                    xtraTabPage1.PageEnabled = true;
                    llenaPedido();
                }


                
            }
            catch
            {
                transa.Rollback();
                clases.ClassMensajes.NoINSERTO(this);
            }
            finally
            {
                conexion.Close();
            }
        }

        

        private void gridLookSerieVale_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC' FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_tipo=3 AND series_documentos.codigo_serie=" + gridLookSerieVale.EditValue + " ORDER BY header_doctos_inv.no_documento DESC LIMIT 1";
                textNumeroDocVale.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
            }
            catch
            {
                textNumeroDocVale.Text = "1";
            }
        }

        private void textPor_EditValueChanged(object sender, EventArgs e)
        {
            textCantidadDe.Text = logicaorto.enletras(textPor.Text.Replace("Q", ""));
        }
        /* Eliminar Fila*/
        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                /* jramirez 20130409 validacion para que no de error si el precio es 0*/
                if (( gridView3.GetFocusedRowCellValue("CANTIDAD").ToString() != "0") && ( gridView3.GetFocusedRowCellValue("PRECIO UNITARIO").ToString() != "0"))
                    totalVale = totalVale - (Convert.ToDouble(gridView3.GetFocusedRowCellValue("CANTIDAD"))*Convert.ToDouble(gridView3.GetFocusedRowCellValue("PRECIO UNITARIO")));
                gridView3.DeleteSelectedRows();
                gridView3.UpdateCurrentRow();
                textTotalVale.Text = totalVale.ToString("C");              
                
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString(), "Error");
            }
        }

        private void gridLookTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                /* cadena = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid<>2"; */
                /*jramirez 2013.07.24 */
                cadena = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE  FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_serie= " + gridLookTipoDocumento.EditValue;
                gridLookBodega.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
                // gridLookBodega.EditValue = 1;  
            }
            catch
            { }

            try
            {
                cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC',header_doctos_inv.codigo_serie FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_tipo=5 AND series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue + " ORDER BY header_doctos_inv.no_documento DESC LIMIT 1";
                textNoDocumento.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                // CargaBodega(int.Parse(gridLookTipoDocumento.EditValue.ToString()));
                CargaBodega(int.Parse(logicaorto.Tabla(cadena).Rows[0][1].ToString()));
                
            }
            catch
            {
                textNoDocumento.Text = "1";
            }
            
        }

        private void textNumeroDocVale_EditValueChanged(object sender, EventArgs e)
        {
            // MessageBox.Show(textNumeroDocVale.Text, "");
        }

        private void CargaDatosValeSeguir_NoDep()
        {
            try
            {
                gridView3.SelectAll();
                gridView3.DeleteSelectedRows();

                DataTable tablatemporal = new DataTable();
                cadena = "SELECT	* FROM detalle_manual WHERE detalle_manual.id_documento=" + id_nuevo_vale;
                tablatemporal = logicaorto.Tabla(cadena);
                foreach (DataRow fila in tablatemporal.Rows)
                {
                    gridView3.AddNewRow();
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "DESCRIPCION", fila["descripcion"]);
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(fila["cantidad"]));
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "PRECIO UNITARIO", fila["precio_unitario"]);
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "TOTAL", fila["precio_total"]);
                    totalVale = totalVale + Convert.ToDouble(fila["precio_total"]);
                    gridView3.UpdateCurrentRow();
                }
                
                
            }
            catch
            { }
        }
        //private void limpiarGrid()
        //(     
        
            
            

        //        foreach (DataRow fila in tablatemp.Rows)
        //        {
        //            gridView3.AddNewRow();                    
        //            gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "DESCRIPCION", fila["descripcion"]);
        //            gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(fila["cantidad"]));
        //            gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "PRECIO UNITARIO", fila["precio_unitario"]);
        //            gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "TOTAL", fila["precio_total"]);
        //            totalVale = totalVale+ Convert.ToDouble(fila["precio_total"]);
        //            gridView3.UpdateCurrentRow();
        //        }
        
        //);

        private void CargaDatosValeSeguir()
        {
            try
            {
                // limpiarGrid();
                gridView3.SelectAll();
                gridView3.DeleteSelectedRows();
                
                DataTable tablatemporal = new DataTable();
                cadena = "SELECT	* FROM detalle_manual WHERE detalle_manual.id_documento=" + id_nuevo_vale;
                tablatemporal = logicaorto.Tabla(cadena);
                

                foreach (DataRow fila in tablatemporal.Rows)
                {
                    gridView3.AddNewRow();                    
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "DESCRIPCION", fila["descripcion"]);
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(fila["cantidad"]));
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "PRECIO UNITARIO", fila["precio_unitario"]);
                    gridView3.SetRowCellValue(gridView3.FocusedRowHandle, "TOTAL", fila["precio_total"]);
                    totalVale = totalVale+ Convert.ToDouble(fila["precio_total"]);
                    gridView3.UpdateCurrentRow();
                }
                if (radioGroup2.SelectedIndex == 0)
                {
                    LlenaDatosRecibo();
                    xtraTabPage2.PageEnabled = true;
                    simpleButton11.Enabled = false;
                    groupControl5.Enabled = false;
                }
                else
                {
                    xtraTabPage1.PageEnabled = true;
                    llenaPedido();
                }
            }
            catch
            { }
        }
        private void SbNoterminado_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            cadena = "SELECT header_doctos_inv.id_documento AS 'CODIGO',header_doctos_inv.fecha AS 'FECHA PEDIDO',header_doctos_inv.no_documento 'NUMERO DOCUMENTO',clientes.nombre_cliente AS 'NOMBRE CLIENTE',header_doctos_inv.descripcion AS 'DESCRIPCION',header_doctos_inv.monto_neto AS 'TOTAL PEDIDO' FROM header_doctos_inv INNER JOIN series_documentos ON series_documentos.codigo_serie=header_doctos_inv.codigo_serie INNER JOIN clientes ON header_doctos_inv.codigo_cliente=clientes.codigo_cliente WHERE series_documentos.codigo_tipo=3 AND (header_doctos_inv.estadoid=8 )";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {

                id_nuevo_vale = clases.ClassVariables.id_busca;
                DataTable tempLlena = new DataTable();
                cadena = "SELECT cli.referido_por,cli.nombre_paciente,cli.nombre_cliente,cab.fecha,cli.codigo_cliente,cli.nit,cab.codigo_serie,cab.tipo_pago,cab.no_documento,cab.descripcion,cab.razon_ajuste,cab.descuento,cab.monto_neto,cab.contado_credito FROM header_doctos_inv cab INNER JOIN clientes cli ON cab.codigo_cliente=cli.codigo_cliente WHERE cab.id_documento=" + id_nuevo_vale;
                tempLlena = logicaorto.Tabla(cadena);
                textNombreCliente.Text = tempLlena.Rows[0]["nombre_cliente"].ToString();
                textUtilizadoPaciente.Text=tempLlena.Rows[0]["nombre_paciente"].ToString();
                id_cliente = tempLlena.Rows[0]["codigo_cliente"].ToString();
                id_socioComercial = tempLlena.Rows[0]["referido_por"].ToString();
                textNitCliente.Text = tempLlena.Rows[0]["nit"].ToString();
                gridLookTipoDocumento.EditValue = tempLlena.Rows[0]["codigo_serie"].ToString();
                gridLookTipoPago.EditValue = tempLlena.Rows[0]["tipo_pago"].ToString();
                textNoDocumento.Text = tempLlena.Rows[0]["no_documento"].ToString();
                memoDescripcion.Text = tempLlena.Rows[0]["descripcion"].ToString();
                dateEdit1.DateTime = Convert.ToDateTime(tempLlena.Rows[0]["fecha"]);
                textTotalDescuento.Text = Convert.ToDouble(tempLlena.Rows[0]["descuento"]).ToString("C");
                textTotalPedido.Text = Convert.ToDouble(tempLlena.Rows[0]["monto_neto"]).ToString("C");
                textTotalVale.Text = Convert.ToDouble(tempLlena.Rows[0]["monto_neto"]).ToString("C");
                radioGroup1.SelectedIndex = Convert.ToInt16(Convert.ToBoolean(tempLlena.Rows[0]["contado_credito"]));
                textNumeroDocVale.Text = tempLlena.Rows[0]["no_documento"].ToString();
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                try
                {
                    tempLlena = logicaorto.Tabla(cadena);
                    id_SocioComercialCompara = id_socioComercial;
                    textSocioComercial.Text = tempLlena.Rows[0]["NOMBRE CLIENTE"].ToString();
                }
                catch
                { }                
                CargaDatosValeSeguir();
                cadena = "SELECT * FROM recibos WHERE recibos.id_recibos IN (SELECT relacion_venta.id_documento FROM relacion_venta WHERE relacion_venta.id_vale="+id_nuevo_vale+")";
                if (logicaorto.Tabla(cadena).Rows.Count > 0)
                {
                    cadena = "SELECT recibos.no_recibo FROM recibos WHERE recibos.id_recibos IN (SELECT relacion_venta.id_documento FROM relacion_venta WHERE relacion_venta.id_vale=" + id_nuevo_vale + ")";
                    textNoRecibo.Text=logicaorto.Tabla(cadena).Rows[0][0].ToString();
                    textNoReciboVale = textNoRecibo;
                    llenaPedido();
                    xtraTabPage1.PageEnabled = true;
                    sbSaveReciboCaja.Enabled = false;
                    
                }               
            }
            if (string.IsNullOrEmpty(id_nuevo_vale) != true)
                simpleButton9.Enabled = true;
            Cursor.Current = Cursors.Default;
            gridLookTipoDocumento.EditValue = 6;
        }

        private void textNitCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                cadena = "SELECT clientes.codigo_cliente FROM clientes WHERE clientes.nit='" + textNitCliente.Text + "'";
                tempTabla = logicaorto.Tabla(cadena);
                if (tempTabla.Rows.Count > 0)
                {
                    id_cliente = tempTabla.Rows[0][0].ToString();
                    DataTable tempCliente = new DataTable();
                    cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_casa,contacto FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                    tempCliente = logicaorto.Tabla(cadena);
                    textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                    textNitCliente.Text = tempCliente.Rows[0]["nit"].ToString();
                    id_socioComercial = tempCliente.Rows[0]["referido_por"].ToString();
                    textUtilizadoPaciente.Text = tempCliente.Rows[0]["nombre_paciente"].ToString();
                    textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_casa"].ToString();
                    textDoctorPedido.Text = tempCliente.Rows[0]["contacto"].ToString();
                    cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                    try
                    {
                        tempCliente = logicaorto.Tabla(cadena);
                        id_SocioComercialCompara = id_socioComercial;
                        textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                    }
                    catch
                    { }
                }
                else {
                    textNombreCliente.Text = "";
                    textNitCliente.Text = "";
                    textTelefonoCliente.Text = "";
                    textUtilizadoPaciente.Text = "";
                
                }
            }
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.SelectedIndex == 1)
            {
                gridLookTipoPago.EditValue = 5;
                gridLookTipoPago.Enabled = false;
            }
            else
            {
                gridLookTipoPago.EditValue = 1;
                gridLookTipoPago.Enabled = true;
            }

        }

        private void gridLookSerieRecibo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                cadena = "SELECT (recibos.no_recibo+1)AS 'NODOC' FROM recibos INNER JOIN series_documentos ON recibos.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie=" + gridLookSerieRecibo.EditValue + " ORDER BY recibos.no_recibo DESC LIMIT 1";
                textNoRecibo.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
            }
            catch { }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "EXISTENCIABODEGA")
            {
                string existencia = view.GetRowCellDisplayText(e.RowHandle, view.Columns["EXISTENCIABODEGA"]);
                string minimo= view.GetRowCellDisplayText(e.RowHandle, view.Columns["MINIMO"]);
                try
                {
                    if (Convert.ToDouble(minimo) > 0 & Convert.ToDouble(existencia) <= Convert.ToDouble(minimo))
                    {
                        e.Appearance.BackColor = Color.Yellow;
                        e.Appearance.BackColor2 = Color.YellowGreen;
                    }
                }
                catch
                { }
                try
                {
                    if (Convert.ToDouble(existencia) == 0)
                    {
                        e.Appearance.BackColor = Color.OrangeRed;
                        e.Appearance.BackColor2 = Color.Red;
                    }
                }
                catch { }
            }
        }

        string id_cotizacion;
        private void simpleButton8_Click_1(object sender, EventArgs e)
        {
             Cursor.Current = Cursors.WaitCursor;
             cadena = "SELECT header_doctos_inv.id_documento AS 'CODIGO',header_doctos_inv.fecha AS 'FECHA PEDIDO',header_doctos_inv.no_documento 'NUMERO DOCUMENTO',clientes.nombre_cliente AS 'NOMBRE CLIENTE',header_doctos_inv.descripcion AS 'DESCRIPCION',header_doctos_inv.monto_neto AS 'TOTAL PEDIDO' FROM header_doctos_inv INNER JOIN series_documentos ON series_documentos.codigo_serie=header_doctos_inv.codigo_serie INNER JOIN clientes ON header_doctos_inv.codigo_cliente=clientes.codigo_cliente WHERE series_documentos.codigo_tipo=13 AND (header_doctos_inv.estadoid=4 ) and header_doctos_inv.codigo_cliente=" + id_cliente;
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                Cursor = Cursors.WaitCursor;
                id_cotizacion=clases.ClassVariables.id_busca;
                cadena = "SELECT bh.codigo_bodega AS IDBODEGA,bh.nombre_bodega AS BODEGA,ar.codigo_articulo AS CODIGO,ar.descripcion AS DESCRIPCION,ddi.cantidad_enviada AS CANTIDAD,ddi.precio_unitario AS 'PRECIO UNITARIO',ddi.precio_total AS 'SUB TOTAL',0 AS EXISTENCIABODEGA,ar.minimo AS MINIMO FROM detalle_doctos_inv ddi INNER JOIN articulos ar ON ddi.codigo_articulo=ar.codigo_articulo INNER JOIN bodegas_header bh ON ddi.codigo_bodega=bh.codigo_bodega WHERE id_documento=" + id_cotizacion;
                gridControl1.DataSource = logicaorto.Tabla(cadena);
                int existencia_tempora;
                bandera_carga_cotizacion = true;
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    cadena = "select existencia_articulo from bodegas where codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA").ToString() + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO").ToString() + "'";
                    existencia_tempora = Convert.ToInt32(logicaorto.Tabla(cadena).Rows[0][0]);
                    gridView1.SetRowCellValue(x, "EXISTENCIABODEGA", existencia_tempora);
                    if (existencia_tempora == 0)
                        gridView1.SetRowCellValue(x, "CANTIDAD", existencia_tempora);
                    else
                        if(Convert.ToInt32(gridView1.GetRowCellValue(x, "IDBODEGA").ToString())>existencia_tempora)
                            gridView1.SetRowCellValue(x, "CANTIDAD", existencia_tempora);
                    gridView1.UpdateCurrentRow();
                }
                bandera_carga_cotizacion = false;
                TotalPedido = 0;
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    TotalPedido = TotalPedido + (Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO UNITARIO")));
                    gridView1.SetRowCellValue(x, "SUB TOTAL", Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO UNITARIO")));
                }
                CalculaDescuento();
                Cursor = Cursors.Default;
             
            }
        }

        private void gridLookTipoPago_EditValueChanged(object sender, EventArgs e)
        {
            if ((radioGroup2.SelectedIndex == 0) && (Convert.ToInt16(gridLookTipoPago.EditValue) == 5))
            {
                gridLookTipoPago.EditValue = 0;
            }
        }


        private void simpleButton9_Click(object sender, EventArgs e)
        {
            /*Imprimir Vale*/
            Vale.DataSet_DGTableAdapters.clientesTableAdapter lg = new Vale.DataSet_DGTableAdapters.clientesTableAdapter();
            DataTable res = new DataTable();
            res=lg.GetData_encontrarabreviaturaSC(textSocioComercial.Text);
            string ab="";
            if(res.Rows.Count>0)
                ab = res.Rows[0][0].ToString();

            MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
            DataTable nt = new DataTable();
            ad.Fill(nt);


            Vale.XtraReportVale reporte = new Vale.XtraReportVale();

            reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
            reporte.Parameters["Direccion"].Value = "Direccion: " + nt.Rows[0]["direccion"].ToString();
            reporte.Parameters["telefono1"].Value = "Telefono: " + nt.Rows[0]["telefono"].ToString();
            reporte.Parameters["telefono2"].Value = "Telefono auxiliar: " + nt.Rows[0]["telefono2"].ToString();
            reporte.Parameters["correo"].Value = "Correo: " + nt.Rows[0]["correo"].ToString();
            
            reporte.Parameters["ID"].Value = id_nuevo_vale;
            reporte.Parameters["abrev"].Value = ab;

            
            // reporte.Parameters["RECIBO"].Value = textNoReciboVale.Text;
            // reporte.Parameters["SOCIO"].Value = textSocioComercial.Text;
            reporte.RequestParameters = false;
            reporte.ShowPreviewDialog();
           
            /**/
        }

        private void GuardarValeEnINVEX()
        {
            decimal total= Convert.ToDecimal(totalVale);
            string telefonoClienteB = logicaorto.Tabla("SELECT telefono_casa FROM clientes WHERE codigo_cliente=" + id_cliente).Rows[0][0].ToString();


            string fdPago = "";
                if (radioGroup2.SelectedIndex == 1)
                {
                    fdPago = "Credito";
                }
                else
                {
                    fdPago = gridLookTipoPago.Text;
                }

                if (fdPago == "Efectivo")
                {
                    Class_anticipo.AnticipoEnEfectivo(textNombreCliente.Text, textNitCliente.Text,telefonoClienteB, total);
                }
                else if (fdPago == "Cheque")
                {
                    f_ValidarPedido nf = new f_ValidarPedido();
                    nf.Cheque(textNombreCliente.Text, textNitCliente.Text,telefonoClienteB, total);
                    nf.ShowDialog();
                }
                else if (fdPago == "Deposito")
                {
                    f_ValidarPedido nf = new f_ValidarPedido();
                    nf.Deposito(textNombreCliente.Text, textNitCliente.Text,telefonoClienteB, total);
                    nf.ShowDialog();
                }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            /* Guardar Vale */
            if (dxValidationImprimeVale.Validate() & gridView3.DataRowCount > 0)
            {

                string fdPago = "";
                if (radioGroup2.SelectedIndex == 1)
                {
                    fdPago = "Credito";
                }
                else
                {
                    fdPago = gridLookTipoPago.Text;
                }

                if (fdPago == "Efectivo" || fdPago == "Cheque" || fdPago == "Deposito")
                {
                    cadena = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE header_doctos_inv.no_documento=" + textNumeroDocVale.Text + " AND series_documentos.codigo_serie=" + gridLookSerieVale.EditValue;
                    if (logicaorto.ExisteRegistro(cadena) == false)
                    {
                        insertaVale();
                        
                    }
                    else
                    {
                        if (guardaValeDeposito())
                            clases.ClassMensajes.INSERTO(this);
                        else
                            alertControl1.Show(this, "INFORMACION", "EL NUMERO DE DOCUMENTO YA EXISTE", Properties.Resources.Advertencia64);
                    }
                }
                else
                    MessageBox.Show("Seleccione una forma de pago valida, estas pueden ser:\nEfectivo\nCheque\nDeposito", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                
            }
            else
                clases.ClassMensajes.FaltanDatosEnCampos(this);
        }
        private Boolean guardaValeDeposito()
        {
            /* Si no hay número de depósito no se guarda nada */
            if (string.IsNullOrEmpty(textDeposito.Text))
                return false;
            DataTable DatoTemp = new DataTable();
            try
            {            
                conexion.Open();
                transa = conexion.BeginTransaction();
                /* jramirez 2013.07.04 Se actualizan todos los documentos relacionados con el número de depósito */
                cadena = "UPDATE header_doctos_inv SET header_doctos_inv.refer_documento='No. Deposito: " + textDeposito.Text + "', tipo_pago=3  WHERE codigo_cliente=" + id_cliente.ToString() + " and header_doctos_inv.id_documento IN (SELECT id_documento FROM relacion_venta WHERE id_vale=" + id_nuevo_vale.ToString() + " AND id_documento<>id_vale ) ";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                transa.Commit();
                simpleButton9.Enabled = false;
                groupControl5.Enabled = false;
            }
            catch
            {
                transa.Rollback();
                return false;                
            }
            finally
            {
                conexion.Close();                
            }
            return true;
        }

        /* Modificar Vales sin deposito*/
        private void sbSinDeposito_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            cadena = "";
            cadena = "SELECT * FROM v_vales_sin_deposito";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {

                groupControl5.Enabled = true;
                id_nuevo_vale = clases.ClassVariables.id_busca;
                DataTable tempLlena = new DataTable();
                cadena = "SELECT cli.referido_por,cli.nombre_paciente,cli.nombre_cliente,cab.fecha,cli.codigo_cliente,cli.nit,cab.codigo_serie,cab.tipo_pago,cab.no_documento,cab.refer_documento,cab.descuento,cab.monto_neto,cab.contado_credito FROM header_doctos_inv cab INNER JOIN clientes cli ON cab.codigo_cliente=cli.codigo_cliente WHERE cab.id_documento =" + id_nuevo_vale;
                tempLlena = logicaorto.Tabla(cadena);
                textNombreCliente.Text = tempLlena.Rows[0]["nombre_cliente"].ToString();
                textNombreCliente.Enabled = false;
                textUtilizadoPaciente.Text = tempLlena.Rows[0]["nombre_paciente"].ToString();
                textUtilizadoPaciente.Enabled = false;
                id_cliente = tempLlena.Rows[0]["codigo_cliente"].ToString();
                id_socioComercial = tempLlena.Rows[0]["referido_por"].ToString();
                textNitCliente.Text = tempLlena.Rows[0]["nit"].ToString();
                textNitCliente.Enabled = false;                                
                // textNumeroDocVale.Enabled = false;
                textNoReciboVale.Enabled = false;
                // memoDescripcion.Text = tempLlena.Rows[0]["descripcion"].ToString();
                // memoDescripcion.Enabled = false;
                dateEdit1.DateTime = Convert.ToDateTime(tempLlena.Rows[0]["fecha"]);
                dateEdit1.Enabled = false;
                textTotalDescuento.Text = Convert.ToDouble(tempLlena.Rows[0]["descuento"]).ToString("C");
                textTotalDescuento.Enabled = false;
                textTotalVale.Text = Convert.ToDouble(tempLlena.Rows[0]["monto_neto"]).ToString("C");
                textTotalVale.Enabled = false;
                radioGroup2.SelectedIndex = Convert.ToInt16(Convert.ToBoolean(tempLlena.Rows[0]["contado_credito"]));
                radioGroup2.Enabled = false;
                simpleButton10.Enabled = false;
                textTelefonoCliente.Enabled = false;
                gridLookSerieVale.EditValue = tempLlena.Rows[0]["codigo_serie"].ToString();
                gridLookSerieVale.Enabled = false;
                textSocioComercial.Enabled = false;
                gridLookDoctores.Enabled = false;
                /* unicos no bloqueados */
                gridLookTipoPago.EditValue = tempLlena.Rows[0]["tipo_pago"].ToString();
                textDeposito.Text = tempLlena.Rows[0]["refer_documento"].ToString();
                textNumeroDocVale.Text = tempLlena.Rows[0]["no_documento"].ToString();
                textNumeroDocVale.Enabled = false;
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                try
                {
                    tempLlena = logicaorto.Tabla(cadena);
                    id_SocioComercialCompara = id_socioComercial;
                    textSocioComercial.Text = tempLlena.Rows[0]["NOMBRE CLIENTE"].ToString();
                }
                catch
                { }                
                CargaDatosValeSeguir_NoDep();
                //xtraTabPage3.PageEnabled = true;
                
            }
            tieneDeposito = 1;
            if (string.IsNullOrEmpty(id_nuevo_vale) != true)
                simpleButton9.Enabled = true;
            simpleButton11.Enabled = true;
            xtraTabPage2.PageEnabled = false;
            xtraTabPage1.PageEnabled = false;
            Cursor.Current = Cursors.Default;
            gridLookTipoDocumento.EditValue = 6;

        }

        private void sbPrintReciboCaja_Click(object sender, EventArgs e)
        {
            ReciboCaja.DataSetReciboCaja dataset = new ReciboCaja.DataSetReciboCaja();
            dataset.Tables["recibos"].Rows.Add(textNoRecibo.Text, dateFechaRecibo.DateTime, 1, textPor.Text.Replace("Q", ""), clases.ClassVariables.id_usuario, 1, textFacturas.Text, textCheque.Text.Replace("Q", ""), textBanco.Text.Replace("Q", ""), textValor.Text.Replace("Q", ""), abono, cancelacion, otro, textCantidadDe.Text, textRecibimosDe.Text, id_cliente, textEfectivo.Text.Replace("Q", ""));
            ReciboCaja.XtraReportReciboCaja reporte = new ReciboCaja.XtraReportReciboCaja();
            reporte.DataSource = dataset;
            reporte.DataMember = dataset.Tables["recibos"].TableName;
            reporte.RequestParameters = false;
            reporte.ShowPreviewDialog();
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        
        private void groupControl1_Paint_1(object sender, PaintEventArgs e)
        {

        }

       

        private void textDetalleVale_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.idnuevo = "";
            clases.ClassVariables.llamadoDentroForm = true;
            Form hijo = new Clientes.frm_medico();
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
            if (clases.ClassVariables.idnuevo != "")
            {
                id_medico = clases.ClassVariables.idnuevo;                
                DataTable tempCliente = new DataTable();
                try
                {
                    cadena = "SELECT codigo_cliente AS codigodoc,nombre_cliente AS nombreDoctor FROM clientes WHERE clientes.`codigo_tipoc` =7 and estadoid=1";
                    gridLookDoctores.Properties.DataSource = logicaorto.Tabla(cadena);
                    gridLookDoctores.Properties.DisplayMember = "nombreDoctor";
                    gridLookDoctores.Properties.ValueMember = "codigodoc";                    
                    gridLookDoctores.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
                    gridLookDoctores.EditValue = id_medico;
                }
                catch
                {
                    cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.telefono_casa,clientes.nombre_paciente FROM clientes WHERE clientes.codigo_cliente=" + id_medico;
                    tempCliente = logicaorto.Tabla(cadena);
                    gridLookDoctores.EditValue = tempCliente.Rows[0]["CODIGO"].ToString();
                    gridLookDoctores.Text = tempCliente.Rows[0]["nombre_cliente"].ToString();
                }
                
            }
        }

        private void gridControl2_Validated(object sender, EventArgs e)
        {
            //recalcular el subtotal y total de cada columna
            int largo = gridView3.DataRowCount;
            double total = 0;

            int cant = 0;
            double pu = 0;
            double subtotal = 0;
            for (int i = 0; i < largo; i++)
            {
                subtotal = 0;
                cant = Convert.ToInt16(gridView3.GetRowCellValue(i, "CANTIDAD"));
                pu = Convert.ToDouble(gridView3.GetRowCellValue(i, "PRECIO UNITARIO"));
                subtotal = subtotal + (cant * pu);
                gridView3.SetRowCellValue(i, "TOTAL", subtotal);
                total = total + subtotal;
            }

            textTotalVale.Text = total.ToString();
        }

        private void gridView3_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            

        }

        private void gridView3_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
        }

        private void gridControl2_EmbeddedNavigator_Validated(object sender, EventArgs e)
        {
        }


         
               
    }
}