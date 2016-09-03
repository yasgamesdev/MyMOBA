using Lidgren.Network;

class TowerInitialData : InitialData
{
    public byte faction { get; private set; }
    public float xPos { get; private set; }
    public float zPos { get; private set; }

    public TowerInitialData(byte faction, float xPos, float zPos)
    {
        this.faction = faction;
        this.xPos = xPos;
        this.zPos = zPos;
    }

    public override void WriteData(NetOutgoingMessage message)
    {
        message.Write(faction);
        message.Write(xPos);
        message.Write(zPos);
    }

    public static TowerInitialData ReadData(NetIncomingMessage message)
    {
        return new TowerInitialData(
            message.ReadByte(),
            message.ReadFloat(),
            message.ReadFloat()
            );
    }
}