using MyMOBA_Server.Objects.Minion;
using SlimMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server
{
    public static class MinionGenerator
    {
        static float timer = 0;
        static bool firstSpawn = false;
        static bool secondSpawn = false;
        static List<Vector2> bluerelay0 = new List<Vector2>();
        static List<Vector2> redrelay0 = new List<Vector2>();
        static List<Vector2> bluerelay1 = new List<Vector2>();
        static List<Vector2> redrelay1 = new List<Vector2>();
        static List<Vector2> bluerelay2 = new List<Vector2>();
        static List<Vector2> redrelay2 = new List<Vector2>();
        static List<Vector2> bluerelay3 = new List<Vector2>();
        static List<Vector2> redrelay3 = new List<Vector2>();
        static List<Vector2> bluerelay4 = new List<Vector2>();
        static List<Vector2> redrelay4 = new List<Vector2>();
        static List<Vector2> bluerelay5 = new List<Vector2>();
        static List<Vector2> redrelay5 = new List<Vector2>();
        static List<Vector2> bluerelay6 = new List<Vector2>();
        static List<Vector2> redrelay6 = new List<Vector2>();
        static List<Vector2> bluerelay7 = new List<Vector2>();
        static List<Vector2> redrelay7 = new List<Vector2>();
        static List<Vector2> bluerelay8 = new List<Vector2>();
        static List<Vector2> redrelay8 = new List<Vector2>();
        static bool init = false;

        public static void Update(float delta)
        {
            if(!init)
            {
                Initialize();
            }

            timer += delta;
            if(timer >= 5.0f && !firstSpawn)
            {
                Generate();
                firstSpawn = true;
            }
            if(timer >= 7.0f && !secondSpawn)
            {
                //Generate();
                secondSpawn = true;
            }
            if(timer >= 30.0f)
            {
                timer = 0;
                firstSpawn = false;
                secondSpawn = false;
            }
        }

        static void Initialize()
        {
            init = true;

            bluerelay0.Add(new Vector2(-39,-39));
            bluerelay0.Add(new Vector2(39, 39));
            redrelay0.Add(new Vector2(39, 39));
            redrelay0.Add(new Vector2(-39, -39));
            bluerelay1.Add(new Vector2(-39-1.4f, -39));
            bluerelay1.Add(new Vector2(39-1.4f, 39));
            redrelay1.Add(new Vector2(39+1.4f, 39));
            redrelay1.Add(new Vector2(-39+1.4f, -39));
            bluerelay2.Add(new Vector2(-39, -39-1.4f));
            bluerelay2.Add(new Vector2(39, 39-1.4f));
            redrelay2.Add(new Vector2(39, 39+1.4f));
            redrelay2.Add(new Vector2(-39, -39+1.4f));

            bluerelay3.Add(new Vector2(-45, -39));
            bluerelay3.Add(new Vector2(-45, 26));
            bluerelay3.Add(new Vector2(-26, 45));
            bluerelay3.Add(new Vector2(39, 45));
            redrelay3.Add(new Vector2(39, 45));
            redrelay3.Add(new Vector2(-26, 45));
            redrelay3.Add(new Vector2(-45, 26));
            redrelay3.Add(new Vector2(-45.1f, -39));
            bluerelay4.Add(new Vector2(-45-1, -39-1));
            bluerelay4.Add(new Vector2(-45 - 1, 26-1));
            bluerelay4.Add(new Vector2(-26 - 1, 45 - 1));
            bluerelay4.Add(new Vector2(39 - 1, 45 - 1));
            redrelay4.Add(new Vector2(39+1, 45 + 1));
            redrelay4.Add(new Vector2(-26 + 1, 45 + 1));
            redrelay4.Add(new Vector2(-45 + 1, 26 + 1));
            redrelay4.Add(new Vector2(-45.1f + 1, -39 + 1));
            bluerelay5.Add(new Vector2(-45+1, -39-1));
            bluerelay5.Add(new Vector2(-45+1, 26-1));
            bluerelay5.Add(new Vector2(-26+1, 45-1));
            bluerelay5.Add(new Vector2(39+1, 45-1));
            redrelay5.Add(new Vector2(39+1, 45-1));
            redrelay5.Add(new Vector2(-26+1, 45-1));
            redrelay5.Add(new Vector2(-45+1, 26-1));
            redrelay5.Add(new Vector2(-45.1f+1, -39-1));

            bluerelay6.Add(new Vector2(-39, -45));
            bluerelay6.Add(new Vector2(26, -45));
            bluerelay6.Add(new Vector2(45, -26));
            bluerelay6.Add(new Vector2(45, 39));
            redrelay6.Add(new Vector2(45.1f, 39));
            redrelay6.Add(new Vector2(45, -26));
            redrelay6.Add(new Vector2(26, -45));
            redrelay6.Add(new Vector2(-39, -45));
            bluerelay7.Add(new Vector2(-39-1, -45+1));
            bluerelay7.Add(new Vector2(26-1, -45+1));
            bluerelay7.Add(new Vector2(45-1, -26+1));
            bluerelay7.Add(new Vector2(45-1, 39+1));
            redrelay7.Add(new Vector2(45.1f-1, 39+1));
            redrelay7.Add(new Vector2(45-1, -26+1));
            redrelay7.Add(new Vector2(26-1, -45+1));
            redrelay7.Add(new Vector2(-39-1, -45+1));
            bluerelay8.Add(new Vector2(-39-1, -45-1));
            bluerelay8.Add(new Vector2(26-1, -45-1));
            bluerelay8.Add(new Vector2(45-1, -26-1));
            bluerelay8.Add(new Vector2(45-1, 39-1));
            redrelay8.Add(new Vector2(45.1f+1, 39+1));
            redrelay8.Add(new Vector2(45+1, -26+1));
            redrelay8.Add(new Vector2(26+1, -45+1));
            redrelay8.Add(new Vector2(-39+1, -45+1));
        }

        static void Generate()
        {
            Minion bluemimion0 = new Minion(0, bluerelay0[0].X, bluerelay0[0].Y, 0, 1.0f, bluerelay0);
            ObjectManager.AddMinion(bluemimion0);
            Minion redminion0 = new Minion(1, redrelay0[0].X, redrelay0[0].Y, 0, 1.0f, redrelay0);
            ObjectManager.AddMinion(redminion0);
            Minion bluemimion1 = new Minion(0, bluerelay1[0].X, bluerelay1[0].Y, 0, 1.0f, bluerelay1);
            ObjectManager.AddMinion(bluemimion1);
            Minion redminion1 = new Minion(1, redrelay1[0].X, redrelay1[0].Y, 0, 1.0f, redrelay1);
            ObjectManager.AddMinion(redminion1);
            Minion bluemimion2 = new Minion(0, bluerelay2[0].X, bluerelay2[0].Y, 0, 1.0f, bluerelay2);
            ObjectManager.AddMinion(bluemimion2);
            Minion redminion2 = new Minion(1, redrelay2[0].X, redrelay2[0].Y, 0, 1.0f, redrelay2);
            ObjectManager.AddMinion(redminion2);

            Minion bluemimion3 = new Minion(0, bluerelay3[0].X, bluerelay3[0].Y, 0, 1.0f, bluerelay3);
            ObjectManager.AddMinion(bluemimion3);
            Minion redminion3 = new Minion(1, redrelay3[0].X, redrelay3[0].Y, 0, 1.0f, redrelay3);
            ObjectManager.AddMinion(redminion3);
            Minion bluemimion4 = new Minion(0, bluerelay4[0].X, bluerelay4[0].Y, 0, 1.0f, bluerelay4);
            ObjectManager.AddMinion(bluemimion4);
            Minion redminion4 = new Minion(1, redrelay4[0].X, redrelay4[0].Y, 0, 1.0f, redrelay4);
            ObjectManager.AddMinion(redminion4);
            Minion bluemimion5 = new Minion(0, bluerelay5[0].X, bluerelay5[0].Y, 0, 1.0f, bluerelay5);
            ObjectManager.AddMinion(bluemimion5);
            Minion redminion5 = new Minion(1, redrelay5[0].X, redrelay5[0].Y, 0, 1.0f, redrelay5);
            ObjectManager.AddMinion(redminion5);

            Minion bluemimion6 = new Minion(0, bluerelay6[0].X, bluerelay6[0].Y, 0, 1.0f, bluerelay6);
            ObjectManager.AddMinion(bluemimion6);
            Minion redminion6 = new Minion(1, redrelay6[0].X, redrelay6[0].Y, 0, 1.0f, redrelay6);
            ObjectManager.AddMinion(redminion6);
            Minion bluemimion7 = new Minion(0, bluerelay7[0].X, bluerelay7[0].Y, 0, 1.0f, bluerelay7);
            ObjectManager.AddMinion(bluemimion7);
            Minion redminion7 = new Minion(1, redrelay7[0].X, redrelay7[0].Y, 0, 1.0f, redrelay7);
            ObjectManager.AddMinion(redminion7);
            Minion bluemimion8 = new Minion(0, bluerelay8[0].X, bluerelay8[0].Y, 0, 1.0f, bluerelay8);
            ObjectManager.AddMinion(bluemimion8);
            Minion redminion8 = new Minion(1, redrelay8[0].X, redrelay8[0].Y, 0, 1.0f, redrelay8);
            ObjectManager.AddMinion(redminion8);
        }        
    }
}
