using UnityEngine;

public class FanRushesStage : Event
{
    public FanRushesStage() : base()
    {
        _duration = 5f;
        Description = "A fan is rushing toward the stage!";
        ChoiceALabel = "Let It Happen";
        ChoiceBLabel = "Block Them";
        ChoiceATradeoff = "Crowd energy +15, fame +5, artist energy -10";
        ChoiceBTradeoff = "Safe — no impact";
        ExpEffect = 50;
    }

    public override void Handle(Concert concert)
    {
        if (_choiceMade == EventChoice.A)
        {
            concert.CrowdEnergy = Mathf.Clamp(concert.CrowdEnergy + 15f, 0f, 100f);
            concert.Artist.Energy -= 10f;
            concert.Artist.Fame += 5;
            ExpEffect = 300;
        }
        else if (_choiceMade == EventChoice.B)
        {
            ExpEffect = 50;
        }
        else // expired — chaos briefly
        {
            concert.CrowdEnergy = Mathf.Clamp(concert.CrowdEnergy - 5f, 0f, 100f);
            ExpEffect = -50;
        }
    }
}
