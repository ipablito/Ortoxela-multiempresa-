using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Controls;
using MySql.Data.MySqlClient;


using DevExpress.XtraReports.UI;
namespace ortoxela.Cotizacion
{
    public partial class frm_cotizacion : DevExpress.XtraEditors.XtraForm
    {
        public frm_cotizacion()
        {
            InitializeComponent();
        }
        int id_serie_documento=27;//esta es una variable constante q alamcena el id de la serie de la cotizacion
        private void frm_cotizacion_Load(object sender, EventArgs e)
        {
            cargaCombos();
            CreaColumnas();
            CargaNoDocumento();
            dateFechaCotizacion.DateTime = DateTime.Now;

        }
        classortoxela logicaorto = new classortoxela();
        string cadena = "";
        private void CargaNoDocumento()
        {
            cadena = "SELECT (COALESCE(MAX(header_doctos_inv.no_documento),0)+1) AS NumeroDocs FROM header_doctos_inv WHERE header_doctos_inv.codigo_serie="+id_serie_documento.ToString();
            labelNoDocumento.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
        }
        private void cargaCombos()
        {
            try
            {
                /* cadena = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid<>2"; */
                /* jramirez 2013.07.24 */
                cadena = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario;
                gridLookBodega.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
                gridLookBodega.EditValue = 1;
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
            temporal.Columns.Add("PRECIO UNITARIO");
            temporal.Columns.Add("SUB TOTAL");
            temporal.Columns.Add("EXISTENCIABODEGA");
            temporal.Columns.Add("MINIMO");
            gridControl1.DataSource = temporal;
            gridView1.Columns["DESCRIPCION"].Width = 200;
            gridView1.Columns["MINIMO"].Visible = false;
            gridView1.Columns["IDBODEGA"].Visible = false;
            gridView1.Columns["DESCRIPCION"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["SUB TOTAL"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["BODEGA"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["CODIGO"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["EXISTENCIABODEGA"].OptionsColumn.ReadOnly = true;
        }
        DataTable tempTabla = new DataTable();
        int ExistenciaProd, existencia_minima;
        string id_articulo;
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
                        id_articulo = tempTabla.Rows[0]["CODIGO"].ToString();
                        textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString();
                        ExistenciaProd = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);
                        existencia_minima = Convert.ToInt32(tempTabla.Rows[0]["minimo"]);
                        textVenta.Text = tempTabla.Rows[0]["precio_venta"].ToString();
                        textCantidadArt.Focus();
                    }
                }
            }
        }

