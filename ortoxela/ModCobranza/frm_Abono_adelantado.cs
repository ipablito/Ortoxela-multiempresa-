using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Controls;
using MySql.Data.MySqlClient;
using System.Globalization;

using DevExpress.XtraReports.UI;
namespace ortoxela.ModCobranza
{
    public partial class frm_Abono_adelantado : DevExpress.XtraEditors.XtraForm
    {
        public frm_Abono_adelantado()
        {
            InitializeComponent();
        }

        private void frm_Abono_Load(object sender, EventArgs e)
        {
            xtraTabPage2.PageEnabled = true;
            try
            {
                dateEdit1.DateTime = DateTime.Now;
                cargaClientes();
                CargaTipoDoc();
                CargaTipoPago();
                CreaColumnasGV();
                CargaBancos();
                //CargRecibos();
                gridControl1.Enabled = false;
            }
            catch { }

        }
        private void CargRecibos()
        {
            cadena = "SELECT r.id_recibos AS CODIGO,CAST(CONCAT(s.serie_documento,'[',r.no_recibo,']') AS CHAR) AS 'NO RECIBO' "+
"FROM recibos r LEFT JOIN  v_abonos_recibos p ON (r.id_recibos=p.id_docto_abono) " +
"INNER JOIN series_documentos s ON r.codigo_serie=s.codigo_serie "+
"WHERE r.codigo_cliente="+id_cliente+" AND (monto-COALESCE(abono,0))>0";

            gridLookSerieRecibo.Properties.DataSource = ortoxela.Tabla(cadena);
            gridLookSerieRecibo.Properties.DisplayMember = "NO RECIBO";
            gridLookSerieRecibo.Properties.ValueMember = "CODIGO";
            gridLookSerieRecibo.EditValue = null;
        }
        string cadena;
        classortoxela ortoxela = new classortoxela();
        private void CargaTipoPago()
        {
            cadena = "SELECT tp.tipo_pago AS CODIGO,tp.nombre_tipo_pago AS 'TIPO PAGO' FROM tipo_pago tp WHERE tp.estadoid=1";
            gridLookTipoPago.Properties.DataSource = ortoxela.Tabla(cadena);
            gridLookTipoPago.Properties.DisplayMember = "TIPO PAGO";
            gridLookTipoPago.Properties.ValueMember = "CODIGO";
            gridLookTipoPago.EditValue = null;
            //gridLookTipoPago.Properties.View.Columns["CODIGO"].Visible = false;
        }
        private void CargaBancos()
        {
            cadena = "SELECT bancos.id_banco AS CODIGO,bancos.nombre_banco AS NOMBRE FROM bancos WHERE bancos.estado=1";           
            gridLookBanco.Properties.DataSource = ortoxela.Tabla(cadena);
            gridLookBanco.Properties.DisplayMember = "NOMBRE";
            gridLookBanco.Properties.ValueMember = "CODIGO";
            gridLookBanco.EditValue = null;
            //gridLookTipoPago.Properties.View.Columns["CODIGO"].Visible = false;
        }
        private void CargaTipoDoc()
        {
            cadena = "SELECT sd.codigo_serie AS CODIGO, concat(td.nombre_documento, '[',sd.serie_documento,']') AS NOMBRE FROM tipos_documento td inner join series_documentos sd on td.codigo_tipo=sd.codigo_tipo WHERE td.estado_id=1 AND td.documento_cobro=1";            
            gridLookTipoDoc.Properties.DataSource = ortoxela.Tabla(cadena);
            gridLookTipoDoc.Properties.DisplayMember = "NOMBRE";
            gridLookTipoDoc.Properties.ValueMember = "CODIGO";
            gridLookTipoDoc.EditValue = null;
            //gridLookTipoDoc.Properties.View.Columns["CODIGO"].Visible = false;
        }
        private void cargaClientes()
        {
            cadena = "SELECT DISTINCT c.codigo_cliente AS CODIGO,c.nombre_cliente AS NOMBRE FROM relacion_pagos rp INNER JOIN clientes c ON rp.codigo_cliente=c.codigo_cliente where rp.saldo_factura>0";
            gridLookCliente.Properties.DataSource = ortoxela.Tabla(cadena);
            gridLookCliente.Properties.DisplayMember = "NOMBRE";
            gridLookCliente.Properties.ValueMember = "CODIGO";
            
        }
        private void LlenaFacturas()
        {
            cadena = "SELECT h.fecha AS 'FECHA',sd.serie_documento AS 'SERIE',h.id_documento,h.no_documento AS 'NO. FACTURA',h.monto_neto AS 'TOTAL',(monto_neto-COALESCE(TotAbono,0)) "+
"AS 'SALDO ACTUAL',0 AS 'ABONO',(monto_neto-COALESCE(TotAbono,0))AS 'SALDO' "+
"FROM header_doctos_inv h LEFT JOIN  v_abonos_factura f ON (h.id_documento=f.id_factura) "+
"inner join series_documentos sd on h.codigo_serie=sd.codigo_serie "+
"WHERE h.codigo_cliente="+id_cliente+" and h.estadoid=4 ANd h.contado_credito=1";
            gridControl1.DataSource = ortoxela.Tabla(cadena);
            gridView1.Columns["id_documento"].Visible = false;
            gridView1.Columns["NO. FACTURA"].OptionsColumn.ReadOnly = true;
            
            gridView1.Columns["TOTAL"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["SALDO ACTUAL"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["SALDO"].OptionsColumn.ReadOnly = true;

        }
        private void sbAceptar_Click(object sender, EventArgs e)
        {
           
        }
        double Total = 0;// esta variable tendra el total de recibo....
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Name == "colABONO")
            {
                if ((Convert.ToDouble(gridView1.GetFocusedRowCellValue("SALDO ACTUAL")) - Convert.ToDouble(e.Value)) >= 0)
                {
                    if (Convert.ToDouble(e.Value) <= Convert.ToDouble(labelCantidadRestante.Text.Replace("Q", "")))
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SALDO", (Convert.ToDouble(gridView1.GetFocusedRowCellValue("SALDO ACTUAL")) - Convert.ToDouble(e.Value)));
                        Total = 0;
                        for (int x = 0; x < gridView1.DataRowCount; x++)
                        {
                            if (Convert.ToDouble(gridView1.GetRowCellValue(x, "ABONO")) > 0)
                            {
                                Total = Total + Convert.ToDouble(gridView1.GetRowCellValue(x, "ABONO"));
                            }
                        }
                        labelCantidadRestante.Text = (Convert.ToDouble(textTotal.Text.Replace("Q", "")) - Total).ToString("C");
                    }
                    else Mensajes.Show(this,"Información","El valor no puede ser mayor a la cantidad restante",Properties.Resources.Advertencia64);
                }
            }
        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
            
        }
        private void Resumatoria()//calcula nuevamente la cantidad restante....
        {
            double resuma = 0;
            for (int x = 0; x<gridView1.DataRowCount;x++ )
            {
                resuma =resuma + Convert.ToDouble(gridView1.GetRowCellValue(x,"ABONO"));
            }
            labelCantidadRestante.Text = (Convert.ToDouble(textTotal.Text.Replace("Q", "")) - resuma).ToString("C");
        }
        private void gridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            ColumnView col = sender as ColumnView;
            //Resumatoria();
            if (Convert.ToDouble(col.GetRowCellValue(e.RowHandle, col.Columns["ABONO"])) > Convert.ToDouble(col.GetRowCellValue(e.RowHandle, col.Columns["SALDO ACTUAL"])))
            {
                e.Valid = false;
                col.SetColumnError(gridView1.Columns["ABONO"], "El Abono no puede se mayor al saldo actual");
            }
            //if (Convert.ToDouble(col.GetRowCellValue(e.RowHandle, col.Columns["ABONO"])) > Convert.ToDouble(labelCantidadRestante.Text.Replace("Q","")))
            //{
            //    e.Valid = false;
            //    col.SetColumnError(gridView1.Columns["ABONO"], "El Abono no puede ser mayor a la cantidad restante");
            //}
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                labelCantRestante.Text = Convert.ToDouble(textTotal.Text.Replace("Q","")).ToString("C");
            }
            catch
            { }

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (dxValidationEncabezado.Validate())
            {
                panelControl3.Enabled = true;
            }
            else
                clases.ClassMensajes.FaltanDatosEnCampos(this);

        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void CreaColumnasGV()
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("IDTIPOPAGO");
            dtTemp.Columns.Add("TIPO PAGO");
            dtTemp.Columns.Add("NO DOCUMENTO");
            dtTemp.Columns.Add("IDBANCO");
            dtTemp.Columns.Add("BANCO");
            dtTemp.Columns.Add("MONTO");
            dtTemp.Columns.Add("SALDO");
            gridControl2.DataSource = dtTemp;
            gridView4.Columns["IDTIPOPAGO"].Visible = false;
            gridView4.Columns["IDBANCO"].Visible = false;
            gridView4.Columns["TIPO PAGO"].OptionsColumn.ReadOnly = true;
            gridView4.Columns["NO DOCUMENTO"].OptionsColumn.ReadOnly = true;
            gridView4.Columns["BANCO"].OptionsColumn.ReadOnly = true;
            gridView4.Columns["MONTO"].OptionsColumn.ReadOnly = true;
            gridView4.Columns["SALDO"].OptionsColumn.ReadOnly = true;
        }
        private void agregaTipoPagoaGV(string id_bancos)
        { 
            
                gridView4.AddNewRow();
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "IDTIPOPAGO",gridLookTipoPago.EditValue.ToString());
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "TIPO PAGO", gridLookTipoPago.Text.ToString());
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "NO DOCUMENTO", textNoDoc.Text);
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "IDBANCO", id_bancos);
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "BANCO", gridLookBanco.Text);
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "MONTO", Convert.ToDouble(textMonto.Text).ToString("C"));
                gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "SALDO", (Convert.ToDouble(labelCantRestante.Text.Replace("Q", "")) - Convert.ToDouble(textMonto.Text)).ToString("C"));
                gridView4.UpdateCurrentRow();
                try
                {
                    gridLookTipoPago.EditValue = null;
                }
                catch { }
                labelCantRestante.Text = (Convert.ToDouble(labelCantRestante.Text.Replace("Q", "")) - Convert.ToDouble(textMonto.Text)).ToString("C");
                textMonto.Text=textNoDoc.Text=gridLookBanco.Text= "";
                gridLookTipoPago.Focus();
        }

        private void gridLookTipoPago_EditValueChanged(object sender, EventArgs e)
        {
            bool banderaExiste = false;

            for (int xy = 0; xy < gridView4.DataRowCount; xy++)
            {
                if (gridView4.GetRowCellValue(xy, "IDTIPOPAGO").ToString() == gridLookTipoPago.EditValue.ToString())
                    banderaExiste = true;
            }
            if (banderaExiste)
            {
                gridLookTipoPago.EditValue = 0;
            }        
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
           // agregaTipoPagoaGV();
        }

        private void SbAgregaTP_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridLookTipoPago.EditValue.ToString() == "1" | gridLookTipoPago.EditValue.ToString() == "4" | gridLookTipoPago.EditValue.ToString() == "5")
                {
                    if (dxValidationTipoPago.Validate() & (Convert.ToDouble(textMonto.Text) <= Convert.ToDouble(labelCantRestante.Text.Replace("Q", ""))))
                    {
                        if(Convert.ToDouble(textMonto.Text)>=0)
                        agregaTipoPagoaGV("0");
                        else
                            Mensajes.Show(this, "Informacion", "El monto tiene que ser positivo.", Properties.Resources.Advertencia48);
                    }
                    else
                        Mensajes.Show(this, "Informacion", "Faltad Datos, o el monto es demsiado grande.", Properties.Resources.Advertencia48);
                }
                else
                {

                    if (dxValidationTipoPago.Validate() & (Convert.ToDouble(textMonto.Text) <= Convert.ToDouble(labelCantRestante.Text.Replace("Q", ""))))
                    {
                        if (dxValidationBancos.Validate())
                        {
                            if (Convert.ToDouble(textMonto.Text) >= 0)
                            agregaTipoPagoaGV(gridLookBanco.EditValue.ToString());
                            else
                                Mensajes.Show(this, "Informacion", "El monto tiene que ser positivo.", Properties.Resources.Advertencia48);
                        }
                        else
                        {
                            clases.ClassMensajes.FaltanDatosEnCampos(this);
                            textNoDoc.Focus();
                        }
                    }
                    else
                        Mensajes.Show(this, "Informacion", "Faltad Datos, o el monto es demsiado grande.", Properties.Resources.Advertencia48);
                }
            }
            catch
            {
                Mensajes.Show(this,"Error de datos","Verifique los datos porfavor",Properties.Resources.error_64);
            }
        }

        private void gridLookCliente_EditValueChanged(object sender, EventArgs e)
        {
            labelNombreCliente.Text = "[Codigo:" + gridLookCliente.EditValue + "]- " + gridLookCliente.Text;
            xtraTabPage2.PageEnabled = false;
            cadena = "SELECT COALESCE(SUM(TotSaldo),0)AS TotalSaldo FROM (SELECT (MIN(rp.saldo_factura))AS TotSaldo  FROM relacion_pagos rp INNER JOIN clientes c ON rp.codigo_cliente=c.codigo_cliente INNER JOIN header_doctos_inv h ON h.id_documento=rp.id_factura WHERE h.estadoid=4 AND c.codigo_cliente=" + gridLookCliente.EditValue + " GROUP BY h.id_documento)tabla";
            lbTotalSaldo.Text = Convert.ToDouble(logicaorto.Tabla(cadena).Rows[0][0].ToString()).ToString("C");
        }
        classortoxela logicaorto = new classortoxela();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dxValidationEncabezado.Validate() && Convert.ToDouble(labelCantRestante.Text.Replace("Q", ""))==0)
            {
                xtraTabPage2.PageEnabled = true;
                labelTotalDoc.Text = "TOTAL " +gridLookTipoDoc.Text.ToUpper() +": "+Convert.ToDouble(textTotal.Text).ToString("C");
                labelCantidadRestante.Text = Convert.ToDouble(textTotal.Text).ToString("C");
                xtraTabControl1.SelectedTabPageIndex=1;
                LlenaFacturas();
                cadena = "SELECT (MAX(recibos.no_recibo)+1)AS 'NORECIBO' FROM recibos";
                textNoRecibo.Text = logicaorto.Tabla(cadena).Rows[0]["NORECIBO"].ToString();
                lbSaldoTotal.Text = lbTotalSaldo.Text;
                lbRestoSaldo.Text = (Convert.ToDouble(lbTotalSaldo.Text.Replace("Q",""))-Convert.ToDouble(textTotal.Text)).ToString("C");
            }
            else
                clases.ClassMensajes.FaltanDatosEnCampos(this);
        }

        private void gridLookTipoDoc_EditValueChanged(object sender, EventArgs e)
        {
            //xtraTabPage2.PageEnabled = false;
        }

        private void gridView1_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
           // Resumatoria();
        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transaccion;
        private void InsertaDatos()
        {
            try
            {
                conexion.Open();
                transaccion = conexion.BeginTransaction();

                //cadena = "INSERT into recibos(no_recibo, codigo_serie, codigo_cliente, fecha_creacion, no_pedido, tipo_pago, monto, usuario_creador, estadoid)" +
                //        "VALUES	('"+textNoRecibo.Text+"', '"+gridLookTipoDoc.EditValue+"', '"+gridLookCliente.EditValue+"', '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', '0', '0', '"+textTotal.Text+"', '"+clases.ClassVariables.id_usuario+"', '4');";
                //comando = new MySqlCommand(cadena, conexion);
                //comando.Transaction = transaccion;
                //comando.ExecuteNonQuery();
                //cadena = "";
                //for (int x = 0; x < gridView4.DataRowCount;x++)
                //{
                //    cadena += "INSERT into detalle_recibos (no_recibo, codigo_serie, tipo_pago, monto,fecha_operacion, id_banco, no_documento, observaciones, activo)" +
                //                "  VALUES	('" + textNoRecibo.Text + "', '"+gridLookTipoDoc.EditValue+"', '" + gridView4.GetRowCellValue(x, "IDTIPOPAGO").ToString() + "', '" + gridView4.GetRowCellValue(x, "MONTO").ToString().Replace("Q", "").Replace(",","") + "', '" + dateEdit1.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + gridView4.GetRowCellValue(x, "IDBANCO").ToString() + "', '" + gridView4.GetRowCellValue(x, "NO DOCUMENTO").ToString() + "', '" + memoEdit1.Text + "', '1');";

                //}
                //comando = new MySqlCommand(cadena, conexion);
                //comando.Transaction = transaccion;
                //comando.ExecuteNonQuery();
                cadena = "";
                string NoTransaccion = DateTime.Now.ToString("yyyyMMddHHmmss");
                for (int y = 0; y < gridView1.DataRowCount; y++)
                {
                    if (Convert.ToDouble(gridView1.GetRowCellValue(y, "ABONO")) > 0)
                    {
                        cadena += "insert into relacion_pagos (no_transaccion, codigo_cliente, id_factura, saldo_factura, fecha_operacion, id_docto_abono, monto_abono, comentarios, activo)" +
                                    "values	('" + NoTransaccion + "', '" + id_cliente + "', '" + gridView1.GetRowCellValue(y, "id_documento") + "', '" + gridView1.GetRowCellValue(y, "SALDO").ToString().Replace(",","") + "', '" + dateEdit1.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + gridLookSerieRecibo.EditValue + "', '" + gridView1.GetRowCellValue(y, "ABONO").ToString().Replace(",","") + "', '" + memoEdit1.Text + "', '1');";
                        if (Convert.ToDouble(gridView1.GetRowCellValue(y, "SALDO").ToString()) == 0)
                        { 
                            cadena+="UPDATE header_doctos_inv h SET h.estadoid=10 WHERE h.id_documento="+gridView1.GetRowCellValue(y, "id_documento")+";";
                        }
                    }
                }
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transaccion;
                comando.ExecuteNonQuery();
                transaccion.Commit();
                clases.ClassMensajes.INSERTO(this);
                Sbimprimir.Enabled = true;
                simpleButton4.Enabled = false;
                groupControl2.Enabled = false;
            }
            catch
            {
                transaccion.Rollback();
                clases.ClassMensajes.NoINSERTO(this);
        }
            finally
            {
                conexion.Close();
                
            }
        }
        
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (dxValidationGuardar.Validate()) //& Convert.ToDouble(labelCantidadRestante.Text.Replace("Q", "")) == 0)
            {
                //cadena = "SELECT * FROM recibos WHERE recibos.codigo_serie=24 AND recibos.no_recibo="+textNoRecibo.Text;
                //if (logicaorto.ExisteRegistro(cadena) == false)
                    InsertaDatos();
                //else
                  //  Mensajes.Show(this,"Informacion","El Documento ya existe en el sistema.",Properties.Resources.Advertencia64);
            }
            else
                Mensajes.Show(this,"Informacion","Faltan datos ó la cantidad restante no ha llegado a 0",Properties.Resources.Advertencia64);
        }

        private void gridView4_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                labelCantRestante.Text = (Convert.ToDouble(labelCantRestante.Text.Replace("Q", "")) + Convert.ToDouble(gridView4.GetFocusedRowCellValue("MONTO").ToString().Replace("Q",""))).ToString("C");
                gridView4.DeleteSelectedRows();
            }
            catch
            { }
        }
        string abono, cancelacion, otro;//solo me sirven para poder generar el recibo
        private void Sbimprimir_Click(object sender, EventArgs e)
        {
            try
            {

                //if (radioGroup1.SelectedIndex == 0)
                //{
                //    abono = "X";
                //    cancelacion = "";
                //    otro = "";
                //}
                //else
                //    if (radioGroup1.SelectedIndex == 1)
                //    {
                        abono = "";
                        cancelacion = "X";
                        otro = "";
                    //}
                    //else
                    //    if (radioGroup1.SelectedIndex == 2)
                    //    {
                    //        abono = "";
                    //        cancelacion = "";
                    //        otro = "X";
                    //    }
                //conexion.Open();
                //transa = conexion.BeginTransaction();

                //string tempValor = textPor.Text.Replace(",", "");
                //cadena = "INSERT into recibos(no_recibo, codigo_serie, codigo_cliente, fecha_creacion, no_pedido, tipo_pago, monto, usuario_creador, estadoid) " +
                //        "VALUES (" + textNoRecibo.Text + ", 3, " + id_cliente + ", '" + Convert.ToDateTime(dateFechaRecibo.EditValue).ToString("yyyy-MM-dd HH:mm:ss") + "', " + id_nuevo_vale + ", 1, " + tempValor.Replace("Q", "") + ", " + clases.ClassVariables.id_usuario + ", 4);";
                //comando = new MySqlCommand(cadena, conexion);
                //comando.Transaction = transa;
                //comando.ExecuteNonQuery();
                //cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                //        "VALUES (" + id_cliente + ", " + id_nuevo_vale + "," + textNoRecibo.Text + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                //comando = new MySqlCommand(cadena, conexion);
                //comando.Transaction = transa;
                //comando.ExecuteNonQuery();
                //transa.Commit();
                        string facturass="";//recoge todas las facturas q se pagaron
                        for (int i = 0; i < gridView1.DataRowCount;i++)
                        {
                            if (Convert.ToDouble(gridView1.GetRowCellValue(i, "MONTO")) > 0)
                                facturass += gridView1.GetRowCellValue(i, "NO. DOCUMENTO").ToString()+", ";
                        }
                        Pedido.ReciboCaja.DataSetReciboCaja dataset = new Pedido.ReciboCaja.DataSetReciboCaja();
                dataset.Tables["recibos"].Rows.Add(textNoRecibo.Text, dateEdit1.DateTime, 1,textTotal.Text.Replace("Q", ""), clases.ClassVariables.id_usuario, 1, facturass, "", "", textTotal.Text, abono, cancelacion, otro,logicaorto.enletras(textTotal.Text.Replace("Q", "")), gridLookCliente.Text, gridLookCliente.EditValue, "");
                Pedido.ReciboCaja.XtraReportReciboCaja reporte = new Pedido.ReciboCaja.XtraReportReciboCaja();
                reporte.DataSource = dataset;
                reporte.DataMember = dataset.Tables["recibos"].TableName;
                reporte.RequestParameters = false;
                reporte.ShowPreviewDialog();
                xtraTabPage1.PageEnabled = true;

                Sbimprimir.Enabled = false;
            }
            catch
            {
                //clases.ClassMensajes.NoINSERTO(this);
                //transa.Rollback();
            }
            finally
            {
                //conexion.Close();
            }
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }
        int id_cliente;
        private void gridLookSerieRecibo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                cadena = "SELECT no_recibo,codigo_cliente,monto AS monto_recibo,(monto-COALESCE(abono,0)) AS saldo "+
