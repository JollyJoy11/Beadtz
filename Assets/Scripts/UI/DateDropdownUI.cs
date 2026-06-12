using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DateDropdownUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dayDropdown;
    [SerializeField] private TMP_Dropdown monthDropdown;
    [SerializeField] private TMP_Dropdown yearDropdown;
    [SerializeField] private TMP_Dropdown hourDropdown;
    [SerializeField] private TMP_Dropdown minuteDropdown;
    [SerializeField] private GameObject popup;

    private GameDateTime currentGameTime;

    void Start()
    {
        currentGameTime = GameManager.Instance.CurrentGameTime;
        InitializeDropdowns();
        SetToCurrentDate();

        monthDropdown.onValueChanged.AddListener((_) => UpdateDayDropdown());
        yearDropdown.onValueChanged.AddListener((_) => UpdateDayDropdown());
    }

    public void InitializeDropdowns()
    {
        dayDropdown.ClearOptions();
        dayDropdown.options.Add(new TMP_Dropdown.OptionData("Day"));
        for (int i = 1; i <= 31; i++)
        {
            dayDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        yearDropdown.ClearOptions();
        yearDropdown.options.Add(new TMP_Dropdown.OptionData("Year"));
        int currentYear = currentGameTime.Year;
        for (int i = currentYear; i <= currentYear + 10; i++)
        {
            yearDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        hourDropdown.ClearOptions();
        hourDropdown.options.Add(new TMP_Dropdown.OptionData("Hour"));
        for (int i = 0; i < 22; i++)
        {
            hourDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString("00")));
        }

        minuteDropdown.ClearOptions();
        minuteDropdown.options.Add(new TMP_Dropdown.OptionData("Minute"));
        for (int i = 0; i < 60; i += 5)
        {
            minuteDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString("00")));
        }
    }

    public void SetToCurrentDate()
    {
        dayDropdown.value = currentGameTime.Day;
        monthDropdown.value = currentGameTime.Month;
        yearDropdown.value = 1;
        hourDropdown.value = currentGameTime.Hour + 1;
        minuteDropdown.value = currentGameTime.Minute;
    }

    public void GetSelectedDateTime()
    {
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int month = monthDropdown.value;
        int day = dayDropdown.value;
        int hour = hourDropdown.value - 1;
        int minute = int.Parse(minuteDropdown.options[minuteDropdown.value].text);

        GameDateTime selectedDate = new GameDateTime(day, month, year, hour, minute);

        if (selectedDate.GetMinutesBetween(currentGameTime) < 0)
        {
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Invalid time selection. Choose a future date!";
        }
        else if (DataCarrier.Instance.SelectedVenue.CheckScheduleConflict(selectedDate, selectedDate.GetAddHourTime(1)))
        {
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Conflicted schedule!";
        }
        else if (currentGameTime.GetDaysBetween(selectedDate) > 7)
        {
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Venues require quick confirmation — plan within 7 days.";
        }
        else
        {
            int equipmentCost = 0;
            if (DataCarrier.Instance.SelectedEquipment.Count != 0)
            {
                foreach (Equipment e in DataCarrier.Instance.SelectedEquipment)
                {
                    equipmentCost += e.Cost;
                }
            }

            if (GameManager.Instance.Player.Money - DataCarrier.Instance.SelectedVenue.Cost - equipmentCost >= 0)
            {
                DataCarrier.Instance.SetSelectedDate(selectedDate);
                SceneManager.LoadScene("In Game");

                List<Equipment> equipmentBought = new List<Equipment>();
                foreach (Equipment e in DataCarrier.Instance.SelectedEquipment)
                {
                    equipmentBought.Add(new Equipment(e));
                }

                GameManager.Instance.Player.PlanConcert(DataCarrier.Instance.SelectedArtist, DataCarrier.Instance.SelectedCity, DataCarrier.Instance.SelectedVenue, DataCarrier.Instance.SelectedSongs, DataCarrier.Instance.SelectedTime, equipmentBought);

                GameManager.Instance.Player.AddMoney(-(DataCarrier.Instance.SelectedVenue.Cost + equipmentCost));
                DataCarrier.Instance.Reset();
            }
            else
            {
                popup.gameObject.SetActive(true);
                popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Insufficient budget to buy equipment.";
            }
        }
    }

    public void UpdateDayDropdown()
    {
        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedMonth = monthDropdown.value;

        int daysInMonth = currentGameTime.GetDaysInMonth(selectedMonth, selectedYear);
        int previouslySelectedDay = dayDropdown.value;

        dayDropdown.ClearOptions();
        dayDropdown.options.Add(new TMP_Dropdown.OptionData("Day"));
        for (int i = 1; i <= daysInMonth; i++)
        {
            dayDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        dayDropdown.value = Mathf.Min(previouslySelectedDay, daysInMonth);
    }
}