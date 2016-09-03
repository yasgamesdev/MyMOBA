using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects
{
    public abstract class ServerSideObject
    {
        protected InitialData init;
        protected SyncData sync;

        //common
        public float radius { get; protected set; }
        public abstract float GetXPos();
        public abstract float GetZPos();

        public virtual void Update(float delta) { }

        public void WriteInitialData(NetOutgoingMessage message)
        {
            if (init != null)
            {
                init.WriteData(message);
            }
        }

        public void WriteSyncData(NetOutgoingMessage message)
        {
            if (sync != null)
            {
                sync.WriteData(message);
            }
        }

        public void Attack(ServerSideObject target, int amount)
        {
            if(target.IsDead())
            {
                return;
            }

            if (this.GetFaction() != target.GetFaction())
            {
                target.Damaged(this, amount);

                if (target.IsDead())
                {
                    int exp = target.CalcExp(this.Level());
                    int gold = target.CalcGold(this.Level());

                    GetExp(exp);
                    GetGold(gold);
                }

                //RefreshVision
                if ((this.GetFaction() == (byte)Faction.Blue && target.GetFaction() == (byte)Faction.Red) ||
                (this.GetFaction() == (byte)Faction.Red && target.GetFaction() == (byte)Faction.Blue))
                {
                    this.RefreshAttackVision();
                    target.RefreshAttackedVision();
                }
            }            
        }

        public virtual void RefreshAttackVision() { }
        public virtual void RefreshAttackedVision() { }

        public abstract byte GetFaction();

        public virtual void Damaged(ServerSideObject attacker, int amount) { }

        public virtual bool IsDead()
        {
            return false;
        }

        public virtual byte Level()
        {
            return 1;
        }

        public virtual int CalcExp(byte enemylevel)
        {
            return 0;
        }

        public virtual void GetExp(int exp) { }

        public virtual int CalcGold(byte enemylevel)
        {
            return 0;
        }

        public virtual void GetGold(int gold) { }

        public ushort GetObjID()
        {
            return sync.objID;
        }

        public virtual int GetAutoAttackPower()
        {
            return 0;
        }
    }
}