        private void textNombreArti_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dxValidationProvider3.Validate())
            {
                cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.precio_venta AS 'PRECIO VENTA',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                clases.ClassVariables.cadenabusca = cadena;
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
                nuevo.Dispose();
            }
        }
        bool banderaRepetido;
        double TotalPedido;
        double TempTotalPedido;//esta variable me sirve para pasar el total del pedido y podes sacar el descuento
        double TotalDescuento;
        private void sbAgregaArt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (dxValidationProvider1.Validate())
                {
                    DataTable TempoPadre = new DataTable();
                    /*cadena = "SELECT articulos.compuesto FROM articulos WHERE articulos.codigo_articulo='" + id_articulo + "'"; 
                     jramirez 20130704*/
                    cadena = "select f_es_compuesto('" + id_articulo + "') AS compuesto;";
                    string compuesto = logicaorto.Tabla(cadena).Rows[0]["compuesto"].ToString();
                    if (Convert.ToBoolean(logicaorto.Tabla(cadena).Rows[0]["compuesto"]))
                    {

                        /* cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',articulos.precio_venta,articulos.costo,articulos.minimo FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND articulos.codigo_padre='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue; */
                        cadena = "CALL sp_devuelve_sistema ('" + id_articulo + "'," + gridLookBodega.EditValue + ")";
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
                                //if(ExistenciaHijo!=0)
                                //{
                                if (Convert.ToInt32(textCantidadArt.Text) <= ExistenciaHijo)
                                {
                                    ExistenciaFija = Convert.ToInt32(textCantidadArt.Text);
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
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(textCantidadArt.Text));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PRECIO UNITARIO", TempoPadre.Rows[x]["precio_venta"]);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", (Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"]) * ExistenciaFija));
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIABODEGA", ExistenciaHijo);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "MINIMO", TempoPadre.Rows[x]["minimo"]);
                                gridView1.UpdateCurrentRow();
                                TotalPedido = TotalPedido + (ExistenciaFija * Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"]));
                               // CalculaDescuento();
                                //}

                            }
                            else
                                clases.ClassMensajes.ProdYaExisteEnListado(this);
                        }

                        textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text = "";
                        textCodigoArt.Focus();
                    }
                    else
                    {
                       // if (Convert.ToInt32(textCantidadArt.Text) <= ExistenciaProd)
                      //  {
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
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIABODEGA", ExistenciaProd);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "MINIMO", existencia_minima);
                                gridView1.UpdateCurrentRow();
                                TotalPedido = TotalPedido + (Convert.ToDouble(textCantidadArt.Text) * Convert.ToDouble(textVenta.Text));
                                //CalculaDescuento();
                                textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text = "";
                                textCodigoArt.Focus();
                            }
                            else
                                clases.ClassMensajes.ProdYaExisteEnListado(this);
                        //}
                        //else
                        //    clases.ClassMensajes.NoHayExistenciaProd(this);
                   }
                    textTotalPedido.Text = TotalPedido.ToString("C");
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

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "EXISTENCIABODEGA")
            {
                string existencia = view.GetRowCellDisplayText(e.RowHandle, view.Columns["EXISTENCIABODEGA"]);
                string minimo = view.GetRowCellDisplayText(e.RowHandle, view.Columns["MINIMO"]);
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
        string id_cliente;
        string id_SocioComercialCompara;
        string id_socioComercial;
        private void textHospital_KeyPress(object sender, KeyPressEventArgs e)
        {
            cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL',clientes.nit AS 'NIT',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes where estadoid<>2";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                id_cliente = clases.ClassVariables.id_busca;
                DataTable tempCliente = new DataTable();
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_casa,contacto FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                tempCliente = logicaorto.Tabla(cadena);
                textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                textNitCliente.Text = tempCliente.Rows[0]["nit"].ToString();
                id_socioComercial = tempCliente.Rows[0]["referido_por"].ToString();
                textUtilizadoPaciente.Text = tempCliente.Rows[0]["nombre_paciente"].ToString();
                textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_casa"].ToString();
                textDoctorPedido.Text = tempCliente.Rows[0]["contacto"].ToString();
                //cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                //try
                //{
                //    tempCliente = logicaorto.Tabla(cadena);
                //    id_SocioComercialCompara = id_socioComercial;
                //    textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                //}
                //catch
                //{ }
            }
            e.KeyChar = Convert.ToChar(13);
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
                    //cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                    //try
                    //{
                    //    tempCliente = logicaorto.Tabla(cadena);
                    //    id_SocioComercialCompara = id_socioComercial;
                    //    textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                    //}
                    //catch
                    //{ }
                }
                else
                {
                    textNombreCliente.Text = "";
                    textNitCliente.Text = "";
                    textTelefonoCliente.Text = "";
                    textUtilizadoPaciente.Text = "";

                }
            }
        }

        private void textNitCliente_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dateFechaCotizacion_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void textVenta_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void gridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            ColumnView ver = sender as ColumnView;
            ////cant1 = Convert.ToInt32(ver.GetFocusedRowCellValue("CANTIDAD"));
            ////cant2 = Convert.ToInt32(ver.GetFocusedRowCellValue("EXISTENCIABODEGA"));
            ////if (cant1 > cant2)
            ////{
            ////    e.Valid = false;
            ////    ver.SetColumnError(gridView1.Columns["CANTIDAD"], "NO HAY EXISTENCIA DE PRODUCTO SOLO HAY " + cant2.ToString());
            ////    Band_permite_borrar = false;
            ////}
            ////else
            ////{
                Band_permite_borrar = true;
                TotalPedido = 0;
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    TotalPedido = TotalPedido + (Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO UNITARIO")));
                    gridView1.SetRowCellValue(x, "SUB TOTAL", Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD")) * Convert.ToDouble(gridView1.GetRowCellValue(x, "PRECIO UNITARIO")));
                }
                textTotalPedido.Text = TotalPedido.ToString("C");
            //}
        }

        private void sbAceptar_Click(object sender, EventArgs e)
        {
            if (dxValidationPedido.Validate() & gridView1.DataRowCount > 0)
            {
                RegistraPedido();

            }
            else
            {
                clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transa;
        string id_nuevo_pedido;
        private void RegistraPedido()
        {
            try
            {
                cadena = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE header_doctos_inv.no_documento=" + labelNoDocumento.Text + " AND series_documentos.codigo_serie=" + id_serie_documento;
                if (logicaorto.ExisteRegistro(cadena))
                    labelNoDocumento.Text = (Convert.ToInt32(labelNoDocumento.Text) + 1).ToString();
                conexion.Open();
                transa = conexion.BeginTransaction();
                cadena = "INSERT into header_doctos_inv(codigo_serie, tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento,fecha_retorno, socio_comercial,razon_ajuste, descripcion, estadoid,contado_credito,refer_documento) " +
                        "VALUES (" + id_serie_documento + ", " + 1 + ", '" + labelNoDocumento.Text + "', " + id_cliente + ", '" + dateFechaCotizacion.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', " + TotalPedido + ", " + TotalDescuento + ", " + TotalPedido + ", " + clases.ClassVariables.id_usuario + ", " + 0 + ",'" + dateCirugia.DateTime.ToString("yyyy-MM-dd") + "','" + id_socioComercial + "', 'Sostenimiento de precio:" + textSostenimiento.Text + "\nMarca: " + textMarcas.Text + "\nTiempo de Entrega: " + textTiempoEntrega.Text + "','" + "Diagnostico: " + textDiagnostico.Text + "\nMedico: " + textDoctorPedido.Text + "', 4,0,'" + textRepresentante.Text + "\nREPRESENTANTE DE VENTAS\n" +textTelRepresent.Text +"');select last_insert_id();";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                id_nuevo_pedido = comando.ExecuteScalar().ToString();
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    if (Convert.ToInt32(gridView1.GetRowCellValue(x, "CANTIDAD")) > 0)
                    {
                        cadena = "INSERT into detalle_doctos_inv(id_documento, cantidad_enviada, precio_unitario, precio_total, codigo_articulo, codigo_bodega) " +
                                    "VALUES (" + id_nuevo_pedido + ", " + gridView1.GetRowCellValue(x, "CANTIDAD") + ", " + gridView1.GetRowCellValue(x, "PRECIO UNITARIO") + ", " + gridView1.GetRowCellValue(x, "SUB TOTAL") + ",'" + gridView1.GetRowCellValue(x, "CODIGO") + "', " + gridView1.GetRowCellValue(x, "IDBODEGA") + ")";
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transa;
                        comando.ExecuteNonQuery();
                    }
                }
                //cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                //            "VALUES (" + id_cliente + ", " + id_nuevo_vale + "," + id_nuevo_pedido + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                //comando = new MySqlCommand(cadena, conexion);
                //comando.Transaction = transa;
                //comando.ExecuteNonQuery();
                //cadena = "update header_doctos_inv SET estadoid = 4 WHERE header_doctos_inv.id_documento=" + id_nuevo_vale;
                //comando = new MySqlCommand(cadena, conexion);
                //comando.Transaction = transa;
                //comando.ExecuteNonQuery();
                transa.Commit();
                clases.ClassMensajes.INSERTO(this);
                sbAceptar.Enabled = false;
                //if (radioGroup2.SelectedIndex == 0)
                    //xtraTabPage2.PageEnabled = true;
                    //LlenaDatosRecibo();
                    simplePrinter.Enabled = true;
                groupControl1.Enabled = false;
                    //cadena = "update bodegas SET  existencia_articulo = existencia_articulo -" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA") + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                    //comando = new MySqlCommand(cadena, conexion);
                    //comando.Transaction = transa;
                    //comando.ExecuteNonQuery();
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
        private void sbnuevo_Click(object sender, EventArgs e)
        {

        }

        private void simplePrinter_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DataSet_CotizacionTableAdapters.DataTable3TableAdapter lg = new DataSet_CotizacionTableAdapters.DataTable3TableAdapter();
                DataTable res = new DataTable();
                res = lg.GetData_cot(Convert.ToInt16(id_nuevo_pedido));

                XtraReport_Cotizacion reporte = new XtraReport_Cotizacion();
                reporte.DataSource = res;
                //reporte.Parameters["ID_Documento"].Value = id_nuevo_pedido;
                reporte.RequestParameters = false;
                reporte.ShowPreviewDialog();
            }
            catch
            { }
            this.Cursor = Cursors.Default;
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void textTelRepresent_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}