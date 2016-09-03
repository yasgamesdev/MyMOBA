using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;

public static class ObjectManager
{
    static Hero player = null;
    static Dictionary<ushort, Hero> heroes = new Dictionary<ushort, Hero>();
    static Dictionary<ushort, Core> cores = new Dictionary<ushort, Core>();
    static Dictionary<ushort, Tower> towers = new Dictionary<ushort, Tower>();
    static Dictionary<ushort, Monster> monsters = new Dictionary<ushort, Monster>();
    static Dictionary<ushort, Minion> minions = new Dictionary<ushort, Minion>();
    static Dictionary<ushort, Ability> abilities = new Dictionary<ushort, Ability>();

    public static void AddHero(InitialData init, SyncData sync, bool isPlayer)
    {
        Hero hero = GetHeroInstance(init, sync, isPlayer);
        heroes.Add(sync.objID, hero);
        if(isPlayer)
        {
            player = hero;
            GameObject.Find("Status").GetComponent<StatusScript>().Initialize(player);
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SetPlayerToCamera(player);
        }
    }

    public static void AddCore(InitialData init, SyncData sync)
    {
        cores.Add(sync.objID, new Core(init, sync));
    }

    public static void AddTower(InitialData init, SyncData sync)
    {
        towers.Add(sync.objID, new Tower(init, sync));
    }

    //debug
    public static void UpdateHero(InitialData init, SyncData sync)
    {
        if(heroes.ContainsKey(sync.objID))
        {
            heroes[sync.objID].ReceiveLatestData(sync);
        }
        else
        {
            heroes.Add(sync.objID, GetHeroInstance(init, sync, false));
        }
    }

    public static void UpdateCore(SyncData sync)
    {
        cores[sync.objID].ReceiveLatestData(sync);
    }

    public static void UpdateTower(SyncData sync)
    {
        towers[sync.objID].ReceiveLatestData(sync);
    }

    public static void UpdateMonsters(List<SyncData> syncs)
    {
        foreach (KeyValuePair<ushort, Monster> monster in monsters)
        {
            monster.Value.ResetUpdateFlag();
        }

        foreach (SyncData sync in syncs)
        {
            if(monsters.ContainsKey(sync.objID))
            {
                monsters[sync.objID].ReceiveLatestData(sync);
            }
            else
            {
                monsters.Add(sync.objID, new Monster(null, sync));
                monsters[sync.objID].ReceiveLatestData(sync);
            }
        }

        var removes = monsters.Where(f => f.Value.updated == false).ToArray();
        foreach(var remove in removes)
        {
            if(remove.Value.IsVisible())
            {
                remove.Value.CreateDeadPrefab();
            }
            UnSetTarget(remove.Value.GetPrefabTransform());
            remove.Value.DestroyPrefab();
            monsters.Remove(remove.Key);
        }
    }

    public static void UpdateMinions(List<SyncData> syncs)
    {
        foreach (KeyValuePair<ushort, Minion> minion in minions)
        {
            minion.Value.ResetUpdateFlag();
        }

        foreach (SyncData sync in syncs)
        {
            if (minions.ContainsKey(sync.objID))
            {
                minions[sync.objID].ReceiveLatestData(sync);
            }
            else
            {
                minions.Add(sync.objID, new Minion(null, sync));
                minions[sync.objID].ReceiveLatestData(sync);
            }
        }

        var removes = minions.Where(f => f.Value.updated == false).ToArray();
        foreach (var remove in removes)
        {
            if (remove.Value.IsVisible())
            {
                remove.Value.CreateDeadPrefab();
            }
            UnSetTarget(remove.Value.GetPrefabTransform());
            remove.Value.DestroyPrefab();
            minions.Remove(remove.Key);
        }
    }

    public static void UpdateAbilities(List<SyncData> syncs)
    {
        foreach (KeyValuePair<ushort, Ability> ability in abilities)
        {
            ability.Value.ResetUpdateFlag();
        }

        foreach (SyncData sync in syncs)
        {
            if (abilities.ContainsKey(sync.objID))
            {
                abilities[sync.objID].ReceiveLatestData(sync);
            }
            else
            {
                if(((AbilitySyncData)sync).abilityID == 0)
                {
                    abilities.Add(sync.objID, new Fireball(null, sync));
                    abilities[sync.objID].ReceiveLatestData(sync);
                }
                else if (((AbilitySyncData)sync).abilityID == 1)
                {
                    abilities.Add(sync.objID, new Fireground(null, sync));
                    abilities[sync.objID].ReceiveLatestData(sync);
                }
                //abilities.Add(sync.objID, new Ability(null, sync));
                //abilities[sync.objID].ReceiveLatestData(sync);
            }
        }

        var removes = abilities.Where(f => f.Value.updated == false).ToArray();
        foreach (var remove in removes)
        {
            UnSetTarget(remove.Value.GetPrefabTransform());
            remove.Value.DestroyPrefab();
            abilities.Remove(remove.Key);
        }
    }

    static Hero GetHeroInstance(InitialData init, SyncData sync, bool isPlayer)
    {
        if(((HeroInitialData)init).heroID == 0)
        {
            return new Unitychan(init, sync, isPlayer);
        }
        else
        {
            //debug
            return new Unitychan(init, sync, isPlayer);
        }
    }

