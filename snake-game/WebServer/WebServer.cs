// <copyright file="WebServer.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// Implemented by Brenden Bryner and Ryan Storm
// December 3, 2025
// </copyright>


using System.Text;
using System.Text.RegularExpressions;
using GUI.Client.Controllers;
using MySql.Data.MySqlClient;
using Networking;

namespace WebServer
{
    /// <summary>
    /// A web server that accesses the snake game database and represents the database with tables on a page
    /// </summary>
    public static class WebServer
    {
        /// <summary>
        /// the Ok header 
        /// </summary>
        private const string httpOkHeader = "HTTP/1.1 200 OK\r\n" +
                                            "Connection: close\r\n" +
                                            "Content-Type: text/html; " +
                                            "charset=UTF-8\r\n";

        /// <summary>
        /// the bad header 
        /// </summary>
        private const string httpBadHeader = "HTTP/1.1 404 Not Found\r\n" +
                                             "Connection:close\r\n" +
                                             "Content-Type: text/html; charset=UTF-8\r\n";

        /// <summary>
        /// connection string to connect to the database
        /// </summary>
        private const string ConnectionString = "server=atr.eng.utah.edu;" +
                                                "database=u1120737;" +
                                                "uid=u1120737;" +
                                                "password=ryan12345storm";
        static void Main()
        {
            Console.WriteLine("Starting web server...");
            // PS8: Listens for connections on port 80, runs a delegate when connected
            Server.StartServer(HandleHttpConnection, 80);
            Console.Read(); // prevent main from returning
        }

        /// <summary>
        /// Handler for the HTTP connection
        /// </summary>
        /// <param name="client">network connection client</param>
        private static void HandleHttpConnection(NetworkConnection client)
        {
            Console.WriteLine("A client connected");

            string message = client.ReadLine();
            // parse the message for the specific game number
            int id = int.Parse(Regex.Match(message, @"\d+").Value);
            Console.WriteLine(message);

            // HOMEPAGE
            if (message.Contains("GET / "))
            {
                string response = httpOkHeader;
                
                string page =
                    "<html>\n  <h3>Welcome to the Snake Games Database!</h3>\n  <a href=\"/games\">View Games</a>\n</html>";
                response += $"Content-Length: {page.Length}\r\n\r\n";
                response += page;
                
                client.Write(response);
            }
            // SPECIFIC GAME PAGE
            else if (message.Contains("GET /games?gid="))
            {
                string response = httpOkHeader;
                
                string page = GetSpecificGamePage(id);
                response += $"Content-Length: {page.Length}\r\n\r\n";
                response += page;
                
                client.Write(response);
            }
            // ALL GAMES PAGE
            else if (message.Contains("GET /games"))
            {
                string response = httpOkHeader;
                
                string page = GetAllGamesPage();
                response += $"Content-Length: {page.Length}\r\n\r\n";
                response += page;
                
                client.Write(response);
            }
            // BAD WEBPAGE 
            else
            {
                string response = httpBadHeader;
                string page = "that is a bad webpage";
                response += $"Content-Length: {page.Length}\r\n\r\n";
                response += page;
                client.Write(response);
            }
        }

        // HELPERS FOR GENERATING PAGES -----

        /// <summary>
        /// Helper method to get all of the games from the database and represent them in HTML
        /// </summary>
        /// <returns>returns a string version of the HTML</returns>
        private static string GetAllGamesPage()
        {
            // create string builder and append the first part of the table
            StringBuilder html = new StringBuilder();
            html.AppendLine(
                "<html><table border=\"1\"><thead><tr><td>ID</td><td>Start</td><td>End</td></tr></thead><tbody>");

            // connect to DB then add tables 
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ID, StartTime, EndTime FROM Games";

                //loop through the reader and get all of the data
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["ID"];
                        string start = reader["StartTime"].ToString();
                        string end = reader["EndTime"].ToString();
                        html.Append($"<tr><td><a href=\"games?gid={id}\">{id}<a/<td><td>{start}</td><td>{end}</td></a></tr>");
                    }
                }
            }

            // close html tag and return
            html.Append("</tbody></table></html>");
            return html.ToString();
        }

        /// <summary>
        /// Helper method to get the specific game pages
        /// </summary>
        /// <param name="gameID">specific game ID for the game that is going to be represented</param>
        /// <returns>string of the HTML </returns>
        private static string GetSpecificGamePage(int gameID)
        {
            StringBuilder html = new StringBuilder();
            html.Append($"<html><h3>Stats page for game {gameID}</h3><table border=\"1\"><thead><tr><td>Player ID</td><td>Player Name</td><td>Max Score</td><td>Enter Time</td><td>Leave Time</td></tr></thead><tbody>");

            // connect to the DB and get all of information that is needed
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"Select ID, Name, MaxScore, EnterTime, LeaveTime FROM Players WHERE gameID={gameID}";

                // loop through the reader and append the data with HTML
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["ID"];
                        string name = reader["Name"].ToString();
                        int maxScore = (int)reader["MaxScore"];
                        string enterTime = reader["EnterTime"].ToString();
                        string leaveTime = reader["LeaveTime"].ToString();
                        
                        html.Append($"<tr><td>{id}</td><td>{name}</td><td>{maxScore}</td><td>{enterTime}</td><td>{leaveTime}</td></tr>");
                    }
                }
            }
            
            //close the HTML tag and return the string
            html.Append("</tbody></table></html>");
            return html.ToString();
        }

    }
}

