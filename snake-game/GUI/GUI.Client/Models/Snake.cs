// <copyright file="Snake.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for NetworkController implemented by:
// Brenden Bryner
// Ryan Storm
// November 18, 2025
// </copyright>
using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// represents a snake in the game
/// </summary>
public class Snake
{
    
    /// <summary>
    /// "snake" - an int representing the snake's unique ID.  
    /// </summary>
    [JsonPropertyName("snake")]
    public int snakeID { get; set; }
    
    /// <summary>
    /// "name" - a string representing the player's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string name { get; set; }
    
    /// <summary>
    /// "body" - a List<Point2D> representing the entire body of the snake.
    /// </summary>
    [JsonPropertyName("body")]
    public List<Point2D> body { get; set; }
    
    /// <summary>
    /// "dir" - a Point2D representing the snake's orientation.
    /// This will always be an axis-aligned vector (purely horizontal or vertical).
    /// This can be inferred from other information, but some clients may find it useful.
    /// </summary>
    [JsonPropertyName("dir")]
    public Point2D dir { get; set; }
    
    /// <summary>
    /// "score" - an int representing the player's score (the number of powerups it has eaten).
    /// </summary>
    [JsonPropertyName("score")]
    public int score { get; set; }
    
    /// <summary>
    /// "died" - a bool indicating if the snake died on this frame.
    /// This will only be true on the exact frame in which the snake died.
    /// </summary>
    [JsonPropertyName("died")]
    public bool died { get; set; }
    
    /// <summary>
    /// "alive" - a bool indicating whether a snake is alive or dead.
    /// </summary>
    [JsonPropertyName("alive")]
    public bool alive { get; set; }
    
    /// <summary>
    /// "dc" - a bool indicating if the player controlling that snake disconnected on that frame.
    /// The server will send the snake with this flag set to true only once,
    /// then it will discontinue sending that snake for the rest of the game.
    /// </summary>
    [JsonPropertyName("dc")]
    public bool dc { get; set; }
    
    /// <summary>
    /// "join" - a bool indicating if the player joined on this frame.
    /// This will only be true for one frame.
    /// </summary>
    [JsonPropertyName("join")]
    public bool join { get; set; }

    /// <summary>
    /// Max score for the snake, ignored because server doesn't need it serialized
    /// </summary>
    [JsonIgnore]
    public int MaxScore { get; set; } = 0;
    
    /// <summary>
    /// default constructor for serialization/deserialization
    /// </summary>
    public Snake()
    {
        
    }

    /// <summary>
    /// method to get the head of the snake
    /// </summary>
    /// <returns>The Point2D representation of where the snake head is</returns>
    public Point2D GetHead()
    {
        return body[^1];
    }

    /// <summary>
    /// method to get the tail of the snake
    /// </summary>
    /// <returns>The Point2D representation of where the snake tail is</returns>
    public Point2D GetTail()
    {
        return body[0];
    }
    
}