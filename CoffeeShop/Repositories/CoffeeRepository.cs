using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;

        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id AS CoffeeId, Title, BeanVarietyId, Name, Region, Notes 
                                        FROM Coffee c 
                                        LEFT JOIN BeanVariety b ON c.BeanVarietyId = b.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Coffee> coffees = new List<Coffee>();

                    while (reader.Read())
                    {
                        Coffee coffee = new Coffee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Region = reader.GetString(reader.GetOrdinal("Region"))
                            }

                        };
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }

                        coffees.Add(coffee);
                    }

                    reader.Close();
                    return coffees;
                }
            }
        }

        public Coffee GetCoffeeById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id AS CoffeeId, Title, BeanVarietyId, Name, Region, Notes 
                                        FROM Coffee c 
                                        LEFT JOIN BeanVariety b ON c.BeanVarietyId = b.Id
                                        WHERE c.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Coffee coffee = null;

                    if (reader.Read())
                    {
                        coffee = new Coffee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Region = reader.GetString(reader.GetOrdinal("Region"))
                            }

                        };
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        reader.Close();
                        return coffee;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }    

        public void DeleteCoffee(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Coffee WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddCoffee(Coffee coffee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Coffee (Title, BeanVarietyId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@title, @beanvarietyid)";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanvarietyid", coffee.BeanVarietyId);

                    coffee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void EditCoffee(Coffee coffee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Coffee
                                        SET
                                           Title = @title,
                                           BeanVarietyId = @beanvarietyid
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanvarietyid", coffee.BeanVarietyId);
                    cmd.Parameters.AddWithValue("@id", coffee.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
