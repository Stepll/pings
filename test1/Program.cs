using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

using MySql.Data.MySqlClient;

namespace test1
{
    class Program
    {
        static void show(MySqlCommand command)
        {
            MySqlDataReader reader1 = command.ExecuteReader();

            while (reader1.Read())
            {
                Console.WriteLine(reader1[0].ToString() + " " + reader1[1].ToString());
            }
            reader1.Close();
        }

        static void Main(string[] args)
        {
            int i = 0; // iterator

            // sql

            string connectstr = "server=localhost;user=root;database=pings;password=1234567895;";

            MySqlConnection connection = new MySqlConnection(connectstr);

            connection.Open();

            string sql = "SELECT id, Ping FROM pings";

            MySqlCommand command = new MySqlCommand(sql, connection);

            MySqlDataReader reader = command.ExecuteReader();

            while ( reader.Read() )
            {
                i++;
                //Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString());
            }
            reader.Close();

            // get ping


            
            Console.Write("enter the number of checks: ");
            string readN = Console.ReadLine();
            int N = Convert.ToInt32(readN);

            Console.Write("enter check period (ms): ");
            string readPeriod = Console.ReadLine();
            int Period = Convert.ToInt32(readPeriod);

            Console.Write("enter url (default \"www.google.com\"): ");
            string url = Console.ReadLine();
            if (url == "")
                url = "www.google.com";


            for (int j = 0; j < N; j++)
            {
                Thread.Sleep(Period);
                Ping ping = new Ping();
                PingReply reply = ping.Send(url);
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"time: {reply.RoundtripTime}");
                    i++;
                    string query = $"INSERT INTO pings (id, Ping) VALUES ({i} , {reply.RoundtripTime})";

                    MySqlCommand insertCommand = new MySqlCommand(query, connection);

                    insertCommand.ExecuteNonQuery();

                }
            }

            show(command);

            Console.ReadLine();

            connection.Close();
        }
    }
}
          