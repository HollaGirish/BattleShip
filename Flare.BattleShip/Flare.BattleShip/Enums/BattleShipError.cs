namespace Flare.BattleShip
{
    /// <summary>
    /// This holds the error codes in the game.
    /// </summary>
    public enum BattleShipError
    {
        None,
        InvalidBattleShipCount,  // number of ships entered is more than the maximum number of ships that can be placed. 
        InvalidShipDetailsFormat,// Details entered are not entered according to the format. Please follow the example.
        InvalidRowOrColPositionFormat,// Details entered for coordinates are not according to the format.
        InvalidRowPositionOutOfGrid, // Row position entered is causing   out of grid placement for the ship.
        InvalidColumnPositionOutOfGrid, // Col position entered is causing   out of grid placement for the ship.
        InvalidShipLengthFormat, // Length of the ship entered is not  according to the format.
        InvalidShipLengthOutOfGrid,// Length of the ship entered  is causing   out of grid placement for the ship.
        InvalidAlignment // Alignment are not entered according to the format.
    }
}
