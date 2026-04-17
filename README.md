# Networked Snake Game (C# / Blazor / MySQL)

This project is a networked multiplayer Snake game built in C# as part of CS 3500 – Software Practice at the University of Utah. It demonstrates client-server architecture, real-time rendering, and database-backed game persistence.

The system consists of a custom web server, a networking layer for client communication, and a Blazor-based front-end that renders the game in real time using an HTML canvas.

The focus of the project is on networking, distributed system design, and real-time synchronization of game state across multiple clients.

---

## Features

- Real-time multiplayer Snake gameplay over a network
- Custom TCP-based networking layer
- Server-authoritative game state management
- Web server handling HTTP requests and dynamic HTML generation
- MySQL database integration for persistent game statistics
- Blazor WebAssembly/Server-based rendering using HTML canvas
- Smooth real-time animation and rendering loop
- Player tracking with scoring and session data

---

## Architecture

This project follows a client-server architecture with a clear separation between networking, game logic, and presentation layers.

### Server (WebServer)

- Handles incoming TCP connections
- Parses basic HTTP requests manually
- Serves dynamically generated HTML pages
- Queries a MySQL database for game and player statistics
- Routes requests to:
  - `/` → homepage
  - `/games` → all games view
  - `/games?gid=ID` → specific game stats

### Networking Layer (NetworkConnection)

- Wraps `TcpClient`, `StreamReader`, and `StreamWriter`
- Provides simplified `Send`, `ReadLine`, `Connect`, and `Disconnect` methods
- Handles message-based communication over TCP
- Used by both client and server communication components

### Client (Blazor Snake GUI)

- Renders game using HTML5 canvas via Blazor
- Maintains a local world model synchronized from the server
- Handles user input (WASD movement controls)
- Continuously renders game state using a timed game loop
- Draws snakes, walls, powerups, and animations in real time

---

## Key Design Concepts

- TCP-based custom networking protocol
- Server-authoritative game state
- Continuous synchronization of world state between server and client
- Manual HTTP request parsing and response generation
- Real-time rendering loop (~50 FPS target)
- Safe concurrency handling using state copies and locking
- Separation of concerns between networking, rendering, and game logic

---

## Database Integration

The server connects to a MySQL database to store and retrieve game data:

- Game sessions (ID, start time, end time)
- Player statistics per game
- Maximum scores and session durations

This allows dynamic generation of HTML pages that display historical game data.

---

## Controls

- W → Move Up
- A → Move Left
- S → Move Down
- D → Move Right

---

## Technologies Used

- C#
- .NET Networking (TcpClient, Streams)
- Blazor (Server/Interactive)
- HTML5 Canvas
- MySQL
- Custom HTTP handling
- Object-oriented design patterns

---

## Academic Context

This project was created for **CS 3500 – Software Practice** at the University of Utah.

It was developed as part of coursework focused on software design principles, networking, distributed systems, and real-time application development.

---

## Academic Integrity Notice

This repository is provided for **portfolio and educational purposes only**.

The code was originally submitted as coursework at the University of Utah. It is not intended for redistribution, reuse in academic submissions, or submission for graded coursework in any form. Doing so may violate university academic integrity policies.

This project is shared solely to demonstrate software engineering and systems development experience.

---
