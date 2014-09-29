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

namespace ortoxela.TrasladoBodega
{
    public partial class TrasladoBodega : DevExpress.XtraEditors.XtraForm
    {
        public TrasladoBodega()
        {
            InitializeComponent();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
        DataTable tempTabla = new DataTable();
        string id_articulo;
        classortoxela logicaxela = new classortoxela();
        string ssql;
        private void CargaDatos()
        {
            try
            {
                /* ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid<>2"; */
                /* jramirez 2013.07.24 */
                ssql = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario;
                gridLookBodegaOrigen.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookBodegaOrigen.Properties.DisplayMember = "NOMBRE";
                gridLookBodegaOrigen.Properties.ValueMember = "CODIGO";
                gridLookBodegaOrigen.Text = "";
                gridLookBodegaOrigen.EditValue = 0;



                //mostrar el numero de traslado q sera
                //cadena = "SELECT (recibos.no_recibo+1) AS 'NODOC' FROM recibos INNER JOIN series_documentos ON recibos.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie=" + serie + " ORDER BY recibos.no_recibo DESC LIMIT 1";
                //textNoRecibo.Text = ortoxela.Tabla(cadena).Rows[0][0].ToString();

                //
                //string cadena = "SELECT   no_traslado_bodega FROM traslado_bodega_header ORDER BY no_traslado_bodega DESC  LIMIT 1";
                //if (logicaorto.Tabla(cadena).Rows.Count != 0)
                //{
                //    int valortemporal = Convert.ToInt16(logicaorto.Tabla(cadena).Rows[0][0]);
                //    label_numerotraslado.Text="Traslado numero: "+(valortemporal + 1).ToString();
                //}
                //else
                //    label_numerotraslado.Text = "0";
                //

                
            }
            catch
            { }
            try
            {
                /* ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where estadoid<>2"; */
                /* jramirez 2013.07.24 */
                ssql = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario;
                gridLookBodegaDestino.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookBodegaDestino.Properties.DisplayMember = "NOMBRE";
                gridLookBodegaDestino.Properties.ValueMember = "CODIGO";
                gridLookBodegaDestino.Text = "";
                gridLookBodegaDestino.EditValue = 0;
                
            }
            catch
            { }
            try
            {
                /* ssql = "SELECT codigo_serie CODIGO,serie_documento as SERIE FROM series_documentos INNER JOIn tipos_documento on series_documentos.codigo_tipo = tipos_documento.codigo_tipo where series_documentos.codigo_tipo=4"; */
                /* jramirez 2013.07.24 */
                ssql = "SELECT distinct codigo_serie AS CODIGO, serie_documento AS SERIE FROM v_bodegas_series_usuarios  WHERE codigo_tipo=4 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_bodega = " + gridLookBodegaOrigen.EditValue.ToString();
                gridLookTipoDocumento.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookTipoDocumento.Properties.DisplayMember = "SERIE";
                gridLookTipoDocumento.Properties.ValueMember = "CODIGO";
                gridLookTipoDocumento.Text = "";
                gridLookTipoDocumento.EditValue = 5;
                
            }
            catch
            { }

        }
        private void TrasladoBodega_Load(object sender, EventArgs e)
        {
            CargaDatos();
            CreaColumnas();
            dateEdit1.DateTime = DateTime.Now;
        }
        private void CreaColumnas()
        {
            DataTable temporal = new DataTable();
            temporal.Columns.Add("CODIGO");
            temporal.Columns.Add("DESCRIPCION");
            temporal.Columns.Add("CANTIDAD");            
            gridControl1.DataSource = temporal;
            gridView1.Columns["DESCRIPCION"].Width = 200;
        }

        private void textCodigoArt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (dxValidationProvider3.Validate())
                {
                    ssql = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND articulos.codigo_articulo='" + textCodigoArt.Text + "' AND bodegas.codigo_bodega=" + gridLookBodegaOrigen.EditValue;                    
                    tempTabla = logicaxela.Tabla(ssql);
                    if (tempTabla.Rows.Count > 0)
                    {
                        id_articulo = tempTabla.Rows[0]["CODIGO"].ToString();
                        textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString();
                        ExistenciaProd = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);
                        textCantidadArt.Focus();
                    }
                }
            }
        }
        private void Limpia()
        {
            gridLookBodegaOrigen.EditValue = 0;
            gridLookBodegaDestino.EditValue = 0;            
            gridLookTipoDocumento.EditValue = 0;            
            textCodigoArt.Text = "";
            textCantidadArt.Text = "";
            textNombreArti.Text = "";            
            memoEdit1.Text = "";            
            CreaColumnas();
            gridLookBodegaOrigen.Focus();
            simplePrinter.Enabled = false;
            sbAceptar.Enabled = true;
            groupControl1.Enabled = true;
            groupControl2.Enabled = true;
            panelControl1.Enabled = true;
        }
        int ExistenciaProd;
        private void textNombreArti_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dxValidationProvider3.Validate())
            {
                ssql = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.precio_venta AS 'PRECIO VENTA',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND bodegas.codigo_bodega=" + gridLookBodegaOrigen.EditValue;
                    clases.ClassVariables.cadenabusca = ssql;
                    Form nuevo = new Buscador.Buscador();
                    nuevo.ShowDialog();
                    if (Buscador.Buscador.SeleccionSiNo)
                    {
                        id_articulo = clases.ClassVariables.id_busca;
                        ssql = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA' FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo where articulos.estadoid<>2 and articulos.codigo_articulo='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodegaOrigen.EditValue; ;
                        tempTabla = logicaxela.Tabla(ssql);
                        if (tempTabla.Rows.Count > 0)
                        {
                            textCodigoArt.Text = tempTabla.Rows[0]["CODIGO"].ToString();
                            textNombreArti.Text = tempTabla.Rows[0]["NOMBRE ARTICULO"].ToString();
                            ExistenciaProd = Convert.ToInt32(tempTabla.Rows[0]["EXISTENCIA"]);
                            textCantidadArt.Focus();
                        }
                    }
                }
            }
            bool banderaRepetido;
            string cadena;
            classortoxela logicaorto = new classortoxela();
            private void sbAgregaArt_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    if (dxValidationProvider1.Validate())
                    {
                        DataTable TempoPadre = new DataTable();
                        /* cadena = "SELECT articulos.compuesto FROM articulos WHERE articulos.codigo_articulo='" + id_articulo + "'"; 
                        jramirez 2013.07.04 
                        */
                        cadena = "select f_es_compuesto('" + id_articulo + "') AS compuesto;";
                        string compuesto = logicaorto.Tabla(cadena).Rows[0]["compuesto"].ToString();
                        if (Convert.ToBoolean(logicaorto.Tabla(cadena).Rows[0]["compuesto"]))
                        {

                            /* cadena = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'NOMBRE ARTICULO',articulos.numero_serie AS 'No SERIE',bodegas.existencia_articulo AS 'EXISTENCIA',articulos.precio_venta,articulos.costo FROM articulos INNER JOIN bodegas ON bodegas.codigo_articulo=articulos.codigo_articulo WHERE articulos.estadoid<>2 AND articulos.codigo_padre='" + id_articulo + "' AND bodegas.codigo_bodega=" + gridLookBodegaOrigen.EditValue; */
                            cadena = "CALL sp_devuelve_sistema ('" + id_articulo + "'," + gridLookBodegaOrigen.EditValue + ")";
                            TempoPadre = logicaorto.Tabla(cadena);
                            int ExistenciaHijo;
                            int ExistenciaFija;
                            for (int x = 0; x < TempoPadre.Rows.Count; x++)
                            {
                                banderaRepetido = true;
                                for (int y = 0; y < gridView1.DataRowCount; y++)
                                {
                                    if (gridView1.GetRowCellValue(y, "CODIGO").ToString() == TempoPadre.Rows[x]["CODIGO"].ToString() )
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
                                        gridView1.AddNewRow();                                        
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", TempoPadre.Rows[x]["CODIGO"]);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", TempoPadre.Rows[x]["NOMBRE ARTICULO"]);
                                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", ExistenciaFija);                                        
                                        gridView1.UpdateCurrentRow();
                                        
                                    }

                                }
                                else
                                    clases.ClassMensajes.ProdYaExisteEnListado(this);
                            }
                            gridView1.AddNewRow();
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", textCodigoArt.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", textNombreArti.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", 0);
                            gridView1.UpdateCurrentRow();
                            textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = "";
                            textCodigoArt.Focus();
                        }
                        else
                        {
                            if (Convert.ToInt32(textCantidadArt.Text) <= ExistenciaProd)
                            {
                                banderaRepetido = true;
                                for (int x = 0; x < gridView1.DataRowCount; x++)
                                {
                                    if (gridView1.GetRowCellValue(x, "CODIGO").ToString() == textCodigoArt.Text)
                                        banderaRepetido = false;
                                }
                                if (banderaRepetido)
                                {
                                    gridView1.AddNewRow();                                    
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", textCodigoArt.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", textNombreArti.Text);
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", Convert.ToInt32(textCantidadArt.Text));                                                                        
                                    gridView1.UpdateCurrentRow();                                    
                                    textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = "";
                                    textCodigoArt.Focus();
                                }
                                else
                                    clases.ClassMensajes.ProdYaExisteEnListado(this);
                            }
                            else
                                clases.ClassMensajes.NoHayExistenciaProd(this);
                        }






                        if (Convert.ToInt32(textCantidadArt.Text) <= ExistenciaProd)
                        {
                            banderaRepetido = true;
                            for (int x = 0; x < gridView1.DataRowCount;x++)
                            {
                            if (gridView1.GetRowCellValue(x, "CODIGO").ToString() == textCodigoArt.Text)
                                banderaRepetido = false;
                        }
                        if (banderaRepetido)
                        {
                            gridView1.AddNewRow();
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CODIGO", textCodigoArt.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DESCRIPCION", textNombreArti.Text);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CANTIDAD", textCantidadArt.Text);
                            gridView1.UpdateCurrentRow();
                            textCodigoArt.Text = textNombreArti.Text = textCantidadArt.Text = "";
                            textCodigoArt.Focus();
                        }
                        else
                        clases.ClassMensajes.ProdYaExisteEnListado(this);
                    }
                    else
                        clases.ClassMensajes.NoHayExistenciaProd(this);
                }
                else
                    clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
            catch
            {

            }
                Cursor.Current = Cursors.Default;
        }

        private void sbnuevo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿DESEA BORRAR LO DATOS?", "INFORMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //mostrar el numero de traslado q sera
                //cadena = "SELECT (recibos.no_recibo+1) AS 'NODOC' FROM recibos INNER JOIN series_documentos ON recibos.codigo_serie=series_documentos.codigo_serie WHERE series_documentos.codigo_serie=" + serie + " ORDER BY recibos.no_recibo DESC LIMIT 1";
                //textNoRecibo.Text = ortoxela.Tabla(cadena).Rows[0][0].ToString();

                //
                //string cadena = "SELECT   no_traslado_bodega FROM traslado_bodega_header ORDER BY no_traslado_bodega DESC  LIMIT 1";
                //if (logicaorto.Tabla(cadena).Rows.Count != 0)
                //{
                //    int valortemporal = Convert.ToInt16(logicaorto.Tabla(cadena).Rows[0][0]);
                //    label_numerotraslado.Text = "Traslado numero: " + (valortemporal + 1).ToString();
                //}
                //else
                //    label_numerotraslado.Text = "0";
                //
                Limpia();
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {                
                gridView1.DeleteSelectedRows();
                gridView1.UpdateCurrentRow();               
            }
            catch
            { }
        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
        MySqlTransaction transac;
        string id_nuevoIngreso;
        private void registraIngreso()
        {

            try
            {


                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    ssql = "SELECT * FROM bodegas WHERE bodegas.codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "' AND bodegas.codigo_bodega="+gridLookBodegaDestino.EditValue;
                    if (logicaxela.ExisteRegistro(ssql))
                    {
                       
                    }
                    else
                    {
                        ssql = "INSERT into bodegas(codigo_bodega, codigo_articulo, existencia_articulo) " +
                              "VALUES (" + gridLookBodegaDestino.EditValue + ", '" + gridView1.GetRowCellValue(x, "CODIGO") + "', 0)";
                        logicaxela.variosservios(ssql);
                    }
                }

                conexion.Open();
                transac = conexion.BeginTransaction();
                comando.Transaction = transac;
                ssql = "INSERT into traslado_bodega_header(bodega_origen, bodega_destino, descripcion, usuario_creador, fecha_creacion, codigo_serie,no_doc_traslado,no_documento) "+
                        "VALUES (" + gridLookBodegaOrigen.EditValue + ", " + gridLookBodegaDestino.EditValue + ", '" + memoEdit1.Text + "', " + clases.ClassVariables.id_usuario + ", '" + dateEdit1.DateTime.ToString("yyyy-MM-dd") + "', " + gridLookTipoDocumento.EditValue + ",'" + textNoDocumento.Text + "'," + textEdit_nodoc.Text + ");SELECT LAST_INSERT_ID();";                
                comando = new MySqlCommand(ssql, conexion);
                comando.Transaction = transac;
                id_nuevoIngreso = comando.ExecuteScalar().ToString();
                for (int x = 0; x < gridView1.DataRowCount; x++)
                {
                    ssql = "INSERT into traslado_bodega_detail(no_traslado_bodega, codigo_articulo, cantidad) "+
                            "VALUES (" + id_nuevoIngreso + ", '" + gridView1.GetRowCellValue(x, "CODIGO") + "', " + gridView1.GetRowCellValue(x, "CANTIDAD") + ");";                    
                    comando = new MySqlCommand(ssql, conexion);
                    comando.Transaction = transac;
                    comando.ExecuteNonQuery();
                    ssql = "update bodegas SET  existencia_articulo = existencia_articulo +" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridLookBodegaDestino.EditValue + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                    comando = new MySqlCommand(ssql, conexion);
                    comando.Transaction = transac;
                    comando.ExecuteNonQuery();
                    ssql = "update bodegas SET  existencia_articulo = existencia_articulo -" + gridView1.GetRowCellValue(x, "CANTIDAD") + " WHERE codigo_bodega=" + gridLookBodegaOrigen.EditValue + " and codigo_articulo='" + gridView1.GetRowCellValue(x, "CODIGO") + "'";
                    comando = new MySqlCommand(ssql, conexion);
                    comando.Transaction = transac;
                    comando.ExecuteNonQuery();
                }
                transac.Commit();
                simplePrinter.Enabled = true;
                sbAceptar.Enabled = false;
                groupControl1.Enabled = false;
                groupControl2.Enabled = false;
                panelControl1.Enabled = false;
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
            if (dxValidationProvider2.Validate() & gridView1.DataRowCount > 0)
            {
                if ((gridLookTipoDocumento.Text != "") && (gridLookTipoDocumento.Text != "[Vacío]"))
                    registraIngreso();
                else
                    MessageBox.Show("Seleccione una serie de Traslado", "", MessageBoxButtons.OK);
            }
            else
                clases.ClassMensajes.FaltanDatosEnCampos(this);
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simplePrinter_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from empresa where idEmpresa=" + clases.ClassVariables.idEmpresa.ToString(), clases.ClassVariables.ConexionMaster);
                DataTable nt = new DataTable();
                ad.Fill(nt);

                PrintTraslado.XtraReportTraslado reporte = new PrintTraslado.XtraReportTraslado();
                reporte.Parameters["nombreEmpresa"].Value = nt.Rows[0]["nombre"].ToString();
                reporte.Parameters["ID"].Value = id_nuevoIngreso;
                reporte.RequestParameters = false;
                reporte.ShowPreviewDialog();
            }
            catch
            {

            }
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            Form hijo = new Series.SerieDoc();
            hijo.WindowState = System.Windows.Forms.FormWindowState.Normal;
            hijo.ShowDialog();
            DataTable dt = new DataTable();
            try
            {
                ssql = "SELECT codigo_serie CODIGO,serie_documento AS 'SERIE DOCUMENTO' FROM series_documentos INNER JOIN tipos_documento ON series_documentos.codigo_tipo = tipos_documento.codigo_tipo WHERE tipos_documento.codigo_tipo=4";
                gridLookTipoDocumento.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookTipoDocumento.Properties.DisplayMember = "SERIE DOCUMENTO";
                gridLookTipoDocumento.Properties.ValueMember = "CODIGO";
                gridLookTipoDocumento.EditValue = clases.ClassVariables.idnuevo;
            }
            catch
            { }
                        

        }

        private void textNombreArti_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridLookBodegaOrigen_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void gridLookTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void gridLookTipoDocumento_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cadena = "SELECT   no_traslado_bodega,no_documento FROM traslado_bodega_header WHERE (bodega_destino ="+gridLookBodegaDestino.EditValue+") AND (codigo_serie="+gridLookTipoDocumento.EditValue+") ORDER BY no_traslado_bodega DESC LIMIT 1;";
                if (logicaorto.Tabla(cadena).Rows.Count != 0)
                {
                    int valortemporal = Convert.ToInt16(logicaorto.Tabla(cadena).Rows[0][0]);
                    valortemporal = Convert.ToInt16(logicaorto.Tabla(cadena).Rows[0][1]);
                    label_nodoc2.Text = "Traslado a la bodega: " + gridLookBodegaDestino.Text + ", Serie: " + gridLookTipoDocumento.Text;
                    textEdit_nodoc.Text = (valortemporal + 1).ToString();
                }
                else
                {
                    label_nodoc2.Text = "Traslado a la bodega: " + gridLookBodegaDestino.Text + ", Serie: " + gridLookTipoDocumento.Text;
                    textEdit_nodoc.Text = "1";
                }
            }
            catch { }
            
        }

        private void gridLookBodegaDestino_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                ssql = "SELECT distinct codigo_serie AS CODIGO, serie_documento AS SERIE FROM v_bodegas_series_usuarios  WHERE codigo_tipo=4 AND userid=" + clases.ClassVariables.id_usuario + " and codigo_bodega = " + gridLookBodegaDestino.EditValue.ToString();
                gridLookTipoDocumento.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookTipoDocumento.Properties.DisplayMember = "SERIE";
                gridLookTipoDocumento.Properties.ValueMember = "CODIGO";
                gridLookTipoDocumento.Text = "";
                gridLookTipoDocumento.EditValue = 5;
            }
            catch { }
        }
    }
}