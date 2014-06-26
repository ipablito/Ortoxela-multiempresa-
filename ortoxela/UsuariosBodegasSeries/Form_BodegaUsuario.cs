using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ortoxela.UsuariosBodegasSeries
{
    public partial class Form_BodegaUsuario : Form
    {
        public Form_BodegaUsuario()
        {
            InitializeComponent();
        }

        void limpiarcheckbox()
        {
            int valor = dataGridView_bodega.Rows.Count;
            for (int i = 0; i < valor; i++)
            {
                dataGridView_bodega.Rows[i].Cells[0].Value = false;
            }
        }

        private void lookUpEdit_bodega_TextChanged(object sender, EventArgs e)
        {
            labelControl2.Text = "Bodegas a las que tiene acceso el usuario: " + lookUpEdit_usuario.Text;
        }

        private void Form_BodegaUsuario_Load(object sender, EventArgs e)
        {
            DataSet_BodegaUsuarioTableAdapters.Lista_usuariosTableAdapter lg1 = new DataSet_BodegaUsuarioTableAdapters.Lista_usuariosTableAdapter();
            lookUpEdit_usuario.Properties.DisplayMember = "nombre";
            lookUpEdit_usuario.Properties.ValueMember = "userid";
            lookUpEdit_usuario.Properties.DataSource = lg1.GetData_ListaUsuariosActivos();

            DataSet_BodegaUsuarioTableAdapters.ListabodegasTableAdapter lg2 = new DataSet_BodegaUsuarioTableAdapters.ListabodegasTableAdapter();
            dataGridView_bodega.DataSource = lg2.GetData_ListaBodegasActivas();
        }

        private void lookUpEdit_bodega_EditValueChanged(object sender, EventArgs e)
        {
            limpiarcheckbox();


            DataSet_BodegaUsuarioTableAdapters.Bodega_de_UsuarioTableAdapter lg = new DataSet_BodegaUsuarioTableAdapters.Bodega_de_UsuarioTableAdapter();
            DataTable res = new DataTable();
            res = lg.GetData_bodegasdeusuario(Convert.ToInt16(lookUpEdit_usuario.EditValue));

            int lR = res.Rows.Count;
            int lB = dataGridView_bodega.Rows.Count;

            int t1 = 0;
            int t2 = 0;
            for (int i = 0; i < lR; i++)
            {
                t1=Convert.ToInt16(res.Rows[i][1]);
                for (int ii = 0; ii < lB; ii++)
                {
                    t2=Convert.ToInt16(dataGridView_bodega.Rows[ii].Cells[1].Value);
                    if (t1==t2)
                    {
                        dataGridView_bodega.Rows[ii].Cells[0].Value = Convert.ToBoolean(res.Rows[i][2]);
                        ii = lB;
                    }
                }
            }
        }

        private void simpleButton_guardar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                int idusu = Convert.ToInt16(lookUpEdit_usuario.EditValue);
                int idusucrea = Convert.ToInt16(clases.ClassVariables.id_usuario);
                DataSet_BodegaUsuarioTableAdapters.Querys lg = new DataSet_BodegaUsuarioTableAdapters.Querys();
                lg.EliminarBodegasdeunUsuario(idusu);


                int largo = dataGridView_bodega.Rows.Count;

                bool temp = false;
                int idbod = 0;
                for (int i = 0; i < largo; i++)
                {
                    temp = Convert.ToBoolean(dataGridView_bodega.Rows[i].Cells[0].Value);
                    idbod = Convert.ToInt16(dataGridView_bodega.Rows[i].Cells[1].Value);
                    if (temp == true)
                    {
                        lg.guardarbodegasdeusuario(idusu, idbod, idusucrea);
                    }
                }

                clases.ClassMensajes.INSERTO(this);
            }
            catch
            {
                clases.ClassMensajes.NoINSERTO(this);
            }

            this.Cursor = Cursors.Default;
            //lookUpEdit_usuario.Text = "";
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
