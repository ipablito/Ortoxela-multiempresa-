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
namespace ortoxela.ReciboCredito
{
    public partial class frm_reciboCredito : DevExpress.XtraEditors.XtraForm
    {
        public frm_reciboCredito()
        {
            InitializeComponent();
        }
        string cadena;
        classortoxela ortoxela = new classortoxela();
        classortoxela logicaorto = new classortoxela();
        private void llenaFacturas()
        {
            cadena = "SELECT id_documento,CAST(CONCAT(nombre_documento,' [',serie_documento,']',' No ',no_documento)AS CHAR CHARACTER SET utf8) AS Documento FROM header_doctos_inv h JOIN v_tipos_documentos t ON(h.codigo_serie=t.codigo_serie) WHERE t.codigo_tipo =1 AND estadoid=4 AND contado_credito=1 ORDER BY h.codigo_serie,no_documento";
            gridLookSerieVale.Properties.DataSource = ortoxela.Tabla(cadena);
            gridLookSerieVale.Properties.DisplayMember = "Documento";           
            gridLookSerieVale.Properties.ValueMember="id_documento";            
            // gridLookSerieVale.Properties.View.Columns["id_documento"].Visible=false;
            gridLookSerieVale.Properties.NullText="SELECCIONE UNA FACTURA";
        }
        private void frm_reciboCredito_Load(object sender, EventArgs e)
        {
            try
            {
                cadena = "SELECT codigo_serie CODIGO,CONCAT(tipos_documento.nombre_documento,' - ',serie_documento) AS DOCUMENTO FROM series_documentos INNER JOIN tipos_documento ON series_documentos.codigo_tipo = tipos_documento.codigo_tipo WHERE tipos_documento.codigo_tipo=2";
                gridLookSerieRecibo.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookSerieRecibo.Properties.DisplayMember = "DOCUMENTO";
                gridLookSerieRecibo.Properties.ValueMember = "CODIGO";
                gridLookSerieRecibo.EditValue = 1;
            }
            catch
            { }
            llenaFacturas();
            simpleButton1.PerformClick();
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.SelectedIndex == 0)
                gridLookSerieVale.Enabled = false;
            else
                gridLookSerieVale.Enabled = true;
        }

