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

namespace ortoxela.Pedido.EnvioSinPedido
{
    public partial class f_EnvioEspecial : DevExpress.XtraEditors.XtraForm
    {
        public f_EnvioEspecial()
        {
            InitializeComponent();
            CreaColumnas();
            CargaDatosCombos();
            dxValidationProvider1.Validate();
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string id_socioComercial = "1";
        string id_SocioComercialCompara = "1";
        string FormaDePagoId = "5";


        int serieID = 38;
        
        
        classortoxela logicaorto = new classortoxela();

        private void CargaBodega(int serie)
        {
            try
            {
                /* ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid=1"; */
                /* jramirez 2013.07.24 */
                string                 cadena = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega=1 AND userid=" + clases.ClassVariables.id_usuario;
                if (serie != 0)
                    cadena = cadena + " and codigo_serie=" + serie.ToString();
                cadena = cadena + " order by codigo_bodega asc ";
                DataTable tempTabla = new DataTable();
                tempTabla = logicaorto.Tabla(cadena);
                gridLookBodega.Properties.DataSource = tempTabla;
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
                //gridLookBodega.EditValue = int.Parse(tempTabla.Rows[0]["CODIGO"].ToString());
                gridLookBodega.Properties.NullText = "";
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
                string cadena = "SELECT distinct codigo_serie AS CODIGO, serie_documento AS 'SERIE DOCUMENTO'  FROM v_bodegas_series_usuarios  WHERE codigo_tipo=5 AND userid=" + clases.ClassVariables.id_usuario;
                gridLookSerie.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookSerie.Properties.DisplayMember = "SERIE DOCUMENTO";
                gridLookSerie.Properties.ValueMember = "CODIGO";

                gridLookSerie.EditValue = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                gridLookSerie.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            }
            catch
            {
            }


            try
            {
                string cadena = "SELECT codigo_cliente AS codigodoc,nombre_cliente AS nombreDoctor FROM clientes WHERE clientes.`codigo_tipoc` =7 and estadoid=1 ";
                gridLookDoctores.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookDoctores.Properties.DisplayMember = "nombreDoctor";
                gridLookDoctores.Properties.ValueMember = "codigodoc";
                gridLookDoctores.Properties.NullText = "";
                gridLookDoctores.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            }
            catch
            { }
        }


        private void textHospital_KeyPress(object sender, KeyPressEventArgs e)
        {
            string cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes WHERE clientes.estadoid<>2 and clientes.socio_comercial=1";
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

        private void f_EnvioEspecial_Load(object sender, EventArgs e)
        {
            textNombreCliente.Focus();
            gridLookDoctores.Text = "";
        }

        string id_temp_Cliente = "1";
        
        private void textCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            string cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit AS 'NIT',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes where estadoid<>2";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                id_temp_Cliente = clases.ClassVariables.id_busca;
                DataTable tempCliente = new DataTable();
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_casa,contacto FROM clientes WHERE clientes.codigo_cliente=" + id_temp_Cliente;
                tempCliente = logicaorto.Tabla(cadena);
                textNombreCliente.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                id_socioComercial = tempCliente.Rows[0]["referido_por"].ToString();
                textUtilizadoPaciente.Text = tempCliente.Rows[0]["nombre_paciente"].ToString();
                //textDoctorPedido.Text = tempCliente.Rows[0]["contacto"].ToString();
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

        private void textEditDoctor_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void gridLookSerie_TextChanged(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();
        }

        private void textNoDocumento_TextChanged(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();
        }

        private void textSocioComercial_TextChanged(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();
        }

        private void textCliente_TextChanged(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();
        }

        private void dateFechaPedido_TextChanged(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();
        }

        private void gridLookSerie_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string cadena = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE  FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_serie= " + gridLookSerie.EditValue;
                gridLookBodega.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookBodega.Properties.DisplayMember = "NOMBRE";
                gridLookBodega.Properties.ValueMember = "CODIGO";
            }
            catch
            { }

            try
            {
                string cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC',header_doctos_inv.codigo_serie FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_tipo=5 AND series_documentos.codigo_serie=" + gridLookSerie.EditValue + " ORDER BY header_doctos_inv.no_documento DESC LIMIT 1";
                textNoDocumento.Text = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                CargaBodega(int.Parse(logicaorto.Tabla(cadena).Rows[0][1].ToString()));

            }
            catch
            {
                textNoDocumento.Text = "1";
            }
            dxValidationProvider1.Validate();
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
            gridView1.Columns["CODIGO"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["SUB TOTAL"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["EXISTENCIABODEGA"].OptionsColumn.ReadOnly = true;

        }


        string id_articulo = "";
        DataTable tempTabla = new DataTable();
        int ExistenciaProd, existencia_minima;
        private void textNombreArti_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dxValidationProvider2.Validate()==true)
            {
                string cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.precio_venta AS 'PRECIO VENTA',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND bodegas.codigo_bodega=" + gridLookBodega.EditValue;
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
            }
        }


