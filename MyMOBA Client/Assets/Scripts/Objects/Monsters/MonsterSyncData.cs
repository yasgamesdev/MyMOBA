using Lidgren.Network;

class MonsterSyncData : HPSyncData
{
    public float xPos { get; private set; }
    public float zPos { get; private set; }
    public float yRot { get; private set; }
    public float wRot { get; private set; }
    public byte anime { get; private set; }
    public BuffDebuff buff { get; private set; }

    public MonsterSyncData(ushort objID, ushort hp, float xPos, float zPos, float yRot, float wRot, byte anime, BuffDebuff buff) : base(objID, hp)
    {
        this.xPos = xPos;
        this.zPos = zPos;
        this.yRot = yRot;
        this.wRot = wRot;
        this.anime = anime;
        this.buff = buff;
    }

    public void Update(float delta)
    {
        buff.Update(delta);
    }

    public override void WriteData(NetOutgoingMessage message)
    {
        message.Write(objID);
        message.Write(hp);
        message.Write(xPos);
        message.Write(zPos);
        message.Write(yRot);
        message.Write(wRot);
        message.Write(anime);
        buff.WriteData(message);
    }

    public static MonsterSyncData ReadData(NetIncomingMessage message)
    {
        return new MonsterSyncData(
            message.ReadUInt16(),
            message.ReadUInt16(),
            message.ReadFloat(),
            message.ReadFloat(),
            message.ReadFloat(),
            message.ReadFloat(),
            message.ReadByte(),
            BuffDebuff.ReadData(message)
            );
    }

    public void Revive(ushort hp, float yRot, float wRot)
    {
        SetHP(hp);
        this.yRot = yRot;
        this.wRot = wRot;
    }
}