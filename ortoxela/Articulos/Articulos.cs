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
    public partial class Articulos : DevExpress.XtraEditors.XtraForm
    {
        public Articulos()
        {
            InitializeComponent();
        }
        classortoxela logica = new classortoxela();
        DataTable dt = new DataTable();
        public static bool BanderaLlamada;
        public static string id_articulo,nombre_articulo,precio_costo,precio_venta;
        string cadena; bool llamadentroform;
        string temo_id_articulao;

        bool ConfigurarSistema = false;
        bool BuscarSistema = false;

        public void Sistema()
        {
            this.Text = "Set";
            this.groupControl1.Text = "Informacion del Set";
            checkEditCompu.Checked = true;
            BuscarSistema = true;
            ConfigurarSistema = true;

            textEdit_utilidad.Visible = false;
            textEdit_utilidad.Text = "0";
            labelControl11.Visible = false;
        }


        private void Proveedor_Load(object sender, EventArgs e)
        {
               llamadentroform = clases.ClassVariables.llamadoDentroForm;
                if (clases.ClassVariables.bandera == 1)
                {
                    bandera = 1;
                    simpleaceptar.Text = "Aceptar";
                    simpleaceptar.Image = Properties.Resources.database_add_24x24_32;
                    simpleButton1.Text = "Nuevo";
                    simpleButton1.Image = Properties.Resources.add_32x32_32;
                    groupControl1.Enabled = true;
                    simpleaceptar.Enabled = true;
                    llenacombos();
                    limpiar();
                }
                else
                {
                    if (clases.ClassVariables.bandera == 2)
                    {
                        bandera = 2;

                        simpleaceptar.Text = "Modificar";
                        simpleaceptar.Image = Properties.Resources.database_process_24x24_32;
                        simpleButton1.Text = "Buscar...";
                        simpleButton1.Image = Properties.Resources._027_folder_search;

                        if (BuscarSistema == false)
                            busca_mod_eli();
                        else
                            busca_mod_eli_sistema();

                        temo_id_articulao = textEditcodigo.Text;
                    }
                    else
                    {
                        if (clases.ClassVariables.bandera == 3)
                        {
                            bandera = 3;
                            simpleaceptar.Text = "Eliminar";
                            simpleaceptar.Image = Properties.Resources.database_remove_24x24_32;
                            simpleButton1.Text = "Buscar...";
                            simpleButton1.Image = Properties.Resources._027_folder_search;

                            if (BuscarSistema == false)
                                busca_mod_eli();
                            else
                                busca_mod_eli_sistema();

                        }
                    }
                }

        }

        string Codigo_Control = "";
        private void busca_mod_eli()
        {
            clases.ClassVariables.cadenabusca = "SELECT * FROM v_articulos_cat_lbod WHERE compuesto=0  ";
                                                         
            Form busca = new Buscador.Buscador();
            busca.ShowDialog();
            if (clases.ClassVariables.id_busca != "")
            {
                llenacombos();
                groupControl1.Enabled = true;
                simpleaceptar.Enabled = true;
                cadena = "CALL sp_obtener_articulo_categoria('" + clases.ClassVariables.id_busca + "');";
                DataTable dt = new DataTable();
                dt = logica.Tabla(cadena);
                foreach (DataRow fila in dt.Rows)
                {
                                                                 
                        textEditcodigo.Text = fila[0].ToString();
                        
                    Codigo_Control = textEditcodigo.Text;
                        
                        gridLookmarca.EditValue = fila[1].ToString();
                        gridLookcategoria.EditValue = fila[14].ToString();    
                        gridLooksubcategoria.EditValue = fila[2].ToString();
                        memoEditdescripcion.Text = fila[3].ToString();
                        texteditpcosto.Text = fila[4].ToString();
                        textEditpventa.Text = fila[5].ToString();
                        textEditpmin.Text = fila[6].ToString();
                        textEditpmax.Text = fila[7].ToString();
                        textEditserie.Text = fila[8].ToString();
                        texteditmodelo.Text = fila[9].ToString();
                        memoEditcomentario.Text = fila[10].ToString();
                        gridLookUpEstado.EditValue = fila[11].ToString();
                        checkEditCompu.Checked = false;
                        //gridLookupartpadre.EditValue = fila[13].ToString();


                    //utilidad 
                        textEdit_utilidad.Text = fila["utilidad"].ToString();
                    }
            }
            else
            {
                groupControl1.Enabled = false;
                simpleaceptar.Enabled = false;
            }
        }

        private void busca_mod_eli_sistema()
        {
            clases.ClassVariables.cadenabusca = "SELECT * FROM v_articulos_cat_lbod WHERE compuesto=1 ";

            Form busca = new Buscador.Buscador();
            busca.ShowDialog();
            if (clases.ClassVariables.id_busca != "")
            {
                llenacombos();
                groupControl1.Enabled = true;
                simpleaceptar.Enabled = true;
                cadena = "CALL sp_obtener_articulo_categoria('" + clases.ClassVariables.id_busca + "');";
                DataTable dt = new DataTable();
                dt = logica.Tabla(cadena);
                foreach (DataRow fila in dt.Rows)
                {

                    textEditcodigo.Text = fila[0].ToString();

                    Codigo_Control = textEditcodigo.Text;

                    gridLookmarca.EditValue = fila[1].ToString();
                    gridLookcategoria.EditValue = fila[14].ToString();
                    gridLooksubcategoria.EditValue = fila[2].ToString();
                    memoEditdescripcion.Text = fila[3].ToString();
                    texteditpcosto.Text = fila[4].ToString();
                    textEditpventa.Text = fila[5].ToString();
                    textEditpmin.Text = fila[6].ToString();
                    textEditpmax.Text = fila[7].ToString();
                    textEditserie.Text = fila[8].ToString();
                    texteditmodelo.Text = fila[9].ToString();
                    memoEditcomentario.Text = fila[10].ToString();
                    gridLookUpEstado.EditValue = fila[11].ToString();
                    checkEditCompu.Checked = true;
                    //gridLookupartpadre.EditValue = fila[13].ToString();
                }
            }
            else
            {
                groupControl1.Enabled = false;
                simpleaceptar.Enabled = false;
            }
        }

        public void limpiar()
        {
            textEditcodigo.Text = "";
            memoEditdescripcion.Text = "";
            texteditpcosto.Text = "";
            textEditpventa.Text = "";
            textEditpmin.Text = "";
            textEditpmax.Text = "";
            textEditserie.Text = "";
            texteditmodelo.Text = "";
            //gridLookupartpadre.Text = "";
            memoEditcomentario.Text = "";
            //checkEditCompu.Checked = false;
            textEditcodigo.Focus();
        }
                

       private void simpleButtonEstado_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            Form prov = new Estado.Estado();
            prov.WindowState = System.Windows.Forms.FormWindowState.Normal;
            prov.ShowDialog();
            if (clases.ClassVariables.idnuevo != null)
            {
                cadena = "SELECT estadoid as CODIGO, nombre_status as NOMBRE FROM estado where activo=1";
                gridLookUpEstado.Properties.DataSource = logica.Tabla(cadena);
                gridLookUpEstado.Properties.ValueMember = "CODIGO";
                gridLookUpEstado.Properties.DisplayMember = "NOMBRE";
                gridLookUpEstado.Text = "";
                gridLookUpEstado.EditValue = clases.ClassVariables.idnuevo;
            }
           }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        int bandera;
        private void simpleaceptar_Click(object sender, EventArgs e)
        {
            if (dxValidationProvider1.Validate())
            {
                if (Convert.ToDouble(texteditpcosto.Text)<=Convert.ToDouble(textEditpventa.Text))
                {
                    if (Convert.ToDouble(textEditpmin.Text)<=Convert.ToDouble(textEditpmax.Text))
                    {
                        string codigo_padre, codigo_padredesc, codigo_marca;
                        //if (gridLookupartpadre.EditValue.ToString() != "")
                        //{
                        //    codigo_padre = "'" + gridLookupartpadre.EditValue.ToString() + "'";
                        //    codigo_padredesc = gridLookupartpadre.EditValue.ToString();
                        //}
                        //else
                        //{
                            codigo_padre = "null";
                            codigo_padredesc = "null";
                        //}

                        if (gridLookmarca.EditValue.ToString() != "")
                            codigo_marca = gridLookmarca.EditValue.ToString();
                        else
                            codigo_marca = "null";

                        if (bandera == 2)
                        {
                            if(temo_id_articulao!=textEditcodigo.Text.ToString())
                            cadena = "SELECT articulos.codigo_articulo FROM articulos where codigo_articulo='" + textEditcodigo.Text + "'";
                            else
                                cadena = "SELECT articulos.codigo_articulo FROM articulos where codigo_articulo='" + textEditcodigo.Text + "d4#z'";
                        }
                        else
                            cadena = "SELECT articulos.codigo_articulo FROM articulos where codigo_articulo='" + textEditcodigo.Text + "'";

                        
                        

                            if (bandera == 1)
                            {
                                if (logica.ExisteRegistro(cadena) == true)
                                {
                                    alertControl1.Show(this, "Cuidado", "Este codigo de producto ya existe", Properties.Resources.Advertencia48);

                                    //aca vamos a ver que hacemos para eliminar los espacios en los codigos
                                    DataSet_articuloTableAdapters.filtrocodigoTableAdapter lg = new DataSet_articuloTableAdapters.filtrocodigoTableAdapter();
                                    DataTable res = new DataTable();
                                    res = lg.GetData_filtrarcodigodearticulo(textEditcodigo.Text);
                                    MessageBox.Show("El codigo " + res.Rows[0][0].ToString() + " ya existe, con la descripcion: " + res.Rows[0][1].ToString() + " y actualmente esta en el estado: " + res.Rows[0][2].ToString(), "Producto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    textEditcodigo.Focus();
                                }
                                else
                                {

                                    if (codigo_padre == "null")
                                    {

                                        cadena = "INSERT into articulos " +
                                                "(codigo_articulo, codigo_marca, codigo_categoria, descripcion, fecha_compra, costo, precio_venta, minimo, maximo, numero_serie, modelo, comentario, usuario_ingresa, estadoid,compuesto, codigo_padre,utilidad) " +
                                                "VALUES ('" + textEditcodigo.Text + "', " + codigo_marca + ", " + gridLooksubcategoria.EditValue + ", '" + memoEditdescripcion.Text + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + texteditpcosto.Text + ", " + textEditpventa.Text + ", " +
                                                "" + textEditpmin.Text + ", " + textEditpmax.Text + ", '" + textEditserie.Text + "', '" + texteditmodelo.Text + "', '" + memoEditcomentario.Text + "', " + clases.ClassVariables.id_usuario + "," + gridLookUpEstado.EditValue + "," + checkEditCompu.Checked + ", " + codigo_padre + ","+textEdit_utilidad.EditValue.ToString()+")";
                                    }
                                    else
                                    {
                                        cadena = "INSERT into articulos " +
                                                "(codigo_articulo, codigo_marca, codigo_categoria,descripcion, fecha_compra, costo, precio_venta, minimo, maximo, numero_serie, modelo, comentario, usuario_ingresa, estadoid,compuesto, codigo_padre,utilidad) " +
                                                "VALUES ('" + textEditcodigo.Text + "', " + codigo_marca + ", " + gridLooksubcategoria.EditValue + ", '" + (memoEditdescripcion.Text + "[" + codigo_padredesc + "]") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + texteditpcosto.Text + ", " + textEditpventa.Text + ", " +
                                                "" + textEditpmin.Text + ", " + textEditpmax.Text + ", '" + textEditserie.Text + "', '" + texteditmodelo.Text + "', '" + memoEditcomentario.Text + "', " + clases.ClassVariables.id_usuario + "," + gridLookUpEstado.EditValue + "," + checkEditCompu.Checked + ", " + codigo_padre + "," + textEdit_utilidad.EditValue.ToString() + ")";
                                    }
                                    if (clases.ClassMensajes.INSERTO(this, cadena))
                                    {
                                        if (ConfigurarSistema == true)
                                        {
                                            F_configurarSistema nf = new F_configurarSistema(textEditcodigo.Text);
                                            nf.ShowDialog();
                                        }




                                        groupControl1.Enabled = false;
                                        simpleaceptar.Enabled = false;

                                        id_articulo = textEditcodigo.Text;
                                        nombre_articulo = memoEditdescripcion.Text;
                                        precio_costo = texteditpcosto.Text;
                                        precio_venta = textEditpventa.Text;
                                        BanderaLlamada = true;

                                        if (llamadentroform == true)
                                        {
                                            llamadentroform = false;
                                            this.Close();
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (bandera == 2)
                                {
                                    DialogResult res=DialogResult.Yes;
                                    if (Codigo_Control != textEditcodigo.Text)
                                    {
                                        res = MessageBox.Show("Esta a punto de cambiar el codigo " + Codigo_Control + " por el codigo " + textEditcodigo.Text + ", este cambio afectara a todas las operaciones que haya tenido este articulo.\n\nDesea continuar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                    }

                                    if ((Codigo_Control != textEditcodigo.Text) && (res == DialogResult.Yes))
                                    {
                                        cadena = "select * from articulos where codigo_articulo='" + textEditcodigo.Text + "';";

                                        DataTable tt = new DataTable();
                                        tt = logica.Tabla(cadena);
                                        if (tt.Rows.Count > 0)
                                        {
                                            MessageBox.Show("Este codigo ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            res = DialogResult.No;
                                        }
                                    }

                                    if (res == DialogResult.Yes)
                                    {
                                            cadena = "update articulos " +
                                                        "SET codigo_articulo='"+textEditcodigo.Text+"', codigo_marca = " + codigo_marca + ", codigo_categoria = " + gridLooksubcategoria.EditValue + ", descripcion = '" + memoEditdescripcion.Text + "', " +
                                                    "costo = " + texteditpcosto.Text + ", precio_venta = " + textEditpventa.Text + ", minimo = " + textEditpmin.Text + ", maximo = " + textEditpmax.Text + ", numero_serie = '" + textEditserie.Text + "', modelo = '" + texteditmodelo.Text + "', comentario = '" + memoEditcomentario.Text + "', " +
                                                    "usuario_modifica = " + clases.ClassVariables.id_usuario + ", estadoid = " + gridLookUpEstado.EditValue + ", compuesto = " + checkEditCompu.Checked + ",codigo_padre = " + codigo_padre + " " +
                                                    " ,utilidad="+textEdit_utilidad.EditValue.ToString()+" "+
                                                    "WHERE articulos.codigo_articulo= '" + Codigo_Control + "'";

                                            if (clases.ClassMensajes.MODIFICAR(this, cadena))
                                            {
                                                groupControl1.Enabled = false;
                                                simpleaceptar.Enabled = false;
                                            }
                                    }


                                }


                                else
                                {
                                    if (bandera == 3)
                                    {

                                        cadena = "SELECT bodegas.existencia_articulo FROM articulos inner join bodegas on bodegas.codigo_articulo= articulos.codigo_articulo " +
                                                    "where bodegas.existencia_articulo>0 and articulos.codigo_articulo='" + clases.ClassVariables.id_busca + "'";
                                        if (logica.ExisteRegistro(cadena) == false)
                                        {
                                            cadena = "update articulos SET fecha_baja = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',estadoid = 2 WHERE articulos.codigo_articulo='" + clases.ClassVariables.id_busca + "'";
                                            if (clases.ClassMensajes.ELIMINAR(this, cadena))
                                            {
                                                groupControl1.Enabled = false;
                                                simpleaceptar.Enabled = false;
                                            }

                                        }
                                        else
                                        {
                                            alertControl1.Show(this, "Cuidado", "No se puede eliminar este producto ya que aun tiene productos en bodega", Properties.Resources.Advertencia48);
                                        }
                                    }
                                }
                            
                        }
                    }
                    else
                    {
                        alertControl1.Show(this, "Cuidado", "La Existencia minima debe ser menor a la existencia maxima", Properties.Resources.Advertencia48);
                    }
                }
                else
                {
                    alertControl1.Show(this, "Cuidado", "El costo debe ser menor al precio de venta", Properties.Resources.Advertencia48);
                }
                }
                else
                {
                    clases.ClassMensajes.FaltanDatosEnCampos(this);

                }
            
        
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (bandera == 1)
            {
                groupControl1.Enabled = true;
                simpleaceptar.Enabled = true;
                limpiar();
            }
            else
            {
                if (bandera == 2 || bandera == 3)
                {
                    //busca_mod_eli();
                    if (BuscarSistema == false)
                        busca_mod_eli();
                    else
                        busca_mod_eli_sistema();
                }
            }
        }
        private void llenacombos()
        {
            cadena = "SELECT estadoid as CODIGO, nombre_status as NOMBRE FROM estado where activo=1";
            gridLookUpEstado.Properties.DataSource = logica.Tabla(cadena);
            gridLookUpEstado.Properties.ValueMember = "CODIGO";
            gridLookUpEstado.Properties.DisplayMember = "NOMBRE";
            gridLookUpEstado.Text = "";
           gridLookUpEstado.EditValue= 1;
            cadena = "SELECT codigo_categoria as CODIGO, nombre_categoria AS NOMBRE FROM categorias WHERE estadoid<>2";
            gridLookcategoria.Properties.DataSource = logica.Tabla(cadena);
            gridLookcategoria.Properties.ValueMember = "CODIGO";
            gridLookcategoria.Properties.DisplayMember = "NOMBRE";
            gridLookcategoria.Text = "";
            gridLooksubcategoria.Enabled = false;
            simpleButtonsubcategoria.Enabled = false;
            cadena = "SELECT codigo_marca as CODIGO, nombre_marca AS NOMBRE FROM marcas WHERE estadoid<>2";
            gridLookmarca.Properties.DataSource = logica.Tabla(cadena);
            gridLookmarca.Properties.ValueMember = "CODIGO";
            gridLookmarca.Properties.DisplayMember = "NOMBRE";
            gridLookmarca.Text = "";
            cadena = "SELECT  articulos.codigo_articulo as CODIGO,descripcion as DESCRIPCION " +
                                                 "FROM articulos WHERE articulos.estadoid<>2 and compuesto=1";
            //gridLookupartpadre.Properties.DataSource = logica.Tabla(cadena);
            //gridLookupartpadre.Properties.ValueMember = "CODIGO";
            //gridLookupartpadre.Properties.DisplayMember = "DESCRIPCION";
            //gridLookupartpadre.Text = "";                                             
            
          }

        
        

        private void simpleButtonmarca_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            Form prov = new Marcas();
            prov.WindowState = System.Windows.Forms.FormWindowState.Normal;
            prov.ShowDialog();
            if (clases.ClassVariables.idnuevo != null)
            {
                cadena = "SELECT codigo_marca as CODIGO, nombre_marca AS NOMBRE FROM marcas WHERE estadoid<>2";
                gridLookmarca.Properties.DataSource = logica.Tabla(cadena);
                gridLookmarca.Properties.ValueMember = "CODIGO";
                gridLookmarca.Properties.DisplayMember = "NOMBRE";
                gridLookmarca.Text = "";
                gridLookmarca.EditValue = clases.ClassVariables.idnuevo;
            }
            }

        private void simpleButtoncategoria_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            Form prov = new Categorias();
            prov.WindowState = System.Windows.Forms.FormWindowState.Normal;
            prov.ShowDialog();
            if (clases.ClassVariables.idnuevo != null)
            {
                cadena = "SELECT codigo_categoria as CODIGO, nombre_categoria AS NOMBRE FROM categorias WHERE estadoid<>2";
                gridLookcategoria.Properties.DataSource = logica.Tabla(cadena);
                gridLookcategoria.Properties.ValueMember = "CODIGO";
                gridLookcategoria.Properties.DisplayMember = "NOMBRE";
                gridLookcategoria.Text = "";
                gridLookcategoria.EditValue = clases.ClassVariables.idnuevo;
            }
        }

        private void checkEditCompu_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditCompu.Checked == true)
            {
                //labelControl11.Enabled = false;
                //gridLookupartpadre.EditValue = "";
                //gridLookupartpadre.Text = "";
                //gridLookupartpadre.Enabled = false;
            }
            else
            {
                //labelControl11.Enabled = true;
                //gridLookupartpadre.Enabled = true;
            }
        }

        private void memoEditdescripcion_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridLookmarca_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void simpleButtonsubcategoria_Click(object sender, EventArgs e)
        {
            clases.ClassVariables.bandera = 1;
            clases.ClassVariables.llamadoDentroForm = true;
            clases.ClassVariables.id_traido = gridLookcategoria.EditValue.ToString();
            Form prov = new SubCategoria();
            prov.WindowState = System.Windows.Forms.FormWindowState.Normal;
            prov.ShowDialog();
            if (clases.ClassVariables.idnuevo != null)
            {
                cadena = "SELECT sub_categorias.codigo_subcat as CODIGO,sub_categorias.nombre_subcategoria as SUBCATEGORIA " +
                          "FROM sub_categorias inner join categorias ON sub_categorias.codigo_categoria = categorias.codigo_categoria " +
                          "where sub_categorias.estadoid<>2 and categorias.codigo_categoria=" + gridLookcategoria.EditValue; 
                gridLooksubcategoria.Properties.DataSource = logica.Tabla(cadena);
                gridLooksubcategoria.Properties.ValueMember = "CODIGO";
                gridLooksubcategoria.Properties.DisplayMember = "SUBCATEGORIA";
                gridLooksubcategoria.Text = "";
                gridLooksubcategoria.EditValue = clases.ClassVariables.idnuevo;
            }
        }

        private void gridLookcategoria_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                cadena = "SELECT sub_categorias.codigo_subcat as CODIGO,sub_categorias.nombre_subcategoria as SUBCATEGORIA " +
                          "FROM sub_categorias inner join categorias ON sub_categorias.codigo_categoria = categorias.codigo_categoria " +
                          "where sub_categorias.estadoid<>2 and categorias.codigo_categoria=" + gridLookcategoria.EditValue;
                gridLooksubcategoria.Enabled = true;
                simpleButtonsubcategoria.Enabled=true;
                gridLooksubcategoria.Properties.DataSource = logica.Tabla(cadena);
                gridLooksubcategoria.Properties.ValueMember = "CODIGO";
                gridLooksubcategoria.Properties.DisplayMember = "SUBCATEGORIA";
                gridLooksubcategoria.Text = "";
            }
            catch
            {

            }
        }

        private void textEditcodigo_Validated(object sender, EventArgs e)
        {
            textEditcodigo.Text = textEditcodigo.Text.Replace(" ", "");
            textEditcodigo.Text = textEditcodigo.Text.Trim();
        }

        private void textEdit_utilidad_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}