using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ortoxela.Articulos
{
    public partial class f_utilidad : DevExpress.XtraEditors.XtraForm
    {

        classortoxela logica = new classortoxela();

        public f_utilidad()
        {
            InitializeComponent();
            nuevo();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void nuevo()
        {
            groupControl_panel.Enabled = true;
            radioGroup1.Focus();
            gridLookUpEdit_categoria.Text = "";
            gridLookUpEdit_subcategoria.Text = "";
            textEdit_utilidad.Text = "";
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                gridLookUpEdit_categoria.Enabled = false;
                gridLookUpEdit_subcategoria.Enabled = false;
            }
            else if (radioGroup1.SelectedIndex == 1)
            {
                try
                {
                    gridLookUpEdit_categoria.Text = "";
                    gridLookUpEdit_categoria.Enabled = true;
                    gridLookUpEdit_subcategoria.Enabled = false;

                    DataTable dt = new DataTable();
                    dt = logica.Tabla("SELECT codigo_categoria 'Codigo', nombre_categoria 'Nombre' FROM categorias WHERE estadoid=1");
                    gridLookUpEdit_categoria.Properties.DisplayMember = "Nombre";
                    gridLookUpEdit_categoria.Properties.ValueMember = "Codigo";
                    gridLookUpEdit_categoria.Properties.DataSource = dt;
                }
                catch
                { }
            }
            else if (radioGroup1.SelectedIndex == 2)
            {
                gridLookUpEdit_categoria.Text = "";
                gridLookUpEdit_subcategoria.Text = "";
                gridLookUpEdit_categoria.Enabled = true;
                gridLookUpEdit_subcategoria.Enabled = true;

                DataTable dt = new DataTable();
                dt = logica.Tabla("SELECT codigo_categoria 'Codigo', nombre_categoria 'Nombre' FROM categorias WHERE estadoid=1");
                gridLookUpEdit_categoria.Properties.DisplayMember = "Nombre";
                gridLookUpEdit_categoria.Properties.ValueMember = "Codigo";
                gridLookUpEdit_categoria.Properties.DataSource = dt;
            }

        }

        private void gridLookUpEdit_categoria_TextChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 2)
            {
                try
                {
                    gridLookUpEdit_subcategoria.Text = "";
                    DataTable dt = new DataTable();
                    dt = logica.Tabla("SELECT codigo_subcat 'Codigo', nombre_subcategoria 'Nombre' FROM sub_categorias WHERE codigo_categoria="+gridLookUpEdit_categoria.EditValue.ToString()+" AND estadoid=1");
                    gridLookUpEdit_subcategoria.Properties.DisplayMember = "Nombre";
                    gridLookUpEdit_subcategoria.Properties.ValueMember = "Codigo";
                    gridLookUpEdit_subcategoria.Properties.DataSource = dt;
                }
                catch { }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0 && dxValidationProvider1.Validate() == true)
            {
                string consulta = "update articulos set utilidad=" + textEdit_utilidad.EditValue.ToString() + " where estadoid=1";
                logica.variosservios(consulta);
                groupControl_panel.Enabled = false;
                clases.ClassMensajes.INSERTO(this);
            }

            else if (radioGroup1.SelectedIndex == 1 && dxValidationProvider1.Validate() == true)
            {
                if (dxValidationProvider2.Validate() == true)
                {
                    string consulta = "UPDATE articulos art SET art.utilidad=" + textEdit_utilidad.EditValue.ToString() + " " +
"WHERE " +
"(SELECT sc.`codigo_categoria` FROM sub_categorias sc WHERE art.`codigo_categoria`=sc.`codigo_subcat`)=" + gridLookUpEdit_categoria.EditValue.ToString();


                    logica.variosservios(consulta);
                    groupControl_panel.Enabled = false;
                    clases.ClassMensajes.INSERTO(this);
                }
            }

            else if (radioGroup1.SelectedIndex == 2 && dxValidationProvider1.Validate() == true)
            {
                if (dxValidationProvider2.Validate() == true)
                {
                    if (dxValidationProvider3.Validate() == true)
                    {
                        string consulta = "update articulos art set art.utilidad=" + textEdit_utilidad.EditValue.ToString() + " where art.codigo_categoria=" + gridLookUpEdit_subcategoria.EditValue.ToString();
                        logica.variosservios(consulta);
                        groupControl_panel.Enabled = false;
                        clases.ClassMensajes.INSERTO(this);
                    }
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            nuevo();
        }
    }
}