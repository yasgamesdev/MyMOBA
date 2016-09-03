using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects
{
    public static class ObjIDGenerator
    {
        public enum ObjType
        {
            Hero,
            Core,
            Tower,
            Monster,
            Minion,
            Ability,
        }
        static ushort[,] ranges = new ushort[6, 2] { { 0, 127 }, { 128, 129 }, { 130, 255 }, { 256, 511 }, { 512, 32767 }, { 32768, 65535 } };
        static ushort[] count = new ushort[6];
        static bool isInit = false;

        public static void ResetID()
        {
            for(int i=0; i<count.Length; i++)
            {
                count[i] = ranges[i, 0];
            }
        }

        public static ushort GenerateID(ObjType type)
        {
            if(!isInit)
            {
                ResetID();
                isInit = true;
            }

            ushort ret = count[(int)type];

            if (count[(int)type] == ranges[(int)type, 1])
            {
                count[(int)type] = ranges[(int)type, 0];
            }
            else
            {
                count[(int)type]++;
            }

            return ret;
        }
    }
}
