using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DecisionPanelUI : MonoBehaviour
{
    public static DecisionPanelUI Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _choiceALabel;
    [SerializeField] private TMP_Text _choiceATradeoff;
    [SerializeField] private TMP_Text _choiceBLabel;
    [SerializeField] private TMP_Text _choiceBTradeoff;
    [SerializeField] private Button _choiceAButton;
    [SerializeField] private Button _choiceBButton;
    [SerializeField] private Image _timerBar;

    private Event _activeEvent;
    private Concert _activeConcert;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _panel.SetActive(false);
        _choiceAButton.onClick.AddListener(OnChoiceA);
        _choiceBButton.onClick.AddListener(OnChoiceB);
    }

    void Update()
    {
        if (_activeEvent == null) return;

        float progress = _activeEvent.TimerProgress;
        _timerBar.fillAmount = progress;
        _timerBar.color = progress > 0.5f
            ? Color.Lerp(Color.yellow, Color.green, (progress - 0.5f) * 2f)
            : Color.Lerp(Color.red, Color.yellow, progress * 2f);

        if (_activeEvent.HasExpired())
            Hide();
    }

    public void Show(Event e, Concert concert)
    {
        _activeEvent = e;
        _activeConcert = concert;

        _description.text = e.Description;
        _choiceALabel.text = e.ChoiceALabel;
        _choiceATradeoff.text = e.ChoiceATradeoff;
        _choiceBLabel.text = e.ChoiceBLabel;
        _choiceBTradeoff.text = e.ChoiceBTradeoff;
        _timerBar.fillAmount = 1f;

        _panel.SetActive(true);
    }

    public void Hide()
    {
        _panel.SetActive(false);
        _activeEvent = null;
        _activeConcert = null;
    }

    private void OnChoiceA()
    {
        if (_activeEvent == null) return;

        if (_activeEvent is CrowdLoseEnergy crowdEvent && _activeConcert != null)
        {
            Equipment best = _activeConcert.EquipmentBought
                .FirstOrDefault(e => e.EquipmentType == EquipmentType.Firework && !e.Used)
                ?? _activeConcert.EquipmentBought
                .FirstOrDefault(e => e.EquipmentType == EquipmentType.LightShow && !e.Used)
                ?? _activeConcert.EquipmentBought
                .FirstOrDefault(e => !e.Used);

            if (best != null)
            {
                crowdEvent.EquipmentUsed = best;
                best.Used = true;
            }
        }

        _activeEvent.MakeChoice(EventChoice.A);
        Hide();
    }

    private void OnChoiceB()
    {
        if (_activeEvent == null) return;
        _activeEvent.MakeChoice(EventChoice.B);
        Hide();
    }
}
