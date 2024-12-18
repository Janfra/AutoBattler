using System;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

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
            for (int i = Response.GetPersistentEventCount() - 1; i >= 0; i--)
            {
                Object target = Response.GetPersistentTarget(i);
                if (target is ScriptableObject targetSO)
                {
                    if (!replacer.ContainsReference(targetSO))
                    {
                        continue;
                    }

                    replacer.SetReference(ref targetSO);
                    UnityAction action = UnityAction.CreateDelegate(typeof(UnityAction), targetSO, Response.GetPersistentMethodName(i)) as UnityAction;
                    Response.AddListener(action);
                    UnityEventTools.RemovePersistentListener(Response, i);
                }
            }
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
