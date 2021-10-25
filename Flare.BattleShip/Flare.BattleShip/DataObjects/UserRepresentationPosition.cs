namespace Flare.BattleShip
{
    /// <summary>
    /// This class holds user representation position. The parameters are of string, because to support different board/user specification say from 1.1 to A.1.
    /// </summary>
    public class UserRepresentationPosition
    {
        public string RIndex { get; set; }
        public string CIndex { get; set; }
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
