namespace ortoxela.Reportes.Ventas
{
    partial class Form_dataGridView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_dataGridView));
            this.dataGridView_mostrar = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mostrarEnExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarEnPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_mostrar)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_mostrar
            // 
            this.dataGridView_mostrar.AllowUserToAddRows = false;
            this.dataGridView_mostrar.AllowUserToDeleteRows = false;
            this.dataGridView_mostrar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView_mostrar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView_mostrar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_mostrar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_mostrar.Location = new System.Drawing.Point(0, 24);
            this.dataGridView_mostrar.Name = "dataGridView_mostrar";
            this.dataGridView_mostrar.ReadOnly = true;
            this.dataGridView_mostrar.Size = new System.Drawing.Size(886, 412);
            this.dataGridView_mostrar.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mostrarEnExcelToolStripMenuItem,
            this.mostrarEnPDFToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(886, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mostrarEnExcelToolStripMenuItem
            // 
            this.mostrarEnExcelToolStripMenuItem.Image = global::ortoxela.Properties.Resources.excel_icon;
            this.mostrarEnExcelToolStripMenuItem.Name = "mostrarEnExcelToolStripMenuItem";
            this.mostrarEnExcelToolStripMenuItem.Size = new System.Drawing.Size(196, 20);
            this.mostrarEnExcelToolStripMenuItem.Text = "Mostrar en un archivo de Excel";
            this.mostrarEnExcelToolStripMenuItem.Click += new System.EventHandler(this.mostrarEnExcelToolStripMenuItem_Click);
            // 
            // mostrarEnPDFToolStripMenuItem
            // 
            this.mostrarEnPDFToolStripMenuItem.Image = global::ortoxela.Properties.Resources.pdf_icon;
            this.mostrarEnPDFToolStripMenuItem.Name = "mostrarEnPDFToolStripMenuItem";
            this.mostrarEnPDFToolStripMenuItem.Size = new System.Drawing.Size(175, 20);
            this.mostrarEnPDFToolStripMenuItem.Text = "Mostrar en un archivo PDF";
            this.mostrarEnPDFToolStripMenuItem.Click += new System.EventHandler(this.mostrarEnPDFToolStripMenuItem_Click);
            // 
            // Form_dataGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 436);
            this.Controls.Add(this.dataGridView_mostrar);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_dataGridView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrtoXela";
            this.Load += new System.EventHandler(this.Form_dataGridView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_mostrar)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_mostrar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mostrarEnExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mostrarEnPDFToolStripMenuItem;
    }
}