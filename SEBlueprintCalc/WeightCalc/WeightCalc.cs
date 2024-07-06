using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static SEBlueprintCalc.Form1;

namespace SEBlueprintCalc.WeightCalc
{
    public class WeightCalc
    {
        public string rootDir = "../"; //Directory.GetCurrentDirectory();
        public Dictionary<string, Component> componentList = new Dictionary<string, Component>();
        public Dictionary<string, Block> blockList = new Dictionary<string, Block>();
        public void LoadComponents()
        {
            JObject comps = JObject.Parse(File.ReadAllText(rootDir + "../Data/Components.json"));

            foreach (var comp in comps)
            {
                Component c = new Component();
                c.mass = comp.Value.SelectToken("Mass").ToObject<int>();
                c.cost = comp.Value.ToObject<ItemData>().Cost;
                c.name = comp.Key;
                componentList.Add(comp.Key, c);
            }
        }

        public void LoadBlocks()
        {
            JObject blocks = JObject.Parse(File.ReadAllText(rootDir + "../Data/Blocks.json"));

            foreach (var block in blocks)
            {

                Block b = new Block();
                b.cost = block.Value.ToObject<ItemData>().Cost;
                foreach(var item in b.cost)
                {
                    bool found = componentList.TryGetValue(item.Key, out Component c);
                    if (!found)
                    {
                        Console.Write($"ERROR Cant find {item.Key}");
                    }
                    else
                    {
                        b.compCost.Add(new ComponentCount { comp = c, count = item.Value });
                    }
                    
                }

                b.name = block.Key;
                blockList.Add(block.Key, b);
            }
        }

        public WeightCalc()
        {
            LoadComponents();
            LoadBlocks();
        }

        public void DoCalc(string data)
        {
            string[] lines = data.Split('\n');
            List<BlockCount> blocks = new List<BlockCount>();
            foreach (string line in lines)
            {
                if(!line.Contains("Block/"))
                {
                    continue;
                }

                string blockNameT = line.Split('/')[1];
                string blockName = blockNameT.Split('=')[0];
                string count = blockNameT.Split('=')[1];
                bool foundBlock = blockList.TryGetValue(blockName, out Block block);
                
                if(foundBlock)
                {
                    blocks.Add(new BlockCount { block = block, count = int.Parse(count) });
                }
            }

            string output = "";

            blocks = blocks.OrderByDescending(x => x.Mass).ToList();

            foreach(BlockCount block in blocks)
            {
                output += $"{block} - {block.Mass}\n";
            }
            Clipboard.SetText(output);
        }


    }


}
