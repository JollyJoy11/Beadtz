using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private List<GameLevel> _levels;

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

    public List<GameLevel> Levels
    {
        get { return _levels; }
        set { _levels = value; }
    }

    public int CurrentLevelIndex(Player player)
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            if (player.CurrentLevel == _levels[i])
            {
                return i;
            }
        }

        return -1;
    }

    public bool LevelUp(Player player)
    {
        int playerLevelIndex = CurrentLevelIndex(player);
        Debug.Log($"Level index: {playerLevelIndex}, Player EXP: {player.EXP}, Required EXP: {_levels[playerLevelIndex].LevelUpEXP}");
        Debug.Log($"Current Level Index: {playerLevelIndex}, Max Level Index: {_levels.Count - 1}");
        if (playerLevelIndex < _levels.Count - 1 && player.EXP >= _levels[playerLevelIndex].LevelUpEXP)
        {
            playerLevelIndex++;
            player.CurrentLevel = _levels[playerLevelIndex];

            return true;
        }

        return false;
    }
}