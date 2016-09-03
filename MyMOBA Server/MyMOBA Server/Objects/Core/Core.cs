using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Core
{
    public class Core : ServerSideObject
    {
        //common
        const ushort maxhp = 5500;

        public Core(byte faction, float xPos, float zPos)
        {
            ushort objID = ObjIDGenerator.GenerateID(ObjIDGenerator.ObjType.Core);

            init = new CoreInitialData(faction, xPos, zPos);
            sync = new CoreSyncData(objID, maxhp, new BuffDebuff(0), new EnemyVision(false));

            radius = 1.0f;
        }

        public override float GetXPos()
        {
            return ((CoreInitialData)init).xPos;
        }

        public override float GetZPos()
        {
            return ((CoreInitialData)init).zPos;
        }

        public override byte GetFaction()
        {
            return ((CoreInitialData)init).faction;
        }

        public override void Update(float delta)
        {
            ((CoreSyncData)sync).Update(delta);
        }

        public override void RefreshAttackVision()
        {
            ((CoreSyncData)sync).vision.Attack();
        }

        public override void RefreshAttackedVision()
        {
            ((CoreSyncData)sync).vision.Attacked();
        }

        public override void Damaged(ServerSideObject attacker, int amount)
        {
            ((CoreSyncData)sync).DecreaseHP(amount);

            if (IsDead())
            {
                //未実装
            }
        }

        public override bool IsDead()
        {
            return ((CoreSyncData)sync).hp == 0;
        }
    }
}
