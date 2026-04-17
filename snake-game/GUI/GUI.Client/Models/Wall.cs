// <copyright file="Wall.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for NetworkController implemented by:
// Brenden Bryner
// Ryan Storm
// November 18, 2025
// </copyright>
using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// represents a wall in the game
/// </summary>
public class Wall
{
    /// <summary>
    /// "wall" - an int representing the wall's unique ID.
    /// </summary>
    [JsonPropertyName("wall")]
    public int wallID { get; set; }
    
    /// <summary>
    /// "p1" - a Point2D representing one endpoint of the wall.
    /// </summary>
    [JsonPropertyName("p1")]
    public Point2D p1  { get; set; }
    
    /// <summary>
    /// "p2" - a Point2D representing the other endpoint of the wall.
    /// </summary>
    [JsonPropertyName("p2")]
    public Point2D p2  { get; set; }

    /// <summary>
    /// default constructor for the JSON deserialization
    /// </summary>
    public Wall()
    {
        
    }

}