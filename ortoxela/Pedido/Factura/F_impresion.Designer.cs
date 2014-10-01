namespace ortoxela.Pedido.Factura
{
    partial class F_impresion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_impresion));
            this.crystalReportViewer_impresion = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crystalReportViewer_impresion
            // 
            this.crystalReportViewer_impresion.ActiveViewIndex = -1;
            this.crystalReportViewer_impresion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer_impresion.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer_impresion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer_impresion.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer_impresion.Name = "crystalReportViewer_impresion";
            this.crystalReportViewer_impresion.ShowCloseButton = false;
            this.crystalReportViewer_impresion.ShowGroupTreeButton = false;
            this.crystalReportViewer_impresion.ShowParameterPanelButton = false;
            this.crystalReportViewer_impresion.ShowRefreshButton = false;
            this.crystalReportViewer_impresion.Size = new System.Drawing.Size(784, 561);
            this.crystalReportViewer_impresion.TabIndex = 0;
            this.crystalReportViewer_impresion.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // F_impresion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.crystalReportViewer_impresion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_impresion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impresiones";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_impresion_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer_impresion;
    }
}