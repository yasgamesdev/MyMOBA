using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Minion : ClientSideObject
{
    //common
    const ushort maxhp = 100;

    public Minion(InitialData init, SyncData sync) : base(init, sync)
    {
        CreatePrefab();
    }

    public override void CreatePrefab()
    {
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/Minion")) as GameObject;
        prefab.transform.position = new Vector3(((MinionSyncData)latest).xPos, 0, ((MinionSyncData)latest).zPos);
        prefab.transform.rotation = new Quaternion(0, ((MinionSyncData)latest).yRot, 0, ((MinionSyncData)latest).wRot);
        prefab.GetComponent<Parent>().SetParent(this);

        if (((MinionSyncData)latest).faction == 0)
        {
            prefab.layer = LayerMask.NameToLayer("Blue");
        }
        else
        {
            prefab.layer = LayerMask.NameToLayer("Red");
        }

        anime = prefab.GetComponentInChildren<Animator>();

        hpbarPrefab = GameObject.Instantiate(Resources.Load("Prefabs/HPBar"), GameObject.Find("HealthBars").transform) as GameObject;
        hpbarPrefab.GetComponent<HPBarScript>().Initialize(this, maxhp, 1.6f, ((MinionSyncData)latest).faction);
    }

    public override bool IsDead()
    {
        return ((MinionSyncData)latest).hp == 0;
    }

    public override void FixedUpdate(float delta)
    {
        if (prefab != null)
        {
            Vector3 latestPos = new Vector3(((MinionSyncData)latest).xPos, prefab.transform.position.y, ((MinionSyncData)latest).zPos);
            prefab.transform.position = Vector3.Lerp(prefab.transform.position, latestPos, delta * 10.0f);
            prefab.transform.rotation = new Quaternion(0, ((MinionSyncData)latest).yRot, 0, ((MinionSyncData)latest).wRot);

            AnimeState state = (AnimeState)((MinionSyncData)latest).anime;
            if (state == AnimeState.Idle)
            {
                SetIdleAnime();
            }
            else if (state == AnimeState.Walk)
            {
                SetWalkAnime();
            }
            else if (state == AnimeState.Attack)
            {
                SetAttackAnime();
            }
        }
    }

    public void SetIdleAnime()
    {
        anime.speed = 1.0f;
        anime.SetBool("MeleeAttack", false);
        anime.SetFloat("Speed", 0);
    }

    public void SetWalkAnime()
    {
        anime.speed = 1.0f;
        anime.SetBool("MeleeAttack", false);
        anime.SetFloat("Speed", 1.0f);
    }

    public void SetAttackAnime()
    {
        anime.SetFloat("Speed", 0);
        anime.SetBool("MeleeAttack", true);
        anime.speed = 1.0f;
    }

    public override int GetCurrentHP()
    {
        return ((HPSyncData)latest).hp;
    }

    public override float GetRadius()
    {
        return 0.3f;
    }

    public override float GetXPos()
    {
        return ((MinionSyncData)latest).xPos;
    }

    public override float GetZPos()
    {
        return ((MinionSyncData)latest).zPos;
    }

    public override void DestroyPrefab()
    {
        if (prefab != null)
        {
            GameObject.Destroy(hpbarPrefab);
            hpbarPrefab = null;
            GameObject.Destroy(prefab);
            prefab = null;
        }
    }

    public override byte GetFaction()
    {
        return ((MinionSyncData)latest).faction;
    }

    public override bool GetVision()
    {
        return ((MinionSyncData)latest).vision.visible;
    }

    public override void ShowModel()
    {
        CreatePrefab();
    }

    public override void HideModel()
    {
        DestroyPrefab();
    }

    public override void CreateDeadPrefab()
    {
        GameObject dead = GameObject.Instantiate(Resources.Load("Prefabs/Dead/DeadMinion")) as GameObject;
        dead.transform.position = prefab.transform.position;
        dead.transform.rotation = prefab.transform.rotation;
    }
}