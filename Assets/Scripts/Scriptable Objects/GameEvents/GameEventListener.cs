using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ScriptableObjects;
public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent Response;

    public void OnEventRaise()
    {
        Response.Invoke();
    }

    private void OnEnable()
    {
        if (gameEvent != null) gameEvent.AddListener(this);
    }


    private void OnDisable()
    {
        if(gameEvent != null) gameEvent.RemoveListener(this);
    }

}
