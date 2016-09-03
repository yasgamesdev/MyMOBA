public abstract class HPSyncData : SyncData
{
    public ushort hp { get; private set; }

    public HPSyncData(ushort objID, ushort hp) : base(objID)
    {
        this.hp = hp;
    }

    public void DecreaseHP(int amount)
    {
        if (hp - amount < 0)
        {
            hp = 0;
        }
        else
        {
            hp -= (ushort)amount;
        }
    }

    public void SetHP(ushort hp)
    {
        this.hp = hp;
    }

    public void UpdateHP(ushort hp)
    {
        this.hp = hp;
    }
}