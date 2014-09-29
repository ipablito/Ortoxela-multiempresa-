namespace ortoxela.Principal
{
    partial class f_logo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_logo));
            this.labelControl_empresa = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.textEdit_direccion = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_direccion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl_empresa
            // 
            this.labelControl_empresa.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_empresa.Location = new System.Drawing.Point(12, 12);
            this.labelControl_empresa.Name = "labelControl_empresa";
            this.labelControl_empresa.Size = new System.Drawing.Size(129, 23);
            this.labelControl_empresa.TabIndex = 0;
            this.labelControl_empresa.Text = "labelControl1";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Location = new System.Drawing.Point(12, 60);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.pictureEdit1.Size = new System.Drawing.Size(308, 243);
            this.pictureEdit1.TabIndex = 1;
            // 
            // textEdit_direccion
            // 
            this.textEdit_direccion.Location = new System.Drawing.Point(428, 126);
            this.textEdit_direccion.Name = "textEdit_direccion";
            this.textEdit_direccion.Size = new System.Drawing.Size(273, 20);
            this.textEdit_direccion.TabIndex = 2;
            this.textEdit_direccion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEdit_direccion_KeyDown);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(543, 152);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(158, 23);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "Seleccionar archivo";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(428, 107);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(62, 13);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Cambiar logo";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = global::ortoxela.Properties.Resources.OK32;
            this.simpleButton2.Location = new System.Drawing.Point(340, 340);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(186, 35);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "Aceptar";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = global::ortoxela.Properties.Resources.error_32;
            this.simpleButton3.Location = new System.Drawing.Point(532, 340);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(160, 37);
            this.simpleButton3.TabIndex = 6;
            this.simpleButton3.Text = "Cancelar";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // f_logo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 389);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.textEdit_direccion);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.labelControl_empresa);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "f_logo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logo de la empresa";
            this.Load += new System.EventHandler(this.f_logo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_direccion.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl_empresa;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit_direccion;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
    }
}