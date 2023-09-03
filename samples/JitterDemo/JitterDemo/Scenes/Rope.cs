using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter;
using Microsoft.Xna.Framework;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using Jitter.LinearMath;
using Jitter.Dynamics.Constraints;

namespace JitterDemo.Scenes
{
    class Rope : Scene
    {

        public Rope(JitterDemo demo)
            : base(demo)
        {
        }

        public override void Build()
        {
            AddGround();

            RigidBody last = null;

            for (int i = 0; i < 12; i++)
            {
                RigidBody body = new RigidBody(new BoxShape(Vector3.One));
                body.Position = new Vector3(i * 1.5f-20, 0.5f, 0);

                Vector3 jpos2 = body.Position;

                Demo.World.AddBody(body);
                body.Update();

                if (last != null)
                {
                    Vector3 jpos3 = last.Position;

                    Vector3 dif; Vector3.Subtract(ref jpos2, ref jpos3, out dif);
                    Vector3.Multiply(ref dif, 0.5f, out dif);
                    Vector3.Subtract(ref jpos2, ref dif, out dif);

                    Constraint cons = new PointOnPoint(last, body, dif);
                    Demo.World.AddConstraint(cons);
                }

                last = body;
            }
           
        }


    }
}
