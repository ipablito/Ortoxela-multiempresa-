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
using System.Globalization;

using integracion_ii;
using Sisnova.Invex.BL;

namespace ortoxela.FacturaTemporal
{
    public partial class IngresarFacturas : DevExpress.XtraEditors.XtraForm
    {
        public IngresarFacturas()
        {
            InitializeComponent();
        }
        classortoxela logicaxela = new classortoxela();
        string ssql;
        double TotalIngresoCosto = 0;
        double TotalIngresoVenta = 0;
        bool NuevoCliente;// esto me servira para saber si el cliente fue buscado o creado o si solo son clientes normales para guradarlos automanticamente
        private void CargaDatos()
        {

            try
            {
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL' FROM clientes WHERE clientes.estadoid<>2 AND clientes.socio_comercial=1";
                gridLookSocioComercial.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookSocioComercial.Properties.DisplayMember = "NOMBRE SOCIO COMERCIAL";
                gridLookSocioComercial.Properties.ValueMember = "CODIGO";
                gridLookSocioComercial.Properties.NullText = "SELECCIONE SOCIO COMERCIAL";
                gridLookSocioComercial.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            }
            catch { }
                      
            try
            {
                /* jramirez 2013.07.24 */
                ssql = "SELECT distinct codigo_serie, CONCAT(nombre_documento,' [', serie_documento,']')  AS DOCUMENTO  FROM v_bodegas_series_usuarios  WHERE codigo_tipo=1 AND userid=" + clases.ClassVariables.id_usuario ;
                gridLookTipoDocumento.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookTipoDocumento.Properties.DisplayMember = "DOCUMENTO";
                gridLookTipoDocumento.Properties.ValueMember = "codigo_serie";
                gridLookTipoDocumento.EditValue = logicaorto.Tabla(ssql).Rows[0][0].ToString();

                gridLookTipoDocumento.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
                
            }
            catch
            {
                //MessageBox.Show("error");
            }

            /*Carga Bodegas*/
            CargaBodega(int.Parse(gridLookTipoDocumento.EditValue.ToString()));

            try
            {
                cadena = "SELECT tipo_pago as CODIGO, nombre_tipo_pago AS 'TIPO PAGO' FROM tipo_pago where estadoid<>2";
                gridLookTipoPago.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoPago.Properties.DisplayMember = "TIPO PAGO";
                gridLookTipoPago.Properties.ValueMember = "CODIGO";
                gridLookTipoPago.EditValue = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                gridLookTipoPago.Properties.BestFitMode = BestFitMode.BestFitResizePopup;

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
            }
            catch
            { }  
        }
        private void CargaBodega(int serie)
        {            
            try
            {
                /* ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid=1"; */
                /* jramirez 2013.07.24 */
                ssql = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega=1 AND userid=" + clases.ClassVariables.id_usuario;
                if (serie != 0)
                    ssql = ssql + " and codigo_serie="+serie.ToString();
                ssql = ssql + " order by codigo_bodega asc ";
                DataTable tempTabla = new DataTable();
                tempTabla = logicaxela.Tabla(ssql);
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
        private void CreaColumnas()
        {
            DataTable temporal = new DataTable();
            temporal.Columns.Add("IDBODEGA");
            temporal.Columns.Add("BODEGA");
            temporal.Columns.Add("CODIGO");
            temporal.Columns.Add("DESCRIPCION");
            temporal.Columns.Add("CANTIDAD");
            temporal.Columns.Add("VENTA");
            temporal.Columns.Add("SUBTOTAL");
            temporal.Columns.Add("DESCUENTO");                        
            temporal.Columns.Add("ACTUALIZA_PRECIO");
            temporal.Columns.Add("INGRESO_EGRESO");
            temporal.Columns.Add("EXISTENCIA");
            gridControl1.DataSource = temporal;
            gridView1.Columns["DESCRIPCION"].Width=200;
            gridView1.Columns["IDBODEGA"].Visible = false;
            gridView1.Columns["ACTUALIZA_PRECIO"].Visible = false;
            gridView1.Columns["INGRESO_EGRESO"].Visible = false;
            gridView1.Columns["EXISTENCIA"].Visible = false;
            gridView1.Columns["DESCUENTO"].Visible = false;
            gridView1.Columns["BODEGA"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["DESCRIPCION"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["SUBTOTAL"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["DESCUENTO"].OptionsColumn.ReadOnly = true;           

        }   
        private void frm_compras_Load(object sender, EventArgs e)
        {
            //gridLookTipoDocumento = new GridLookUpEdit();
            CargaDatos();
            id_proveedor = 0;
            id_usuario_descuento = "0";
            CreaColumnas();
            dateEdit1.DateTime = DateTime.Now;
            NuevoCliente = true;
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
                        textVenta.Text = tempTabla.Rows[0]["precio_venta"].ToString();
                        if (bandera_ingreso_egreso == "2")
                            cant_existencia = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);
                        textCantidadArt.Focus();
                    }
                }
            }
        }
        
        int ExistenciaHijo;
        int ExistenciaFija;
        int cantidadHijo;
        private void sbAgregaArt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int p = gridView1.DataRowCount;
                DialogResult a=DialogResult.Yes;
                if (p == 12)
                {
                    a = MessageBox.Show("Si usted agrega mas de 12 campos a la factura es posible que la impresion sea defectuosa. \n Desea continuar?", "ALERTA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }

                if (a == DialogResult.Yes) 
                {

                    if (dxValidationProvider1.Validate())
                    {
                        int existeCEx = 0;
                        for (int x = 0; x < gridView1.DataRowCount; x++)
                        {
                            if (gridView1.GetRowCellValue(x, "CODIGO").ToString().ToUpper().Contains("CARGOEXTRA"))
                            {
                                existeCEx++;
                            }
                        }

                        if (existeCEx > 0)
                        {
                            MessageBox.Show("No puede agregar Mas Productos Porque Ha Agregado El Recargo Extra, Si desea modificar la factura elimine el recargo extra y vuelva a aplicarlo si es necesario");
                            Cursor.Current = Cursors.Default;
                            return;
                        }


                        DataTable TempoPadre = new DataTable();
                        /* cadena = "SELECT articulos.compuesto FROM articulos WHERE articulos.codigo_articulo='" + id_articulo + "'";
                         jramirez 2013.07.04
                         */
                        cadena = "select f_es_compuesto('" + id_articulo + "') AS compuesto;";
                        string compuesto = logicaxela.Tabla(cadena).Rows[0]["compuesto"].ToString();
                        if (Convert.ToBoolean(logicaxela.Tabla(cadena).Rows[0]["compuesto"]))
                        {
                            // cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',articulos.precio_venta,articulos.costo FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND articulos.codigo_padre='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
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
                                    cantidadHijo = Convert.ToInt32(TempoPadre.Rows[x]["cantidad"]);
                                    if (ExistenciaHijo != 0)
                                    {
                                        if ((Convert.ToInt32(textCantidadArt.Text) * cantidadHijo) <= ExistenciaHijo)
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
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"])) * (ExistenciaFija));
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", bandera_actualiza_precio);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", bandera_ingreso_egreso);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "VENTA", TempoPadre.Rows[x]["precio_venta"]);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIA", ExistenciaHijo);
                                        TotalIngresoVenta = TotalIngresoVenta + (ExistenciaFija * Convert.ToDouble(TempoPadre.Rows[x]["precio_venta"]));
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
                                        descuento = Convert.ToDouble(textVenta.Text) * (Convert.ToDouble(textDescuentoPorce.Text.Replace("%", "")) / 100);
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
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(textVenta.Text)) * (Convert.ToDouble(textCantidadArt.Text)));
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", bandera_actualiza_precio);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", bandera_ingreso_egreso);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "VENTA", textVenta.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIA", cant_existencia);
                                    TotalIngresoVenta = TotalIngresoVenta + (Convert.ToDouble(textCantidadArt.Text) * Convert.ToDouble(textVenta.Text));
                                    gridView1.UpdateCurrentRow();
                                    CalculaDescuento();
                                    //textTotalVenta.Text = TotalIngresoVenta.ToString("C");
                                    textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text = textVenta.Text = "";
                                    textCodigoArt.Focus();
                                }
                            }
                            else
                                clases.ClassMensajes.NoHayExistenciaProd(this);

                    }
                    else
                        clases.ClassMensajes.FaltanDatosEnCampos(this);
                }
                //else
                //    //MessageBox.Show("No se pueden agregar mas de 12 campos a la factura", "", MessageBoxButtons.OK);
                //    MessageBox.Show("Si usted agrega mas de 12 campos es posible que la impresion salga corrida", "ALERTA", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch
            { 
            
            }
            Cursor.Current = Cursors.Default;
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {
        }

        void registarEnInvex()
        {
            
			Venta encabezadodeventa = null;
            string serie=gridLookTipoDocumento.Text;
            serie=serie.Replace("Factura [","");
            serie=serie.Replace("]","");
            string fdPago = "";

            if (radioGroup2.SelectedIndex == 1)
            {
                fdPago = "Credito";
            }
            else
            {
                fdPago = gridLookTipoPago.Text;
            }
            
            string telefonoClienteB = logicaorto.Tabla("SELECT telefono_casa FROM clientes WHERE codigo_cliente=" + id_cliente).Rows[0][0].ToString();
                        encabezadodeventa = integracion_ii.Class_venta.EncabezadoVenta(textNombreCliente.Text, textNitCliente.Text,telefonoClienteB, dateEdit1.DateTime, fdPago, Convert.ToInt16(textNoDocumento.Text),serie);


                        for (int x = 0; x < gridView1.DataRowCount; x++)
                        {
                            DataTable dt = new DataTable();
                            string cns = "SELECT categorias.`codigo_categoria`,categorias.`nombre_categoria`,articulos.`costo` FROM categorias INNER JOIN sub_categorias ON sub_categorias.`codigo_categoria`=categorias.`codigo_categoria` INNER JOIN articulos ON articulos.`codigo_categoria`=sub_categorias.`codigo_subcat` WHERE articulos.codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                            dt = logicaorto.Tabla(cns);
                            string codigoA = dt.Rows[0][0].ToString();
                            string nombreA = dt.Rows[0][1].ToString();
                            decimal costoA = 0;
                            try
                            {
                                costoA = Convert.ToDecimal(dt.Rows[0][2].ToString());
                            }
                            catch { }
                            int cantidadA = Convert.ToInt16(gridView1.GetRowCellValue(x, "CANTIDAD"));
                            decimal precioA = Convert.ToDecimal(gridView1.GetRowCellValue(x, "VENTA"));
                            integracion_ii.Class_venta.DetalleVenta(encabezadodeventa, cantidadA, precioA, codigoA, nombreA,costoA);
                        }


                        if (fdPago=="Credito")
                        {
                            integracion_ii.Class_venta.ValidarEnCretido(encabezadodeventa);
                        }
                        else if (fdPago == "Efectivo")
                        {
                            integracion_ii.Class_venta.validarenEfectivo(encabezadodeventa);
                        }
                        else if (fdPago == "Deposito")
                        {
                            Form validar = new f_validarFactura(2, encabezadodeventa);
                            validar.ShowDialog();
                        }
                        else if (fdPago == "Cheque")
                        {
                            Form validar = new f_validarFactura(1, encabezadodeventa);
                            validar.ShowDialog();
                        }

        }

        int cant_existencia;
        private void textNombreArti_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dxValidationEncabezado.Validate())
            {               
                    cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.precio_venta AS 'PRECIO VENTA',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid=1 AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                clases.ClassVariables.cadenabusca = cadena;
                Form nuevo = new Buscador.Buscador();
                nuevo.ShowDialog();
                if (Buscador.Buscador.SeleccionSiNo)
                {
                    id_articulo = clases.ClassVariables.id_busca;
                        cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',costo,articulos.precio_venta FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo where articulos.codigo_articulo='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                        tempTabla = logicaxela.Tabla(cadena);
                    if (tempTabla.Rows.Count > 0)
                    {
                        textCodigoArt.Text = tempTabla.Rows[0]["CODIGO"].ToString();
                        textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString(); 
                        textVenta.Text = tempTabla.Rows[0]["precio_venta"].ToString();                        
                            cant_existencia = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);
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
            
            //try
            //{
                cadena = "SELECT header_doctos_inv.id_documento FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie='" + gridLookTipoDocumento.EditValue+ "' and header_doctos_inv.no_documento=" + textNoDocumento.Text;
                if (logicaorto.ExisteRegistro(cadena) == false)
                {

                if (id_cliente=="0")
                {
                    if (!textNitCliente.Text.Contains("C"))
                    {
                        string consulta = "SELECT * FROM clientes WHERE clientes.nit='" + textNitCliente.Text + "'";
                        if (logicaxela.ExisteRegistro(consulta))
                        {
                            alertControl1.Show(this, "ADVERTENCIA", "EL CLIENTE YA EXISTE, VERIFIQUE POR FAVOR", Properties.Resources.Advertencia64);
                            return;
                        }
                        else
                        {
                            consulta = "SELECT * FROM clientes where nombre_cliente like '%"+textNombreCliente.Text+"%'";
                            if (logicaorto.ExisteRegistro(consulta))
                            {
                                if (MessageBox.Show("Es posible que el cliente ya exita...\n ¿Desea crearlo de todos modos[Pueda ser que se duplique]?", "INFORMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                {
                                    cadena = "INSERT into clientes(nombre_cliente, nit, telefono_casa, telefono_celular,fecha_ingreso,direccion,referido_por,usuario_creador, socio_comercial,estadoid, codigo_tipoc) " +
                                                "VALUES ('" + textNombreCliente.Text + "', '" + textNitCliente.Text + "', '" + textTelefonoCliente.Text + "', '" + textTelefonoCliente.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + textDireccion.Text + "'," + gridLookSocioComercial.EditValue + "," + clases.ClassVariables.id_usuario + ",0,1, 1)";
                                    id_cliente = logicaorto.nuevoid(cadena);
                                    tipo_cliente_conta = "1";
                                }
                                else
                                { return; }
                            }
                            else
                            {
                                cadena = "INSERT into clientes(nombre_cliente, nit, telefono_casa, telefono_celular,fecha_ingreso,direccion,referido_por,usuario_creador, socio_comercial,estadoid, codigo_tipoc) " +
                                            "VALUES ('" + textNombreCliente.Text + "', '" + textNitCliente.Text + "', '" + textTelefonoCliente.Text + "', '" + textTelefonoCliente.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + textDireccion.Text + "'," + gridLookSocioComercial.EditValue + "," + clases.ClassVariables.id_usuario + ",0,1, 1)";
                                id_cliente = logicaorto.nuevoid(cadena);
                                tipo_cliente_conta = "1";
                            }
                        }
                    }
                    else
                    {
                        string consulta = "SELECT * FROM clientes WHERE clientes.nombre_cliente='" + textNombreCliente.Text+ "'";
                        if (logicaxela.ExisteRegistro(consulta))
                        {
                            alertControl1.Show(this, "ADVERTENCIA", "EL CLIENTE YA EXISTE, VERIFIQUE POR FAVOR", Properties.Resources.Advertencia64);
                            if (MessageBox.Show("Es posible que el Cliente ya exista, verifique los registros, de clic en el botón [SI] si desea crear un nuevo registro de lo contrario hacer clic en el botón [NO] y hacer la busca del Cliente.", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                cadena = "INSERT into clientes(nombre_cliente, nit, telefono_casa, telefono_celular,fecha_ingreso,direccion,referido_por,usuario_creador, socio_comercial,estadoid, codigo_tipoc) " +
                                        "VALUES ('" + textNombreCliente.Text + "', '" + textNitCliente.Text + "', '" + textTelefonoCliente.Text + "', '" + textTelefonoCliente.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + textDireccion.Text + "'," + gridLookSocioComercial.EditValue + "," + clases.ClassVariables.id_usuario + ",0,1, 1)";
                                id_cliente = logicaorto.nuevoid(cadena);
                            }
                            else
                                return;
                        }
                        else
                        {
                            cadena = "INSERT into clientes(nombre_cliente, nit, telefono_casa, telefono_celular,fecha_ingreso,direccion,referido_por,usuario_creador, socio_comercial,estadoid, codigo_tipoc) " +
                                        "VALUES ('" + textNombreCliente.Text + "', '" + textNitCliente.Text + "', '" + textTelefonoCliente.Text + "', '" + textTelefonoCliente.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + textDireccion.Text + "'," + gridLookSocioComercial.EditValue + "," + clases.ClassVariables.id_usuario + ",0,1, 1)";
                            id_cliente = logicaorto.nuevoid(cadena);
                        }
                    }
                }
                //else
                //{
                //    cadena = "SELECT * FROM clientes WHERE clientes.nit='" + textNitCliente.Text + "'";
                //    if (logicaxela.ExisteRegistro(cadena))
                //    {
                //        cadena = "SELECT * FROM clientes WHERE clientes.nit='" + textNitCliente.Text + "'";
                //        id_cliente=logicaorto.Tabla(cadena).Rows[0]["codigo_cliente"].ToString();
                //    }
                //    else
                //    {
                //        cadena = "INSERT into clientes(nombre_cliente, nit, telefono_casa, telefono_celular,fecha_ingreso,direccion,referido_por,usuario_creador, socio_comercial,estadoid, codigo_tipoc) " +
                //                    "VALUES ('" + textNombreCliente.Text + "', '" + textNitCliente.Text + "', '" + textTelefonoCliente.Text + "', '" + textTelefonoCliente.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + textDireccion.Text + "'," + gridLookSocioComercial.EditValue + "," + clases.ClassVariables.id_usuario + ",0,1, 1)";
                //        id_cliente = logicaorto.nuevoid(cadena);
                //    }                       
                //}

                try
                {
                    conexion.Open();
                }
                catch
                {
                    conexion.Close();
                    conexion.Open();
                }
                transac = conexion.BeginTransaction();   

                    int iddvendedor=Convert.ToInt16(gridLookUpEdit1.EditValue);
                ssql = "INSERT into header_doctos_inv(codigo_serie,tipo_pago,no_documento,codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento,socio_comercial,estadoid,contado_credito,refer_documento,vendedor) " +
                        "VALUES ("+gridLookTipoDocumento.EditValue+","+gridLookTipoPago.EditValue+",'"+textNoDocumento.Text+"',"+id_cliente+" ,'"+dateEdit1.DateTime.ToString("yyyy-MM-dd")+"', "+TotalIngresoCosto+", "+TotalDescuento+", "+TotalIngresoVenta+", "+clases.ClassVariables.id_usuario+", "+id_usuario_descuento+","+gridLookSocioComercial.EditValue+",4,"+radioGroup2.SelectedIndex+",'"+textDeposito.Text+"'"+","+iddvendedor.ToString()+");SELECT LAST_INSERT_ID();";

                
                    
                    
                    




                comando = new MySqlCommand(ssql, conexion);
                comando.Transaction=transac;                
                id_nuevoIngreso=comando.ExecuteScalar().ToString();
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    ssql = "INSERT into detalle_doctos_inv(id_documento, cantidad_enviada, precio_unitario, precio_total,codigo_articulo, codigo_bodega, precio_venta) "+
                               "VALUES (" + id_nuevoIngreso + ", " + gridView1.GetRowCellValue(x, "CANTIDAD") + ", " + gridView1.GetRowCellValue(x, "VENTA") + ", " + gridView1.GetRowCellValue(x, "SUBTOTAL") + ",'" + gridView1.GetRowCellValue(x, "CODIGO") + "', " + gridView1.GetRowCellValue(x, "IDBODEGA") + ", " + gridView1.GetRowCellValue(x, "VENTA") + ");";
                    comando = new MySqlCommand(ssql, conexion);
                    comando.Transaction = transac;
                    comando.ExecuteNonQuery();                    
                        ssql = "update bodegas SET  existencia_articulo = existencia_articulo -" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA") + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                        comando = new MySqlCommand(ssql, conexion);
                        comando.Transaction = transac;
                        comando.ExecuteNonQuery();

                }

                if (Class_integracion.logeado == true)
                {
                    registarEnInvex();
                }
                        //**********************************************************************INGRESO DE PARTIDAS CONTABILIDAD**************************************************************
                cadena = "SELECT * FROM catalogo_partidas WHERE activo=1 AND id_cond in(SELECT idcondiciones_contabilidad FROM condiciones_contabilidad WHERE codigo_serie=" + gridLookTipoDocumento.EditValue + " AND tipo_pago=" + radioGroup2.SelectedIndex + " AND tipo_cliente=" + tipo_cliente_conta + " AND activo=1)";
                DataTable partidas = new DataTable();
                double MontoPartida = 0;
                partidas = logicaorto.Tabla(cadena);
                cadena="INSERT INTO partidas_header(no_partida, folio, descripcion, fecha, monto_debe, monto_haber, codigo_serie, id_docto, usuario, estado)"+
                        "VALUES((SELECT COALESCE(MAX(p.no_partida),0)+1 AS 'noPartida' FROM partidas_header p), '1', 'VENTA AL CONTADO', '" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + "', '" + TotalIngresoVenta + "', '" + TotalIngresoVenta + "', '" + gridLookTipoDocumento.EditValue + "', '" + id_nuevoIngreso + "', '" + clases.ClassVariables.id_usuario + "', '1');select last_insert_id();";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction=transac;     
                string id_partida_header=comando.ExecuteScalar().ToString();
                   foreach(DataRow fila in partidas.Rows)
                    {
                        if (fila["id_cuenta_debe"].ToString() != "0")
                        {
                            MontoPartida = TotalIngresoVenta * (Convert.ToDouble(fila["porcentaje"])/100);
                            cadena = "INSERT INTO partidas_detalle (id_partidas_header, id_cuenta, monto_debe, monto_haber, descripcion_documento, numero_documento, activo)" +
                            "VALUES('" + id_partida_header + "', '" + fila["id_cuenta_debe"] + "', '" + MontoPartida + "', '0', 'DEPOSITO', '" + textDeposito.Text + "', 1);";
                        }
                        else
                            if (fila["id_cuenta_haber"].ToString() != "0")
                            {
                                MontoPartida = TotalIngresoVenta * (Convert.ToDouble(fila["porcentaje"]) / 100);
                                cadena= "INSERT INTO partidas_detalle (id_partidas_header, id_cuenta, monto_debe, monto_haber, descripcion_documento, numero_documento, activo)" +
                        "VALUES('" + id_partida_header + "', '" + fila["id_cuenta_haber"] + "', '0', '" + MontoPartida + "', 'FACTURA', '" + textNoDocumento.Text + "', 1);";
                            }
                        comando = new MySqlCommand(cadena, conexion);
                        comando.Transaction = transac;
                        comando.ExecuteNonQuery();
                    }                
                
                //**********************************************************************FIN INGRESO PARTIDAS CONTABILIDAD*************************************************************
                transac.Commit();
                simplePrinter.Enabled = true;
                sbAceptar.Enabled = false;
                simplePrinter.Enabled = true;
                groupControl1.Enabled = false;
                groupControl2.Enabled = false;
                panelControl1.Enabled = false;
                clases.ClassMensajes.INSERTO(this);
                }
                else
                {
                    alertControl1.Show(this, "INFORMACION", "EL NUMERO DE DOCUMENTO YA EXISTE", Properties.Resources.Advertencia64);
                }
            //}
            //catch
            //{
            //    transac.Rollback();
            //    clases.ClassMensajes.NoINSERTO(this);
            //}
            //finally
            //{
            //    conexion.Close();
            //}
        }
        private void sbAceptar_Click(object sender, EventArgs e)
        {
            if(dxValidationProvider2.Validate() & gridView1.DataRowCount>0 & gridLookTipoDocumento.EditValue != "")
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

                if (fdPago == "Credito" || fdPago == "Efectivo" || fdPago == "Cheque" || fdPago == "Deposito")
                    registraIngreso();
                else
                    MessageBox.Show("Seleccione una forma de pago valida, estas pueden ser:\nCredito\nEfectivo\nCheque\nDeposito","Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            textVenta.Text = "";            
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
             textNitCliente.Text = textNombreCliente.Text = textSocioComercial.Text = textTelefonoCliente.Text = "";
            CargaDatos();

            #region calculo de nuevo correlativo

            try
            {
                cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC' FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue + " ORDER BY header_doctos_inv.no_documento DESC LIMIT 1";
                textNoDocumento.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
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
            Cursor = Cursors.WaitCursor;
            try
            {
                int p = gridView1.DataRowCount;
                DialogResult a=DialogResult.Yes;
                if (p >= 12)
                {
                    a = MessageBox.Show("Esta factura tiene mas de 12 campos, es posible que la impresion sea defectuosa. \n Desea continuar?", "ALERTA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }

                if (a == DialogResult.Yes)
                {
                    //Pedido.Factura.XtraReportFactura reporte = new Pedido.Factura.XtraReportFactura();
                    //reporte.Parameters["ID"].Value = id_nuevoIngreso;
                    //reporte.Parameters["LETRAS"].Value = logicaorto.enletras(double.Parse(textPrecioTotal.Text.ToString(), NumberStyles.Currency).ToString());
                    //reporte.Parameters["Vende"].Value = gridLookUpEdit1.Text;
                    //reporte.Parameters["SM_PC_FO"].Value = "Paciente: " + textNombreCliente.Text + " - Socio Comercial: " + gridLookSocioComercial.Text + " - Operado: " + dateEdit1.Text;
                    //reporte.RequestParameters = false;
                    //reporte.ShowPreviewDialog();



                    string letras = logicaorto.enletras(double.Parse(textPrecioTotal.Text.ToString(), NumberStyles.Currency).ToString());

                    Pedido.Factura.F_impresion nf = new Pedido.Factura.F_impresion();

                    string con, cre;

                    //ver si es credito o contado
                    if (radioGroup2.SelectedIndex == 0)
                    {
                        con = "X";
                        cre = " ";
                    }
                    else
                    {
                        con = " ";
                        cre = "X";
                    }



                    //nf.facturaOtroTipo(Convert.ToInt16(id_nuevoIngreso), letras, con, cre, Convert.ToInt16(gridLookTipoDocumento.EditValue));
                    if (gridLookTipoDocumento.Text == "Factura [A]")
                    {
                        nf.facturaA(Convert.ToInt16(id_nuevoIngreso), letras, con, cre, Convert.ToInt16(gridLookTipoDocumento.EditValue), gridLookSocioComercial.EditValue.ToString());
                    }
                    else if (gridLookTipoDocumento.Text == "Factura [B]")
                    {
                        nf.facturaB(Convert.ToInt16(id_nuevoIngreso), letras, con, cre, Convert.ToInt16(gridLookTipoDocumento.EditValue), gridLookSocioComercial.EditValue.ToString());
                    }
                    else if (gridLookTipoDocumento.Text == "Factura [C]")
                    {
                        nf.facturaC(Convert.ToInt16(id_nuevoIngreso), letras, con, cre, Convert.ToInt16(gridLookTipoDocumento.EditValue), gridLookSocioComercial.EditValue.ToString());
                    }
                    else if (gridLookTipoDocumento.Text == "Factura [D]")
                    {
                        nf.facturaD(Convert.ToInt16(id_nuevoIngreso), letras, con, cre, Convert.ToInt16(gridLookTipoDocumento.EditValue), gridLookSocioComercial.EditValue.ToString());
                    }
                    else if (gridLookTipoDocumento.Text == "Factura [E]")
                    {
                        nf.facturaE(Convert.ToInt16(id_nuevoIngreso), letras, con, cre, Convert.ToInt16(gridLookTipoDocumento.EditValue), gridLookSocioComercial.EditValue.ToString());
                    }


                    nf.ShowDialog();
                }

            }
            catch
            {
            
            }

            Cursor = Cursors.Default;
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
                        TempTotalPedido = TotalIngresoVenta;                        
                        textTotalDescuento.Text = TotalDescuento.ToString("C");
                        textPrecioTotal.Text = (TotalIngresoVenta).ToString("C");                        
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
        /*cambio de serie*/
        string bandera_actualiza_precio,bandera_ingreso_egreso;
        string cadena;
        private void gridLookTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {           
            cadena = "SELECT tipos_documento.actualiza_precios,tipos_documento.signo FROM tipos_documento INNER JOIN series_documentos ON tipos_documento.codigo_tipo=series_documentos.codigo_tipo WHERE series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue;
            
            tempTabla = logicaxela.Tabla(cadena);
            if (tempTabla.Rows.Count > 0)
            {
                bandera_actualiza_precio = tempTabla.Rows[0][0].ToString();
                bandera_ingreso_egreso = tempTabla.Rows[0][1].ToString();
            }

            CreaColumnas();
            TotalIngresoCosto = 0;
            TotalDescuento = 0;
            textTotalIva.Text = TotalDescuento.ToString("C");
            TotalIngresoVenta = 0;
            textTotalSinIva.Text = TotalIngresoVenta.ToString("C");
            textPrecioTotal.Text = TotalIngresoVenta.ToString("C");

            try
            {
                cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC' FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie=" + gridLookTipoDocumento.EditValue + " ORDER BY header_doctos_inv.no_documento DESC LIMIT 1";
                textNoDocumento.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
            }
            catch
            {
                textNoDocumento.Text = "1";
            }
            CargaBodega(int.Parse(gridLookTipoDocumento.EditValue.ToString()));
        }     
        double cant_devolucion, preciounitario, total_devuelta;// solo me sirve para calcular el total de devolucion
        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            total_devuelta = 0;
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (Convert.ToDouble(gridView1.GetFocusedRowCellValue("CANTIDAD")) * Convert.ToDouble(gridView1.GetFocusedRowCellValue("VENTA"))));
            for (int x = 0; x < gridView1.DataRowCount; x++)
            {
                cant_devolucion = Convert.ToDouble(gridView1.GetRowCellValue(x, "CANTIDAD"));
                preciounitario = Convert.ToDouble(gridView1.GetRowCellValue(x, "VENTA"));
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
        string id_cliente, id_socioComercial, id_SocioComercialCompara, tipo_cliente_conta;
        classortoxela logicaorto = new classortoxela();
        
        private void textNombreCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            id_cliente = "0";
            if (e.KeyChar == 13)
            {
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL',clientes.nit AS 'NIT',clientes.telefono_celular AS 'TELEFONO CELULAR',(if(clientes.socio_comercial=1,'SI','NO')) AS 'SOCIO COMERCIAL' FROM clientes where estadoid<>2";
                clases.ClassVariables.cadenabusca = cadena;
                Form nuevo = new Buscador.Buscador();
                nuevo.ShowDialog();
                if (Buscador.Buscador.SeleccionSiNo)
                {
                    id_cliente = clases.ClassVariables.id_busca;
                    DataTable tempCliente = new DataTable();
                    cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_celular,clientes.direccion,clientes.tipo_cliente_conta FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                    tempCliente = logicaorto.Tabla(cadena);
                    textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                    textNitCliente.Text = tempCliente.Rows[0]["nit"].ToString();
                    id_socioComercial = tempCliente.Rows[0]["referido_por"].ToString();
                    textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_celular"].ToString();
                    textDireccion.Text = tempCliente.Rows[0]["direccion"].ToString();
                    tipo_cliente_conta= tempCliente.Rows[0]["tipo_cliente_conta"].ToString();

                    //cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                    //try
                    //{
                    //    tempCliente = logicaorto.Tabla(cadena);
                    //    id_SocioComercialCompara = id_socioComercial;
                    //    textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                    //}
                    //catch
                    //{ }
                    NuevoCliente = false;
                }
                else
                    NuevoCliente = true;
                e.KeyChar = Convert.ToChar(13);
            }
            else
                NuevoCliente =true;
        }

        private void textSocioComercial_KeyPress(object sender, KeyPressEventArgs e)
        {
            cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL' FROM clientes WHERE clientes.estadoid<>2 AND clientes.socio_comercial=1";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                id_socioComercial = clases.ClassVariables.id_busca;
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                textSocioComercial.Text = logicaorto.Tabla(cadena).Rows[0]["NOMBRE CLIENTE"].ToString();
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
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.telefono_celular,clientes.direccion,clientes.tipo_cliente_conta FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                tempCliente = logicaorto.Tabla(cadena);
                textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                textNitCliente.Text = tempCliente.Rows[0]["nit"].ToString();
                textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_celular"].ToString();
                textDireccion.Text = tempCliente.Rows[0]["direccion"].ToString();
                tipo_cliente_conta = tempCliente.Rows[0]["tipo_cliente_conta"].ToString();
                NuevoCliente = false;
            }
            else
                NuevoCliente = true;

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.sociocomercial = true;
            clases.ClassVariables.llamadoDentroForm = true;
            clases.ClassVariables.idnuevo = "";
            Form hijo = new Clientes.form_cliente();
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
            if (clases.ClassVariables.idnuevo != "")
            {
                id_cliente = clases.ClassVariables.idnuevo;
                DataTable tempCliente = new DataTable();
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE' FROM clientes " +
                            "WHERE socio_comercial=1 AND clientes.codigo_cliente=" + id_cliente;
                tempCliente = logicaorto.Tabla(cadena);
                textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
            }
        }

        private void gridLookSocioComercial_EditValueChanged(object sender, EventArgs e)
        {
            try { id_socioComercial = gridLookSocioComercial.EditValue.ToString(); }
            catch{}
        }

        private void textNitCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            id_cliente = "0";
            if (e.KeyChar == 13)
            {
                cadena = "SELECT clientes.codigo_cliente FROM clientes WHERE clientes.nit='" + textNitCliente.Text + "'";
                tempTabla = logicaorto.Tabla(cadena);
                if (tempTabla.Rows.Count > 0)
                {
                    id_cliente = tempTabla.Rows[0][0].ToString();
                    DataTable tempCliente = new DataTable();
                    cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_casa,contacto,tipo_cliente_conta FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                    tempCliente = logicaorto.Tabla(cadena);
                    textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                    textNitCliente.Text = tempCliente.Rows[0]["nit"].ToString();
                    id_socioComercial = tempCliente.Rows[0]["referido_por"].ToString();
                    textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_casa"].ToString();
                    tipo_cliente_conta = tempCliente.Rows[0]["tipo_cliente_conta"].ToString();
                    //cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.socio_comercial FROM clientes WHERE clientes.codigo_cliente=" + id_socioComercial;
                    //try
                    //{
                    //    tempCliente = logicaorto.Tabla(cadena);
                    //    id_SocioComercialCompara = id_socioComercial;
                    //    textSocioComercial.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                    //}
                    //catch
                    //{ }
                    NuevoCliente = false;

                }
                else
                {
                    textNombreCliente.Text = "";
                    textNitCliente.Text = "";
                    textTelefonoCliente.Text = "";
                    tipo_cliente_conta = "0";
                    NuevoCliente = true;
                }
            }
            else
                NuevoCliente = true;
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

        private void textNitCliente_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridLookTipoPago_EditValueChanged(object sender, EventArgs e)
        {
            if ((radioGroup2.SelectedIndex == 0) && (Convert.ToInt16(gridLookTipoPago.EditValue) == 5))
            {
                gridLookTipoPago.EditValue = 0;
            }
        }

       
        private void textCodigoArt_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void textEdit2_EditValueChanged_1(object sender, EventArgs e)
        {
            double tmpCEv = 0.00;

            if (checkBox2.Checked)
            {
                try
                {
                    //tmpCEv = Convert.ToDouble(textTotalDeFactura.Text.ToString().Replace("Q", "")) * Convert.ToDouble(textEdit2.Text) / 100;
                    tmpCEv = double.Parse(textPrecioTotal.Text.ToString(), NumberStyles.Currency) * Convert.ToDouble(textEdit2.Text) / 100;
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

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            double tmpCEv = 0.00;

            if (checkBox2.Checked)
            {
                try
                {
                    //tmpCEv = Convert.ToDouble(textTotalDeFactura.Text.ToString().Replace("Q", "")) * Convert.ToDouble(textEdit2.Text) / 100;
                    tmpCEv = double.Parse(textPrecioTotal.Text.ToString(), NumberStyles.Currency) * Convert.ToDouble(textEdit2.Text) / 100;
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

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            int existeCEx = 0;
            for (int x = 0; x < gridView1.DataRowCount; x++)
            {
                if (gridView1.GetRowCellValue(x, "CODIGO").ToString().ToUpper().Contains("CARGOEXTRA"))
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
                    gridView1.AddNewRow();
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "IDBODEGA", gridLookBodega.EditValue);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "BODEGA", gridLookBodega.Text + "[" + gridLookBodega.EditValue + "]");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", "CARGOEXTRA");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", "Cargo Extra");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", 1);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUBTOTAL", (vCEX));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ACTUALIZA_PRECIO", false);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "INGRESO_EGRESO", false);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "VENTA", vCEX);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIA", 1);
                    TotalIngresoVenta = TotalIngresoVenta + (ExistenciaFija * vCEX);
                    gridView1.UpdateCurrentRow();
                    CalculaDescuento();
                    //textTotalVenta.Text = TotalIngresoVenta.ToString("C");
                    //double tmpNewTotx = 0;
                    //tmpNewTotx = double.Parse(textTotalDeFactura.Text.ToString(), NumberStyles.Currency) + double.Parse(textEdit3.Text.ToString(), NumberStyles.Currency);
                    //textTotalDeFactura.Text = tmpNewTotx.ToString();
                    MessageBox.Show("Se ha aplicado el cargo extra exitosamente");
                }
            }
            else
            {
                MessageBox.Show("No puede volver a aplicar el cargo extra, Si desea cambiar el cargo extra actual elimine y vuelva a cargar el cargo extra actual.");
            }
        }

        private void gridControl1_Validated(object sender, EventArgs e)
        {
            //recalcular el subtotal y total de cada columna
            int largo = gridView1.DataRowCount;
            double total = 0;

            int cant = 0;
            double pu = 0;
            double subtotal = 0;
            for (int i = 0; i < largo; i++)
            {
                subtotal = 0;
                cant = Convert.ToInt16(gridView1.GetRowCellValue(i, "CANTIDAD"));
                pu = Convert.ToDouble(gridView1.GetRowCellValue(i, "VENTA"));
                subtotal = subtotal + (cant * pu);
                gridView1.SetRowCellValue(i, "SUBTOTAL", subtotal);
                total = total + subtotal;
            }
            textPrecioTotal.Text = total.ToString();
        }

    

      
    }
}