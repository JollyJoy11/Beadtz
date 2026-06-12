public class Security : Event
{
    public Security() : base()
    {
        _duration = 6f;
        Description = "A fight is breaking out in the crowd!";
        ChoiceALabel = "Call Security";
        ChoiceBLabel = "Ignore It";
        ChoiceATradeoff = "Situation contained";
        ChoiceBTradeoff = "Crowd energy -10";
        ExpEffect = -300;
    }

    public override void Handle(Concert concert)
    {
        if (_choiceMade == EventChoice.A)
        {
            ExpEffect = 100;
        }
        else
        {
            concert.CrowdEnergy -= 10;
            ExpEffect = -300;
        }
    }
}