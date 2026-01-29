using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "NewGameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Invoke()
        {
            foreach (GameEventListener a in listeners)
            {
                a.OnEventRaise();
            }
        }

        public void AddListener(GameEventListener ls)
        {
            listeners.Add(ls);
        }

        public void RemoveListener(GameEventListener ls)
        {
            listeners.Remove(ls);
        }
    }
    
}
