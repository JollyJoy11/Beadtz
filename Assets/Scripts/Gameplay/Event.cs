using UnityEngine;
using System;

public abstract class Event
{
    private int _expEffect;
    private float _duration;
    protected bool _handled;
    private float _startTime;

    public Event()
    {
        _expEffect = 0;
        _duration = 5;
        _handled = false;
        _startTime = Time.time;
    }

    public bool HasExpired()
    {
        return Time.time - _startTime >= _duration; 
    }

    public bool Handled
    {
        get { return _handled; }
        set { _handled = value; }
    }

    public int ExpEffect
    {
        get { return _expEffect; }
        set { _expEffect = value; }
    }

    public abstract void Handle(Concert concert);
}