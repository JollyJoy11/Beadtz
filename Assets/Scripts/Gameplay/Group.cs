using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GroupArtist", menuName = "Beadtz/Artist/Group")]
public class Group : Artist
{
    [SerializeField] private List<string> _members;

    public override void DecreaseEnergy(Player player)
    {
        int consecutiveConcerts = player.ConsecutiveConcertsCount(ArtistName);

        float fatigueFactor = (Mathf.Log(consecutiveConcerts + 1) + 3) * 9f / (3 + _members.Count);
        Energy -= fatigueFactor;
        
        if (Energy < 5)
        {
            IsOnBreak = true;
            BreakStartTime = new GameDateTime(GameManager.Instance.CurrentGameTime);
        }
    }
    
    public List<string> Members
    {
        get { return _members; }
        set { _members = value; }
    }
}