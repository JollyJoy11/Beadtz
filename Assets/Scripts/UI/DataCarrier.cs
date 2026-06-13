using System.Collections.Generic;
using UnityEngine;

public class DataCarrier : MonoBehaviour
{
    public static DataCarrier Instance;

    public Artist SelectedArtist;
    public City SelectedCity;
    public Venue SelectedVenue;
    public List<Song> SelectedSongs = new List<Song>();
    public GameDateTime SelectedTime;
    public List<Equipment> SelectedEquipment = new List<Equipment>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedLocation(City city, Venue venue)
    {
        SelectedCity = city;
        SelectedVenue = venue;
        Debug.Log("City set: " + SelectedCity);
    }

    public void UnSetLocation()
    {
        SelectedCity = null;
        SelectedVenue = null;
        Debug.Log("City removed");
    }

    public void SetSelectedArtist(Artist artist)
    {
        SelectedArtist = artist;
    }

    public void SetSelectedDate(GameDateTime time)
    {
        SelectedTime = time;
    }

    public void Reset()
    {
        SelectedCity = null;
        SelectedVenue = null;
        SelectedArtist = null;
        SelectedSongs = new List<Song>();
        SelectedTime = null;
        SelectedEquipment = new List<Equipment>();
    }
}
