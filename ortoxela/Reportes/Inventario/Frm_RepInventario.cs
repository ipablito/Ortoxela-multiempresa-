using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
namespace ortoxela.Reportes.Inventario
{
    public partial class Frm_RepInventario : DevExpress.XtraEditors.XtraForm
    {
        public Frm_RepInventario()
        {
            InitializeComponent();
        }


        string Bodega_origen_int = "0";
        string Bodega_origen_int_nombre = "";
        /* TRASLADOS DE BODEGA */       
        private void simpleButton1_Click(object sender, EventArgs e)
        {                
                this.Cursor = Cursors.WaitCursor;
                if ((FechaInicio.EditValue == null) || (FechaFin.EditValue == null))
                {
                    MessageBox.Show("Faltan Fechas", "Error");
                    this.Cursor = Cursors.Default;
                    return;
                }
                cargarbodegas();
                Int32 Bodega_destino_int;                
                string consulta = "";
                //Bodega_origen_int = "";
                //Bodega_origen_int = bodegas.SelectedValue.ToString();
                //Bodega_origen_int = Int32.Parse(listBox_bodegas.SelectedIndex.ToString());
                Bodega_destino_int = Int32.Parse(BodegaDestino.SelectedValue.ToString());       
                //consulta = "CALL sp_traslados_bodegas('" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00','" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59'," + Bodega_origen_int +/* ","+ Bodega_destino_int +*/"); ";

                consulta = "SELECT" +
          " `h`.`no_traslado_bodega` AS `no_traslado_bodega`," +
          " `h`.`bodega_origen`      AS `bodega_origen`," +
          " `bh`.`nombre_bodega`     AS `nombre_origen`," +
          " `h`.`bodega_destino`     AS `bodega_destino`," +
          " `b2`.`nombre_bodega`     AS `nombre_destino`," +
          " `h`.`descripcion`        AS `descripcion`," +
          " `h`.`usuario_creador`    AS `usuario_creador`," +
          "  `h`.`fecha_creacion`     AS `fecha`," +
          " `h`.`codigo_serie`       AS `codigo_serie`," +
          " `t`.`nombre_documento`   AS `nombre_documento`," +
          " `h`.`no_doc_traslado`    AS `no_doc_traslado`," +
          " `d`.`codigo_articulo`    AS `codigo_articulo`," +
          " `d`.`cantidad`           AS `cantidad`," +
          " a.descripcion AS nombre_articulo" +
        " FROM `traslado_bodega_header` `h`" +
              " JOIN `traslado_bodega_detail` `d`" +
            " ON ((`h`.`no_traslado_bodega` = `d`.`no_traslado_bodega`))" +
            " JOIN articulos a" +
            " ON (d.codigo_articulo=a.codigo_articulo)" +
             " JOIN `v_tipos_documentos` `t`" +
               " ON ((`h`.`codigo_serie` = `t`.`codigo_serie`))" +
            " JOIN `bodegas_header` `bh`" +
              " ON ((`h`.`bodega_origen` = `bh`.`codigo_bodega`))" +
           " JOIN `bodegas_header` `b2`" +
             " ON ((`h`.`bodega_destino` = `b2`.`codigo_bodega`))"+
            " WHERE h.bodega_origen IN ("+Bodega_origen_int+")	AND h.fecha_creacion BETWEEN '"+FechaInicio.DateTime.ToString("yyyy-MM-dd") +" 00:00:00'"+
         " AND '"+FechaFin.DateTime.ToString("yyyy-MM-dd")+" 23:59:59';";

            consulta = consulta.Replace("\t", "");
                
                MySqlDataAdapter adaptadori = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);

                DataSet dataseti = new DataSet();
                
                adaptadori.Fill(dataseti, "v_traslados");
                XtraReport_Traslados reportei = new XtraReport_Traslados();


