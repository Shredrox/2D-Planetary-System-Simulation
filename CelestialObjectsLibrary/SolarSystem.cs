using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CelestialObjectsLibrary
{
    [Serializable]
    [XmlInclude(typeof(List<CelestialObject>))]
    public class SolarSystem
    {
        public string SystemName { get; set; }
        public List<CelestialObject> SystemPlanets { get; set; }
        public int PlanetCount { get; set; }
        public string Description { get; set; }

        public SolarSystem()
        {

        }

        public override string ToString()
        {
            return SystemName;
        }
    }
}
