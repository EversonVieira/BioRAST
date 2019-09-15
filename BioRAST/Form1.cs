using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BioRAST
{
    public partial class Form1 : Form
    {
        #region Comments EN
        //This project is a basis for a personal project by Everson Vieira:https://github.com/EversonVieira
        //This project use the database SQLITE: https://www.sqlite.org/index.html
        //This project is free and open-source for study.
        #endregion
        #region Comentários PT-BR
        // Este projeto é uma base para um projeto pessoal de Everson Vieira: https://github.com/EversonVieira
        // Este projeto utiliza o banco de dados SQLITE: https://www.sqlite.org/index.html
        // Este projeto é gratuíto e de código aberto para estudo.
        #endregion

        #region Code
        #region Initialization
        Repositório.CargaDAO cargas = new Repositório.CargaDAO();
        public static bool refresh = false;
        public static string currentCOD;
        public static bool controle;
        public static bool openform;
        public static View.cargaADD cargaADD;
        public Form1()
        {
            InitializeComponent();
        }
        #region front
        private void HideBTTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CloseBTTN_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SizeBTTN_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                SizeBTTN.BackgroundImage = Properties.Resources.normalizar3;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                SizeBTTN.BackgroundImage = Properties.Resources.Maximizar2;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Task.Run(() => moverBarra(configBTTN.Location.Y));
        }
        Point p;
        private void moverBarra(int y)
        {
            if (y > pictureBox1.Location.Y)
            {
                do
                {
                    Thread.Sleep(10);
                    BeginInvoke((MethodInvoker)delegate
                    {
                        p = new Point(0, 11 + pictureBox1.Location.Y);
                        pictureBox1.Location = p;
                    });
                }
                while (pictureBox1.Location.Y < y - 11);
            }
            if (y < pictureBox1.Location.Y)
            {
                do
                {
                    Thread.Sleep(10);
                    BeginInvoke((MethodInvoker)delegate
                    {
                        p = new Point(0, pictureBox1.Location.Y - 11);
                        pictureBox1.Location = p;
                    });
                }
                while (pictureBox1.Location.Y > y + 11);
            }
        }
        private void HomeBTTN_Click(object sender, EventArgs e)
        {
            Task.Run(() => moverBarra(homeBTTN.Location.Y));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = cargas.TabelaCargas();
            Ts();
        }
        #endregion
        public void refreshGrid()
        {
            while (true)
            {
                if (refresh)
                {
                    refresh = false;
                    BeginInvoke((MethodInvoker)delegate ()
                    {
                        dataGridView1.DataSource = cargas.TabelaCargas();
                    });
                }
            }
        }
        public void Ts()
        {
            Thread t1 = new Thread(refreshGrid);
            t1.IsBackground = true;
            t1.Start();
        }
        #endregion
        #region functions
        private void Button1_Click(object sender, EventArgs e) // Insertion Form Method
        {
            controle = false;
            if (openform == false)
            {
                openform = true;
                if (cargaADD == null)
                {
                    cargaADD = new View.cargaADD();
                }
                cargaADD.Show(this);
            }
            else
            {
                cargaADD.Visible = true;
            }
        }
        private void Button5_Click_1(object sender, EventArgs e) // Refresh Function
        {
            dataGridView1.DataSource = cargas.TabelaCargas();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) // Search Function
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("[NOME] LIKE '{0}*' OR " +
                                                                                        "[CÓDIGO] LIKE '{0}*' OR " +
                                                                                        "[TIPO] LIKE '{0}*' OR" +
                                                                                        "[TRANSPORTADORA] LIKE '{0}*' OR" +
                                                                                        "[DESCRIÇÃO] LIKE '{0}*'", textBox1.Text);

        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) // 
        {
            openEDIT(); // Update Form Function
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            openEDIT(); // Update Form Function
        }
        private void openEDIT() // Update Form Function
        {
            try
            {
                controle = true;
                currentCOD = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                if (openform == false)
                {
                    openform = true;
                    if (cargaADD == null)
                    {
                        cargaADD = new View.cargaADD();
                    }
                    cargaADD.Show(this);
                }
                else
                {
                    cargaADD.Visible = true;
                }
            }
            catch
            {
                throw;
            }
        }

        private void Button3_Click(object sender, EventArgs e) // Remove Function
        {
            try
            {
                Objetos.CargaOBJ carga = new Objetos.CargaOBJ();
                carga.cod = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                cargas.removerCargaDB(carga);
                MessageBox.Show("Sucesso ao remover a carga");
                dataGridView1.DataSource = cargas.TabelaCargas();
                dataGridView1.Refresh();
            }
            catch
            {
                MessageBox.Show("Erro, nenhuma célula selecionada");
            }
        }
        #endregion
        #endregion
    }
}
