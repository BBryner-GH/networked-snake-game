// <copyright file="Point2D.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for NetworkController implemented by:
// Brenden Bryner
// Ryan Storm
// November 18, 2025
// </copyright>
namespace GUI.Client.Models;

/// <summary>
/// a simple class that represents a 2D point in space (an X, Y pair).
/// <list type="bullet">
/// All locations represented by the server using a Point2D are world-space locations.
/// </list>
/// </summary>
public class Point2D
{
    
    // instance variables
    
    /// <summary>
    /// int value of X
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    /// int value of Y
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// empty constructor for serialization
    /// </summary>
    public Point2D()
    {
        
    }
}