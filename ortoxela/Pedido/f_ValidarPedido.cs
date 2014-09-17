using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using integracion_ii;

namespace ortoxela.Pedido
{
    public partial class f_ValidarPedido : DevExpress.XtraEditors.XtraForm
    {
        public f_ValidarPedido()
        {
            InitializeComponent();
        }

        string nombrecliente="";
        string nitcliente="";
        decimal total=0;
        string telefono="";

        public void Cheque(string nombre__Cliente, string nit__Cliente, string telefono_,decimal monto__)
        {
            nombrecliente = nombre__Cliente;
            nitcliente = nit__Cliente;
            total = monto__;
            groupControl_cheque.Visible = true;
            comboBox_banco.DisplayMember = "Nombre";
            comboBox_banco.DataSource = Class_cuentaBancaria.listaBancos();
        }

        public void Deposito(string nombre__Cliente, string nit__Cliente, string telefono_, decimal monto__)
        {
            nombrecliente = nombre__Cliente;
            nitcliente = nit__Cliente;
            total = monto__;
            groupControl_deposito.Visible = true;
            comboBox_cuentaBancaria.DisplayMember = "Nombre";
            comboBox_cuentaBancaria.DataSource = Class_cuentaBancaria.listaCuentasBancarias();
        }


        private void f_ValidarPedido_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (comboBox_banco.Text != "" && textEdit_nocheque.Text != "" && dateEdit_cheque.Text!="")
            {
                Class_anticipo.AnticipoEnCheque(nombrecliente, nitcliente,telefono, comboBox_banco.Text, Convert.ToInt32(textEdit_nocheque.Text), total,dateEdit_cheque.DateTime);
                this.Close();
            }
            else
                comboBox_banco.Focus();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (comboBox_cuentaBancaria.Text != "" && textEdit_nodeposito.Text != "" && comboBox_forma.Text!="" && dateEdit_deposito.Text!="")
            {
                Class_anticipo.AnticipoEnDeposito(nombrecliente, nitcliente,telefono, comboBox_cuentaBancaria.Text, Convert.ToInt32(textEdit_nodeposito.Text), total,comboBox_forma.Text,dateEdit_deposito.DateTime);
                this.Close();
            }
            else
                comboBox_cuentaBancaria.Focus();
        }
    }
}