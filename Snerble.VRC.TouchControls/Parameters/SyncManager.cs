using Snerble.VRC.TouchControls.VRCPlayers;
using System.Collections.Generic;
using System.Linq;

namespace Snerble.VRC.TouchControls.Parameters
{
    public static class SyncManager
    {
        private static Dictionary<object, object> _locks = new Dictionary<object, object>();

        [Hook]
        public static void OnApplicationStart()
        {
            VRCPlayerUtils.CurrentPlayerReady += (sender, e) => _locks.Clear();
        }

        public static bool OwnsLock(object owner)
        {
            lock (_locks)
            {
                return _locks.Values.Contains(owner); 
            }
        }

        public static void Lock(object value, object owner)
        {
            lock (_locks)
            {
                _locks[value] = owner; 
            }
        }

        public static void Release(object owner)
        {
            lock (_locks)
            {
                var value = _locks.FirstOrDefault(x => x.Value == owner).Key;
                if (value != null)
                    _locks.Remove(value); 
            }
        }
    }
}
