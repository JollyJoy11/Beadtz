using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class ArtistDetailUI : MonoBehaviour
{
    [Header("Artist Details")]
    [SerializeField] private Image _artistImage;
    [SerializeField] private TMP_Text _artistName;
    [SerializeField] private TMP_Text _members;
    [SerializeField] private TMP_Text _fame;
    [SerializeField] private TMP_Text _concertsHeld;
    [SerializeField] private TMP_Text _energy;
    [SerializeField] private TMP_Text _break;

    [Header("Artists List Display")]
    [SerializeField] private GameObject schedulePrefab;
    [SerializeField] private Transform schedulecontentPanel;

    [Header("Artists List")]
    [SerializeField] private GameObject concertList;
    [SerializeField] private GameObject songList;
    private bool songListPopulated = false;
    private bool concertListPopulated = false;

    [Header("Plan Concert")]
    [SerializeField] private GameObject onBreakPopup;

    void Start()
    {
        GameObject concertButton = GameObject.Find("ConcertScheduleButton");
        GameObject songButton = GameObject.Find("SongButton");

        if (concertButton != null) concertButton.GetComponent<Button>().onClick.AddListener(SwitchList);
        if (songButton != null) songButton.GetComponent<Button>().onClick.AddListener(SwitchList);
    }

    public void PopulateSchedules(Artist artist, List<Concert> concerts)
    {
        int count = 0;
        foreach (Concert concert in concerts)
        {
            if (concert.Artist == artist)
            {
                GameObject scheduleUI = Instantiate(schedulePrefab, schedulecontentPanel);

                scheduleUI.transform.Find("ConcertDate").GetComponent<TMP_Text>().text = concert.ConcertTime.DisplayInString();

                scheduleUI.transform.Find("ConcertVenue").GetComponent<TMP_Text>().text = $"{concert.Venue.VenueName}, {concert.City.CityName}";

                scheduleUI.transform.Find("ConcertStatus").GetComponent<TMP_Text>().text = concert.Status.ToString();

                if (concert.Status == ConcertStatus.Finished)
                {
                    count++;
                }

            }
        }
        _concertsHeld.text = $"Concerts Held: {count}";
    }

    public void DisplayArtistInfo(Artist artist)
    {
        _artistImage.sprite = artist.ArtistImage;
        _artistName.text = artist.ArtistName;

        if (artist is Group group)
        {
            _members.text = string.Join(", ", group.Members);
        }
        else
        {
            _members.gameObject.SetActive(false);
        }

        _fame.text = $"Fame: {artist.Fame}";
        _energy.text = $"Energy: {artist.Energy.ToString("F2")}";

        if (!artist.IsOnBreak)
        {
            _break.gameObject.SetActive(false);
        }
        else
        {
            _break.text = "Currently On Break!";
        }

        concertList.SetActive(true);
        if (!concertListPopulated)
        {
            PopulateSchedules(DataCarrier.Instance.SelectedArtist, GameManager.Instance.Player.Concerts);
            concertListPopulated = true;
        }
    }

    public void SwitchList()
    {
        concertList.SetActive(false);
        songList.SetActive(false);

        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton == null)
        {
            Debug.LogError("No button is selected—SwitchList() was called incorrectly!");
            return;
        }

        if (clickedButton.name == "ConcertScheduleButton")
        {
            concertList.SetActive(true);

            if (!concertListPopulated)
            {
                PopulateSchedules(DataCarrier.Instance.SelectedArtist, GameManager.Instance.Player.Concerts);
                concertListPopulated = true;
            }
        }
        else if (clickedButton.name == "SongButton")
        {
            songList.SetActive(true);

            if (!songListPopulated)
            {
                FindFirstObjectByType<DisplaySongsUI>().PopulateSongs(DataCarrier.Instance.SelectedArtist, false);
                songListPopulated = true;
            }
        }
    }
    
    public void PlanConcertScene()
    {
        if (!DataCarrier.Instance.SelectedArtist.IsOnBreak)
        {
            SceneManager.LoadScene("PlanConcertMap");
        }
        else
        {
            onBreakPopup.gameObject.SetActive(true);
            onBreakPopup.transform.Find("Text").GetComponent<TMP_Text>().text = $"{DataCarrier.Instance.SelectedArtist.ArtistName} is currently on break!";
        }
    }
}