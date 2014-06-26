namespace ortoxela.UsuariosBodegasSeries
{
    partial class Form_BodegaUsuario
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
            this.lookUpEdit_usuario = new DevExpress.XtraEditors.LookUpEdit();
            this.dataGridView_bodega = new System.Windows.Forms.DataGridView();
            this.simpleButton_guardar = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.Estado = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_usuario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_bodega)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpEdit_usuario
            // 
            this.lookUpEdit_usuario.Location = new System.Drawing.Point(40, 202);
            this.lookUpEdit_usuario.Name = "lookUpEdit_usuario";
            this.lookUpEdit_usuario.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEdit_usuario.Properties.Appearance.Options.UseFont = true;
            this.lookUpEdit_usuario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_usuario.Size = new System.Drawing.Size(250, 22);
            this.lookUpEdit_usuario.TabIndex = 0;
            this.lookUpEdit_usuario.EditValueChanged += new System.EventHandler(this.lookUpEdit_bodega_EditValueChanged);
            this.lookUpEdit_usuario.TextChanged += new System.EventHandler(this.lookUpEdit_bodega_TextChanged);
            // 
            // dataGridView_bodega
            // 
            this.dataGridView_bodega.AllowUserToAddRows = false;
            this.dataGridView_bodega.AllowUserToDeleteRows = false;
            this.dataGridView_bodega.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_bodega.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_bodega.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Estado,
            this.ID,
            this.Nombre});
            this.dataGridView_bodega.Location = new System.Drawing.Point(313, 46);
            this.dataGridView_bodega.Name = "dataGridView_bodega";
            this.dataGridView_bodega.RowHeadersVisible = false;
            this.dataGridView_bodega.Size = new System.Drawing.Size(618, 383);
            this.dataGridView_bodega.TabIndex = 1;
            // 
            // simpleButton_guardar
            // 
            this.simpleButton_guardar.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton_guardar.Appearance.Options.UseFont = true;
            this.simpleButton_guardar.Location = new System.Drawing.Point(40, 332);
            this.simpleButton_guardar.Name = "simpleButton_guardar";
            this.simpleButton_guardar.Size = new System.Drawing.Size(250, 35);
            this.simpleButton_guardar.TabIndex = 2;
            this.simpleButton_guardar.Text = "Guardar";
            this.simpleButton_guardar.Click += new System.EventHandler(this.simpleButton_guardar_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(40, 180);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(43, 16);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Usuario";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(357, 6);
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
            this.sbCancelar.Location = new System.Drawing.Point(776, 435);
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
            this.ID.DataPropertyName = "codigo_bodega";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 24;
            // 
            // Nombre
            // 
            this.Nombre.DataPropertyName = "nombre_bodega";
            this.Nombre.HeaderText = "Bodega";
            this.Nombre.Name = "Nombre";
            this.Nombre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Nombre.Width = 50;
            // 
            // Form_BodegaUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 490);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton_guardar);
            this.Controls.Add(this.dataGridView_bodega);
            this.Controls.Add(this.lookUpEdit_usuario);
            this.Name = "Form_BodegaUsuario";
            this.Text = "Bodegas - Usuarios";
            this.Load += new System.EventHandler(this.Form_BodegaUsuario_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_usuario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_bodega)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_usuario;
        private System.Windows.Forms.DataGridView dataGridView_bodega;
        private DevExpress.XtraEditors.SimpleButton simpleButton_guardar;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton sbCancelar;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Estado;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
    }
}