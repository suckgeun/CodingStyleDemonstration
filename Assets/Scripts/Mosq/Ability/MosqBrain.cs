using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosqBrain : MonoBehaviour, IMosqAbility
{
    [SerializeField]
    [Range(0f, 1f)]
    private float _probEnterRoom;
    [SerializeField]
    private float _frequency;

    private float _timer;
    private IRoomEventListener[] _roomListeners;

    private void Awake()
    {
        _roomListeners = GetComponents<IRoomEventListener>();
    }

    public void ProcessEarlyAbility()
    {
        if (Time.time - _timer < _frequency) { return; }
        _timer = Time.time;

        // calculate the probability  
        float prob = Random.Range(0f, 1f);
        if (prob < _probEnterRoom)
        {
            RaiseEnterRoomEvent();
        }
        else
        {
            RaiseEnterAreaEvent();
        }

    }

    private void RaiseEnterRoomEvent()
    {
        foreach (IRoomEventListener e in _roomListeners)
        {
            e.OnRoomEnter();
        }
    }

    private void RaiseEnterAreaEvent()
    {
        foreach (IRoomEventListener e in _roomListeners)
        {
            e.OnAreaEnter();
        }
    }

    public void ProcessAbility() { }
}
