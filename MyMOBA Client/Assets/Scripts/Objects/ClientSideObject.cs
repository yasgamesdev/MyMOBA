using UnityEngine;
using System.Collections;

public abstract class ClientSideObject {
    protected InitialData init = null;
    protected SyncData previous, latest;
    public bool updated { get; private set; }
    protected GameObject prefab = null;
    protected GameObject hpbarPrefab = null;
    protected Animator anime = null;
    int wallLayerMask;

    public ClientSideObject(InitialData init, SyncData sync)
    {
        this.init = init;
        this.latest = sync;
        this.previous = sync;

        wallLayerMask = LayerMask.GetMask("Wall");
    }

    public void ReceiveLatestData(SyncData sync)
    {
        this.previous = this.latest;
        this.latest = sync;

        updated = true;

        ManageDiffrence();
    }

    public virtual void ManageDiffrence() { }

    public void ResetUpdateFlag()
    {
        updated = false;
    }

    public virtual bool IsDead()
    {
        return false;
    }

    public virtual void FixedUpdate(float delta) { }

    public virtual int GetCurrentHP()
    {
        return 0;
    }

    public Transform GetPrefabTransform()
    {
        if (prefab != null)
        {
            return prefab.transform;
        }
        else
        {
            return null;
        }
    }

    public abstract void CreatePrefab();

    public ushort GetObjID()
    {
        return latest.objID;
    }

    public abstract float GetRadius();
    public abstract float GetXPos();
    public abstract float GetZPos();

    public float Distance(ClientSideObject target)
    {
        return Vector2.Distance(new Vector2(target.GetXPos(), target.GetZPos()), new Vector2(this.GetXPos(), this.GetZPos()))
            - target.GetRadius() - this.GetRadius();        
    }

    public bool WallStand(ClientSideObject target)
    {
        Vector3 origin = new Vector3(GetXPos(), 0.5f, GetZPos());
        Vector3 goal = new Vector3(target.GetXPos(), 0.5f, target.GetZPos());
        Ray ray = new Ray(origin, goal - origin);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit, Vector3.Distance(goal, origin), wallLayerMask);
    }

    public virtual void DestroyPrefab() { }

    public void SetToVisible()
    {
        if(prefab == null)
        {
            ShowModel();
        }
    }

    public void SetToInVisible()
    {
        if(prefab != null)
        {
            ObjectManager.UnSetTarget(GetPrefabTransform());
            HideModel();
        }
    }

    public virtual void ShowModel() { }

    public virtual void HideModel() { }

    public abstract byte GetFaction();

    public virtual bool GetVision()
    {
        return false;
    }

    public bool IsVisible()
    {
        return prefab != null;
    }

    public virtual Vector3 GetSrcBulletPos()
    {
        return new Vector3(GetXPos(), 1.0f, GetZPos());
    }

    public virtual Vector3 GetDestBulletPos()
    {
        return new Vector3(GetXPos(), 1.0f, GetZPos());
    }

    public virtual void CreateDeadPrefab() { }
}
