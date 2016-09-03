using Lidgren.Network;

class HeroInitialData : InitialData
{
    public byte faction { get; private set; }
    public byte heroID { get; private set; }

    public HeroInitialData(byte faction, byte heroID)
    {
        this.faction = faction;
        this.heroID = heroID;
    }

    public override void WriteData(NetOutgoingMessage message)
    {
        message.Write(faction);
        message.Write(heroID);
    }

    public static HeroInitialData ReadData(NetIncomingMessage message)
    {
        return new HeroInitialData(
            message.ReadByte(),
            message.ReadByte()
            );
    }
}