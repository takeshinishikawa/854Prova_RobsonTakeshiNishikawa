# 854Prova_RobsonTakeshiNishikawa
 Completion work of the Programming Logic module of the Web Fullstack track by Let's Code.
 Exercise proposed by the teacher (https://github.com/heberhenrique)
 The project has a bonus challenge resolution that includes a game mode against the computer.

## Naval battle
 Create a naval battle game where 2 players can participate.

## About the game
 Naval Battle is a two-player board game in which players have to guess which squares their opponent's ships are on.
 Each player has their own 10x10 board where rows are represented by letters (A-J) and columns are represented by numbers (1-10).
 Players must place their vessels within the corresponding quadrants. Vessels must be positioned vertically or horizontally, always forming a straight line and never diagonally.
 Each player can fire once in each turn and in order to fire he must inform the position of the quadrant by letter and number. Example: E7. If the shot hits a vessel, that location is signaled. When a ship receives all the hits it sinks.
 The game ends when one of the two players sinks all of their opponent's ships. Each player owns the following vessels:
- 1 Aircraft Carrier (5 quadrants)
- 2 Tanker (4 quadrants)
- 3 Destroyers (3 quadrants)
- 4 Submarines (2 quadrants)
## Program definitions and rules
 Playing against another opponent
1. Program asks if the game will be between two real players or if it will be played alone, that is, against the computer.
2. If it is against another opponent, the program asks for the name of the first player and stores it.
3. After storing the name of the first player, the program asks for the name of the second player and stores it.
4. After storing both names, the program asks the player where he will place each of the ships.
5. Ships are identified by acronyms:
- PS - Aircraft Carrier (5 quadrants)
- NT - Tanker (4 quadrants)
- DS - Destroyers (3 quadrants)
- SB - Submarines (2 quadrants)
6. The program asks for the type of vessel and then the start and end positions. Example:

 ```shConsole.WriteLine("What type of vessel?")
 var vessel = Console.ReadLine();
 // input: SB
 Console.WriteLine("What is your position?")
 // input: H1H2
 Console.WriteLine("What type of vessel?")
 var vessel = Console.ReadLine();
 // input: SB
 Console.WriteLine("What is your position?")
 // input: F6G6
The program repeats the instructions until the first player finishes placing the ships.
```
8. When both players finish placing the ships the game starts.
9. When the player takes a shot, the program receives the indicated position and checks if it hit the ship.

## General rules
- The program must only receive valid entries, that is, the acronym must be one of the corresponding ones.
- The vessel cannot be of a larger size than specified for each of them.
- One vessel cannot overlap the other.
- A map of the opponent's field must be shown each time the player takes a shot.
- The map should represent the spaces as follows:
Blank: Spaces that have not been shot
Letter 'A': Space that was shot but had no vessel
Letter 'X': Space that was shot and had a vessel.
- At each turn, the program must clear the screen and present the opponent's board
- When a trigger is correct, the program should display a message and also when the trigger is wrong.

## Bonus challenge
- Develop an algorithm that allows you to play against the computer. (Just answer "Nao" when asked if it's a multiplayer).
