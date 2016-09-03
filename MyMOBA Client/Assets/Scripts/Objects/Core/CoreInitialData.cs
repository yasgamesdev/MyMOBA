using Lidgren.Network;

public class CoreInitialData : InitialData
{
    public byte faction { get; private set; }
    public float xPos { get; private set; }
    public float zPos { get; private set; }

    public CoreInitialData(byte faction, float xPos, float zPos)
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

    public static CoreInitialData ReadData(NetIncomingMessage message)
    {
        return new CoreInitialData(
            message.ReadByte(),
            message.ReadFloat(),
            message.ReadFloat()
            );
    }
}