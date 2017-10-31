using System;
using System.Collections;
using System.Collections.Generic;
using log4net;

namespace LoggingKata
{
    /// <summary>
    /// Parses a POI file to locate all the TacoBells
    /// </summary>
    public class TacoParser
    {
        public TacoParser()
        {

        }

        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ITrackable Parse(string line)
        {
            var cells = line.Split(',');

            if (cells.Length < 3)
            {
            Logger.Error("Must have at least three elements to parse into ITrackable");
            return null;
            }

            double lon = 0;
            double lat = 0;

            try
            {
                Logger .Debug("Attempt Parsing Longitude");
                lon = double.Parse(cells[0]);

                Logger.Debug("Attempt Parsing Latitude");
                lat = double.Parse(cells[1]);
            }

            catch (Exception e)
            {
                Logger.Error("Failed to parse the location", e);
                Console.WriteLine(e);
                return null;
            }

            return new TacoBell
            {
                Name = cells[2],
                Location = new Point { Latitude = lat, Longitude = lon }
            };
        }
    }
}