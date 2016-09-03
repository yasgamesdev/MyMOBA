using MyMOBA_Server.Objects.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Abilities
{
    public abstract class Ability : ServerSideObject
    {
        public Hero caster { get; private set; }
        protected float lifeTimer;

        public Ability(Hero caster, byte abilityID, float lifeTimer, float xPos, float zPos, float yRot, float wRot)
        {
            this.caster = caster;
            this.lifeTimer = lifeTimer;

            ushort objID = ObjIDGenerator.GenerateID(ObjIDGenerator.ObjType.Ability);

            init = null;
            sync = new AbilitySyncData(objID, abilityID, caster.GetObjID(), caster.GetFaction(), xPos, zPos, yRot, wRot, 0, new EnemyVision(false));
        }

        public override float GetXPos()
        {
            return ((AbilitySyncData)sync).xPos;
        }

        public override float GetZPos()
        {
            return ((AbilitySyncData)sync).zPos;
        }

        public override byte GetFaction()
        {
            return ((AbilitySyncData)sync).faction;
        }

        public override void Update(float delta)
        {
            lifeTimer -= delta;
            ((AbilitySyncData)sync).Update(delta);
        }

        public override void RefreshAttackVision()
        {
            ((AbilitySyncData)sync).vision.Attack();
            caster.RefreshAttackVision();
        }

        public override void RefreshAttackedVision()
        {
            ((AbilitySyncData)sync).vision.Attacked();
        }

        public override bool IsDead()
        {
            return lifeTimer <= 0;
        }

        public override void GetExp(int exp)
        {
            caster.GetExp(exp);
        }

        public override void GetGold(int gold)
        {
            caster.GetGold(gold);
        }
    }
}
