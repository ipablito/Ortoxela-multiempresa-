using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Localization;


using System.IO;

namespace ortoxela.Principal
{
    public partial class Principal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.sociocomercial = false;
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.form_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.bandera = 1;
                Form nuevo = new Clientes.form_cliente();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Proveedores.Proveedor)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Proveedores.Proveedor();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }
        string cadena;
        classortoxela logicaorto = new classortoxela();
        private void ValidaBotones()
        {
            try
            {
            cadena = "SELECT permisos.nombre_permiso FROM roles_permisos INNER JOIN permisos ON roles_permisos.permisoid=permisos.permisoid INNER JOIN roles ON roles_permisos.codigo_rol=roles.codigo_rol INNER JOIN rol_usuario ON roles.codigo_rol=rol_usuario.codigo_rol WHERE rol_usuario.userid="+clases.ClassVariables.id_usuario;
            DataTable tabla=new DataTable();
            tabla=logicaorto.Tabla(cadena);
            BarButtonItem CapturaBoton=new BarButtonItem();
            RibbonPageCategory capturaribbonpage = new RibbonPageCategory();
                foreach(DataRow NombreBoton in tabla.Rows)
                {                    
                    foreach(object BotonBuscar in ribbon.Items)
                    {
                        if(BotonBuscar is BarButtonItem)
                        {
                            CapturaBoton=(BarButtonItem)BotonBuscar;
                            if (NombreBoton["nombre_permiso"].ToString() == CapturaBoton.Name.ToString())
                            {
                                CapturaBoton.Enabled = true;
                            }                      
                        }                    
                    }
                }

                Boolean permiso_existe;                   
                foreach (RibbonPageCategory ribbonPageCategoryBase in ribbon.PageCategories)
                {
                    permiso_existe = false;
                    foreach (DataRow NombreBoton in tabla.Rows)
                    {
                        if (NombreBoton["nombre_permiso"].ToString() == ribbonPageCategoryBase.Name)
                            permiso_existe = true;                        
                    }
                    if (permiso_existe == false)
                        ribbonPageCategoryBase.Visible = false;
                }
                                
            }
            catch
            {}

            
        }
        public class MyBarLocalizer : BarLocalizer
        {
            public override string GetLocalizedString(BarString id)
            {
                if (id == BarString.SkinCaptions)
                {
                    string defaultSkinCaptions = base.GetLocalizedString(id);
                    string newSkinCaptions = defaultSkinCaptions.Replace("|DevExpress Style|", "|My Favorite Skin|");
                    return newSkinCaptions;
                }
                return base.GetLocalizedString(id);
            }
        }


        private void Principal_Load(object sender, EventArgs e)
        {
            
            #region temas de devexpress

            /* registrando los skins*/
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();


            BarLocalizer.Active = new MyBarLocalizer();

            SkinHelper.InitSkinGallery(ribbonGalleryBarItem2, true);
            SkinHelper.InitSkinPopupMenu(popupMenu1);


            string tema = "";
            StreamReader leer = new StreamReader("tema.txt");

            while (!leer.EndOfStream)
            {
                tema = leer.ReadLine().ToString();
            }
            
            leer.Close();




            Properties.Settings.Default.mascara = tema;
            Properties.Settings.Default.Save();
            defaultLookAndFeel2.LookAndFeel.SetSkinStyle(tema);
            #endregion


            ValidaBotones();
            barStaticItem1.Caption = "USUARIO: "+clases.ClassVariables.NombreComple ;
            //frm_datos_inicio nuevo = new frm_datos_inicio();
            //nuevo.MdiParent = this;
            //nuevo.Show();                   
            // Reportes.Pedidos.DataSet_Pedidos data = new Reportes.Pedidos.DataSet_Pedidos();
            actualizarDataGrid();
            actualizar_sistemasEnBodega();

        }

        private void actualizarDataGrid()
        {
            try
            {
                Reportes.Pedidos.ClassPedidos pedidos = new Reportes.Pedidos.ClassPedidos();
                classortoxela orto = new classortoxela();
                dataGridView1.DataSource = orto.Tabla("SELECT CAST(CONCAT(tipo_documento,' - ',no_documento)AS CHAR) AS Pedido, "+
"Nombre_Cliente AS 'Nombre de cliente',"+
"nombre_paciente AS 'Nombre del paciente',"+
"Socio_Comercial AS 'Socio Comercial',"+
"COALESCE((SELECT Articulo FROM v_pedidos e WHERE e.no_documento=p.no_documento AND e.sistema=1 AND e.cantidad_enviada!=0 LIMIT 1),'Sin set') AS 'Set en el envio'," +
"fecha_compra AS 'Fecha'"+


"FROM v_pedidos p "+
"WHERE cantidad_enviada!=0 "+

"GROUP BY tipo_documento,no_documento,Nombre_Cliente "+

"ORDER BY fecha ASC");

                dataGridView1.Refresh();
            }
            catch
            { }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Estado.Estado)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Estado.Estado();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Paises)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Paises();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Compra.frm_compras)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new Compra.frm_compras();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
       }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Departamentos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Departamentos();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Municipios)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Municipios();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Direcciones)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Direcciones();
                nuevo.MdiParent = this;
                nuevo.Show();
            }           
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Ciudades)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Ciudades();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Usuario)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form usu = new Usuario.Usuario();
                usu.MdiParent = this;
                usu.Show();
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.form_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Clientes.form_cliente();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        

        private void barButtonItem30_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Direcciones)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Direcciones();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem31_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Direcciones)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Direcciones();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem26_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Usuario)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form usu = new Usuario.Usuario();
                usu.MdiParent = this;
                usu.Show();
            }
        }

        private void barButtonItem27_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Usuario)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form usu = new Usuario.Usuario();
                usu.MdiParent = this;
                usu.Show();
            }
        }

       

        private void barButtonItem38_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Bodega.Tipobodega)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form bodega = new Bodega.Tipobodega();
                bodega.MdiParent = this;
                bodega.Show();
            }
        }

        private void barButtonItem34_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Bodega.Tipobodega)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form bodega = new Bodega.Tipobodega();
                bodega.MdiParent = this;
                bodega.Show();
            }
        }

        private void barButtonItem32_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Bodega.Tipobodega)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form bodega = new Bodega.Tipobodega();
                bodega.MdiParent = this;
                bodega.Show();
            }
        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Proveedores.Tipo_Proveedor)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Proveedores.Tipo_Proveedor();
                nuevo.MdiParent = this;
                nuevo.Show();
            }    
        }

        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Proveedores.Tipo_Proveedor)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Proveedores.Tipo_Proveedor();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Proveedores.Tipo_Proveedor)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Proveedores.Tipo_Proveedor();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Proveedores.Proveedor)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Proveedores.Proveedor();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Proveedores.Proveedor)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Proveedores.Proveedor();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem39_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Departamentos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Departamentos();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem40_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Departamentos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Departamentos();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem28_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Paises)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Paises();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem29_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Paises)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Paises();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem43_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Categorias)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Categorias();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem44_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Categorias)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Categorias();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem45_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Categorias)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Categorias();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem46_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Marcas)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Marcas();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem47_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Marcas)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Marcas();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem48_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Marcas)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Marcas();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem50_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Reportes.Producto.frm_Existencias)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new Reportes.Producto.frm_Existencias();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Articulos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Articulos();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem41_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Articulos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Articulos();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem42_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Articulos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.Articulos();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem49_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TrasladoBodega.TrasladoBodega)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new TrasladoBodega.TrasladoBodega();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Estado.Estado)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Estado.Estado();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Estado.Estado)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Estado.Estado();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem51_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TipoPago.TipPago)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new TipoPago.TipPago();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem52_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TipoPago.TipPago)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new TipoPago.TipPago();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem53_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TipoPago.TipPago)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new TipoPago.TipPago();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem54_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TipoDocumentos.TipoDoc)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new TipoDocumentos.TipoDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem55_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TipoDocumentos.TipoDoc)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new TipoDocumentos.TipoDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem56_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Series.SerieDoc)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.bandera = 1;
                Form nuevo = new Series.SerieDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem57_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Series.SerieDoc)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Series.SerieDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem58_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Modulo)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Modulo();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem59_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Modulo)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Modulo();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem60_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Modulo)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Modulo();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

       

        private void barButtonItem64_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Roles)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Roles();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem65_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Roles)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Roles();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem66_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Municipios)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Municipios();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem67_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Municipios)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Municipios();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem68_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Ciudades)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Ciudades();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem69_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Direcciones.Ciudades)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Direcciones.Ciudades();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem35_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        

        private void Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cerrarsesion == false)
            {
                Application.Exit();
            }
            else
            {
                for (int i = 0; i < this.MdiChildren.Length; i++)
                {
                    this.MdiChildren[i].Close();
                }
            }
        }
        Boolean cerrarsesion = false;
        private void barButtonItem119_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "Login")
                {
                    form.Show();
                    cerrarsesion = true;
                }
            }
            this.Close();
        }

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
              

        }

        private void barButtonItem70_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Pedido.frm_pedido)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new Pedido.frm_pedido();
                nuevo.MdiParent = this;
                nuevo.BringToFront();
                //simpleButton1.SendToBack();
                nuevo.Show();
            }
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.form_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Clientes.form_cliente();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem61_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Rol_usuario)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Rol_usuario();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
            }

        private void barButtonItem62_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Rol_usuario)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Rol_usuario();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem63_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Rol_usuario)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Rol_usuario();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.Tipo_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Clientes.Tipo_cliente();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.Tipo_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Clientes.Tipo_cliente();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.Tipo_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Clientes.Tipo_cliente();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem71_ItemClick(object sender, ItemClickEventArgs e)
        {
             int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.Roles)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Usuario.Roles();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem72_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Permisos.Selector_Permisos)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new Permisos.Selector_Permisos();
                nuevo.Show();
            }
        }

        private void barButtonItem73_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Pedido.frm_regreso)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new Pedido.frm_regreso();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem74_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.SubCategoria)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.id_traido = "";
                Form nuevo = new Articulos.SubCategoria();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem75_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.SubCategoria)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.SubCategoria();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem76_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.SubCategoria)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Articulos.SubCategoria();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem77_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TipoDocumentos.TipoDoc)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new TipoDocumentos.TipoDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem79_ItemClick(object sender, ItemClickEventArgs e)
        {
            FacturaTemporal.IngresarFacturas FactTemporal = new FacturaTemporal.IngresarFacturas();
            FactTemporal.MdiParent = this;
            FactTemporal.Show();
        }

        private void barButtonItem80_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Vueltos.Vueltos)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new Vueltos.Vueltos();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem78_ItemClick(object sender, ItemClickEventArgs e)
        {
            Reportes.Compras.Frm_RepComp reporte = new Reportes.Compras.Frm_RepComp();
            reporte.MdiParent = this;
            reporte.Show();
        }

        private void barButtonItem81_ItemClick(object sender, ItemClickEventArgs e)
        {

            Form nuevo = new Reportes.Pedidos.frm_ReportePedidos();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem82_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new Reportes.Ventas.Frm_RepVentas();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem83_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem84_ItemClick(object sender, ItemClickEventArgs e)
        {
            //MiniLogin.LoginMini.id_UsuarioModifica = "";
            //Form logins = new MiniLogin.LoginMini();
            //logins.ShowDialog();
            //if (MiniLogin.LoginMini.id_UsuarioModifica != "")
            //{
                //AnulaFactura.FacturaAnula.id_usuario_mod = MiniLogin.LoginMini.id_UsuarioModifica;
                AnulaFactura.FacturaAnula.id_usuario_mod = clases.ClassVariables.id_usuario;
                Form anulaFactura = new AnulaFactura.FacturaAnula();
                anulaFactura.MdiParent = this;
                anulaFactura.Show();
            //}
        }

        private void barButtonItem85_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form NUEVO = new Reportes.Inventario.Frm_RepInventario();
            NUEVO.MdiParent=this;
            NUEVO.Show();
        }

        private void barButtonItem86_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new Reimpresion.frm_reimpresion();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem87_ItemClick(object sender, ItemClickEventArgs e)
        {

            Form nuevo = new AnularDocumento.frm_reimpresion();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem88_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new ReciboCredito.frm_reciboCredito();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem90_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem91_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form form = new ModCobranza.frm_Abono();
            form.MdiParent = this;
            form.Show();
        }

        private void barButtonItem93_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.TipoDocs.TipoDoc )
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new ModCobranza.TipoDocs.TipoDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem94_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.TipoDocs.TipoDoc)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new ModCobranza.TipoDocs.TipoDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem95_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.TipoDocs.TipoDoc)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new ModCobranza.TipoDocs.TipoDoc();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem83_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Reportes.Pedidos.frm_RepVueltos nuevo = new Reportes.Pedidos.frm_RepVueltos();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem89_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.Bancos.Banco)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new ModCobranza.Bancos.Banco();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem90_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.Bancos.Banco)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new ModCobranza.Bancos.Banco();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem92_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.Bancos.Banco)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new ModCobranza.Bancos.Banco();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void ribbonGalleryBarItem2_GalleryItemClick(object sender, GalleryItemClickEventArgs e)
        {
            Properties.Settings.Default.mascara = e.Item.Caption;
            Properties.Settings.Default.Save();
            defaultLookAndFeel2.LookAndFeel.SkinName = Properties.Settings.Default.mascara;
            //MessageBox.Show(Properties.Settings.Default.mascara);
            StreamWriter escribir = new StreamWriter("tema.txt");
            escribir.WriteLine(Properties.Settings.Default.mascara);
            escribir.Close();
        }

        private void barButtonItem96_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new Reportes.Ventas.Frm_CortesCaja();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem97_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form form = new ModCobranza.frm_Abono_adelantado();
            form.MdiParent = this;
            form.Show();
        }

        private void barButtonItem98_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form form = new ModCobranza.Recibos.frm_recibo();
            form.MdiParent = this;
            form.Show();
        }

        private void barButtonItem99_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form form = new Cotizacion.frm_cotizacion();
            form.MdiParent = this;
            form.Show();
        }


        private void xtraTabbedMdiManager1_PageAdded(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            splitContainerDatos.Visible = false;
        }

        private void xtraTabbedMdiManager1_PageRemoved(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            if (xtraTabbedMdiManager1.Pages.Count == 0)
               splitContainerDatos.Visible = true;
        }

        private void barButtonItem100_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form form = new Cotizacion.frm_solicitud_compra();
            form.MdiParent = this;
            form.Show();
        }

        private void barButtonItem101_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new Buscador.frm_Buscador_documentos();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem102_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new Reportes.Ventas.Frm_VentasClientes();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem103_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.op_reporte = 1;
            Form nuevo = new ModCobranza.Reporte.frm_reportes();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            
        }

        private void barButtonItem104_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new ModContabilidad.Partidas.frm_condicion_conta();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem106_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new ModContabilidad.Partidas.frm_config_partida();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem107_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new ModContabilidad.Partidas.frm_partida_manual();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem110_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new ModContabilidad.Reportes.frm_partidas();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem111_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form nuevo = new Compra.frm_compras_varias();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem112_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form form = new ModCobranza.frm_pagos_proveedores();
            form.MdiParent = this;
            form.Show();
        }

        private void barButtonItem113_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.op_reporte = 2;
            Form nuevo = new ModCobranza.Reporte.Proveedores.frm_reportes_pagos_a_proveedores();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem120_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem115_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem122_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Usuario.CambioContrasena)
                    cont++;
            }
            if (cont == 0)
            {                
                clases.ClassVariables.llamadoDentroForm = true;
                Form contrasena = new Usuario.CambioContrasena();
                contrasena.WindowState = System.Windows.Forms.FormWindowState.Normal;
                //contrasena.MdiParent = this;
                contrasena.ShowDialog();
            }
        }

        private void barButtonItem124_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is TrasladoBodega.TrasladoBodega)
                    cont++;
            }
            if (cont == 0)
            {
                Form nuevo = new TrasladoBodega.TrasladoBodega();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void ribbon_Click(object sender, EventArgs e)
        {
            //if (ribbon.SelectedPage == ribbonPageCategory2.Pages[0])
            //{
            //    ribbonPageCategory1.Pages[0].Visible = false;
            //    ribbonPageCategory1.Pages[1].Visible = false;
            //}    
            
        }

        private void barButtonItem125_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.op_reporte = 3;
            Form nuevo = new ModCobranza.Reporte.Proveedores.frm_reportes_pagos_a_proveedores();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem126_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.op_reporte = 1;
            Form nuevo = new ModCobranza.Reporte.Proveedores.frm_reportes_pagos_a_proveedores();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem127_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.op_reporte = 2;
            Form nuevo = new ModCobranza.Reporte.frm_reportes();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem128_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.op_reporte = 3;
            Form nuevo = new ModCobranza.Reporte.frm_reportes();
            nuevo.MdiParent = this;
            nuevo.Show();
        }

        private void barButtonItem129_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.Tipo_cliente_conta)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.bandera = 1;
                Form nuevo = new Clientes.Tipo_cliente_conta();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem130_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.Tipo_cliente_conta)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Clientes.Tipo_cliente_conta();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem131_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.Tipo_cliente_conta)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Form nuevo = new Clientes.Tipo_cliente_conta();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem132_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.Proveedores.Tipo_proveedor_conta)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.bandera = 1;
                Form nuevo = new ModCobranza.Proveedores.Tipo_proveedor_conta();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem133_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.Proveedores.Tipo_proveedor_conta)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.bandera = 2;
                Form nuevo = new ModCobranza.Proveedores.Tipo_proveedor_conta();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem134_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is ModCobranza.Proveedores.Tipo_proveedor_conta)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.bandera = 3;
                Form nuevo = new ModCobranza.Proveedores.Tipo_proveedor_conta();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem136_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form adminRep = new Reportes.Admin.Frm_ReportesAdmin();
            adminRep.MdiParent = this;
            adminRep.Show();
        }

        private void barButtonItem137_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form ajreqrep = new Reportes.Requisiciones_Ajustes.F_reportesrequisicionyajuste();
            ajreqrep.MdiParent = this;
            ajreqrep.Show();
        }

        private void barButtonItem138_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.sociocomercial = false;
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.form_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.llamadoDentroForm = false;
                clases.ClassVariables.bandera = 1;
                //Form nuevo = new Clientes.form_cliente();
                Clientes.form_cliente nuevo = new Clientes.form_cliente();
                nuevo.sociocomercial();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem139_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.form_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                //Form nuevo = new Clientes.form_cliente();
                Clientes.form_cliente nuevo = new Clientes.form_cliente();
                nuevo.MdiParent = this;
                nuevo.sociocomercial();
                clases.ClassVariables.sociocomercial = true;
                nuevo.Show();
            }
        }

        private void barButtonItem140_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Clientes.form_cliente)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                //Form nuevo = new Clientes.form_cliente();
                //nuevo.MdiParent = this;
                Clientes.form_cliente nuevo = new Clientes.form_cliente();
                nuevo.MdiParent = this;
                nuevo.sociocomercial();
                clases.ClassVariables.sociocomercial = true;
                nuevo.Show();
            }
        }

        private void barButtonItem141_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            Form reimtras = new TrasladoBodega.ReimpresionTraslado();
            reimtras.MdiParent = this;
            reimtras.Show();
        }

        private void barButtonItem142_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form bodus = new UsuariosBodegasSeries.Form_BodegaUsuario();
            bodus.MdiParent = this;
            bodus.Show();
        }

        private void barButtonItem143_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form serbod = new UsuariosBodegasSeries.Form_BodegaSerie();
            serbod.MdiParent = this;
            serbod.Show();
        }

        private void barButtonItem144_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Articulos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 1;
                clases.ClassVariables.llamadoDentroForm = false;
                Articulos.Articulos nuevo = new Articulos.Articulos();
                nuevo.Sistema();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem145_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Articulos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 2;
                clases.ClassVariables.llamadoDentroForm = false;
                Articulos.Articulos nuevo = new Articulos.Articulos();
                nuevo.Sistema();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem146_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cont = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Articulos.Articulos)
                    cont++;
            }
            if (cont == 0)
            {
                clases.ClassVariables.bandera = 3;
                clases.ClassVariables.llamadoDentroForm = false;
                Articulos.Articulos nuevo = new Articulos.Articulos();
                nuevo.Sistema();
                nuevo.MdiParent = this;
                nuevo.Show();
            }
        }

        private void barButtonItem147_ItemClick(object sender, ItemClickEventArgs e)
        {
            clases.ClassVariables.cadenabusca = "SELECT * FROM v_articulos_cat_lbod WHERE compuesto=1 ";

            Form busca = new Buscador.Buscador();
            busca.ShowDialog();
            if (clases.ClassVariables.id_busca != "")
            {
                Form nf = new Articulos.F_configurarSistema(clases.ClassVariables.id_busca);
                nf.MdiParent = this;
                nf.Show();
            }
        }

        bool incrementar = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBarControl_PRINCIPAL.EditValue.ToString() == "100")
            {
                incrementar = false;
            }
            else if (progressBarControl_PRINCIPAL.EditValue.ToString() == "1")
            {
                incrementar = true;
            }

            if (incrementar == true)
            {
                progressBarControl_PRINCIPAL.Increment(1);
            }
            else
            {
                progressBarControl_PRINCIPAL.Decrement(1);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }

        void actualizar_sistemasEnBodega()
        {
            string cadena="SELECT articulos.`codigo_articulo` AS 'Codigo del set', "+
"articulos.`descripcion` AS 'Descripcion', "+
"bodegas.`existencia_articulo` AS 'Existencia', "+
"bodegas_header.`nombre_bodega` AS 'Bodega' "+
"FROM articulos "+
"INNER JOIN bodegas "+
"ON bodegas.`codigo_articulo`=articulos.`codigo_articulo` "+
"INNER JOIN bodegas_header "+
"ON bodegas.`codigo_bodega`=bodegas_header.`codigo_bodega` "+
"WHERE articulos.`estadoid`=1 AND articulos.`compuesto`=1 "+
"AND bodegas.`existencia_articulo`!=0";
            gridControl2.DataSource = logicaorto.Tabla(cadena);



            cadena = "SELECT articulos.`codigo_articulo` AS 'Codigo del set', " +
"articulos.`descripcion` AS 'Descripcion', " +
"bodegas.`existencia_articulo` AS 'Existencia', " +
"bodegas_header.`nombre_bodega` AS 'Bodega' " +
"FROM articulos " +
"INNER JOIN bodegas " +
"ON bodegas.`codigo_articulo`=articulos.`codigo_articulo` " +
"INNER JOIN bodegas_header " +
"ON bodegas.`codigo_bodega`=bodegas_header.`codigo_bodega` " +
"WHERE articulos.`estadoid`=1 AND articulos.`compuesto`=1 " +
"AND bodegas.`existencia_articulo`=0";
            gridControl1.DataSource = logicaorto.Tabla(cadena);


        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //progressBarControl_PRINCIPAL.Position=
            timer1.Stop();
            actualizarDataGrid();
            actualizar_sistemasEnBodega();
            timer1.Start();
            this.Cursor = Cursors.Default;
        }

        


       
    }
}
