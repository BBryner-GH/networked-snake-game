// <copyright file="ControlCommand.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for NetworkController implemented by:
// Brenden Bryner
// Ryan Storm
// November 18, 2025
// </copyright>
using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// ControlCommand class to representing information on how the client will tell the server what it wants to do (directions it is moving)
/// </summary>
public class ControlCommand
{
    /// <summary>
    /// "moving" -
    /// a string representing whether the player wants to move or not, and the desired direction.
    /// Possible values are: "none", "up", "left", "down", "right".
    /// </summary>
    [JsonPropertyName("moving")]
    public string moving { get; set; }

    /// <summary>
    /// empty constructor for serialization purposes
    /// </summary>
    public ControlCommand()
    {
    }
    
    /// <summary>
    /// basic constructor including the direction
    /// </summary>
    /// <param name="direction"></param>
    public ControlCommand(string direction)
    {
        moving = direction;
    }
}