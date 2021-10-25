using System;
using System.Collections.Generic;

namespace Flare.BattleShip
{
    /// <summary>
    /// This class holds the Ship details entered by the user.
    /// </summary>
    public class InputShipInfo
    {
        public InputShipInfo()
        {
            this.ShipId = Guid.NewGuid();
        }
        public string Name { get; set; }
        public Guid ShipId { get; set; }
        public int StartingRowPosition { get; set; }
        public int StartingColPosition { get; set; }
        public int Length { get; set; }
        public ShipAlignment PositionAlignment { get; set; }
        public List<ActualBoardIndexPosition> GridOccupiedActualPositions { get; set; }
        public bool HasEror
        {
            get
            {
                return this.ErrorCode == BattleShipError.None ? false : true;
            }
        }
        public BattleShipError ErrorCode { get; set; }
    }
}