        bool banderaRepetido;
        double TotalPedido = 0;
        private void sbAgregaArt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (dxValidationProvider2.Validate()==true)
                {
                    DataTable TempoPadre = new DataTable();
                    string cadena = "select f_es_compuesto('" + id_articulo + "') AS compuesto;";
                    string compuesto = logicaorto.Tabla(cadena).Rows[0]["compuesto"].ToString();
                    if (Convert.ToBoolean(logicaorto.Tabla(cadena).Rows[0]["compuesto"]))
                    {
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
                            }
                            else
                                clases.ClassMensajes.ProdYaExisteEnListado(this);
                        }

                        textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text = "";
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
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "EXISTENCIABODEGA", ExistenciaProd);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "MINIMO", existencia_minima);
                                gridView1.UpdateCurrentRow();
                                TotalPedido = TotalPedido + (Convert.ToDouble(textCantidadArt.Text) * Convert.ToDouble(textVenta.Text));
                                
                                textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = textVenta.Text = "";
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


            int a = gridView1.RowCount;
            double st = 0;
            try
            {
                for (int i = 0; i < a; i++)
                {
                    st = st + Convert.ToDouble(gridView1.GetRowCellValue(i, "SUB TOTAL").ToString());
                }

                textTotalPedido.Text = st.ToString();
            }
            catch
            { }

            Cursor.Current = Cursors.Default;
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
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


        bool Band_permite_borrar;
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (Band_permite_borrar==true || Band_permite_borrar==false)
                {
                    TotalPedido = TotalPedido - (Convert.ToDouble(gridView1.GetFocusedRowCellValue("CANTIDAD")) * Convert.ToDouble(gridView1.GetFocusedRowCellValue("PRECIO UNITARIO")));
                    gridView1.DeleteSelectedRows();
                    gridView1.UpdateCurrentRow();
                    textTotalPedido.Text = TotalPedido.ToString("C");
                }


                int a = gridView1.RowCount;
                double st = 0;
                try
                {
                    for (int i = 0; i < a; i++)
                    {
                        st = st + Convert.ToDouble(gridView1.GetRowCellValue(i, "SUB TOTAL").ToString());
                    }

                    textTotalPedido.Text = st.ToString();
                }
                catch
                { }

            }
            catch
            { }
        }

        private void gridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            ColumnView ver = sender as ColumnView;
            int cant1 = Convert.ToInt32(ver.GetFocusedRowCellValue("CANTIDAD"));
            int cant2 = Convert.ToInt32(ver.GetFocusedRowCellValue("EXISTENCIABODEGA"));
            
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
                }
                //textTotalPedido.Text = TotalPedido.ToString();

                
        }

        private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.Name == "colCANTIDAD" || e.Column.Name == "colPRECIOUNITARIO")
                {
                    decimal zo = 0;
                    decimal ex = 0;

                    zo = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PRECIO UNITARIO"));

                    ex = Convert.ToDecimal(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD"));

                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SUB TOTAL", zo*ex);
                }

                else if (e.Column.Name == "colSUBTOTAL")
                {
                    int a = gridView1.RowCount;
                    double st = 0;
                    for (int i = 0; i < a; i++)
                    {
                        st = st + Convert.ToDouble(gridView1.GetRowCellValue(i, "SUB TOTAL").ToString());
                    }

                    textTotalPedido.Text = st.ToString();
                }
            }
            catch
            { }
        }


        string textNumeroDocVale = "1";
        private void sbAceptar_Click(object sender, EventArgs e)
        {
            /* Guardar Vale */
            if(dxValidationProvider1.Validate()==true && gridView1.DataRowCount>0)
            {
                try
                {
                    string cadena = "SELECT (header_doctos_inv.no_documento+1)AS 'NODOC' FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_tipo=3 AND series_documentos.codigo_serie="+ serieID.ToString()+" ORDER BY header_doctos_inv.no_documento DESC LIMIT 1";
                    textNumeroDocVale = logicaorto.Tabla(cadena).Rows[0][0].ToString();
                }
                catch
                {
                    textNumeroDocVale = "1";
                }

                string  cadena2 = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE header_doctos_inv.no_documento=" + textNumeroDocVale + " AND series_documentos.codigo_serie="+serieID.ToString();
                    if (logicaorto.ExisteRegistro(cadena2) == false)
                    {
                        insertaVale();
                    }
            }
            else
                clases.ClassMensajes.FaltanDatosEnCampos(this);
        }

        private void sbnuevo_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Desea cargar un nuevo documento?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                sbAceptar.Enabled = true;
                simplePrinter.Enabled = false;
                gridControl1.DataSource = null;
                CreaColumnas();
                textSocioComercial.Text = "";
                textNombreCliente.Text = "";
                textUtilizadoPaciente.Text = "";
                gridLookDoctores.Text = "";
                dateFechaPedido.Text = "";
                gridLookSerie.Text = "";
                gridLookBodega.Text = "";
            }
        }






        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transa;
        string id_nuevo_vale = "1";
        int tipo_pago = 5;//pago al credito


        private void insertaVale()
        {
            DataTable DatoTemp = new DataTable();
            try
            {
                conexion.Open();
                transa = conexion.BeginTransaction();
                if (id_socioComercial != id_SocioComercialCompara & textSocioComercial.Text != "")
                {
                    string cadena2 = "UPDATE clientes SET clientes.referido_por=" + id_socioComercial + " WHERE clientes.codigo_cliente=" + id_temp_Cliente;
                    comando = new MySqlCommand(cadena2, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                }

                
                
                string cadena = "INSERT into header_doctos_inv(codigo_serie,tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, socio_comercial, estadoid, contado_credito,refer_documento,doctor) " +
                         "VALUES (" + serieID + "," + tipo_pago + ", '" + textNumeroDocVale + "', " + id_temp_Cliente + ", '" + dateFechaPedido.DateTime.ToString("yyyy-MM-dd") + "', " + /*totalVale*/"0" + ",0, " + /*totalVale*/"0" + ", " + clases.ClassVariables.id_usuario + ",'" + id_socioComercial + "',8," + /*radioGroup2.SelectedIndex*/"1" + ",'" + /*textDeposito.Text*/"" + "'," + gridLookDoctores.EditValue.ToString() + ");SELECT LAST_INSERT_ID();";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                id_nuevo_vale = comando.ExecuteScalar().ToString();
                
                cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                            "VALUES (" + id_temp_Cliente + ", " + id_nuevo_vale + "," + id_nuevo_vale + ", '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                transa.Commit();


                //ahora si insertamos el pedido
                RegistraPedido();

                

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

        string id_nuevo_pedido = "1";
        string valorreporte = "1";
        private void RegistraPedido()
        {
                string cadena = "SELECT * FROM header_doctos_inv INNER JOIN series_documentos ON header_doctos_inv.codigo_serie=series_documentos.codigo_serie WHERE header_doctos_inv.no_documento=" + textNoDocumento.Text + " AND series_documentos.codigo_serie=" + serieID;
                if (logicaorto.ExisteRegistro(cadena))
                    textNoDocumento.Text = (Convert.ToInt32(textNoDocumento.Text) + 1).ToString();
            //    conexion.Close();    
            //conexion.Open();
                transa = conexion.BeginTransaction();


                /*cadena = "INSERT into header_doctos_inv(codigo_serie,tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento, socio_comercial, descripcion, estadoid,contado_credito) " +
                            "VALUES (" + gridLookTipoDocumento.EditValue + ", " + gridLookTipoPago.EditValue + ", '" + textNoDocumento.Text + "', " + id_cliente + ", '" + dateFechaPedido.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', " + TotalPedido + ", " + TotalDescuento + ", " + TempTotalPedido + ", " + clases.ClassVariables.id_usuario + ", " + id_usuario_descuento + ",'" + id_socioComercial + "', '" + memoDescripcion.Text + "', 8," + radioGroup2.SelectedIndex + ");select last_insert_id();";
                 */
                    cadena = "INSERT into header_doctos_inv(codigo_serie,tipo_pago, no_documento, codigo_cliente, fecha, monto, descuento, monto_neto, usuario_creador, usuario_descuento, socio_comercial, descripcion, estadoid,contado_credito) " +
                        "VALUES (" + gridLookSerie.EditValue + ", " + tipo_pago + ", '" + textNoDocumento.Text + "', " + id_temp_Cliente + ", '" + dateFechaPedido.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', " + TotalPedido + ", " + 0 + ", " + TotalPedido + ", " + clases.ClassVariables.id_usuario + ", " + clases.ClassVariables.id_usuario + ",'" + id_socioComercial + "', '" + memoDescripcion.Text + "', 8," + "1" + ");select last_insert_id();";
                        //"VALUES (" + gridLookSerie.EditValue + ", " + tipo_pago + ", '" + textNoDocumento.Text + "', " + id_temp_Cliente + ", '" + dateFechaPedido.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', " + TotalPedido + ", " + "0" + ", " + TotalPedido + ", " + clases.ClassVariables.id_usuario + ", " + "" + ",'" + id_socioComercial + "', '" + memoDescripcion.Text + "', 8," + /*radioGroup2.SelectedIndex*/"1" + ");select last_insert_id();";
                
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                id_nuevo_pedido = comando.ExecuteScalar().ToString();
                valorreporte = textNoDocumento.Text;
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    cadena = "INSERT into detalle_doctos_inv(id_documento, cantidad_enviada, precio_unitario, precio_total, codigo_articulo, codigo_bodega) " +
                                "VALUES (" + id_nuevo_pedido + ", " + gridView1.GetRowCellValue(x, "CANTIDAD") + ", " + gridView1.GetRowCellValue(x, "PRECIO UNITARIO") + ", " + gridView1.GetRowCellValue(x, "SUB TOTAL") + ",'" + gridView1.GetRowCellValue(x, "CODIGO") + "', " + gridView1.GetRowCellValue(x, "IDBODEGA") + ")";
                    comando = new MySqlCommand(cadena, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                    cadena = "update bodegas SET  existencia_articulo = existencia_articulo -" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridView1.GetRowCellValue(x, "IDBODEGA") + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                    comando = new MySqlCommand(cadena, conexion);
                    comando.Transaction = transa;
                    comando.ExecuteNonQuery();
                }
                cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                            "VALUES (" + id_temp_Cliente + ", " + id_nuevo_vale + "," + id_nuevo_pedido + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                cadena = "update header_doctos_inv SET estadoid = 4 WHERE header_doctos_inv.id_documento=" + id_nuevo_vale;
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
                transa.Commit();
                clases.ClassMensajes.INSERTO(this);


                
                groupControl2.Enabled = false;
                groupControl3.Enabled = false;
                panelControl1.Enabled = false;
                sbAceptar.Enabled = false;
                simplePrinter.Enabled = true;
        }

        private void gridLookDoctores_TextChanged(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();
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

        private void gridLookBodega_TextChanged(object sender, EventArgs e)
        {
            dxValidationProvider2.Validate();
        }
    }
}