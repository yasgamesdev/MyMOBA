using Lidgren.Network;

public class BuffDebuff
{
    public int state { get; private set; }
    public BuffDebuff(int state)
    {
        this.state = state;
    }

    public void Update(float delta)
    {

    }

    public void WriteData(NetOutgoingMessage message)
    {
        message.Write(state);
    }

    public static BuffDebuff ReadData(NetIncomingMessage message)
    {
        return new BuffDebuff(message.ReadInt32());
    }
}