// <copyright file="Powerup.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for NetworkController implemented by:
// Brenden Bryner
// Ryan Storm
// November 18, 2025
// </copyright>
using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// represents a powerup in the snake game
/// </summary>
public class Powerup
{
    /// <summary>
    /// "power" - an int representing the powerup's unique ID.
    /// </summary>
    [JsonPropertyName("power")]
    public int powerID { get; set; }
    
    /// <summary>
    /// "loc" - a Point2D representing the location of the powerup.
    /// </summary>
    [JsonPropertyName("loc")]
    public Point2D loc { get; set; }
    
    /// <summary>
    /// "died" - a bool indicating if the powerup "died" (was collected by a player) on this frame. The server will send the dead powerups only once.
    /// </summary>
    [JsonPropertyName("died")]
    public bool died { get; set; }

    /// <summary>
    /// default constructor for the serialization
    /// </summary>
    public Powerup()
    {
    }
}