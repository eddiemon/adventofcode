using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class D52
    {
        public string Answer
        {
            get
            {
                var objectDescriptions = File.ReadAllLines("d5.txt");

                foreach (var o in objectDescriptions)
                {
                    CreateSpaceObjects(o);
                }

                BuildOrbiters();

                var sanJumps = new Dictionary<SpaceObject, int>();
                var spaceObject = _spaceObjects["SAN"];
                int counter = 0;
                while (true)
                {
                    if (spaceObject.Orbits == null)
                        break;
                    spaceObject = spaceObject.Orbits;
                    sanJumps[spaceObject] = counter++;
                }

                var youJumps = new Dictionary<SpaceObject, int>();
                spaceObject = _spaceObjects["YOU"];
                counter = 0;
                while (true)
                {
                    if (spaceObject.Orbits == null)
                        break;
                    spaceObject = spaceObject.Orbits;
                    youJumps[spaceObject] = counter++;
                }

                SpaceObject commonSpaceObj = null;
                foreach (var kvp in youJumps)
                {
                    if (sanJumps.ContainsKey(kvp.Key))
                    {
                        commonSpaceObj = kvp.Key;
                        break;
                    }
                }
                var totalJumps = sanJumps[commonSpaceObj] + youJumps[commonSpaceObj];

                return totalJumps.ToString();
            }
        }

        private void BuildOrbiters()
        {
            foreach (var spaceObj in _spaceObjects.Values)
            {
                if (spaceObj.Orbits != null)
                {
                    spaceObj.Orbits.Orbiters.Add(spaceObj);
                }
            }
        }

        private Dictionary<string, SpaceObject> _spaceObjects = new Dictionary<string, SpaceObject>();

        private void CreateSpaceObjects(string o)
        {
            var parts = o.Split(')');
            SpaceObject obj1;
            if (_spaceObjects.TryGetValue(parts[0], out var existingObj1))
            {
                obj1 = existingObj1;
            } else
            {
                obj1 = new SpaceObject() { Name = parts[0] };
                _spaceObjects[parts[0]] = obj1;
            }
            var obj2 = _spaceObjects.TryGetValue(parts[1], out var existingObj2) ? existingObj2 : new SpaceObject() { Name = parts[1] };
            obj2.Orbits = obj1;

            _spaceObjects[parts[1]] = obj2;
        }

        private class SpaceObject
        {
            public string Name;
            public SpaceObject Orbits = null;
            public List<SpaceObject> Orbiters = new List<SpaceObject>();

            public override string ToString() => Name;
        }
    }
}