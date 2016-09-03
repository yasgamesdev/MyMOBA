using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Core : ClientSideObject
{
    //common
    const ushort maxhp = 5500;

    public Core(InitialData init, SyncData sync) : base(init, sync)
    {
        CreatePrefab();
    }

    public override void CreatePrefab()
    {
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/Core")) as GameObject;
        prefab.transform.position = new Vector3(((CoreInitialData)init).xPos, 0, ((CoreInitialData)init).zPos);
        prefab.GetComponent<Parent>().SetParent(this);

        GameObject top = prefab.transform.FindChild("Model/Top").gameObject;
        if (((CoreInitialData)init).faction == 0)
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
        hpbarPrefab.GetComponent<HPBarScript>().Initialize(this, maxhp, 2.0f, ((CoreInitialData)init).faction);
    }

    public override bool IsDead()
    {
        return ((CoreSyncData)latest).hp == 0;
    }

    public override int GetCurrentHP()
    {
        return ((HPSyncData)latest).hp;
    }

    public override float GetRadius()
    {
        return 1.0f;
    }

    public override float GetXPos()
    {
        return ((CoreInitialData)init).xPos;
    }

    public override float GetZPos()
    {
        return ((CoreInitialData)init).zPos;
    }

    public override void ManageDiffrence()
    {
        if (((CoreSyncData)previous).hp > 0 && ((CoreSyncData)latest).hp == 0)
        {
            if (prefab != null)
            {
                ObjectManager.UnSetTarget(prefab.transform);
                DestroyPrefab();

                if(GetFaction() == 1)
                {
                    GameObject.Find("Victory").GetComponent<Text>().text = "Blue Team Won!";
                }
                else
                {
                    GameObject.Find("Victory").GetComponent<Text>().text = "Red Team Won!";
                }
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
        return ((CoreInitialData)init).faction;
    }

    public override bool GetVision()
    {
        return ((CoreSyncData)latest).vision.visible;
    }
}