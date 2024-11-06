using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ModularData
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            if(Event != null)
            {
                Event.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if(Event != null)
            {
                Event.UnregisterListener(this);
            }
        }

        public void OnInvoke()
        {
            Response.Invoke();
        }
    }
}
