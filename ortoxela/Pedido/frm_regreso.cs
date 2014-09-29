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
using System.Globalization;

using DevExpress.XtraReports.UI;
using Sisnova.Invex.BL;
using integracion_ii;

namespace ortoxela.Pedido
{
    public partial class frm_regreso : DevExpress.XtraEditors.XtraForm
    {
        public frm_regreso()
        {
            InitializeComponent();
        }
        private void CargaDatosCombos()
        {
            try
            {
                cadena = "SELECT tipo_pago as CODIGO, nombre_tipo_pago AS 'TIPO PAGO' FROM tipo_pago where estadoid<>2";
                gridLookTipoPago.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoPago.Properties.DisplayMember = "TIPO PAGO";
                gridLookTipoPago.Properties.ValueMember = "CODIGO";
                gridLookTipoPago.EditValue = 0;
                gridLookTipoPago.Text = "";
            }
            catch
            { }       
            try
            {
                /* cadena = "SELECT codigo_serie CODIGO,CONCAT(tipos_documento.nombre_documento,' - ',serie_documento) as DOCUMENTO FROM series_documentos INNER JOIn tipos_documento on series_documentos.codigo_tipo = tipos_documento.codigo_tipo";*/
                /* jramirez 2013.07.24 */
                cadena = "SELECT distinct codigo_serie AS CODIGO, CONCAT(nombre_documento,' - ',serie_documento) as DOCUMENTO FROM v_bodegas_series_usuarios  WHERE userid=" + clases.ClassVariables.id_usuario;
                gridLookTipoDocumento.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoDocumento.Properties.DisplayMember = "DOCUMENTO";
                gridLookTipoDocumento.Properties.ValueMember = "CODIGO";
                gridLookTipoDocumento.EditValue = 0;
                gridLookTipoDocumento.Text = "";
            }
            catch
            { }
            try
            {
                /* cadena = "SELECT codigo_serie CODIGO,CONCAT(tipos_documento.nombre_documento,' - ',serie_documento) AS DOCUMENTO FROM series_documentos INNER JOIN tipos_documento ON series_documentos.codigo_tipo = tipos_documento.codigo_tipo WHERE tipos_documento.codigo_tipo=1"; */
                /* jramirez 2013.07.24 */
                cadena = "SELECT distinct codigo_serie AS CODIGO, CONCAT(nombre_documento,' - ',serie_documento) as DOCUMENTO FROM v_bodegas_series_usuarios  WHERE codigo_tipo=1 and userid=" + clases.ClassVariables.id_usuario;
                gridLookDocFactura.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookDocFactura.Properties.DisplayMember = "DOCUMENTO";
                gridLookDocFactura.Properties.ValueMember = "CODIGO";
                gridLookDocFactura.EditValue = 1;
            }
            catch
            { }

            try
            {
                cadena = "SELECT userid,username FROM usuarios WHERE mostrar_ventas=1";
                gridLookUpEdit1.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookUpEdit1.Properties.DisplayMember = "username";
                gridLookUpEdit1.Properties.ValueMember = "userid";
                gridLookUpEdit1.EditValue = 0;
                gridLookUpEdit1.Text = "";
            }
            catch
            { }  

        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transa;
        
        string cadena;
        
        classortoxela logicaorto = new classortoxela();
        private void frm_regreso_Load(object sender, EventArgs e)
        {
            xtraTabPage3.PageVisible = false;
            xtraTabPage2.PageEnabled = false;
            CargaDatosCombos();
            CreaColumnas();
            CargaDatos();
            dateEdit1.DateTime = DateTime.Now;
            
        }
        private void CargaDatos()
        {
            try
            {

                DataTable tablatemporal = new DataTable();
                cadena = "SELECT bodegas_header.codigo_bodega,bodegas_header.nombre_bodega,articulos.codigo_articulo,articulos.descripcion,detalle_doctos_inv.cantidad_enviada,detalle_doctos_inv.precio_unitario,detalle_doctos_inv.precio_total FROM detalle_doctos_inv INNER JOIN articulos ON detalle_doctos_inv.codigo_articulo=articulos.codigo_articulo INNER JOIN bodegas_header ON detalle_doctos_inv.codigo_bodega=bodegas_header.codigo_bodega WHERE detalle_doctos_inv.id_documento=" + id_pedido;
                tablatemporal = logicaorto.Tabla(cadena);
                foreach (DataRow fila in tablatemporal.Rows)
                {
                    gridView1.AddNewRow();
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", fila["codigo_bodega"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", fila["nombre_bodega"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", fila["codigo_articulo"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", fila["descripcion"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(fila["cantidad_enviada"]));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO UNITARIO", fila["precio_unitario"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", fila["precio_total"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD DEVUELTA", Convert.ToInt32(fila["cantidad_enviada"]));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "UTILIZADO", 0);
                    gridView1.UpdateCurrentRow();
                }
            }
            catch
            { }
        }
        private void CargaDatosAnulado()
        {
            try
            {
                DataTable tablatemporal = new DataTable();
                cadena = "SELECT bodegas_header.codigo_bodega,bodegas_header.nombre_bodega,articulos.codigo_articulo,articulos.descripcion,detalle_doctos_inv.cantidad_enviada,detalle_doctos_inv.precio_unitario,detalle_doctos_inv.precio_total,cantidad_devuelta FROM detalle_doctos_inv INNER JOIN articulos ON detalle_doctos_inv.codigo_articulo=articulos.codigo_articulo INNER JOIN bodegas_header ON detalle_doctos_inv.codigo_bodega=bodegas_header.codigo_bodega WHERE detalle_doctos_inv.id_documento=" + id_pedido;
                tablatemporal = logicaorto.Tabla(cadena);
                foreach (DataRow fila in tablatemporal.Rows)
                {
                    gridView1.AddNewRow();
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", fila["codigo_bodega"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", fila["nombre_bodega"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", fila["codigo_articulo"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", fila["descripcion"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(fila["cantidad_enviada"]));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO UNITARIO", fila["precio_unitario"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", fila["precio_total"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD DEVUELTA", Convert.ToInt32(fila["cantidad_devuelta"]));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "UTILIZADO", 0);
                    gridView1.UpdateCurrentRow();
                }
                sbAceptar.Enabled = false;
                groupControl2.Enabled = false;
                xtraTabPage2.PageEnabled = true;
                llenaFactura();
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
            temporal.Columns.Add("PRECIO UNITARIO");
            temporal.Columns.Add("SUB TOTAL");
            temporal.Columns.Add("CANTIDAD");
            temporal.Columns.Add("CANTIDAD DEVUELTA");
            temporal.Columns.Add("UTILIZADO");
            gridControl1.DataSource = temporal;
            gridView1.Columns["IDBODEGA"].Visible =false;
            gridView1.Columns["DESCRIPCION"].Width = 200;
            gridView1.Columns["BODEGA"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["CODIGO"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["DESCRIPCION"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["CANTIDAD"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["PRECIO UNITARIO"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["SUB TOTAL"].OptionsColumn.ReadOnly = true;
            gridView1.UpdateCurrentRow();
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       string id_pedido,id_cliente,id_vale,id_recibo,id_factura,id_socio_comercial;
        private void sbnuevo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            /* Carga pedidos con status pendiente */
            cadena = "SELECT header_doctos_inv.id_documento AS 'CODIGO',header_doctos_inv.fecha AS 'FECHA PEDIDO',header_doctos_inv.no_documento 'NUMERO DOCUMENTO',clientes.nombre_cliente AS 'NOMBRE CLIENTE',header_doctos_inv.descripcion AS 'DESCRIPCION',header_doctos_inv.refer_documento AS 'DEPOSITO',header_doctos_inv.monto_neto AS 'TOTAL PEDIDO' FROM header_doctos_inv INNER JOIN series_documentos ON series_documentos.codigo_serie=header_doctos_inv.codigo_serie INNER JOIN clientes ON header_doctos_inv.codigo_cliente=clientes.codigo_cliente WHERE series_documentos.codigo_tipo=5 AND (header_doctos_inv.estadoid=8 OR header_doctos_inv.estadoid=5)";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
               
                id_pedido = clases.ClassVariables.id_busca;
                DataTable tempLlena = new DataTable();
                cadena = "SELECT cli.nombre_cliente,cli.codigo_cliente,cli.nit,cab.codigo_serie,cab.tipo_pago,cab.no_documento,cab.descripcion,cab.razon_ajuste,cab.descuento,cab.monto_neto,cab.contado_credito,cab.socio_comercial,refer_documento FROM header_doctos_inv cab INNER JOIN clientes cli ON cab.codigo_cliente=cli.codigo_cliente WHERE cab.id_documento=" + id_pedido;
                
                    tempLlena = logicaorto.Tabla(cadena);
                    textNombreCliente.Text = tempLlena.Rows[0]["nombre_cliente"].ToString();
                    id_cliente = tempLlena.Rows[0]["codigo_cliente"].ToString();
                    textNitCliente.Text = tempLlena.Rows[0]["nit"].ToString();
                    gridLookTipoDocumento.EditValue = tempLlena.Rows[0]["codigo_serie"].ToString();
                    gridLookTipoPago.EditValue = tempLlena.Rows[0]["tipo_pago"].ToString();
                    textNoDocumento.Text = tempLlena.Rows[0]["no_documento"].ToString();
                    textDeposito.Text = tempLlena.Rows[0]["refer_documento"].ToString();
                    try
                    { id_socio_comercial = tempLlena.Rows[0]["socio_comercial"].ToString();
                    
                        if (id_socio_comercial == "")
                        id_socio_comercial = "1";
                    }
                    catch { }
                    memoDescripcion.Text = tempLlena.Rows[0]["descripcion"].ToString();
                    memoRazonAjuste.Text = tempLlena.Rows[0]["razon_ajuste"].ToString();
                    textTotalDescuento.Text = Convert.ToDouble(tempLlena.Rows[0]["descuento"]).ToString("C");
                    textTotalPedido.Text = Convert.ToDouble(tempLlena.Rows[0]["monto_neto"]).ToString("C");
                    radioGroup1.SelectedIndex = Convert.ToInt16(Convert.ToBoolean(tempLlena.Rows[0]["contado_credito"]));
                    cadena = "SELECT cli.codigo_cliente,cli.nombre_cliente FROM header_doctos_inv cab INNER JOIN clientes cli ON cli.codigo_cliente=cab.socio_comercial WHERE cab.id_documento=" + id_pedido;
                    tempLlena = logicaorto.Tabla(cadena);
                if(tempLlena.Rows.Count>0)
                    textSocioComercial.Text = tempLlena.Rows[0]["nombre_cliente"].ToString();

                    cadena = "SELECT header_doctos_inv.estadoid FROM header_doctos_inv WHERE header_doctos_inv.id_documento="+id_pedido;
                    tempLlena = logicaorto.Tabla(cadena);
                    int Estado = Convert.ToInt16(tempLlena.Rows[0][0]);
                    if (Estado == 4 | Estado==6 | Estado==8)
                    {
                        CreaColumnas();
                        CargaDatos();
                        sbAceptar.Enabled = true;
                        simpleButton9.Enabled = true;
                        groupControl2.Enabled = true;
                    }
                    else
                        if(Estado==5)
                    {
                        CreaColumnas();
                        CargaDatosAnulado();
                    }
                    cadena = "SELECT relacion_venta.id_vale FROM relacion_venta WHERE relacion_venta.id_documento=" + id_pedido;
                    id_vale = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                    try
                    {
                        cadena = "SELECT recibos.id_recibos FROM relacion_venta INNER JOIN recibos ON relacion_venta.id_documento=recibos.id_recibos WHERE relacion_venta.id_vale=" + id_vale;
                        id_recibo = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                    }
                    catch
                    { id_recibo = "0"; }

                    textTotalPedido.Text = Convert.ToDouble(logicaorto.Tabla("SELECT header_doctos_inv.monto_neto FROM header_doctos_inv WHERE header_doctos_inv.id_documento=" + id_vale).Rows[0][0]).ToString("C");

                    string tmpVueltox = "";
                     try
                    {
                        cadena = "SELECT monto_vuelto FROM vueltos WHERE id_pedido = " + id_pedido;
                        tmpVueltox = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                    }
                    catch
                    {   tmpVueltox = "0"; }

                     textTotalDevuelta.Text = tmpVueltox;
            }
            Cursor.Current = Cursors.Default;
        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        int CantPedido, CantDevuelta;// ESTAS VARIABLES ME SIRVEN PARA  HACER LA VALIDACION EN EL GRIDCONTROL DE LAS CANTIDADES

        private void gridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            //try
            {
                ColumnView ver = sender as ColumnView;
                CantPedido = Convert.ToInt32(ver.GetRowCellValue(e.RowHandle, ver.Columns["CANTIDAD"]));
                CantDevuelta = Convert.ToInt32(ver.GetRowCellValue(e.RowHandle, ver.Columns["CANTIDAD DEVUELTA"]));
                if (CantDevuelta > CantPedido)
                {
                    e.Valid = false;
                    ver.SetColumnError(gridView1.Columns["CANTIDAD"], "LA CANTIDAD A DEVOLVER TIENE QUE SER MENOR O IGUAL A LA CANTIDAD PEDIDA");
                    ver.SetColumnError(gridView1.Columns["CANTIDAD DEVUELTA"], "LA CANTIDAD DEVUELTA TIENE QUE SER MENOR O IGUAL A LA CANTIDAD PEDIDA");
                }
            }
            //catch 
            { }
        }

        private void sbAceptar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (dxValidationAceptaDev.Validate() & gridView1.DataRowCount > 0)
            {
                if (MessageBox.Show("¿ESTA SEGURO DE CONTINUAR, AL HACER ESTO NO HAY VUELTA ATRAS?", "INFORMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        conexion.Open();
                        transa = conexion.BeginTransaction();
                        for (int x = 0; x < gridView1.DataRowCount; x++)
                        {
                            cant_devolucion = Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD DEVUELTA"));
                            preciounitario = Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD"));
                            total_devuelta += cant_devolucion * preciounitario;
                            cadena = "UPDATE detalle_doctos_inv SET detalle_doctos_inv.cantidad_devuelta=" + cant_devolucion + ",detalle_doctos_inv.cantidad_final=" + (preciounitario - cant_devolucion) + ",detalle_doctos_inv.precio_unitario='" + gridView1.GetRowCellValue(x, "PRECIO UNITARIO") + "' WHERE detalle_doctos_inv.codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "' and detalle_doctos_inv.id_documento=" + id_pedido + " AND detalle_doctos_inv.codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA");                            
                            comando = new MySqlCommand(cadena, conexion);
                            comando.Transaction = transa;
                            comando.ExecuteNonQuery();
                            cadena = "UPDATE bodegas SET bodegas.existencia_articulo=bodegas.existencia_articulo+" + cant_devolucion + " WHERE bodegas.codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "' AND bodegas.codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA");
                            comando = new MySqlCommand(cadena, conexion);
                            comando.Transaction = transa;
                            comando.ExecuteNonQuery();
                            
                        }
                        string tempoValor = textTotalDevuelta.Text.Replace(",", "");

                        //if (double.Parse(tempoValor.ToString(), NumberStyles.Currency) > 0)
                        //{
                        //    cadena = "INSERT into vueltos(codigo_cliente, id_vale,id_recibo,id_pedido, monto_vuelto, fecha_creacion, estadoid) " +
                        //                "VALUES (" + id_cliente + ", " + id_vale + "," + id_recibo + "," + id_pedido + ", " + double.Parse(tempoValor.ToString(), NumberStyles.Currency) + ", '" + DateTime.Now.ToString("yyyy-MM-dd") + "',4)";
                        //    comando = new MySqlCommand(cadena, conexion);
                        //    comando.Transaction = transa;
                        //    comando.ExecuteNonQuery();
                        //}


                        cadena = "UPDATE header_doctos_inv SET header_doctos_inv.estadoid=5 WHERE header_doctos_inv.id_documento=" + id_pedido;
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        comando.ExecuteNonQuery();                        
                        transa.Commit();
                        clases.ClassMensajes.INSERTO(this);
                        sbAceptar.Enabled = false;
                        groupControl2.Enabled = false;
                        xtraTabPage2.PageEnabled = true;
                        llenaFactura();
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
            }
            else
            {
                clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
            Cursor.Current = Cursors.Default ;
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                textDetalleFactura.Enabled = true;  
                textCantidadVale.Enabled = true;
                textUnitarioVale.Enabled = true;
                simpleButton10.Enabled = true;
            }
            else
            {
                textDetalleFactura.Enabled = false;
                textCantidadVale.Enabled = false;
                textUnitarioVale.Enabled = false;
                simpleButton10.Enabled = false;
            }
        }
        private void llenaFactura()
        {
            textEditCODIGO.Text = id_cliente;
            textClienteFactura.Text = textNombreCliente.Text;
            textNitFactura.Text = textNitCliente.Text;
            cadena = "SELECT codigo_cliente, nombre_cliente, nit,direccion FROM clientes where codigo_cliente="+id_cliente;
            DataTable dt = new DataTable();
            dt=logicaorto.Tabla(cadena);
            foreach (DataRow fila in dt.Rows)
            {
                textDireccionFactura.Text = fila[3].ToString();
            }
            try
            {
                cadena = "SELECT  contado_credito FROM header_doctos_inv where id_documento=" + id_pedido;
                dt.Clear();
                dt = logicaorto.Tabla(cadena);
                foreach (DataRow fila in dt.Rows)
                {
                    if (Convert.ToBoolean(fila[0].ToString()) == true)
                        radioGroup1.SelectedIndex = 1;
                    else
                        radioGroup1.SelectedIndex = 0;

                }
            }
            catch
            { }
            textEditVENDEDOR.Text = clases.ClassVariables.NombreComple;
            // gridLookUpEdit1.Text = clases.ClassVariables.NombreComple;
            
                DataTable tempoFactura = new DataTable();
            tempoFactura.Columns.Add("CODIGO");
            tempoFactura.Columns.Add("DESCRIPCION");
            tempoFactura.Columns.Add("CANTIDAD");
            tempoFactura.Columns.Add("UNITARIO");
            tempoFactura.Columns.Add("TOTAL");            
            gridControl2.DataSource = tempoFactura;
            gridView4.Columns["CODIGO"].OptionsColumn.ReadOnly = true;
            gridView4.Columns["DESCRIPCION"].OptionsColumn.ReadOnly = true;
            gridView4.Columns["CANTIDAD"].OptionsColumn.ReadOnly = true;
            //gridView4.Columns["UNITARIO"].OptionsColumn.ReadOnly = true;
            gridView4.Columns["TOTAL"].OptionsColumn.ReadOnly = true;
            for (int x = 0; x < gridView1.DataRowCount;x++)
            {
                cant_devolucion = Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD DEVUELTA"));
                preciounitario = Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD"));
                if ((preciounitario - cant_devolucion) > 0)
                {
                    gridView4.AddNewRow();
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "CODIGO", gridView1.GetRowCellValue(x, "CODIGO"));
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "DESCRIPCION", gridView1.GetRowCellValue(x, "DESCRIPCION"));
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "CANTIDAD", (preciounitario-cant_devolucion));
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "UNITARIO", gridView1.GetRowCellValue(x, "PRECIO UNITARIO"));
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "TOTAL", (Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO UNITARIO")) * (preciounitario - cant_devolucion)));
                    gridView4.UpdateCurrentRow();
                }
            }
            textTotalDeFactura.Text = textTotalFactura.Text;
        }
        double cant_devolucion,preciounitario,total_devuelta,TotalFactura;// solo me sirve para calcular el total de devolucion
        private void gridView1_RowUpdated(object sender, RowObjectEventArgs e)
        {
            total_devuelta = 0;
            TotalFactura = 0;
            cant_devolucion = Convert.ToDouble(gridView1.GetFocusedRowCellValue("CANTIDAD"));
            preciounitario = Convert.ToDouble(gridView1.GetFocusedRowCellValue("PRECIO UNITARIO"));
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", (cant_devolucion*preciounitario));           
            for(int x = 0; x < gridView1.DataRowCount; x++)
            {
                cant_devolucion = Convert.ToDouble(gridView1.GetRowCellValue(x,"CANTIDAD DEVUELTA"));
                preciounitario = Convert.ToDouble(gridView1.GetRowCellValue(x,"PRECIO UNITARIO"));
                total_devuelta += cant_devolucion * preciounitario;
                TotalFactura =TotalFactura+ Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * preciounitario;
                gridView1.SetRowCellValue(x, "UTILIZADO", (Convert.ToInt32(gridView1.GetRowCellValue(x, "CANTIDAD")) - Convert.ToInt32(gridView1.GetRowCellValue(x, "CANTIDAD DEVUELTA"))));
            }
            
            textTotalFactura.Text = TotalFactura.ToString("C");
            textTotalDevuelta.Text = total_devuelta.ToString("C");

            if (((double.Parse(textTotalFactura.Text.ToString(), NumberStyles.Currency) - total_devuelta)) >= 0)
                textTotalFactura.Text = (double.Parse(textTotalFactura.Text.ToString(), NumberStyles.Currency) - total_devuelta).ToString("C");
            total_devuelta =  double.Parse(textTotalPedido.Text.ToString(), NumberStyles.Currency)   - double.Parse(textTotalFactura.Text.ToString(), NumberStyles.Currency);
            textTotalDevuelta.Text = total_devuelta.ToString("C");
        }

        private void LlenaDatosRecibo()
        {
            textRecibimosDe.Text = textNombreCliente.Text;
            dateFechaRecibo.DateTime = DateTime.Now;
            textPor.Text = TotalFactura.ToString();
            textCodigoCobrador.Text = clases.ClassVariables.id_usuario;
            cadena = "SELECT (MAX(recibos.no_recibo)+1)AS 'NORECIBO' FROM recibos";
            textNoRecibo.Text = logicaorto.Tabla(cadena).Rows[0]["NORECIBO"].ToString();

            if (Convert.ToInt16(gridLookTipoPago.EditValue) == 1)
            {
                textEfectivo.Enabled = true;
                textEfectivo.Text = textTotalPedido.Text;
                textCheque.Enabled = false;
                textBanco.Enabled = false;
            }
            else
            {
                textEfectivo.Enabled = false;
                textEfectivo.Text = "";
                textCheque.Enabled = true;
                textBanco.Enabled = true;
                textValor.Text = textTotalPedido.Text;
            }
        }

        void registarenInvex()
        {
                #region integracion con INVEX, encabezado de venta
                Venta encabezadodeventa = null;
            string serie = gridLookDocFactura.Text;
            serie = serie.Replace("Factura - ","");
            string telefonoClienteB = logicaorto.Tabla("SELECT telefono_casa FROM clientes WHERE codigo_cliente=" + id_cliente).Rows[0][0].ToString();
            encabezadodeventa = integracion_ii.Class_venta.EncabezadoVenta(textClienteFactura.Text, textNitFactura.Text,telefonoClienteB, dateEdit1.DateTime, "Crédito", Convert.ToInt16(textNoDocumento.Text), serie);
                #endregion


                for (int x = 0; x < gridView4.DataRowCount; x++)
                {

                        #region integracion con INVEX, detalle de venta
                        DataTable dt = new DataTable();
                        string cns = "SELECT categorias.`codigo_categoria`,categorias.`nombre_categoria`,articulos.`costo` FROM categorias INNER JOIN sub_categorias ON sub_categorias.`codigo_categoria`=categorias.`codigo_categoria` INNER JOIN articulos ON articulos.`codigo_categoria`=sub_categorias.`codigo_subcat` WHERE articulos.codigo_articulo='" + gridView4.GetRowCellValue(x, "CODIGO") + "'";
                        dt = logicaorto.Tabla(cns);
                        string codigoA = dt.Rows[0][0].ToString();
                        string nombreA = dt.Rows[0][1].ToString();
                        decimal costoA = 0;
                        try
                        {
                            costoA = Convert.ToDecimal(dt.Rows[0][2].ToString());
                        }
                        catch { }
                        int cantidadA = Convert.ToInt16(gridView4.GetRowCellValue(x, "CANTIDAD"));
                        decimal precioA = Convert.ToDecimal(gridView4.GetRowCellValue(x, "UNITARIO"));
                        integracion_ii.Class_venta.DetalleVenta(encabezadodeventa, cantidadA, precioA, codigoA, nombreA,costoA);
                        #endregion
                }

                    #region validar venta con o sin deposito
                    //if (gridLookTipoPago.Text == "Cheque")
                    //{
                    //    integracion_ii.Class_venta.ValidarVentaConDeposito(encabezadodeventa, "cuenta", Convert.ToInt32(textDeposito.Text));
                    //}
                    //else
                    //{
                    //    integracion_ii.Class_venta.ValidarVentaSinDeposito(encabezadodeventa);
                    //}
                integracion_ii.Class_venta.ValidarEnCretido(encabezadodeventa);
                    #endregion

            }


        string id_nuevo_factura;
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //try
            //{
            Cursor = Cursors.WaitCursor;
            try
            {
                int p = gridView4.DataRowCount;
                DialogResult a=DialogResult.Yes;
                if (p >= 12)
                {
                    a = MessageBox.Show("Esta factura tiene mas de 12 campos, es posible que la impresion sea defectuosa. \n Desea continuar?", "ALERTA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }

                if (a == DialogResult.Yes)
                {
                    cadena = "SELECT header_doctos_inv.id_documento FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie='" + gridLookDocFactura.EditValue + "' and header_doctos_inv.no_documento=" + textNumeroDocFactura.Text;
                    if (logicaorto.ExisteRegistro(cadena) == false)
                    {
                        conexion.Close();
                        conexion.Open();
                        transa = conexion.BeginTransaction();
                        string tempoTotalDeFactura = textTotalDeFactura.Text.Replace(",", "");
                        cadena = "INSERT into header_doctos_inv(codigo_serie,tipo_pago,no_documento, codigo_cliente, fecha, monto,descuento , monto_neto, usuario_creador,socio_comercial, estadoid,contado_credito,refer_documento,vendedor) " +
                                "VALUES (" + gridLookDocFactura.EditValue + "," + gridLookTipoPago.EditValue + ", '" + textNumeroDocFactura.Text + "', " + id_cliente + ", '" + dateEdit1.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', " + double.Parse(tempoTotalDeFactura.ToString(), NumberStyles.Currency) + ",0," + double.Parse(tempoTotalDeFactura.ToString(), NumberStyles.Currency) + ", " + clases.ClassVariables.id_usuario + "," + id_socio_comercial + ",4," + radioGroup1.SelectedIndex + ",'" + textDeposito.Text + "'," + gridLookUpEdit1.EditValue + ");select last_insert_id();";

                        
                        
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        id_nuevo_factura = comando.ExecuteScalar().ToString();
                        for (int x = 0; x < gridView4.DataRowCount; x++)
                        {
                            cadena = "INSERT into detalle_doctos_inv(id_documento, cantidad_enviada, precio_unitario, precio_total, codigo_articulo, codigo_bodega) " +
                                        "VALUES (" + id_nuevo_factura + ", " + gridView4.GetRowCellValue(x, "CANTIDAD") + ", " + gridView4.GetRowCellValue(x, "UNITARIO") + ", " + gridView4.GetRowCellValue(x, "TOTAL") + ",'" + gridView4.GetRowCellValue(x, "CODIGO") + "', 1)";
                            comando = new MySqlCommand(cadena, conexion);
                            comando.Transaction = transa;
                            comando.ExecuteNonQuery();
                        }


                        string tempoValor = ""; //textTotalDevuelta.Text.Replace(",", "");
                        decimal can1 = Convert.ToDecimal(textTotalPedido.Text.Replace(",", "").Replace("Q", ""));
                        decimal can3=Convert.ToDecimal(textTotalDeFactura.Text.Replace(",", ""));
                        decimal vvv = can1- can3;
                        tempoValor = vvv.ToString();
                        //vueltos
                        if (double.Parse(tempoValor.ToString(), NumberStyles.Currency) > 0)
                        {
                            cadena = "INSERT into vueltos(codigo_cliente, id_vale,id_recibo,id_pedido, monto_vuelto, fecha_creacion, estadoid) " +
                                        "VALUES (" + id_cliente + ", " + id_vale + "," + id_recibo + "," + id_pedido + ", " + double.Parse(tempoValor.ToString(), NumberStyles.Currency) + ", '" + DateTime.Now.ToString("yyyy-MM-dd") + "',4)";
                            comando = new MySqlCommand(cadena, conexion);
                            comando.Transaction = transa;
                            comando.ExecuteNonQuery();
                        }
                        //

                        if (Class_integracion.logeado == true)
                        {
                            registarenInvex();
                        }
                        


                       

                        cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                                    "VALUES (" + id_cliente + ", " + id_vale + "," + id_nuevo_factura + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        comando.ExecuteNonQuery();

                        cadena = "UPDATE vueltos set vueltos.id_factura=" + id_nuevo_factura + " where vueltos.id_vale=" + id_vale;
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        comando.ExecuteNonQuery();
                        cadena = "UPDATE header_doctos_inv SET header_doctos_inv.estadoid=4 WHERE header_doctos_inv.id_documento=" + id_pedido;
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        comando.ExecuteNonQuery();
                        transa.Commit();

                        

                        clases.ClassMensajes.INSERTO(this);
                        if (radioGroup1.SelectedIndex == 1)
                        {
                            LlenaDatosRecibo();
                            xtraTabPage3.PageVisible = true;
                        }

                        try
                        {
                            //Pedido.Factura.XtraReportFactura reporte = new Pedido.Factura.XtraReportFactura();
                            //reporte.Parameters["ID"].Value = id_nuevo_factura;
                            //reporte.Parameters["LETRAS"].Value = logicaorto.enletras(double.Parse(textTotalDeFactura.Text.ToString(), NumberStyles.Currency).ToString());

                            //reporte.Parameters["Vende"].Value = textEditVENDEDOR.Text;
                            //reporte.Parameters["SM_PC_FO"].Value = "Paciente: " + textNombreCliente.Text + " - Socio Comercial: " + textSocioComercial.Text + " - Operado: " + textEdit4.Text;
                            //reporte.RequestParameters = false;
                            //reporte.ShowPreviewDialog();






                            //k


                            string letras = logicaorto.enletras(double.Parse(textTotalDeFactura.Text.ToString(), NumberStyles.Currency).ToString());

                            Pedido.Factura.F_impresion nf = new Pedido.Factura.F_impresion();

                            string con, cre;

                            //ver si es credito o contado
                            if (radioGroup1.SelectedIndex == 0)
                            {
                                con = "X";
                                cre = " ";
                            }
                            else
                            {
                                con = " ";
                                cre = "X";
                            }




                            if (gridLookDocFactura.Text == "Factura - A")
                            {
                                nf.facturaA(Convert.ToInt16(id_nuevo_factura), letras, con, cre, Convert.ToInt16(gridLookDocFactura.EditValue), id_socio_comercial);
                            }
                            else if (gridLookDocFactura.Text == "Factura - B")
                            {
                                nf.facturaB(Convert.ToInt16(id_nuevo_factura), letras, con, cre, Convert.ToInt16(gridLookDocFactura.EditValue), id_socio_comercial);
                            }
                            else if (gridLookDocFactura.Text == "Factura - C")
                            {
                                nf.facturaC(Convert.ToInt16(id_nuevo_factura), letras, con, cre, Convert.ToInt16(gridLookDocFactura.EditValue), id_socio_comercial);
                            }
                            else if (gridLookDocFactura.Text == "Factura - D")
                            {
                                nf.facturaD(Convert.ToInt16(id_nuevo_factura), letras, con, cre, Convert.ToInt16(gridLookDocFactura.EditValue), id_socio_comercial);
                            }
                            else if (gridLookDocFactura.Text == "Factura - E")
                            {
                                nf.facturaE(Convert.ToInt16(id_nuevo_factura), letras, con, cre, Convert.ToInt16(gridLookDocFactura.EditValue), id_socio_comercial);
                            }


                            nf.ShowDialog();
                            //k
                        }
                        catch
                        {

                        }
                        simpleButton9.Enabled = false;
                    }
                    else
                    {
                        alertControl1.Show(this, "INFORMACION", "EL NUMERO DE DOCUMENTO YA EXISTE", Properties.Resources.Advertencia64);
                    }
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
            Cursor = Cursors.Default;
        }

        private void xtraTabPage2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridLookDocFactura_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC' FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie=" + gridLookDocFactura.EditValue + " ORDER BY header_doctos_inv.no_documento DESC LIMIT 1";
                textNumeroDocFactura.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
            }
            catch
            {
                textNumeroDocFactura.Text = "1";
            }
        }
        string abono, cancelacion, otro;//solo me sirven para poder generar el recibo
        private void sbPrintReciboCaja_Click(object sender, EventArgs e)
        {
            if (dxValidationRecibo.Validate())
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
                            "VALUES (" + textNoRecibo.Text + ", 3, " + id_cliente + ", '" + Convert.ToDateTime(dateFechaRecibo.EditValue).ToString("yyyy-MM-dd HH:mm:ss") + "', " + id_vale+ ", 1, " + tempValor.Replace("Q", "") + ", " + clases.ClassVariables.id_usuario + ", 4);";
                    comando = new MySqlCommand(cadena, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                    cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                            "VALUES (" + id_cliente + ", " + id_vale + "," + textNoRecibo.Text + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                    comando = new MySqlCommand(cadena, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                    cadena = "UPDATE header_doctos_inv SET header_doctos_inv.estadoid=5 WHERE header_doctos_inv.id_documento=" + id_nuevo_factura;
                    comando = new MySqlCommand(cadena, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();            
                    transa.Commit();
                    clases.ClassMensajes.INSERTO(this);
                    ReciboCaja.DataSetReciboCaja dataset = new ReciboCaja.DataSetReciboCaja();
                    dataset.Tables["recibos"].Rows.Add(textNoRecibo.Text, DateTime.Now, 1, textPor.Text.Replace("Q", ""), clases.ClassVariables.id_usuario, 1, textFacturas.Text, textCheque.Text, textBanco.Text, textValor.Text, abono, cancelacion, otro, textCantidadDe.Text, textRecibimosDe.Text, id_cliente, textEfectivo.Text);
                    ReciboCaja.XtraReportReciboCaja reporte = new ReciboCaja.XtraReportReciboCaja();
                    reporte.DataSource = dataset;
                    reporte.DataMember = dataset.Tables["recibos"].TableName;
                    reporte.RequestParameters = false;
                    reporte.ShowPreviewDialog();                    
                    sbPrintReciboCaja.Enabled = false;
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
                clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
        }

        private void textPor_EditValueChanged(object sender, EventArgs e)
        {
            textCantidadDe.Text = logicaorto.enletras(textPor.Text.Replace("Q", ""))+" QUETZALES EXACTOS";
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        
        private void simpleButton10_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            int existeCEx = 0;
            for (int x = 0; x < gridView4.DataRowCount; x++)
            {
                if (gridView4.GetRowCellValue(x, "CODIGO").ToString().ToUpper().Contains("CARGOEXTRA"))
                {
                    existeCEx++;
                }
            }

            if (existeCEx <= 0)
            {
                double vCEX = 0;

                try
                {
                    vCEX = Convert.ToDouble(textEdit3.Text);
                }
                catch
                {
                    vCEX = 0;
                }
                if (vCEX <= 0)
                {
                    MessageBox.Show("No ha definido Correctamente el monto del Cargo Extra");
                }
                else
                {
                    gridView4.AddNewRow();
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "CODIGO", "CARGOEXTRA");
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "DESCRIPCION", "Cargo Extra");
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "CANTIDAD", 1);
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "UNITARIO", vCEX);
                    gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "TOTAL", vCEX);
                    gridView4.UpdateCurrentRow();
                    double tmpNewTotx = 0;
                    tmpNewTotx = double.Parse(textTotalDeFactura.Text.ToString(), NumberStyles.Currency) + double.Parse(textEdit3.Text.ToString(), NumberStyles.Currency);
                    textTotalDeFactura.Text = tmpNewTotx.ToString();
                    MessageBox.Show("Se ha aplicado el cargo extra exitosamente");
                }
            }
            else
            {
                MessageBox.Show("No puede volver a aplicar el cargo extra, Si desea cambiar el cargo extra actual elimine y vuelva a cargar el cargo extra actual.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void textTotalDevuelta_EditValueChanged(object sender, EventArgs e)
        {
            textEdit1.Text = textTotalDevuelta.Text;
        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {
            //double tmpMaxVuelto = Convert.ToDouble(textEdit1.Text);
            double tmpCEv = 0.00;

            if (checkBox2.Checked)
            {
                try
                {
                    //tmpCEv = Convert.ToDouble(textTotalDeFactura.Text.ToString().Replace("Q", "")) * Convert.ToDouble(textEdit2.Text) / 100;
                    tmpCEv = double.Parse(textTotalDeFactura.Text.ToString(), NumberStyles.Currency) * Convert.ToDouble(textEdit2.Text) / 100;
                    textEdit3.Text = tmpCEv.ToString();
                }
                catch
                {
                    textEdit3.Text = "0";
                    tmpCEv = 0.00;
                }

            }
            else
            {
                textEdit3.Text = textEdit2.Text;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //double tmpMaxVuelto = Convert.ToDouble(textEdit1.Text);
            double tmpCEv = 0.00;

            if (checkBox2.Checked)
            {
                try
                {
                    //tmpCEv = Convert.ToDouble(textTotalDeFactura.Text.ToString().Replace("Q", "")) * Convert.ToDouble(textEdit2.Text) / 100;
                    tmpCEv = double.Parse(textTotalDeFactura.Text.ToString(), NumberStyles.Currency) * Convert.ToDouble(textEdit2.Text) / 100;
                    textEdit3.Text = tmpCEv.ToString();
                }
                catch
                {
                    textEdit3.Text = "0";
                    tmpCEv = 0.00;
                }

            }
            else
            { 
                textEdit3.Text = textEdit2.Text;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < gridView4.DataRowCount; x++)
            {
                if (gridView4.GetRowCellValue(x, "CODIGO").ToString().ToUpper().Contains("CARGOEXTRA"))
                {
                    double delTmpRE = 0;
                    delTmpRE = Convert.ToDouble(gridView4.GetRowCellValue(x, "TOTAL"));
                    gridView4.DeleteRow(x);
                    double tmpNewTotx = 0;
                    tmpNewTotx = double.Parse(textTotalDeFactura.Text.ToString(), NumberStyles.Currency) -delTmpRE;
                    textTotalDeFactura.Text = tmpNewTotx.ToString();
                    MessageBox.Show("Se ha borrado el Cargo Extra");
                }
            }
        }

        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            textEditVENDEDOR.Text = gridLookUpEdit1.Text;
        }

        private void gridView4_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            /*if (e.Column.Name == "colZonaacumulada")
            {
                decimal zo = 0;
                decimal ex = 0;

                    zo = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Zona acumulada"));
                    ex = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Examen Final"));

                decimal t = zo + ex;
                if (t <= 100)
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", t);
                }

                else
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Zona acumulada", 0);
                }
            }
             */

            if (e.Column.Name == "colUNITARIO")
            {
                //calcular subtotal
                decimal st = 0;
                decimal u = 0;
                decimal c = 0;

                try
                {
                    u = Convert.ToDecimal(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "UNITARIO"));
                    c = Convert.ToDecimal(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "CANTIDAD"));
                }
                catch
                { }
                st = u * c;
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "TOTAL", st);
            }

            else if (e.Column.Name == "colTOTAL")
            { 
                //calcular total
                decimal total = 0;
                try
                {
                    int a = gridView4.RowCount;

                    for (int i = 0; i < a; i++)
                    {
                        decimal tt = Convert.ToDecimal(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "TOTAL"));
                        total = tt;
                    }
                }
                catch
                { }
                textTotalDeFactura.Text = total.ToString();
            }
        }
    }
}