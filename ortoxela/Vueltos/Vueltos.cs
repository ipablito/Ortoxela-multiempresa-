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

using DevExpress.XtraReports.UI;


using integracion_ii;
using Sisnova.Invex.BL;

namespace ortoxela.Vueltos
{
    public partial class Vueltos : DevExpress.XtraEditors.XtraForm
    {
        public Vueltos()
        {
            InitializeComponent();
        }
        private void CargaDatosCombos()
        {
            try
            {
                cadena = "SELECT tipo_pago as CODIGO, nombre_tipo_pago AS 'TIPO PAGO' FROM tipo_pago where estadoid<>2";
                gridLookTipoPago.Properties.DataSource = logicaorto.Tabla(cadena);
                gridLookTipoPago.Properties.DisplayMember = "TIPO PAGO";
                gridLookTipoPago.Properties.ValueMember = "CODIGO";
                gridLookTipoPago.Text = "";
                gridLookTipoPago.EditValue = 2;

                comboBox_cuentas.DataSource = integracion_ii.Class_cuentaBancaria.listaCuentasBancarias();
                comboBox_cuentas.DisplayMember = "Nombre";
                comboBox_cuentas.ValueMember = "Nombre";
            }
            catch
            { }       
           

        }
        MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
        MySqlCommand comando = new MySqlCommand();
       
        
        string cadena;
        
