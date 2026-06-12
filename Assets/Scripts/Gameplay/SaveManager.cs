using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath => Application.persistentDataPath + "/savefile.json";

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

    void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            exp = GameManager.Instance.Player.EXP,
            money = GameManager.Instance.Player.Money,
            currentLevel = LevelManager.Instance.CurrentLevelIndex(GameManager.Instance.Player),
            currentGameTime = GameManager.Instance.CurrentGameTime,

            unlockedArtistData = GameManager.Instance.Player.ArtistsUnlocked.Select(artist => new ArtistSaveData
            {
                artistName = artist.ArtistName,
                fame = artist.Fame,
                energy = artist.Energy,
                isOnBreak = artist.IsOnBreak,
                breakStartTime = artist.BreakStartTime
            }).ToList(),

            cityData = GameManager.Instance.Cities.Where(city => city.Unlocked).Select(city => new CitySaveData
            {
                cityName = city.CityName,

                crowdMusicTaste = city.CrowdMusicTaste.Select(taste => new GenrePreferenceSaveData
                {
                    genreName = taste.Genre.ToString(),
                    preference = taste.Preference
                }).ToList(),

                venueData = city.Venues.Where(venue => venue.IsBought).Select(venue => new VenueSaveData
                {
                    venueName = venue.VenueName,
                    concertCount = venue.ConcertCount,

                    schedules = venue.Schedules.Select(venueSchedule => new VenueScheduleSaveData
                    {
                        scheduleType = venueSchedule.ScheduleType.ToString(),
                        startTime = venueSchedule.StartTime,
                        endTime = venueSchedule.EndTime
                    }).ToList()
                }).ToList()
            }).ToList(),

            concertData = GameManager.Instance.Player.Concerts.Select(concert => new ConcertSaveData
            {
                artistName = concert.Artist.ArtistName,
                cityName = concert.City.CityName,
                venueName = concert.Venue.VenueName,
                setlistSongNames = concert.Setlist.Select(song => song.SongName).ToList(),
                concertTime = concert.ConcertTime,
                equipmentBought = concert.EquipmentBought.Select(equipment => new EquipmentSaveData
                {
                    equipmentName = equipment.EquipmentType.ToString(),
                    used = equipment.Used
                }).ToList(),

                completedEvents = concert.CompletedEvents.Select(events => new EventSaveData
                {
                    eventType = events.GetType().Name,
                    expEffect = events.ExpEffect,
                    handled = events.Handled
                }).ToList(),

                crowdSize = concert.CrowdSize,
                crowdEnergy = concert.CrowdEnergy,
                status = concert.Status.ToString(),
                expEarned = concert.ExpEarned,
                moneyEarned = concert.MoneyEarned,
                notificationSent = concert.NotificationSent,
                confettiCount = concert.ConfettiCount
            }).ToList()
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            Debug.Log($"Save file path: {Application.persistentDataPath}/savefile.json");
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            GameManager.Instance.Player.EXP = saveData.exp;
            GameManager.Instance.Player.Money = saveData.money;
            GameManager.Instance.Player.CurrentLevel = LevelManager.Instance.Levels[saveData.currentLevel];
            GameManager.Instance.CurrentGameTime = new GameDateTime(saveData.currentGameTime);

            for (int i = 0; i <= saveData.currentLevel; i++)
            {
                foreach (Artist artist in LevelManager.Instance.Levels[i].ArtistsToUnlock)
                {
                    GameManager.Instance.Player.ArtistsUnlocked.Add(artist);

                    ArtistSaveData artistData = saveData.unlockedArtistData.FirstOrDefault(a => a.artistName == artist.ArtistName);

                    if (artistData != null)
                    {
                        artist.Fame = artistData.fame;
                        artist.Energy = artistData.energy;
                        artist.IsOnBreak = artistData.isOnBreak;
                        artist.BreakStartTime = artistData.breakStartTime;
                    }
                }
            }

            foreach (City city in GameManager.Instance.Cities)
            {
                CitySaveData cityData = saveData.cityData.FirstOrDefault(c => c.cityName == city.CityName);

                if (cityData != null)
                {
                    city.Unlocked = true;

                    foreach (GenrePreferenceSaveData tasteData in cityData.crowdMusicTaste)
                    {
                        GenreDict genreEntry = city.CrowdMusicTaste.FirstOrDefault(g => g.Genre.ToString() == tasteData.genreName);

                        if (genreEntry != null)
                        {
                            genreEntry.Preference = tasteData.preference;
                        }
                    }

                    foreach (Venue venue in city.Venues)
                    {
                        VenueSaveData venueData = cityData.venueData.FirstOrDefault(v => v.venueName == venue.VenueName);

                        if (venueData != null)
                        {
                            venue.IsBought = true;
                            venue.ConcertCount = venueData.concertCount;

                            foreach (VenueScheduleSaveData venueScheduleData in venueData.schedules)
                            {
                                venue.AddSchedule(new VenueSchedule(Enum.Parse<ScheduleType>(venueScheduleData.scheduleType), venueScheduleData.startTime, venueScheduleData.endTime));
                            }
                        }
                    }
                }
            }

            foreach (ConcertSaveData concertData in saveData.concertData)
            {
                Artist a = GameManager.Instance.Player.ArtistsUnlocked.FirstOrDefault(artist => artist.ArtistName == concertData.artistName);
                City c = GameManager.Instance.Cities.FirstOrDefault(city => city.CityName == concertData.cityName);
                Venue v = c.Venues.FirstOrDefault(venue => venue.VenueName == concertData.venueName);

                List<Song> setlist = new List<Song>();
                foreach (string s in concertData.setlistSongNames)
                {
                    Song song = a.Songs.FirstOrDefault(song => song.SongName == s);

                    if (song != null)
                    {
                        setlist.Add(song);
                    }
                }

                List<Equipment> equipments = new List<Equipment>();
                foreach (EquipmentSaveData e in concertData.equipmentBought)
                {
                    Equipment equipment = GameManager.Instance.SearchEquipment(Enum.Parse<EquipmentType>(e.equipmentName));
                    if (equipment != null)
                    {
                        equipment.Used = e.used;
                        equipments.Add(equipment);
                    }
                }

                Concert concert = new Concert(a, c, v, setlist, concertData.concertTime, equipments);

                foreach (EventSaveData eventData in concertData.completedEvents)
                {
                    Event completedEvent;
                    if (eventData.eventType == "CrowdLoseEnergy")
                    {
                        completedEvent = new CrowdLoseEnergy();
                    }
                    else if (eventData.eventType == "Encore")
                    {
                        completedEvent = new Encore();
                    }
                    else if (eventData.eventType == "Security")
                    {
                        completedEvent = new Security();
                    }
                    else if (eventData.eventType == "FanRushesStage")
                    {
                        completedEvent = new FanRushesStage();
                    }
                    else if (eventData.eventType == "ArtistExhausted")
                    {
                        completedEvent = new ArtistExhausted();
                    }
                    else
                    {
                        completedEvent = null;
                    }

                    if (completedEvent != null)
                    {
                        completedEvent.ExpEffect = eventData.expEffect;
                        completedEvent.Handled = eventData.handled;
                        concert.CompletedEvents.Add(completedEvent);
                    }
                }

                concert.CrowdSize = concertData.crowdSize;
                concert.CrowdEnergy = concertData.crowdEnergy;
                concert.Status = Enum.Parse<ConcertStatus>(concertData.status);
                concert.ExpEarned = concertData.expEarned;
                concert.MoneyEarned = concertData.moneyEarned;
                concert.NotificationSent = concertData.notificationSent;
                concert.ConfettiCount = concertData.confettiCount;

                GameManager.Instance.Player.Concerts.Add(concert);
            }
        }
        else
        {
            GameManager.Instance.CurrentGameTime = new GameDateTime(1, 1, 2010, 12, 0);
            GameManager.Instance.Player.LevelUpReward();
        }
    }
}