using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int exp;
    public int money;
    public int currentLevel;
    public GameDateTime currentGameTime;
    public List<ArtistSaveData> unlockedArtistData;
    public List<CitySaveData> cityData;
    public List<ConcertSaveData> concertData;
}

[System.Serializable]
public class ArtistSaveData
{
    public string artistName;
    public int fame;
    public float energy;
    public bool isOnBreak;
    public GameDateTime breakStartTime;
}

[System.Serializable]
public class CitySaveData
{
    public string cityName;
    public List<GenrePreferenceSaveData> crowdMusicTaste;
    public List<VenueSaveData> venueData;
}

[System.Serializable]
public class ConcertSaveData
{
    public string artistName;
    public string cityName;
    public string venueName;
    public List<string> setlistSongNames;
    public GameDateTime concertTime;
    public List<EquipmentSaveData> equipmentBought;
    public List<EventSaveData> completedEvents;
    public int crowdSize;
    public float crowdEnergy;
    public string status;
    public int expEarned;
    public int moneyEarned;
    public bool notificationSent;
    public int confettiCount;
}

[System.Serializable]
public class EquipmentSaveData
{
    public string equipmentName;
    public bool used;
}

[System.Serializable]
public class EventSaveData
{
    public string eventType;
    public int expEffect;
    public bool handled;
}

[System.Serializable]
public class GenrePreferenceSaveData
{
    public string genreName;  
    public float preference;
}

[System.Serializable]
public class VenueSaveData
{
    public string venueName;
    public int concertCount;
    public List<VenueScheduleSaveData> schedules;
}

[System.Serializable]
public class VenueScheduleSaveData
{
    public string scheduleType;
    public GameDateTime startTime;
    public GameDateTime endTime;
}