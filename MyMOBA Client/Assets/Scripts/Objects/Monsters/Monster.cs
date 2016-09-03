using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Monster : ClientSideObject
{
    //common
    const ushort maxhp = 200;

    public Monster(InitialData init, SyncData sync) : base(init, sync)
    {
        CreatePrefab();
    }

    public override void CreatePrefab()
    {
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/Monster")) as GameObject;
        prefab.transform.position = new Vector3(((MonsterSyncData)latest).xPos, 0, ((MonsterSyncData)latest).zPos);
        prefab.transform.rotation = new Quaternion(0, ((MonsterSyncData)latest).yRot, 0, ((MonsterSyncData)latest).wRot);
        prefab.GetComponent<Parent>().SetParent(this);

        prefab.layer = LayerMask.NameToLayer("Yellow");

        anime = prefab.GetComponentInChildren<Animator>();

        hpbarPrefab = GameObject.Instantiate(Resources.Load("Prefabs/HPBar"), GameObject.Find("HealthBars").transform) as GameObject;
        hpbarPrefab.GetComponent<HPBarScript>().Initialize(this, maxhp, 2.0f, 2);
    }

    public override bool IsDead()
    {
        return ((MonsterSyncData)latest).hp == 0;
    }

    public override void FixedUpdate(float delta)
    {
        if (prefab != null)
        {
            prefab.transform.rotation = new Quaternion(0, ((MonsterSyncData)latest).yRot, 0, ((MonsterSyncData)latest).wRot);

            AnimeState state = (AnimeState)((MonsterSyncData)latest).anime;
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
        anime.speed = 1.0f / 2.0f;
    }

    public override int GetCurrentHP()
    {
        return ((HPSyncData)latest).hp;
    }

    public override float GetRadius()
    {
        return 0.5f;
    }

    public override float GetXPos()
    {
        return ((MonsterSyncData)latest).xPos;
    }

    public override float GetZPos()
    {
        return ((MonsterSyncData)latest).zPos;
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
        return 2;
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
        GameObject dead = GameObject.Instantiate(Resources.Load("Prefabs/Dead/DeadMonster")) as GameObject;
        dead.transform.position = prefab.transform.position;
        dead.transform.rotation = prefab.transform.rotation;
    }
}