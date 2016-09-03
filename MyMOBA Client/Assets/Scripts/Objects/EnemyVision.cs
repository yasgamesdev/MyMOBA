using Lidgren.Network;

public class EnemyVision
{
    public bool visible { get; private set; }
    const float disappearTime = 3.0f;
    float attackTimer = 0;
    float attackedTimer = 0;

    public EnemyVision(bool visible)
    {
        this.visible = visible;
    }

    public void Update(float delta)
    {
        if (attackTimer > 0)
        {
            attackTimer -= delta;
        }
        if (attackedTimer > 0)
        {
            attackedTimer -= delta;
        }
        if (attackTimer <= 0 && attackedTimer <= 0)
        {
            visible = false;
        }
        else
        {
            visible = true;
        }
    }

    public void Attack()
    {
        attackTimer = disappearTime;
        visible = true;
    }

    public void Attacked()
    {
        attackedTimer = disappearTime;
        visible = true;
    }

    public void WriteData(NetOutgoingMessage message)
    {
        message.Write(visible);
    }

    public static EnemyVision ReadData(NetIncomingMessage message)
    {
        return new EnemyVision(message.ReadBoolean());
    }
}