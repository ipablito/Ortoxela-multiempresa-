﻿namespace ortoxela.ModCobranza.Proveedores
{
    partial class Tipo_proveedor_conta
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule2 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule3 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleaceptar = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.textporcentaje = new DevExpress.XtraEditors.TextEdit();
            this.textproveedorconta = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textporcentaje.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textproveedorconta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleaceptar);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Location = new System.Drawing.Point(28, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(600, 290);
            this.panelControl1.TabIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Image = global::ortoxela.Properties.Resources.window_remove_32x32_32;
            this.simpleButton2.Location = new System.Drawing.Point(444, 221);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(130, 40);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "Cancelar/Salir";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleaceptar
            // 
            this.simpleaceptar.Appearance.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleaceptar.Appearance.Options.UseFont = true;
            this.simpleaceptar.Image = global::ortoxela.Properties.Resources.accept_32x32_32;
            this.simpleaceptar.Location = new System.Drawing.Point(240, 221);
            this.simpleaceptar.Name = "simpleaceptar";
            this.simpleaceptar.Size = new System.Drawing.Size(130, 40);
            this.simpleaceptar.TabIndex = 8;
            this.simpleaceptar.Text = "Agregar";
            this.simpleaceptar.Click += new System.EventHandler(this.simpleaceptar_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = global::ortoxela.Properties.Resources.add_32x32_32;
            this.simpleButton1.Location = new System.Drawing.Point(36, 221);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(130, 40);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "NUEVO";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.textporcentaje);
            this.groupControl1.Controls.Add(this.textproveedorconta);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(36, 27);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(538, 171);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Tipo Proveedor Contabilidad";
            // 
            // textporcentaje
            // 
            this.textporcentaje.Location = new System.Drawing.Point(267, 98);
            this.textporcentaje.Name = "textporcentaje";
            this.textporcentaje.Properties.Mask.EditMask = "P";
            this.textporcentaje.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.textporcentaje.Size = new System.Drawing.Size(204, 20);
            this.textporcentaje.TabIndex = 3;
            conditionValidationRule2.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule2.ErrorText = "Ingrese un Valor en Porcentaje";
            this.dxValidationProvider1.SetValidationRule(this.textporcentaje, conditionValidationRule2);
            // 
            // textproveedorconta
            // 
            this.textproveedorconta.Location = new System.Drawing.Point(267, 65);
            this.textproveedorconta.Name = "textproveedorconta";
            this.textproveedorconta.Size = new System.Drawing.Size(204, 20);
            this.textproveedorconta.TabIndex = 2;
            conditionValidationRule3.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule3.ErrorText = "Ingrese el Tipo de Proveedor";
            this.dxValidationProvider1.SetValidationRule(this.textproveedorconta, conditionValidationRule3);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(182, 96);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(68, 20);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Porcentaje:";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(18, 63);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(232, 20);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Nombre Tipo Proveedor  Contabilidad:";
            // 
            // Tipo_proveedor_conta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 344);
            this.Controls.Add(this.panelControl1);
            this.Name = "Tipo_proveedor_conta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tipo Proveedor Contabilidad";
            this.Load += new System.EventHandler(this.Tipo_proveedor_conta_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textporcentaje.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textproveedorconta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleaceptar;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit textporcentaje;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.TextEdit textproveedorconta;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}