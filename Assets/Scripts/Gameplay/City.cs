using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "City", menuName = "Beadtz/City")]
public class City : ScriptableObject
{
    [SerializeField] private string _cityName;
    [SerializeField] private int _population;
    [SerializeField] private GenreDict[] _crowdMusicTaste;
    [SerializeField] private List<Venue> _venues;
    private bool _unlocked = false;

    public bool Unlocked
    {
        get { return _unlocked; }
        set { _unlocked = value; }
    }
    
    public int Population
    {
        get { return _population; }
    }

    public GenreDict[] CrowdMusicTaste
    {
        get { return _crowdMusicTaste; }
        set { _crowdMusicTaste = value; }
    }

    public string CityName
    {
        get { return _cityName; }
    }

    public List<Venue> Venues
    {
        get { return _venues; }
    }

    public float GetMusicTaste(Genre genre)
    {
        foreach (GenreDict g in _crowdMusicTaste)
        {
            if (g.Genre == genre)
            {
                return g.Preference;
            }
        }

        return 0;
    }

    public void UpdateMusicTaste(Genre genre)
    {
        foreach (GenreDict g in _crowdMusicTaste) {
            if (g.Genre == genre)
            {
                float increaseAmount = 0.1f * (1 - g.Preference); 
                g.Preference = Mathf.Min(1, g.Preference + increaseAmount);
            }
            else
            {
                g.Preference = Mathf.Max(0, g.Preference * 0.99f);
            }
        }
    }
}