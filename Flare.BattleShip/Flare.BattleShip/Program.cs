using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Flare.BattleShip
{
    class Program
    {
        static void Main(string[] args)
        {
            //Adding appsettings JSON file.
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", true, true)
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IBattleShipShipManager battleShipManager = serviceProvider.GetService<IBattleShipShipManager>();

            //Create a board.
            battleShipManager.CreateBoard();

            //Get the input from user: Total battleship to place on the board.
            battleShipManager.GetBattleShipCount();

            //Get input from user & place ships.
            battleShipManager.PlaceShips();

            //Start the game.
            battleShipManager.BeginGame();
        }

        private static void ConfigureServices(ServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddLogging(configure => configure.AddSerilog(
                         new LoggerConfiguration()
                         .WriteTo.File("log.txt")
                        //.ReadFrom.Configuration(configuration)
                        .CreateLogger()))
                        .AddTransient<IBattleShipShipManager, BattleShipShipManager>()
                        .AddTransient<IBattleShipPositionEngine, BattleShipPositionEngine>()
                        .AddTransient<IUserInterface, UserInterface>();
        }
    }
}
