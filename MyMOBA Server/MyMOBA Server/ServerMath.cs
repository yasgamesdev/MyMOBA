using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimMath;
using MyMOBA_Server.Objects;

namespace MyMOBA_Server
{
    public static class ServerMath
    {
        public static float Distance(float xPos0, float zPos0, float radius0, float xPos1, float zPos1, float radius1)
        {
            return Vector2.Distance(new Vector2(xPos0, zPos0), new Vector2(xPos1, zPos1)) - radius0 - radius1;
        }

        public static float Distance(ServerSideObject a, ServerSideObject b)
        {
            return Vector2.Distance(new Vector2(a.GetXPos(), a.GetZPos()), new Vector2(b.GetXPos(), b.GetZPos())) - a.radius - b.radius;
        }

        public static Quaternion LookAt(ServerSideObject source, ServerSideObject dest)
        {
            Vector3 sourcePoint = new Vector3(source.GetXPos(), 0, source.GetZPos());
            Vector3 destPoint = new Vector3(dest.GetXPos(), 0, dest.GetZPos());
            return LookAt(sourcePoint, destPoint);
        }

        public static Quaternion LookAt(Vector3 sourcePoint, Vector3 destPoint)
        {
            Vector3 forward = new Vector3(0, 0, 1);
            Vector3 up = new Vector3(0, 1, 0);

            Vector3 forwardVector = Vector3.Normalize(destPoint - sourcePoint);

            float dot = Vector3.Dot(forward, forwardVector);

            if (Math.Abs(dot - (-1.0f)) < 0.000001f)
            {
                return new Quaternion(up.X, up.Y, up.Z, 3.1415926535897932f);
            }
            if (Math.Abs(dot - (1.0f)) < 0.000001f)
            {
                return Quaternion.Identity;
            }

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Cross(forward, forwardVector);
            rotAxis = Vector3.Normalize(rotAxis);
            return CreateFromAxisAngle(rotAxis, rotAngle);
        }

        static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float halfAngle = angle * .5f;
            float s = (float)Math.Sin(halfAngle);
            Quaternion q;
            q.X = axis.X * s;
            q.Y = axis.Y * s;
            q.Z = axis.Z * s;
            q.W = (float)Math.Cos(halfAngle);
            return q;
        }
    }
}
