using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Flare.BattleShip
{
    /// <summary>
    /// This is the main Battle ship manager class, which performs the important decisions and business logic.
    /// </summary>
    public class BattleShipShipManager : IBattleShipShipManager
    {
        private BoardGrid[,] _board;
        private List<InputShipInfo> _ships;
        private IBattleShipPositionEngine _battleShipPositionEngine;
        private IUserInterface _userInterface;
        private readonly ILogger _logger;
        private const int BoardSize = 10;

        /// <summary>
        /// Constructor with the size of the board as argument.
        /// </summary>
        /// <param name="size">Size of the board.</param>
        public BattleShipShipManager(ILogger<BattleShipShipManager> logger, IBattleShipPositionEngine battleShipPositionEngine, IUserInterface userInterface)
        {
            _logger = logger;
            _ships = new List<InputShipInfo>();
            //_battleShipPositionConvertor = new BattleShipPositionEngine(BoardSize);
            _battleShipPositionEngine = battleShipPositionEngine;
            _battleShipPositionEngine.BoardSize = BoardSize;
            //_userInterface = new UserInterface();
            _userInterface = userInterface;
            _logger.LogInformation("Inside constructor.");
        }

        public int SelectedBattleShipCount { get; set; }
        public int TotalPositionPlaced { get; private set; }

        /// <summary>
        /// This method gets total number of battle ship that needs to be placed on the board as the user input .
        /// </summary>
        public void GetBattleShipCount()
        {
            try
            {
                _logger.LogInformation("Begin: Getting battle ship count as user input");
                int battleShipCount = 0;
                _userInterface.Display("Please Select Number of battle ships that you want to place",MessageType.Info);
                while (true)
                {
                    //TODO: Move to IO class.
                    string inputBattleShipCount = _userInterface.ReadInput();

                    bool isNumeric = int.TryParse(inputBattleShipCount, out battleShipCount);
                    if (isNumeric)
                    {
                        this.SelectedBattleShipCount = battleShipCount;
                        break;
                    }
                     _userInterface.Display("Error: Invalid selection. Please Select Number of battle ships that you want to place", MessageType.Failure);
                }
                _logger.LogInformation("Complete: Getting battle ship count as user input.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// This method creates a 'Battle Ship' Board.
        /// </summary>
        public void CreateBoard()
        {
            try
            {
                _logger.LogInformation("Begin: Create Battleship board.");
                _userInterface.Display("Welcome to the 'Battle Ship Game'", MessageType.Header);
                //Initialise board with 10 X 10 size.
                _board = new BoardGrid[BoardSize, BoardSize];
                _logger.LogInformation("Completed: Create Battleship board.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// This method takes the details of the ships to be placed on the board.
        /// Input will be in the format "Row.Column,ShipSize,ShipDirection".
        /// </summary>
        public void PlaceShips()
        {
            try
            {
                _logger.LogInformation("Begin: Create Battleship board.");
                string newShipName = string.Format("Ship #{0}", _ships.Count + 1);

                //Loop until the desired number of battle ships are not created.
                while (this._ships.Count < this.SelectedBattleShipCount)
                {
                    //POSITION (row,col)- length of the ship - alingment (H/V). "1.5,5,V"
                     _userInterface.Display(string.Format(_battleShipPositionEngine.GetExampleString(), _ships.Count + 1), MessageType.Info);

                    //Capture the ship's position , size and the direction.
                    string startingPosition = _userInterface.ReadInput();
                    _logger.LogInformation("User entered ship position: " + startingPosition);


                    //Validate the user selected ship details
                    InputShipInfo shipInfo = _battleShipPositionEngine.ValidateNewShipDetails(startingPosition, BoardSize);

                    if (shipInfo.HasEror)
                    {
                        //Get the error message from the utility class.
                        string errorMessage = BattleShipUtils.GetErrorMessage(shipInfo.ErrorCode);
                         _userInterface.Display("This is an invalid selection: " + errorMessage, MessageType.Failure);
                        _logger.LogWarning("PlaceShips: This is an invalid selection: " + errorMessage);
                    }
                    else
                    {
                        _logger.LogInformation("PlaceShips: Position validation completed");
                        shipInfo.Name = newShipName;
                        bool allValidationPassed = false;
                        int positionPlaced = 0;

                        //Check if there is any overlapping or cross over positions by other ships.

                        //Get the new Ship's Grid position spread.
                        List<ActualBoardIndexPosition> actualPositionSpread = _battleShipPositionEngine.GetGridPositionsOnBoard(shipInfo);
                        _logger.LogInformation("PlaceShips: Battle ship position list generation complete.");

                        //Copy the main board object temporarly.
                        BoardGrid[,] boardCopy = new BoardGrid[BoardSize, BoardSize];
                        Array.Copy(_board, boardCopy, _board.Length);

                        //Iterate through new 'Battle Ship' position spread and check for crossover or overlapping positions from other ships.
                        foreach (ActualBoardIndexPosition position in actualPositionSpread)
                        {
                            //Get the grid for the current position and check whether this position is occupied by other ships.
                            BoardGrid gridBlock = boardCopy[position.RIndex, position.CIndex];
                            if (gridBlock == null)
                                gridBlock = new BoardGrid();
                            if (!gridBlock.IsShipOccupied)
                            {
                                //No: This position is not occupied by other ship.
                                gridBlock.IsShipOccupied = true;
                                gridBlock.ShipName = shipInfo.Name;
                                boardCopy[position.RIndex, position.CIndex] = gridBlock;
                                positionPlaced++;
                                allValidationPassed = true;
                            }
                            else
                            {  //Yes this position is already occupied by other ship.

                                //Get the user representation position of the occupied ship.
                                UserRepresentationPosition userRepPos = _battleShipPositionEngine.GetUserRepresentationPosition(position);

                                //Get the display position.
                                string displayPos = string.Format("{0}.{1}", userRepPos.RIndex, userRepPos.CIndex);

                                //Write to console stating that, there is already other ship occupied. Need to select different position.
                                 _userInterface.Display(string.Format("This selection is causing overlap with {0} placed in position {1}", gridBlock.ShipName, displayPos), MessageType.Warning);
                                _logger.LogInformation(string.Format("This selection is causing overlap with {0} placed in position {1}", gridBlock.ShipName, displayPos));

                                allValidationPassed = false;
                                break;
                            }
                        }
                        //Check if all validation passed. If yes, update the master board and add the Battle ship info to the Ship list.
                        if (allValidationPassed)
                        {
                            _board = boardCopy;
                            _ships.Add(shipInfo);
                            this.TotalPositionPlaced += positionPlaced;
                            _logger.LogInformation("PlaceShips: All validation passed and ship placed on the board.");
                        }
                    }
                }
                //If all the validation pass create a ship with occupied co-ordinates and add to the ship list.
                 _userInterface.Display("Ship created and placed successfully", MessageType.Success);
                _logger.LogInformation("Ship created and placed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// This takes the player to Game Mode state. Where oppenent can attack positions.
        /// </summary>
        public void BeginGame()
        {
            try
            {
                _logger.LogInformation("BeginGame: Game starts here.");

                //Create a battleship game state.
                BattleShipGameState gameState = new BattleShipGameState(this.TotalPositionPlaced);

                //The game state should continue till all the ships are hit.
                while (gameState.TotalPositionsOccupied != gameState.TotalHits)
                {
                     _userInterface.Display("Please enter Opponent's attack position in format : 'row.column'", MessageType.Info);

                    //Read the attack position.
                    string attachPositionInput = _userInterface.ReadInput();
                    _logger.LogInformation("BeginGame: User entered attack position is " + attachPositionInput);

                    //Validate and get the UserRepresentationPosition position object for the attack position.
                    UserRepresentationPosition attackPosition = _battleShipPositionEngine.ValidatePosition(attachPositionInput);
                    if (attackPosition.HasError)
                    {
                        string errorMessage = BattleShipUtils.GetErrorMessage(attackPosition.ErrorCode);
                        _userInterface.Display(errorMessage, MessageType.Failure);
                        continue;
                    }

                    //Gets the actual/matrix position representation.
                    ActualBoardIndexPosition actualAttackPos = _battleShipPositionEngine.GetActualPosition(attackPosition);

                    //Get the grid posiont from the board and check whether the call is hit or a miss.
                    BoardGrid gridBlock = _board[actualAttackPos.RIndex, actualAttackPos.CIndex];

                    if (gridBlock == null || !gridBlock.IsShipOccupied)
                    {
                         _userInterface.Display("Miss: There is no Ship in this position.", MessageType.Warning); //Red color
                        _logger.LogInformation("BeginGame: Miss: There is no Ship in this position " + attachPositionInput);

                        gameState.IncrimentMiss();
                        continue;
                    }
                    //Check whether the position is already a Hit position. 
                    if (gridBlock.IsHit)
                    {
                         _userInterface.Display("Duplicate attack position. As this was already called out.", MessageType.Warning);
                        _logger.LogInformation("BeginGame: Duplicate attack position. As this was already called out" + attachPositionInput);

                        continue;
                    }

                    if (gridBlock.IsShipOccupied)
                    {
                         _userInterface.Display("Hit: There is Ship in this position.", MessageType.Success); //Red color
                        _logger.LogInformation("BeginGame: Hit: There is Ship in this position" + attachPositionInput);

                        gridBlock.IsHit = true;
                        gameState.IncrimentHit();
                        continue;
                    }

                    //if (!gridBlock.IsShipOccupied)
                    //{
                    //     _userInterface.Display("Miss: There is no Ship in this position."); //Red color
                    //    gameState.IncrimentMiss();
                    //    continue;
                    //}
                    //else
                    //{
                    //     _userInterface.Display("Hit: There is Ship in this position."); //Red color
                    //    gridBlock.IsHit = true;
                    //    gameState.IncrimentHit();
                    //    continue;
                    //}
                }
                //Check if the total hits are reached then display Gameover.
                if (gameState.TotalPositionsOccupied == gameState.TotalHits)
                {
                    _logger.LogInformation("Game Over");
                     _userInterface.Display(string.Format("!!! Game Over !!! with {0} Total Attempts and {1} Miss and {2} Hits ", gameState.TotalAttempts, gameState.TotalMiss, gameState.TotalHits), MessageType.Success);
                }
                else
                {
                     _userInterface.Display("Game was interrupted.", MessageType.Info);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
