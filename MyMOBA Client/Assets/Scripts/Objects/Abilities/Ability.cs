using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Ability : ClientSideObject
{
    public Ability(InitialData init, SyncData sync) : base(init, sync)
    {
    }

    public override void FixedUpdate(float delta)
    {
        if (prefab != null)
        {
            Vector3 latestPos = new Vector3(((AbilitySyncData)latest).xPos, prefab.transform.position.y, ((AbilitySyncData)latest).zPos);
            prefab.transform.position = Vector3.Lerp(prefab.transform.position, latestPos, delta * 10.0f);
            prefab.transform.rotation = new Quaternion(0, ((AbilitySyncData)latest).yRot, 0, ((AbilitySyncData)latest).wRot);
        }
    }

    public override float GetXPos()
    {
        return ((AbilitySyncData)latest).xPos;
    }

    public override float GetZPos()
    {
        return ((AbilitySyncData)latest).zPos;
    }

    public override byte GetFaction()
    {
        return ((AbilitySyncData)latest).faction;
    }

    public override void DestroyPrefab()
    {
        if (prefab != null)
        {
            GameObject.Destroy(prefab);
            prefab = null;
        }
    }

    public override bool GetVision()
    {
        return ((AbilitySyncData)latest).vision.visible;
    }

    public override void ShowModel()
    {
        CreatePrefab();
    }

    public override void HideModel()
    {
        DestroyPrefab();
    }
}