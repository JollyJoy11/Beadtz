using UnityEngine;

public enum EventChoice { None, A, B }

public abstract class Event
{
    private int _expEffect;
    protected float _duration;
    protected bool _handled;
    private float _startTime;
    protected EventChoice _choiceMade = EventChoice.None;

    public string Description { get; protected set; } = "";
    public string ChoiceALabel { get; protected set; } = "Act";
    public string ChoiceBLabel { get; protected set; } = "Ignore";
    public string ChoiceATradeoff { get; protected set; } = "";
    public string ChoiceBTradeoff { get; protected set; } = "";

    public Event()
    {
        _expEffect = 0;
        _duration = 8f;
        _handled = false;
        _startTime = Time.time;
    }

    public bool HasExpired() => Time.time - _startTime >= _duration;

    public float TimerProgress => Mathf.Clamp01(1f - (Time.time - _startTime) / _duration);

    public void MakeChoice(EventChoice choice)
    {
        _choiceMade = choice;
        _handled = (choice == EventChoice.A);
    }

    public EventChoice ChoiceMade => _choiceMade;

    public bool Handled
    {
        get => _handled || _choiceMade == EventChoice.B;
        set
        {
            _handled = value;
            if (value && _choiceMade == EventChoice.None)
                _choiceMade = EventChoice.A;
        }
    }

    public int ExpEffect
    {
        get => _expEffect;
        set => _expEffect = value;
    }

    public abstract void Handle(Concert concert);
}