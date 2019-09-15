using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BioRAST.View
{
    public partial class cargaADD : Form
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
        Repositório.CargaDAO cargaDAO = new Repositório.CargaDAO();
        public cargaADD()
        {
            InitializeComponent();
        }

        private void CargaADD_Load(object sender, EventArgs e)
        {
            startup();
        }
        private void startup()
        {
            if (Form1.controle)
            {
                Objetos.CargaOBJ cargaInfo = cargaDAO.CargaOBJ(Form1.currentCOD);
                Nome.Text = cargaInfo.nome;
                Cod.Text = cargaInfo.cod;
                Tipo.Text = cargaInfo.tipo;
                Transportadora.Text = cargaInfo.transportadora;
                Desc.Text = cargaInfo.desc;
                button1.Text = "ATUALIZAR";
                Cod.Enabled = false;
            }
            else
            {
                Cod.Enabled = true;
                button1.Text = "ADICIONAR";
            }
        }
        private void CargaADD_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.cargaADD.Dispose();
            Form1.cargaADD = null;
            Form1.openform = false;
        }

        #endregion
        #region Function Buttons
        private void CloseBTTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HideBTTN_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Nome.Text = "";
            Cod.Text = "";
            Tipo.Text = "";
            Transportadora.Text = "";
            Desc.Text = "";
        }
        #endregion
        #region Insert/Update
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Nome.Text != "" && Cod.Text != "" && Transportadora.Text != "" && Tipo.Text != "")
                {
                    Objetos.CargaOBJ carga = new Objetos.CargaOBJ();
                    carga.nome = Nome.Text;
                    carga.cod = Cod.Text;
                    carga.tipo = Tipo.Text;
                    carga.transportadora = Transportadora.Text;
                    carga.desc = Desc.Text;
                    if (Form1.controle)//UPDATE
                    {
                        if (cargaDAO.atualizarCargaDB(carga))
                        {
                            carga = null;
                            MessageBox.Show("Sucesso ao atualizar a carga", "Inserir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Form1.refresh = true;
                        }
                        else
                        {
                            MessageBox.Show("Carga já cadastrada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else//INSERT
                    {

                        if (cargaDAO.inserirCargaDB(carga))
                        {
                            carga = null;
                            MessageBox.Show("Sucesso ao inserir a carga", "Inserir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Form1.refresh = true;
                        }
                        else
                        {
                            MessageBox.Show("Carga já cadastrada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Insira todos os dados obrigatórios para inserir", "Inserir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch
            {
                throw;
            }
        }
        #endregion
        #endregion
    }
}
