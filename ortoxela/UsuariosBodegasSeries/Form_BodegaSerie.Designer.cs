namespace ortoxela.UsuariosBodegasSeries
{
    partial class Form_BodegaSerie
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
            this.dataGridView_serie = new System.Windows.Forms.DataGridView();
            this.lookUpEdit_bodega = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.Estado = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoDeDocumento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Serie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_serie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_bodega.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_serie
            // 
            this.dataGridView_serie.AllowUserToAddRows = false;
            this.dataGridView_serie.AllowUserToDeleteRows = false;
            this.dataGridView_serie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_serie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_serie.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Estado,
            this.ID,
            this.TipoDeDocumento,
            this.Serie});
            this.dataGridView_serie.Location = new System.Drawing.Point(334, 40);
            this.dataGridView_serie.Name = "dataGridView_serie";
            this.dataGridView_serie.RowHeadersVisible = false;
            this.dataGridView_serie.Size = new System.Drawing.Size(590, 409);
            this.dataGridView_serie.TabIndex = 0;
            // 
            // lookUpEdit_bodega
            // 
            this.lookUpEdit_bodega.Location = new System.Drawing.Point(30, 207);
            this.lookUpEdit_bodega.Name = "lookUpEdit_bodega";
            this.lookUpEdit_bodega.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_bodega.Properties.Appearance.Options.UseFont = true;
            this.lookUpEdit_bodega.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_bodega.Size = new System.Drawing.Size(255, 22);
            this.lookUpEdit_bodega.TabIndex = 1;
            this.lookUpEdit_bodega.EditValueChanged += new System.EventHandler(this.lookUpEdit_bodega_EditValueChanged);
            this.lookUpEdit_bodega.TextChanged += new System.EventHandler(this.lookUpEdit_bodega_TextChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(30, 183);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(49, 18);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Bodega";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(30, 368);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(255, 44);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "Guardar";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(334, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(0, 16);
            this.labelControl2.TabIndex = 4;
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.sbCancelar.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sbCancelar.Appearance.Options.UseFont = true;
            this.sbCancelar.Image = global::ortoxela.Properties.Resources.window_remove_32x32_32;
            this.sbCancelar.Location = new System.Drawing.Point(793, 455);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(155, 43);
            this.sbCancelar.TabIndex = 13;
            this.sbCancelar.Text = "Cancelar/Salir";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // Estado
            // 
            this.Estado.HeaderText = "Estado";
            this.Estado.Name = "Estado";
            this.Estado.Width = 46;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "codigo_serie";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 24;
            // 
            // TipoDeDocumento
            // 
            this.TipoDeDocumento.DataPropertyName = "nombre_documento";
            this.TipoDeDocumento.HeaderText = "Tipo De Documento";
            this.TipoDeDocumento.Name = "TipoDeDocumento";
            this.TipoDeDocumento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TipoDeDocumento.Width = 98;
            // 
            // Serie
            // 
            this.Serie.DataPropertyName = "serie_documento";
            this.Serie.HeaderText = "Serie De Documento";
            this.Serie.Name = "Serie";
            this.Serie.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Serie.Width = 101;
            // 
            // Form_BodegaSerie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 505);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lookUpEdit_bodega);
            this.Controls.Add(this.dataGridView_serie);
            this.Name = "Form_BodegaSerie";
            this.Text = "Bodegas - Series";
            this.Load += new System.EventHandler(this.Form_BodegaSerie_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_serie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_bodega.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_serie;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_bodega;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton sbCancelar;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Estado;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoDeDocumento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Serie;
    }
}