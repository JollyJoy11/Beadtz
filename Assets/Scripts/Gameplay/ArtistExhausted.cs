using UnityEngine;

public class ArtistExhausted : Event
{
    public ArtistExhausted() : base()
    {
        _duration = 8f;
        Description = "Your artist is visibly exhausted on stage!";
        ChoiceALabel = "Push Through";
        ChoiceBLabel = "Cut the Set";
        ChoiceATradeoff = "Artist energy → 0, forced break after show";
        ChoiceBTradeoff = "Concert ends in 2 min, reduced earnings";
        ExpEffect = -100;
    }

    public override void Handle(Concert concert)
    {
        if (_choiceMade == EventChoice.A)
        {
            concert.Artist.Energy = 0;
            concert.Artist.IsOnBreak = true;
            concert.Artist.BreakStartTime = new GameDateTime(GameManager.Instance.CurrentGameTime);
            ExpEffect = 100;
        }
        else // cut the set or expired
        {
            concert.EndTime = GameManager.Instance.CurrentGameTime.GetAddMinuteTime(2);
            ExpEffect = -200;
        }
    }
}
