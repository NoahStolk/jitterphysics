using System;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;

namespace JitterDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread()]
        static void Main(string[] args)
        {
            using (JitterDemo game = new JitterDemo())
            {
                //Jitter.DynamicTree dt = new Jitter.DynamicTree();

                //JBBox jb;
                //jb.Min = Vector3.Zero;
                //jb.Max = Vector3.One;

                //JBBox jb2;
                //jb2.Min = Vector3.Zero;
                //jb.Max = Vector3.One * 2.0f;

                //dt.CreateProxy(ref jb, 1);
                //dt.CreateProxy(ref jb, 2);

                //JBBox testBox;
                //testBox.Min = Vector3.Zero;
                //testBox.Max = Vector3.One *20.0f;

                //dt.Query(bla, ref testBox);
                //dt.MoveProxy


                game.Run();
            }



        }

        private static bool bla(int i)
        {

            return true;
        }
    }
}

