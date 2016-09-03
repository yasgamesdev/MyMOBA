using Lidgren.Network;
using MyMOBA_Server.Objects;
using MyMOBA_Server.Objects.Abilities;
using MyMOBA_Server.Objects.Core;
using MyMOBA_Server.Objects.Heroes;
using MyMOBA_Server.Objects.Minion;
using MyMOBA_Server.Objects.Monsters;
using MyMOBA_Server.Objects.Tower;
using SlimMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server
{
    public static class ObjectManager
    {
        static Dictionary<NetConnection, Client> clients = new Dictionary<NetConnection, Client>();
        static Dictionary<ushort, Hero> heroes = new Dictionary<ushort, Hero>();
        static Dictionary<ushort, Core> cores = new Dictionary<ushort, Core>();
        static Dictionary<ushort, Tower> towers = new Dictionary<ushort, Tower>();
        static Dictionary<ushort, Monster> monsters = new Dictionary<ushort, Monster>();
        static Dictionary<ushort, Minion> minions = new Dictionary<ushort, Minion>();
        static Dictionary<ushort, Ability> abilities = new Dictionary<ushort, Ability>();
        static List<Bullet> bullets = new List<Bullet>();

        public static void AddClient(Client client)
        {
            clients.Add(client.connect, client);
        }

        public static void AddHero(Hero hero)
        {
            heroes.Add(hero.GetObjID(), hero);
        }

        public static void AddTower(Tower tower)
        {
            towers.Add(tower.GetObjID(), tower);
        }

        public static void AddCore(Core core)
        {
            cores.Add(core.GetObjID(), core);
        }

        public static void AddMonster(Monster monster)
        {
            monsters.Add(monster.GetObjID(), monster);
        }

        public static void AddMinion(Minion minion)
        {
            minions.Add(minion.GetObjID(), minion);
        }

        public static void AddBullet(ushort srcObjID, ushort destObjID)
        {
            bullets.Add(new Bullet(srcObjID, destObjID));
        }

        public static void AddFireball(NetConnection connect, float xPos, float zPos, float xDir, float zDir)
        {
            Hero hero = GetHero(connect);
            if (hero.GetMP() >= 10)
            {
                Fireball ability = new Fireball(hero, xPos, zPos, xDir, zDir);
                abilities.Add(ability.GetObjID(), ability);
                hero.SetMP((ushort)(hero.GetMP() - 10));
            }
        }

        public static void AddFireground(NetConnection connect, float xPos, float zPos)
        {
            Hero hero = GetHero(connect);
            if (hero.GetMP() >= 20)
            {
                Fireground ability = new Fireground(hero, xPos, zPos);
                abilities.Add(ability.GetObjID(), ability);
                hero.SetMP((ushort)(hero.GetMP() - 20));
            }
        }

        public static void WriteInitialData(NetOutgoingMessage message)
        {
            message.Write((byte)heroes.Count);
            foreach(KeyValuePair<ushort, Hero> hero in heroes)
            {
                hero.Value.WriteInitialData(message);
                hero.Value.WriteSyncData(message);
            }

            message.Write((byte)cores.Count);
            foreach (KeyValuePair<ushort, Core> core in cores)
            {
                core.Value.WriteInitialData(message);
                core.Value.WriteSyncData(message);
            }

            message.Write((byte)towers.Count);
            foreach (KeyValuePair<ushort, Tower> tower in towers)
            {
                tower.Value.WriteInitialData(message);
                tower.Value.WriteSyncData(message);
            }
        }

        public static void WriteSnapshot(NetOutgoingMessage message)
        {
            message.Write((byte)heroes.Count);
            foreach (KeyValuePair<ushort, Hero> hero in heroes)
            {
                hero.Value.WriteInitialData(message);
                hero.Value.WriteSyncData(message);
            }

            message.Write((byte)cores.Count);
            foreach (KeyValuePair<ushort, Core> core in cores)
            {
                core.Value.WriteSyncData(message);
            }

            message.Write((byte)towers.Count);
            foreach (KeyValuePair<ushort, Tower> tower in towers)
            {
                tower.Value.WriteSyncData(message);
            }

            message.Write(GetAliveMonsterCount());
            foreach (KeyValuePair<ushort, Monster> monster in monsters)
            {
                if (!monster.Value.IsDead())
                {
                    monster.Value.WriteSyncData(message);
                }
            }

            message.Write((byte)minions.Count);
            foreach (KeyValuePair<ushort, Minion> minion in minions)
            {
                minion.Value.WriteSyncData(message);
            }

            message.Write((byte)abilities.Count);
            foreach (KeyValuePair<ushort, Ability> ability in abilities)
            {
                ability.Value.WriteSyncData(message);
            }

            message.Write((byte)bullets.Count);
            foreach (Bullet bullet in bullets)
            {
                bullet.WriteData(message);
            }
            bullets.Clear();
        }

        public static Hero GetHero(NetConnection connet)
        {
            ushort objID = clients[connet].objID;
            return heroes[objID];
        }

        public static void ApplyAutoAttack(NetConnection connet, ushort tarObjID)
        {
            ushort objID = clients[connet].objID;
            Hero hero = heroes[objID];

            ServerSideObject target = GetObject(tarObjID);

            if(target != null)
            {
                hero.Attack(target, hero.GetAutoAttackPower());
                AddBullet(hero.GetObjID(), target.GetObjID());
            }
        }

        public static ServerSideObject GetObject(ushort objID)
        {
            if (heroes.ContainsKey(objID))
            {
                return heroes[objID];
            }

            if (cores.ContainsKey(objID))
            {
                return cores[objID];
            }

            if (towers.ContainsKey(objID))
            {
                return towers[objID];
            }

            if (monsters.ContainsKey(objID))
            {
                return monsters[objID];
            }

            if (minions.ContainsKey(objID))
            {
                return minions[objID];
            }

            if (abilities.ContainsKey(objID))
            {
                return abilities[objID];
            }

            return null;
        }

        public static void Update(float delta)
        {
            foreach (KeyValuePair<ushort, Hero> hero in heroes)
            {
                hero.Value.Update(delta);
            }
            foreach (KeyValuePair<ushort, Core> core in cores)
            {
                core.Value.Update(delta);
            }
            foreach (KeyValuePair<ushort, Tower> tower in towers)
            {
                tower.Value.Update(delta);
            }
            foreach (KeyValuePair<ushort, Monster> monster in monsters)
            {
                monster.Value.Update(delta);
            }
            foreach (KeyValuePair<ushort, Minion> minion in minions)
            {
                minion.Value.Update(delta);
            }
            foreach (KeyValuePair<ushort, Ability> ability in abilities)
            {
                ability.Value.Update(delta);
            }
        }

        static byte GetAliveMonsterCount()
        {
            byte count = 0;
            foreach (KeyValuePair<ushort, Monster> monster in monsters)
            {
                if(!monster.Value.IsDead())
                {
                    count++;
                }
            }
            return count;
        }

        public static void DeleteDeadMinion()
        {
            var removes = minions.Where(f => f.Value.IsDead() == true).ToArray();
            foreach (var remove in removes)
            {
                minions.Remove(remove.Key);
            }
        }

        public static void DeleteExpiredAbility()
        {
            var removes = abilities.Where(f => f.Value.IsDead() == true).ToArray();
            foreach (var remove in removes)
            {
                abilities.Remove(remove.Key);
            }
        }

        public static ServerSideObject GetNearMinionsHeroes(ServerSideObject tower, float attackRange)
        {
            ServerSideObject ret = null;
            float mindistance = float.MaxValue;

            foreach (KeyValuePair<ushort, Minion> minion in minions)
            {
                if (!minion.Value.IsDead() && minion.Value.GetFaction() != tower.GetFaction()) {
                    float distance = ServerMath.Distance(tower, minion.Value);
                    if (distance <= attackRange && distance < mindistance)
                    {
                        ret = minion.Value;
                        mindistance = distance;
                    }
                }
            }

            if(ret != null)
            {
                return ret;
            }

            foreach (KeyValuePair<ushort, Hero> hero in heroes)
            {
                if (!hero.Value.IsDead() && hero.Value.GetFaction() != tower.GetFaction())
                {
                    float distance = ServerMath.Distance(tower, hero.Value);
                    if (distance <= attackRange && distance < mindistance)
                    {
                        ret = hero.Value;
                        mindistance = distance;
                    }
                }
            }

            return ret;
        }

        public static ServerSideObject GetNearMinionsCoresTowersHeroes(ServerSideObject myminion, float attackRange)
        {
            ServerSideObject ret = null;
            float mindistance = float.MaxValue;

            foreach (KeyValuePair<ushort, Minion> minion in minions)
            {
                if (!minion.Value.IsDead() && minion.Value.GetFaction() != myminion.GetFaction())
                {
                    float distance = ServerMath.Distance(myminion, minion.Value);
                    if (distance <= attackRange && distance < mindistance)
                    {
                        ret = minion.Value;
                        mindistance = distance;
                    }
                }
            }

            if (ret != null)
            {
                return ret;
            }

            foreach (KeyValuePair<ushort, Core> core in cores)
            {
                if (!core.Value.IsDead() && core.Value.GetFaction() != myminion.GetFaction())
                {
                    float distance = ServerMath.Distance(myminion, core.Value);
                    if (distance <= attackRange && distance < mindistance)
                    {
                        ret = core.Value;
                        mindistance = distance;
                    }
                }
            }

            if (ret != null)
            {
                return ret;
            }

            foreach (KeyValuePair<ushort, Tower> tower in towers)
            {
                if(!tower.Value.IsDead() && tower.Value.GetFaction() != myminion.GetFaction())
                {
                    float distance = ServerMath.Distance(myminion, tower.Value);
                    if (distance <= attackRange && distance < mindistance)
                    {
                        ret = tower.Value;
                        mindistance = distance;
                    }
                }
            }

            if (ret != null)
            {
                return ret;
            }

            foreach (KeyValuePair<ushort, Hero> hero in heroes)
            {
                if (!hero.Value.IsDead() && hero.Value.GetFaction() != myminion.GetFaction())
                {
                    float distance = ServerMath.Distance(myminion, hero.Value);
                    if (distance <= attackRange && distance < mindistance)
                    {
                        ret = hero.Value;
                        mindistance = distance;
                    }
                }
            }

            return ret;
        }

        public static void BuyItem(NetConnection connet, byte itemID)
        {
            ushort objID = clients[connet].objID;
            Hero hero = heroes[objID];

            hero.BuyItem(itemID);
        }

        public static void SellItem(NetConnection connet, byte slotID)
        {
            ushort objID = clients[connet].objID;
            Hero hero = heroes[objID];

            hero.SellItem(slotID);
        }

        public static void HealHeroes()
        {
            List<Hero> healed = new List<Hero>();

            //blue
            foreach (KeyValuePair<ushort, Hero> hero in heroes)
            {
                if (!hero.Value.IsDead() && hero.Value.GetFaction() == 0)
                {
                    float distance = Vector2.Distance(new Vector2(-50.0f, -50.0f), new Vector2(hero.Value.GetXPos(), hero.Value.GetZPos()));
                    if(distance <= 5.0f)
                    {
                        healed.Add(hero.Value);
                    }
                }
            }

            //red
            foreach (KeyValuePair<ushort, Hero> hero in heroes)
            {
                if (!hero.Value.IsDead() && hero.Value.GetFaction() == 1)
                {
                    float distance = Vector2.Distance(new Vector2(50.0f, 50.0f), new Vector2(hero.Value.GetXPos(), hero.Value.GetZPos()));
                    if (distance <= 5.0f)
                    {
                        healed.Add(hero.Value);
                    }
                }
            }

            //heal
            foreach(Hero hero in healed)
            {
                hero.Heal();
            }
        }

        public static bool ApplyFireball(Fireball ability)
        {
            KeyValuePair<ushort, Hero>[] enemyheroes = heroes.Where(f => f.Value.GetFaction() != ability.GetFaction() && !f.Value.IsDead()).ToArray();
            foreach (KeyValuePair<ushort, Hero> enemy in enemyheroes)
            {
                if(ServerMath.Distance(ability, enemy.Value) <= 0)
                {
                    ability.Attack(enemy.Value, 180);
                    return true;
                }
            }

            return false;
        }

        public static void ApplyFireground(Fireground ability)
        {
            KeyValuePair<ushort, Hero>[] enemyheroes = heroes.Where(f => f.Value.GetFaction() != ability.GetFaction() && !f.Value.IsDead()).ToArray();
            foreach (KeyValuePair<ushort, Hero> enemy in enemyheroes)
            {
                if (ServerMath.Distance(ability, enemy.Value) <= 0)
                {
                    ability.Attack(enemy.Value, 5);
                }
            }

            KeyValuePair<ushort, Minion>[] enemyminions = minions.Where(f => f.Value.GetFaction() != ability.GetFaction() && !f.Value.IsDead()).ToArray();
            foreach (KeyValuePair<ushort, Minion> enemy in enemyminions)
            {
                if (ServerMath.Distance(ability, enemy.Value) <= 0)
                {
                    ability.Attack(enemy.Value, 5);
                }
            }

            foreach (KeyValuePair<ushort, Monster> enemy in monsters)
            {
                if (!enemy.Value.IsDead())
                {
                    if (ServerMath.Distance(ability, enemy.Value) <= 0)
                    {
                        ability.Attack(enemy.Value, 5);
                    }
                }
            }
        }
    }
}
