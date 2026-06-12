using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ConcertEvaluationUI : MonoBehaviour
{
    [SerializeField] private Image _artistImage;
    [SerializeField] private TMP_Text _artistName;
    [SerializeField] private TMP_Text _members;
    [SerializeField] private TMP_Text _date;
    [SerializeField] private TMP_Text _location;
    [SerializeField] private TMP_Text _crowdSize;
    [SerializeField] private TMP_Text _crowdEnergy;
    [SerializeField] private TMP_Text _setlist;
    [SerializeField] private TMP_Text _encore;
    [SerializeField] private TMP_Text _securityEvents;
    [SerializeField] private TMP_Text _exp;
    [SerializeField] private TMP_Text _money;
    [SerializeField] private TMP_Text _musicTaste;
    [SerializeField] private TMP_Text _fame;
    [SerializeField] private TMP_Text _profit;
    [SerializeField] private Image _approved;

    public void DisplayConcertInfo(Concert c)
    {
        _artistImage.sprite = c.Artist.ArtistImage;
        _artistName.text = c.Artist.ArtistName;

        if (c.Artist is Group group)
        {
            _members.text = string.Join(", ", group.Members);
        }
        else
        {
            _members.gameObject.SetActive(false);
        }

        _date.text = $"Date: {c.ConcertTime.DisplayInString()}";
        _location.text = $"Location: {c.Venue.VenueName}, {c.City.CityName}";
        _crowdSize.text = $"Crowd Size: {c.CrowdSize}";
        _crowdEnergy.text = $"Crowd Energy: {c.CrowdEnergy:F2}";
        _setlist.text = string.Join("\n", c.Setlist.Select(song => song.SongName));

        if (c.CompletedEvents.OfType<Encore>().Any())
        {
            _encore.text = "Encore: Yes";
        }
        else
        {
            _encore.text = "Encore: -";
        }

        _securityEvents.text = $"Security Events: {c.CompletedEvents.OfType<Security>().Count(e => e.Handled)}";

        _exp.text = $"EXP: {c.ExpEarned}";
        _money.text = $"Money: {c.MoneyEarned}";
        _musicTaste.text = $"Main Genre: {c.GetMainGenre()}";
        _fame.text = $"Fame: {c.Artist.Fame}";
        _profit.text = $"Profit: {c.CalculateProfit()}";

        if (c.CrowdEnergy > 60)
        {
            _approved.gameObject.SetActive(true);
        }
    }
}