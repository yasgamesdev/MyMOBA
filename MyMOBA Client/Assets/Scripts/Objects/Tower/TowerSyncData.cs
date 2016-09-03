using Lidgren.Network;

class TowerSyncData : HPSyncData
{
    public BuffDebuff buff { get; private set; }
    public EnemyVision vision { get; private set; }

    public TowerSyncData(ushort objID, ushort hp, BuffDebuff buff, EnemyVision vision) : base(objID, hp)
    {
        this.buff = buff;
        this.vision = vision;
    }

    public void Update(float delta)
    {
        buff.Update(delta);
        vision.Update(delta);
    }

    public override void WriteData(NetOutgoingMessage message)
    {
        message.Write(objID);
        message.Write(hp);
        buff.WriteData(message);
        vision.WriteData(message);
    }

    public static TowerSyncData ReadData(NetIncomingMessage message)
    {
        return new TowerSyncData(
            message.ReadUInt16(),
            message.ReadUInt16(),
            BuffDebuff.ReadData(message),
            EnemyVision.ReadData(message)
            );
    }
}