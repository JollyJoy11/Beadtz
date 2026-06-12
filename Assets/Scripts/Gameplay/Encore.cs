using UnityEngine;

public class Encore : Event
{
    public Encore() : base()
    {
        _duration = 8f;
        Description = "The crowd is chanting for an encore!";
        ChoiceALabel = "Do It!";
        ChoiceBLabel = "Wrap Up";
        ChoiceATradeoff = "Crowd energy +5, artist tires faster";
        ChoiceBTradeoff = "Artist energy preserved, fame -5";
        ExpEffect = 400;
    }

    public override void Handle(Concert concert)
    {
        if (_choiceMade == EventChoice.A)
        {
            VenueSchedule cleaningSchedule = concert.Venue.SearchCleaningSchedule(concert.EndTime);
            concert.EndTime.AddMinutes(5);

            if (cleaningSchedule != null)
            {
                cleaningSchedule.StartTime = concert.EndTime;
                cleaningSchedule.EndTime = cleaningSchedule.StartTime.GetAddMinuteTime(5);
            }

            concert.CrowdEnergy = Mathf.Clamp(concert.CrowdEnergy + 5f, 0f, 100f);
            concert.Artist.Energy -= 5f;
        }
        else
        {
            ExpEffect = 0;
            concert.Artist.Fame -= 5;
        }
    }
}