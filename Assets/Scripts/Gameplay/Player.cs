using System.Collections.Generic;
using UnityEngine;
using System;

public class Player
{
    private int _exp;
    private int _money;
    private GameLevel _currentLevel;
    private List<Artist> _artistsUnlocked;
    private List<Concert> _concerts;

    public Player()
    {
        _exp = 0;
        _money = 0;
        _currentLevel = LevelManager.Instance.Levels[0];
        _artistsUnlocked = new List<Artist>();
        _concerts = new List<Concert>();
    }

    public int Money
    {
        get { return _money; }
        set { _money = value; }
    }

    public GameLevel CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }

    public List<Artist> ArtistsUnlocked
    {
        get { return _artistsUnlocked; }
    }

    public int EXP
    {
        get { return _exp; }
        set { _exp = value; }
    }

    public List<Concert> Concerts
    {
        get { return _concerts; }
        set { _concerts = value; }
    }

    public void LevelUpReward()
    {
        foreach (Artist a in _currentLevel.ArtistsToUnlock)
        {
            _artistsUnlocked.Add(a);
        }

        _currentLevel.CityToUnlock.Unlocked = true;
        AddMoney(_currentLevel.RewardCoin);
    }

    public void AddEXP(int exp)
    {
        _exp = Mathf.Max(0, _exp + exp);

        if (LevelManager.Instance.LevelUp(this))
        {
            LevelUpReward();
        }
    }

    public void AddMoney(int money)
    {
        _money += money;
    }

    public void PlanConcert(Artist artist, City city, Venue venue, List<Song> setlist, GameDateTime concertTime, List<Equipment> equipmentBought)
    {
        Concert newConcert = new Concert(artist, city, venue, setlist, concertTime, equipmentBought);
        _concerts.Add(newConcert);

        VenueSchedule newConcertSchedule = new VenueSchedule(ScheduleType.Concerts, newConcert.ConcertTime, newConcert.EndTime);
        newConcert.Venue.AddSchedule(newConcertSchedule);

        VenueSchedule newCleaningSchedule = new VenueSchedule(ScheduleType.Cleanup, newConcert.EndTime, newConcert.EndTime.GetAddMinuteTime(5));
        newConcert.Venue.AddSchedule(newCleaningSchedule);
    }

    public void ViewConcertEvaluation(Concert concert)
    {
        concert.Status = ConcertStatus.Evaluated;

        concert.Artist.IncreaseFame(concert.CalculateCrowdSize(), concert.CrowdEnergy);
        AddMoney(concert.CalculateMoney());
        AddEXP(concert.CalculateExpEarned());
    }

    public int ConsecutiveConcertsCount(string artistName)
    {
        int count = 0;

        for (int i = _concerts.Count - 1; i >= 0; i--)
        {
            if (_concerts[i].Status == ConcertStatus.Evaluated &&
                _concerts[i].Artist.ArtistName == artistName)
            {
                count++;
            }
            else
            {
                break;
            }
        }

        return count;
    }

    public GameDateTime SearchLastConcertTime(string artist)
    {
        for (int i = _concerts.Count - 1; i >= 0; i--)
        {
            if (_concerts[i].Artist.ArtistName == artist && _concerts[i].Status == ConcertStatus.Evaluated)
            {
                return _concerts[i].EndTime;
            }
        }

        return null;
    }

    public bool BuyVenue(Venue v)
    {
        if (_money > v.Cost)
        {
            v.IsBought = true;
            _money -= v.Cost;
        }

        return v.IsBought;
    }
}