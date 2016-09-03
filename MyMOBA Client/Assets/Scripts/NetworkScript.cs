using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lidgren.Network;

public class NetworkScript : MonoBehaviour {
    NetClient client;
    float frameSpan;
    bool initialized = false;

    void Start () {
        NetPeerConfiguration config = new NetPeerConfiguration("MyMOBA0.1");
        client = new NetClient(config);
        client.Start();
        //client.Connect(host: "127.0.0.1", port: 15123);
        client.Connect(ConnectConfig.host, ConnectConfig.port);

        frameSpan = 0;
    }

    void Update()
    {
        ObjectManager.UpdateVision();

        frameSpan += Time.deltaTime;
        if (frameSpan >= 0.1f)
        {
            frameSpan = 0;

            ReceiveMessage(client);
            SendMessage(client);
        }
    }

    void FixedUpdate()
    {
        ObjectManager.FixedUpdate(Time.deltaTime);
    }

    void OnDestroy()
    {
        if (client != null)
        {
            client.Shutdown("bye");
        }
    }

    void ReceiveMessage(NetClient client)
    {
        NetIncomingMessage message;
        while ((message = client.ReadMessage()) != null)
        {
            switch (message.MessageType)
            {
                case NetIncomingMessageType.Data:
                    ReceiveServerData(message);
                    break;
                case NetIncomingMessageType.StatusChanged:
                    if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                    {
                    }
                    else if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                    {
                    }
                    break;
                case NetIncomingMessageType.DebugMessage:
                    break;
                default:
                    break;
            }
        }
    }

    void SendMessage(NetClient client)
    {
        if (initialized)
        {
            ObjectManager.PushClientData(client);
        }
    }

    void ReceiveServerData(NetIncomingMessage message)
    {
        DataType type = (DataType)message.ReadByte();
        if(type == DataType.Initialize)
        {
            HeroInitialData init = HeroInitialData.ReadData(message);
            HeroSyncData sync = HeroSyncData.ReadData(message);
            ObjectManager.AddHero(init, sync, true);

            ReceiveInitialData(message);

            initialized = true;
        }
        else if(type == DataType.Snapshot)
        {
            if(initialized)
            {
                ReadSnapshot(message);
            }            
        }
    }

    void ReceiveInitialData(NetIncomingMessage message)
    {
        byte count = message.ReadByte();
        for(int i=0; i<count; i++)
        {
            HeroInitialData init = HeroInitialData.ReadData(message);
            HeroSyncData sync = HeroSyncData.ReadData(message);
            ObjectManager.AddHero(init, sync, false);
        }

        count = message.ReadByte();
        for (int i = 0; i < count; i++)
        {
            CoreInitialData init = CoreInitialData.ReadData(message);
            CoreSyncData sync = CoreSyncData.ReadData(message);
            ObjectManager.AddCore(init, sync);
        }

        count = message.ReadByte();
        for (int i = 0; i < count; i++)
        {
            TowerInitialData init = TowerInitialData.ReadData(message);
            TowerSyncData sync = TowerSyncData.ReadData(message);
            ObjectManager.AddTower(init, sync);
        }
    }

    void ReadSnapshot(NetIncomingMessage message)
    {
        byte count = message.ReadByte();
        for (int i = 0; i < count; i++)
        {
            HeroInitialData init = HeroInitialData.ReadData(message);
            HeroSyncData sync = HeroSyncData.ReadData(message);
            ObjectManager.UpdateHero(init, sync);
        }

        count = message.ReadByte();
        for (int i = 0; i < count; i++)
        {
            CoreSyncData sync = CoreSyncData.ReadData(message);
            ObjectManager.UpdateCore(sync);
        }

        count = message.ReadByte();
        for (int i = 0; i < count; i++)
        {
            TowerSyncData sync = TowerSyncData.ReadData(message);
            ObjectManager.UpdateTower(sync);
        }

        count = message.ReadByte();
        List<SyncData> monsters = new List<SyncData>();
        for (int i = 0; i < count; i++)
        {
            monsters.Add(MonsterSyncData.ReadData(message));
        }
        ObjectManager.UpdateMonsters(monsters);

        count = message.ReadByte();
        List<SyncData> minions = new List<SyncData>();
        for (int i = 0; i < count; i++)
        {
            minions.Add(MinionSyncData.ReadData(message));
        }
        ObjectManager.UpdateMinions(minions);

        count = message.ReadByte();
        List<SyncData> abilities = new List<SyncData>();
        for (int i = 0; i < count; i++)
        {
            abilities.Add(AbilitySyncData.ReadData(message));
        }
        ObjectManager.UpdateAbilities(abilities);

        count = message.ReadByte();
        for (int i = 0; i < count; i++)
        {
            Bullet bullet = Bullet.ReadData(message);
            ObjectManager.AddBullet(bullet);
        }
    }

    public void SendAutoAttack(ushort tarObjID)
    {
        NetOutgoingMessage message = client.CreateMessage();
        message.Write((byte)DataType.AutoAttack);
        message.Write(tarObjID);
        client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public void SendBuyItem(byte itemID)
    {
        NetOutgoingMessage message = client.CreateMessage();
        message.Write((byte)DataType.BuyItem);
        message.Write(itemID);
        client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public void SendSellItem(byte slotID)
    {
        NetOutgoingMessage message = client.CreateMessage();
        message.Write((byte)DataType.SellItem);
        message.Write(slotID);
        client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public void SendFireball(Vector2 pos, Vector2 dir)
    {
        NetOutgoingMessage message = client.CreateMessage();
        message.Write((byte)DataType.Fireball);
        message.Write(pos.x);
        message.Write(pos.y);
        message.Write(dir.x);
        message.Write(dir.y);
        client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    public void SendFireground(Vector2 pos)
    {
        NetOutgoingMessage message = client.CreateMessage();
        message.Write((byte)DataType.Fireground);
        message.Write(pos.x);
        message.Write(pos.y);
        client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }
}
