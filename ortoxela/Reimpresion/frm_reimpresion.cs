using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using DevExpress.XtraReports.UI;
using MySql.Data.MySqlClient;

namespace ortoxela.Reimpresion
{
    public partial class frm_reimpresion : DevExpress.XtraEditors.XtraForm
    {
        public frm_reimpresion()
        {
            InitializeComponent();
        }
        string cadena;
        classortoxela ortoxela = new classortoxela();
        private void CargaDatos()
        {
            cadena = "SELECT tipos_documento.codigo_tipo AS CODIGO,tipos_documento.nombre_documento AS'TIPO DOCUMENTO' FROM tipos_documento WHERE tipos_documento.codigo_tipo<>2 AND tipos_documento.codigo_tipo<>4";
            gridLookSerieVale.Properties.DataSource = ortoxela.Tabla(cadena);
            gridLookSerieVale.Properties.DisplayMember = "TIPO DOCUMENTO";
            gridLookSerieVale.Properties.ValueMember = "CODIGO";
            //gridLookSerieVale.Properties.View.Columns["CODIGO"].Visible = false;
            gridLookSerieVale.Properties.NullText = "Seleccione un documento";
        }
        private void frm_reimpresion_Load(object sender, EventArgs e)
        {
            CargaDatos();
            radioGroup1.SelectedIndex = 1;
            radioGroup1.SelectedIndex = 0;
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable td = new DataTable();
            gridControl1.DataSource = td;
            if (radioGroup1.SelectedIndex == 0)
            {
                gridLookSerieVale.Enabled = false;

            }
            else
            {
                gridLookSerieVale.Enabled = true;
            }
        }

