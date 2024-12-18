using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Shared Event", menuName = "ScriptableObjects/SharedValues/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<EventListener> listeners = new List<EventListener>();

        public void Invoke()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnInvoke();
            }
        }

        public void RegisterListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}

