using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc
{
    public class D51
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

                int orbitalCounts = CountOrbitersOf(_spaceObjects["COM"], 0);

                return orbitalCounts.ToString();
            }
        }

        private int CountOrbitersOf(SpaceObject spaceObject, int orbitalCounts)
        {
            int newCount = orbitalCounts;
            foreach (var o in spaceObject.Orbiters)
            {
                newCount += CountOrbitersOf(o, orbitalCounts + 1);
            }
            return newCount;
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