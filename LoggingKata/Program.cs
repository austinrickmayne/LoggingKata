using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using Geolocation;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace LoggingKata
{
    class Program
    {
        //Why do you think we use ILog?
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var path = Environment.CurrentDirectory + "\\Taco_Bell-US-AL-Alabama.csv";

            Logger.Info("Log initialized");
            Logger.Info("Grabbing from path: " + path);

            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
            {
                Logger.Error("No locations to check. Must have at least one location.");
            }
            else if (lines.Length == 1)
            {
                Logger.Warn("Only one location provided. Must have two to perform a check");
            }
            var parser = new TacoParser();
            Logger.Info(("Initialized our Parser"));

            var locations = lines.Select(line => parser.Parse(line))
                .OrderBy(loc => loc.Location.Longitude)
                .ThenBy(loc => loc.Location.Latitude)
                .ToArray();

            ITrackable a = null;
            ITrackable b = null;
            double distance = 0;

            foreach (var locA in locations)
            {
                var origin = new Coordinate
                {
                    Latitude = locA.Location.Latitude,
                    Longitude = locA.Location.Longitude
                };

                foreach (var locB in locations)
                {
                    var dest = new Coordinate
                    {
                        Latitude = locB.Location.Latitude,
                        Longitude = locB.Location.Longitude
                    };

                    var nDist = GeoCalculator.GetDistance(origin, dest);

                    if (nDist > distance)
                    {
                        distance = nDist;
                        a = locA;
                        b = locB;
                    }
                }
            }

            Console.WriteLine($"The first Taco Bell location is {a.Name} and the furthest distance away {b.Name}, with a distance of {distance}");
            Console.ReadLine();
        }
    }
}