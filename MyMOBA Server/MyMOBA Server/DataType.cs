using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server
{
    public enum DataType
    {
        Initialize,
        Snapshot,
        PushClientData,
        AutoAttack,
        BuyItem,
        SellItem,
        Fireball,
        Fireground,
    }
}
