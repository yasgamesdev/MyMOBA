using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Tower : ClientSideObject
{
    //common
    const ushort maxhp = 3000;

    public Tower(InitialData init, SyncData sync) : base(init, sync)
    {
        CreatePrefab();

        if(IsDead())
        {
            DestroyPrefab();
        }
    }

    public override bool IsDead()
    {
        return ((TowerSyncData)latest).hp == 0;
    }

    public override int GetCurrentHP()
    {
        return ((HPSyncData)latest).hp;
    }

    public override void CreatePrefab()
    {
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/Tower")) as GameObject;
        prefab.transform.position = new Vector3(((TowerInitialData)init).xPos, 0, ((TowerInitialData)init).zPos);
        prefab.GetComponent<Parent>().SetParent(this);

        GameObject top = prefab.transform.FindChild("Model/Top").gameObject;
        if (((TowerInitialData)init).faction == 0)
        {
            top.GetComponent<Renderer>().material.color = Color.cyan;
            prefab.layer = LayerMask.NameToLayer("Blue");
        }
        else
        {
            top.GetComponent<Renderer>().material.color = Color.magenta;
            prefab.layer = LayerMask.NameToLayer("Red");
        }

        hpbarPrefab = GameObject.Instantiate(Resources.Load("Prefabs/HPBar"), GameObject.Find("HealthBars").transform) as GameObject;
        hpbarPrefab.GetComponent<HPBarScript>().Initialize(this, maxhp, 3.5f, ((TowerInitialData)init).faction);
    }

    public override float GetRadius()
    {
        return 0.5f;
    }

    public override float GetXPos()
    {
        return ((TowerInitialData)init).xPos;
    }

    public override float GetZPos()
    {
        return ((TowerInitialData)init).zPos;
    }

    public override void ManageDiffrence()
    {
        if(((TowerSyncData)previous).hp > 0 && ((TowerSyncData)latest).hp == 0)
        {
            if (prefab != null)
            {
                ObjectManager.UnSetTarget(prefab.transform);
                DestroyPrefab();
            }
        }
    }

    public override void DestroyPrefab()
    {
        if (prefab != null)
        {
            GameObject.Destroy(hpbarPrefab);
            hpbarPrefab = null;
            prefab.transform.FindChild("Model/Top").gameObject.SetActive(false);
            prefab.layer = LayerMask.NameToLayer("Default");
        }
    }

    public override byte GetFaction()
    {
        return ((TowerInitialData)init).faction;
    }

    public override bool GetVision()
    {
        return ((TowerSyncData)latest).vision.visible;
    }

    public override Vector3 GetSrcBulletPos()
    {
        return new Vector3(GetXPos(), 2.5f, GetZPos());
    }
}