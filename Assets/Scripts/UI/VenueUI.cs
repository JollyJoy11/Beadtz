using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class VenueUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Venue venue;
    [SerializeField] private GameObject venuePrefab;
    [SerializeField] private GameObject cityDisplay;
    [SerializeField] private Button backgroundCloseButton;
    [SerializeField] private GameObject popup;

    void Start()
    {
        backgroundCloseButton.onClick.AddListener(CloseAll);
        backgroundCloseButton.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI Image Clicked: " + gameObject.name);

        if (venue == null) return;

        foreach (City c in GameManager.Instance.Cities)
        {
            foreach (Venue v in c.Venues)
            {
                if (v == venue)
                {
                    Debug.Log("City Clicked: " + c.CityName);
                    Debug.Log("Venue Clicked: " + v.VenueName);
                    DisplayVenue(c);

                    if (DataCarrier.Instance != null)
                    {
                        DataCarrier.Instance.SetSelectedLocation(c, v);
                    }
                    return;
                }
            }
        }
    }

    public void DisplaySchedule(GameDateTime time)
    {
        Transform constructionTransform = transform.Find("ConstructionImage");
        Transform cleaningTransform = transform.Find("CleaningImage");

        if (constructionTransform != null)
        {
            Image constructionImage = constructionTransform.GetComponent<Image>();
            Image cleaningImage = cleaningTransform.GetComponent<Image>();

            foreach (VenueSchedule v in venue.Schedules)
            {
                if (v.ScheduleType == ScheduleType.Renovation && time.IsTimeBetween(v.StartTime, v.EndTime))
                {
                    constructionImage.gameObject.SetActive(true);
                    cleaningImage.gameObject.SetActive(false);
                }
                else if (v.ScheduleType == ScheduleType.Cleanup && time.IsTimeBetween(v.StartTime.GetAddMinuteTime(1), v.EndTime))
                {
                    cleaningImage.gameObject.SetActive(true);
                    constructionImage.gameObject.SetActive(false);
                }
                else
                {
                    constructionImage.gameObject.SetActive(false);
                    cleaningImage.gameObject.SetActive(false);
                }
            }
        }
    }

    public void DisplayVenue(City c)
    {
        venuePrefab.SetActive(true);
        venuePrefab.transform.Find("Image").GetComponent<Image>().sprite = venue.VenueImage;
        venuePrefab.transform.Find("VenueName").GetComponent<TMP_Text>().text = venue.VenueName;
        venuePrefab.transform.Find("VenueType").GetComponent<TMP_Text>().text = $"Venue Type: {venue.Type}";
        venuePrefab.transform.Find("Capacity").GetComponent<TMP_Text>().text = $"Capacity: {venue.Capacity}";
        venuePrefab.transform.Find("Cost").GetComponent<TMP_Text>().text = $"Cost: {venue.Cost}";

        if (!venue.IsBought)
        {
            venuePrefab.transform.Find("BuyButton").gameObject.SetActive(true);
            venuePrefab.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => BuyVenue());
        }
        else
        {
            venuePrefab.transform.Find("BuyButton").gameObject.SetActive(false);
        }

        cityDisplay.SetActive(true);
        cityDisplay.transform.Find("CityName").GetComponent<TMP_Text>().text = c.CityName;
        cityDisplay.transform.Find("Population").GetComponent<TMP_Text>().text = $"Population: {c.Population}";

        StringBuilder genreText = new StringBuilder();
        foreach (GenreDict g in c.CrowdMusicTaste)
        {
            genreText.Append($"{g.Genre}: {g.Preference.ToString("F2")}\n");
        }
        cityDisplay.transform.Find("GenreDict").GetComponent<TMP_Text>().text = genreText.ToString();

        backgroundCloseButton.gameObject.SetActive(true);
    }

    public void BuyVenue()
    {
        if (GameManager.Instance.Player.BuyVenue(venue))
        {
            venuePrefab.transform.Find("BuyButton").gameObject.SetActive(false);
            venuePrefab.transform.Find("BuyingSound").GetComponent<AudioSource>().Play();
            FindFirstObjectByType<UIManager>().UpdatePlayerStats(GameManager.Instance.Player);
        }
        else
        {
            venuePrefab.transform.Find("ErrorSound").GetComponent<AudioSource>().Play();
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Insufficient Balance";
        }
    }

    void CloseAll()
    {
        venuePrefab.SetActive(false);
        cityDisplay.SetActive(false);
        backgroundCloseButton.gameObject.SetActive(false);

        if (DataCarrier.Instance != null)
        {
            DataCarrier.Instance.UnSetLocation();
        }
    }
}