using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBlueprintCalc.WeightCalc
{
    public class Block
    {
        public string name;
        public List<ComponentCount> compCost;
        public Dictionary<string, float> cost;
        public string category;
        public float Mass { get { return GetMass(); } }

        public float GetMass()
        {
            float mass = 0;
            foreach(ComponentCount c in compCost)
            {
                mass += c.comp.mass * c.count;
            }
            return mass;
        }

        public Block()
        {
            compCost = new List<ComponentCount>();
        }
    }

    public class BlockCount
    {
        public Block block;
        public float count;

        public float Mass { get { return block.Mass * count; } }

        public override string ToString()
        {
            return $"{block.name} x {count}";
        }

    }
}
