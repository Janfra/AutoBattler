using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ModularData
{
    [Serializable]
    public class EventListener : IRuntimeScriptableObject
    {
        [SerializeField]
        private GameEvent Event;
        [SerializeField]
        private UnityEvent Response;

        public void Register()
        {
            if (Event != null)
            {
                Event.RegisterListener(this);
            }
        }

        public void Unregister()
        {
            if (Event != null)
            {
                Event.UnregisterListener(this);
            }
        }

        public void OnInvoke()
        {
            Response?.Invoke();
        }

        public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
        {
            if (Event == null)
            {
                return;
            }

            replacer.SetReference(ref Event);
        }
    }

    public class GameEventListenerComponent : MonoBehaviour, IRuntimeScriptableObject
    {
        [SerializeField]
        private List<EventListener> Events;

        private void OnEnable()
        {
            foreach (EventListener listener in Events)
            {
                listener.Register();
            }
        }

        private void OnDisable()
        {
            foreach (EventListener listener in Events)
            {
                listener.Unregister();
            }
        }

        public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            foreach (var listener in Events)
            {
                listener.OnReplaceReferences(replacer);
            }
        }
    }
}
