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

    class CheckPing
    {
        private MySqlConnection connection;
        MySqlCommand SelectCommand;
        private int i = 0; // iteretor
        private int N, Period;
        private string url;

        public CheckPing(int N, int Period, string url)
        {
            this.N = N;
            this.Period = Period;
            this.url = url;

            connection = new MySqlConnection("server=localhost;user=root;database=pings;password=1234567895;");

            connection.Open();

            SelectCommand = new MySqlCommand("SELECT id, Ping FROM pings", connection);

            MySqlDataReader reader = SelectCommand.ExecuteReader();

            while (reader.Read())
            {
                i++;
                //Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString()); // current database
            }
            reader.Close();
        }

        public void show()
        {
            MySqlDataReader reader = SelectCommand.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString());
            }
            reader.Close();
        }

        public void run()
        {
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
        }

        ~CheckPing()
        {
            connection.Close();
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
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

            CheckPing Exemple = new CheckPing(N, Period, url);
            Exemple.run();
            Exemple.show();

            Console.ReadLine();
        }
    }
}
          