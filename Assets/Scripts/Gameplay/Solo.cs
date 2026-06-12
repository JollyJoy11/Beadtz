using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoloArtist", menuName = "Beadtz/Artist/Solo")]
public class Solo : Artist
{
    public override void DecreaseEnergy(Player player)
    {
        int consecutiveConcerts = player.ConsecutiveConcertsCount(ArtistName);

        float fatigueFactor = Mathf.Log(consecutiveConcerts + 1) * 15f;
        fatigueFactor = Mathf.Clamp(fatigueFactor, 6f, 30f);
        Energy -= fatigueFactor;

        if (Energy < 10)
        {
            IsOnBreak = true;
            BreakStartTime = new GameDateTime(GameManager.Instance.CurrentGameTime);
        }
    }
}