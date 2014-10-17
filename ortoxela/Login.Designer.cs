namespace ortoxela
{
    partial class Login
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
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule1 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::ortoxela.WaitForm1), true, true);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl_empresa = new DevExpress.XtraEditors.LabelControl();
            this.gridLookUpEdit_empresa = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.textEditcontraseña = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEditnombre = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleaceptar = new DevExpress.XtraEditors.SimpleButton();
            this.simplecancelar = new DevExpress.XtraEditors.SimpleButton();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit_empresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditcontraseña.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditnombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelControl1.Appearance.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.labelControl_empresa);
            this.panelControl1.Controls.Add(this.gridLookUpEdit_empresa);
            this.panelControl1.Controls.Add(this.checkEdit1);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.simpleaceptar);
            this.panelControl1.Controls.Add(this.simplecancelar);
            this.panelControl1.Location = new System.Drawing.Point(7, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(526, 290);
            this.panelControl1.TabIndex = 1;
            // 
            // labelControl_empresa
            // 
            this.labelControl_empresa.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.labelControl_empresa.Appearance.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_empresa.Appearance.ForeColor = System.Drawing.Color.Teal;
            this.labelControl_empresa.Location = new System.Drawing.Point(15, 8);
            this.labelControl_empresa.Name = "labelControl_empresa";
            this.labelControl_empresa.Size = new System.Drawing.Size(102, 22);
            this.labelControl_empresa.TabIndex = 14;
            this.labelControl_empresa.Text = "Mastetech GT";
            // 
            // gridLookUpEdit_empresa
            // 
            this.gridLookUpEdit_empresa.EditValue = "fsdafads";
            this.gridLookUpEdit_empresa.Location = new System.Drawing.Point(205, 198);
            this.gridLookUpEdit_empresa.Name = "gridLookUpEdit_empresa";
            this.gridLookUpEdit_empresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEdit_empresa.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F2);
            this.gridLookUpEdit_empresa.Properties.View = this.gridLookUpEdit1View;
            this.gridLookUpEdit_empresa.Size = new System.Drawing.Size(281, 20);
            this.gridLookUpEdit_empresa.TabIndex = 13;
            this.gridLookUpEdit_empresa.Visible = false;
            this.gridLookUpEdit_empresa.TextChanged += new System.EventHandler(this.gridLookUpEdit_empresa_TextChanged);
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(71, 198);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "Cambiar de Empresa";
            this.checkEdit1.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style16;
            this.checkEdit1.Size = new System.Drawing.Size(128, 22);
            this.checkEdit1.TabIndex = 12;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged_1);
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.pictureEdit1);
            this.groupControl1.Controls.Add(this.textEditcontraseña);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.textEditnombre);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(15, 36);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(471, 151);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Login";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Location = new System.Drawing.Point(200, 120);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(0, 16);
            this.labelControl3.TabIndex = 6;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureEdit1.EditValue = global::ortoxela.Properties.Resources.LogoMT;
            this.pictureEdit1.Location = new System.Drawing.Point(14, 24);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.pictureEdit1.Size = new System.Drawing.Size(157, 122);
            this.pictureEdit1.TabIndex = 5;
            this.pictureEdit1.EditValueChanged += new System.EventHandler(this.pictureEdit1_EditValueChanged);
            // 
            // textEditcontraseña
            // 
            this.textEditcontraseña.Location = new System.Drawing.Point(278, 91);
            this.textEditcontraseña.Name = "textEditcontraseña";
            this.textEditcontraseña.Properties.PasswordChar = '●';
            this.textEditcontraseña.Size = new System.Drawing.Size(173, 20);
            this.textEditcontraseña.TabIndex = 1;
            conditionValidationRule1.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule1.ErrorText = "Ingrese su contraseña";
            this.dxValidationProvider1.SetValidationRule(this.textEditcontraseña, conditionValidationRule1);
            this.textEditcontraseña.TextChanged += new System.EventHandler(this.textEditcontraseña_TextChanged);
            this.textEditcontraseña.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEditcontraseña_KeyPress);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(190, 94);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(81, 18);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Contraseña";
            // 
            // textEditnombre
            // 
            this.textEditnombre.Location = new System.Drawing.Point(278, 48);
            this.textEditnombre.Name = "textEditnombre";
            this.textEditnombre.Size = new System.Drawing.Size(173, 20);
            this.textEditnombre.TabIndex = 0;
            this.dxValidationProvider1.SetValidationRule(this.textEditnombre, conditionValidationRule1);
            this.textEditnombre.TextChanged += new System.EventHandler(this.textEditnombre_TextChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(190, 47);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(54, 18);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Usuario";
            // 
            // simpleaceptar
            // 
            this.simpleaceptar.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleaceptar.Appearance.Options.UseFont = true;
            this.simpleaceptar.Image = global::ortoxela.Properties.Resources.entrar;
            this.simpleaceptar.Location = new System.Drawing.Point(183, 233);
            this.simpleaceptar.Name = "simpleaceptar";
            this.simpleaceptar.Size = new System.Drawing.Size(142, 42);
            this.simpleaceptar.TabIndex = 2;
            this.simpleaceptar.Text = "Entrar";
            this.simpleaceptar.Click += new System.EventHandler(this.simpleaceptar_Click);
            // 
            // simplecancelar
            // 
            this.simplecancelar.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simplecancelar.Appearance.Options.UseFont = true;
            this.simplecancelar.Image = global::ortoxela.Properties.Resources.salir;
            this.simplecancelar.Location = new System.Drawing.Point(331, 233);
            this.simplecancelar.Name = "simplecancelar";
            this.simplecancelar.Size = new System.Drawing.Size(142, 42);
            this.simplecancelar.TabIndex = 3;
            this.simplecancelar.Text = "Cancelar/Salir";
            this.simplecancelar.Click += new System.EventHandler(this.simplecancelar_Click);
            // 
            // Login
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 298);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "facinvOX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit_empresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditcontraseña.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditnombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEditnombre;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleaceptar;
        private DevExpress.XtraEditors.SimpleButton simplecancelar;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
        private DevExpress.XtraEditors.TextEdit textEditcontraseña;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpEdit_empresa;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl_empresa;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;

    }
}