using System;
using System.Collections.Generic;

namespace Flare.BattleShip
{
    /// <summary>
    /// This class impliments IBattleShipPositionConvertor. This is needed mainly to convert position of the board to matrix position.
    /// E.g. A,0 is represented as 0,0 Position. OR 1,1 is represented as 0,0.
    /// </summary>
    public class BattleShipPositionEngine : IBattleShipPositionEngine
    {
        //private readonly int _boardSize;

        ///// <summary>
        ///// Constructor with the board size.z
        ///// </summary>
        ///// <param name="boardSize"></param>
        //public BattleShipPositionEngine(int boardSize)
        //{
        //    _boardSize = boardSize;
        //}

        /// <summary>
        /// Constructor with the board size.z
        /// </summary>
        public BattleShipPositionEngine()
        {
             //Default constructor.
        }

        /// <summary>
        /// Gets or sets the board size.
        /// </summary>
        public int BoardSize { get; set; }

        /// <summary>
        /// This method gets the Board's index postion for a given User friendly positoin. Eg. User friendly position starts from 1.1 where as board's index would be 0.0.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public ActualBoardIndexPosition GetActualPosition(UserRepresentationPosition position)
        {
            int rIndex = 0;
            int cIndex = 0;
            bool isNumeric = int.TryParse(position.RIndex, out rIndex);
            if (!isNumeric)
                throw new Exception();

            isNumeric = int.TryParse(position.CIndex, out cIndex);
            if (!isNumeric)
                throw new Exception();

            return new ActualBoardIndexPosition() {  RIndex = rIndex - 1, CIndex = cIndex - 1 };
        }

        /// <summary>
        /// This method gets the user representated position for a given Board index position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public UserRepresentationPosition GetUserRepresentationPosition(ActualBoardIndexPosition position)
        {
            return new UserRepresentationPosition() { RIndex = (position.RIndex + 1).ToString() , CIndex = (position.CIndex + 1).ToString()};
        }


        /// <summary>
        /// This method gets an example string for placing the ship.
        /// </summary>
        /// <returns></returns>
        public string GetExampleString()
        {
            //POSITION (row,col)- length of the ship - alingment (H/V). "1.5,5,V"
            return "Please place the 'Ship {0}' in the following format 'row.col,length,alingment(H/V)'. Example '1.5,6,V' create a ship in 1st row and 5th column of size 6 and place it vertically on the board.";
        }
        
        /// <summary>
        /// This method gets the ship's girid position on board.
        /// </summary>
        /// <param name="inputShipInfo"></param>
        /// <returns>list of positions.</returns>
        public List<ActualBoardIndexPosition> GetGridPositionsOnBoard(InputShipInfo inputShipInfo)
        {
            ActualBoardIndexPosition actualPosition = this.GetActualPosition(new UserRepresentationPosition() { CIndex = inputShipInfo.StartingColPosition.ToString(), RIndex = inputShipInfo.StartingRowPosition.ToString() });
            List<ActualBoardIndexPosition> shipOccupiedPosition = new List<ActualBoardIndexPosition>();

            for (int i = 0; i < inputShipInfo.Length; i++)
            {
                ActualBoardIndexPosition pos = new ActualBoardIndexPosition();
                if (inputShipInfo.PositionAlignment == ShipAlignment.Vertical)
                {
                    pos.RIndex = actualPosition.RIndex + i;
                    pos.CIndex = actualPosition.CIndex;
                }
                else if (inputShipInfo.PositionAlignment == ShipAlignment.Horizontal)
                {
                    pos.RIndex = actualPosition.RIndex;
                    pos.CIndex = actualPosition.CIndex + i;
                }
                shipOccupiedPosition.Add(pos);
            }

            return shipOccupiedPosition;
        }


        /// <summary>
        /// This method converts the raw user entered position and converts to UserRepresentaionPosition object. 
        /// any error in the format is updated in the object.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>UserRepresentaionPosition object</returns>
        public UserRepresentationPosition ValidatePosition(string position)
        {
            UserRepresentationPosition userRepPos = new UserRepresentationPosition();

            string[] posIndex = position.Split('.', StringSplitOptions.RemoveEmptyEntries);
            int rIndex;
            int cIndex;
            if (posIndex.Length != 2)
            {
                userRepPos.ErrorCode = BattleShipError.InvalidRowOrColPositionFormat;
                return userRepPos;
            }

            //Check for Numeric
            bool isNumeric = int.TryParse(posIndex[0], out rIndex);
            if (!isNumeric)
            {
                userRepPos.ErrorCode = BattleShipError.InvalidRowOrColPositionFormat;
                return userRepPos;
            }

            isNumeric = int.TryParse(posIndex[1], out cIndex);
            if (!isNumeric)
            {
                userRepPos.ErrorCode = BattleShipError.InvalidRowOrColPositionFormat;
                return userRepPos;
            }

            //Check for out of Board
            if (rIndex > this.BoardSize || rIndex < 1)
            {
                userRepPos.ErrorCode = BattleShipError.InvalidRowPositionOutOfGrid;
                return userRepPos;
            }

            //Check for out of Board
            if (cIndex > this.BoardSize || cIndex < 1)
            {
                userRepPos.ErrorCode = BattleShipError.InvalidColumnPositionOutOfGrid;
                return userRepPos;
            }

            userRepPos.RIndex = rIndex.ToString();
            userRepPos.CIndex = cIndex.ToString();

            return userRepPos;
        }


