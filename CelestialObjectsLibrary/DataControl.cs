using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using System.Xml;

namespace CelestialObjectsLibrary
{
    public static class DataControl
    {
        private static List<CelestialObject> _systemObjects = new List<CelestialObject>();
        private static List<SolarSystem> _solarSystems = new List<SolarSystem>();
        private static List<string> _paths = new List<string>();

        public static void AddObject(CelestialObject obj)
        {
            _systemObjects.Add(obj);
        }

        public static void RemoveObject(CelestialObject obj)
        {
            _systemObjects.Remove(obj);
        }

        public static void ClearSystemObjects()
        {
            _systemObjects.Clear();
        }

        public static int SystemObjectsCount()
        {
            return _systemObjects.Count;
        }

        public static void AddMoons(CelestialObject obj, ComboBox SystemList, int systemIndex, int selectedPlanetIndex)
        {
            if (((Planet)obj).MoonCount <= 3 && ((Planet)obj).MoonCount > 0)
            {
                for (int m = 1; m < ((Planet)obj).MoonCount + 1; m++)
                {
                    ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.Insert(selectedPlanetIndex + 1,
                        new Moon($"Moon {m}", CreateImage("../../Images/moon.png"), 10, 10, true, 5, 5, obj, obj.Width / 2 + 10 + m * 4, m));

                    _systemObjects.Insert(selectedPlanetIndex + 1,
                        new Moon($"Moon {m}", CreateImage("../../Images/moon.png"), 10, 10, true, 5, 5, obj, obj.Width / 2 + 10 + m * 4, m));
                }
            }
            else if (((Planet)obj).MoonCount > 3)
            {
                for (int m = 1; m < 4; m++)
                {
                    ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.Insert(selectedPlanetIndex + 1,
                        new Moon($"Moon {m}", CreateImage("../../Images/moon.png"), 10, 10, true, 5, 5, obj, obj.Width / 2 + 10 + m * 4, m));

                    _systemObjects.Insert(selectedPlanetIndex + 1,
                        new Moon($"Moon {m}", CreateImage("../../Images/moon.png"), 10, 10, true, 5, 5, obj, obj.Width / 2 + 10 + m * 4, m));
                }
            }
        }

        public static void RemoveMoons(CelestialObject planetToRemove, ComboBox SystemList, int systemIndex)
        {
            var moonsToRemove = _systemObjects
                .Where(o => o is Moon)
                    .Where(m => ((Moon)m).GravityCenter == planetToRemove)
                    .ToList();

            _systemObjects.RemoveAll(m => moonsToRemove.Contains(m));
            ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.RemoveAll(m => moonsToRemove.Contains(m));
        }

        public static CelestialObject SelectPlanet(int index)
        {
            var onlyPlanets = _systemObjects
                .Where(s => s is Planet);

            return onlyPlanets
                .ElementAt(index);
        }

        public static List<CelestialObject> GetOnlyPlanets(SolarSystem solarSystem)
        {
            return solarSystem.SystemPlanets
                .Where(s => s is Planet)
                .ToList();
        }

        public static int GetPlanetIndex(CelestialObject obj)
        {
            return _systemObjects
                .FindIndex(p => p.Equals(obj));
        }

        public static void UpdatePlanet(CelestialObject obj, int index)
        {
            _systemObjects[index] = obj;
        }

        public static int PlanetCount()
        {
            return _systemObjects
                .OfType<Planet>()
                .Count();
        }

        public static void SetOrbitRadius()
        {
            int orbitCounter = 1;
            for (int p = 0; p < _systemObjects.Count; p++)
            {
                if (_systemObjects[p] is Planet)
                {
                    switch (orbitCounter)
                    {
                        case 1: ((Planet)_systemObjects[p]).Radius = 50; break;
                        case 2: ((Planet)_systemObjects[p]).Radius = 100; break;
                        case 3: ((Planet)_systemObjects[p]).Radius = 150; break;
                        case 4: ((Planet)_systemObjects[p]).Radius = 200; break;
                        case 5: ((Planet)_systemObjects[p]).Radius = 280; break;
                        case 6: ((Planet)_systemObjects[p]).Radius = 330; break;
                        case 7: ((Planet)_systemObjects[p]).Radius = 370; break;
                        case 8: ((Planet)_systemObjects[p]).Radius = 400; break;
                    }
                    orbitCounter++;
                }
            }
        }

