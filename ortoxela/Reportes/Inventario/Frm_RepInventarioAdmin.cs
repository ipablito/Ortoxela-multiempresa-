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

namespace ortoxela.Reportes.Inventario
{
    public partial class Frm_RepInventarioAdmin : Form
    {
        public Frm_RepInventarioAdmin()
        {
            InitializeComponent();
        }
        /* COSTOS Y EXISTENCIA RESUMIDO */
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            XtraReport_InventarioResumen reporte = new XtraReport_InventarioResumen();
            reporte.Parameters["Fecha_inicio"].Value = dateEdit6.EditValue;
            reporte.Parameters["Fecha_fin"].Value = dateEdit5.EditValue;
            reporte.RequestParameters = false;
            reporte.ShowPreview();
        }

        /* COSTOS Y EXISTENCIA DETALLADO */
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Int32 bodega1 = 0;
            Int32 bodega2 = 100;
            string botittle = "Todas";
            string consulta = "SELECT precio_venta,codigo_articulo, articulo, Ult_compra, Ult_venta, Ult_precio, nombre_bodega, categoria, codigo_categoria, existencia_articulo, codigo_bodega  " +
                               " FROM v_inventario2h where 1=1 ";
            if (bodegas.SelectedValue.ToString() != "0")
            {
                consulta = "SELECT `a`.`precio_venta` AS `precio_venta`,`a`.`codigo_articulo` AS `codigo_articulo`,REPLACE(a.descripcion,'\"','') AS articulo,SUM(COALESCE(`b`.`existencia_articulo`,0)) AS `existencia_articulo`, " +
                " DATE_FORMAT((SELECT f_ultima_compra(b.codigo_articulo) FROM DUAL),'%d-%m-%Y') AS Ult_compra, " +
                " DATE_FORMAT((SELECT f_ultima_venta(b.codigo_articulo) FROM DUAL),'%d-%m-%Y') AS Ult_venta, " +
                " (`a`.`costo` / 1.12) AS `Ult_precio`, " +
                    // " (SUM(COALESCE(`b`.`existencia_articulo`,0))*(`a`.`costo` / 1.12)) AS costo_total, "
                " `bh`.`codigo_bodega` AS `codigo_bodega`,`bh`.`nombre_bodega` AS `nombre_bodega`,`a`.`codigo_categoria` AS `codigo_categoria`,`s`.`nombre_subcategoria` AS `categoria`  " +
                " FROM `bodegas_header` `bh` JOIN `bodegas` `b` ON(`bh`.`codigo_bodega` = `b`.`codigo_bodega` ) " +
                " JOIN `articulos` `a` ON(`b`.`codigo_articulo` = `a`.`codigo_articulo`) " +
                " JOIN `sub_categorias` `s` ON (`a`.`codigo_categoria` = `s`.`codigo_subcat`) " +
                " WHERE bh.codigo_bodega= " + bodegas.SelectedValue.ToString();
                if (comboBoxCategorias.SelectedValue.ToString() != "0")
                {
                    consulta = consulta + "  and a.codigo_categoria = " + comboBoxCategorias.SelectedValue;
                }
                consulta = consulta + " GROUP BY b.codigo_articulo ORDER BY a.descripcion";
                // consulta = consulta + " and codigo_bodega =" + bodegas.SelectedValue.ToString();
                bodega1 = Int32.Parse(bodegas.SelectedValue.ToString());
                bodega2 = Int32.Parse(bodegas.SelectedValue.ToString());
                botittle = bodegas.Text;
            }
            else
            {
                if (comboBoxCategorias.SelectedValue.ToString() != "0")
                {
                    consulta = consulta + "  and a.codigo_categoria = " + comboBoxCategorias.SelectedValue;
                }
            };

            MySqlDataAdapter adaptadori = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
            DataSet_Inventario2H dataseti = new DataSet_Inventario2H();
            adaptadori.Fill(dataseti, "v_inventario2h");
            XtraReport_Inventario2H reportei = new XtraReport_Inventario2H();
            //  XtraReport_TomaInventario reportet = new XtraReport_TomaInventario();            
            reportei.DataSource = dataseti;
            reportei.DataMember = dataseti.Tables["v_inventario2h"].TableName;
            reportei.Parameters["Fecha_inicio"].Value = dateEdit1.EditValue;
            reportei.Parameters["Fecha_fin"].Value = dateEdit2.EditValue;
            reportei.Parameters["existencia"].Value = 0;
            reportei.Parameters["codigo_bodega1"].Value = bodega1;
            reportei.Parameters["codigo_bodega2"].Value = bodega2;
            reportei.Parameters["bodega"].Value = botittle;
            reportei.RequestParameters = false;
            reportei.ShowPreview();
            this.Cursor = Cursors.Default;
            // ShowPreviewDialog();
            /*}
            else
            {

                XtraReport_Inventario1 reporteI = new XtraReport_Inventario1();
                reporteI.Parameters["Fecha_inicio"].Value = dateEdit1.EditValue;
                reporteI.Parameters["Fecha_fin"].Value = dateEdit2.EditValue;                
                reporteI.Parameters["existencia"].Value = 0;
                reporteI.RequestParameters = false;
                reporteI.ShowPreview();
            }; */
        }
    }
}