    public static void FixedUpdate(float delta)
    {
        foreach (KeyValuePair<ushort, Hero> hero in heroes)
        {
            hero.Value.FixedUpdate(delta);
        }
        foreach (KeyValuePair<ushort, Core> core in cores)
        {
            core.Value.FixedUpdate(delta);
        }
        foreach (KeyValuePair<ushort, Tower> tower in towers)
        {
            tower.Value.FixedUpdate(delta);
        }
        foreach (KeyValuePair<ushort, Monster> monster in monsters)
        {
            monster.Value.FixedUpdate(delta);
        }
        foreach (KeyValuePair<ushort, Minion> minion in minions)
        {
            minion.Value.FixedUpdate(delta);
        }
        foreach (KeyValuePair<ushort, Ability> ability in abilities)
        {
            ability.Value.FixedUpdate(delta);
        }
    }

    public static void PushClientData(NetClient client)
    {
        player.PushClientData(client);
    }

    public static void UnSetTarget(Transform target)
    {
        if(!player.IsDead())
        {
            player.UnSetTarget(target);
        }
    }

    public static void UpdateVision()
    {
        if(player == null)
        {
            return;
        }
        byte faction = player.GetFaction();
        float heroVisionRange = 10.0f;
        float minionVisionRange = 10.0f;
        float towerVisionRange = 10.0f;

        KeyValuePair<ushort, Hero>[] allyheroes = heroes.Where(f => f.Value.GetFaction() == faction && !f.Value.IsDead()).ToArray();
        KeyValuePair<ushort, Hero>[] enemyheroes = heroes.Where(f => f.Value.GetFaction() != faction && !f.Value.IsDead()).ToArray();
        KeyValuePair<ushort, Minion>[] allyminions = minions.Where(f => f.Value.GetFaction() == faction).ToArray();
        KeyValuePair<ushort, Minion>[] enemyminions = minions.Where(f => f.Value.GetFaction() != faction).ToArray();
        KeyValuePair<ushort, Ability>[] enemyabilities = abilities.Where(f => f.Value.GetFaction() != faction).ToArray();
        KeyValuePair<ushort, Tower>[] allytowers = towers.Where(f => f.Value.GetFaction() == faction && !f.Value.IsDead()).ToArray();

        List<ClientSideObject> visible = new List<ClientSideObject>();
        List<ClientSideObject> invisible = new List<ClientSideObject>();

        foreach(KeyValuePair<ushort, Hero> enemy in enemyheroes)
        {
            if (enemy.Value.GetVision())
            {
                visible.Add(enemy.Value);
            }
            else
            {
                invisible.Add(enemy.Value);
            }
        }
        foreach (KeyValuePair<ushort, Minion> enemy in enemyminions)
        {
            if (enemy.Value.GetVision())
            {
                visible.Add(enemy.Value);
            }
            else
            {
                invisible.Add(enemy.Value);
            }
        }
        foreach (KeyValuePair<ushort, Ability> enemy in enemyabilities)
        {
            if (enemy.Value.GetVision())
            {
                visible.Add(enemy.Value);
            }
            else
            {
                invisible.Add(enemy.Value);
            }
        }
        foreach (KeyValuePair<ushort, Monster> monster in monsters)
        {
            invisible.Add(monster.Value);
        }

        //heroes vision
        foreach (KeyValuePair<ushort, Hero> ally in allyheroes)
        {
            for(int i= invisible.Count -1; i>=0; i--)
            {
                if(ally.Value.Distance(invisible[i]) <= heroVisionRange && !ally.Value.WallStand(invisible[i]))
                {
                    visible.Add(invisible[i]);
                    invisible.RemoveAt(i);
                }
            }
        }
        //minions vision
        foreach (KeyValuePair<ushort, Minion> ally in allyminions)
        {
            for (int i = invisible.Count - 1; i >= 0; i--)
            {
                if (ally.Value.Distance(invisible[i]) <= minionVisionRange && !ally.Value.WallStand(invisible[i]))
                {
                    visible.Add(invisible[i]);
                    invisible.RemoveAt(i);
                }
            }
        }
        //towers vision
        foreach (KeyValuePair<ushort, Tower> ally in allytowers)
        {
            for (int i = invisible.Count - 1; i >= 0; i--)
            {
                if (ally.Value.Distance(invisible[i]) <= towerVisionRange && !ally.Value.WallStand(invisible[i]))
                {
                    visible.Add(invisible[i]);
                    invisible.RemoveAt(i);
                }
            }
        }

        foreach (ClientSideObject v in visible)
        {
            v.SetToVisible();
        }

        foreach (ClientSideObject v in invisible)
        {
            v.SetToInVisible();
        }
    }

    public static void AddBullet(Bullet bullet)
    {
        ClientSideObject src = GetObject(bullet.srcObjID);
        ClientSideObject dest = GetObject(bullet.destObjID);

        if(src != null && dest != null && src.IsVisible() && dest.IsVisible())
        {
            GameObject prefab = GameObject.Instantiate(Resources.Load("Prefabs/Bullet")) as GameObject;
            prefab.GetComponent<BulletScript>().Initialize(src.GetSrcBulletPos(), dest.GetDestBulletPos());
        }
    }

    public static ClientSideObject GetObject(ushort objID)
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

    public static byte GetPlayerFaction()
    {
        return player.GetFaction();
    }
}

