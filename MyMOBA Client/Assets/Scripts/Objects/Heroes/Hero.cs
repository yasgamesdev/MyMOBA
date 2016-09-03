using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Hero : ClientSideObject
{
    public bool isPlayer { get; private set; }

    public Hero(InitialData init, SyncData sync, bool isPlayer) : base(init, sync)
    {
        this.isPlayer = isPlayer;
    }

    public override bool IsDead()
    {
        return ((HeroSyncData)latest).hp == 0;
    }

    public override void FixedUpdate(float delta)
    {
        if (prefab != null && !isPlayer)
        {
            Vector3 latestPos = new Vector3(((HeroSyncData)latest).xPos, prefab.transform.position.y, ((HeroSyncData)latest).zPos);
            prefab.transform.position = Vector3.Lerp(prefab.transform.position, latestPos, delta * 10.0f);
            prefab.transform.rotation = new Quaternion(0, ((HeroSyncData)latest).yRot, 0, ((HeroSyncData)latest).wRot);

            AnimeState state = (AnimeState)((HeroSyncData)latest).anime;
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
        anime.speed = 1.0f / GetAttackRate();
    }

    protected void AddDynamicComponets()
    {
        if (isPlayer)
        {
            prefab.AddComponent<PlayerControl>().SetParent(this);
        }
        else
        {
            NavMeshObstacle obstacle = prefab.AddComponent<NavMeshObstacle>();
            obstacle.shape = NavMeshObstacleShape.Capsule;
            obstacle.radius = 0.3f;
            obstacle.height = 1.6f;
            obstacle.center = new Vector3(0, 0.8f, 0);
        }
    }

    public int GetEnemyLayerMask()
    {
        if(((HeroInitialData)init).faction == 0)
        {
            return LayerMask.GetMask("Red", "Yellow");
        }
        else
        {
            return LayerMask.GetMask("Blue", "Yellow");
        }
    }

    public void PushClientData(NetClient client)
    {
        if(!IsDead())
        {
            NetOutgoingMessage message = client.CreateMessage();
            message.Write((byte)DataType.PushClientData);
            message.Write(prefab.transform.position.x);
            message.Write(prefab.transform.position.z);
            message.Write(prefab.transform.rotation.y);
            message.Write(prefab.transform.rotation.w);
            message.Write(prefab.GetComponent<PlayerControl>().anime);
            client.SendMessage(message, NetDeliveryMethod.UnreliableSequenced);
        }
    }

    public override int GetCurrentHP()
    {
        return ((HPSyncData)latest).hp;
    }

    public int GetCurrentMP()
    {
        return ((HeroSyncData)latest).mp;
    }

    public override float GetRadius()
    {
        return 0.3f;
    }

    public override float GetXPos()
    {
        return ((HeroSyncData)latest).xPos;
    }

    public override float GetZPos()
    {
        return ((HeroSyncData)latest).zPos;
    }

    public virtual float GetAttackRange()
    {
        return 5.0f;
    }

    public virtual float GetAttackRate()
    {
        return 1.0f;
    }

    public void UnSetTarget(Transform target)
    {
        prefab.GetComponent<PlayerControl>().UnSetTarget(target);
    }

    public override void ManageDiffrence()
    {
        //cancel recall
        if(isPlayer && ((HeroSyncData)previous).hp > ((HeroSyncData)latest).hp)
        {
            prefab.GetComponent<PlayerControl>().CancelRecall();
        }

        if (((HeroSyncData)previous).hp > 0 && ((HeroSyncData)latest).hp == 0)
        {
            if (prefab != null)
            {
                CreateDeadPrefab();

                ObjectManager.UnSetTarget(prefab.transform);
                DestroyPrefab();                
            }

            if(isPlayer)
            {
                GameObject.Find("UICanvas").GetComponent<UIScript>().UnSetPlayer();
            }
        }
        else if(((HeroSyncData)previous).hp == 0 && ((HeroSyncData)latest).hp > 0)
        {
            CreatePrefab();
        }

        if (((HeroSyncData)previous).level != ((HeroSyncData)latest).level)
        {
            hpbarPrefab.GetComponent<HPMPBarScript>().SetMaxHP(GetMaxHP());
            hpbarPrefab.GetComponent<HPMPBarScript>().SetLevel(GetLevel());

            if(isPlayer)
            {
                GameObject.Find("Status").GetComponent<StatusScript>().SetMaxHP(GetMaxHP());
            }
        }

        if (!((HeroSyncData)previous).items.Equals(((HeroSyncData)latest).items))
        {
            hpbarPrefab.GetComponent<HPMPBarScript>().SetMaxHP(GetMaxHP());
            if (isPlayer)
            {
                GameObject.Find("Status").GetComponent<StatusScript>().SetMaxHP(GetMaxHP());
                GameObject.Find("Status").GetComponent<StatusScript>().SetItemIcons(((HeroSyncData)latest).items);
            }
        }
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

    public abstract ushort GetMaxHP();
    public abstract ushort GetMaxMP();

    public override byte GetFaction()
    {
        return ((HeroInitialData)init).faction;
    }

    public override void ShowModel()
    {
        CreatePrefab();
    }

    public override void HideModel()
    {
        DestroyPrefab();
    }

    public override bool GetVision()
    {
        return ((HeroSyncData)latest).vision.visible;
    }

    public byte GetLevel()
    {
        return ((HeroSyncData)latest).level;
    }

    public ushort GetGold()
    {
        return ((HeroSyncData)latest).gold;
    }

    public ushort GetMP()
    {
        return ((HeroSyncData)latest).mp;
    }
}