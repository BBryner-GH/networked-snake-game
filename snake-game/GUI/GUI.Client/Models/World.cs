// <copyright file="World.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// Methods for World implemented by:
// Ryan Storm
// Brenden Bryner
// November 18, 2025
// </copyright>
namespace GUI.Client.Models;

/// <summary>
/// represents the world in the game
/// </summary>
public class World
{
    // ---------- Instance Variables ----------
    
    /// <summary>
    /// size for the world representation
    /// </summary>
    public int Size {get; set;}

    /// <summary>
    /// dictionary to hold snakes with their ID
    /// </summary>
    public Dictionary<int, Snake> Snakes { get; private set; }= new();
    
    /// <summary>
    /// dictionary to hold walls with their ID
    /// </summary>
    public Dictionary<int, Wall> Walls { get; private set; }= new();
    
    /// <summary>
    /// dictionary to hold powerups with their ID
    /// </summary>
    public Dictionary<int, Powerup> Powerups { get; private set; }= new();
    
    /// <summary>
    /// dictionary to hold explosions
    /// </summary>
    public Dictionary<int, Explosion> Explosions { get; set; }= new();

    // ---------- Constructors ----------
    
    /// <summary>
    /// constructor for the world
    /// </summary>
    /// <param name="size">size of the world</param>
    public World(int size)
    {
        Size = size;
    }
    
    /// <summary>
    /// Creates a new world form an old world
    /// </summary>
    /// <param name="otherWorld"></param>
    public World(World otherWorld)
    {
        Snakes = new (otherWorld.Snakes);
        Walls = new(otherWorld.Walls);
        Powerups = new(otherWorld.Powerups);
        Size = otherWorld.Size;
        Explosions = new(otherWorld.Explosions);
    }

    // ---------- Walls ----------
    
    /// <summary>
    /// method to add a wall to the world
    /// </summary>
    /// <param name="wall">wall object</param>
    public void AddWall(Wall wall)
    {
        Walls.Add(wall.wallID, wall);
    }
    
    // ---------- Snakes ----------
    
    /// <summary>
    /// method to update the snake in the work
    /// </summary>
    /// <param name="snake">Snake object</param>
    /// <param name="updateDB">Whether the database should be updated</param>
    /// <param name="newPlayer">Whether or not the snake is a new player</param>
    public void UpdateSnake(Snake snake, out Boolean updateDB, out Boolean newPlayer)
    {
        // if snake left update DB
        if (snake.dc)
        {
            Snakes.Remove(snake.snakeID);
            updateDB = true;
            newPlayer = false;
            return;
        }

        // If new snake update DB
        if (!Snakes.ContainsKey(snake.snakeID))
        {
            updateDB = true;
            newPlayer = true;
        }
        
        // snake is not new, check max scores
        else
        {
            Snake oldSnake = Snakes[snake.snakeID];
            if (snake.score > oldSnake.MaxScore)
            {
                snake.MaxScore = snake.score;
                updateDB = true;
            }
            else
            {
                snake.MaxScore = oldSnake.MaxScore;
                updateDB = false;
            }
            newPlayer = false;
        }
        
        // assign new snake value
        Snakes[snake.snakeID] = snake;
        
        // set off explosion animation
        if (snake.died)
        {
            lock (Explosions)
            {
                Explosions.TryAdd(snake.snakeID, new Explosion());
            }
        }
    }
    
    // ---------- Powerups ----------
    
    /// <summary>
    /// method to update powerups in the world
    /// </summary>
    /// <param name="powerup">powerup object</param>
    public void UpdatePowerup(Powerup powerup)
    {
        if (powerup.died)
        {
            Powerups.Remove(powerup.powerID);
        }
        else
        {
            Powerups[powerup.powerID] = powerup;
        }
    }
}