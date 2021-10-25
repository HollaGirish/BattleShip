namespace Flare.BattleShip
{
    /// <summary>
    /// This is a utility class for battle ship game.
    /// </summary>
    public static class BattleShipUtils
    {
        /// <summary>
        /// This method gets the message from the error code.
        /// </summary>
        /// <param name="errorCode">error code.</param>
        /// <returns></returns>
        public static string GetErrorMessage(BattleShipError errorCode)
        {
            switch (errorCode)
            {

                case BattleShipError.InvalidBattleShipCount:
                    return "Number of ships entered is more than the maximum number of ships that can be placed.";
                case BattleShipError.InvalidShipDetailsFormat:
                    return "Details entered are not entered according to the format. Please follow the example.";
                case BattleShipError.InvalidRowOrColPositionFormat:
                    return "Details entered for coordinates are not according to the format.";
                case BattleShipError.InvalidRowPositionOutOfGrid:
                    return "Row position entered is causing out of grid placement for the ship.";
                case BattleShipError.InvalidColumnPositionOutOfGrid:
                    return "Column position entered is causing out of grid placement for the ship.";
                case BattleShipError.InvalidShipLengthFormat:
                    return "Length of the ship entered is not  according to the format.";
                case BattleShipError.InvalidShipLengthOutOfGrid:
                    return "Length of the ship entered  is causing   out of grid placement for the ship.";
                case BattleShipError.InvalidAlignment:
                    return "Alignment are not entered according to the format";
                default:
                    return "Something went wrong. Please restart the application and try again.";
            }
        }
    }
}
