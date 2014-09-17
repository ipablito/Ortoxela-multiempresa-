namespace ortoxela.FacturaTemporal
{
    partial class f_validarFactura
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupControl_cheque = new DevExpress.XtraEditors.GroupControl();
            this.textEdit_nocheque = new DevExpress.XtraEditors.TextEdit();
            this.comboBox_banco = new System.Windows.Forms.ComboBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl_deposito = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit_nodeposito = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_deposito = new System.Windows.Forms.ComboBox();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_cheque)).BeginInit();
            this.groupControl_cheque.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_nocheque.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_deposito)).BeginInit();
            this.groupControl_deposito.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_nodeposito.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl_cheque
            // 
            this.groupControl_cheque.Controls.Add(this.textEdit_nocheque);
            this.groupControl_cheque.Controls.Add(this.comboBox_banco);
            this.groupControl_cheque.Controls.Add(this.labelControl2);
            this.groupControl_cheque.Controls.Add(this.labelControl1);
            this.groupControl_cheque.Controls.Add(this.simpleButton1);
            this.groupControl_cheque.Location = new System.Drawing.Point(33, 44);
            this.groupControl_cheque.Name = "groupControl_cheque";
            this.groupControl_cheque.Size = new System.Drawing.Size(298, 306);
            this.groupControl_cheque.TabIndex = 1;
            this.groupControl_cheque.Text = "Cheque";
            this.groupControl_cheque.Visible = false;
            // 
            // textEdit_nocheque
            // 
            this.textEdit_nocheque.Location = new System.Drawing.Point(103, 109);
            this.textEdit_nocheque.Name = "textEdit_nocheque";
            this.textEdit_nocheque.Properties.Mask.EditMask = "d";
            this.textEdit_nocheque.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.textEdit_nocheque.Size = new System.Drawing.Size(190, 20);
            this.textEdit_nocheque.TabIndex = 10;
            // 
            // comboBox_banco
            // 
            this.comboBox_banco.FormattingEnabled = true;
            this.comboBox_banco.Location = new System.Drawing.Point(103, 45);
            this.comboBox_banco.Name = "comboBox_banco";
            this.comboBox_banco.Size = new System.Drawing.Size(190, 21);
            this.comboBox_banco.TabIndex = 9;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(6, 117);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(57, 13);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "No. Cheque";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(6, 53);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(29, 13);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "Banco";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(38, 277);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(223, 24);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "Guardar";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupControl_deposito
            // 
            this.groupControl_deposito.Controls.Add(this.labelControl3);
            this.groupControl_deposito.Controls.Add(this.textEdit_nodeposito);
            this.groupControl_deposito.Controls.Add(this.label1);
            this.groupControl_deposito.Controls.Add(this.comboBox_deposito);
            this.groupControl_deposito.Controls.Add(this.simpleButton2);
            this.groupControl_deposito.Location = new System.Drawing.Point(337, 47);
            this.groupControl_deposito.Name = "groupControl_deposito";
            this.groupControl_deposito.Size = new System.Drawing.Size(298, 306);
            this.groupControl_deposito.TabIndex = 2;
            this.groupControl_deposito.Text = "Deposito";
            this.groupControl_deposito.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(18, 112);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(59, 13);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "No.Deposito";
            // 
            // textEdit_nodeposito
            // 
            this.textEdit_nodeposito.Location = new System.Drawing.Point(122, 112);
            this.textEdit_nodeposito.Name = "textEdit_nodeposito";
            this.textEdit_nodeposito.Properties.Mask.EditMask = "d";
            this.textEdit_nodeposito.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.textEdit_nodeposito.Size = new System.Drawing.Size(159, 20);
            this.textEdit_nodeposito.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Cuenta Bancaria";
            // 
            // comboBox_deposito
            // 
            this.comboBox_deposito.FormattingEnabled = true;
            this.comboBox_deposito.Location = new System.Drawing.Point(122, 45);
            this.comboBox_deposito.Name = "comboBox_deposito";
            this.comboBox_deposito.Size = new System.Drawing.Size(159, 21);
            this.comboBox_deposito.TabIndex = 5;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(38, 277);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(223, 24);
            this.simpleButton2.TabIndex = 4;
            this.simpleButton2.Text = "Guardar";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // f_validarFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 365);
            this.ControlBox = false;
            this.Controls.Add(this.groupControl_deposito);
            this.Controls.Add(this.groupControl_cheque);
            this.Name = "f_validarFactura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validacion de factura";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_cheque)).EndInit();
            this.groupControl_cheque.ResumeLayout(false);
            this.groupControl_cheque.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_nocheque.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_deposito)).EndInit();
            this.groupControl_deposito.ResumeLayout(false);
            this.groupControl_deposito.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_nodeposito.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl_cheque;
        private DevExpress.XtraEditors.GroupControl groupControl_deposito;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.ComboBox comboBox_banco;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_deposito;
        private DevExpress.XtraEditors.TextEdit textEdit_nocheque;
        private DevExpress.XtraEditors.TextEdit textEdit_nodeposito;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}