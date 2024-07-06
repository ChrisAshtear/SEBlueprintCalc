using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBlueprintCalc.WeightCalc
{
    public class Component
    {
        public string name;
        public float mass;
        public Dictionary<string,float> cost;
    }

    public struct ComponentCount
    {
        public Component comp;
        public float count;
    }
}
