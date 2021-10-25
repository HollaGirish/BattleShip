namespace Flare.BattleShip
{
    /// <summary>
    /// Interface for Battleship Manager
    /// </summary>
    public interface IBattleShipShipManager
    {
        void GetBattleShipCount();
        void CreateBoard();
        void PlaceShips();
        void BeginGame();
    }
}
