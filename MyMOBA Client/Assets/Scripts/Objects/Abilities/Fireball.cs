using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Fireball : Ability
{
    public Fireball(InitialData init, SyncData sync) : base(init, sync)
    {
        CreatePrefab();
    }

    public override void CreatePrefab()
    {
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/Fireball")) as GameObject;
        prefab.transform.position = new Vector3(((AbilitySyncData)latest).xPos, 1.0f, ((AbilitySyncData)latest).zPos);
        prefab.transform.rotation = new Quaternion(0, ((AbilitySyncData)latest).yRot, 0, ((AbilitySyncData)latest).wRot);
        prefab.GetComponent<Parent>().SetParent(this);
    }

    public override float GetRadius()
    {
        return 0.25f;
    }
}