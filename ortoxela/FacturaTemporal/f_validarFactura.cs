using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;


using integracion_ii;
using Sisnova.Invex.BL;

namespace ortoxela.FacturaTemporal
{
    public partial class f_validarFactura : DevExpress.XtraEditors.XtraForm
    {
        Venta EncVentaRecibido = null;
        public f_validarFactura(int IdOperacion,Venta EncVenta)
        {
            InitializeComponent();
            EncVentaRecibido = EncVenta;
            if (IdOperacion == 1)
                Ocheque();
            else
                Odeposito();
        }

        void Ocheque()
        {
            groupControl_cheque.Visible = true;
            object t = integracion_ii.Class_cuentaBancaria.listaBancos();
            comboBox_banco.DisplayMember = "nombre";
            comboBox_banco.DataSource = t;
            textEdit_nocheque.Focus();
        }

        void Odeposito()
        {
            groupControl_deposito.Visible = true;
            object t = integracion_ii.Class_cuentaBancaria.listaCuentasBancarias();
            comboBox_deposito.DisplayMember = "nombre";
            comboBox_deposito.DataSource = t;
            textEdit_nodeposito.Focus();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (comboBox_banco.Text != "" && textEdit_nocheque.Text!="")
            {
                integracion_ii.Class_venta.validarenCheque(EncVentaRecibido, comboBox_banco.Text, Convert.ToInt32(textEdit_nocheque.Text));
                this.Close();
            }
            else
                comboBox_banco.Focus();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (comboBox_deposito.Text != "" && textEdit_nodeposito.Text!="")
            {
                integracion_ii.Class_venta.validarenDeposito(EncVentaRecibido, comboBox_deposito.Text, Convert.ToInt32(textEdit_nodeposito.Text));
                this.Close();
            }
            else
                comboBox_deposito.Focus();
        }
    }
}