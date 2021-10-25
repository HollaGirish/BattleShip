using Flare.BattleShip;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flare.BattleShip
{
    public class UserInterface : IUserInterface
    {
        public void Display(string information, MessageType messageType = MessageType.None)
        {
            switch (messageType)
            {
                case MessageType.None:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case MessageType.Info:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case MessageType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case MessageType.Failure:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MessageType.Header:
                    Console.ForegroundColor =  ConsoleColor.Yellow;
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine(information);
                    Console.WriteLine("-------------------------------------------------");
                    return;
                default:
                    break;
            }
            Console.WriteLine(information);

        }

        public string ReadInput()
        {
            return Console.ReadLine();
        }
    }
}
