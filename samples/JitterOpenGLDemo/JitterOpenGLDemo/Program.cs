using System;
using Jitter.LinearMath;
using Jitter.Collision;
using Jitter;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;
using System.Numerics;

namespace JitterOpenGLDemo;

public class Program : DrawStuffOtk
{
    private readonly World world;
    private float accTime;
    private bool initFrame = true;

    private const string _title = "Jitter OpenGL - Press 'Space' to shoot a sphere, 'R' to Reset";

    private Program()
        : base(800, 600)
    {
        world = new(new CollisionSystemSAP())
        {
            Gravity = new(0, 0, -10),
        };

        VSync = OpenTK.VSyncMode.Off;
        Title = _title;

        Keyboard.KeyDown += Keyboard_KeyDown;

        BuildScene();
    }

    private void BuildScene()
    {
        world.Clear();

        RigidBody body = AddBox(new(0, 0, -0.5f), Vector3.Zero,
        new(300, 300, 1));

        body.IsStatic = true;
        body.Tag = false;

        for (int i = 0; i < 20; i++)
        {
            for (int e = i; e < 20; e++)
            {
                AddBox(new(0.0f, (e - i * 0.5f) * 1.01f, 0.5f + i * 1.0f),
                Vector3.Zero, Vector3.One);
            }
        }
    }

    private void ShootSphere()
    {
        dsGetViewPoint(out Vector3 pos, out Vector3 ang);

        RigidBody body = new(new SphereShape(1.0f))
        {
            Position = pos,
        };

        Vector3 unit;
        unit.X = (float)Math.Cos(ang.X / 180.0f * JMath.Pi);
        unit.Y = (float)Math.Sin(ang.X / 180.0f * JMath.Pi);
        unit.Z = (float)Math.Sin(ang.Y / 180.0f * JMath.Pi);

        body.LinearVelocity = unit * 50.0f;

        world.AddBody(body);
    }

    private RigidBody AddBox(Vector3 position, Vector3 velocity, Vector3 size)
    {
        BoxShape shape = new(size);
        RigidBody body = new(shape);
        world.AddBody(body);
        body.Position = position;
        body.Material.Restitution = 0.0f;
        body.LinearVelocity = velocity;
        body.IsActive = false;
        return body;
    }

    private void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
    {
        if (e.Key == OpenTK.Input.Key.Space) ShootSphere();
        if (e.Key == OpenTK.Input.Key.R) BuildScene();
    }

    protected override void OnBeginRender(double elapsedTime)
    {
        if (initFrame)
        {
            dsSetViewpoint(new float[] { 18, 10, 8 }, new float[] { 190, -10, 0 });
            initFrame = false;
        }

        RenderAll();

        base.OnBeginRender(elapsedTime);
    }

    private void RenderAll()
    {
        dsSetTexture(DS_TEXTURE_NUMBER.DS_WOOD);

        foreach (RigidBody body in world.RigidBodies)
        {
            if (body.Tag is bool) continue;

            if (body.Shape is BoxShape)
            {
                BoxShape shape = body.Shape as BoxShape;

                if (body.IsActive) dsSetColor(1, 1, 1);
                else dsSetColor(0.5f, 0.5f, 1);

                dsDrawBox(body.Position, body.Orientation, shape.Size);
            }
            else if (body.Shape is SphereShape)
            {
                SphereShape shape = body.Shape as SphereShape;

                if (body.IsActive) dsSetColor(1,1,0);
                else dsSetColor(0.5f, 0.5f, 1);

                dsDrawSphere(body.Position, body.Orientation, shape.Radius-0.1f);
            }
        }
    }

    protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
    {
        accTime += 1.0f / (float)RenderFrequency;

        if (accTime > 1.0f)
        {
            Title = _title + " " + RenderFrequency.ToString("##.#") + " fps";
            accTime = 0.0f;
        }

        float step = 1.0f / (float)RenderFrequency;
        if (step > 1.0f / 100.0f) step = 1.0f / 100.0f;
        world.Step(step, true);

        base.OnUpdateFrame(e);
    }

    private static void Main(string[] args)
    {
        using Program p = new();
        p.Run();
    }
}
