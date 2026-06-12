using System.Linq;
using UnityEngine;

public static class EventFactory
{
    public static Event CreateEvent(Concert concert)
    {
        GameDateTime encoreTriggerTime = concert.EndTime.GetSubtractMinuteTime(8);

        if (GameManager.Instance.CurrentGameTime.IsTimeBetween(encoreTriggerTime, concert.EndTime) && concert.CrowdEnergy > 85 && !concert.CompletedEvents.OfType<Encore>().Any() && !concert.Venue.CheckScheduleAfter(concert.EndTime.GetAddMinuteTime(10)))
        {
            return new Encore();
        }
        else if (concert.CrowdEnergy < 30 && !concert.CompletedEvents.OfType<CrowdLoseEnergy>().Any())
        {
            return new CrowdLoseEnergy();
        }
        else
        {
            float securityRate;
            if (concert.Venue.Type == VenueType.Outdoor)
            {
                securityRate = 0.09f;
            }
            else
            {
                securityRate = 0.05f;
            }

            if (Random.value < securityRate && GameManager.Instance.CurrentGameTime.GetMinutesBetween(concert.LastSecurityEventTime) >= 5 && !GameManager.Instance.CurrentGameTime.IsTimeBetween(concert.EndTime.GetSubtractMinuteTime(5), concert.EndTime))
            {
                return new Security();
            }
        }

        return null;
    }
}