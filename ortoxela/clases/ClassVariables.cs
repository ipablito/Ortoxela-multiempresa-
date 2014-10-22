using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ortoxela.clases
{
    class ClassVariables
    {
        public static string id_usuario;
        public static string id_busca;
        public static string id_rol; //Este variable almacena el nivel de usuario.......
        public static string cadenabusca;
        public static int bandera;
        public static string NombreComple;
        public static string idnuevo;
        public static bool llamadoDentroForm;// esta bandera o variable me servira para saber si el formulario fue llamada desde el principal o de otro form
        public static string id_traido;//esta variable me sirve para q cuando se este filtrando categorias o direcciones se lleve el id deseado
        public static bool sociocomercial;
        public static int op_reporte = 0;
        public static string nombreEmpresa;
        public static int idEmpresa;
        public static string ConexionMaster = "server=192.168.1.8;User Id=root;password=ortox3l4;Persist Security Info=True;database=master";
        public static string ConexionMasterRemoto = "server=181.209.252.2;User Id=root;password=ortox3l4;Persist Security Info=True;database=master";
        public static string LocalRemoto="1";//Local es 1... Remoto es 0
    }
}
