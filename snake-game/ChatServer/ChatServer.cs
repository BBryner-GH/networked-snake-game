// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// Implemented by Brenden Bryner and Ryan Storm
// November 4, 2025
// </copyright>

using Networking;

// ReSharper disable once CheckNamespace
namespace Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public abstract class ChatServer
{
    
    // instance variables
    
    /// <summary>
    /// Dictionary to hold the client list for message sending
    /// </summary>
    private static readonly Dictionary<NetworkConnection, string?> ClientsList = new();
    
    /// <summary>
    ///   The main program.
    /// </summary>
    private static void Main( string[] _ )
    {
        Server.StartServer( HandleConnect, 11_000 );
        Console.Read(); // don't stop the program.
    }

    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect( NetworkConnection connection )
    {
        // handle all messages until disconnect.
        try
        {
            connection.Send("Please type your name: ");

            // Read the name
            string name = connection.ReadLine() ?? "Read Error";

            // Stop race conditions
            lock (ClientsList)
            {
                ClientsList.Add(connection, name);
            }

            // Send confirmation
            connection.Send($"thanks {name}!");

            while (true)
            {
                string chat = connection.ReadLine();
                string message = $"{name}: {chat}";

                Broadcast(message);
            }
        }
        catch
        {
            // ignored
        }
        finally
        {
            lock (ClientsList)
            {
                ClientsList.Remove(connection);
            }

            try
            { connection.Disconnect(); }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// helper method for broadcasting messages
    /// </summary>
    /// <param name="message"></param>
    private static void Broadcast(string message)
    {
        // copy of the client list
        List<NetworkConnection> copy;
        lock (ClientsList)
        {
            copy = ClientsList.Keys.ToList();
        }
        
        // iterate through all the keys to send message
        foreach (var clientConnection in copy)
        {
            try
            {
                // send message
                clientConnection.Send(message);
            }
            catch
            {
                lock (ClientsList)
                {
                    // if the sending fails remove the the client from the list
                    ClientsList.Remove(clientConnection);
                }
                // get rid of the connection 
                clientConnection.Dispose();
            }
        }
    }
}