using UnityEngine;
using System;

[System.Serializable]
public class Song
{
    [SerializeField] private string _songName;
    [SerializeField] private Genre _songGenre;
    [SerializeField] private Sprite _songImage;

    public Song(string songName, Genre songGenre, Sprite songImage)
    {
        _songName = songName;
        _songGenre = songGenre;
        _songImage = songImage;
    }

    public Song()
    {
        _songName = "ABC";
        _songGenre = Genre.Pop;
        _songImage = null;
    }

    public string SongName
    {
        get { return _songName; }
    }

    public Genre SongGenre
    {
        get { return _songGenre; }
    }
    
    public Sprite SongImage
    {
        get { return _songImage; }
    }
}
