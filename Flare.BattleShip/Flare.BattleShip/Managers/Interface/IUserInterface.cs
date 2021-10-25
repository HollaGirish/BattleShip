using System;
using System.Collections.Generic;
using System.Text;

namespace Flare.BattleShip
{
    public interface IUserInterface
    {
        string ReadInput();
        void Display(string information, MessageType messageType = MessageType.None);
    }

    public enum MessageType
    {
        None,
        Info,
        Success,
        Warning,
        Failure,
        Header
    }
}
