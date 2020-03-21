using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;

namespace CareerCloud.ADODataAccessLayer
{
    public class CompanyDescriptionRepository : IDataRepository<CompanyDescriptionPoco>
    {
        private readonly string _connStr;
        public CompanyDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (CompanyDescriptionPoco poco in items)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"INSERT INTO [dbo].[Company_Descriptions]
                                            ([Id]
                                            ,[Company]
                                            ,[LanguageID]
                                            ,[Company_Name]
                                            ,[Company_Description])
                                        VALUES
                                            (@Id
                                            ,@Company
                                            ,@LanguageID
                                            ,@Company_Name
                                            ,@Company_Description)";

                    cmd.Parameters.AddWithValue("Id", poco.Id);
                    cmd.Parameters.AddWithValue("Company", poco.Company);
                    cmd.Parameters.AddWithValue("LanguageID", poco.LanguageId);
                    cmd.Parameters.AddWithValue("Company_Name", poco.CompanyName);
                    cmd.Parameters.AddWithValue("Company_Description", poco.CompanyDescription);
                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyDescriptionPoco> GetAll(params System.Linq.Expressions.Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT [Id]
                                    ,[Company]
                                    ,[LanguageID]
                                    ,[Company_Name]
                                    ,[Company_Description]
                                    ,[Time_Stamp]
                                FROM [dbo].[Company_Descriptions]";

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                CompanyDescriptionPoco[] pocos = new CompanyDescriptionPoco[1000];
                int index = 0;

                while (reader.Read())
                {
                    CompanyDescriptionPoco poco = new CompanyDescriptionPoco();
                    poco.Id = reader.GetGuid(0);
                    poco.Company = reader.GetGuid(1);
                    poco.LanguageId = reader.GetString(2);
                    poco.CompanyName = reader.GetString(3);
                    poco.CompanyDescription = reader.GetString(4);
                    poco.TimeStamp = (byte[])reader[5];
                    pocos[index] = poco;
                    index++;
                }
                connection.Close();
                return pocos.Where(async => async != null).ToList();
            }
        }

        public IList<CompanyDescriptionPoco> GetList(System.Linq.Expressions.Expression<Func<CompanyDescriptionPoco, bool>> where, params System.Linq.Expressions.Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyDescriptionPoco GetSingle(System.Linq.Expressions.Expression<Func<CompanyDescriptionPoco, bool>> where, params System.Linq.Expressions.Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyDescriptionPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                foreach (CompanyDescriptionPoco poco in items)
                {
                    cmd.CommandText = @"DELETE Company_Descriptions
                                        WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", poco.Id);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (CompanyDescriptionPoco poco in items)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"UPDATE [dbo].[Company_Descriptions]
                                            SET 
                                                [Company] = @Company
                                                ,[LanguageID] = @LanguageID
                                                ,[Company_Name] = @Company_Name
                                                ,[Company_Description] = @Company_Description
                                            WHERE [Id] = @Id";

                    cmd.Parameters.AddWithValue("Id", poco.Id);
                    cmd.Parameters.AddWithValue("Company", poco.Company);
                    cmd.Parameters.AddWithValue("LanguageID", poco.LanguageId);
                    cmd.Parameters.AddWithValue("Company_Name", poco.CompanyName);
                    cmd.Parameters.AddWithValue("Company_Description", poco.CompanyDescription);
                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
