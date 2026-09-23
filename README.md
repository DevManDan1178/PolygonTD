# Polygon TD

Polygon TD is a simple tower defense game built in Unity where towers and enemies are represented by basic shapes.

Place turrets on tiles, defend the endpoint, and customize your tower upgrades to optimize your defenses.

## Gameplay

- Place turrets on available tiles.
- Defend the endpoint from incoming enemies.
- Customize tower upgrades to suit your strategy.
- Exclusive level 3 and 4 features:
  - Extend the path to change the level layout.
    - Path extensions only apply when they are connected to the endpoint through adjacent tiles.
  - Place tiles to allow turret placement
    - Tile placement is only allowed on empty tiles

## Technical Highlights

- **Tower System** — Handles turret placement, targeting, attacks, and upgrades.
- **Enemy System** — Manages enemy movement, health, damage, and interaction with the path.
- **Tile-Based Map** — Uses a grid-based system for tower placement and level modification.
- **Dynamic Path Extension** — Validates path extensions based on whether they remain connected to the endpoint.
- **Upgrade System** — Allows towers to be customized through different stat upgrade choices, with adaptive pricing.
- **Wave System** — Controls enemy spawning and progression through gameplay waves.
- **Game State & UI** — Manages gameplay state, tower information, upgrades, and player interaction.
- **Unity/C# Development** — Built using Unity's component-based architecture and C# scripting.

## Skills Demonstrated

- C#
- Unity
- Gameplay programming
- Object-oriented programming
- Component-based architecture
- Grid-based systems
- Path validation
- Game-state management
- UI implementation
- Game systems integration
- Game balancing and iteration
- WebGL deployment

## What I Learned

Polygon TD was an opportunity to work with multiple gameplay systems that needed to interact with one another.

The project provided experience with:

- Designing gameplay systems from scratch.
- Building interconnected systems in Unity.
- Managing interactions between towers, enemies, tiles, upgrades, and game state.
- Implementing grid-based gameplay.
- Handling dynamic path validation.
- Designing upgrade systems that support different player strategies.
- Iterating on gameplay mechanics and balancing.

## Project Status

This is a preserved version of a game developed in Unity earlier in my development experience.

The project is no longer actively developed and is unlikely to receive further updates. It is maintained as a portfolio project demonstrating the implementation of a complete playable game and its underlying gameplay systems.

## Play the Game

A playable WebGL version is available on my portfolio:

[Play Polygon TD](https://devmandan.vercel.app/polygontd)

## Built With

- Unity
- C#

## Repository Structure

The repository contains the original Unity project, including:

- `Assets` — Game assets, scenes, scripts, and project resources.
- `Packages` — Unity package configuration.
- `ProjectSettings` — Unity project configuration.
- `Data/Plugins` — Project plugins and supporting data.
- `.vscode` — Editor configuration.
