using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Artist", menuName = "Beadtz/Artist")]
public abstract class Artist : ScriptableObject
{
    [SerializeField] private string _artistName;
    [SerializeField] private int _fame;
    [SerializeField] private Sprite _artistImage;
    private float _energy = 100;
    private bool _isOnBreak = false;
    private GameDateTime _breakStartTime;
    [SerializeField] private List<Song> _songs;

    public string ArtistName
    {
        get { return _artistName; }
    }

    public Sprite ArtistImage
    {
        get { return _artistImage; }
    }

    public int Fame
    {
        get { return _fame; }
        set { _fame = value; }
    }

    public float Energy
    {
        get { return _energy; }
        set { _energy = value; }
    }

    public bool IsOnBreak
    {
        get { return _isOnBreak; }
        set { _isOnBreak = value; }
    }

    public List<Song> Songs
    {
        get { return _songs; }
    }
    
    public GameDateTime BreakStartTime
    {
        get { return _breakStartTime; }
        set { _breakStartTime = value; }
    }

    public void RestoreEnergy(GameDateTime time, Player player)
    {
        if (!IsOnBreak)
        {
            GameDateTime lastConcertDate = player.SearchLastConcertTime(_artistName);

            if (lastConcertDate == null)
            {
                lastConcertDate = time;
            }

            int daysSinceLastRest = lastConcertDate.GetDaysBetween(time);

            if (daysSinceLastRest >= 3) 
            {
                int recoveryAmount = Mathf.Clamp((daysSinceLastRest - 1) * 1, 1, 100);
                _energy = Mathf.Min(_energy + recoveryAmount, 100);
            }
        }
        else
        {
            if (_breakStartTime == null)
            {
                _breakStartTime = time; 
            }

            int daysResting = _breakStartTime.GetDaysBetween(time);
            Energy = Mathf.Min(_energy + daysResting * 5, 100);

            if (Energy >= 100)
            {
                _isOnBreak = false;
            }
        }
    }

    public void IncreaseFame(int crowdSize, float crowdEnergy)
    {
        if (crowdEnergy > 0)
        {
            float fameBoost = Mathf.Sqrt(crowdSize) / Mathf.Log10(_fame + 10);
            fameBoost = Mathf.Clamp(fameBoost, 1, 30);

            _fame += Mathf.RoundToInt(fameBoost);
        }
        else
        {
            _fame -= 20;
        }
    }

    public void DecreaseFame()
    {
        _fame -= 30;
    }

    public abstract void DecreaseEnergy(Player player);
} 