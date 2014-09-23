using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;

using DevExpress.XtraReports.UI;

namespace ortoxela.Reportes.Admin
{
    public partial class Frm_ReportesAdmin : Form
    {
        public Frm_ReportesAdmin()
        {
            InitializeComponent();
            
        }

        private void labelControl25_Click(object sender, EventArgs e)
        {

        }
        
        string Bodega_origen_int = "0";
        string Bodega_origen_int_nombre = "";
        
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

        string categoria_origen = "0";
        void cargarcate()
        {
            categoria_origen = "0";
            if (listBox_cate.SelectedItems.Count > 0)
            {
                for (int cnt = 0; cnt < listBox_cate.SelectedItems.Count; cnt++)
                {
                    DataRowView ctgs = listBox_cate.SelectedItems[cnt] as DataRowView;
                    categoria_origen += "," + ctgs["codigo"].ToString();
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                Reportes.Inventario.XtraReport_InventarioResumen reporte = new Reportes.Inventario.XtraReport_InventarioResumen();
                reporte.Parameters["Fecha_inicio"].Value = dateEdit6.EditValue;
                reporte.Parameters["Fecha_fin"].Value = dateEdit5.EditValue;
                reporte.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa;
                reporte.RequestParameters = false;
                reporte.ShowPreview();
            }
            catch
            { }

            this.Cursor = Cursors.Default;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            cargarbodegas();
            
            string consulta = "SELECT codigo_articulo, articulo, Ult_compra, Ult_venta, Ult_precio, nombre_bodega, categoria, codigo_categoria, existencia_articulo, codigo_bodega  " +
                               " FROM v_inventario where 1=1 ";
            if (Bodega_origen_int!="0")
            {
                consulta = "SELECT `a`.`codigo_articulo` AS `codigo_articulo`,REPLACE(a.descripcion,'\"','') AS articulo,SUM(COALESCE(`b`.`existencia_articulo`,0)) AS `existencia_articulo`, " +
                " DATE_FORMAT((SELECT f_ultima_compra(b.codigo_articulo) FROM DUAL),'%d-%m-%Y') AS Ult_compra, " +
                " DATE_FORMAT((SELECT f_ultima_venta(b.codigo_articulo) FROM DUAL),'%d-%m-%Y') AS Ult_venta, " +
                " (`a`.`costo` / 1.12) AS `Ult_precio`, " +
                    // " (SUM(COALESCE(`b`.`existencia_articulo`,0))*(`a`.`costo` / 1.12)) AS costo_total, "
                " `bh`.`codigo_bodega` AS `codigo_bodega`,`bh`.`nombre_bodega` AS `nombre_bodega`,`a`.`codigo_categoria` AS `codigo_categoria`,`s`.`nombre_subcategoria` AS `categoria`  " +
                " FROM `bodegas_header` `bh` JOIN `bodegas` `b` ON(`bh`.`codigo_bodega` = `b`.`codigo_bodega` ) " +
                " JOIN `articulos` `a` ON(`b`.`codigo_articulo` = `a`.`codigo_articulo`) " +
                " JOIN `sub_categorias` `s` ON (`a`.`codigo_categoria` = `s`.`codigo_subcat`) " +
                " WHERE bh.codigo_bodega in (" + Bodega_origen_int + ") ";
                
                
                
                //if (comboBoxCategorias.SelectedValue.ToString() != "0")
                //{
                //    consulta = consulta + "  and a.codigo_categoria = " + comboBoxCategorias.SelectedValue;
                //}

                if (listBox_cate.SelectedItems.Count > 0)
                {
                    cargarcate();
                    consulta = consulta + " and a.codigo_categoria in(" + categoria_origen + ") ";
                }



                consulta = consulta + " GROUP BY b.codigo_articulo ORDER BY a.descripcion";
                // consulta = consulta + " and codigo_bodega =" + bodegas.SelectedValue.ToString();
                
            }
            else
            {
                if (listBox_cate.SelectedItems.Count > 0)
                {
                    cargarcate();
                    consulta = consulta + " and a.codigo_categoria in(" + categoria_origen + ") ";
                }
            };
            MySqlDataAdapter adaptadori = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
            Reportes.Inventario.DataSet_Inventario dataseti = new Reportes.Inventario.DataSet_Inventario();
            adaptadori.Fill(dataseti, "v_inventario");
            Reportes.Inventario.XtraReport_Inventario1 reportei = new Reportes.Inventario.XtraReport_Inventario1();
                 
            reportei.DataSource = dataseti;
            reportei.DataMember = dataseti.Tables["v_inventario"].TableName;
            reportei.Parameters["Fecha_inicio"].Value = dateEdit6.EditValue;
            reportei.Parameters["Fecha_fin"].Value = dateEdit5.EditValue;
            //reportei.Parameters["existencia"].Value = 0;
            //reportei.Parameters["codigo_bodega1"].Value = bodega1;
            //reportei.Parameters["codigo_bodega2"].Value = bodega2;
            reportei.Parameters["bodega"].Value = Bodega_origen_int_nombre;
            reportei.Parameters["nombreEmpresa"].Value = clases.ClassVariables.nombreEmpresa; 
            reportei.RequestParameters = false;
            reportei.ShowPreview();
            this.Cursor = Cursors.Default;
        }

        classortoxela ortoxela = new classortoxela();
        classortoxela logicaxela = new classortoxela();
        private void Frm_ReportesAdmin_Load(object sender, EventArgs e)
        {
            this.Text = "Reportes de Administracion - " + clases.ClassVariables.nombreEmpresa; 
            try
            {
                /*string ssql = "  Select 0 as codigo_bodega, 'Todas' as nombre_bodega from dual " +
                            " union all  " +
                            " select codigo_bodega,nombre_bodega from bodegas_header where estadoid=1";*/
                string ssql ="select codigo_bodega,nombre_bodega from bodegas_header where estadoid=1";

                

                listBox_bodegas.DataSource = logicaxela.Tabla(ssql);
                listBox_bodegas.DisplayMember = "nombre_bodega";
                listBox_bodegas.ValueMember = "codigo_bodega";

            }
            catch
            { }
            /* Se llena Combo de Categoarias*/
            try
            {
                //string ssql = "(select 0 as codigo,'Todas' as categoria from dual) union all (select  codigo_subcat as codigo,nombre_subcategoria as categoria from sub_categorias where estadoid=1 order by nombre_subcategoria asc)";
                //comboBoxCategorias.DataSource = logicaxela.Tabla(ssql);
                //comboBoxCategorias.DisplayMember = "categoria";
                //comboBoxCategorias.ValueMember = "codigo";

                string ssql = "select  codigo_subcat as codigo,nombre_subcategoria as categoria from sub_categorias where estadoid=1 order by nombre_subcategoria asc";
                listBox_cate.DataSource = logicaxela.Tabla(ssql);
                listBox_cate.DisplayMember = "categoria";
                listBox_cate.ValueMember = "codigo";

            }
            catch
            { }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }
    }
}
