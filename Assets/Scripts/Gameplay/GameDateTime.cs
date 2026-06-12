using UnityEngine;
using System;

[System.Serializable]
public class GameDateTime
{
    [SerializeField] private int _day;
    [SerializeField] private int _month;
    [SerializeField] private int _year;
    [SerializeField] private int _hour;
    [SerializeField] private int _minute;

    public GameDateTime(int day, int month, int year, int hour, int minute)
    {
        _day = day;
        _month = month;
        _year = year;
        _hour = hour;
        _minute = minute;
    }

    public GameDateTime(GameDateTime time)
    {
        _day = time._day;
        _month = time._month;
        _year = time._year;
        _hour = time._hour;
        _minute = time._minute;
    }

    public int Day
    {
        get { return _day; }
    }

    public int Month
    {
        get { return _month; }
    }

    public int Year
    {
        get { return _year; }
    }

    public int Hour
    {
        get { return _hour; }
    }

    public int Minute
    {
        get { return _minute; }
    }

    public int GetDaysInMonth(int month, int year)
    {
        int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        if (month == 2 && ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)))
        {
            return 29;
        }
            
        return daysInMonth[month - 1];
    }

    public void UpdateTime()
    {
        _minute += 1;

        if (_minute >= 60)
        {
            _minute = 0;
            _hour += 1;
        }

        if (_hour >= 24)
        {
            _hour = 0;
            _day += 1;
        }

        if (_day > GetDaysInMonth(_month, _year))
        {
            _day = 1;
            _month += 1;
        }

        if (_month > 12)
        {
            _month = 1;
            _year += 1;
        }
    }

    public void AddHours(int hours)
    {
        _hour += hours;
        while (_hour >= 24)
        {
            _hour -= 24;
            _day += 1;

            if (_day > GetDaysInMonth(_month, _year))
            {
                _day = 1;
                _month += 1;
            }

            if (_month > 12)
            {
                _month = 1;
                _year += 1;
            }
        }
    }

    public void AddMinutes(int minutes)
    {
        _minute += minutes;
        while (_minute >= 60)
        {
            _minute -= 60;
            _hour += 1;

            AddHours(0);
        }
    }

    public GameDateTime GetSubtractMinuteTime(int minute)
    {
        int newMinute = _minute - minute;
        int newHour = _hour;

        while (newMinute < 0)
        {
            newMinute += 60;
            newHour -= 1;
        }

        return new GameDateTime(_day, _month, _year, newHour, newMinute);
    }

    public GameDateTime GetAddMinuteTime(int minute)
    {
        int newMinute = _minute + minute;
        int newHour = _hour;
        
        while (newMinute >= 60) 
        {
            newMinute -= 60;
            newHour += 1;
        }

        return new GameDateTime(_day, _month, _year, newHour, newMinute);
    }

    public GameDateTime GetAddHourTime(int hours)
    {
        return new GameDateTime(_day, _month, _year, _hour + hours, _minute);
    }

    public string DisplayInString()
    {
        return $"{_day}/{_month}/{_year} {(_hour < 10 ? "0" : "")}{_hour}:{(_minute < 10 ? "0" : "")}{_minute}";
    }

    public string DisplayTimeInString()
    {
        return $"{(_hour < 10 ? "0" : "")}{_hour}:{(_minute < 10 ? "0" : "")}{_minute}";
    }

    public bool IsTimeBetween(GameDateTime startTime, GameDateTime endTime)
    {
        int currentTotalMinutes = (_year * 525600) + (_month * 43200) + (_day * 1440) + (_hour * 60) + _minute;

        int startTotalMinutes = (startTime._year * 525600) + (startTime._month * 43200) + (startTime._day * 1440) + (startTime._hour * 60) + startTime._minute;

        int endTotalMinutes = (endTime._year * 525600) + (endTime._month * 43200) + (endTime._day * 1440) + (endTime._hour * 60) + endTime._minute;

        return currentTotalMinutes >= startTotalMinutes && currentTotalMinutes <= endTotalMinutes;
    }

    public int GetDaysBetween(GameDateTime endDate)
    {
        DateTime start = new DateTime(_year, _month, _day);
        DateTime end = new DateTime(endDate._year, endDate._month, endDate._day);

        return (end - start).Days;
    }

    public int GetMinutesBetween(GameDateTime otherTime)
    {
        DateTime thisDateTime = new DateTime(_year, _month, _day, _hour, _minute, 0);
        DateTime otherDateTime = new DateTime(otherTime._year, otherTime._month, otherTime._day, otherTime._hour, otherTime._minute, 0);

        return (int)(thisDateTime - otherDateTime).TotalMinutes;
    }

    public bool Overlap(GameDateTime time)
    {
        DateTime thisDateTime = new DateTime(_year, _month, _day, _hour, _minute, 0);
        DateTime otherDateTime = new DateTime(time._year, time._month, time._day, time._hour, time._minute, 0);

        TimeSpan concertDuration = TimeSpan.FromHours(2);

        DateTime thisEndTime = thisDateTime + concertDuration;
        DateTime otherEndTime = otherDateTime + concertDuration;

        return thisDateTime < otherEndTime && thisEndTime > otherDateTime;
    }
}