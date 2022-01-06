using MelonLoader;
using System;
using System.Collections;
using UnityEngine;

namespace Snerble.VRC.TouchControls.Components
{
    public abstract class ComponentBase : IDisposable
    {
        private object _updateCoroutineToken;

        public ComponentBase(GameObject gameObject)
        {
            GameObject = gameObject;
            Awake();
            _updateCoroutineToken = MelonCoroutines.Start(UpdateCoroutine());
        }

        public GameObject GameObject { get; }
        public Transform Transform => GameObject.transform;

        private IEnumerator UpdateCoroutine()
        {
            // Determine if the update method is overridden
            var updateMethod = ((Action)Update).Method;
            if (updateMethod.GetBaseDefinition().DeclaringType == updateMethod.DeclaringType)
                yield break;

            while (true)
            {
                Update();
                yield return null;
            }
        }

        public T GetComponent<T>() => GameObject.GetComponent<T>();

        public virtual void Awake() { }
        public virtual void Update() { }

        public virtual void Dispose()
        {
            if (_updateCoroutineToken != null)
            {
                MelonCoroutines.Stop(_updateCoroutineToken);
                _updateCoroutineToken = null;
            }
        }
    }
}
