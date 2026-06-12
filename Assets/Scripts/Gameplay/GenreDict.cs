using UnityEngine;
using System;

[System.Serializable]
public class GenreDict
{
    [SerializeField] private Genre _genre;
    [SerializeField] private float _preference;

    public GenreDict(Genre genre, float preference)
    {
        _genre = genre;
        _preference = preference;
    }

    public Genre Genre
    {
        get { return _genre; }
    }

    public float Preference
    {
        get { return _preference; }
        set { _preference = value; }
    }
}