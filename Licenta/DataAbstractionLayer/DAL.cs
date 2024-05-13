using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using Licenta.Models;
//using Org.BouncyCastle.Asn1;

namespace Licenta.DataAbstractionLayer
{
    public class DAL
    {

        public MySqlConnection getConnection()
        {
            string myConnectionString;
            myConnectionString = "Server=localhost;Database=licenta;Uid=root;Pwd=;";
            return new MySqlConnection(myConnectionString);

        }








        /*********************************************************************************************************************************/
        /*                                        Login/RegisterResetPassword Part                                                       */
        /*********************************************************************************************************************************/







        public string Login(string name, string password)
        {
            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT 'admin' AS role FROM admin WHERE adminname = @name AND password = @password " +
                                  "UNION " +
                                  "SELECT 'customer' AS role FROM customer WHERE email = @name AND password = @password " +
                                  "UNION " +
                                  "SELECT 'gym' AS role FROM gym WHERE name = @name AND password = @password";

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@password", password);

                MySqlDataReader myreader = cmd.ExecuteReader();

                string role = null;

                if (myreader.Read())
                {
                    role = myreader.GetString("role");
                }

                myreader.Close();
                connection.Close();

                return role;
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return null;
            }
        }
    


        public bool ResetPassword(string userEmail, string newPassword)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;

