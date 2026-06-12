using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Venue", menuName = "Beadtz/Venue")]
public class Venue : ScriptableObject
{
    [SerializeField] private string _venueName;
    [SerializeField] private VenueType _type;
    [SerializeField] private int _capacity;
    [SerializeField] private int _cost;
    [SerializeField] private Sprite _venueImage;
    private int _concertCount = 0;
    private bool _isBought = false;
    private List<VenueSchedule> _schedules = new List<VenueSchedule>();

    public string VenueName
    {
        get { return _venueName; }
    }

    public VenueType Type
    {
        get { return _type; }
    }

    public Sprite VenueImage
    {
        get { return _venueImage; }
    }

    public int Capacity
    {
        get { return _capacity; }
    }

    public int Cost
    {
        get { return _cost; }
    }

    public bool IsBought
    {
        get { return _isBought; }
        set { _isBought = value; }
    }

    public List<VenueSchedule> Schedules
    {
        get { return _schedules; }
    }

    public int ConcertCount
    {
        get { return _concertCount; }
        set { _concertCount = value; }
    }

    public void AddSchedule(VenueSchedule v)
    {
        _schedules.Add(v);
    }

    public bool CheckScheduleAfter(GameDateTime time)
    {
        foreach (VenueSchedule s in _schedules)
        {
            if (time.IsTimeBetween(s.StartTime, s.EndTime))
            {
                return true;
            }
        }

        return false;
    }

    public bool ShouldRenovate()
    {
        if (_concertCount >= 3)
        {
            return true;
        }

        return false;
    }

    public bool CheckScheduleConflict(GameDateTime start, GameDateTime end)
    {
        foreach (VenueSchedule existingSchedule in _schedules)
        {
            bool startsInside = start.IsTimeBetween(existingSchedule.StartTime, existingSchedule.EndTime);
            bool endsInside = end.IsTimeBetween(existingSchedule.StartTime, existingSchedule.EndTime);
            bool fullyOverlaps = existingSchedule.StartTime.IsTimeBetween(start, end) && existingSchedule.EndTime.IsTimeBetween(start, end);

            if (startsInside || endsInside || fullyOverlaps)
            {
                return true;
            }
        }

        return false;
    }

    public VenueSchedule SearchCleaningSchedule(GameDateTime concertEndTime)
    {
        foreach (VenueSchedule v in _schedules)
        {
            if (v.ScheduleType == ScheduleType.Cleanup && v.StartTime == concertEndTime)
            {
                return v;
            }
        }

        return null;
    }
}
