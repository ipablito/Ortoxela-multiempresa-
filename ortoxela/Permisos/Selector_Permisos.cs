using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;

namespace ortoxela.Permisos
{
    public partial class Selector_Permisos : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Selector_Permisos()
        {
            InitializeComponent();
        }

        BarButtonItem botoningresa;
        string cadena;
        classortoxela logica = new classortoxela();

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            cadena = "SELECT * FROM permisos";
            DataTable dtpermisos = new DataTable();
            dtpermisos = logica.Tabla(cadena);
            Boolean existe_permiso=true;

            try
            {                
                foreach (object boton in ribbon.Items)
                {
                    existe_permiso = false;
                    if (boton is BarButtonItem)
                    {                        
                        botoningresa = (BarButtonItem)boton;
                        foreach (DataRow fila in dtpermisos.Rows)
                        {
                            if (botoningresa.Name == fila[1].ToString())
                                existe_permiso = true;                           
                        }
                        if (existe_permiso == false)
                        {
                            cadena = "insert into permisos (nombre_permiso) values ('" + botoningresa.Name + "')";
                            logica.variosservios(cadena);
                        }
                    }
                    
                }                
                clases.ClassMensajes.INSERTO(this);
            }
            catch
            {
                clases.ClassMensajes.NoINSERTO(this);
            }
        }

        Dictionary<string, string> dicboton = new Dictionary<string, string>();
        private void controlboton(object sender, ItemClickEventArgs e)
        {
            if (dicboton.ContainsValue(e.Item.Name) == false)
            {
                dicboton.Add(e.Item.Name, e.Item.Name);
            }
            else
            {
                dicboton.Remove(e.Item.Name);
            }
        }

        private void Selector_Permisos_Load(object sender, EventArgs e)
        {
            barStaticItem1.Caption = "USUARIO: " + clases.ClassVariables.NombreComple;
            cadena = "SELECT codigo_rol as CODIGO,nombre_rol AS NOMBRE FROM roles where estadoid<>2";
            gridControl1.DataSource = logica.Tabla(cadena);
        }

        string codigo;
        string nombre;
        DataTable dt = new DataTable();

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            ListBox lista = new ListBox();
            foreach (string fila in dicboton.Values)
            {
                lista.Items.Add(fila);
            }

            for (int x = 0; x < lista.Items.Count; x++)
            {
                foreach (object boton in ribbon.Items)
                {
                    if (boton is BarButtonItem)
                    {
                        botoningresa = (BarButtonItem)boton;
                        if (botoningresa.Name.ToString() == lista.Items[x].ToString())
                        {
                            botoningresa.PerformClick();
                        }
                    }
                }
            }

            codigo = gridView1.GetFocusedRowCellValue("CODIGO").ToString();
            nombre = gridView1.GetFocusedRowCellValue("NOMBRE").ToString();
            labelControl3.Text = nombre;
            ribbon.Enabled = true;
            groupControl1.Enabled = true;
            cadena = "SELECT roles_permisos.codigo_rol,  permisos.nombre_permiso " +
                        "FROM roles_permisos inner join permisos ON roles_permisos.permisoid = permisos.permisoid " +
                        "where roles_permisos.codigo_rol=" + codigo;
            dt = logica.Tabla(cadena);
            foreach (DataRow fila in dt.Rows)
            {
                foreach (object boton in ribbon.Items)
                {
                    if (boton is BarButtonItem)
                    {
                        botoningresa = (BarButtonItem)boton;
                        if (botoningresa.Name == fila[1].ToString())
                        {
                            if (dicboton.ContainsValue(fila[1].ToString()) == false)
                            {
                                botoningresa.PerformClick();
                            }
                        }
                    }
                }
            }

            //activa en los checkedit los permisos ya existentes en la tabla permisos del rol seleccionado
            checkEdit1.Checked = checkEdit2.Checked = checkEdit3.Checked = checkEdit4.Checked = checkEdit5.Checked = false;
            CheckEdit check;
            foreach (DataRow NombreBoton in dt.Rows)
            {
                foreach (Control control in this.groupControl1.Controls)
                {
                    if (control is CheckEdit)
                    {                        
                        check = (CheckEdit)control;
                        if (NombreBoton[1].ToString() == check.Text)
                            check.Checked = true;                        
                    }
                }                 
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                cadena = "DELETE FROM roles_permisos WHERE codigo_rol=" + codigo;
                logica.variosservios(cadena);
                foreach (string permiso in dicboton.Values)
                {
                    cadena = "INSERT into roles_permisos (usuario_creador, codigo_rol, permisoid) " +
                                "VALUES (" + clases.ClassVariables.id_usuario + ", " + codigo + ", (SELECT permisoid FROM permisos where nombre_permiso='" + permiso + "'))";
                    logica.variosservios(cadena);
                }
                clases.ClassMensajes.INSERTO(this);
                simpleButton4.PerformClick();
            }
            catch
            {
                clases.ClassMensajes.NoINSERTO(this);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
                
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CheckEdit check;
            try
            {
                foreach (Control control in this.groupControl1.Controls)
                {
                    if (control is CheckEdit)
                    {
                        check = (CheckEdit)control;
                        if (check.Checked)
                        {
                            cadena = "DELETE FROM roles_permisos WHERE permisoid= (SELECT permisoid FROM permisos where nombre_permiso='" + check.Text + "'and codigo_rol='" + codigo + "')";
                            logica.variosservios(cadena);
                            cadena = "INSERT into roles_permisos (usuario_creador, codigo_rol, permisoid) " +
                                            "VALUES (" + clases.ClassVariables.id_usuario + ", " + codigo + ", (SELECT permisoid FROM permisos where nombre_permiso='" + check.Text + "'))";
                            logica.variosservios(cadena);
                        }
                        else if (check.Checked == false)
                        {
                            cadena = "DELETE FROM roles_permisos WHERE permisoid= (SELECT permisoid FROM permisos where nombre_permiso='" + check.Text + "'and codigo_rol='" + codigo + "')";
                            logica.variosservios(cadena);
                        }
                    }
                }  
                ////Modulo Facturacion
                //if (checkEdit1.Checked)
                //{
                //    cadena = "INSERT into roles_permisos (usuario_creador, codigo_rol, permisoid) " +
                //                    "VALUES (" + clases.ClassVariables.id_usuario + ", " + codigo + ", (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit1.Text + "'))";
                //    logica.variosservios(cadena);
                //}
                //else if (checkEdit1.Checked == false)
                //{
                //    cadena = "DELETE FROM roles_permisos WHERE permisoid= (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit1.Text + "')";
                //    logica.variosservios(cadena);
                //}
                ////Modulo Administracion
                //if (checkEdit2.Checked)
                //{
                //    cadena = "INSERT into roles_permisos (usuario_creador, codigo_rol, permisoid) " +
                //                    "VALUES (" + clases.ClassVariables.id_usuario + ", " + codigo + ", (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit2.Text + "'))";
                //    logica.variosservios(cadena);
                //}
                //else if (checkEdit2.Checked == false)
                //{
                //    cadena = "DELETE FROM roles_permisos WHERE permisoid= (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit2.Text + "')";
                //    logica.variosservios(cadena);
                //}
                ////Modulo Cobranza
                //if (checkEdit3.Checked)
                //{
                //    cadena = "INSERT into roles_permisos (usuario_creador, codigo_rol, permisoid) " +
                //                    "VALUES (" + clases.ClassVariables.id_usuario + ", " + codigo + ", (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit3.Text + "'))";
                //    logica.variosservios(cadena);
                //}
                //else if (checkEdit3.Checked == false)
                //{
                //    cadena = "DELETE FROM roles_permisos WHERE permisoid= (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit3.Text + "')";
                //    logica.variosservios(cadena);
                //}
                ////Modulo Proveedores
                //if (checkEdit4.Checked)
                //{
                //    cadena = "INSERT into roles_permisos (usuario_creador, codigo_rol, permisoid) " +
                //                    "VALUES (" + clases.ClassVariables.id_usuario + ", " + codigo + ", (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit4.Text + "'))";
                //    logica.variosservios(cadena);
                //}
                //else if (checkEdit4.Checked == false)
                //{
                //    cadena = "DELETE FROM roles_permisos WHERE permisoid= (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit4.Text + "')";
                //    logica.variosservios(cadena);
                //}
                ////Modulo Contabilidad
                //if (checkEdit5.Checked)
                //{
                //    cadena = "INSERT into roles_permisos (usuario_creador, codigo_rol, permisoid) " +
                //                    "VALUES (" + clases.ClassVariables.id_usuario + ", " + codigo + ", (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit5.Text + "'))";
                //    logica.variosservios(cadena);
                //}
                //else if (checkEdit5.Checked == false)
                //{
                //    cadena = "DELETE FROM roles_permisos WHERE permisoid= (SELECT permisoid FROM permisos where nombre_permiso='" + checkEdit5.Text + "')";
                //    logica.variosservios(cadena);
                //}
                clases.ClassMensajes.INSERTO(this);
            }
            catch
            {
                clases.ClassMensajes.NoINSERTO(this);
            }
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem114_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        

        


    }
}
