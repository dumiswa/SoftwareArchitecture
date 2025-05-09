# 🛡️ Tower Defense Game – Software Architecture Assignment

This project is a **Tower Defense** game developed in Unity as part of a university assignment for the *Software Architecture* course. The goal was to demonstrate the application of software architecture principles such as **modular design**, **event-driven communication**, **scriptable objects**, and **separation of concerns** within a structured gameplay system.

---

## 🎮 Game Overview

In this game, the player strategically places towers on selectable tiles to defend against waves of enemies. Towers can be upgraded or destroyed, and the player earns money by eliminating enemies. The game ends in victory if all waves are cleared, or in defeat if too many enemies reach the end point.

---

## 🏗️ Architecture Summary

This project follows a **decoupled component-based design** where systems communicate via event broadcasting (`UIEventHandler`). Most game logic is separated into distinct manager classes with focused responsibilities. The game loop is handled primarily by `WaveManager` and `GameManager`, ensuring scalability and maintainability.

---

## 🧠 Key Features & Concepts

- **Modular Architecture** using Unity MonoBehaviours
- **Singleton Pattern** for managers (e.g., `GameManager`, `UIEventHandler`, `BuildingManager`)
- **Observer/Event-driven Communication** via delegates and `Action` events (`UIEventHandler`)
- **Tower Placement System** with visual range feedback
- **Tower Upgrade System** with live stat previews and cost checks
- **Enemy Navigation via Waypoints**
- **Wave Spawning System** with dynamic enemy scaling
- **Money System** with visual feedback through banknotes
- **UI Management** for shop, upgrade panel, wave info, and win/lose screens
- **Polymorphic Enemy and Weapon behavior support** via ScriptableObjects and interfaces

---

## 🗂️ Folder/Script Breakdown

| Script | Responsibility |
|--------|----------------|
| `GameManager.cs` | Central controller handling game states (win/lose), money, and speed control |
| `UIEventHandler.cs` | Handles UI events and dispatches game-wide actions via event delegates |
| `WaveManager.cs` | Manages timed enemy wave spawning and win progression |
| `BuildingManager.cs` | Handles tower instantiation, preview range, approval, and placement |
| `ShopManager.cs` | Toggles the visibility of the tower selection and approval panels |
| `TowerUpgradeManager.cs` | Opens and closes the upgrade panel for towers |
| `SelectionManager.cs` | Manages selection of tiles and towers for placement or upgrade |
| `Enemy.cs` | Controls movement, health, death handling, and money drop logic |
| `SelectableTile.cs` | Simple tile component to check if a tile is available or taken |
| `Waypoints.cs` | Singleton holding waypoint positions for enemy pathfinding |

---

##  ▶️ Getting Started

1. Clone the repository:
   ```bash
   https://github.com/dumiswa/SoftwareArchitecture

2. Open the project in Unity (2021.3 or newer recommended).

3. Run the MainScene.unity.

4. Left-click tiles to open the tower shop, approve tower builds, and defend against waves!

---

## ✅ Requirements

- Unity 2021.3 or later
- TextMeshPro (included by default)

---

## 🖼️ Screenshots

In-game screenshots showing the gameplay and UI:

![2](https://github.com/user-attachments/assets/2a6e392c-a6a3-4eda-98a4-95400b59408f)
![3](https://github.com/user-attachments/assets/ac9f016f-a699-43e8-8771-998c4e1f3aa0)

---

## 📚 Acknowledgements
This project was developed as part of the Software Architecture module at `Saxion University of Applied Sciences`. Special thanks to course instructors for guidance on clean and scalable Unity development practices.
   
