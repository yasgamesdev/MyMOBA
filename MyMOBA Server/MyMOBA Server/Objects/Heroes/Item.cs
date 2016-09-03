using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Heroes
{
    public class Item
    {
        public byte itemID;
        public Item(byte itemID)
        {
            this.itemID = itemID;
        }

        public static Item ReadData(NetIncomingMessage message)
        {
            return new Item(message.ReadByte());
        }

        public ushort GetHP()
        {
            if(itemID == 3)
            {
                return 50;
            }
            else if(itemID == 4)
            {
                return 400;
            }
            else
            {
                return 0;
            }
        }

        public int GetAttackPower()
        {
            if (itemID == 1)
            {
                return 50;
            }
            else if (itemID == 2)
            {
                return 400;
            }
            else
            {
                return 0;
            }
        }
    }
}
