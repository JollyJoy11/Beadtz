using UnityEngine;
using System.Collections.Generic;

public class Concert
{
    private Artist _artist;
    private City _city;
    private Venue _venue;
    private List<Song> _setlist;
    private GameDateTime _concertTime;
    private GameDateTime _endTime;
    private List<Equipment> _equipmentBought;
    private Event _currentEvent;
    private GameDateTime _lastSecurityEventTime;
    private List<Event> _completedEvents;
    private int _crowdSize;
    private float _crowdEnergy;
    private ConcertStatus _status;
    private int _expEarned;
    private int _moneyEarned;
    private bool _notificationSent;
    private int _confettiCount;

    public Concert(Artist artist, City city, Venue venue, List<Song> setlist, GameDateTime concertTime, List<Equipment> equipmentBought)
    {
        _artist = artist;
        _city = city;
        _venue = venue;
        _setlist = setlist;
        _concertTime = concertTime;
        _endTime = concertTime.GetAddHourTime(1);
        _lastSecurityEventTime = new GameDateTime(_concertTime);
        _equipmentBought = equipmentBought;
        _currentEvent = null;
        _completedEvents = new List<Event>();
        _crowdSize = 0;
        _crowdEnergy = 100;
        _status = ConcertStatus.Scheduled;
        _expEarned = 0;
        _moneyEarned = 0;
        _confettiCount = 3;
    }

    public void TriggerEvent()
    {
        _currentEvent = EventFactory.CreateEvent(this);

        if (_currentEvent != null)
        {
            if (_currentEvent is Security)
            {
                _lastSecurityEventTime = new GameDateTime(GameManager.Instance.CurrentGameTime);
            }
        }
    }

    public void UpdateCompletedEvents()
    {
        if (_currentEvent != null)
        {
            if (_currentEvent.Handled || _currentEvent.HasExpired())
            {
                _completedEvents.Add(_currentEvent);
                _currentEvent.Handle(this);
                _currentEvent = null;
            }
        }
    }

    public void UpdateCrowdEnergy()
    {
        bool hasLightStick = false;
        float baseDecayRate = Mathf.Lerp(0.1f, 1.6f, 1 - _city.GetMusicTaste(GetMainGenre()));

        foreach (Equipment e in _equipmentBought)
        {
            if (e.EquipmentType == EquipmentType.LightStick)
            {
                hasLightStick = true;
            }
        }

        if (hasLightStick)
        {
            baseDecayRate *= 0.5f;
        }

        if (_artist.IsOnBreak)
        {
            baseDecayRate *= 2f; 
        }

        _crowdEnergy -= baseDecayRate;
        Debug.Log($"Updated crowd energy: {_crowdEnergy}");
    }

    public void MarkAsMissed()
    {
        _status = ConcertStatus.Missed;
        _artist.DecreaseFame();
        _crowdEnergy = 0;
    }

    public List<Song> Setlist
    {
        get { return _setlist; }
    }

    public int ConfettiCount
    {
        get { return _confettiCount; }
        set { _confettiCount = value; }
    }

    public bool NotificationSent
    {
        get { return _notificationSent; }
        set { _notificationSent = value; }
    }

    public GameDateTime LastSecurityEventTime
    {
        get { return _lastSecurityEventTime; }
    }

    public List<Event> CompletedEvents
    {
        get { return _completedEvents; }
        set { _completedEvents = value; }
    }

    public Event CurrentEvent
    {
        get { return _currentEvent; }
    }

    public int CrowdSize
    {
        get { return _crowdSize; }
        set { _crowdSize = value; }
    }

    public float CrowdEnergy
    {
        get { return _crowdEnergy; }
        set { _crowdEnergy = value; }
    }

    public Artist Artist
    {
        get { return _artist; }
    }

    public GameDateTime ConcertTime
    {
        get { return _concertTime; }
    }

    public GameDateTime EndTime
    {
        get { return _endTime; }
        set { _endTime = value; }
    }

    public Venue Venue
    {
        get { return _venue; }
    }

    public City City
    {
        get { return _city; }
    }

    public int ExpEarned
    {
        get { return _expEarned; }
        set { _expEarned = value; } 
    }

    public int MoneyEarned
    {
        get { return _moneyEarned; }
        set { _moneyEarned = value; }
    }

    public ConcertStatus Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public List<Equipment> EquipmentBought
    {
        get { return _equipmentBought; } 
    }

    public bool CheckEquipmentBought(EquipmentType equipmentType)
    {
        foreach (Equipment e in _equipmentBought)
        {
            if (e.EquipmentType == equipmentType && e.Used == false)
            {
                return true;
            }
        }

        return false;
    }

    public int CalculateExpEarned()
    {
        int totalEventEXP = 0;
        foreach (Event e in _completedEvents)
        {
            totalEventEXP += e.ExpEffect;
        }

        foreach (Equipment eq in _equipmentBought)
        {
            totalEventEXP += eq.ExpEarned;
        }

        if (_status == ConcertStatus.Missed)
        {
            _expEarned = -(_crowdSize / 5) + totalEventEXP;
        }
        else
        {
            _expEarned = _artist.Fame / 2 + (int)(_crowdEnergy * 5) + _crowdSize / 5 + totalEventEXP;
        }
        
        return _expEarned;
    }

    public Genre GetMainGenre()
    {
        Dictionary<Genre, int> genreCount = new Dictionary<Genre, int>();

        foreach (Song song in _setlist)
        {
            if (genreCount.ContainsKey(song.SongGenre))
            {
                genreCount[song.SongGenre]++;
            }
            else
            {
                genreCount[song.SongGenre] = 1;
            }
        }

        Genre mainGenre = Genre.Pop;
        int maxCount = 0;

        foreach (KeyValuePair<Genre, int> genre in genreCount)
        {
            if (genre.Value > maxCount)
            {
                mainGenre = genre.Key;
                maxCount = genre.Value;
            }
        }

        return mainGenre;
    }

    public int CalculateCrowdSize()
    {
        float population = _city.Population * 0.02f;

        float genreImpact = Mathf.Pow(_city.GetMusicTaste(GetMainGenre()), 0.7f) * 0.8f + 0.2f;

        float fameFactor = 0.08f + (_artist.Fame / 5000f);

        float venueFactor = _venue.Type == VenueType.Indoor ? 0.85f : 1.15f;

        float randomFactor = Random.Range(0.85f, 1.15f);

        float rawAttendance = fameFactor * genreImpact * population * venueFactor * randomFactor;

        int crowdSize = Mathf.RoundToInt(rawAttendance);

        crowdSize = Mathf.Min(crowdSize, _venue.Capacity);

        _crowdSize = crowdSize;
        _city.UpdateMusicTaste(GetMainGenre());
        
        return _crowdSize;
    }

    public int CalculateMoney()
    {
        if (_crowdEnergy > 60)
        {
            _moneyEarned = _crowdSize / 4;
        }
        else if (_crowdEnergy > 30)
        {
            _moneyEarned = _crowdSize / 8;
        }
        else if (_crowdEnergy > 0)
        {
            _moneyEarned = _crowdSize / 10;
        }
        else
        {
            _moneyEarned = 0;
        }

        return _moneyEarned;
    }

    public int CalculateProfit()
    {
        int totalCost = 0;
        int profit;

        foreach (Equipment equipment in _equipmentBought)
        {
            totalCost += equipment.Cost;
        }

        profit = _moneyEarned - _venue.Cost - totalCost;
        
        return profit;
    }
}