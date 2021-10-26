using UnityEngine;

namespace Snerble.VRC.TouchControls
{
    public static class ComponentExtensions
    {
        public static string GetPath(this Component c)
        {
            if (c == null || !c)
                return null;
            return GetPath(c.transform.parent) + "/" + c.gameObject.name;
        }
    }
}
