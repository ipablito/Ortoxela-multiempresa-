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

namespace ortoxela.Reportes.Proveedores
{
    public partial class Frm_RepProveedores : DevExpress.XtraEditors.XtraForm
    {
        public Frm_RepProveedores()
        {
            InitializeComponent();
        }

        string ssql;
        classortoxela logicaxela = new classortoxela();

        private void CargaDatos()
        {
            try
            {
                ssql = "SELECT codigo_proveedor AS CODIGO,nombre_proveedor AS NOMBRE FROM proveedores WHERE estadoid<>2";
                gridLookProveedor.Properties.DataSource = logicaxela.Tabla(ssql);
                gridLookProveedor.Properties.DisplayMember = "NOMBRE";
                gridLookProveedor.Properties.ValueMember = "CODIGO";

            }
            catch
            { }
            

        }
        int id_proveedor;
        private void gridLookProveedor_EditValueChanged(object sender, EventArgs e)
        {
            id_proveedor = Convert.ToInt32(gridLookProveedor.EditValue);
        }

        private void Frm_RepProveedores_Load(object sender, EventArgs e)
        {
            CargaDatos();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
//            string consulta = "select p.codigo_proveedor, p.nombre_proveedor,h.no_documento,h.monto_neto, sum(d.cantidad_enviada) unidades,sum(d.precio_unitario*d.cantidad_enviada) as total,"+
//" h.fecha as fecha_compra,DATE_ADD(h.fecha,interval p.dias_credito day) as fecha_venc, if(h.contado_credito=0,'Contado','Credito') as Tipo_Pago "+
//" FROM header_doctos_inv h join ortoxela.detalle_doctos_inv d  on(h.id_documento = d.id_documento) "+
//"join proveedores p on (h.codigo_proveedor = p.codigo_proveedor) where codigo_serie =7 and p.codigo_proveedor="+gridLookProveedor.EditValue+"  group by p.nombre_proveedor,h.no_documento";

//            MySqlDataAdapter adaptador = new MySqlDataAdapter(consulta, Properties.Settings.Default.ortoxelaConnectionString);
//            DataSet_RepProveedores datasetp = new DataSet_RepProveedores();
//            adaptador.Fill(datasetp,"v_compras_proveedor");
//            XtraReport_RepProveedores reporteP =new XtraReport_RepProveedores();
//            reporteP.DataSource = datasetp;
//            reporteP.DataMember = datasetp.Tables["v_compras_proveedor"].TableName;
//            reporteP.RequestParameters = false;
//            reporteP.ShowPreviewDialog();
            
            XtraReport_RepUnProveedor Reporteu = new XtraReport_RepUnProveedor();
            Reporteu.Parameters["Fecha_inicio"].Value = dateEdit1.EditValue;
            Reporteu.Parameters["Fecha_fin"].Value = dateEdit2.EditValue;
            Reporteu.Parameters["Codigo_proveedor"].Value = gridLookProveedor.EditValue;
            Reporteu.RequestParameters = false;
            Reporteu.ShowPreview();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraReport_RepProveedores Reportes = new XtraReport_RepProveedores();
            Reportes.Parameters["Fecha_inicio"].Value = dateEdit3.EditValue;
            Reportes.Parameters["Fecha_fin"].Value = dateEdit4.EditValue;
            Reportes.RequestParameters = false;
            Reportes.ShowPreview();
        }

       }
}