"FROM recibos r LEFT JOIN  v_abonos_recibos p ON (r.id_recibos=p.id_docto_abono) "+
"WHERE r.codigo_cliente="+id_cliente+" and r.id_recibos=" + gridLookSerieRecibo.EditValue;
                DataTable datosCliente = logicaorto.Tabla(cadena);
                //id_cliente = int.Parse(datosCliente.Rows[0]["codigo_cliente"].ToString());
               // labelNombreCliente.Text = "[Codigo:" + id_cliente + "]- " + datosCliente.Rows[0]["nombre_cliente"].ToString();
                labelTotalDoc.Text = "TOTAL RECIBO: " + Convert.ToDouble(datosCliente.Rows[0]["monto_recibo"]).ToString("C");
                labelCantidadRestante.Text = Convert.ToDouble(datosCliente.Rows[0]["saldo"]).ToString("C");
                textTotal.Text = Convert.ToDouble(datosCliente.Rows[0]["saldo"]).ToString();
                lbRestoSaldo.Text = (Convert.ToDouble(lbTotalSaldo.Text.Replace("Q", "")) - Convert.ToDouble(textTotal.Text)).ToString("C");
               // xtraTabControl1.SelectedTabPageIndex = 1;
               // LlenaFacturas();
                //cadena = "SELECT (MAX(recibos.no_recibo)+1)AS 'NORECIBO' FROM recibos";
                //cadena = "SELECT COALESCE(SUM(TotSaldo),0)AS TotalSaldo FROM (SELECT (MIN(rp.saldo_factura))AS TotSaldo  FROM relacion_pagos rp INNER JOIN clientes c ON rp.codigo_cliente=c.codigo_cliente INNER JOIN header_doctos_inv h ON h.id_documento=rp.id_factura WHERE h.estadoid=4 AND c.codigo_cliente=" + id_cliente + " GROUP BY h.id_documento)tabla";
                //lbTotalSaldo.Text = Convert.ToDouble(logicaorto.Tabla(cadena).Rows[0][0].ToString()).ToString("C");
                //textNoRecibo.Text = gridLookSerieRecibo.Text;
                //lbSaldoTotal.Text = lbTotalSaldo.Text;
                //lbRestoSaldo.Text = (Convert.ToDouble(lbTotalSaldo.Text.Replace("Q", "")) - Convert.ToDouble(textTotal.Text)).ToString("C");
                gridControl1.Enabled = true;
            }
            catch
            { }
        }

        private void textRecibimosDe_KeyPress(object sender, KeyPressEventArgs e)
        {
            cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE SOCIO COMERCIAL',clientes.nit AS 'NIT',clientes.telefono_celular AS 'TELEFONO CELULAR' FROM clientes where estadoid<>2";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                id_cliente = int.Parse(clases.ClassVariables.id_busca);
                DataTable tempCliente = new DataTable();
                cadena = "SELECT clientes.codigo_cliente AS CODIGO,clientes.nombre_cliente AS 'NOMBRE CLIENTE',clientes.nit,clientes.referido_por,clientes.nombre_paciente,clientes.telefono_casa,contacto FROM clientes WHERE clientes.codigo_cliente=" + id_cliente;
                tempCliente = logicaorto.Tabla(cadena);
                textRecibimosDe.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();
                CargRecibos();
                gridLookSerieRecibo.Enabled = true;
                //id_socioComercial = tempCliente.Rows[0]["referido_por"].ToString();
                //textUtilizadoPaciente.Text = tempCliente.Rows[0]["nombre_paciente"].ToString();
                //textTelefonoCliente.Text = tempCliente.Rows[0]["telefono_casa"].ToString();
                //textDoctorPedido.Text = tempCliente.Rows[0]["contacto"].ToString();
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
            try
            {                
                labelNombreCliente.Text = "[Codigo:" + id_cliente + "]- " + textRecibimosDe.Text;
                LlenaFacturas();
                //cadena = "SELECT (MAX(recibos.no_recibo)+1)AS 'NORECIBO' FROM recibos";
                cadena = "SELECT COALESCE(SUM(SALDO),0)AS TotalSaldo FROM(SELECT (monto_neto-COALESCE(TotAbono,0))AS 'SALDO' "+
"FROM header_doctos_inv h LEFT JOIN  v_abonos_factura f ON (h.id_documento=f.id_factura) "+
"INNER JOIN series_documentos sd ON h.codigo_serie=sd.codigo_serie "+
"WHERE h.codigo_cliente="+id_cliente+" AND h.estadoid=4 AND h.contado_credito=1)TablaSaldo";
                lbTotalSaldo.Text = Convert.ToDouble(logicaorto.Tabla(cadena).Rows[0][0].ToString()).ToString("C");
                //textNoRecibo.Text = gridLookSerieRecibo.Text;
                lbSaldoTotal.Text = lbTotalSaldo.Text;
                lbRestoSaldo.Text = (Convert.ToDouble(lbTotalSaldo.Text.Replace("Q", "")) - Convert.ToDouble(textTotal.Text)).ToString("C");
            }
            catch
            { }
            e.KeyChar = Convert.ToChar(13);
        }
    }
}