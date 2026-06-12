using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScheduleUI : MonoBehaviour
{
    [SerializeField] private GameObject artistSchedulePrefab;
    [SerializeField] private Transform artistSchedulecontentPanel;
    [SerializeField] private GameObject venueSchedulePrefab;
    [SerializeField] private Transform venueSchedulecontentPanel;

    public void PopulateArtistSchedule(Artist artist, List<Concert> concerts)
    {
        foreach (Concert concert in concerts)
        {
            if (concert.Artist == artist && (concert.Status == ConcertStatus.Ongoing || concert.Status == ConcertStatus.Scheduled))
            {
                GameObject artistScheduleUI = Instantiate(artistSchedulePrefab, artistSchedulecontentPanel);

                artistScheduleUI.transform.Find("ConcertDate").GetComponent<TMP_Text>().text = $"{concert.ConcertTime.DisplayInString()}-{concert.EndTime.DisplayTimeInString()}";

                artistScheduleUI.transform.Find("ConcertVenue").GetComponent<TMP_Text>().text = $"{concert.Venue.VenueName}, {concert.City.CityName}";
            }
        }
    }

    public void PopulateVenueSchedule(Venue venue, GameDateTime time)
    {
        foreach (VenueSchedule v in venue.Schedules)
        {
            if (v.StartTime.GetMinutesBetween(time) > 0 || time.IsTimeBetween(v.StartTime, v.EndTime))
            {
                GameObject venueScheduleUI = Instantiate(venueSchedulePrefab, venueSchedulecontentPanel);

                venueScheduleUI.transform.Find("ScheduleType").GetComponent<TMP_Text>().text = v.ScheduleType.ToString();

                venueScheduleUI.transform.Find("ScheduleDate").GetComponent<TMP_Text>().text = $"{v.StartTime.DisplayInString()}-{v.EndTime.DisplayTimeInString()}";
            }
        }
    }
}