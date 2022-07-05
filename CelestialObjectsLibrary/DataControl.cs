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
using System.Diagnostics;
using System.Reflection;

namespace CelestialObjectsLibrary
{
    public class DataControl
    {
        private List<CelestialObject> _systemObjects = new List<CelestialObject>();
        private List<SolarSystem> _solarSystems = new List<SolarSystem>();
        private List<string> _paths = new List<string>();
        public static string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string userImagesDir = System.IO.Path.Combine(appDirectory, "UserImages");

        public void AddObject(CelestialObject obj)
        {
            _systemObjects.Add(obj);
        }

        public void RemoveObject(CelestialObject obj)
        {
            _systemObjects.Remove(obj);
        }

        public void ClearSystemObjects()
        {
            _systemObjects.Clear();
        }

        public int SystemObjectsCount()
        {
            return _systemObjects.Count;
        }

        public void AddMoons(CelestialObject obj, ComboBox SystemList, int systemIndex, int selectedPlanetIndex)
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

        public void RemoveMoons(CelestialObject planetToRemove, ComboBox SystemList, int systemIndex)
        {
            var moonsToRemove = _systemObjects
                .Where(o => o is Moon)
                    .Where(m => ((Moon)m).GravityCenter == planetToRemove)
                    .ToList();

            _systemObjects.RemoveAll(m => moonsToRemove.Contains(m));
            ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.RemoveAll(m => moonsToRemove.Contains(m));
        }

        public CelestialObject SelectPlanet(int index)
        {
            var onlyPlanets = _systemObjects
                .Where(s => s is Planet);

            return onlyPlanets
                .ElementAt(index);
        }

        public List<CelestialObject> GetOnlyPlanets(SolarSystem solarSystem)
        {
            return solarSystem.SystemPlanets
                .Where(s => s is Planet)
                .ToList();
        }

        public List<int> GetOrbitRadiuses()
        {
            return _systemObjects
                .Where(o => o is Planet)
                    .Select(p => ((Planet)p).Radius)
                .ToList();
        }

        public int GetPlanetIndex(CelestialObject obj)
        {
            return _systemObjects
                .FindIndex(p => p.Equals(obj));
        }

        public void UpdatePlanet(CelestialObject obj, int index)
        {
            _systemObjects[index] = obj;
        }

        public int PlanetCount()
        {
            return _systemObjects
                .OfType<Planet>()
                .Count();
        }

        public int GetSystemMoonCount()
        {
            return _systemObjects
                .Where(o => o is Planet)
                    .Select(p => ((Planet)p).MoonCount)
                .Sum();
        }

        public void FillCanvas(Canvas canvas)
        {
            for (int i = 0; i < _systemObjects.Count; i++)
            {
                canvas.Children.Add(_systemObjects[i].Shape);
            }
        }

        public Stopwatch Timer = new Stopwatch();
        private double lag = 0.0;
        private double previous;

        public void AnimationUpdate(object sender, EventArgs e)
        {
            double current = Timer.Elapsed.TotalMilliseconds;
            double elapsed = current - previous;
            previous = current;

            lag += elapsed;

            while (lag >= 12)
            {
                foreach (CelestialObject obj in _systemObjects)
                {
                    obj.Update();
                }
                lag -= 12;
            }
        }

        public static BitmapImage CreateImage(string filePath)
        {
            if (filePath.Contains("file://"))
            {
                string[] pathSplit = filePath.Split(new string[] { "///" }, StringSplitOptions.None);
                filePath = pathSplit[1];
            }
            else
            {
                string filename = Path.Combine(appDirectory, "Images", filePath);
                filePath = filename;
            }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            bitmapImage.Freeze();
     
            return bitmapImage;
        }

        public void SaveToFile(ComboBox SystemList)
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

        public void LoadFromFile(ComboBox SystemList)
        {
            if (!File.Exists("data"))
            {
                return;
            }

            XmlSerializer mySerializer = new XmlSerializer(typeof(List<SolarSystem>));

            using (var myFileStream = new FileStream("data", FileMode.Open))
            {
                _solarSystems = (List<SolarSystem>)mySerializer.Deserialize(myFileStream);
            }

            foreach (var system in _solarSystems)
            {
                foreach (var o in system.SystemPlanets)
                {
                    LoadedObjectShapeSet(o);
                    if (o is Moon)
                    {
                        ((Moon)o).GravityCenter = system.SystemPlanets
                            .Where(p => p is Planet)
                                .Where(p => ((Planet)((Moon)o).GravityCenter).Radius == ((Planet)p).Radius)
                                .SingleOrDefault();
                    }
                }
                SystemList.Items.Add(system);
            }
        }

        public void LoadedObjectShapeSet(CelestialObject systemObj)
        {
            systemObj.Image.ImageSource = CreateImage(systemObj.ImageUri);
            systemObj.Shape.Width = systemObj.Width;
            systemObj.Shape.Height = systemObj.Height;
            systemObj.Shape.SetValue(Canvas.LeftProperty, systemObj.X);
            systemObj.Shape.SetValue(Canvas.TopProperty, systemObj.Y);
            systemObj.Shape.Margin = new Thickness(-systemObj.Shape.Height / 2);
            systemObj.Shape.Fill = systemObj.Image;
        }

        public void ImageCopy(List<string> paths)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i].Contains("PlanetarySystem") && paths[i].Contains("Images"))
                {
                    continue;
                }
               
                if (paths[i].Contains("file://"))
                {
                    string[] pathSplit = paths[i].Split(new string[] { "///" }, StringSplitOptions.None);
                    string path = pathSplit[1];
                    string name = System.IO.Path.GetFileName(path);
                    string combinedPath = System.IO.Path.Combine(userImagesDir, name);
                    File.Copy(path, combinedPath, true);
                    paths[i] = combinedPath;
                }
            }
        }

        public void ImageDelete()
        {
            string[] imageFilesPaths = Directory.GetFiles(userImagesDir);
            var userImages = _paths
                .Where(p => p.Contains("/UserImages") || p.Contains(@"\UserImages"))
                .ToArray();

            var userImagesPaths = new string[userImages.Length];

            for (int i = 0; i < userImages.Length; i++)
            {
                if (userImages[i].Contains("file://"))
                {
                    string[] pathSplit = userImages[i].Split(new string[] { "///" }, StringSplitOptions.None);
                    userImagesPaths[i] = pathSplit[1];
                }
                else
                {
                    userImagesPaths[i] = userImages[i].Replace(@"\", @"/");
                }
            }

            for (int i = 0; i < imageFilesPaths.Length; i++)
            {
                string path = imageFilesPaths[i].Replace(@"\", @"/");
                if (!userImagesPaths.Contains(path))
                {
                    File.Delete(imageFilesPaths[i]);
                }
            }
        }
    }
}
