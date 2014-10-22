using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using MySql.Data.MySqlClient;

namespace ortoxela.Principal
{
    public partial class f_logo : DevExpress.XtraEditors.XtraForm
    {
        public f_logo()
        {
            InitializeComponent();
            labelControl_empresa.Text ="Empresa: "+ clases.ClassVariables.nombreEmpresa;


            try
            {
                pictureEdit1.Image = CargarImagen();
            }
            catch
            { }
        }

        private void f_logo_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Archivos de imagen | *.jpg";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textEdit_direccion.Text = openFileDialog1.FileName;
                    pictureEdit1.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        static MySqlConnection GetNewConnection()
        {
            var conn = new MySqlConnection();
            try
            {
                conn = new MySqlConnection(clases.ClassVariables.ConexionMaster);
                conn.Open();
            }
            catch
            {
                conn = new MySqlConnection(clases.ClassVariables.ConexionMasterRemoto);
                conn.Open();
            }
            return conn;
        }

        static void GuardarImagen(Image imagen)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imgArr = ms.ToArray();
                using (MySqlConnection conn = GetNewConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "update empresa set logo=@imgArr where idempresa="+clases.ClassVariables.idEmpresa;
                        cmd.Parameters.AddWithValue("@imgArr", imgArr);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }


        static Image CargarImagen()
        {
            using (MySqlConnection conn = GetNewConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT logo FROM empresa WHERE idempresa = "+clases.ClassVariables.idEmpresa;
                    byte[] imgArr = (byte[])cmd.ExecuteScalar();
                    imgArr = (byte[])cmd.ExecuteScalar();
                    using (var stream = new MemoryStream(imgArr))
                    {
                        Image img = Image.FromStream(stream);
                        return img;
                    }
                }
            }
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                GuardarImagen(pictureEdit1.Image);
                clases.ClassMensajes.INSERTO(this);
                this.Close();
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

        private void textEdit_direccion_KeyDown(object sender, KeyEventArgs e)
        {
            simpleButton1.PerformClick();
        }

        
    }
}