using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Lidgren.Network;
using MyMOBA_Server.Objects.Heroes;
using MyMOBA_Server.Objects.Tower;
using MyMOBA_Server.Objects.Core;
using MyMOBA_Server.Objects.Monsters;
using MyMOBA_Server.Objects.Minion;
using SlimMath;

namespace MyMOBA_Server
{
    class Program
    {
        const int FRAMERATE = 100;
        static void Main(string[] args)
        {
            Console.WriteLine("Please Type UDP Port Number...(Default 15123)");
            int port = int.Parse(Console.ReadLine());
                        
            NetPeerConfiguration config = new NetPeerConfiguration("MyMOBA0.1");
            config.Port = port;
            NetServer server = new NetServer(config);
            server.Start();

            LoadMap();

            while (true)
            {
                DateTime startTime = DateTime.Now;

                ReceiveMessage(server);

                Update(10.0f / FRAMERATE);

                SendMessage(server);

                TimeSpan frameSpan = DateTime.Now - startTime;
                if ((int)frameSpan.TotalMilliseconds < FRAMERATE)
                {
                    Thread.Sleep(FRAMERATE - (int)frameSpan.TotalMilliseconds);
                }
            }
        }

        static void ReceiveMessage(NetServer server)
        {
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        ReceiveClientData(message);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Console.WriteLine("NewConnect:" + message.SenderConnection.ToString());
                            Client client = new Client(message.SenderConnection);
                            Hero hero = new Unitychan(0, 0, 0, 0, 0);
                            client.objID = hero.GetObjID();

                            InitializeClient(server, client, hero);

                            ObjectManager.AddClient(client);
                            ObjectManager.AddHero(hero);
                        }
                        else if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                        {
                            Console.WriteLine("Disconnect:" + message.SenderConnection.ToString());
                        }
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine("DebugMessage:" + message.ReadString());
                        break;
                    default:
                        Console.WriteLine("UnknownMessage:" + message.MessageType);
                        break;
                }
            }
        }

        static void Update(float delta)
        {
            MinionGenerator.Update(delta);

            ObjectManager.HealHeroes();

            ObjectManager.Update(delta);

            ObjectManager.DeleteDeadMinion();
            ObjectManager.DeleteExpiredAbility();
        }

        static void SendMessage(NetServer server)
        {
            NetOutgoingMessage message = server.CreateMessage();
            message.Write((byte)DataType.Snapshot);

            ObjectManager.WriteSnapshot(message);

            server.SendToAll(message, NetDeliveryMethod.UnreliableSequenced);
        }

        static void ReceiveClientData(NetIncomingMessage message)
        {
            DataType type = (DataType)message.ReadByte();
            if(type == DataType.PushClientData)
            {
                Hero hero = ObjectManager.GetHero(message.SenderConnection);
                hero.PushFromClient(message);
            }
            else if(type == DataType.AutoAttack)
            {
                ushort tarObjID = message.ReadUInt16();
                ObjectManager.ApplyAutoAttack(message.SenderConnection, tarObjID);
            }
            else if(type == DataType.BuyItem)
            {
                byte itemID = message.ReadByte();
                ObjectManager.BuyItem(message.SenderConnection, itemID);
            }
            else if(type == DataType.SellItem)
            {
                byte slotID = message.ReadByte();
                ObjectManager.SellItem(message.SenderConnection, slotID);
            }
            else if (type == DataType.Fireball)
            {
                float xPos = message.ReadFloat();
                float zPos = message.ReadFloat();
                float xDir = message.ReadFloat();
                float zDir = message.ReadFloat();
                ObjectManager.AddFireball(message.SenderConnection, xPos, zPos, xDir, zDir);
            }
            else if (type == DataType.Fireground)
            {
                float xPos = message.ReadFloat();
                float zPos = message.ReadFloat();
                ObjectManager.AddFireground(message.SenderConnection, xPos, zPos);
            }
        }

        static void InitializeClient(NetServer server, Client client, Hero hero)
        {
            NetOutgoingMessage message = server.CreateMessage();
            message.Write((byte)DataType.Initialize);

            hero.WriteInitialData(message);
            hero.WriteSyncData(message);

            ObjectManager.WriteInitialData(message);
            
            server.SendMessage(message, client.connect, NetDeliveryMethod.ReliableOrdered);
        }

        static void LoadMap()
        {
            Tower bluetower0 = new Tower(0, -40, -37);
            Tower bluetower1 = new Tower(0, -37, -40);
            Tower bluetower2 = new Tower(0, -30, -25);
            Tower bluetower3 = new Tower(0, -41, -22);
            Tower bluetower4 = new Tower(0, -22, -41);
            Tower bluetower5 = new Tower(0, -16, -20);
            Tower bluetower6 = new Tower(0, -12, -7);
            Tower bluetower7 = new Tower(0, -42, -6);
            Tower bluetower8 = new Tower(0, -42, 20);
            Tower bluetower9 = new Tower(0, -6, -42);
            Tower bluetower10 = new Tower(0, 20, -42);
            Tower redtower0 = new Tower(1, 40, 37);
            Tower redtower1 = new Tower(1, 37, 40);
            Tower redtower2 = new Tower(1, 30, 25);
            Tower redtower3 = new Tower(1, 41, 22);
            Tower redtower4 = new Tower(1, 22, 41);
            Tower redtower5 = new Tower(1, 16, 20);
            Tower redtower6 = new Tower(1, 12, 7);
            Tower redtower7 = new Tower(1, 42, 6);
            Tower redtower8 = new Tower(1, 42, -20);
            Tower redtower9 = new Tower(1, 6, 42);
            Tower redtower10 = new Tower(1, -20, 42);
            ObjectManager.AddTower(bluetower0);
            ObjectManager.AddTower(bluetower1);
            ObjectManager.AddTower(bluetower2);
            ObjectManager.AddTower(bluetower3);
            ObjectManager.AddTower(bluetower4);
            ObjectManager.AddTower(bluetower5);
            ObjectManager.AddTower(bluetower6);
            ObjectManager.AddTower(bluetower7);
            ObjectManager.AddTower(bluetower8);
            ObjectManager.AddTower(bluetower9);
            ObjectManager.AddTower(bluetower10);
            ObjectManager.AddTower(redtower0);
            ObjectManager.AddTower(redtower1);
            ObjectManager.AddTower(redtower2);
            ObjectManager.AddTower(redtower3);
            ObjectManager.AddTower(redtower4);
            ObjectManager.AddTower(redtower5);
            ObjectManager.AddTower(redtower6);
            ObjectManager.AddTower(redtower7);
            ObjectManager.AddTower(redtower8);
            ObjectManager.AddTower(redtower9);
            ObjectManager.AddTower(redtower10);

            Core c0 = new Core(0, -42.0f, -42.0f);
            Core c1 = new Core(1, 42.0f, 42.0f);
            ObjectManager.AddCore(c0);
            ObjectManager.AddCore(c1);

            Monster m0 = new Monster(-25, -6.5f, 1.0f, 0);
            Monster m1 = new Monster(25, 6.5f, 1.0f, 0);
            Monster m2 = new Monster(-25, 3.0f, 1.0f, 0);
            Monster m3 = new Monster(25, -3.0f, 1.0f, 0);
            Monster m4 = new Monster(-37, 6.5f, 1.0f, 0);
            Monster m5 = new Monster(37, -6.5f, 1.0f, 0);
            Monster m6 = new Monster(-3.5f, -14.5f, 1.0f, 0);
            Monster m7 = new Monster(3.5f, 14.5f, 1.0f, 0);
            Monster m8 = new Monster(3.5f, -24, 1.0f, 0);
            Monster m9 = new Monster(-3.5f, 24, 1.0f, 0);
            Monster m10 = new Monster(6.5f, -34.5f, 1.0f, 0);
            Monster m11 = new Monster(-6.5f, 34.5f, 1.0f, 0);
            Monster m12 = new Monster(16, -24, 1.0f, 0);
            Monster m13 = new Monster(-16, 24, 1.0f, 0);
            ObjectManager.AddMonster(m0);
            ObjectManager.AddMonster(m1);
            ObjectManager.AddMonster(m2);
            ObjectManager.AddMonster(m3);
            ObjectManager.AddMonster(m4);
            ObjectManager.AddMonster(m5);
            ObjectManager.AddMonster(m6);
            ObjectManager.AddMonster(m7);
            ObjectManager.AddMonster(m8);
            ObjectManager.AddMonster(m9);
            ObjectManager.AddMonster(m10);
            ObjectManager.AddMonster(m11);
            ObjectManager.AddMonster(m12);
            ObjectManager.AddMonster(m13);
        }
    }
}
