using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Jitter.LinearMath;
#if WINDOWS

using Jitter.Collision;
using System.Globalization;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;

namespace JitterDemo.Scenes
{

    class ConvexDecomposition : Scene
    {

        public ConvexDecomposition(JitterDemo demo)
            : base(demo)
        {


        }

        public override void Build()
        {
            AddGround();

            List<ConvexHullShape> shapes = BuildFromHACDTestObjFile(@"Content/ConvexDecomposition.obj");

            CompoundShape.TransformedShape[] transformedShapes
                = new CompoundShape.TransformedShape[shapes.Count];

            for (int i = 0; i < shapes.Count; i++)
            {
                transformedShapes[i] = new CompoundShape.TransformedShape();
                transformedShapes[i].Shape = shapes[i];
                transformedShapes[i].Orientation = Matrix4x4.Identity;
                transformedShapes[i].Position = -1.0f * shapes[i].Shift;
            }


            // Create one compound shape
            CompoundShape cs = new CompoundShape(transformedShapes);

            for (int i = 0; i < 1; i++)
            {
                RigidBody compoundBody = new RigidBody(cs);
                compoundBody.EnableDebugDraw = true;
                compoundBody.Position = new Vector3(0, 5+ i*10, 0) - cs.Shift;
                Demo.World.AddBody(compoundBody);
            }


            // Create several single bodies.
            for (int i = 0; i < shapes.Count; i++)
            {
                RigidBody body = new RigidBody(shapes[i]);
                body.Position = -1.0f * shapes[i].Shift + new Vector3(-10, 5, 0);
                body.EnableDebugDraw = true;
                Demo.World.AddBody(body);
            }

            for (int i = 0; i < shapes.Count; i++)
            {
                RigidBody body = new RigidBody(shapes[i]);
                body.Position = -1.0f * shapes[i].Shift + new Vector3(-20, 5, 0);
                body.EnableDebugDraw = true;
                body.IsStatic = true;
                Demo.World.AddBody(body);
            }

        }

        /// <summary>
        /// A really stupid parser for convex decomposed files made by testhacd.exe (see Other\hacdtest)
        /// </summary>
        public List<ConvexHullShape> BuildFromHACDTestObjFile(string path)
        {
            List<ConvexHullShape> shapes = new List<ConvexHullShape>();

            string[] lines = File.ReadAllLines(path);
            Char[] splitter = new Char [] {' '};

            List<Vector3> convexPoints = new List<Vector3>();
            
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("v"))
                {
                    string[] values = line.Split(splitter);

                    Vector3 vertex = new Vector3(float.Parse(values[1], NumberFormatInfo.InvariantInfo),
                        float.Parse(values[2], NumberFormatInfo.InvariantInfo),
                        float.Parse(values[3], NumberFormatInfo.InvariantInfo));

                    convexPoints.Add(vertex * 5f);
                }

                if(line.StartsWith("#"))
                {
                    if(convexPoints.Count > 0)
                    {
                        List<Vector3> copyVertex = new List<Vector3>(convexPoints);
                        convexPoints.Clear();

                        ConvexHullShape cvhs = new ConvexHullShape(copyVertex);

                        if(cvhs.Mass > 0.001f) shapes.Add(cvhs);
                    }
                }
            }


            return shapes;
        }

    }
}

#endif
