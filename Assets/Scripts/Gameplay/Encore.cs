using UnityEngine;
using System;

public class Encore : Event
{
    public Encore() : base()
    {
        ExpEffect = 400;
    }

    public override void Handle(Concert concert)
    {
        if (!_handled)
        {
            ExpEffect = 0;
        }
        else
        {
            VenueSchedule cleaningSchedule = concert.Venue.SearchCleaningSchedule(concert.EndTime);
            concert.EndTime.AddMinutes(5);

            if (cleaningSchedule != null)
            {
                cleaningSchedule.StartTime = concert.EndTime; 
                cleaningSchedule.EndTime = cleaningSchedule.StartTime.GetAddMinuteTime(5);
            }

            concert.CrowdEnergy += 5;
            concert.CrowdEnergy = Mathf.Clamp(concert.CrowdEnergy, 0f, 100f);
        }
    }
}