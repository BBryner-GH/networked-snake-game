// <copyright file="Snake.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for NetworkController implemented by:
// Brenden Bryner
// Ryan Storm
// November 18, 2025
// </copyright>
namespace GUI.Client.Models;

/// <summary>
/// represents an explosion
/// </summary>
public class Explosion
{
    /// <summary>
    /// represents the frame the animation is on
    /// </summary>
    public int explosionFrame  { get; set; } = 0;
    
    /// <summary>
    /// represents the time in between frames
    /// </summary>
    public int explosionTick   { get; set; } = 0;
}