using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace Capa_Modelo_Consulta
{
    public class consultaSentencias
    {
        consultaConexion con = new consultaConexion();
        protected consultaConexion conn;
        private static string baseDatos = "";

        public consultaSentencias()
        {
            this.conn = new consultaConexion();
            baseDatos = this.conn.connection().Database;
        }

        /*
         Modificado por Carlos González 
         */

        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public OdbcDataAdapter buscartbl(string BD)
        {
            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ?";

            try
            {
                using (OdbcConnection connection = con.connection())
                {
                    connection.Open();

                    using (OdbcCommand command = new OdbcCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("?", BD);

                        using (OdbcDataAdapter datatable = new OdbcDataAdapter(command))
                        {
                            return datatable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar la base de datos: " + ex.Message);
                return null;
            }
        }

        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public void insertar(string dato, string tipo, string tabla)
        {
            string sql = $"INSERT INTO {tabla} ({tipo}) VALUES (?)";

            try
            {
                using (OdbcConnection connection = con.connection())
                {
                    connection.Open();
                    using (OdbcCommand cmd = new OdbcCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("?", dato);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al insertar datos: " + e.Message);
            }
        }


        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public List<string> ObtenerColumnas(string tabla)
        {
            List<string> columns = new List<string>();

            string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                           "WHERE TABLE_SCHEMA = '" + baseDatos + "' AND TABLE_NAME = '" + tabla + "';";

            try
            {
                using (OdbcConnection connection = this.conn.connection()) // Se abre la conexión
                {
                    connection.Open();

                    using (OdbcCommand cmd = new OdbcCommand(query, connection))
                    {
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                columns.Add(reader.GetString(0));
                            }
                        }
                    }
                } // La conexión se cierra automáticamente aquí
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener columnas: " + e.Message);
            }

            return columns;
        }

        /*
         Fin de la participación de Carlos González
         */


        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public void actualizar(string setClause, string tabla, string condicion)
        {
            try
            {
                string sql = $"UPDATE {tabla} SET {setClause} WHERE {condicion}";

                using (OdbcConnection connection = con.connection()) // Se abre la conexión
                {
                    connection.Open();

                    using (OdbcCommand cmd = new OdbcCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                } // La conexión se cierra automáticamente aquí
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al actualizar: " + e.Message);
            }
        }

        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public void insertarconsulta(string cadena)
        {
            try
            {
                using (OdbcConnection connection = con.connection()) // Abre la conexión
                {
                    connection.Open();
                    using (OdbcCommand consulta = new OdbcCommand(cadena, connection))
                    {
                        consulta.ExecuteNonQuery();
                    }
                } // La conexión se cierra automáticamente aquí
            }
            catch (Exception Error)
            {
                Console.WriteLine("Error en modelo: " + Error.Message);
            }
        }

        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public OdbcDataAdapter llenartb2(string sql)
        {
            try
            {
                using (OdbcConnection connection = con.connection()) // Abre la conexión
                {
                    connection.Open();
                    return new OdbcDataAdapter(sql, connection);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al llenar tabla: " + e.Message);
                return null;
            }
        }

        public string ObtenerTipoDeDato(string seleccion, string tablaN1)
        {
            string Tipo = string.Empty;
            using (OdbcConnection conn = con.connection())
            {
                string query = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA ='" + baseDatos + "' AND TABLE_NAME = '" + tablaN1 + "' AND COLUMN_NAME = '" + seleccion + "'";
                using (OdbcCommand cmd = new OdbcCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        Tipo = result.ToString();
                    }
                }
            } return Tipo;
        }

        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public DataTable BuscarPor2(string datobuscar, string buscaren, string operador, string tableN, DataTable dt, Label lbl_cadena)
        {
            if (string.IsNullOrEmpty(buscaren))
            {
                MessageBox.Show("El campo buscar se encuentra vacío.");
                return dt;
            }

            try
            {
                string cadenaB = "SELECT * FROM " + tableN + " WHERE " + buscaren + " " + operador + " '" + datobuscar + "';";
                lbl_cadena.Text = "Buscando: " + datobuscar + " en Columna: " + buscaren;

                using (OdbcConnection con = new OdbcConnection("Dsn=HotelSConexion")) // Abre la conexión
                {
                    con.Open();
                    using (OdbcDataAdapter datos = new OdbcDataAdapter(cadenaB, con)) // Usar OdbcDataAdapter con la conexión
                    {
                        datos.Fill(dt); // Rellena el DataTable
                    }
                } // La conexión se cierra automáticamente aquí

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                MessageBox.Show("No se pudo realizar la búsqueda.");
            }

            return dt;
        }



        //modficado por Sebastian Luna

        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public List<string> ObtenerNombresConsultas()
        {
            List<string> nombresConsultas = new List<string>();
            try
            {
                // Consulta para obtener solo los nombres de las consultas
                string query = "SELECT consultaInteligente_nombre_consulta FROM tbl_consultaInteligente;";

                // Ejecutamos el comando con la conexión activa usando 'using'
                using (OdbcConnection con = this.conn.connection()) // Asegúrate de que esta es la conexión activa
                {
                    con.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, con)) // Utilizamos el comando dentro de 'using'
                    using (OdbcDataReader reader = cmd.ExecuteReader()) // El 'OdbcDataReader' también debe ser usado dentro de 'using'
                    {
                        // Añadimos los nombres de consulta a la lista
                        while (reader.Read())
                        {
                            string nombreConsulta = reader.GetString(0);
                            nombresConsultas.Add(nombreConsulta);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener nombres de consultas: " + e.Message);
            }
            return nombresConsultas;
        }

        public string ObtenerQueryPorNombre(string nombreConsulta)
        {
            string query = string.Empty;
            try
            {
                // Consulta SQL para obtener el texto del query por su nombre
                string sql = "SELECT consultaInteligente_consulta_SQLE  FROM tbl_consultaInteligente WHERE  consultaInteligente_nombre_consulta = ?";
                using (OdbcConnection conn = this.conn.connection())
                {
                    using (OdbcCommand cmd = new OdbcCommand(sql, conn))
                    {
                        // Usamos parámetros para evitar inyecciones SQL
                        cmd.Parameters.AddWithValue("?", nombreConsulta);

                        // Ejecutar el comando
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            query = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el query por nombre: " + ex.Message);
            } return query;
        }

        //Cierre de conexiones (Daniel Sierra 0901-21-12740) 14/02/2025
        public OdbcDataAdapter EjecutarQuery(string query)
        {
            try
            {
                // Usamos 'using' para asegurar que la conexión y adaptador se cierren correctamente
                using (OdbcConnection con = this.conn.connection()) // Crea y maneja la conexión
                {
                    con.Open(); // Abre la conexión

                    // Usamos el 'using' para el adaptador de datos, asegurando su cierre adecuado
                    using (OdbcDataAdapter adapter = new OdbcDataAdapter(query, con))
                    {
                        return adapter; // Retornamos el adaptador para que se pueda usar fuera de esta función
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar el query: " + ex.Message);
                return null; // Retorna null en caso de error
            }
        }


        public void EliminarConsulta(string nombreConsulta)
        {
            try
            {
                string sql = "UPDATE tbl_consultaInteligente SET  consultaInteligente_consulta_estatus = 0 WHERE consultaInteligente_nombre_consulta = ?";
                using (OdbcCommand cmd = new OdbcCommand(sql, con.connection()))
                {
                    cmd.Parameters.AddWithValue("@nombreConsulta", nombreConsulta);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al eliminar la consulta: " + e.Message);
            }
        }
        //Fin de participacion de sebastian luna

        // para ayudas
        public string modIndice(string idAyuda)
        {
            string indice = "";
            string query = "SELECT indice FROM ayuda WHERE id_ayuda = ?"; // Parámetro seguro

            using (OdbcCommand command = new OdbcCommand(query,this.conn.connection()))
            {
                command.Parameters.AddWithValue("Id_ayuda", idAyuda);
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        indice = reader.GetString(0); // Asignamos el valor de la columna Indice
                    }
                }
            }

            return indice;
        }
        public string modRuta(string idAyuda)
        {
            string ruta = "";
            string query = "SELECT Ruta FROM ayuda WHERE Id_ayuda = ?"; // Parámetro seguro

            using (OdbcCommand command = new OdbcCommand(query,this.conn.connection()))
            {
                command.Parameters.AddWithValue("id_ayuda", idAyuda);
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ruta = reader.GetString(0); // Asignamos el valor de la columna Ruta
                    }
                }
            }

            return ruta;
        }
    }
}