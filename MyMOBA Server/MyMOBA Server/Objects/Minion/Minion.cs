using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimMath;

namespace MyMOBA_Server.Objects.Minion
{
    public class Minion : ServerSideObject
    {
        //common
        const ushort maxhp = 100;
        const float moveSpeed = 2.0f;
        const float attackRange = 4.0f;
        const float attackRate = 1.0f;
        float realAttackTime = 0.6f;
        float cooldownTime = 0.4f;
        float startMotion = 0;
        float endMotion = 0;

        List<Vector2> relayPoints = new List<Vector2>();
        int currentPath = 0;

        public Minion(byte faction, float xPos, float zPos, float yRot, float wRot, List<Vector2> relayPoints)
        {
            ushort objID = ObjIDGenerator.GenerateID(ObjIDGenerator.ObjType.Minion);

            init = null;
            sync = new MinionSyncData(objID, maxhp, faction, xPos, zPos, yRot, wRot, 1, new BuffDebuff(0), new EnemyVision(false));

            radius = 0.3f;

            this.relayPoints = relayPoints;
        }

        public override float GetXPos()
        {
            return ((MinionSyncData)sync).xPos;
        }

        public override float GetZPos()
        {
            return ((MinionSyncData)sync).zPos;
        }

        public override byte GetFaction()
        {
            return ((MinionSyncData)sync).faction;
        }

        public override void Update(float delta)
        {
            endMotion -= delta;

            ((MinionSyncData)sync).Update(delta);

            if (!IsDead())
            {
                AttackMove(delta);
            }
        }

        public override void RefreshAttackVision()
        {
            ((MinionSyncData)sync).vision.Attack();
        }

        public override void RefreshAttackedVision()
        {
            ((MinionSyncData)sync).vision.Attacked();
        }

        public override void Damaged(ServerSideObject attacker, int amount)
        {
            ((MinionSyncData)sync).DecreaseHP(amount);
        }

        public override bool IsDead()
        {
            return ((MinionSyncData)sync).hp == 0;
        }

        public override int CalcExp(byte enemylevel)
        {
            return 10;
        }

        public override int CalcGold(byte enemylevel)
        {
            return 10;
        }

        void AttackMove(float delta)
        {
            if (currentPath < relayPoints.Count)
            {
                Vector3 src = new Vector3(GetXPos(), 0, GetZPos());
                Vector3 dest = new Vector3(relayPoints[currentPath].X, 0, relayPoints[currentPath].Y);
                if (Vector3.Distance(src, dest) < 0.2f)
                {
                    currentPath++;
                }
                else
                {
                    ServerSideObject target = ObjectManager.GetNearMinionsCoresTowersHeroes(this, attackRange);
                    if (target != null)
                    {
                        Quaternion rot = ServerMath.LookAt(this, target);
                        ((MinionSyncData)sync).SetRotate(rot.Y, rot.W);
                        ((MinionSyncData)sync).SetAnime(2);

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
                        Quaternion rot = ServerMath.LookAt(src, dest);
                        ((MinionSyncData)sync).SetRotate(rot.Y, rot.W);
                        ((MinionSyncData)sync).SetAnime(1);

                        Vector3 direct = (dest - src);
                        direct.Normalize();
                        Vector3 newPos = src + direct * moveSpeed * delta;
                        ((MinionSyncData)sync).SetPosition(newPos.X, newPos.Z);                        
                    }
                }
            }
        }

        public override int GetAutoAttackPower()
        {
            return 10;
        }
    }
}
