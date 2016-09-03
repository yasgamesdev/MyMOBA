using MyMOBA_Server.Objects.Abilities;
using SlimMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Monsters
{
    public class Monster : ServerSideObject
    {
        //common
        const ushort maxhp = 200;
        const float attackRange = 5.0f;
        const float attackRate = 2.0f;
        float realAttackTime = 1.2f;
        float cooldownTime = 0.8f;
        float startMotion = 0;
        float endMotion = 0;
        ServerSideObject target = null;

        float respawnTimer;
        float init_yRot, init_wRot;

        public Monster(float xPos, float zPos, float yRot, float wRot)
        {
            ushort objID = ObjIDGenerator.GenerateID(ObjIDGenerator.ObjType.Monster);

            init = null;
            sync = new MonsterSyncData(objID, maxhp, xPos, zPos, yRot, wRot, 0, new BuffDebuff(0));

            radius = 0.5f;

            init_yRot = yRot;
            init_wRot = wRot;
        }

        public override float GetXPos()
        {
            return ((MonsterSyncData)sync).xPos;
        }

        public override float GetZPos()
        {
            return ((MonsterSyncData)sync).zPos;
        }

        public override byte GetFaction()
        {
            return (byte)Faction.Yellow;
        }

        public override void Update(float delta)
        {
            endMotion -= delta;

            ((MonsterSyncData)sync).Update(delta);

            if (IsDead())
            {
                respawnTimer -= delta;
                if (respawnTimer <= 0)
                {
                    ((MonsterSyncData)sync).Revive(maxhp, init_yRot, init_wRot);
                    target = null;
                    startMotion = 0;
                }
            }
            else
            {
                if (target != null)
                {
                    if(target.IsDead())
                    {
                        ((MonsterSyncData)sync).SetRotate(init_yRot, init_wRot);
                        ((MonsterSyncData)sync).SetAnime(0);
                        target = null;
                        startMotion = 0;
                    }
                    else
                    {
                        AttackEnemy(delta);
                    }
                }
            }
        }

        public override void Damaged(ServerSideObject attacker, int amount)
        {
            ((MonsterSyncData)sync).DecreaseHP(amount);

            if (IsDead())
            {
                respawnTimer = CalcRespawnTime();
            }
            else
            {
                if (target == null)
                {
                    if (attacker.GetType() == typeof(Fireground))
                    {
                        target = ((Ability)attacker).caster;
                    }
                    else
                    {
                        target = attacker;
                    }
                }
            }
        }

        public virtual float CalcRespawnTime()
        {
            return 60.0f;
        }

        public override bool IsDead()
        {
            return ((MonsterSyncData)sync).hp == 0;
        }

        public override int CalcExp(byte enemylevel)
        {
            return 30;
        }

        public override int CalcGold(byte enemylevel)
        {
            return 30;
        }

        void AttackEnemy(float delta)
        {
            float distance = ServerMath.Distance(this, target);

            if (distance <= attackRange)
            {
                Quaternion rot = ServerMath.LookAt(this, target);
                ((MonsterSyncData)sync).SetRotate(rot.Y, rot.W);
                ((MonsterSyncData)sync).SetAnime(2);

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
                ((MonsterSyncData)sync).SetRotate(init_yRot, init_wRot);
                ((MonsterSyncData)sync).SetAnime(0);
                target = null;
                startMotion = 0;                
            }
        }

        public override int GetAutoAttackPower()
        {
            return 20;
        }
    }
}
