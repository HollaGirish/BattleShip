namespace Flare.BattleShip
{
    /// <summary>
    /// This is a single Grid in the board.
    /// </summary>
    public class BoardGrid
    {
        public string ShipName { get; set; }
        public bool IsShipOccupied { get; set; }
        public bool IsHit { get; set; }
    }
}
