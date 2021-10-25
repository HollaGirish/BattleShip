namespace Flare.BattleShip
{
    /// <summary>
    /// This class holds actual board index position. (Row and Column).
    /// </summary>
    public class ActualBoardIndexPosition
    {
        public int RIndex { get; set; }
        public int CIndex { get; set; }
        public bool HasError
        {
            get
            {
                return this.ErrorCode == BattleShipError.None ? false : true;
            }
        }
        public BattleShipError ErrorCode { get; set; }
    }
}
