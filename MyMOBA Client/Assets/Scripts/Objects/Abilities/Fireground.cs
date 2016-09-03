using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Fireground : Ability
{
    public Fireground(InitialData init, SyncData sync) : base(init, sync)
    {
        CreatePrefab();
    }

    public override void CreatePrefab()
    {
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/Fireground")) as GameObject;
        prefab.transform.position = new Vector3(((AbilitySyncData)latest).xPos, 0, ((AbilitySyncData)latest).zPos);
        prefab.transform.rotation = new Quaternion(0, ((AbilitySyncData)latest).yRot, 0, ((AbilitySyncData)latest).wRot);
        prefab.GetComponent<Parent>().SetParent(this);
    }

    public override float GetRadius()
    {
        return 2.0f;
    }
}