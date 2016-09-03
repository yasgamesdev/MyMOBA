using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMOBA_Server.Objects.Heroes;

namespace MyMOBA_Server.Objects.Abilities
{
    public class Fireball : Ability
    {
        const float speed = 8.0f;
        float xDir, zDir;

        public Fireball(Hero caster, float xPos, float zPos, float xDir, float zDir) : base(caster, 0, 1.5f, xPos + xDir * 0.3f, zPos + zDir * 0.3f, 0, 1.0f)
        {
            this.xDir = xDir;
            this.zDir = zDir;

            radius = 0.25f;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            AbilitySyncData absync = (AbilitySyncData)sync;
            float newXPos = absync.xPos + xDir * speed * delta;
            float newZPos = absync.zPos + zDir * speed * delta;
            absync.SetPosition(newXPos, newZPos);

            if(ObjectManager.ApplyFireball(this))
            {
                lifeTimer = 0;
            }
        }
    }
}
