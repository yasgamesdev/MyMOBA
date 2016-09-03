using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Unitychan : Hero
{
    //common
    const ushort maxhp = 500;
    const ushort maxmp = 100;

    public Unitychan(InitialData init, SyncData sync, bool isPlayer) : base(init, sync, isPlayer)
    {
        CreatePrefab();
    }

    public override void CreatePrefab()
    {
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/Unitychan")) as GameObject;
        prefab.transform.position = new Vector3(((HeroSyncData)latest).xPos, 0, ((HeroSyncData)latest).zPos);
        prefab.transform.rotation = new Quaternion(0, ((HeroSyncData)latest).yRot, 0, ((HeroSyncData)latest).wRot);
        prefab.GetComponent<Parent>().SetParent(this);

        if (((HeroInitialData)init).faction == 0)
        {
            prefab.layer = LayerMask.NameToLayer("Blue");
        }
        else
        {
            prefab.layer = LayerMask.NameToLayer("Red");
        }

        anime = prefab.GetComponentInChildren<Animator>();

        AddDynamicComponets();

        hpbarPrefab = GameObject.Instantiate(Resources.Load("Prefabs/HPMPBar"), GameObject.Find("HealthBars").transform) as GameObject;
        hpbarPrefab.GetComponent<HPMPBarScript>().Initialize(this, GetMaxHP(), maxmp, GetLevel(), 2.0f, ((HeroInitialData)init).faction);
    }

    public override ushort GetMaxHP()
    {
        //common
        return (ushort)(maxhp + (int)(maxhp * 0.1f) * (((HeroSyncData)latest).level - 1) + ((HeroSyncData)latest).items.GetHP());
    }

    public override ushort GetMaxMP()
    {
        return maxmp;
    }

    public override void CreateDeadPrefab()
    {
        GameObject dead = GameObject.Instantiate(Resources.Load("Prefabs/Dead/DeadUnitychan")) as GameObject;
        dead.transform.position = prefab.transform.position;
        dead.transform.rotation = prefab.transform.rotation;
    }
}