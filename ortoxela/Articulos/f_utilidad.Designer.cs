namespace ortoxela.Articulos
{
    partial class f_utilidad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_utilidad));
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule1 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule2 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule3 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            this.groupControl_panel = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridLookUpEdit_subcategoria = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridLookUpEdit_categoria = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.textEdit_utilidad = new DevExpress.XtraEditors.TextEdit();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.dxValidationProvider2 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.dxValidationProvider3 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_panel)).BeginInit();
            this.groupControl_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit_subcategoria.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit2View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit_categoria.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_utilidad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl_panel
            // 
            this.groupControl_panel.Controls.Add(this.labelControl1);
            this.groupControl_panel.Controls.Add(this.gridLookUpEdit_subcategoria);
            this.groupControl_panel.Controls.Add(this.gridLookUpEdit_categoria);
            this.groupControl_panel.Controls.Add(this.textEdit_utilidad);
            this.groupControl_panel.Controls.Add(this.labelControl11);
            this.groupControl_panel.Controls.Add(this.radioGroup1);
            this.groupControl_panel.Controls.Add(this.simpleButton2);
            this.groupControl_panel.Location = new System.Drawing.Point(12, 12);
            this.groupControl_panel.Name = "groupControl_panel";
            this.groupControl_panel.Size = new System.Drawing.Size(702, 315);
            this.groupControl_panel.TabIndex = 0;
            this.groupControl_panel.Text = "Utilidades por:";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl1.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Vertical;
            this.labelControl1.Location = new System.Drawing.Point(186, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(506, 48);
            this.labelControl1.TabIndex = 58;
            this.labelControl1.Text = resources.GetString("labelControl1.Text");
            // 
            // gridLookUpEdit_subcategoria
            // 
            this.gridLookUpEdit_subcategoria.Enabled = false;
            this.gridLookUpEdit_subcategoria.Location = new System.Drawing.Point(168, 198);
            this.gridLookUpEdit_subcategoria.Name = "gridLookUpEdit_subcategoria";
            this.gridLookUpEdit_subcategoria.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEdit_subcategoria.Properties.View = this.gridLookUpEdit2View;
            this.gridLookUpEdit_subcategoria.Size = new System.Drawing.Size(276, 20);
            this.gridLookUpEdit_subcategoria.TabIndex = 57;
            conditionValidationRule1.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule1.ErrorText = "Debe seleccionar una subcategoria";
            this.dxValidationProvider3.SetValidationRule(this.gridLookUpEdit_subcategoria, conditionValidationRule1);
            // 
            // gridLookUpEdit2View
            // 
            this.gridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit2View.Name = "gridLookUpEdit2View";
            this.gridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
            // 
            // gridLookUpEdit_categoria
            // 
            this.gridLookUpEdit_categoria.Enabled = false;
            this.gridLookUpEdit_categoria.Location = new System.Drawing.Point(168, 134);
            this.gridLookUpEdit_categoria.Name = "gridLookUpEdit_categoria";
            this.gridLookUpEdit_categoria.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEdit_categoria.Properties.View = this.gridLookUpEdit1View;
            this.gridLookUpEdit_categoria.Size = new System.Drawing.Size(276, 20);
            this.gridLookUpEdit_categoria.TabIndex = 56;
            conditionValidationRule2.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule2.ErrorText = "Debe seleccionar una categoria";
            this.dxValidationProvider2.SetValidationRule(this.gridLookUpEdit_categoria, conditionValidationRule2);
            this.gridLookUpEdit_categoria.TextChanged += new System.EventHandler(this.gridLookUpEdit_categoria_TextChanged);
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // textEdit_utilidad
            // 
            this.textEdit_utilidad.Location = new System.Drawing.Point(592, 167);
            this.textEdit_utilidad.Name = "textEdit_utilidad";
            this.textEdit_utilidad.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit_utilidad.Properties.Appearance.Options.UseFont = true;
            this.textEdit_utilidad.Properties.Mask.EditMask = "p";
            this.textEdit_utilidad.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.textEdit_utilidad.Size = new System.Drawing.Size(100, 22);
            this.textEdit_utilidad.TabIndex = 55;
            conditionValidationRule3.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule3.ErrorText = "Debe ingresa el porcentaje de utilidad";
            this.dxValidationProvider1.SetValidationRule(this.textEdit_utilidad, conditionValidationRule3);
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl11.Location = new System.Drawing.Point(514, 170);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(68, 16);
            this.labelControl11.TabIndex = 54;
            this.labelControl11.Text = "Utilidad (%)";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(19, 46);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "General"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Por categoria"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Por subcategoria")});
            this.radioGroup1.Size = new System.Drawing.Size(117, 203);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = global::ortoxela.Properties.Resources.accept_32x32_32;
            this.simpleButton2.Location = new System.Drawing.Point(455, 261);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(242, 47);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "Guardar y Calcular";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::ortoxela.Properties.Resources.Nuevo32;
            this.simpleButton1.Location = new System.Drawing.Point(205, 333);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(160, 45);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Nuevo";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = global::ortoxela.Properties.Resources.error_32;
            this.simpleButton3.Location = new System.Drawing.Point(394, 333);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(160, 45);
            this.simpleButton3.TabIndex = 2;
            this.simpleButton3.Text = "Cancelar";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // f_utilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 384);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.groupControl_panel);
            this.Name = "f_utilidad";
            this.Text = "Utilidades";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_panel)).EndInit();
            this.groupControl_panel.ResumeLayout(false);
            this.groupControl_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit_subcategoria.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit2View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit_categoria.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_utilidad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl_panel;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.TextEdit textEdit_utilidad;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpEdit_subcategoria;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit2View;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpEdit_categoria;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider2;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}