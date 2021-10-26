# BattleShip
Battle Ship challenge
This is a console game of "Battle Ship" written in C# .NET Core 3.1.

Instruction for playing:-
Note: Board size is 10X10
1. Player1 need to specify the 'Number of battle ship' that needs to be placed on the board. This cannot be greated than 10.
2. Next, Player1 will be prompted to place his ships on the board. The format will be "<Row Position>.<Column Position>,<Size or length of the ship>,<Alingment Horizontal/Vertical>
Example for placing the ship of grid size 3, Horizontally in the postion 1st row 5th column (1.5) The expected input format will be "1.5,3,H".
3. Once all the ships placed in the above format, then the game mode state will begin.
4. Next Player1 will be prompted for the attack position from Player2. Format "<Row Position>.<Column Position>" eg 1.7 (1st Row and 7th Column).
5. The Game will display eithet a "Hit" or a "Miss" or "Duplicate posion called out".
6. The game continues till all the ships are "Hit".
7. Once all the ships are "Hit", the game will end providing the stats.