        classortoxela logicaorto = new classortoxela();
        private void frm_regreso_Load(object sender, EventArgs e)
        {
            dateEdit1.DateTime = DateTime.Now;
            CargaDatosCombos();
                      
        }
        public void limpiar()
        {
            //textBANCO.Text = "";
            textCHEQUE.Text = "";
            memoDescripcion.Text = "";
        }

               
        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string id_vuelto;
        private void sbnuevo_Click(object sender, EventArgs e)
        {
            limpiar();
            cadena = "SELECT vueltos.id_vuelto AS CODIGO,COALESCE(f.no_documento,'N/A')AS FACTURA,p.no_documento AS PEDIDO,concat(re.no_recibo,' [',sr.serie_documento,']') AS RECIBO,v.no_documento AS VALE,vueltos.monto_vuelto as MONTO,clientes.nombre_cliente AS CLIENTE FROM vueltos INNER JOIN  clientes  ON clientes.codigo_cliente = vueltos.codigo_cliente "+
                        "INNER JOIN header_doctos_inv v ON vueltos.id_vale = v.id_documento INNER JOIN header_doctos_inv p ON vueltos.id_pedido = p.id_documento LEFT JOIN header_doctos_inv f ON (vueltos.id_factura = f.id_documento)LEFT JOIN recibos re ON (re.id_recibos = vueltos.id_recibo)LEFT JOIN series_documentos sr ON (re.codigo_serie = sr.codigo_serie) WHERE vueltos.estadoid=4";
            clases.ClassVariables.cadenabusca = cadena;
            Form nuevo = new Buscador.Buscador();
            nuevo.ShowDialog();
            if (Buscador.Buscador.SeleccionSiNo)
            {
                id_vuelto = clases.ClassVariables.id_busca;
                DataTable tempLlena = new DataTable();
                cadena = "SELECT vueltos.id_vuelto as CODIGO,vueltos.id_factura AS FACTURA,vueltos.id_pedido AS PEDIDO,vueltos.id_recibo AS RECIBO,vueltos.id_vale AS VALE,clientes.nombre_cliente AS CLIENTE,vueltos.monto_vuelto as VUELTO " +
                            "FROM vueltos INNER JOIN clientes ON vueltos.codigo_cliente = clientes.codigo_cliente WHERE vueltos.id_vuelto=" + id_vuelto;
                tempLlena = logicaorto.Tabla(cadena);
                foreach (DataRow fila in tempLlena.Rows)
                {
                    textFACTURA.Text = fila[1].ToString();
                    textPEDIDO.Text = fila[2].ToString();
                    textRECIBO.Text = fila[3].ToString();
                    textVALE.Text = fila[4].ToString();
                    textNombreCliente.Text = fila[5].ToString();
                    textTotaL.Text = fila[6].ToString();
                    
                    sbAceptar.Enabled = true;
                    groupControl1.Enabled = true;
                }
                cadena = "SELECT vueltos.id_vuelto AS CODIGO,f.no_documento AS FACTURA,p.no_documento AS PEDIDO,vueltos.id_recibo AS RECIBO,v.no_documento AS VALE,clientes.nombre_cliente AS CLIENTE FROM vueltos INNER JOIN  clientes  ON clientes.codigo_cliente = vueltos.codigo_cliente " +
                        "INNER JOIN header_doctos_inv v ON vueltos.id_vale = v.id_documento INNER JOIN header_doctos_inv p ON vueltos.id_pedido = p.id_documento INNER JOIN header_doctos_inv f ON vueltos.id_factura = f.id_documento WHERE vueltos.estadoid=4 AND vueltos.id_vuelto=" + id_vuelto;
                tempLlena = logicaorto.Tabla(cadena);
                try
                {
                    textPEDIDO.Text = tempLlena.Rows[0]["PEDIDO"].ToString();
                    textRECIBO.Text = tempLlena.Rows[0]["RECIBO"].ToString();
                    textVALE.Text = tempLlena.Rows[0]["VALE"].ToString();
                    textFACTURA.Text = tempLlena.Rows[0]["FACTURA"].ToString();
                }
                catch
                { }
            }      
            
        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        string monto = ""; string banco = "";
        string no_vuelto;

        private void GuardarEnINVEX()
        {
            DataTable dt = new DataTable();
            string cns = "SELECT nit,nombre_cliente,telefono_casa FROM clientes INNER JOIN vueltos ON clientes.`codigo_cliente`=vueltos.`codigo_cliente` WHERE vueltos.`id_vuelto`=" + id_vuelto.ToString();
            dt = logicaorto.Tabla(cns);
            int noCheque = Convert.ToInt32(textCHEQUE.Text);
            string nombreCuenta = comboBox_cuentas.Text;
            string nitCliente = dt.Rows[0][0].ToString();
            string nombreCliente = dt.Rows[0][1].ToString();
            string telefonoB = dt.Rows[0][2].ToString();
            decimal monto = Convert.ToDecimal(textTotaL.Text);
            integracion_ii.Class_devolucion.ChequeDeDevolucion(noCheque, nombreCuenta, nitCliente, nombreCliente,telefonoB, monto);
        }
        private void sbAceptar_Click(object sender, EventArgs e)
        {
            cadena = "SELECT coalesce(max(vueltos.no_vuelto),0)+1 as no_vlto from vueltos";
            no_vuelto = logicaorto.Tabla(cadena).Rows[0][0].ToString();
            if (dxValidationAceptaDev.Validate()&&(textCHEQUE.Text!=""))
            {
                if (MessageBox.Show("¿ESTA SEGURO DE CONTINUAR, AL HACER ESTO NO HAY VUELTA ATRAS?", "INFORMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cadena = "update vueltos "+
                            "SET fecha_pago = '"+dateEdit1.DateTime.ToString("yyyy-MM-dd HH:mm:ss")+"', tipo_pago = "+gridLookTipoPago.EditValue+", no_cheque = '"+textCHEQUE.Text+"', nombre_banco = '"+ comboBox_cuentas.Text +"', decripcion = '"+memoDescripcion.Text+"', estadoid = 5 ,no_vuelto="+no_vuelto+
                            " WHERE vueltos.id_vuelto="+id_vuelto;
                    
                    

                    if (logicaorto.variosservios(cadena) == 1)
                    {
                        if (Class_integracion.logeado == true)
                        {
                            GuardarEnINVEX();
                        }

                        clases.ClassMensajes.INSERTO(this);
                        sbimprimir.Enabled = true;
                        sbAceptar.Enabled = false;
                        groupControl1.Enabled = false;
                        monto = textTotaL.Text;
                        banco = comboBox_cuentas.Text;
                    }
                    else
                        clases.ClassMensajes.NoINSERTO(this);

                }
            }
            else
            {
                clases.ClassMensajes.FaltanDatosEnCampos(this);
            }
        }

        private void sbimprimir_Click(object sender, EventArgs e)
        {
            try
            {
                Vuelto.XtraReportVueltos reporte = new Vuelto.XtraReportVueltos();
                reporte.Parameters["id"].Value = id_vuelto;
                reporte.Parameters["letras"].Value = logicaorto.enletras(monto)+" QUETZALES";
                reporte.Parameters["banco"].Value = banco+".";
                reporte.Parameters["no_vuelto"].Value = no_vuelto;
                reporte.RequestParameters = false;
                reporte.ShowPreviewDialog();
               
            }
            catch
            {

            }
        }

       

        
    }
}