        DataTable datatable = new DataTable();
        bool FacturaDirecta;
        string id_vale,id_cliente;
        private void gridLookSerieVale_EditValueChanged(object sender, EventArgs e)
        {

            cadena="SELECT relacion_venta.id_vale FROM relacion_venta WHERE relacion_venta.id_documento="+gridLookSerieVale.EditValue;
            datatable=ortoxela.Tabla(cadena);
            if (datatable.Rows.Count > 0)
            {
                FacturaDirecta = false;
                id_vale = datatable.Rows[0][0].ToString();
            }
            else
            {
                FacturaDirecta = true;
            }
            cadena = "SELECT h.codigo_cliente,h.id_documento,h.monto_neto,c.nombre_cliente FROM  header_doctos_inv h INNER JOIN clientes c ON h.codigo_cliente=c.codigo_cliente WHERE h.id_documento=" + gridLookSerieVale.EditValue;
            datatable = ortoxela.Tabla(cadena);
            try
            {
                id_cliente = datatable.Rows[0]["codigo_cliente"].ToString();
                textRecibimosDe.Text=datatable.Rows[0]["nombre_cliente"].ToString();
                textPor.Text = datatable.Rows[0]["monto_neto"].ToString();
                textCantidadDe.Text = ortoxela.enletras(textPor.Text.Replace("Q", "")) + " QUETZALES EXACTOS";
                textEfectivo.Text =textValor.Text=textPor.Text;
                dateFechaRecibo.DateTime = DateTime.Now;                
                textCodigoCobrador.Text = clases.ClassVariables.id_usuario;
                cadena = "SELECT (MAX(recibos.no_recibo)+1)AS 'NORECIBO' FROM recibos";
                textNoRecibo.Text = ortoxela.Tabla(cadena).Rows[0]["NORECIBO"].ToString();

            }
            catch
            { 
            
            }
        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transa;
        string abono, cancelacion, otro;//solo me sirven para poder generar el recibo
        private void sbPrintReciboCaja_Click(object sender, EventArgs e)
        {
            if (dxValidationRecibo.Validate())
            {
                cadena = "SELECT * FROM recibos WHERE codigo_serie='"+gridLookSerieRecibo.EditValue+"' and recibos.no_recibo=" + textNoRecibo.Text;
                if (ortoxela.ExisteRegistro(cadena) == false)
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
                        if (radioGroup2.SelectedIndex == 0)
                        {
                            //cadena = "INSERT into clientes(nombre_cliente) " +
                            //    "VALUES ('" + textRecibimosDe + "')";
                            //id_cliente = ortoxela.nuevoid(cadena);

                            cadena = "INSERT into recibos(no_recibo, codigo_serie, codigo_cliente, fecha_creacion, tipo_pago, monto, usuario_creador, estadoid) " +
                                 "VALUES (" + textNoRecibo.Text + ", "+gridLookSerieRecibo.EditValue+", " + id_cliente + ", '" + Convert.ToDateTime(dateFechaRecibo.EditValue).ToString("yyyy-MM-dd HH:mm:ss") + "', 1, " + tempValor.Replace("Q", "") + ", " + clases.ClassVariables.id_usuario + ", 4);";
                            comando = new MySqlCommand(cadena, conexion);
                            comando.Transaction = transa;
                            comando.ExecuteNonQuery();                            
                            transa.Commit();
                            Pedido.ReciboCaja.DataSetReciboCaja dataset = new Pedido.ReciboCaja.DataSetReciboCaja();
                            dataset.Tables["recibos"].Rows.Add(textNoRecibo.Text, dateFechaRecibo.DateTime, 1, textPor.Text.Replace("Q", ""), clases.ClassVariables.id_usuario, 1, textFacturas.Text, textCheque.Text, textBanco.Text, textValor.Text, abono, cancelacion, otro, textCantidadDe.Text, textRecibimosDe.Text, id_cliente, textEfectivo.Text);
                            Pedido.ReciboCaja.XtraReportReciboCaja reporte = new Pedido.ReciboCaja.XtraReportReciboCaja();
                            reporte.DataSource = dataset;
                            reporte.DataMember = dataset.Tables["recibos"].TableName;
                            reporte.RequestParameters = false;
                            reporte.ShowPreviewDialog();
                            sbPrintReciboCaja.Enabled = false;
                        }
                        else
                        {
                            if (FacturaDirecta)
                            {
                                cadena = "INSERT into recibos(no_recibo, codigo_serie, codigo_cliente, fecha_creacion, tipo_pago, monto, usuario_creador, estadoid) " +
                                        "VALUES (" + textNoRecibo.Text + ", 3, " + id_cliente + ", '" + Convert.ToDateTime(dateFechaRecibo.EditValue).ToString("yyyy-MM-dd HH:mm:ss") + "', 1, " + tempValor.Replace("Q", "") + ", " + clases.ClassVariables.id_usuario + ", 4);";
                                comando = new MySqlCommand(cadena, conexion);
                                comando.Transaction = transa;
                                comando.ExecuteNonQuery();
                                cadena = "INSERT into relacion_venta(codigo_cliente, id_documento, fecha_creacion, usuario_creador, estadoid,id_factura) " +
                                        "VALUES (" + id_cliente + "," + textNoRecibo.Text + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4,"+gridLookSerieVale.EditValue+")";
                                comando = new MySqlCommand(cadena, conexion);
                                comando.Transaction = transa;
                                comando.ExecuteNonQuery();
                                cadena = "UPDATE header_doctos_inv SET header_doctos_inv.estadoid=5 WHERE header_doctos_inv.id_documento=" + gridLookSerieVale.EditValue;
                                comando = new MySqlCommand(cadena, conexion);
                                comando.Transaction = transa;
                                comando.ExecuteNonQuery();            
                                transa.Commit();
                                Pedido.ReciboCaja.DataSetReciboCaja dataset = new Pedido.ReciboCaja.DataSetReciboCaja();
                                dataset.Tables["recibos"].Rows.Add(textNoRecibo.Text, dateFechaRecibo.DateTime, 1, textPor.Text.Replace("Q", ""), clases.ClassVariables.id_usuario, 1, textFacturas.Text, textCheque.Text, textBanco.Text, textValor.Text, abono, cancelacion, otro, textCantidadDe.Text, textRecibimosDe.Text, id_cliente, textEfectivo.Text);
                                Pedido.ReciboCaja.XtraReportReciboCaja reporte = new Pedido.ReciboCaja.XtraReportReciboCaja();
                                reporte.DataSource = dataset;
                                reporte.DataMember = dataset.Tables["recibos"].TableName;
                                reporte.RequestParameters = false;
                                reporte.ShowPreviewDialog();
                                sbPrintReciboCaja.Enabled = false;
                            }
                            else
                            {
                                cadena = "INSERT into recibos(no_recibo, codigo_serie, codigo_cliente, fecha_creacion, no_pedido, tipo_pago, monto, usuario_creador, estadoid) " +
                                "VALUES (" + textNoRecibo.Text + ", 3, " + id_cliente + ", '" + Convert.ToDateTime(dateFechaRecibo.EditValue).ToString("yyyy-MM-dd HH:mm:ss") + "', " + id_vale + ", 1, " + tempValor.Replace("Q", "") + ", " + clases.ClassVariables.id_usuario + ", 4);";
                                comando = new MySqlCommand(cadena, conexion);
                                comando.Transaction = transa;
                                comando.ExecuteNonQuery();
                                cadena = "INSERT into relacion_venta(codigo_cliente, id_vale, id_documento, fecha_creacion, usuario_creador, estadoid) " +
                                        "VALUES (" + id_cliente + ", " + id_vale + "," + textNoRecibo.Text + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + clases.ClassVariables.id_usuario + ",4)";
                                comando = new MySqlCommand(cadena, conexion);
                                comando.Transaction = transa;
                                comando.ExecuteNonQuery();
                                cadena = "UPDATE header_doctos_inv SET header_doctos_inv.estadoid=5 WHERE header_doctos_inv.id_documento=" + gridLookSerieVale.EditValue;
                                comando = new MySqlCommand(cadena, conexion);
                                comando.Transaction = transa;
                                comando.ExecuteNonQuery();
                                cadena = "UPDATE header_doctos_inv SET header_doctos_inv.estadoid=5 WHERE header_doctos_inv.id_documento=" + gridLookSerieVale.EditValue;
                                comando = new MySqlCommand(cadena, conexion);
                                comando.Transaction = transa;
                                comando.ExecuteNonQuery();            
                                transa.Commit();
                                Pedido.ReciboCaja.DataSetReciboCaja dataset = new Pedido.ReciboCaja.DataSetReciboCaja();
                                dataset.Tables["recibos"].Rows.Add(textNoRecibo.Text, dateFechaRecibo.DateTime, 1, textPor.Text.Replace("Q", ""), clases.ClassVariables.id_usuario, 1, textFacturas.Text, textCheque.Text, textBanco.Text, textValor.Text, abono, cancelacion, otro, textCantidadDe.Text, textRecibimosDe.Text, id_cliente, textEfectivo.Text);
                                Pedido.ReciboCaja.XtraReportReciboCaja reporte = new Pedido.ReciboCaja.XtraReportReciboCaja();
                                reporte.DataSource = dataset;
                                reporte.DataMember = dataset.Tables["recibos"].TableName;
                                reporte.RequestParameters = false;
                                reporte.ShowPreviewDialog();
                                sbPrintReciboCaja.Enabled = false;
                            }
                        }
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
        DevExpress.XtraEditors.TextEdit cajatexto=new TextEdit();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            foreach(Control contro in groupControl3.Controls)
            {
                if (contro is DevExpress.XtraEditors.TextEdit)
                {
                    cajatexto = (TextEdit)contro;
                    cajatexto.Text = "";
                }   
            }
            radioGroup2.SelectedIndex = 0;
            dateFechaRecibo.DateTime = DateTime.Now;
            textCodigoCobrador.Text = clases.ClassVariables.id_usuario;
            string serie = gridLookSerieRecibo.EditValue.ToString();
            if (serie == "")
                serie = "3";
            cadena = "SELECT (recibos.no_recibo+1) AS 'NODOC' FROM recibos INNER JOIN series_documentos ON recibos.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie=" + serie + " ORDER BY recibos.no_recibo DESC LIMIT 1";
            textNoRecibo.Text = ortoxela.Tabla(cadena).Rows[0][0].ToString();
            sbPrintReciboCaja.Enabled = true;
            llenaFacturas();
        }

        private void textPor_EditValueChanged(object sender, EventArgs e)
        {
            textCantidadDe.Text = ortoxela.enletras(textPor.Text.Replace("Q", ""));
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

        private void textRecibimosDe_KeyPress(object sender, KeyPressEventArgs e)
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
                textRecibimosDe.Text = tempCliente.Rows[0]["NOMBRE CLIENTE"].ToString();                
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
            e.KeyChar = Convert.ToChar(13);
        }
    }
}