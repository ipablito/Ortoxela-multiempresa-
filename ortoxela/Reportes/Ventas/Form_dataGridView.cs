using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//para excel
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

//para PDF
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ortoxela.Reportes.Ventas
{
    public partial class Form_dataGridView : Form
    {
        public Form_dataGridView()
        {
            InitializeComponent();
            this.Text = clases.ClassVariables.nombreEmpresa + "  -  Estadisticas por mes";
        }

        #region Variables para abrir un excel
        private static object vk_missing = System.Reflection.Missing.Value;
        private static object vk_visible = true;
        private static object vk_false = false;
        private static object vk_true = true;
        private object vk_update_links = 0;
        private object vk_read_only = vk_true;
        private object vk_format = 1;
        private object vk_password = vk_missing;
        private object vk_write_res_password = vk_missing;
        private object vk_ignore_read_only_recommend = vk_true;
        private object vk_origin = vk_missing;
        private object vk_delimiter = vk_missing;
        private object vk_editable = vk_false;
        private object vk_notify = vk_false;
        private object vk_converter = vk_missing;
        private object vk_add_to_mru = vk_false;
        private object vk_local = vk_false;
        private object vk_corrupt_load = vk_false;
        #endregion


        #region meter el datagridview al archivo de Excel destino
        public void deDGVaExcel(DataGridView MiDataGrid, string RT)
        {
            string[] letras = new string[100];
            letras[0] = "A";
            letras[1] = "B";
            letras[2] = "C";
            letras[3] = "D";
            letras[4] = "E";
            letras[5] = "F";
            letras[6] = "G";
            letras[7] = "H";
            letras[8] = "I";
            letras[9] = "J";
            letras[10] = "K";
            letras[11] = "L";
            letras[12] = "M";
            letras[13] = "N";
            letras[14] = "O";
            letras[15] = "P";
            letras[16] = "Q";
            letras[17] = "R";
            letras[18] = "S";
            letras[19] = "T";
            letras[20] = "U";
            letras[21] = "V";
            letras[22] = "W";
            letras[23] = "X";
            letras[24] = "Y";
            letras[25] = "Z";
            letras[26] = "AA";
            letras[27] = "AB";
            letras[28] = "AC";
            letras[29] = "AD";
            letras[30] = "AE";
            letras[31] = "AF";
            letras[32] = "AG";
            letras[33] = "AH";
            letras[34] = "AI";
            letras[35] = "AJ";
            letras[36] = "AK";
            letras[37] = "AL";
            letras[38] = "AM";
            letras[39] = "AN";
            letras[40] = "AO";
            letras[41] = "AP";
            letras[42] = "AQ";
            letras[43] = "AR";
            letras[44] = "AS";
            letras[45] = "AT";
            letras[46] = "AU";
            letras[47] = "AV";
            letras[48] = "AW";
            letras[49] = "AX";
            letras[50] = "AY";
            letras[51] = "AZ";
            letras[52] = "BA";
            letras[53] = "BB";
            letras[54] = "BC";
            letras[55] = "BD";
            letras[56] = "BE";
            letras[57] = "BF";
            letras[58] = "BG";
            letras[59] = "BH";
            letras[60] = "BI";
            letras[61] = "BJ";
            letras[62] = "BK";
            letras[63] = "BL";
            letras[64] = "BM";
            letras[65] = "BN";
            letras[66] = "BO";
            letras[67] = "BP";
            letras[68] = "BQ";
            letras[69] = "BR";
            letras[70] = "BS";
            letras[71] = "BT";
            letras[72] = "BU";
            letras[73] = "BV";
            letras[74] = "BW";
            letras[75] = "BX";
            letras[76] = "BY";
            letras[77] = "BZ";
            letras[78] = "CA";
            letras[79] = "CB";
            letras[80] = "CC";

            String ruta = Application.StartupPath + RT;

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(ruta, vk_update_links, vk_read_only, vk_format, vk_password,
            vk_write_res_password, vk_ignore_read_only_recommend, vk_origin,
            vk_delimiter, vk_editable, vk_notify, vk_converter, vk_add_to_mru,
            vk_local, vk_corrupt_load);
            Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1);

            for (int i = 0; i < MiDataGrid.ColumnCount; i++)
            {
                string lt = letras[i];
                excelWorksheet.Cells[1, lt] = MiDataGrid.Columns[i].HeaderText;
            }

            int fila = 1;
            for (int i = 0; i < MiDataGrid.Rows.Count; i++)//le qite esta linea porque se tardaba mucho            
            {
                fila += 1;
                for (int j = 0; j < MiDataGrid.Columns.Count; j++)
                {
                    string lt = letras[j];
                    excelWorksheet.Cells[fila, lt] = MiDataGrid.Rows[i].Cells[j].Value;
                }
            }
            excelApp.Visible = true;
        }  //meter la informacion de un datagrid
        #endregion

        #region meter el datagridview al archivo PDF
        public void GenerarDocumento(Document document,DataGridView data)
        {

            //iPablito_
            //hago esto porq mi data gridview no se llama como x default
            DataGridView dataGridView1 = data;

            //se crea un objeto PdfTable con el numero de columnas del 


            //dataGridView

            PdfPTable datatable = new PdfPTable(dataGridView1.ColumnCount);

            //asignamos algunas propiedades para el diseño del pdf

            datatable.DefaultCell.Padding = 3;

            float[] headerwidths = GetTamañoColumnas(dataGridView1);


            datatable.SetWidths(headerwidths);


            datatable.WidthPercentage = 100;


            datatable.DefaultCell.BorderWidth = 2;


            //datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;




            //SE GENERA EL ENCABEZADO DE LA TABLA EN EL PDF


            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {


                datatable.AddCell(dataGridView1.Columns[i].HeaderText);


            }




            datatable.HeaderRows = 1;


            datatable.DefaultCell.BorderWidth = 1;




            //SE GENERA EL CUERPO DEL PDF


            for (int i = 0; i < dataGridView1.RowCount; i++)
            {


                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {


                    datatable.AddCell(dataGridView1[j, i].Value.ToString());


                }


                datatable.CompleteRow();


            }

            //SE AGREGAR LA PDFPTABLE AL DOCUMENTO
            document.Add(datatable);
        }

        //Función que obtiene los tamaños de las columnas del grid

        public float[] GetTamañoColumnas(DataGridView dg)
        {

            float[] values = new float[dg.ColumnCount];

            for (int i = 0; i < dg.ColumnCount; i++)
            {

                values[i] = (float)dg.Columns[i].Width;

            }

            return values;

        }
        #endregion


        public void CargarGrid(DataTable datos)
        {
            datos = SumarColumna(datos);
            dataGridView_mostrar.DataSource = datos;
            AlinearGrid();
            //darformato();
        }

        public void AlinearGrid()
        {
            dataGridView_mostrar.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView_mostrar.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView_mostrar.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            int filas = dataGridView_mostrar.Columns.Count;

            for (int i = 3; i < filas; i++)
            {
                dataGridView_mostrar.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        public void darformato()
        {
            int columnas = dataGridView_mostrar.Columns.Count;
            for (int i = 3; i < columnas; i++)
            {
                this.dataGridView_mostrar.Columns[i].DefaultCellStyle.Format = "Q.";
            }
        }

        public DataTable SumarColumna(DataTable datos_)
        {
            datos_.Rows.Add("Total");
            int filas = datos_.Rows.Count;
            filas = filas - 1;
            int columnas = datos_.Columns.Count;
            double[] resultados = new double[columnas];

            for (int i = 0; i < columnas; i++)
            {
                resultados[i] = 0;
            }


            for (int ix = 3; ix < columnas; ix++)
            {
                for (int iy = 0; iy < filas; iy++)
                {
                    resultados[ix] = resultados[ix] + Convert.ToDouble(datos_.Rows[iy][ix]);
                }
            }


            for (int i = 3; i < columnas; i++)
            {
                datos_.Rows[filas][i] = resultados[i].ToString();
            }
            return datos_;
        }


        public void DeGridA_PDF(DataGridView data)
        {
            try
            {
                //Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                Document doc = new Document(PageSize.A0.Rotate(), 10, 10, 10, 10);
                string filename = "PDF.pdf";
                FileStream file = new FileStream(filename, FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.ReadWrite);
                PdfWriter.GetInstance(doc, file);
                doc.Open();
                GenerarDocumento(doc,data);
                doc.Close();
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mostrarEnExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string rutaExcel="\\Excel.xlsx";
            deDGVaExcel(dataGridView_mostrar, rutaExcel);
            this.Cursor = Cursors.Default;
        }

        private void mostrarEnPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //
            try
            {
                File.Delete("PDF.pdf");
            }
            catch { }
            DeGridA_PDF(dataGridView_mostrar);
            //
            this.Cursor = Cursors.Default;
        }

        private void Form_dataGridView_Load(object sender, EventArgs e)
        {

        }
    }
}