                    // Check if the email exists
                    cmd.CommandText = "SELECT COUNT(*) FROM customer WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", userEmail);
                    int existingCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        // Update the password for the user with the specified email
                        cmd.CommandText = "UPDATE customer SET password = @newPassword WHERE email = @email";
                        cmd.Parameters.AddWithValue("@newPassword", newPassword);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }

            return false;
        }


        public bool registercustomer(Customer customer)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;

                    // Check if the username (surname) is already in use
                    cmd.CommandText = "SELECT COUNT(*) FROM customer WHERE surname = @surname";
                    cmd.Parameters.AddWithValue("@surname", customer.surname);
                    int existingCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (existingCount == 0)
                    {
                        // Insert the new customer into the database
                        cmd.CommandText = "INSERT INTO customer (surname, lastname, age, gender, email, password) " +
                                          "VALUES (@surname, @lastname, @age, @gender, @email, @password)";
                        cmd.Parameters.AddWithValue("@lastname", customer.lastname);
                        cmd.Parameters.AddWithValue("@age", customer.age);
                        cmd.Parameters.AddWithValue("@gender", customer.gender);
                        cmd.Parameters.AddWithValue("@email", customer.email);
                        cmd.Parameters.AddWithValue("@password", customer.password);

                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }

            return false;
        }




        public bool registergym(Gym gym)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;

                    // Check if the username (surname) is already in use
                    cmd.CommandText = "SELECT COUNT(*) FROM gym WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", gym.name);
                    int existingCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (existingCount == 0)
                    {
                        // Insert the new customer into the database
                        cmd.CommandText = "INSERT INTO gym (name, adress, openHour, closeHour, rating, password)" +
                            " VALUES (@name, @adress, @openHour, @closeHour, @rating, @password)";

                        cmd.Parameters.AddWithValue("@adress", gym.adress);
                        cmd.Parameters.AddWithValue("@openHour", gym.openHour);
                        cmd.Parameters.AddWithValue("@closeHour", gym.closeHour);
                        cmd.Parameters.AddWithValue("@rating", gym.rating);
                        cmd.Parameters.AddWithValue("@password", gym.password);

                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }

            return false;
        }







        /*********************************************************************************************************************************/
        /*                                            Customer Part                                                                      */
        /*********************************************************************************************************************************/









        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM customer";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                idc = reader.GetInt32("idc"), // Adjust the column names accordingly
                                surname = reader.GetString("surname"),
                                lastname = reader.GetString("lastname"),
                                age = reader.GetInt32("age"),
                                gender = reader.GetString("gender"),
                                email = reader.GetString("email"),
                                password = reader.GetString("password")
                                // Add other properties as needed
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return customers;
        }


        public Customer GetCustomer(int id)
        {
            Customer customer = new Customer();

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM customer WHERE idc = @idc";
                cmd.Parameters.AddWithValue("@idc", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    customer.idc = reader.GetInt32("idc");
                    customer.surname = reader.GetString("surname");
                    customer.lastname = reader.GetString("lastname");
                    customer.age = reader.GetInt32("age");
                    customer.gender = reader.GetString("gender");
                    customer.email = reader.GetString("email");
                    customer.password = reader.GetString("password");

                }

                reader.Close();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }

            return customer;
        }

        public bool AddNewCustomer(Customer customer)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO customer (surname, lastname, age, gender, email, password) VALUES (@surname, @lastname, @age, @gender, @email, @password)";

                    cmd.Parameters.AddWithValue("@surname", customer.surname);
                    cmd.Parameters.AddWithValue("@lastname", customer.lastname);
                    cmd.Parameters.AddWithValue("@age", customer.age);
                    cmd.Parameters.AddWithValue("@gender", customer.gender);
                    cmd.Parameters.AddWithValue("@email", customer.email);
                    cmd.Parameters.AddWithValue("@password", customer.password);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }


        public bool DeleteCustomer(int customerId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM customer WHERE idc = @id";
                    cmd.Parameters.AddWithValue("@id", customerId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }


        public bool UpdateCustomer(Customer updatedCustomer)
        {
            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE customer SET surname=@surname, lastname=@lastname, age=@age, gender=@gender, email=@email, password=@password WHERE idc=@idc";

                cmd.Parameters.AddWithValue("@surname", updatedCustomer.surname);
                cmd.Parameters.AddWithValue("@lastname", updatedCustomer.lastname);
                cmd.Parameters.AddWithValue("@age", updatedCustomer.age);
                cmd.Parameters.AddWithValue("@gender", updatedCustomer.gender);
                cmd.Parameters.AddWithValue("@email", updatedCustomer.email);
                cmd.Parameters.AddWithValue("@password", updatedCustomer.password);
                cmd.Parameters.AddWithValue("@idc", updatedCustomer.idc);

                int rowCount = cmd.ExecuteNonQuery();

                connection.Close();
                return rowCount == 1;
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }

            return false;
        }


        public int GetCustomerIdByEmail(string email)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT idc FROM customer WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            // Return a default value, for example, -1, to indicate failure or not found
            return -1;
        }







        /*********************************************************************************************************************************/
        /*                                                 Gym Part                                                                      */
        /*********************************************************************************************************************************/








        public List<Gym> GetAllGyms()
        {
            List<Gym> gyms = new List<Gym>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM gym";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gym gym = new Gym
                            {
                                idg = reader.GetInt32("idg"),
                                name = reader.GetString("name"),
                                adress = reader.GetString("adress"),
                                openHour = reader.GetString("openHour"),
                                closeHour = reader.GetString("closeHour"),
                                rating = reader.GetFloat("rating"),
                                password = reader.GetString("password"),


                                // Add other properties as needed
                            };
                            gyms.Add(gym);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return gyms;
        }


        public Gym GetGym(int id)
        {
            Gym gym = new Gym();

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM gym WHERE idg = @idg";
                cmd.Parameters.AddWithValue("@idg", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    gym.idg = reader.GetInt32("idg");
                    gym.name = reader.GetString("name");
                    gym.adress = reader.GetString("adress");
                    gym.openHour = reader.GetString("openHour");
                    gym.closeHour = reader.GetString("closeHour");
                    gym.rating = reader.GetFloat("rating");
                    gym.password = reader.GetString("password");

                }

                reader.Close();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }

            return gym;
        }



        public bool AddNewGym(Gym gym)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO gym (name, adress, openHour, closeHour, rating, password) VALUES (@name, @adress, @openHour, @closeHour, @rating, @password)";

                    cmd.Parameters.AddWithValue("@name", gym.name);
                    cmd.Parameters.AddWithValue("@adress", gym.adress);
                    cmd.Parameters.AddWithValue("@openHour", gym.openHour);
                    cmd.Parameters.AddWithValue("@closeHour", gym.closeHour);
                    cmd.Parameters.AddWithValue("@rating", gym.rating);
                    cmd.Parameters.AddWithValue("@password", gym.password);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }


        public bool DeleteGym(int gymId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM gym WHERE idg = @id";
                    cmd.Parameters.AddWithValue("@id", gymId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool UpdateGym(Gym updatedGym)
        {
            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE gym SET name=@name, adress=@adress, openHour=@openHour, closeHour=@closeHour, rating=@rating, password = @password WHERE idg=@idg";

                cmd.Parameters.AddWithValue("@name", updatedGym.name);
                cmd.Parameters.AddWithValue("@adress", updatedGym.adress);
                cmd.Parameters.AddWithValue("@openHour", updatedGym.openHour);
                cmd.Parameters.AddWithValue("@closeHour", updatedGym.closeHour);
                cmd.Parameters.AddWithValue("@rating", updatedGym.rating);
                cmd.Parameters.AddWithValue("@password", updatedGym.password);
                cmd.Parameters.AddWithValue("@idg", updatedGym.idg);

                int rowCount = cmd.ExecuteNonQuery();

                connection.Close();
                return rowCount == 1;
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }

            return false;
        }


        public int GetGymIdByName(string name)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT idg FROM gym WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", name);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return -1;
        }


        public string GetGymNameById(int gymId)
        {
            string gymName = null;

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT name FROM gym WHERE idg = @gymId";
                    cmd.Parameters.AddWithValue("@gymId", gymId);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        gymName = result.ToString();
                    }
                }
            }
            catch (MySqlException e)
            {
                // Handle exceptions as needed
                Console.WriteLine(e.Message);
            }

            return gymName;
        }


        public int GetGymPriceById(int gymId)
        {
            int gymPrice = 0; // Default price

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT price FROM gym WHERE idg = @gymId";
                    cmd.Parameters.AddWithValue("@gymId", gymId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            gymPrice = Convert.ToInt32(reader["price"]);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }

            return gymPrice;
        }







        /*********************************************************************************************************************************/
        /*                                                 Gym+Customer Part                                                             */
        /*********************************************************************************************************************************/












        public List<Favourite> GetFavouriteGymsByCustomerId(int customerId)
        {
            List<Favourite> favouriteGyms = new List<Favourite>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT f.*, g.name AS gymName, g.adress AS gymAddress, g.rating AS gymRating FROM favourite f INNER JOIN gym g ON f.idg = g.idg WHERE f.idc = @customerId";
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Favourite favourite = new Favourite
                            {
                                idf = reader.GetInt32("idf"),
                                idc = reader.GetInt32("idc"),
                                idg = reader.GetInt32("idg"),
                                Gym = new Gym
                                {
                                    idg = reader.GetInt32("idg"),
                                    name = reader.GetString("gymName"),
                                    adress = reader.GetString("gymAddress"),
                                    rating = reader.GetFloat("gymRating")

                                }
                            };

                            favouriteGyms.Add(favourite);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return favouriteGyms;
        }






        /*********************************************************************************************************************************/
        /*                                                 Favourite List Part                                                           */
        /*********************************************************************************************************************************/






        public bool RemoveFromFavorites(int favouriteId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM favourite WHERE idf = @favouriteId";
                    cmd.Parameters.AddWithValue("@favouriteId", favouriteId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }


        public bool IsGymInFavorites(int customerId, int gymId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT COUNT(*) FROM Favourite WHERE idc = @customerId AND idg = @gymId";
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@gymId", gymId);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool AddToFavourite(int customerId, int gymId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO Favourite (idc, idg) VALUES (@customerId, @gymId)";
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@gymId", gymId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }







        /*********************************************************************************************************************************/
        /*                                                 Coaches Part                                                                      */
        /*********************************************************************************************************************************/








        public List<Coach> GetAllCoaches()
        {
            List<Coach> coaches = new List<Coach>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM coach";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Coach coach = new Coach
                            {
                                idco = reader.GetInt32("idco"),
                                name = reader.GetString("name"),
                                age = reader.GetInt32("age"),
                                gender = reader.GetString("gender"),
                                trainerType = reader.GetString("trainerType"),
                                phoneNumber = reader.GetString("phoneNumber"),
                                email = reader.GetString("email"),
                                photo = (byte[])reader["photo"], // Retrieve photo as byte array
                                idg = reader.GetInt32("idg")
                                // Add other properties as needed
                            };
                            coaches.Add(coach);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return coaches;
        }

        public Coach GetCoach(int id)
        {
            Coach coach = new Coach();

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM coach WHERE idco = @idco";
                cmd.Parameters.AddWithValue("@idco", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    coach.idco = reader.GetInt32("idco");
                    coach.name = reader.GetString("name");
                    coach.age = reader.GetInt32("age");
                    coach.gender = reader.GetString("gender");
                    coach.trainerType = reader.GetString("trainerType");
                    coach.phoneNumber = reader.GetString("phoneNumber");
                    coach.email = reader.GetString("email");
                    coach.photo = (byte[])reader["photo"]; // Retrieve photo as byte array
                    coach.idg = reader.GetInt32("idg");
                    // Add other properties as needed
                }

                reader.Close();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }

            return coach;
        }



        public string GetCoachNameById(int id)
        {
            string coachName = string.Empty;

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT name FROM coach WHERE idco = @idco";
                cmd.Parameters.AddWithValue("@idco", id);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    coachName = result.ToString();
                }

                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }

            return coachName;
        }


        public bool AddNewCoach(Coach coach)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO coach (name, age, gender, trainerType, phoneNumber, email, photo, idg) " +
                                      "VALUES (@name, @age, @gender, @trainerType, @phoneNumber, @email, @photo, @idg)";

                    cmd.Parameters.AddWithValue("@name", coach.name);
                    cmd.Parameters.AddWithValue("@age", coach.age);
                    cmd.Parameters.AddWithValue("@gender", coach.gender);
                    cmd.Parameters.AddWithValue("@trainerType", coach.trainerType);
                    cmd.Parameters.AddWithValue("@phoneNumber", coach.phoneNumber);
                    cmd.Parameters.AddWithValue("@email", coach.email);

                    // Convert photo byte array to MySqlDbType.Blob and pass it as parameter
                    cmd.Parameters.Add("@photo", MySqlDbType.Blob).Value = coach.photo;

                    cmd.Parameters.AddWithValue("@idg", coach.idg);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }



        public bool DeleteCoach(int coachId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM coach WHERE idco = @id";
                    cmd.Parameters.AddWithValue("@id", coachId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }


        public bool UpdateCoach(Coach updatedCoach)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE coach SET name=@name, age=@age, gender=@gender, trainerType=@trainerType, phoneNumber=@phoneNumber, email=@email, photo=@photo, idg=@idg WHERE idco=@id";

                    cmd.Parameters.AddWithValue("@name", updatedCoach.name);
                    cmd.Parameters.AddWithValue("@age", updatedCoach.age);
                    cmd.Parameters.AddWithValue("@gender", updatedCoach.gender);
                    cmd.Parameters.AddWithValue("@trainerType", updatedCoach.trainerType);
                    cmd.Parameters.AddWithValue("@phoneNumber", updatedCoach.phoneNumber);
                    cmd.Parameters.AddWithValue("@email", updatedCoach.email);
                    cmd.Parameters.Add("@photo", MySqlDbType.Blob).Value = updatedCoach.photo; // Convert photo byte array to MySqlDbType.Blob and pass it as parameter
                    cmd.Parameters.AddWithValue("@idg", updatedCoach.idg);
                    cmd.Parameters.AddWithValue("@id", updatedCoach.idco);

                    int rowCount = cmd.ExecuteNonQuery();

                    connection.Close();
                    return rowCount == 1;
                }
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }

            return false;
        }


        public List<Coach> GetCoachesByGymId(int gymId)
        {
            List<Coach> coaches = new List<Coach>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM coach WHERE idg = @gymId";
                    cmd.Parameters.AddWithValue("@gymId", gymId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Coach coach = new Coach
                            {
                                idco = reader.GetInt32("idco"),
                                name = reader.GetString("name"),
                                age = reader.GetInt32("age"),
                                gender = reader.GetString("gender"),
                                trainerType = reader.GetString("trainerType"),
                                phoneNumber = reader.GetString("phoneNumber"),
                                email = reader.GetString("email"),
                                photo = (byte[])reader["photo"], // Retrieve photo as byte array
                                idg = reader.GetInt32("idg"),
                                // You may fetch the gym object if needed
                            };

                            coaches.Add(coach);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }

            return coaches;
        }







        /*********************************************************************************************************************************/
        /*                                                 ClassType Part                                                                      */
        /*********************************************************************************************************************************/


        public List<ClassType> GetAllClassTypes()
        {
            List<ClassType> classTypes = new List<ClassType>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM classtype";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClassType classType = new ClassType
                            {
                                idct = reader.GetInt32("idct"),
                                type = reader.GetString("type"),
                                difficulty = reader.GetInt32("difficulty")
                                // Add other properties as needed
                            };
                            classTypes.Add(classType);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return classTypes;
        }

        public ClassType GetClassType(int id)
        {
            ClassType classType = new ClassType();

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM classtype WHERE idct = @idct";
                cmd.Parameters.AddWithValue("@idct", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    classType.idct = reader.GetInt32("idct");
                    classType.type = reader.GetString("type");
                    classType.difficulty = reader.GetInt32("difficulty");
                    // Add other properties as needed
                }

                reader.Close();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return classType;
        }


        public string GetClassTypeTypeById(int id)
        {
            string classTypeType = string.Empty;

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT type FROM classtype WHERE idct = @idct";
                cmd.Parameters.AddWithValue("@idct", id);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    classTypeType = result.ToString();
                }

                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }

            return classTypeType;
        }



        public bool AddNewClassType(ClassType classType)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO classtype (type, difficulty) " +
                                      "VALUES (@type, @difficulty)";

                    cmd.Parameters.AddWithValue("@type", classType.type);
                    cmd.Parameters.AddWithValue("@difficulty", classType.difficulty);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool DeleteClassType(int classTypeId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM classtype WHERE idct = @id";
                    cmd.Parameters.AddWithValue("@id", classTypeId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool UpdateClassType(ClassType updatedClassType)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE classtype SET type=@type, difficulty=@difficulty WHERE idct=@id";

                    cmd.Parameters.AddWithValue("@type", updatedClassType.type);
                    cmd.Parameters.AddWithValue("@difficulty", updatedClassType.difficulty);
                    cmd.Parameters.AddWithValue("@id", updatedClassType.idct);

                    int rowCount = cmd.ExecuteNonQuery();

                    connection.Close();
                    return rowCount == 1;
                }
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }

            return false;
        }





        /*********************************************************************************************************************************/
        /*                                                 Class Part                                                                      */
        /*********************************************************************************************************************************/




        public List<Classes> GetAllClasses()
        {
            List<Classes> classes = new List<Classes>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM class";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Classes classInstance = new Classes
                            {
                                idcl = reader.GetInt32("idcl"),
                                idct = reader.GetInt32("idct"),
                                idco = reader.GetInt32("idco"),
                                name = reader.GetString("name"),
                                description = reader.GetString("description"),
                                date = reader.GetDateTime("date"),
                                time = reader.GetTimeSpan("time")
                                // Add other properties as needed
                            };
                            classes.Add(classInstance);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return classes;
        }


        public Classes GetClass(int id)
        {
            Classes classInstance = new Classes();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM class WHERE idcl = @idcl";
                    cmd.Parameters.AddWithValue("@idcl", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            classInstance.idcl = reader.GetInt32("idcl");
                            classInstance.idco = reader.GetInt32("idco");
                            classInstance.idct = reader.GetInt32("idct");
                            classInstance.name = reader.GetString("name");
                            classInstance.description = reader.GetString("description");
                            classInstance.date = reader.GetDateTime("date");
                            classInstance.time = reader.GetTimeSpan("time");
                            // Add other properties as needed
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return classInstance;
        }



        public bool AddNewClass(Classes newClass)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO class (idco, idct, name, description, date, time) " +
                                      "VALUES (@idco, @idct, @name, @description, @date, @time)";

                    cmd.Parameters.AddWithValue("@idco", newClass.idco);
                    cmd.Parameters.AddWithValue("@idct", newClass.idct);
                    cmd.Parameters.AddWithValue("@name", newClass.name);
                    cmd.Parameters.AddWithValue("@description", newClass.description);
                    cmd.Parameters.AddWithValue("@date", newClass.date);
                    cmd.Parameters.AddWithValue("@time", newClass.time);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool DeleteClass(int classId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM class WHERE idcl = @id";
                    cmd.Parameters.AddWithValue("@id", classId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool UpdateClass(Classes updatedClass)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE class SET idco=@idco, idct=@idct, name=@name, description=@description, date=@date, time=@time WHERE idcl=@id";

                    cmd.Parameters.AddWithValue("@idco", updatedClass.idco);
                    cmd.Parameters.AddWithValue("@idct", updatedClass.idct);
                    cmd.Parameters.AddWithValue("@name", updatedClass.name);
                    cmd.Parameters.AddWithValue("@description", updatedClass.description);
                    cmd.Parameters.AddWithValue("@date", updatedClass.date);
                    cmd.Parameters.AddWithValue("@time", updatedClass.time);
                    cmd.Parameters.AddWithValue("@id", updatedClass.idcl);

                    int rowCount = cmd.ExecuteNonQuery();

                    connection.Close();
                    return rowCount == 1;
                }
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }

            return false;
        }




        public List<Classes> GetClassesByGymId(int gymId)
        {
            List<Classes> gymClasses = new List<Classes>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT c.* 
                                FROM class c 
                                JOIN coach co ON c.idco = co.idco 
                                WHERE co.idg = @gymId";
                    cmd.Parameters.AddWithValue("@gymId", gymId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Classes gymClass = new Classes
                            {
                                idcl = reader.GetInt32("idcl"),
                                idct = reader.GetInt32("idct"),
                                idco = reader.GetInt32("idco"),
                                name = reader.GetString("name"),
                                description = reader.GetString("description"),
                                date = reader.GetDateTime("date"),
                                time = reader.GetTimeSpan("time")
                            };

                            gymClasses.Add(gymClass);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return gymClasses;
        }


        public List<Classes> GetClassesByCoachId(int coachId)
        {
            List<Classes> classes = new List<Classes>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM class WHERE idco = @coachId";
                    cmd.Parameters.AddWithValue("@coachId", coachId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Classes classObj = new Classes
                            {
                                idcl = reader.GetInt32("idcl"),
                                idco = reader.GetInt32("idco"),
                                idct = reader.GetInt32("idct"),
                                name = reader.GetString("name"),
                                description = reader.GetString("description"),
                                date = reader.GetDateTime("date"),
                                time = reader.GetTimeSpan("time")
                                // You may fetch related entities if needed
                            };

                            classes.Add(classObj);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }

            return classes;
        }




        public List<Classes> GetClassesWithCoachByGymId(int gymId)
        {
            List<Classes> gymClasses = new List<Classes>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT c.*, co.name AS coachName
                                FROM class c 
                                JOIN coach co ON c.idco = co.idco 
                                WHERE co.idg = @gymId";
                    cmd.Parameters.AddWithValue("@gymId", gymId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Classes gymClass = new Classes
                            {
                                idcl = reader.GetInt32("idcl"),
                                idct = reader.GetInt32("idct"),
                                idco = reader.GetInt32("idco"),
                                name = reader.GetString("name"),
                                description = reader.GetString("description"),
                                date = reader.GetDateTime("date"),
                                time = reader.GetTimeSpan("time"),
                                CoachName = reader.GetString("coachName") // Retrieve coach name
                            };

                            gymClasses.Add(gymClass);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return gymClasses;
        }


        public List<Coach> GetAllCoachesForGym(int gymId)
        {
            List<Coach> coaches = new List<Coach>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM coaches WHERE idgym = @idgym";
                    cmd.Parameters.AddWithValue("@idgym", gymId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Coach coach = new Coach
                            {
                                // Assuming the column names in the database match the property names in the Coach class
                                idco = Convert.ToInt32(reader["idco"]),
                                name = reader["name"].ToString(),
                                // Add other properties as needed
                            };

                            coaches.Add(coach);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }

            return coaches;
        }







        /*********************************************************************************************************************************/
        /*                                                 Reservation Part                                                                      */
        /*********************************************************************************************************************************/


        public List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM reservation";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservation reservation = new Reservation
                            {
                                idr = reader.GetInt32("idr"),
                                idc = reader.GetInt32("idc"),
                                idcl = reader.GetInt32("idcl"),
                                Date = reader.GetDateTime("date"),
                                Time = reader.GetTimeSpan("time")
                                // Add other properties as needed
                            };
                            reservations.Add(reservation);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return reservations;
        }

        public Reservation GetReservation(int id)
        {
            Reservation reservation = new Reservation();

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM reservation WHERE idr = @idr";
                cmd.Parameters.AddWithValue("@idr", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reservation.idr = reader.GetInt32("idr");
                    reservation.idc = reader.GetInt32("idc");
                    reservation.idcl = reader.GetInt32("idcl");
                    reservation.Date = reader.GetDateTime("date");
                    reservation.Time = reader.GetTimeSpan("time");
                    // Add other properties as needed
                }

                reader.Close();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return reservation;
        }


        public bool AddNewReservation(Reservation reservation)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO reservation (idc, idcl, date, time) " +
                                      "VALUES (@idc, @idcl, @date, @time)";

                    cmd.Parameters.AddWithValue("@idc", reservation.idc);
                    cmd.Parameters.AddWithValue("@idcl", reservation.idcl);
                    cmd.Parameters.AddWithValue("@date", reservation.Date);
                    cmd.Parameters.AddWithValue("@time", reservation.Time);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool DeleteReservation(int reservationId)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM reservation WHERE idr = @id";
                    cmd.Parameters.AddWithValue("@id", reservationId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public bool UpdateReservation(Reservation updatedReservation)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE reservation SET idc=@idc, idcl=@idcl, date=@date, time=@time WHERE idr=@id";

                    cmd.Parameters.AddWithValue("@idc", updatedReservation.idc);
                    cmd.Parameters.AddWithValue("@idcl", updatedReservation.idcl);
                    cmd.Parameters.AddWithValue("@date", updatedReservation.Date);
                    cmd.Parameters.AddWithValue("@time", updatedReservation.Time);
                    cmd.Parameters.AddWithValue("@id", updatedReservation.idr);

                    int rowCount = cmd.ExecuteNonQuery();

                    connection.Close();
                    return rowCount == 1;
                }
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }

            return false;
        }


        public List<Reservation> GetReservationsByCustomerId(int customerId)
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                MySqlConnection connection = getConnection();
                connection.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM reservation WHERE idc = @idc";
                cmd.Parameters.AddWithValue("@idc", customerId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Reservation reservation = new Reservation
                    {
                        idr = reader.GetInt32("idr"),
                        idc = reader.GetInt32("idc"),
                        idcl = reader.GetInt32("idcl"),
                        Date = reader.GetDateTime("date"),
                        Time = reader.GetTimeSpan("time")
                        // Add other properties as needed
                    };

                    reservations.Add(reservation);
                }

                reader.Close();
                connection.Close();
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return reservations;
        }





        /*********************************************************************************************************************************/
        /*                                                 Review Part                                                                      */
        /*********************************************************************************************************************************/


        public List<Review> GetAllReviews()
        {
            List<Review> reviews = new List<Review>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM review";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Review review = new Review
                            {
                                idrev = reader.GetInt32("idrev"),
                                idc = reader.GetInt32("idc"),
                                idg = reader.GetInt32("idg"),
                                description = reader.GetString("description"),
                                rating = reader.GetFloat("rating"),
                                customerEmail = reader.GetString("customerEmail")

                                // Add other properties as needed
                            };
                            reviews.Add(review);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }

            return reviews;
        }



        public void AddReview(Review review)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO review (idc, idg, description, rating, customerEmail) VALUES (@idc, @idg, @description, @rating, @customerEmail)";
                    cmd.Parameters.AddWithValue("@idc", review.idc);
                    cmd.Parameters.AddWithValue("@idg", review.idg);
                    cmd.Parameters.AddWithValue("@description", review.description);
                    cmd.Parameters.AddWithValue("@rating", review.rating);
                    cmd.Parameters.AddWithValue("@customerEmail", review.customerEmail);


                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }
        }


        public List<Models.Review> GetReviewsByGymId(int gymId)
        {
            List<Models.Review> reviews = new List<Models.Review>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    string query = "SELECT * FROM Review WHERE idg = @gymId";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@gymId", gymId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Review review = new Models.Review
                                {
                                    idrev = reader.GetInt32("idrev"),
                                    idc = reader.GetInt32("idc"),
                                    idg = reader.GetInt32("idg"),
                                    description = reader.GetString("description"),
                                    rating = reader.GetFloat("rating"),
                                    customerEmail = reader.GetString("customerEmail")
                                    // You might need to fetch Gym and Customer objects based on their IDs
                                };

                                // Add the review to the list
                                reviews.Add(review);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }

            return reviews;
        }













        public List<GymCard> GetGymCardsByCustomerId(int customerId)
        {
            List<GymCard> gymCards = new List<GymCard>();

            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT gc.*, g.name AS gymName FROM gymcard gc INNER JOIN gym g ON gc.idg = g.idg WHERE gc.idc = @customerId";
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GymCard gymCard = new GymCard
                            {
                                idgc = reader.GetInt32("idgc"),
                                idc = reader.GetInt32("idc"),
                                idg = reader.GetInt32("idg"),
                                price = reader.GetInt32("price"),
                                madeDate = reader.GetDateTime("madeDate"),
                                expirationDate = reader.GetDateTime("expirationDate"),
                                Gym = new Gym
                                {
                                    idg = reader.GetInt32("idg"),
                                    name = reader.GetString("gymName"),
                                    // Add other properties as needed
                                }
                            };

                            gymCards.Add(gymCard);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                // Handle exceptions as needed
            }

            return gymCards;
        }



        public void AddGymCardToDatabase(GymCard gymCard)
        {
            try
            {
                using (MySqlConnection connection = getConnection())
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO gymcard (idc, idg, price, madeDate, expirationDate) VALUES (@idc, @idg, @price, @madeDate, @expirationDate)";
                    cmd.Parameters.AddWithValue("@idc", gymCard.idc);
                    cmd.Parameters.AddWithValue("@idg", gymCard.idg);
                    cmd.Parameters.AddWithValue("@price", gymCard.price);
                    cmd.Parameters.AddWithValue("@madeDate", gymCard.madeDate);
                    cmd.Parameters.AddWithValue("@expirationDate", gymCard.expirationDate);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                // Handle exceptions as needed
            }
        }


    }
}