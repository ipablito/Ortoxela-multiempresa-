using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using integracion_ii;
using Sisnova.Invex.BL;
using System.IO;

namespace ortoxela
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
        }

        

        classortoxela logica = new classortoxela();
        string cadena;




        private void simplecancelar_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }
        DataTable tabla=new DataTable();
        private void simpleaceptar_Click(object sender, EventArgs e)
        {
            if (dxValidationProvider1.Validate())
            {
                splashScreenManager1.ShowWaitForm();
                cadena = "SELECT userid, nombre, apellido,mostrar_ventas " +
                            "FROM usuarios where username='" + textEditnombre.Text + "' and pasword='" + logica.encripta(textEditcontraseña.Text) + "' and estadoid<>2";
                tabla = logica.Tabla(cadena);
                if (tabla.Rows.Count == 1)
                {
                    
                    bool r = false;
                    string b=tabla.Rows[0][3].ToString();//es para verificar si el valor "mostrar ventas" es verdadero o falso, los usuarios de ellos si tienen que aparecer, los d noosotros no es necesario q existan en invex
                    if (b!= "0")
                    {
                        #region integracion con INVEX, logeo de usuario
                        try
                        {
                            
                            r = integracion_ii.Class_globales.Connect(textEditnombre.Text);
                        }
                        catch
                        {
                        }

                        if (r == false)
                        {
                            splashScreenManager1.CloseWaitForm();
                            textEditnombre.Focus();
                            MessageBox.Show("Este usuario no existe, o puede que este deshabilitado en INVEX ERP, verifique porfavor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Class_integracion.logeado = false;
                            return;
                            //MessageBox.Show("Actualmente estas operaciones no se registraran en INVEX ERP", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //Class_integracion.logeado = false;
                        }
                        else
                        {
                            Class_integracion.logeado = true;
                            alertControl1.Show(this, "Login", "Las operaciones que usted realize tambien seran cargadas a INVEX ERP", Properties.Resources.accept_32x32_32);
                        }

                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("Con este usuario las operaciones que usted haga no se registraran en INVEX ERP", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Class_integracion.logeado = false;
                    }

                    foreach (DataRow fila in tabla.Rows)
                    {
                        clases.ClassVariables.id_usuario = fila[0].ToString();
                        clases.ClassVariables.NombreComple = fila[1].ToString() + " " + fila[2].ToString();
                    }
                    cadena = "SELECT r.codigo_rol FROM rol_usuario r WHERE r.estadoid=1 and r.userid=" + clases.ClassVariables.id_usuario;
                    DataTable dt_rol = new DataTable();
                    dt_rol = logica.Tabla(cadena);
                    clases.ClassVariables.id_rol = dt_rol.Rows[0][0].ToString();

                    // cadena = " SELECT Sucursal  FROM master.sucursales  WHERE IDSuc=1 ";
                    // DataTable dt_empresa = new DataTable();
                    // dt_empresa = logica.Tabla(cadena);
                    clases.ClassVariables.nombreEmpresa = "OrtoXela"; //dt_empresa.Rows[0][0].ToString();
                    
                    textEditcontraseña.Text = "";
                    textEditnombre.Text = "";
                    textEditnombre.Focus();
                    labelControl3.Text = "";
                    Form nuevo = new Principal.Principal();
                    alertControl1.Show(this, "Inicio Sesión", "Usted es " + clases.ClassVariables.NombreComple, Properties.Resources.sesion);
                    this.Hide();
                    splashScreenManager1.CloseWaitForm();
                    nuevo.Show();
                }
                else
                {
                    splashScreenManager1.CloseWaitForm();
                    labelControl3.Text = "El Usuario o La Contraseña son incorrectos";
                    textEditnombre.Focus();
                }
            }
            else
            {
                clases.ClassMensajes.FaltanDatosEnCampos(this);
            }            
        }



        private void Login_Load(object sender, EventArgs e)
        {
            #region temas de devexpress

            /* registrando los skins*/
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();




            string tema = "";
            StreamReader leer = new StreamReader("tema.txt");

            while (!leer.EndOfStream)
            {
                tema = leer.ReadLine().ToString();
            }

            leer.Close();




            Properties.Settings.Default.mascara = tema;
            Properties.Settings.Default.Save();
            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(tema);
            #endregion
            textEditnombre.Focus();
        }

        private void textEditcontraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                simpleaceptar.PerformClick();
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Show();
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
       
        }
    }
