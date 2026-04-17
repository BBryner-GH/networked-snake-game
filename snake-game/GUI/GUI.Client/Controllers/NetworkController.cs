// <copyright file="NetworkController.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for NetworkController implemented by:
// Brenden Bryner
// Ryan Storm
// November 18, 2025
// </copyright>

using System.Data;
using System.Text.Json;
using Blazor.Extensions.Canvas.Canvas2D;
using GUI.Client.Models;

namespace GUI.Client.Controllers;

/// <summary>
/// NetworkController class that holds logic for a network controller to parse information recieved 
/// from the network and updating the model based on that information.
/// </summary>
public class NetworkController
{
    // instance variables
    private NetworkConnection _connection;
    private int _playerID;
    private World _world;
    private bool _isConnected = false;
    private DatabaseController _databaseController;
    
    public World World => _world;
    public int PlayerID => _playerID;
    
    // ---------- Connection Methods ----------
    
    /// <summary>
    /// this method handles the player connection and initialization of the model
    /// </summary>
    /// <param name="serverAddress">the server's address to connect to</param>
    /// <param name="port">the port to connect to</param>
    /// <param name="playerName">the name of the player</param>
    public async Task ConnectControllerAsync(String serverAddress, int port, string playerName)
    {
        // Create the NetworkConnection instance
        _connection = new NetworkConnection();
        
        // Create the Database Controller
        _databaseController = new DatabaseController();
        
        // Create the connection
        await _connection.ConnectConnectionAsync(serverAddress, port);
        _isConnected = true;
        
        // Send the player name
        await _connection.WriteAsync(playerName);
        
        // Read id and parse
        string id = await _connection.ReadLineAsync();
        _playerID = int.Parse(id);
        
        // Read world size, parse, create world
        string size = await _connection.ReadLineAsync();
        int worldSize = int.Parse(size);
        _world = new World(worldSize);

        // Loop read walls and update world model instance
        while (true)
        {
            string message = await _connection.ReadLineAsync();
            if (message.Contains("\"wall\""))
            {
                Wall? wall = JsonSerializer.Deserialize<Wall>(message);
                if (wall != null) _world.AddWall(wall);
            }
            else
            {
                Update(message);
                break;
            }
        }

        // Create a new thread to read server messages until disconnect
        _ = Task.Run(RecieveServerDataAsync);
    }
    
    /// <summary>
    /// Disconnects the player from the network
    /// </summary>
    public void Disconnect()
    {
        if (!_isConnected) return;
        _isConnected = false;
        _connection.Disconnect();

        foreach (int snakeID in _world.Snakes.Keys)
        {
            _databaseController.UpdatePlayerLeave(snakeID, DateTime.Now);
        }
        
        _databaseController.EndGame(DateTime.Now);
    }
    
    /// <summary>
    /// this method asynchronously waits for new messages and sends them to update while the player is still connected
    /// otherwise, if the connection is broken the player will be disconnected
    /// </summary>
    private async Task RecieveServerDataAsync()
    {
        try
        {
            // Keep reading message while connected
            while (_isConnected)
            {
                string message = await _connection.ReadLineAsync();
                Update(message);
            }
        }
        catch (Exception)
        {

        }
    }
    
    // ---------- Data Methods ----------
    
    /// <summary>
    /// this method parses JSON strings and calls the respective method in World to update the model
    /// </summary>
    /// <param name="message">the json message to be parsed and updated</param>
    private void Update(string message)
    {
        // processing  snake or powerups
        if (message.Contains("\"snake\""))
        {
            Snake? snake = JsonSerializer.Deserialize<Snake>(message);
            if (snake != null)
            {
                _world.UpdateSnake(snake, out Boolean updateDB, out Boolean newPlayer);
                
                // check if db needs to be updated
                if (snake.dc) 
                    _databaseController.UpdatePlayer(snake.snakeID, snake.name, snake.MaxScore, DateTime.Now);
                else if (updateDB && !newPlayer)
                    _databaseController.UpdatePlayer(snake.snakeID, snake.name, snake.MaxScore, null);
                else if (updateDB && newPlayer)
                    _databaseController.AddPlayer(snake.snakeID, snake.name, snake.MaxScore, DateTime.Now, null);
            }
        } else if (message.Contains("\"power\""))
        {
            Powerup? power = JsonSerializer.Deserialize<Powerup>(message);
            if (power != null)
            {
                _world.UpdatePowerup(power);
            }
        } else if (message.Contains("\"wall\""))
        {
            Wall? wall = JsonSerializer.Deserialize<Wall>(message);
            if (wall != null)
            {
                _world.AddWall(wall);
            }
        }
    }

    /// <summary>
    /// this method sends a JSON control command to the server
    /// </summary>
    /// <param name="commandMessage">the direction of the command</param>
    public async Task SendMovementCommand(string commandMessage)
    {
        ControlCommand command = new ControlCommand(commandMessage);
        string message = JsonSerializer.Serialize(command);
        await _connection.WriteAsync(message);
    }
    
}