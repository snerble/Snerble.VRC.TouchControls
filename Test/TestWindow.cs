using Blotch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snerble.VRC.TouchControls.Touch;
using Test.Touch;
using static Test.Touch.DynamicBoneTouchSensor;

namespace Test;

internal class TestWindow : BlWindow3D
{
    private readonly List<GameObject> gameObjects = new();

    private DynamicBone dynamicBones;
    private DynamicBoneTouchSensor sensor;
    private DynamicBoneColliderTouchProbe[] probes;
    private TouchUnit unit;

    private SpriteFont Font;
    private Model Sphere;
    private Texture2D Albedo;

    public TestWindow()
    {
        var bones = new Transform(new(0, 0, 0))
        {
            new(new(1, 0, 0))
            {
                new(new(2, 0, 0))
                {
                    new(new (3, 0, 0))
                    {
                        new(new (4, 0, 0))
                        {
                            new(new (5, 0, 0))
                            {
                                new(new (6, 0, 0))
                                {
                                    new(new (7, 0, 0))
                                    {
                                        new(new (8, 0, 0))
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var colliders = new List<DynamicBoneCollider>
        {
            new(new(new(0, 2, 0)), new(), 0.5f)
        };

        dynamicBones = new DynamicBone()
        {
            m_Root = bones,
            m_Colliders = colliders,
            m_Radius = 0.5f,
            m_RadiusDistrib = new(t => Math.Clamp((float)Math.Pow(t, 0.5f) + 0.1f, 0, 1))
        };

        sensor = new DynamicBoneTouchSensor(dynamicBones);
        probes = DynamicBoneColliderTouchProbe.FromDynamicBones(dynamicBones).ToArray();

        unit = new TouchUnit(sensor, probes);
    }

    protected override void Setup()
    {
        var myContent = new ContentManager(Services, "Content");

        Albedo = Graphics.LoadFromImageFile("white.png");
        Font = myContent.Load<SpriteFont>("Arial14");
        Sphere = myContent.Load<Model>("uv_sphere_192x96");

        for (int i = 0; i < sensor.Sensors.Length; i++)
        {
            BoneSegmentSensor s = (BoneSegmentSensor)sensor.Sensors[i];
            
            var sprite = new BlSprite(Graphics, "Sensor" + i)
            {
                BoundSphere = new BoundingSphere(new Vector3(), 1),
                Mipmap = Albedo
            };
            sprite.LODs.Add(Sphere);

            gameObjects.Add(new(
                () => s.Position,
                () => s.Radius,
                sprite));
        }

        for (int i = 0; i < probes.Length; i++)
        {
            DynamicBoneColliderTouchProbe probe = probes[i];

            var sprite = new BlSprite(Graphics, "Probe" + i)
            {
                BoundSphere = new BoundingSphere(new Vector3(), 1),
                Mipmap = Albedo
            };
            sprite.LODs.Add(Sphere);

            gameObjects.Add(new(
                () => probe.Position,
                () => probe.Radius,
                sprite));
        }
    }

    protected override void Update(GameTime timeInfo)
    {
        base.Update(timeInfo);
    }

    protected override void FrameDraw(GameTime timeInfo)
    {
        Graphics.DoDefaultGui();

        var measurement = unit.Measure();
        var color = Color.Lerp(Color.Red, Color.Green, measurement).ToVector3();

        foreach (var obj in gameObjects)
        {
            var p = obj.Position();

            obj.Sprite.Color = color;
            obj.Sprite.Draw(
                Matrix.CreateScale(obj.Scale())
                * Matrix.CreateTranslation(new(p.X, p.Y, p.Z))
                );
        }

        var keyboard = Keyboard.GetState();
        foreach (var key in keyboard.GetPressedKeys())
        {
            var m = ((Vector3)(key switch
            {
                Keys.Up => new(0, 1, 0),
                Keys.Down => new(0, -1, 0),
                Keys.Right => new(1, 0, 0),
                Keys.Left => new(-1, 0, 0),
                _ => default
            })) * (float)timeInfo.ElapsedGameTime.TotalSeconds;

            dynamicBones.m_Colliders[0].transform.position += new System.Numerics.Vector3(m.X, m.Y, m.Z);
        }

        try
        {
            var MyHud = $@"
Camera controls:
Dolly  -  Wheel
Zoom   -  Left-CTRL-wheel
Truck  -  Left-drag 
Rotate -  Right-drag
Pan    -  Left-ALT-left-drag
Reset  -  Esc
Fine control  -  Left-Shift

Touch: {measurement}

Eye: {Graphics.Eye}
LookAt: {Graphics.LookAt}
MaxDistance: {Graphics.MaxCamDistance}
MinDistance: {Graphics.MinCamDistance}
ViewAngle: {Graphics.Zoom}
Probe: {probes[0].Position}";

            Graphics.DrawText(MyHud, Font, new Vector2(50, 50));
        }
        catch { }
    }
}

public record GameObject(
    Func<System.Numerics.Vector3> Position,
    Func<float> Scale,
    BlSprite Sprite);