        public static void FillCanvas(Canvas canvas)
        {
            for (int i = 0; i < _systemObjects.Count; i++)
            {
                canvas.Children.Add(_systemObjects[i].Shape);
            }
        }

        public static void AnimationUpdate(object sender, EventArgs e)
        {
            foreach (CelestialObject obj in _systemObjects)
            {
                obj.Update();
            }
        }

        public static BitmapImage CreateImage(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        public static void SaveToFile(ComboBox SystemList)
        {
            _solarSystems.Clear();
            int pathIndex = 0;

            foreach (var system in SystemList.Items)
            {
                for (int i = 0; i < ((SolarSystem)system).SystemPlanets.Count; i++)
                {
                    _paths.Add(((SolarSystem)system).SystemPlanets[i].Image.ImageSource.ToString());
                }
                ImageCopy(_paths);

                for (int i = 0; i < ((SolarSystem)system).SystemPlanets.Count; i++)
                {
                    ((SolarSystem)system).SystemPlanets[i].ImageUri = _paths[i + pathIndex];
                }

                pathIndex += ((SolarSystem)system).SystemPlanets.Count;
                _solarSystems.Add((SolarSystem)system);
            }

            XmlSerializer mySerializer = new XmlSerializer(typeof(List<SolarSystem>));
            XmlTextWriter myWriter = new XmlTextWriter("data", Encoding.UTF8);
            mySerializer.Serialize(myWriter, _solarSystems);
            myWriter.Close();
        }

        public static void LoadFromFile(ComboBox SystemList)
        {
            _solarSystems.Clear();
            XmlSerializer mySerializer = new XmlSerializer(typeof(List<SolarSystem>));

            if (File.Exists("data"))
            {
                using (var myFileStream = new FileStream("data", FileMode.Open))
                {
                    _solarSystems = (List<SolarSystem>)mySerializer.Deserialize(myFileStream);
                }

                foreach (var system in _solarSystems)
                {
                    foreach (var s in system.SystemPlanets)
                    {
                        LoadedObjectShapeSet(s);
                        if (s is Moon)
                        {
                            int moonIndex = system.SystemPlanets.FindIndex(p => p.Equals(s));

                            for (int i = moonIndex; i >= 0; i--)
                            {
                                if (system.SystemPlanets[i] is Planet)
                                {
                                    ((Moon)s).GravityCenter = system.SystemPlanets[i];
                                    break;
                                }
                            }
                        }
                    }
                    SystemList.Items.Add(system);
                }
            }
        }

        public static void LoadedObjectShapeSet(CelestialObject systemObj)
        {
            systemObj.Image.ImageSource = CreateImage(systemObj.ImageUri);
            systemObj.Shape.Width = systemObj.Width;
            systemObj.Shape.Height = systemObj.Height;
            systemObj.Shape.SetValue(Canvas.LeftProperty, systemObj.X);
            systemObj.Shape.SetValue(Canvas.TopProperty, systemObj.Y);
            systemObj.Shape.Margin = new Thickness(-systemObj.Shape.Height / 2);
            systemObj.Shape.Fill = systemObj.Image;
        }

        public static void ImageCopy(List<String> paths)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i].Contains("file://"))
                {
                    string[] pathSplit = paths[i].Split(new string[] { "///" }, StringSplitOptions.None);
                    string path = pathSplit[1];
                    string name = System.IO.Path.GetFileName(path);
                    string combinedPath = System.IO.Path.Combine("../../UserImages/", name);
                    File.Copy(path, combinedPath, true);
                    paths[i] = combinedPath;
                }
            }
        }

        public static void ImageDelete()
        {
            string[] imageFilesPaths = Directory.GetFiles("../../UserImages/");
            var userImages = _paths
                .Where(p => p.Contains("/UserImages"))
                .ToArray();

            for (int i = 0; i < imageFilesPaths.Length; i++)
            {
                if (!userImages.Contains(imageFilesPaths[i]))
                {
                    File.Delete(imageFilesPaths[i]);
                }
            }
        }
    }
}
