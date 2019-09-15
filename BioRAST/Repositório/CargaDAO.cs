using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace BioRAST.Repositório
{
    class CargaDAO
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
        SQLiteConnection db_connect;
        private bool connection()
        {
            try
            {
                if (!File.Exists("dataBase.db"))
                {
                    SQLiteConnection.CreateFile("dataBase.db");
                }
                using (var conn = new SQLiteConnection("Data Source = dataBase.db"))
                {
                    conn.Open();
                    using (var comm = new SQLiteCommand(conn))
                    {
                        comm.CommandText = "create table if not exists Cargas(cod string, nome string, tipo string, transportadora string, desc string)";
                        comm.ExecuteNonQuery();
                        conn.Close();
                    }
                    db_connect = new SQLiteConnection("Data Source = dataBase.db");
                    db_connect.Open();
                    return true;
                }
            }
            catch
            {
                return false;
                throw;
            }
        } // Connection Function
        public bool inserirCargaDB(Objetos.CargaOBJ carga)
        {
            try
            {
                if (connection())
                {
                    using (var command = new SQLiteCommand(db_connect))
                    {
                        command.CommandText = "Select * from Cargas where cod = \'" + carga.cod + "\'";
                        SQLiteDataReader reader = command.ExecuteReader();
                        bool chck = false;
                        while (reader.Read())
                        {
                            chck = true;
                        }
                        if (!chck)
                        {
                            using (var comm = new SQLiteCommand(db_connect))
                            {
                                comm.CommandText = string.Format("INSERT INTO Cargas(cod,nome,tipo,transportadora,desc) VALUES(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")",
                                                                carga.cod, carga.nome, carga.tipo, carga.transportadora, carga.desc);
                                comm.ExecuteNonQuery();
                                db_connect.Close();
                                return true;
                            }
                        }
                        else
                        {
                            db_connect.Close();
                            return false;
                        }
                    }
                }
                else
                {
                    db_connect.Close();
                    return false;
                }
            }
            catch
            {
                db_connect.Close();
                throw;
            }
        } // Insert Function
        public bool atualizarCargaDB(Objetos.CargaOBJ carga)
        {
            try
            {
                if (connection())
                {
                    using (var comm = new SQLiteCommand(db_connect))
                    {
                        comm.CommandText = string.Format("UPDATE Cargas SET " +
                            "cod = \"{0}\", nome = \"{1}\", tipo = \"{2}\", transportadora = \"{3}\"," +
                            "desc = \"{4}\" where cod = \"{0}\"", carga.cod, carga.nome, carga.tipo, carga.transportadora, carga.desc);
                        comm.ExecuteNonQuery();
                        db_connect.Close();
                    }
                    return true;
                }
                else
                {
                    db_connect.Close();
                    return false;
                }
            }
            catch
            {
                db_connect.Close();
                throw;
            }
        } // Update Function
        public bool removerCargaDB(Objetos.CargaOBJ carga)
        {
            try
            {
                if (connection())
                {
                    using (var comm = new SQLiteCommand(db_connect))
                    {
                        comm.CommandText = string.Format("Delete from Cargas WHERE cod = \'{0}\'", carga.cod);
                        comm.ExecuteNonQuery();
                        db_connect.Close();
                        return true;
                    }
                }
                else
                {
                    db_connect.Close();
                    return false;
                }
            }
            catch
            {
                db_connect.Close();
                throw;
            }
        } // Remove Function
        public DataTable TabelaCargas()
        {
            DataTable auxTable = new DataTable();
            try
            {
                if (connection())
                {
                    using (var comm = new SQLiteCommand("SELECT * FROM Cargas", db_connect))
                    {
                        using (var adapter = new SQLiteDataAdapter(comm))
                        {
                            adapter.Fill(auxTable);
                            auxTable.Columns[0].ColumnName = "CÓDIGO";
                            auxTable.Columns[1].ColumnName = "NOME";
                            auxTable.Columns[2].ColumnName = "TIPO";
                            auxTable.Columns[3].ColumnName = "TRANSPORTADORA";
                            auxTable.Columns[4].ColumnName = "DESCRIÇÃO";
                            db_connect.Close();
                            return auxTable;
                        }
                    }
                }
                else
                {
                    return auxTable;
                }
            }
            catch
            {
                throw;
            }
        } // Get* Function with a DataTable return
        public Objetos.CargaOBJ CargaOBJ(string cod)
        {
            try
            {
                if (connection())
                {
                    Objetos.CargaOBJ auxCarga = new Objetos.CargaOBJ();
                    using (var comm = new SQLiteCommand(db_connect))
                    {
                        comm.CommandText = "Select * from Cargas where cod = \'" + cod + "\'";
                        SQLiteDataReader reader = comm.ExecuteReader();
                        while (reader.Read())
                        {
                            auxCarga.cod = cod;
                            auxCarga.nome = reader[1].ToString();
                            auxCarga.tipo = reader[2].ToString();
                            auxCarga.transportadora = reader[3].ToString();
                            auxCarga.desc = reader[4].ToString();
                        }
                        db_connect.Close();
                        return auxCarga;
                    }
                }
                return null;
            }
            catch
            {
                throw;
            }
        } // Get one Item Function with a Object Return 
        #endregion
    }
}
