using System.Linq;
using UnityEngine;

public static class EventFactory
{
    public static Event CreateEvent(Concert concert)
    {
        GameDateTime now = GameManager.Instance.CurrentGameTime;
        GameDateTime encoreTriggerTime = concert.EndTime.GetSubtractMinuteTime(8);

        if (now.IsTimeBetween(encoreTriggerTime, concert.EndTime)
            && concert.CrowdEnergy > 85
            && !concert.CompletedEvents.OfType<Encore>().Any()
            && !concert.Venue.CheckScheduleAfter(concert.EndTime.GetAddMinuteTime(10)))
        {
            return new Encore();
        }

        if (concert.Artist.Energy < 20
            && !concert.CompletedEvents.OfType<ArtistExhausted>().Any())
        {
            return new ArtistExhausted();
        }

        if (concert.CrowdEnergy < 30
            && !concert.CompletedEvents.OfType<CrowdLoseEnergy>().Any())
        {
            return new CrowdLoseEnergy();
        }

        float securityRate = concert.Venue.Type == VenueType.Outdoor ? 0.09f : 0.05f;
        if (Random.value < securityRate
            && now.GetMinutesBetween(concert.LastSecurityEventTime) >= 5
            && !now.IsTimeBetween(concert.EndTime.GetSubtractMinuteTime(5), concert.EndTime))
        {
            return new Security();
        }

        float fanRushRate = concert.Venue.Type == VenueType.Outdoor ? 0.06f : 0.02f;
        if (Random.value < fanRushRate
            && !concert.CompletedEvents.OfType<FanRushesStage>().Any()
            && concert.CrowdEnergy > 50)
        {
            return new FanRushesStage();
        }

        return null;
    }
}
