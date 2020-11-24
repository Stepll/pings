using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

using MySql.Data.MySqlClient;

namespace test1
{
    class Program
    {
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
                Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString());
            }
            reader.Close();

            // get ping

            Ping ping = new Ping();
            PingReply reply = ping.Send("www.google.com");
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine($"time: {reply.RoundtripTime}");
                i++;
                string query = $"INSERT INTO pings (id, Ping) VALUES ({i} , {reply.RoundtripTime})";

                MySqlCommand insertCommand = new MySqlCommand(query, connection);

                insertCommand.ExecuteNonQuery();

            }
            Console.ReadLine();

            connection.Close();
        }
    }
}
          