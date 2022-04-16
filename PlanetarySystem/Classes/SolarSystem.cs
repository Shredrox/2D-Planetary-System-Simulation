using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlanetarySystem
{
    [Serializable]
    [XmlInclude(typeof(List<CelestialObject>))]
    public class SolarSystem
    {
        public string SystemName { get; set; }
        public List<CelestialObject> SystemPlanets { get; set; }
        public int PlanetCount { get; set; }

        public SolarSystem()
        {

        }

        public override string ToString()
        {
            return SystemName;
        }
    }
}
