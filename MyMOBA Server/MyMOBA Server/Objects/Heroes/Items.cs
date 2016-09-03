using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Heroes
{
    public class Items
    {
        public Item[] items;

        public Items()
        {
            items = new Item[6];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Item(0);
            }
        }

        public Items(Item[] items)
        {
            this.items = items;
        }

        public void WriteData(NetOutgoingMessage message)
        {
            foreach (Item i in items)
            {
                message.Write(i.itemID);
            }
        }

        public static Items ReadData(NetIncomingMessage message)
        {
            Item[] items = new Item[6];
            for (int i = 0; i < 6; i++)
            {
                items[i] = Item.ReadData(message);
            }

            return new Items(items);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Items target = (Items)obj;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].itemID != target.items[i].itemID)
                {
                    return false;
                }
            }

            return true;
        }

        public ushort GetItemPrice(byte itemID)
        {
            if (itemID == 1 || itemID == 3)
            {
                return 200;
            }
            else
            {
                return 1200;
            }
        }

        public bool HaveSpace()
        {
            foreach(Item item in items)
            {
                if(item.itemID == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void BuyItem(byte itemID)
        {
            foreach (Item item in items)
            {
                if (item.itemID == 0)
                {
                    item.itemID = itemID;
                    break;
                }
            }
        }

        public void SellItem(byte slotID)
        {
            items[slotID].itemID = 0;
        }

        public ushort GetHP()
        {
            ushort sum = 0;
            foreach (Item item in items)
            {
                sum += item.GetHP();
            }

            return sum;
        }

        public int GetAttackPower()
        {
            int sum = 0;
            foreach (Item item in items)
            {
                sum += item.GetAttackPower();
            }

            return sum;
        }
    }
}
