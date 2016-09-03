using Lidgren.Network;

public abstract class InitialData
{
    public abstract void WriteData(NetOutgoingMessage message);
}
