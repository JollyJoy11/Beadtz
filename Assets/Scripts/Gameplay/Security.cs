using System;

public class Security : Event
{
    public Security() : base()
    {
        ExpEffect = -300;
    }

    public override void Handle(Concert concert)
    {
        if (_handled)
        {
            ExpEffect = 100;
        }
        else
        {
            concert.CrowdEnergy -= 10;
        }
    }
}