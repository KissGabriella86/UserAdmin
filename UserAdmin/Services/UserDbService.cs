using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using UserAdmin.Models;

namespace UserAdmin.Services
{
    class UserDbService
    {
        public string ConnectionString = "Server=localhost;Database=useradmin;User=root;Password=;";

        public void Add(User user)
        {
            var connection = new MySqlConnection(ConnectionString); 
            connection.Open();
            string sql = @"INSERT INTO `users`(`username`, `email`, `password`, `registeredAt`) 
VALUES (@username,@email,@password,@registeredAt)"; 
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@registeredAt", user.RegisteredAt);

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public User? FindByEmail(string email)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT `username`, `email`, `password`, `registeredAt` FROM `users` WHERE email = @email";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@email", email);

            var reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                var user = new User
                {
                    Username = reader.GetString(0),
                    Email = reader.GetString(1),
                    Password = reader.GetString(2),
                    RegisteredAt = reader.GetDateTime(3)
                };

                connection.Close();
                return user;
            }
            else
            {
                connection.Close();
                return null;
            }
        }
    }
}