        private void llenagridview()
        {
            DataTable dt = new DataTable();
            gridControl1.DataSource = dt;
            gridView1.Columns.Clear();
            cadena = "SELECT h.id_documento,h.no_documento AS 'NO DOCUMENTO',CONCAT(t.nombre_documento,' [',s.serie_documento,']')AS 'SERIE DOCUMENTO' ,h.fecha AS 'FECHA',c.nombre_cliente AS 'NOMBRE CLIENTE',h.monto_neto AS 'MONTO' FROM header_doctos_inv h INNER JOIN series_documentos s ON h.codigo_serie=s.codigo_serie INNER JOIN tipos_documento t ON t.codigo_tipo=s.codigo_tipo LEFT JOIN clientes c ON h.codigo_cliente=c.codigo_cliente WHERE t.codigo_tipo=" + gridLookSerieVale.EditValue + " AND (h.fecha BETWEEN '" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + "' AND '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + "') ORDER BY h.fecha DESC ";
            gridControl1.DataSource = ortoxela.Tabla(cadena);
            gridView1.Columns["id_documento"].Visible = false;
            gridView1.Columns["NO DOCUMENTO"].Width = 60;
            gridView1.Columns["SERIE DOCUMENTO"].Width = 75;
            gridView1.Columns["FECHA"].Width = 75;
            gridView1.Columns["MONTO"].Width = 60;
        }
        private void llenaRecibos()
        {

            DataTable dt = new DataTable();
            gridControl1.DataSource = dt;
            gridView1.Columns.Clear();
            cadena = "SELECT r.no_recibo AS 'NO RECIBO',r.fecha_creacion AS 'FECHA',c.nombre_cliente AS 'NOMBRE CLIENTE',r.monto AS 'MONTO' FROM recibos r INNER JOIN clientes c ON r.codigo_cliente=c.codigo_cliente WHERE (r.fecha_creacion BETWEEN '" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + "' AND '" + dateEdit2.DateTime.ToString("yyyy-MM-dd") + "') ORDER BY r.fecha_creacion DESC";
            gridControl1.DataSource = ortoxela.Tabla(cadena);
            gridView1.Columns["NO RECIBO"].Width = 60;
            gridView1.Columns["FECHA"].Width = 60;
        }
        private void llenagridview1()
        {
            DataTable dt = new DataTable();
            gridControl1.DataSource = dt;
            gridView1.Columns.Clear();
            cadena = "SELECT h.id_documento,h.no_documento AS 'NO DOCUMENTO',CONCAT(t.nombre_documento,' [',s.serie_documento,']')AS 'SERIE DOCUMENTO' ,h.fecha AS 'FECHA',c.nombre_cliente AS 'NOMBRE CLIENTE',h.monto_neto AS 'MONTO' FROM header_doctos_inv h INNER JOIN series_documentos s ON h.codigo_serie=s.codigo_serie INNER JOIN tipos_documento t ON t.codigo_tipo=s.codigo_tipo LEFT JOIN clientes c ON h.codigo_cliente=c.codigo_cliente WHERE t.codigo_tipo=" + gridLookSerieVale.EditValue + " AND h.no_documento='"+textEdit1.Text+"' ORDER BY h.fecha DESC";
            gridControl1.DataSource = ortoxela.Tabla(cadena);
            gridView1.Columns["id_documento"].Visible = false;
            gridView1.Columns["NO DOCUMENTO"].Width = 60;
            gridView1.Columns["SERIE DOCUMENTO"].Width = 75;
            gridView1.Columns["FECHA"].Width = 75;
            gridView1.Columns["MONTO"].Width = 60;
        }
        private void llenaRecibos1()
        {

            DataTable dt = new DataTable();
            gridControl1.DataSource = dt;
            gridView1.Columns.Clear();
            cadena = "SELECT r.no_recibo AS 'NO RECIBO',r.fecha_creacion AS 'FECHA',c.nombre_cliente AS 'NOMBRE CLIENTE',r.monto AS 'MONTO' FROM recibos r INNER JOIN clientes c ON r.codigo_cliente=c.codigo_cliente WHERE r.no_recibo='"+textEdit1.Text+"' ORDER BY r.fecha_creacion DESC";
            gridControl1.DataSource = ortoxela.Tabla(cadena);
            gridView1.Columns["NO RECIBO"].Width = 60;
            gridView1.Columns["FECHA"].Width = 60;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (textEdit1.Text != "")
            {
                if (radioGroup1.SelectedIndex == 0)
                {
                    llenaRecibos1();
                }
                else
                {
                    if (dxValidationProvider2.Validate())
                        llenagridview1();
                    else clases.ClassMensajes.FaltanDatosEnCampos(this);
                }
            }
            else
                if(textEdit1.Text=="")
                {
            if (dxValidationProvider1.Validate())
            {
                if (radioGroup1.SelectedIndex == 0)
                {
                    llenaRecibos();
                }
                else
                {
                    if (dxValidationProvider2.Validate())
                    llenagridview();
                    else clases.ClassMensajes.FaltanDatosEnCampos(this);
                }
            }
                }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.DataRowCount > 0)
                {
                    if (radioGroup1.SelectedIndex == 0)
                    {
                        DataTable tablas = new DataTable();
                        tablas = ortoxela.Tabla("SELECT * FROM recibos WHERE recibos.no_recibo=" + gridView1.GetFocusedRowCellValue("NO RECIBO"));
                        Pedido.ReciboCaja.DataSetReciboCaja dataset = new Pedido.ReciboCaja.DataSetReciboCaja();
                        dataset.Tables["recibos"].Rows.Add(gridView1.GetFocusedRowCellValue("NO RECIBO"), gridView1.GetFocusedRowCellValue("FECHA"), 1, gridView1.GetFocusedRowCellValue("MONTO"), clases.ClassVariables.id_usuario, 1, " ", " ", " ", gridView1.GetFocusedRowCellValue("MONTO"), "", "X", "", ortoxela.enletras(gridView1.GetFocusedRowCellValue("MONTO").ToString()), gridView1.GetFocusedRowCellValue("NOMBRE CLIENTE"), tablas.Rows[0]["codigo_cliente"], gridView1.GetFocusedRowCellValue("MONTO"));
                        Pedido.ReciboCaja.XtraReportReciboCaja reporte = new Pedido.ReciboCaja.XtraReportReciboCaja();
                        reporte.DataSource = dataset;
                        reporte.DataMember = dataset.Tables["recibos"].TableName;
                        reporte.RequestParameters = false;
                        reporte.ShowPreviewDialog();

                    }
                    else
                    {
                        if (Convert.ToInt32(gridLookSerieVale.EditValue) == 3)
                        {
                            MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                            DataTable nt = new DataTable();
                            ad.Fill(nt);

                            Pedido.Vale.XtraReportVale reporte = new Pedido.Vale.XtraReportVale();
                            reporte.Parameters["ID"].Value = gridView1.GetFocusedRowCellValue("id_documento");

                            reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                            reporte.Parameters["Direccion"].Value = "Direccion: "+nt.Rows[0]["direccion"].ToString();
                            reporte.Parameters["telefono1"].Value = "Telefono: "+nt.Rows[0]["telefono"].ToString();
                            reporte.Parameters["telefono2"].Value = "Telefono auxiliar: "+nt.Rows[0]["telefono2"].ToString();
                            reporte.Parameters["correo"].Value ="Correo: "+ nt.Rows[0]["correo"].ToString();
                            reporte.RequestParameters = false;
                            reporte.ShowPreviewDialog();
                        }
                        else
                            if (Convert.ToInt32(gridLookSerieVale.EditValue) == 5)
                            {
                                MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                                DataTable nt = new DataTable();
                                ad.Fill(nt);

                                Pedido.EnvioDetallado.XtraReportEnvioDetallado reporte = new Pedido.EnvioDetallado.XtraReportEnvioDetallado();
                                reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                                reporte.Parameters["telefonos"].Value = "Telefono: "+nt.Rows[0]["telefono"].ToString()+", Telefono auxiliar: "+nt.Rows[0]["telefono2"].ToString()+".";
                                //reporte.Parameters["ID"].Value = gridView1.GetFocusedRowCellValue("id_documento");
                                reporte.Parameters["ID"].Value = gridView1.GetFocusedRowCellValue("NO DOCUMENTO");
                                reporte.RequestParameters = false;
                                reporte.ShowPreviewDialog();
                            }
                            else
                                if (Convert.ToInt32(gridLookSerieVale.EditValue) == 1)
                                {
                                    Pedido.Factura.XtraReportFactura reporte = new Pedido.Factura.XtraReportFactura();
                                    reporte.Parameters["ID"].Value = gridView1.GetFocusedRowCellValue("id_documento");
                                    reporte.Parameters["LETRAS"].Value = ortoxela.enletras(gridView1.GetFocusedRowCellValue("MONTO").ToString());
                                    reporte.RequestParameters = false;
                                    reporte.ShowPreviewDialog();
                                }
                                else
                                {
                                    MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                                    DataTable nt = new DataTable();
                                    ad.Fill(nt);

                                    Compra.PrintIngresoProd.XtraReportIngresoProd reporte = new Compra.PrintIngresoProd.XtraReportIngresoProd();
                                    reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                                    reporte.Parameters["ID"].Value = gridView1.GetFocusedRowCellValue("id_documento");
                                    reporte.RequestParameters = false;
                                    reporte.ShowPreviewDialog();
                                }
                    }
                }
            }
            catch
            { }
        }
    }
}