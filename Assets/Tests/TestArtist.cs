using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;

[TestFixture]
public class TestArtist
{
    /// <summary>
    /// This is a method to test the abstract DecreaseEnergy method of a Group artist after two consecutive concerts
    /// </summary>
    [Test]
    public void TestGroupArtistDecreaseEnergy()
    {
        var levelManagerGO = new GameObject("LevelManager");
        var levelManager = levelManagerGO.AddComponent<LevelManager>();
        GameLevel gl1 = ScriptableObject.CreateInstance<GameLevel>();
        LevelManager.Instance = levelManager;
        levelManager.Levels = new List<GameLevel> { gl1 };

        Player player = new Player();
        Group g1 = ScriptableObject.CreateInstance<Group>();
        g1.Members = new List<string> { "Hailey", "Apple", "Peach" };
        player.ArtistsUnlocked.Add(g1);

        City c1 = ScriptableObject.CreateInstance<City>();
        Venue v1 = ScriptableObject.CreateInstance<Venue>();

        Song song1 = new Song();
        Song song2 = new Song();
        List<Song> songList = new List<Song> { song1, song2 };

        Concert concert1 = new Concert(g1, c1, v1, songList, new GameDateTime(1, 1, 2010, 12, 0), null);
        concert1.Status = ConcertStatus.Evaluated;

        Concert concert2 = new Concert(g1, c1, v1, songList, new GameDateTime(1, 2, 2010, 12, 0), null);
        concert2.Status = ConcertStatus.Evaluated;

        player.Concerts.Add(concert1);
        player.Concerts.Add(concert2);

        g1.DecreaseEnergy(player);

        Assert.AreEqual(Math.Round(g1.Energy, 2), 93.85);
    }

    /// <summary>
    /// This is a method to test the abstract DecreaseEnergy method of a Solo artist after two consecutive concerts
    /// </summary>
    [Test]
    public void TestSoloArtistDecreaseEnergy()
    {
        var levelManagerGO = new GameObject("LevelManager");
        var levelManager = levelManagerGO.AddComponent<LevelManager>();
        GameLevel gl1 = ScriptableObject.CreateInstance<GameLevel>();
        LevelManager.Instance = levelManager;
        levelManager.Levels = new List<GameLevel> { gl1 };

        Player player = new Player();
        Solo s1 = ScriptableObject.CreateInstance<Solo>();
        player.ArtistsUnlocked.Add(s1);

        City c1 = ScriptableObject.CreateInstance<City>();
        Venue v1 = ScriptableObject.CreateInstance<Venue>();

        Song song1 = new Song();
        Song song2 = new Song();
        List<Song> songList = new List<Song> { song1, song2 };

        Concert concert1 = new Concert(s1, c1, v1, songList, new GameDateTime(1, 1, 2010, 12, 0), null);
        concert1.Status = ConcertStatus.Evaluated;

        Concert concert2 = new Concert(s1, c1, v1, songList, new GameDateTime(1, 2, 2010, 12, 0), null);
        concert2.Status = ConcertStatus.Evaluated;

        player.Concerts.Add(concert1);
        player.Concerts.Add(concert2);

        s1.DecreaseEnergy(player);

        Assert.AreEqual(Math.Round(s1.Energy, 2), 83.52);
    }
}
