using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace ortoxela.Articulos
{
    public partial class F_configurarSistema : Form
    {
        string codigo_sistema;
        classortoxela logica = new classortoxela();
        
        public F_configurarSistema(string Codigo_Nuevo_Sistema)
        {
            InitializeComponent();
            CrearColumnasGrid1();
            labelControl1.Text="Configuracion del Set "+Codigo_Nuevo_Sistema;
            string cs = "SELECT configuracion_equipos_detail.`codigo_articulo` AS Codigo, "+
                        "articulos.`descripcion` AS Descripcion, "+
                        "configuracion_equipos_detail.`cantidad` AS Cantidad FROM configuracion_equipos_detail JOIN articulos " +
                        "ON articulos.`codigo_articulo`=configuracion_equipos_detail.`codigo_articulo` " +
                        "WHERE configuracion_equipos_detail.`codigo_sistema`='"+Codigo_Nuevo_Sistema+"'";
            gridControl_seleccionados.DataSource = logica.Tabla(cs);
            codigo_sistema = Codigo_Nuevo_Sistema;
        }

        

        void CrearColumnasGrid1()
        {
            DataTable temporal = new DataTable();
            temporal.Columns.Add("Codigo");
            temporal.Columns.Add("Descripcion");
            temporal.Columns.Add("Cantidad");
            gridControl_seleccionados.DataSource = temporal;
            gridView1.Columns["Codigo"].OptionsColumn.ReadOnly = true;
        }

        void Agregar()
        {
            string temp_codigo = "";
            string temp_descripcion = "";
            int temp_cantidad = 1;

            temp_codigo = gridView2.GetFocusedRowCellValue("codigo").ToString();
            temp_descripcion = gridView2.GetFocusedRowCellValue("descripcion").ToString();
            temp_cantidad = 1;

            int filas = gridView1.RowCount;
            bool ya_existe = false;

            for (int i = 0; i < filas; i++)
            {
                if (gridView1.GetRowCellValue(i, "Codigo").ToString() == temp_codigo)
                {
                    string ct = gridView1.GetRowCellValue(i, "Cantidad").ToString();
                    ct=ct.Replace(".00", "");
                    int can = Convert.ToInt16(ct);
                    can++;
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Cantidad", can);
                    gridView1.SetRowCellValue(i, "Cantidad", can);
                    ya_existe = true;
                    i = filas;
                }
            }

            if (ya_existe == false)
            {
                gridView1.AddNewRow();
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Codigo", temp_codigo);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Descripcion", temp_descripcion);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Cantidad", temp_cantidad);
                gridView1.UpdateCurrentRow();
            }
        }

        private void F_configurarSistema_Load(object sender, EventArgs e)
        {            
            string cadena = "SELECT codigo,descripcion,existencia FROM v_articulos_cat_lbod WHERE compuesto=0";
            DataTable r = new DataTable();
            r = logica.Tabla(cadena);
            gridControl_todos.DataSource = r;
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Agregar();
        }

        private void gridControl_todos_DoubleClick(object sender, EventArgs e)
        {
            Agregar();
        }

        private void gridControl_todos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                Agregar();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            MySqlConnection conexion = new MySqlConnection(Properties.Settings.Default.ortoxelaConnectionString);
            MySqlCommand comando = new MySqlCommand();
            MySqlTransaction transa;

            string cadena = "";
            string cod = "";
            string cant = "";

            conexion.Open();
            transa = conexion.BeginTransaction();

            cadena = "DELETE FROM configuracion_equipos_detail WHERE codigo_sistema='"+codigo_sistema+"'";
            comando = new MySqlCommand(cadena, conexion);
            comando.Transaction = transa;
            comando.ExecuteNonQuery();

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                cod = gridView1.GetRowCellValue(i, "Codigo").ToString();
                cant = gridView1.GetRowCellValue(i, "Cantidad").ToString();
                cadena = "INSERT INTO configuracion_equipos_detail(codigo_articulo,cantidad,activo,codigo_sistema) " +
                        "VALUES ('"+cod+"',"+cant+",'1','"+codigo_sistema+"');";
                comando = new MySqlCommand(cadena, conexion);
                comando.Transaction = transa;
                comando.ExecuteNonQuery();
            }

            transa.Commit();
            clases.ClassMensajes.INSERTO(this);
            this.Close();
        }

        private void gridControl_seleccionados_DoubleClick(object sender, EventArgs e)
        {
            gridView1.DeleteRow(gridView1.GetSelectedRows()[0]);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