        /// <summary>
        /// This method parses & validates the user input ship positoin details and create a ship object.
        /// </summary>
        /// <param name="shipDetails"></param>
        /// <param name="boardSize"></param>
        /// <returns></returns>
        public InputShipInfo ValidateNewShipDetails(string shipDetails, int boardSize)
        {
            InputShipInfo inputShipInfo = new InputShipInfo();
            string[] splitDetails = shipDetails.Split(',', StringSplitOptions.RemoveEmptyEntries);

            //Check if the length after splitting is 3. If not its error and return - value.
            if (splitDetails.Length != 3)
            {
                inputShipInfo.ErrorCode = BattleShipError.InvalidShipDetailsFormat;
                return inputShipInfo;
            }

            //Validate the position, which is in (row.col) format.
            UserRepresentationPosition userRepPos = ValidatePosition(splitDetails[0]);
            if(userRepPos.HasError)
            {
                inputShipInfo.ErrorCode = userRepPos.ErrorCode;
                return inputShipInfo;
            }
            else
            {
                inputShipInfo.StartingRowPosition = Convert.ToInt32(userRepPos.RIndex);
                inputShipInfo.StartingColPosition = Convert.ToInt32(userRepPos.CIndex);
            }
            //ValidatePositionDetails(boardSize, splitDetails, inputShipInfo);

            //Validate Ship Length.
            int shipLength;
            bool isNumeric = int.TryParse(splitDetails[1], out shipLength);
            if (!isNumeric)
            {
                inputShipInfo.ErrorCode = BattleShipError.InvalidShipLengthFormat;
                return inputShipInfo;
            }

            //Validate second parameter 'Length' of the ship.
            if (shipLength > boardSize || shipLength < 0)
            {
                inputShipInfo.ErrorCode = BattleShipError.InvalidShipLengthOutOfGrid;
                return inputShipInfo;
            }

            inputShipInfo.Length = shipLength;


            //Validate ship's alingment. Has to be eith 'V', 'v' or 'H' 'h'.
            List<string> verticalAlignmentValues = new List<string>() { "V", "v" };
            List<string> horizontalAlignmentValues = new List<string>() { "H", "h" };


            if (!verticalAlignmentValues.Contains(splitDetails[2].Trim()) && !horizontalAlignmentValues.Contains(splitDetails[2].Trim()))
            {
                inputShipInfo.ErrorCode = BattleShipError.InvalidAlignment;
                return inputShipInfo;
            }
            if (verticalAlignmentValues.Contains(splitDetails[2].Trim()))
            {
                inputShipInfo.PositionAlignment = ShipAlignment.Vertical;
            }
            else if (horizontalAlignmentValues.Contains(splitDetails[2].Trim()))
            {
                inputShipInfo.PositionAlignment = ShipAlignment.Horizontal;
            }
            else
            {
                inputShipInfo.ErrorCode = BattleShipError.InvalidAlignment;
                return inputShipInfo;
            }

            //Check if adding ship lenght to the current position is going out of board.
            if (inputShipInfo.PositionAlignment == ShipAlignment.Vertical && (inputShipInfo.StartingRowPosition + (inputShipInfo.Length - 1)) > this.BoardSize)
            {
                inputShipInfo.ErrorCode = BattleShipError.InvalidColumnPositionOutOfGrid;
                return inputShipInfo;
            }
            else if (inputShipInfo.PositionAlignment == ShipAlignment.Horizontal && (inputShipInfo.StartingColPosition + (inputShipInfo.Length - 1)) > this.BoardSize)
            {
                inputShipInfo.ErrorCode = BattleShipError.InvalidRowPositionOutOfGrid;
                return inputShipInfo;
            }

            return inputShipInfo;
        }

        ///// <summary>
        ///// This method validates the ships position details.
        ///// </summary>
        ///// <param name="boardSize"></param>
        ///// <param name="splitDetails"></param>
        ///// <param name="inputShipInfo"></param>
        //void ValidatePositionDetails(int boardSize, string[] splitDetails, InputShipInfo inputShipInfo)
        //{
        //    // Validate the position details of the ship. Valid format is row.col
        //    string[] shipPosDetails = splitDetails[0].Split('.', StringSplitOptions.RemoveEmptyEntries);

        //    int rIndex;
        //    int cIndex;

        //    //Check for row and col position is numeric. If not return error value -2.
        //    bool isNumeric = int.TryParse(shipPosDetails[0], out rIndex);
        //    if (!isNumeric)
        //    {
        //        inputShipInfo.ErrorCode = BattleShipError.InvalidRowOrColPositionFormat;
        //        return;
        //    }

        //    isNumeric = int.TryParse(shipPosDetails[1], out cIndex);
        //    if (!isNumeric)
        //    {
        //        inputShipInfo.ErrorCode = BattleShipError.InvalidRowOrColPositionFormat;
        //        return;
        //    }

        //    //Check for the entered row and column is valid position.
        //    if (rIndex <= 0 || rIndex > _boardSize)
        //    {
        //        inputShipInfo.ErrorCode = BattleShipError.InvalidRowPositionOutOfGrid;
        //        return;
        //    }

        //    if (rIndex <= 0 || rIndex > _boardSize)
        //    {
        //        inputShipInfo.ErrorCode = BattleShipError.InvalidColumnPositionOutOfGrid;
        //        return;
        //    }

        //    inputShipInfo.StartingRowPosition = rIndex;
        //    inputShipInfo.StartingColPosition = cIndex;

        //    return;
        //}
    }
}
