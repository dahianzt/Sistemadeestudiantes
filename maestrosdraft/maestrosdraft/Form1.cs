using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace maestrosdraft
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dgvmirar.ColumnCount = 7;
            dgvmirar.Columns[0].Name = "ID";
            dgvmirar.Columns[1].Name = "Nombre";
            dgvmirar.Columns[2].Name = "Apellido";
            dgvmirar.Columns[3].Name = "Curso";
            dgvmirar.Columns[4].Name = "Promedio español";
            dgvmirar.Columns[5].Name = "Promedio sociales";
            dgvmirar.Columns[6].Name = "Promedio matemáticas";
       

            //ancho
            dgvmirar.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvmirar.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvmirar.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvmirar.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvmirar.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvmirar.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvmirar.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            //dgvmirar.Rows.Add(txtid.Text, txtnombre.Text, txtapellido.Text, txtcurso.Text);
            // mandar las calificaciones de los textbox para calcular:
            if (double.TryParse(lengua1.Text, out double lengua1grade) &&
                double.TryParse(lengua2.Text, out double lengua2grade) &&
                double.TryParse(lengua3.Text, out double lengua3grade) &&
                double.TryParse(social1.Text, out double social1grade) &&
                double.TryParse(social2.Text, out double social2grade) &&
                double.TryParse(social3.Text, out double social3grade) &&
                double.TryParse(mates1.Text, out double mates1grade) &&
                double.TryParse(mates2.Text, out double mates2grade) &&
                double.TryParse(mates3.Text, out double mates3grade))
            {
                // para calcular los promedios de las calificaciones:
                double promediolengua = (lengua1grade + lengua2grade + lengua3grade) / 3;
                double promediosocial = (social1grade + social2grade + social3grade) / 3;
                double promediomatematicas = (mates1grade + mates2grade + mates3grade) / 3;


                // agregar cosas al datagridview: ✨
                dgvmirar.Rows.Add(txtid.Text, txtnombre.Text, txtapellido.Text, txtcurso.Text, promediolengua.ToString(), promediosocial.ToString(), promediomatematicas.ToString());
                //eliminar luego de que la info pase al datagridview
                txtid.Clear();
                txtnombre.Clear();
                txtapellido.Clear();
                txtcurso.Clear();
                lengua1.Clear();
                lengua2.Clear();
                lengua3.Clear();
                social1.Clear();
                social2.Clear();
                social3.Clear();
                mates1.Clear();
                mates2.Clear();
                mates3.Clear();
            }
            else
            {
                MessageBox.Show("Ingrese cantidades", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncalcular_Click(object sender, EventArgs e)
        {   
            //ALGORITMOOOS en practica ✨

            //comienza el procedimiento
            // mirar la informacion del dgv anterior:
            if (dgvmirar.Rows.Count > 0)
            {
                //clear en caso de ser necesario
                dgvescala.Rows.Clear();

                //nombres y promedios de cada estudiante (en este paso se guardan)
                List<Tuple<string, double>> datosEstudiantes = new List<Tuple<string, double>>();

                //recorro filas, algoritmooo
                foreach (DataGridViewRow fila in dgvmirar.Rows)
                {
                    // hay que ver si no estan nulos cada una de las celdas de los promedios del dgv anterior
                    if (fila.Cells["Nombre"].Value != null && fila.Cells["Promedio español"].Value != null &&
                        fila.Cells["Promedio sociales"].Value != null && fila.Cells["Promedio matemáticas"].Value != null)
                    {
                        string nombrealumno = fila.Cells["Nombre"].Value.ToString();

                        // toca calcular el promedio con cada asignatura
                        double promediolengua = Convert.ToDouble(fila.Cells["Promedio español"].Value);
                        double promediosocial = Convert.ToDouble(fila.Cells["Promedio sociales"].Value);
                        double promediomatematicas = Convert.ToDouble(fila.Cells["Promedio matemáticas"].Value);

                        double promedioacumulado = (promediolengua + promediosocial + promediomatematicas) / 3;

                        // ahora se ve el nombre y el promedio
                        datosEstudiantes.Add(new Tuple<string, double>(nombrealumno, promedioacumulado));
                    }
                }

                // quicksort se encarga de comparar y ordenar los promedios
                quicksort(datosEstudiantes, 0, datosEstudiantes.Count - 1);

                dgvescala.Columns.Clear();
                dgvescala.Columns.Add("Nombre", "Nombre");
                dgvescala.Columns.Add("MejorPromedio", "Mejor Promedio");


                // filas en mi dgv para observar
                foreach (var estudiante in datosEstudiantes)
                {
                    dgvescala.Rows.Add(estudiante.Item1, estudiante.Item2);
                }
            }
            else
            {
                MessageBox.Show("No hay info calculable", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void quicksort(List<Tuple<string, double>> list, int izquierda, int derecha)
        {
            if (izquierda < derecha)
            {
                int indpartir = part(list, izquierda, derecha);

                if (indpartir > 1)
                {
                    quicksort(list, izquierda, indpartir - 1);
                }
                if (indpartir + 1 < derecha)
                {
                    quicksort(list, indpartir + 1, derecha);
                }
            }
        }

        private int part(List<Tuple<string, double>> list, int izquierda, int derecha)
        {
            double pivote = list[izquierda].Item2;
            while (true)
            {
                while (list[izquierda].Item2 > pivote)
                {
                    izquierda++;
                }

                while (list[derecha].Item2 < pivote)
                {
                    derecha--;
                }

                if (izquierda < derecha)
                {
                    Tuple<string, double> temp = list[izquierda];
                    list[izquierda] = list[derecha];
                    list[derecha] = temp;
                }
                else
                {
                    return derecha;
                }
            }
        }


    }
 }
  
    
