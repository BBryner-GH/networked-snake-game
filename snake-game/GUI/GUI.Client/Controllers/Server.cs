// <copyright file="Server.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// Implemented by Brenden Bryner and Ryan Storm
// November 4, 2025
// </copyright>


using System.Net;
using System.Net.Sockets;
using GUI.Client.Controllers;

namespace Networking;

/// <summary>
///   Represents a server task that waits for connections on a given
///   port and calls the provided delegate when a connection is made.
/// </summary>
public static class Server
{
    /// <summary>
    ///   Wait on a TcpListener for new connections. Alert the main program
    ///   via a callback (delegate) mechanism.
    /// </summary>
    /// <param name="handleConnect">
    ///   Handler for what the user wants to do when a connection is made.
    ///   This should be run asynchronously via a new thread.
    /// </param>
    /// <param name="port"> The port (e.g., 11000) to listen on. </param>
    public static void StartServer( Action<NetworkConnection> handleConnect, int port )
    {
        //start listener
        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        
        while(true){
            //accept client
            TcpClient client = listener.AcceptTcpClient(); // blocking?
            NetworkConnection connection = new NetworkConnection( client );
            
            
            //handle client asynchronously
            new Thread(() => handleConnect( connection )).Start();
        }
    }
}