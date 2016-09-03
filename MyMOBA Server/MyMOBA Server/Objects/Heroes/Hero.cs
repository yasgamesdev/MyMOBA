using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Heroes
{
    public abstract class Hero : ServerSideObject
    {
        protected ExpPool pool;
        float respawnTimer;

        public Hero(byte faction, byte heroID, ushort hp, ushort mp, float xPos, float zPos, float yRot, float wRot)
        {            
            ushort objID = ObjIDGenerator.GenerateID(ObjIDGenerator.ObjType.Hero);

            //debug
            if(objID % 2 == 0)
            {
                faction = 0;
                xPos = -48.0f;
                zPos = -48.0f;
            }
            else
            {
                faction = 1;
                xPos = 48.0f;
                zPos = 48.0f;
            }

            init = new HeroInitialData(faction, heroID);
            sync = new HeroSyncData(objID, hp, mp, xPos, zPos, yRot, wRot, 0, 1, 0, new Items(), new BuffDebuff(0), new EnemyVision(false));
            pool = new ExpPool((HeroSyncData)sync);

            radius = 0.3f;            
        }

        public override float GetXPos()
        {
            return ((HeroSyncData)sync).xPos;
        }

        public override float GetZPos()
        {
            return ((HeroSyncData)sync).zPos;
        }

        public abstract ushort GetMaxHP();
        public virtual ushort GetMaxMP()
        {
            return 0;
        }

        public override byte GetFaction()
        {
            return ((HeroInitialData)init).faction;
        }

        public override void Update(float delta)
        {
            ((HeroSyncData)sync).Update(delta);

            if(IsDead())
            {
                respawnTimer -= delta;
                if(respawnTimer <= 0)
                {
                    if(GetFaction() == (byte)Faction.Blue)
                    {
                        ((HeroSyncData)sync).Revive(GetMaxHP(), GetMaxMP(), -48.0f, -48.0f, 0, 1.0f);
                    }
                    else
                    {
                        ((HeroSyncData)sync).Revive(GetMaxHP(), GetMaxMP(), 48.0f, 48.0f, 0, 1.0f);
                    }
                }
            }
        }

        public override void RefreshAttackVision()
        {
            ((HeroSyncData)sync).vision.Attack();
        }

        public override void RefreshAttackedVision()
        {
            ((HeroSyncData)sync).vision.Attacked();
        }

        public override void Damaged(ServerSideObject attacker, int amount)
        {
            ((HeroSyncData)sync).DecreaseHP(amount);

            if(IsDead())
            {
                respawnTimer = CalcRespawnTime(((HeroSyncData)sync).level);
            }
        }

        float CalcRespawnTime(int level)
        {
            //common
            return 5.0f;
        }

        public override bool IsDead()
        {
            return ((HeroSyncData)sync).hp == 0;
        }

        public override byte Level()
        {
            return ((HeroSyncData)sync).level;
        }

        public override int CalcExp(byte enemylevel)
        {
            return 100;
        }

        public override void GetExp(int exp)
        {
            pool.GetExp(exp);
        }

        public override int CalcGold(byte enemylevel)
        {
            return 100;
        }

        public override void GetGold(int gold)
        {
            ((HeroSyncData)sync).IncreaseGold(gold);
        }        

        public void PushFromClient(NetIncomingMessage message)
        {
            float xPos = message.ReadFloat();
            float zPos = message.ReadFloat();
            float yRot = message.ReadFloat();
            float wRot = message.ReadFloat();
            byte anime = message.ReadByte();

            ((HeroSyncData)sync).Push(xPos, zPos, yRot, wRot, anime);
        }

        public override int GetAutoAttackPower()
        {
            //debug
            return 25 + ((HeroSyncData)sync).items.GetAttackPower();
        }

        public void BuyItem(byte itemID)
        {
            Items items = ((HeroSyncData)sync).items;
            ushort price = items.GetItemPrice(itemID);
            if(((HeroSyncData)sync).gold < price)
            {
                return;
            }

            if(!items.HaveSpace())
            {
                return;
            }

            items.BuyItem(itemID);
            ((HeroSyncData)sync).gold -= price;
        }

        public void SellItem(byte slotID)
        {
            if(slotID < 0 || slotID >= 6)
            {
                return;
            }

            Item item = ((HeroSyncData)sync).items.items[slotID];
            if(item.itemID == 0)
            {
                return;
            }

            ushort price = ((HeroSyncData)sync).items.GetItemPrice(item.itemID);
            price = (ushort)(price / 2);

            ((HeroSyncData)sync).items.SellItem(slotID);
            ((HeroSyncData)sync).gold += price;

            ushort maxhp = GetMaxHP();
            if(((HeroSyncData)sync).hp > maxhp)
            {
                ((HeroSyncData)sync).SetHP(maxhp);
            }
        }

        public void Heal()
        {
            ushort maxhp = GetMaxHP();
            ushort hpamount = (ushort)(maxhp / 100);           

            ushort maxmp = GetMaxMP();
            ushort mpamount = (ushort)(maxmp / 100);

            ((HeroSyncData)sync).Heal(hpamount);
            if (((HeroSyncData)sync).hp > maxhp)
            {
                ((HeroSyncData)sync).SetHP(maxhp);
            }

            ((HeroSyncData)sync).mp += mpamount;
            if(((HeroSyncData)sync).mp > maxmp)
            {
                ((HeroSyncData)sync).mp = maxmp;
            }
        }

        public ushort GetMP()
        {
            return ((HeroSyncData)sync).mp;
        }

        public void SetMP(ushort amount)
        {
            ((HeroSyncData)sync).mp = amount;
        }
    }
}
