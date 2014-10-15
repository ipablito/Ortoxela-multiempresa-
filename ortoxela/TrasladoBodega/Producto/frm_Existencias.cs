using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ortoxela.Reportes.Producto
{
    public partial class frm_Existencias : DevExpress.XtraEditors.XtraForm
    {
        public frm_Existencias()
        {
            InitializeComponent();
        }
        string ssql;
        classortoxela logicaxela = new classortoxela();
        private void frm_Existencias_Load(object sender, EventArgs e)
        {
            /* ssql = "SELECT codigo_bodega as CODIGO, nombre_bodega AS NOMBRE FROM bodegas_header where bodegas_header.estadoid=1"; */
            /* jramirez 2013.07.24 */
            //ssql = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega=1 AND userid=" + clases.ClassVariables.id_usuario;
            ssql = "SELECT distinct codigo_bodega AS CODIGO, nombre_bodega AS NOMBRE FROM v_bodegas_series_usuarios  WHERE estadoid_bodega=1";
            gridLookBodega.Properties.DataSource = logicaxela.Tabla(ssql);
            gridLookBodega.Properties.DisplayMember = "NOMBRE";
            gridLookBodega.Properties.ValueMember = "CODIGO";
            gridLookBodega.EditValue = 0;
            gridLookBodega.Text = "";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dxValidationProvider1.Validate())
                {
                    ssql = "SELECT articulos.codigo_articulo AS CODIGO,articulos.descripcion AS 'ARTICULO', bodegas.existencia_articulo AS 'EXISTENCIA'"+/*,articulos.costo as 'PRECIO COSTO'*/", articulos.precio_venta AS 'PRECIO VENTA' FROM articulos INNER JOIN bodegas ON articulos.codigo_articulo=bodegas.codigo_articulo WHERE bodegas.codigo_bodega=" + gridLookBodega.EditValue;
                    gridControl1.DataSource = logicaxela.Tabla(ssql);
                    gridView1.Columns["ARTICULO"].Width = 600;
                }
                else
                    clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
            catch
            { }
        }

        private void gridLookBodega_EditValueChanged(object sender, EventArgs e)
        {
            gridView1.Columns.Clear();
        }
    }
}