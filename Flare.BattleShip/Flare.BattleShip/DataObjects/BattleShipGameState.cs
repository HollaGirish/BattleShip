namespace Flare.BattleShip
{
    /// <summary>
    /// This class holds the state of the game once all the battle ships are placed.
    /// </summary>
    public class BattleShipGameState
    {
        public BattleShipGameState(int totalPositionsOccupied)
        {
            this.TotalPositionsOccupied = totalPositionsOccupied;
        }
        public int TotalPositionsOccupied { get; private set; }
        public int TotalHits { get; private set; }
        public int TotalMiss { get; private set; }
        public int TotalAttempts { get { return this.TotalHits + this.TotalMiss; } }

        public void IncrimentHit()
        {
            this.TotalHits++;
        }

        public void IncrimentMiss()
        {
            this.TotalMiss++;
        }
    }
}
