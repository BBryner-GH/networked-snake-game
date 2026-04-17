// <copyright file="NetworkController.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for DatabaseController implemented by:
// Brenden Bryner
// Ryan Storm
// November 30, 2025
// </copyright>

using System.Text;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;

namespace GUI.Client.Controllers;

/// <summary>
/// Database controller that holds logic to communicate with the database for adding and updating
/// players to each game
/// </summary>
public class DatabaseController
{
    
    // --- Connection String ---
    
    /// <summary>
    /// The connection string.
    /// Your uID login name serves as both your database name and your uid
    /// </summary>
    private const string ConnectionString = "server=atr.eng.utah.edu;" +
                                            "database=u1120737;" +
                                            "uid=u1120737;" +
                                            "password=ryan12345storm";
    // instance variables
    private int GameID = 0;

    /// <summary>
    /// The constructor for the database controller. Creates a new game row and records
    /// the game id in the GameID instance variable
    /// </summary>
    public DatabaseController()
    {
        string insertGame = @"
            INSERT INTO u1120737.Games (startTime)
            VALUES (@DateTime);
        ";
        
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            
            // Create the row
            MySqlCommand command = new MySqlCommand(insertGame, connection);
            command.Parameters.AddWithValue("@DateTime", DateTime.Now);
            command.ExecuteNonQuery();
            
            // Grab the game ID
            MySqlCommand insertCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", connection);
            var gameID = insertCommand.ExecuteScalar();
            GameID =  Convert.ToInt32(gameID);
        }
    }

    /// <summary>
    /// This method adds a player to the players table. It will overwrite a player
    /// if it already exists 
    /// </summary>
    /// <param name="playerID">the player snake ID</param>
    /// <param name="playerName">the player snake name</param>
    /// <param name="maxScore">the player's snake max score</param>
    /// <param name="enterTime">the time the player entered the game</param>
    /// <param name="leaveTime">the time the player left the game</param>
    public void AddPlayer(int playerID, string playerName, int maxScore, DateTime enterTime,  DateTime? leaveTime)
    {
        string query = @"
                          INSERT INTO u1120737.Players (ID, gameID, name, maxscore, entertime, leavetime) 
                          VALUES (@ID, @GameID, @Name, @MaxScore, @EnterTime, @LeaveTime) 
                          ON DUPLICATE KEY UPDATE 
                              name = VALUES(name),
                              maxscore = VALUES(maxscore), 
                              leavetime = VALUES(leavetime)";
        
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            
            // create command and execute
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", playerID);
            command.Parameters.AddWithValue(@"GameID", GameID);
            command.Parameters.AddWithValue("@Name", playerName);
            command.Parameters.AddWithValue("@MaxScore", maxScore);
            command.Parameters.AddWithValue("@EnterTime", enterTime);
            command.Parameters.AddWithValue("@LeaveTime", leaveTime);
            command.ExecuteNonQuery();
        }
    }


    /// <summary>
    /// This method updates a player on the players table. 
    /// </summary>
    /// <param name="playerID">the player snake ID</param>
    /// <param name="playerName">the player snake name</param>
    /// <param name="maxScore">the player's snake max score</param>
    /// <param name="leaveTime">the time the player left the game</param>
    public void UpdatePlayer(int playerID, string playerName, int maxScore, DateTime? leaveTime)
    {
        string query = @"
                          INSERT INTO u1120737.Players (ID, gameID, name, maxscore, leavetime) 
                          VALUES (@ID, @GameID, @Name, @MaxScore, @LeaveTime) 
                          ON DUPLICATE KEY UPDATE 
                              name = VALUES(name),
                              maxscore = VALUES(maxscore), 
                              leavetime = VALUES(leavetime)";
        
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            
            // create command and execute
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", playerID);
            command.Parameters.AddWithValue("@GameID", GameID);
            command.Parameters.AddWithValue("@Name", playerName);
            command.Parameters.AddWithValue("@MaxScore", maxScore);
            command.Parameters.AddWithValue("@LeaveTime", leaveTime);
            command.ExecuteNonQuery();
        }
    }
    
    /// <summary>
    /// This method updates a player's leave time
    /// </summary>
    /// <param name="playerID">the player snake ID</param>
    /// <param name="leaveTime">the time the player left the game</param>
    public void UpdatePlayerLeave(int playerID, DateTime leaveTime)
    {
        string playerLeaveCommand = @"
                          UPDATE u1120737.Players
                          SET leavetime = @LeaveTime
                          WHERE ID = @ID AND gameID = @GameID;
                       ";
        
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            
            // create command and execute
            MySqlCommand leaveCommand = new MySqlCommand(playerLeaveCommand, connection);
            leaveCommand.Parameters.AddWithValue("@ID", playerID);
            leaveCommand.Parameters.AddWithValue(@"GameID", GameID);
            leaveCommand.Parameters.AddWithValue("@LeaveTime", leaveTime);
            leaveCommand.ExecuteNonQuery();
        }
        
        
    }

    /// <summary>
    /// Sets the end time for the current game
    /// </summary>
    /// <param name="endTime">the time the game ended</param>
    public void EndGame(DateTime endTime)
    {
        string gameEndCommand = @"
                          UPDATE u1120737.Games
                          SET endtime = @EndTime
                          WHERE ID = @ID;
                       ";
        
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            
            MySqlCommand endCommand = new MySqlCommand(gameEndCommand, connection);
            endCommand.Parameters.AddWithValue("@ID", GameID);
            endCommand.Parameters.AddWithValue("@EndTime", endTime);
            endCommand.ExecuteNonQuery();
        }
    }
    
}