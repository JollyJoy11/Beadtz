using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerMoney;
    [SerializeField] private TMP_Text _currentTime;
    [SerializeField] private TMP_Text _currentLevel;
    [SerializeField] private GameObject[] _cityObjects;
    [SerializeField] private City[] _cityDatas;
    [SerializeField] private TMP_Text _expDisplay;

    public void UpdatePlayerStats(Player player)
    {
        _playerMoney.text = player.Money.ToString();

        if (_currentLevel != null)
        {
            _currentLevel.text = (LevelManager.Instance.CurrentLevelIndex(player) + 1).ToString();
        }

        if (_expDisplay != null)
        {
            _expDisplay.text = $"{player.EXP}/{player.CurrentLevel.LevelUpEXP}";
        }
    }

    public void UpdateClock(GameDateTime time)
    {
        _currentTime.text = time.DisplayInString();
    }

    public void UpdateCityUI()
    {
        for (int i = 0; i < _cityObjects.Length; i++)
        {
            _cityObjects[i].SetActive(_cityDatas[i].Unlocked);
        }
    }

    public void DisplayMoneyAfterSelection(Player player)
    {
        _playerMoney.text = $"{player.Money - DataCarrier.Instance.SelectedVenue.Cost}";
    }
}