using System.Data.SqlClient;
using DameChanceSV2.Models;
using DameChanceSV2.Models;
using Microsoft.Data.SqlClient;

namespace DameChanceSV2.DAL
{
    public class UsuarioDAL
    {
        private readonly Database _database;

        public UsuarioDAL(Database database)
        {
            _database = database;
        }

        // Inserta un nuevo usuario y retorna el Id insertado.
        public int InsertUsuario(Usuario usuario)
        {
            int newId = 0;
            using (SqlConnection conn = _database.GetConnection())
            {
                string query = @"
            INSERT INTO Usuarios (Nombre, Correo, Contrasena, Estado, RolId)
            OUTPUT INSERTED.Id
            VALUES (@Nombre, @Correo, @Contrasena, @Estado, @RolId)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                    cmd.Parameters.AddWithValue("@RolId", usuario.RolId);

                    conn.Open();
                    newId = (int)cmd.ExecuteScalar();
                }
            }
            return newId;
        }

        public Usuario GetUsuarioByCorreo(string correo)
        {
            Usuario usuario = null;
            using (SqlConnection conn = _database.GetConnection())
            {
                string query = "SELECT Id, Nombre, Correo, Contrasena, Estado FROM Usuarios WHERE Correo = @Correo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Correo = reader.GetString(2),
                                Contrasena = reader.GetString(3),
                                Estado = reader.GetBoolean(4)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        // Dentro de la clase UsuarioDAL en DameChance/DAL/UsuarioDAL.cs
        public void UpdateEstado(int usuarioId, bool estado)
        {
            using (SqlConnection conn = _database.GetConnection())
            {
                string query = "UPDATE Usuarios SET Estado = @Estado WHERE Id = @UsuarioId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateContrasena(int userId, string nuevaContrasenaHash)
        {
            using (SqlConnection conn = _database.GetConnection())
            {
                string query = "UPDATE Usuarios SET Contrasena = @Contrasena WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Contrasena", nuevaContrasenaHash);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }
}
