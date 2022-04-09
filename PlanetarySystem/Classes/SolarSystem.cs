using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetarySystem
{
    public class SolarSystem
    {
        public string SystemName { get; set; }
        public List<CelestialObject> SystemPlanets { get; set; }
        public int PlanetCount { get; set; }

        public override string ToString()
        {
            return SystemName;
        }
    }
}
