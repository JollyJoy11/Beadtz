using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ConcertUI : MonoBehaviour
{
    [SerializeField] private Venue _venue;

    private Image _circleImage;
    private Button _encoreButton;
    private Button _securityButton;
    private Button _crowdLoseEnergyButton;
    private Button _concertButton;
    private Button _lightShowButton;
    private Button _fireworkButton;
    private Button _confettiButton;
    private float currentFill = 1f;
    private Concert _currentConcert;

    void Start()
    {
        _circleImage = transform.Find("ConcertTime").GetComponent<Image>();

        _encoreButton = transform.Find("Events/Encore").GetComponent<Button>();
        _securityButton = transform.Find("Events/Security").GetComponent<Button>();
        _crowdLoseEnergyButton = transform.Find("Events/CrowdLoseEnergy").GetComponent<Button>();
        _concertButton = transform.Find("ImageContainer/ArtistImage").GetComponent<Button>();

        _encoreButton.onClick.AddListener(OnEventIconTapped);
        _securityButton.onClick.AddListener(OnEventIconTapped);
        _crowdLoseEnergyButton.onClick.AddListener(ToggleEquipments);
        _concertButton.onClick.AddListener(ToggleEquipments);

        _lightShowButton = transform.Find("Equipments/Lightshow/Image").GetComponent<Button>();
        _fireworkButton = transform.Find("Equipments/Firework/Image").GetComponent<Button>();
        _confettiButton = transform.Find("Equipments/Confetti/Image").GetComponent<Button>();

        _lightShowButton.onClick.AddListener(() => UseEquipment(EquipmentType.LightShow));
        _fireworkButton.onClick.AddListener(() => UseEquipment(EquipmentType.Firework));
        _confettiButton.onClick.AddListener(UseConfetti);
    }

    void Update()
    {
        transform.Find("ImageContainer/ArtistImage").Rotate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnEventIconTapped()
    {
        if (_currentConcert?.CurrentEvent == null) return;
        if (_currentConcert.CurrentEvent.Handled || _currentConcert.CurrentEvent.HasExpired()) return;

        DecisionPanelUI.Instance.Show(_currentConcert.CurrentEvent, _currentConcert);
    }

    public void UseEquipment(EquipmentType type)
    {
        if (_currentConcert == null) return;
        Equipment e = _currentConcert.EquipmentBought.FirstOrDefault(eq => eq.EquipmentType == type && !eq.Used);
        if (e == null) return;

        if (_currentConcert.CurrentEvent is CrowdLoseEnergy crowdEvent)
        {
            crowdEvent.EquipmentUsed = e;
            _currentConcert.CurrentEvent.Handled = true;
            ToggleEquipments();
        }
        else
        {
            float boost = type == EquipmentType.Firework ? 40f : type == EquipmentType.LightShow ? 30f : 5f;
            _currentConcert.CrowdEnergy = Mathf.Clamp(_currentConcert.CrowdEnergy + boost, 0f, 100f);
        }

        e.Used = true;
        RefreshEquipmentButtons();
    }

    public void UseConfetti()
    {
        if (_currentConcert == null) return;

        if (_currentConcert.CurrentEvent is CrowdLoseEnergy)
            _currentConcert.CurrentEvent.Handled = true;
        else
            _currentConcert.CrowdEnergy = Mathf.Clamp(_currentConcert.CrowdEnergy + 5f, 0f, 100f);

        _currentConcert.ConfettiCount--;
        RefreshEquipmentButtons();
    }

    private void RefreshEquipmentButtons()
    {
        if (_currentConcert == null) return;

        _lightShowButton.interactable = _currentConcert.CheckEquipmentBought(EquipmentType.LightShow);
        _fireworkButton.interactable = _currentConcert.CheckEquipmentBought(EquipmentType.Firework);
        _confettiButton.interactable = _currentConcert.ConfettiCount > 0;

        transform.Find("Equipments/ConfettiCount/ConfettiCountText").GetComponent<TMP_Text>().text = _currentConcert.ConfettiCount.ToString();
        transform.Find("Equipments/ConfettiCount").gameObject.SetActive(_currentConcert.ConfettiCount > 0);
    }

    public void ToggleEquipments()
    {
        var equipments = transform.Find("Equipments").gameObject;
        equipments.SetActive(!equipments.activeSelf);
    }

    public void DisplayOngoingConcert(Concert c)
    {
        if (_currentConcert != c) Reset();

        _currentConcert = c;

        transform.Find("CrowdEnergy").GetComponent<TMP_Text>().text = c.CrowdEnergy.ToString("F2");
        transform.Find("ImageContainer/ArtistImage").GetComponent<Image>().sprite = c.Artist.ArtistImage;

        RefreshEquipmentButtons();

        float totalDuration = c.EndTime.GetMinutesBetween(c.ConcertTime);
        float durationLeft = GameManager.Instance.CurrentGameTime.GetMinutesBetween(c.ConcertTime);
        currentFill = durationLeft < totalDuration ? 1f - (durationLeft / totalDuration) : 0f;
        _circleImage.fillAmount = currentFill;

        // show the right event icon, hide the rest
        bool hasEncore = c.CurrentEvent is Encore && !c.CurrentEvent.Handled && !c.CurrentEvent.HasExpired();
        bool hasSecurity = c.CurrentEvent is Security && !c.CurrentEvent.Handled && !c.CurrentEvent.HasExpired();
        bool hasCrowdLoseEnergy = c.CurrentEvent is CrowdLoseEnergy && !c.CurrentEvent.Handled && !c.CurrentEvent.HasExpired();

        transform.Find("Events/Encore").gameObject.SetActive(hasEncore);
        transform.Find("Events/Security").gameObject.SetActive(hasSecurity);
        transform.Find("Events/CrowdLoseEnergy").gameObject.SetActive(hasCrowdLoseEnergy);

        bool anyEvent = hasEncore || hasSecurity || hasCrowdLoseEnergy;
        transform.Find("Events").gameObject.SetActive(anyEvent);

        // for new event types (FanRushesStage, ArtistExhausted) show CrowdLoseEnergy icon as fallback
        if (!anyEvent && c.CurrentEvent != null && !c.CurrentEvent.Handled && !c.CurrentEvent.HasExpired())
        {
            transform.Find("Events").gameObject.SetActive(true);
            transform.Find("Events/CrowdLoseEnergy").gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        _currentConcert = null;
        transform.Find("Events").gameObject.SetActive(false);
        transform.Find("Events/Encore").gameObject.SetActive(false);
        transform.Find("Events/Security").gameObject.SetActive(false);
        transform.Find("Events/CrowdLoseEnergy").gameObject.SetActive(false);
        transform.Find("Equipments").gameObject.SetActive(false);
        transform.Find("Equipments/ConfettiCount").gameObject.SetActive(true);

        if (_lightShowButton != null) _lightShowButton.interactable = true;
        if (_fireworkButton != null) _fireworkButton.interactable = true;
        if (_confettiButton != null) _confettiButton.interactable = true;
    }

    public Venue Venue => _venue;
}