                    reportei.DataSource = dataseti;
                    reportei.DataMember = dataseti.Tables["v_traslados"].TableName;
                    reportei.Parameters["Fecha_inicial"].Value = FechaInicio.EditValue;
                    reportei.Parameters["Fecha_final"].Value = FechaFin.EditValue;
                    //reportei.Parameters["Texto_bodega"].Value = bodegas.Text.ToString();
                    reportei.Parameters["Texto_bodega"].Value = Bodega_origen_int_nombre;
                    //reportei.Parameters["codigo_bodegai"].Value = Bodega_origen_int;
                    reportei.Parameters["codigo_bodegaf"].Value = Bodega_destino_int;
                    reportei.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                    reportei.RequestParameters = false;
                    reportei.ShowPreview();
                 
                this.Cursor = Cursors.Default;

        }
        /* TOMA INVENTARIO EXISTENCIA */
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            Int32 bodega1 = 0;
            Int32 bodega2 = 100;
            string botittle = "Todas";
            string consulta = "";

            cargarbodegas();

            //verificar si lo quiere detallado o no
            if (checkBox_detallado1.Checked == true)
            {
                consulta = "SELECT codigo_articulo, articulo, Ult_compra, Ult_venta, Ult_precio, CONCAT('En bodega: ', nombre_bodega) AS nombre_bodega, categoria, codigo_categoria, existencia_articulo, codigo_bodega  " +
                                       " FROM v_inventario where codigo_bodega in(" + Bodega_origen_int + ")";
            }
            else
            {
                consulta = "SELECT codigo_articulo, articulo, Ult_compra, Ult_venta, Ult_precio, categoria, codigo_categoria, existencia_articulo, codigo_bodega  " +
                                       " FROM v_inventario where codigo_bodega in(" + Bodega_origen_int + ") GROUP BY codigo_articulo ";
            }

                if (comboBoxCategorias.SelectedValue.ToString() != "0")
                {
                    consulta = consulta + "  and codigo_categoria = " + comboBoxCategorias.SelectedValue;
                }
                else
                {
                    if ((textEdit2.Text.Trim() != "") || (textEdit3.Text.Trim() != ""))
                    {
                        if ((textEdit2.Text.Trim() != "") && (textEdit3.Text.Trim() == ""))
                        {
                            consulta = consulta + " and  codigo_articulo like '%" + textEdit2.Text + "%'";
                        }
                        else
                        {
                            if ((textEdit2.Text.Trim() == "") && (textEdit3.Text.Trim() != ""))
                            {
                                consulta = consulta + " and codigo_articulo like '%" + textEdit3.Text + "%'";
                            }
                            else
                            {
                                consulta = consulta + " and codigo_articulo between '" + textEdit2.Text + "' and '" + textEdit3.Text + "' ";
                            }
                        }

                    }
                }


                MySqlDataAdapter adaptador1 = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
                DataSet_Inventario dataset = new DataSet_Inventario();
                adaptador1.Fill(dataset, "v_inventario");
            
