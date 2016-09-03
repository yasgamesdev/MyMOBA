using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Tower
{
    public class Tower : ServerSideObject
    {
        //common
        const ushort maxhp = 3000;
        const float attackRange = 5.0f;
        const float attackRate = 1.0f;
        float realAttackTime = 0.5f;
        float cooldownTime = 0.5f;
        float startMotion = 0;
        float endMotion = 0;

        public Tower(byte faction, float xPos, float zPos)
        {
            ushort objID = ObjIDGenerator.GenerateID(ObjIDGenerator.ObjType.Tower);

            init = new TowerInitialData(faction, xPos, zPos);
            sync = new TowerSyncData(objID, maxhp, new BuffDebuff(0), new EnemyVision(false));

            radius = 0.5f;
        }

        public override float GetXPos()
        {
            return ((TowerInitialData)init).xPos;
        }

        public override float GetZPos()
        {
            return ((TowerInitialData)init).zPos;
        }

        public override byte GetFaction()
        {
            return ((TowerInitialData)init).faction;
        }

        public override void Update(float delta)
        {
            endMotion -= delta;

            ((TowerSyncData)sync).Update(delta);

            if (!IsDead())
            {
                AttackEnemy(delta);
            }
        }

        public override void RefreshAttackVision()
        {
            ((TowerSyncData)sync).vision.Attack();
        }

        public override void RefreshAttackedVision()
        {
            ((TowerSyncData)sync).vision.Attacked();
        }

        public override void Damaged(ServerSideObject attacker, int amount)
        {
            ((TowerSyncData)sync).DecreaseHP(amount);
        }

        public override bool IsDead()
        {
            return ((TowerSyncData)sync).hp == 0;
        }

        public override int CalcExp(byte enemylevel)
        {
            return 150;
        }

        public override int CalcGold(byte enemylevel)
        {
            return 150;
        }

        void AttackEnemy(float delta)
        {
            ServerSideObject target = ObjectManager.GetNearMinionsHeroes(this, attackRange);
            if(target != null)
            {
                if (endMotion <= 0)
                {
                    startMotion += delta;
                    if (startMotion >= realAttackTime)
                    {
                        Attack(target, GetAutoAttackPower());
                        ObjectManager.AddBullet(this.GetObjID(), target.GetObjID());
                        startMotion = 0;
                        endMotion = cooldownTime;
                    }
                }
            }
            else
            {
                startMotion = 0;
            }
        }

        public override int GetAutoAttackPower()
        {
            return 50;
        }
    }
}
