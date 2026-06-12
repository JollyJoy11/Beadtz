using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameDateTime _currentGameTime;
    private float _timeCounter = 0f;
    private Player _player;
    [SerializeField] private List<City> _cities;
    [SerializeField] private List<Equipment> _allEquipments;
    public static Concert EvaluatingConcert;

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

    private void Start()
    {
        _player = new Player();
        SaveManager.Instance.LoadGame();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "In Game")
        {
            _timeCounter += Time.deltaTime;

            if (_timeCounter >= 1f)
            {
                GenerateVenueSchedule();
                _currentGameTime.UpdateTime();
                FindFirstObjectByType<UIManager>().UpdateClock(_currentGameTime);
                _timeCounter = 0f;

                foreach (Concert c in _player.Concerts)
                {
                    c.UpdateCompletedEvents();

                    Debug.Log($"Checking {c.Artist.ArtistName} concert at {c.ConcertTime.DisplayInString()} {c.EndTime.DisplayInString()} {_currentGameTime.DisplayInString()} Status {c.Status} {_currentGameTime.IsTimeBetween(c.ConcertTime, c.EndTime)}");

                    if (_currentGameTime.IsTimeBetween(c.ConcertTime, c.EndTime) && c.Status == ConcertStatus.Scheduled)
                    {
                        TryStartConcert(c);
                    }
                    else if (c.Status == ConcertStatus.Ongoing)
                    {
                        c.UpdateCrowdEnergy();

                        if (c.CurrentEvent == null)
                        {
                            c.TriggerEvent();
                        }

                        if (c.CrowdEnergy <= 0)
                        {
                            c.Status = ConcertStatus.Finished;
                            c.EndTime = new GameDateTime(_currentGameTime);
                        }
                    }

                    if (!_currentGameTime.IsTimeBetween(c.ConcertTime, c.EndTime) && c.Status == ConcertStatus.Ongoing)
                    {
                        c.Status = ConcertStatus.Finished;
                        AudioManager.Instance.PlayClapSound();
                        c.Artist.DecreaseEnergy(_player);
                        c.Venue.ConcertCount++;
                    }
                }

                foreach (Artist a in _player.ArtistsUnlocked)
                {
                    a.RestoreEnergy(_currentGameTime, _player);
                }

                if (HasNewCompletedConcert())
                {
                    FindFirstObjectByType<NotificationUI>().PlayNotification();
                }

                foreach (VenueUI venueUI in FindObjectsOfType<VenueUI>(true))
                {
                    venueUI.DisplaySchedule(_currentGameTime);
                }
            }

            foreach (Concert c in _player.Concerts)
            {
                foreach (ConcertUI cu in FindObjectsOfType<ConcertUI>(true))
                {
                    bool isConcertOngoing = _player.Concerts.Exists(c => c.Venue == cu.Venue && c.Status == ConcertStatus.Ongoing);

                    if (isConcertOngoing)
                    {
                        cu.gameObject.SetActive(true);
                        Concert matchingConcert = _player.Concerts.Find(c => c.Venue == cu.Venue && c.Status == ConcertStatus.Ongoing);
                        cu.DisplayOngoingConcert(matchingConcert);
                    }
                    else
                    {
                        cu.gameObject.SetActive(false);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
    
    private bool HasNewCompletedConcert()
    {
        foreach (Concert concert in Player.Concerts)
        {
            if ((concert.Status == ConcertStatus.Finished || concert.Status == ConcertStatus.Missed) && !concert.NotificationSent)
            {
                concert.NotificationSent = true;
                return true;
            }
        }

        return false;
    }

    public void GenerateVenueSchedule()
    {
        foreach (City c in _cities)
        {
            foreach (Venue v in c.Venues)
            {
                if (v.ShouldRenovate())
                {
                    GameDateTime renovationStart = _currentGameTime.GetAddMinuteTime(5);
                    GameDateTime renovationEnd = _currentGameTime.GetAddMinuteTime(15);
                    
                    if (!v.CheckScheduleConflict(renovationStart, renovationEnd))
                    {
                        v.AddSchedule(new VenueSchedule(ScheduleType.Renovation, renovationStart, renovationEnd));
                        Debug.Log($"Renovation scheduled at {v.VenueName} during {renovationStart.DisplayInString()}!");
                        v.ConcertCount = 0;
                    }
                }
            }
        }
    }

    public void TryStartConcert(Concert concert)
    {
        foreach (Concert c in _player.Concerts)
        {
            if (c.Status == ConcertStatus.Ongoing && c.Artist == concert.Artist)
            {
                if (concert.ConcertTime.Overlap(c.ConcertTime))
                {
                    Debug.LogError($"Concert clash detected: {concert.ConcertTime.DisplayInString()} conflicts with {c.ConcertTime.DisplayInString()}");
                    concert.MarkAsMissed();
                    return;
                }
            }
        }

        concert.Status = ConcertStatus.Ongoing;
    }

    public GameDateTime CurrentGameTime
    {
        get { return _currentGameTime; }
        set { _currentGameTime = value; }
    }

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public Equipment SearchEquipment(EquipmentType equipmentType)
    {
        foreach (Equipment e in _allEquipments)
        {
            if (e.EquipmentType == equipmentType)
            {
                return e;
            }
        }

        return null;
    }

    public List<City> Cities
    {
        get { return _cities; }
    }
}
