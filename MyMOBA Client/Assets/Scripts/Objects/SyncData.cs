using Lidgren.Network;

public abstract class SyncData
{
    public ushort objID { get; private set; }

    public SyncData(ushort objID)
    {
        this.objID = objID;
    }

    public abstract void WriteData(NetOutgoingMessage message);
}