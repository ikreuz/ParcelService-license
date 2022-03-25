using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelBlock
{
    public class Blockchain
    {
        public IList<Block> Node { get; set; }
        public int Difficulty { get; set; } = 2;

        public Blockchain()
        {
            Initialize();
            AddGenesis();
        }

        public void Initialize()
        {
            Node = new List<Block>();
        }

        public Block Genesis()
        {
            //return new Block(Convert.ToDateTime("2021-12-10T19:08:21.8669267-06:00"), null, "{}");
            Block block = new Block(DateTime.Now, null, "{}");
            block.Index = 0;
            block.TimeStamp = Convert.ToDateTime("2020-11-09T19:08:21.8669267-06:00");
            block.PreviousHash = null;
            block.Hash = "0rRIxb1nYUrHgmC59gLxLXB481AsxPK9gNzwX8dfPrA=";
            block.Data = "{}";
            return block;
        }

        public void AddGenesis()
        {
            Node.Add(Genesis());
        }

        public Block GetLastBlock()
        {
            return Node[Node.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latesBlock = GetLastBlock();
            block.Index = latesBlock.Index + 1;
            block.PreviousHash = latesBlock.Hash;
            block.Mine(this.Difficulty);

            Node.Add(block);
        }

        public bool IsValid()
        {
            for(int i = 1; i < Node.Count; i++)
            {
                Block currentBlock = Node[i];
                Block previousBlock = Node[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash()) return false;

                if (currentBlock.PreviousHash != previousBlock.Hash) return false;
            }

            return true;
        }
    }
}
