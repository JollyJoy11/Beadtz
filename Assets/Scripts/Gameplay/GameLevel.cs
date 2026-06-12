using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GameLevel", menuName = "Beadtz/GameLevel")]
public class GameLevel : ScriptableObject
{
    [SerializeField] private int _levelUpEXP;
    [SerializeField] private int _rewardCoin;
    [SerializeField] private List<Artist> _artistsToUnlock;
    [SerializeField] private City _cityToUnlock;

    public List<Artist> ArtistsToUnlock
    {
        get { return _artistsToUnlock; }
    }

    public int LevelUpEXP
    {
        get { return _levelUpEXP; }
    }

    public int RewardCoin
    {
        get { return _rewardCoin; }
    }
    
    public City CityToUnlock
    {
        get { return _cityToUnlock; }
    }
}