using MyMOBA_Server.Objects.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Abilities
{
    public class Fireground : Ability
    {
        public Fireground(Hero caster, float xPos, float zPos) : base(caster, 1, 4.0f, xPos, zPos, 0, 1.0f)
        {
            radius = 2.0f;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            ObjectManager.ApplyFireground(this);
        }
    }
}
