using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ConcertUI : MonoBehaviour
{
    [SerializeField] private Venue _venue;
    private Image circleImage;
    private Button encoreButton;
    private Button securityButton;
    private Button concertButton;
    private Button lightShowButton;
    private Button fireworkButton;
    private Button confettiButton;
    private float currentFill = 1f;
    private Concert _currentConcert;

    void Start()
    {
        circleImage = transform.Find("ConcertTime").GetComponent<Image>();

        encoreButton = transform.Find("Events/Encore").GetComponent<Button>();
        securityButton = transform.Find("Events/Security").GetComponent<Button>();
        concertButton = transform.Find("ImageContainer/ArtistImage").GetComponent<Button>();

        encoreButton.onClick.AddListener(() => HandleButtonClick("Encore"));
        securityButton.onClick.AddListener(() => HandleButtonClick("Security"));
        concertButton.onClick.AddListener(() => HandleButtonClick("Concert"));

        lightShowButton = transform.Find("Equipments/Lightshow/Image").GetComponent<Button>();
        fireworkButton = transform.Find("Equipments/Firework/Image").GetComponent<Button>();
        confettiButton = transform.Find("Equipments/Confetti/Image").GetComponent<Button>();

        lightShowButton.onClick.AddListener(() => HandleButtonClick("LightShow"));
        fireworkButton.onClick.AddListener(() => HandleButtonClick("Firework"));
        confettiButton.onClick.AddListener(() => HandleButtonClick("Confetti"));
    }

    void Update()
    {
        transform.Find("ImageContainer/ArtistImage").Rotate(Vector3.forward * 10f * Time.deltaTime);
    }

    public void AcceptEncore(bool playerChoice)
    {
        if (playerChoice)
        {
            _currentConcert.CurrentEvent.Handled = true;
        }
        else
        {
            transform.Find("Events/Encore").gameObject.SetActive(false);
        }
    }

    public void HandleButtonClick(string button)
    {
        if (button == "Encore")
        {
            EncorePopupUI.Instance.ShowPopup(AcceptEncore);
        }
        else if (button == "Security")
        {
            _currentConcert.CurrentEvent.Handled = true;
        }
        
        if (button == "Concert")
        {
            ToggleEquipments();
        }

        if (button == "LightShow")
        {
            if (_currentConcert.CurrentEvent != null && _currentConcert.CurrentEvent.GetType() == typeof(CrowdLoseEnergy))
            {
                CrowdLoseEnergy crowdEvent = _currentConcert.CurrentEvent as CrowdLoseEnergy;
                _currentConcert.CurrentEvent.Handled = true;
                crowdEvent.EquipmentUsed = _currentConcert.EquipmentBought.FirstOrDefault(e => e.EquipmentType == EquipmentType.LightShow);
        
                ToggleEquipments();
            }
            else
            {
                _currentConcert.CrowdEnergy += 30;
            }

            _currentConcert.EquipmentBought.FirstOrDefault(e => e.EquipmentType == EquipmentType.LightShow).Used = true;

            lightShowButton.interactable = false;
        }
        else if (button == "Firework")
        {
            if (_currentConcert.CurrentEvent != null && _currentConcert.CurrentEvent.GetType() == typeof(CrowdLoseEnergy))
            {
                CrowdLoseEnergy crowdEvent = _currentConcert.CurrentEvent as CrowdLoseEnergy;
                _currentConcert.CurrentEvent.Handled = true;
                crowdEvent.EquipmentUsed = _currentConcert.EquipmentBought.FirstOrDefault(e => e.EquipmentType == EquipmentType.Firework);

                ToggleEquipments();
            }
            else
            {
                _currentConcert.CrowdEnergy += 40;
            }

            _currentConcert.EquipmentBought.FirstOrDefault(e => e.EquipmentType == EquipmentType.Firework).Used = true;

            fireworkButton.interactable = false;
        }
        else if (button == "Confetti")
        {
            if (_currentConcert.CurrentEvent != null && _currentConcert.CurrentEvent.GetType() == typeof(CrowdLoseEnergy))
            {
                CrowdLoseEnergy crowdEvent = _currentConcert.CurrentEvent as CrowdLoseEnergy;
                _currentConcert.CurrentEvent.Handled = true;

                ToggleEquipments();
            }
            else
            {
                _currentConcert.CrowdEnergy += 5;
            }
            _currentConcert.ConfettiCount--;
        }

        _currentConcert.CrowdEnergy = Mathf.Clamp(_currentConcert.CrowdEnergy, 0f, 100f);
    }

    public void ToggleEquipments()
    {
        if (transform.Find("Equipments").gameObject.activeSelf)
        {
            transform.Find("Equipments").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("Equipments").gameObject.SetActive(true);
        }
    }

    public void DisplayOngoingConcert(Concert c)
    {
        if (_currentConcert != c)
        {
            Reset();
        }
        
        _currentConcert = c;

        transform.Find("CrowdEnergy").GetComponent<TMP_Text>().text = c.CrowdEnergy.ToString("F2");

        transform.Find("Equipments/ConfettiCount/ConfettiCountText").GetComponent<TMP_Text>().text = _currentConcert.ConfettiCount.ToString();

        if (c.CheckEquipmentBought(EquipmentType.Firework) == false)
        {
            fireworkButton.interactable = false;
        }

        if (c.CheckEquipmentBought(EquipmentType.LightShow) == false)
        {
            lightShowButton.interactable = false;
        }

        if (_currentConcert.ConfettiCount <= 0)
        {
            confettiButton.interactable = false;
            transform.Find("Equipments/ConfettiCount").gameObject.SetActive(false);
        }

        transform.Find("ImageContainer/ArtistImage").GetComponent<Image>().sprite = c.Artist.ArtistImage;

        float totalDuration = c.EndTime.GetMinutesBetween(c.ConcertTime);
        float durationLeft = GameManager.Instance.CurrentGameTime.GetMinutesBetween(c.ConcertTime);

        if (durationLeft < totalDuration)
        {
            currentFill = 1f - (durationLeft / totalDuration);
            circleImage.fillAmount = currentFill;
        }
        else
        {
            circleImage.fillAmount = 0f;
        }

        if (c.CurrentEvent != null)
        {
            if (c.CurrentEvent.GetType() == typeof(Encore))
            {
                transform.Find("Events/Encore").gameObject.SetActive(true);
            }
            else if (c.CurrentEvent.GetType() == typeof(CrowdLoseEnergy))
            {
                transform.Find("Events/CrowdLoseEnergy").gameObject.SetActive(true);

                transform.Find("Equipments").gameObject.SetActive(true);
            }
            else if (c.CurrentEvent.GetType() == typeof(Security))
            {
                transform.Find("Events/Security").gameObject.SetActive(true);
            }

            transform.Find("Events").gameObject.SetActive(
                transform.Find("Events/Encore").gameObject.activeSelf ||
                transform.Find("Events/CrowdLoseEnergy").gameObject.activeSelf ||
                transform.Find("Events/Security").gameObject.activeSelf
            );

            if (c.CurrentEvent.Handled || c.CurrentEvent.HasExpired())
            {
                transform.Find("Events").gameObject.SetActive(false);

                transform.Find("Events/Encore").gameObject.SetActive(false);
                transform.Find("Events/CrowdLoseEnergy").gameObject.SetActive(false);
                transform.Find("Events/Security").gameObject.SetActive(false);
            }
        }
        else
        {
            transform.Find("Events").gameObject.SetActive(false);

            transform.Find("Events/Encore").gameObject.SetActive(false);
            transform.Find("Events/CrowdLoseEnergy").gameObject.SetActive(false);
            transform.Find("Events/Security").gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        _currentConcert = null;
        transform.Find("Events").gameObject.SetActive(false);
        transform.Find("Equipments").gameObject.SetActive(false);
        transform.Find("Events/Encore").gameObject.SetActive(false);
        transform.Find("Events/CrowdLoseEnergy").gameObject.SetActive(false);
        transform.Find("Events/Security").gameObject.SetActive(false);

        lightShowButton.interactable = true;
        fireworkButton.interactable = true;
        confettiButton.interactable = true;
        
        transform.Find("Equipments/ConfettiCount").gameObject.SetActive(true);
    }

    public Venue Venue
    {
        get { return _venue; }
    }
}