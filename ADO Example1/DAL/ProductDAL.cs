using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using ADO_Example1.Models;

namespace ADO_Example1.DAL
{
    public class ProductDAL
    {
        string conString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();

        //Get All Products

        public List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqlData = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlData.Fill(dtProducts);
                connection.Close();


                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        Productid = Convert.ToInt32(dr["Productid"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString(),
                    });
                }
            }

            return productList;
        }
      
        //Insert Products

        public bool InsertProduct(Product product)
        {
            int id = 0;
            using(SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InserProducts",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName",product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();
                if (id>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }


        //Get Products By Product ID

        public List<Product> GetProductsByID(int ProductID)
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductByID";
                command.Parameters.AddWithValue("@ProductID",ProductID);
                SqlDataAdapter sqlData = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlData.Fill(dtProducts);
                connection.Close();


                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        Productid = Convert.ToInt32(dr["Productid"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString(),
                    });
                }
            }

            return productList;
        }

        //Update Products

        public bool UpdatetProduct(Product product)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", product.Productid);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        //Delete Product
        public string DeleteProduct(int productid)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DelectProduct", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Productid", productid);
                command.Parameters.Add("@OUTPUTMESSAGE ", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OUTPUTMESSAGE "].Value.ToString();
                connection.Close();;
            }
            return result;
        }
    }
}