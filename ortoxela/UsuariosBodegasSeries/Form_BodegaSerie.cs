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
    public partial class Form_BodegaSerie : Form
    {
        public Form_BodegaSerie()
        {
            InitializeComponent();
        }

        void limpiarcheckbox()
        {
            int cont = dataGridView_serie.Rows.Count;
            for (int i = 0; i < cont; i++)
            {
                dataGridView_serie.Rows[i].Cells[0].Value = false;
            }
        }

        private void lookUpEdit_bodega_TextChanged(object sender, EventArgs e)
        {
            labelControl2.Text = "Series de la bodega: " + lookUpEdit_bodega.Text;
        }

        private void Form_BodegaSerie_Load(object sender, EventArgs e)
        {
            DataSet_BodegaUsuarioTableAdapters.ListabodegasTableAdapter lg = new DataSet_BodegaUsuarioTableAdapters.ListabodegasTableAdapter();
            lookUpEdit_bodega.Properties.DisplayMember = "nombre_bodega";
            lookUpEdit_bodega.Properties.ValueMember = "codigo_bodega";
            lookUpEdit_bodega.Properties.DataSource = lg.GetData_ListaBodegasActivas();


            DataSet_BodegaSerieTableAdapters.Lista_series_activasTableAdapter lg1 = new DataSet_BodegaSerieTableAdapters.Lista_series_activasTableAdapter();
            dataGridView_serie.DataSource = lg1.GetData_listaseriesActivas();
        }

        private void lookUpEdit_bodega_EditValueChanged(object sender, EventArgs e)
        {
            limpiarcheckbox();
            DataSet_BodegaSerieTableAdapters.seriesdebodegaTableAdapter lg = new DataSet_BodegaSerieTableAdapters.seriesdebodegaTableAdapter();
            DataTable res = lg.GetData_seriesdeunabodega(Convert.ToInt16(lookUpEdit_bodega.EditValue));

            int Lr = res.Rows.Count;
            int Ld = dataGridView_serie.Rows.Count;

            int t1 = 0;
            int t2 = 0;
            for (int i = 0; i < Lr; i++)
            {
                t1 = Convert.ToInt16(res.Rows[i][1]);
                for (int ii = 0; ii < Ld; ii++)
                {
                    t2 = Convert.ToInt16(dataGridView_serie.Rows[ii].Cells[1].Value);
                    if (t1 == t2)
                    {
                        dataGridView_serie.Rows[ii].Cells[0].Value = Convert.ToBoolean(res.Rows[i][2]);
                        ii = Ld;
                    }
                }
            }


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DataSet_BodegaSerieTableAdapters.Queries lg = new DataSet_BodegaSerieTableAdapters.Queries();
                int idbodega = Convert.ToInt16(lookUpEdit_bodega.EditValue);

                lg.borrarseriesdeunabodega(idbodega);

                bool temp = false;
                int largo = dataGridView_serie.Rows.Count;
                int tempidserie = 0;
                int usuariocreador = Convert.ToInt16(clases.ClassVariables.id_usuario);

                for (int i = 0; i < largo; i++)
                {
                    temp = Convert.ToBoolean(dataGridView_serie.Rows[i].Cells[0].Value);
                    tempidserie = Convert.ToInt16(dataGridView_serie.Rows[i].Cells[1].Value);
                    if (temp == true)
                    {
                        lg.guardarseriesdebodega(idbodega, tempidserie, usuariocreador);
                    }
                }

                clases.ClassMensajes.INSERTO(this);
            }
            catch
            {
                clases.ClassMensajes.NoINSERTO(this);
            }
            this.Cursor = Cursors.Default;
            //lookUpEdit_bodega.Text = "";
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
