using System.Collections.Generic;

namespace Flare.BattleShip
{
    /// <summary>
    /// This is the Interface for the Battle Ship position
    /// </summary>
    public interface IBattleShipPositionEngine
    {
        /// <summary>
        /// Gets or sets the board size.
        /// </summary>
        public int BoardSize { get; set; }

        /// <summary>
        /// This method gets the user representated position for a given Board index position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        UserRepresentationPosition GetUserRepresentationPosition(ActualBoardIndexPosition position);

        /// <summary>
        /// This method gets the Board's index postion for a given User friendly positoin. Eg. User friendly position starts from 1.1 where as board's index would be 0.0.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        ActualBoardIndexPosition GetActualPosition(UserRepresentationPosition position);


        /// <summary>
        /// This method gets an example string for placing the ship.
        /// </summary>
        /// <returns></returns>
        string GetExampleString();

        /// <summary>
        /// This method converts the raw user entered position and converts to UserRepresentaionPosition object. 
        /// any error in the format is updated in the object.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>UserRepresentaionPosition object</returns>
        UserRepresentationPosition ValidatePosition(string position);

        /// <summary>
        /// This method gets the ship's girid position on board.
        /// </summary>
        /// <param name="inputShipInfo"></param>
        /// <returns>list of positions.</returns>
        List<ActualBoardIndexPosition> GetGridPositionsOnBoard(InputShipInfo inputShipInfo);

        /// <summary>
        /// This method parses & validates the user input ship positoin details and create a ship object.
        /// </summary>
        /// <param name="shipDetails"></param>
        /// <param name="boardSize"></param>
        /// <returns></returns>
        InputShipInfo ValidateNewShipDetails(string shipDetails, int boardSize);
    }
}
