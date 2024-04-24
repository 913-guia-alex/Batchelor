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
                                  "SELECT 'gym' AS role FROM gym WHERE name = @name AND adress = @password";

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
                    cmd.CommandText = "INSERT INTO gym (name, adress, openHour, closeHour, rating) VALUES (@name, @adress, @openHour, @closeHour, @rating)";

                    cmd.Parameters.AddWithValue("@name", gym.name);
                    cmd.Parameters.AddWithValue("@adress", gym.adress);
                    cmd.Parameters.AddWithValue("@openHour", gym.openHour);
                    cmd.Parameters.AddWithValue("@closeHour", gym.closeHour);
                    cmd.Parameters.AddWithValue("@rating", gym.rating);

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
                cmd.CommandText = "UPDATE gym SET name=@name, adress=@adress, openHour=@openHour, closeHour=@closeHour, rating=@rating WHERE idg=@idg";

                cmd.Parameters.AddWithValue("@name", updatedGym.name);
                cmd.Parameters.AddWithValue("@adress", updatedGym.adress);
                cmd.Parameters.AddWithValue("@openHour", updatedGym.openHour);
                cmd.Parameters.AddWithValue("@closeHour", updatedGym.closeHour);
                cmd.Parameters.AddWithValue("@rating", updatedGym.rating);
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









        /*********************************************************************************************************************************/
        /*                                                 Gym+Customer Part                                                             */
        /*********************************************************************************************************************************/









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



    }
}