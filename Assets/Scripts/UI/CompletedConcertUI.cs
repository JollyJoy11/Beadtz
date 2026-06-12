using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class CompledtedConcertUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _completedConcerts;
    private Dictionary<int, Concert> _concertsOption = new Dictionary<int, Concert>(); 

    void Start()
    {
        _completedConcerts.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void Update()
    {
        InitializeDropdown();
    }

    public void OnDropDownClicked()
    {
        FindFirstObjectByType<NotificationUI>().StopNotification(); 
    }

    public void InitializeDropdown()
    {
        string emoji;
        _completedConcerts.options.Clear();
        _concertsOption.Clear();

        _completedConcerts.options.Add(new TMP_Dropdown.OptionData("Completed Concerts"));

        int idx = 0;
        foreach (Concert concert in GameManager.Instance.Player.Concerts)
        {
            if (concert.Status == ConcertStatus.Finished)
            {
                if (concert.CrowdEnergy <= 0)
                {
                    emoji = "\U0001F606";
                }
                else if (concert.CrowdEnergy > 90)
                {
                    emoji = "\U0001F60D";
                }
                else
                {
                    emoji = "\U0001F604";
                }

                _completedConcerts.options.Add(new TMP_Dropdown.OptionData($"{concert.Artist} Concert Ends {concert.EndTime.DisplayInString()} {emoji}"));
                _concertsOption[idx] = concert;
                idx++;
            }
            else if (concert.Status == ConcertStatus.Missed)
            {
                emoji = "\U0001F606";
                _completedConcerts.options.Add(new TMP_Dropdown.OptionData($"<color=#FF0000>{concert.Artist} Concert Missed {emoji}"));
                _concertsOption[idx] = concert;
                idx++;
            }
        }
    }

    public void OnDropdownValueChanged(int idx)
    {
        if (_concertsOption.TryGetValue(idx - 1, out Concert selectedConcert))
        {
            GameManager.EvaluatingConcert = selectedConcert;
            Debug.Log($"Show Concert: {GameManager.EvaluatingConcert.Artist}, Status: {GameManager.EvaluatingConcert.Status}");

            _completedConcerts.options.RemoveAt(idx);
            
            FindFirstObjectByType<SceneManagementUI>().SwitchScene("ConcertEvaluation");
        }
    }
}