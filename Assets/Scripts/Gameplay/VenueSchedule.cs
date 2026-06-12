using System;

public class VenueSchedule
{
    private ScheduleType _scheduleType;
    private GameDateTime _startTime;
    private GameDateTime _endTime;

    public VenueSchedule(ScheduleType scheduleType, GameDateTime startTime, GameDateTime endTime)
    {
        _scheduleType = scheduleType;
        _startTime = startTime;
        _endTime = endTime;
    }

    public GameDateTime StartTime
    {
        get { return _startTime; }
        set { _startTime = value; }
    }

    public GameDateTime EndTime
    {
        get { return _endTime; }
        set { _endTime = value; }
    }

    public ScheduleType ScheduleType
    {
        get { return _scheduleType; }
    }
}