            //comprobar que el dataset no este vacio para no devuelva basura
                if (dataset.v_inventario.Rows.Count==0)
                {
                    MessageBox.Show("Este reporte no contiene informacion");
                }
                else
                {
                    XtraReport_InventarioExistencia reportee = new XtraReport_InventarioExistencia();
                    reportee.DataSource = dataset;
                    reportee.DataMember = dataset.Tables["v_inventario"].TableName;
                    reportee.Parameters["bodega"].Value = Bodega_origen_int_nombre;
                    reportee.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                    reportee.RequestParameters = false;
                    reportee.ShowPreviewDialog();
                }
            this.Cursor = Cursors.Default;
        }

        /* KARDEX */
        classortoxela ortoxela = new classortoxela();
        private void simpleButton4_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            cargarbodegas();
            Int32 codigo_bodega = 1;
            string co_bo = "", botittle="" ;
            Int16 cat1 = 0, cat2 = 10000;
            string consulta = "SELECT codigo_articulo, Articulo, cantidad, costo_sin_iva, costo_iva, codigo_bodega, nombre_bodega, fecha, Tipo_Pago, nombre_proveedor, nombre_cliente, dias_credito, "+
                             "signo, tipo_docto, no_documento, refer_documento, bodega_destino, codigo_categoria, categoria "+
                            "FROM v_kardex  where 1=1 ";

            if (Bodega_origen_int!="0")
            {
                co_bo = " and codigo_bodega in(" + Bodega_origen_int + ") ";
                consulta = consulta + co_bo;
                //codigo_bodega = Int32.Parse(bodegas.SelectedValue.ToString());
                //botittle = bodegas.Text;

                if (comboBoxCategorias.SelectedValue.ToString() != "0")
                {
                    consulta = consulta + "  and codigo_categoria = " + comboBoxCategorias.SelectedValue;
                    cat1 = Convert.ToInt16(comboBoxCategorias.SelectedValue.ToString());
                    cat2 = cat1;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una Bodega para este Reporte.", "Reporte Kardex");
                return;
            };

            if ((textEdit1.Text.Trim() != "") || (textEdit4.Text.Trim() != ""))
            {
                if ((textEdit1.Text.Trim() != "") && (textEdit4.Text.Trim() == ""))
                {
                    consulta = consulta + " and codigo_articulo like '%" + textEdit1.Text + "%'";
                }
                else
                {
                    if ((textEdit1.Text.Trim() == "") && (textEdit4.Text.Trim() != ""))
                    {
                        consulta = consulta + " and codigo_articulo like '%" + textEdit4.Text + "%'";
                    }
                    else
                    {
                        consulta = consulta + " and codigo_articulo between '" + textEdit1.Text + "' and '" + textEdit4.Text + "' ";
                    }
                }

            }
            string fechaInicialx = "2011-12-01";
            
            if ((FechaInicio.EditValue != null) && (FechaFin.EditValue != null))
            {
                fechaInicialx = FechaInicio.DateTime.ToString("yyyy-MM-dd");
                consulta = consulta + " and fecha between '" + FechaInicio.DateTime.ToString("yyyy-MM-dd") + " 00:00:00' ";
                consulta = consulta + " and '" + FechaFin.DateTime.ToString("yyyy-MM-dd") + " 23:59:59' ";
            }
            

            MySqlDataAdapter adaptador = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
            DataSet_Inventario dataset = new DataSet_Inventario();
            adaptador.Fill(dataset, "v_kardex");

           
                XtraReport_Kardex reporteP = new XtraReport_Kardex();
                reporteP.DataSource = dataset;
                reporteP.DataMember = dataset.Tables["v_kardex"].TableName;
                //reporteP.Parameters["codigo_bodega"].Value = codigo_bodega;
                reporteP.Parameters["bodega"].Value = Bodega_origen_int_nombre;
                reporteP.Parameters["Fecha_inicio"].Value = Convert.ToDateTime(fechaInicialx);
                reporteP.Parameters["Fecha_fin"].Value = FechaFin.EditValue;
                reporteP.Parameters["categoria1"].Value = cat1;
                reporteP.Parameters["categoria2"].Value = cat2;
                reporteP.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                reporteP.RequestParameters = false;
                reporteP.ShowPreviewDialog();
          

            
            
                this.Cursor = Cursors.Default;
        }

        classortoxela logicaxela = new classortoxela();
        private void Frm_RepInventario_Load(object sender, EventArgs e)
        {
            this.Text = "Reportes de Inventario - " + clases.ClassVariables.nombreEmpresa; 
            try
            {
                string ssql = "SELECT distinct codigo_bodega, nombre_bodega FROM v_bodegas_series_usuarios  WHERE estadoid_bodega<>2 AND userid=" + clases.ClassVariables.id_usuario;
                    // "  Select 0 as codigo_bodega, 'Todas' as nombre_bodega from dual " +
                            // " union all  "+
                            // " select codigo_bodega,nombre_bodega from bodegas_header where estadoid=1";
                            
                
                //
                //BodegaOrigen.DataSource = logicaxela.Tabla(ssql);
                //BodegaOrigen.DisplayMember = "nombre_bodega";
                //BodegaOrigen.ValueMember = "codigo_bodega";
                //
                
                BodegaDestino.DataSource = logicaxela.Tabla(ssql);
                BodegaDestino.DisplayMember = "nombre_bodega";
                BodegaDestino.ValueMember = "codigo_bodega";


                //listar bodegas en el listbox para multiseleccion
                listBox_bodegas.DataSource = logicaxela.Tabla(ssql);
                listBox_bodegas.DisplayMember = "nombre_bodega";
                listBox_bodegas.ValueMember = "codigo_bodega";
                 

            }
            catch
            { }
            /* Se llena Combo de Categoarias*/
            try
            {
                string ssql = "(select 0 as codigo,'Todas' as categoria from dual) union all (select  codigo_subcat as codigo,nombre_subcategoria as categoria from sub_categorias where estadoid=1 order by nombre_subcategoria asc)";
                comboBoxCategorias.DataSource = logicaxela.Tabla(ssql);
                comboBoxCategorias.DisplayMember = "categoria";
                comboBoxCategorias.ValueMember = "codigo";

            }
            catch
            { }

            /* FECHAS */
            try
            {

                /* jramirez 2014.01.20 */

                DateTime now = DateTime.Now;

                string date = now.GetDateTimeFormats('d')[0];
                this.FechaFin.EditValue = date;

                DateTime now2 = DateTime.Now.AddMonths(-1);

                string date2 = now2.ToShortDateString();
                this.FechaInicio.EditValue = date2;

            }
            catch
            { }
        }
        /* TOMA INVENTARIO */
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            cargarbodegas();
            string consulta = "SELECT        codigo_articulo, articulo, Ult_compra, Ult_venta, Ult_precio, nombre_bodega, categoria, codigo_categoria, existencia_articulo, codigo_bodega FROM v_inventario where codigo_bodega in (" + Bodega_origen_int + ")  ";


            if (comboBoxCategorias.SelectedValue.ToString() == "0")
            {
                if ((textEdit6.Text.Trim() != "") || (textEdit5.Text.Trim() != ""))
                {
                    if ((textEdit6.Text.Trim() != "") && (textEdit5.Text.Trim() == ""))
                    {
                        consulta = consulta + " and  codigo_articulo like '%" + textEdit6.Text + "%'";
                    }
                    else
                    {
                        if ((textEdit6.Text.Trim() == "") && (textEdit5.Text.Trim() != ""))
                        {
                            consulta = consulta + " and codigo_articulo like '%" + textEdit5.Text + "%'";
                        }
                        else
                        {
                            consulta = consulta + " and codigo_articulo between '" + textEdit6.Text + "' and '" + textEdit5.Text + "' ";
                        }
                    }

                }
            }
            else
            {
                consulta = consulta + " and codigo_categoria = " + comboBoxCategorias.SelectedValue;
            }
            consulta = consulta + " GROUP BY codigo_articulo";


            MySqlDataAdapter adaptador1 = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
            DataSet_Inventario dataset = new DataSet_Inventario();
            adaptador1.Fill(dataset, "v_inventario");

            //comprobar que el dataset no este vacio para no devuelva basura
            if (dataset.v_inventario.Rows.Count == 0)
            {
                MessageBox.Show("Este reporte no contiene informacion");
            }
            else
            {
                XtraReport_TomaInventario reportet = new XtraReport_TomaInventario();
                reportet.DataSource = dataset;
                reportet.DataMember = dataset.Tables["v_inventario"].TableName;
                reportet.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                reportet.RequestParameters = false;
                reportet.ShowPreviewDialog();
            }
            this.Cursor = Cursors.Default;
        }

        private void listBox_bodegas_MouseUp(object sender, MouseEventArgs e)
        {
            
        }


        void cargarbodegas()
        {
            Bodega_origen_int = "0";
            Bodega_origen_int_nombre = "";
            if (listBox_bodegas.SelectedItems.Count > 0)
            {

                for (int cnt = 0; cnt < listBox_bodegas.SelectedItems.Count; cnt++)
                {
                    DataRowView bdgs = listBox_bodegas.SelectedItems[cnt] as DataRowView;
                    Bodega_origen_int += "," + bdgs["codigo_bodega"].ToString();
                    Bodega_origen_int_nombre += "," + bdgs["nombre_bodega"].ToString();
                }
            }
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }
                                                   
    }
}