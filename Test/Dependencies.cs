using System.Collections;
using System.Numerics;

namespace Test;

public class DynamicBone
{
    public Transform m_Root { get; set; }

    public float m_Radius { get; set; }

    public AnimationCurve m_RadiusDistrib { get; set; } = new();

    public List<DynamicBoneCollider> m_Colliders { get; set; } = new();

    public List<Transform> m_Exclusions { get; set; } = new();
}

public class DynamicBoneCollider
{
    public DynamicBoneCollider(Transform transform, Vector3 center, float radius)
    {
        this.transform = transform;
        m_Center = center;
        m_Radius = radius;
    }

    public Transform transform { get; set; }

    public Vector3 m_Center { get; set; }

    public float m_Radius { get; set; }
}

public class Transform : IEnumerable<Transform>
{
    internal List<Transform> _children = new();

    public Transform(Vector3 position)
    {
        this.position = position;
    }

    public Vector3 position { get; set; }

    public Transform parent { get; set; }

    public int childCount => _children.Count;

    public Transform GetChild(int i) => _children[i];

    public void Add(Transform child) => _children.Add(child);

    public IEnumerator<Transform> GetEnumerator()
    {
        return ((IEnumerable<Transform>)_children).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_children).GetEnumerator();
    }
}

public class AnimationCurve
{
    private readonly Func<float, float> func;

    public AnimationCurve(Func<float, float> func = null)
    {
        this.func = func ?? (t => 1);
    }

    public float Evaluate(float time)
    {
        return func(time);
    }
}
