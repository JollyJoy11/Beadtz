using UnityEngine;

public class CrowdLoseEnergy : Event
{
    private Equipment _equipmentUsed;

    public CrowdLoseEnergy() : base()
    {
        _duration = 7f;
        Description = "The crowd is losing interest fast!";
        ChoiceALabel = "Deploy Equipment";
        ChoiceBLabel = "Let It Slide";
        ChoiceATradeoff = "Uses best available equipment";
        ChoiceBTradeoff = "Crowd energy -15";
        ExpEffect = -100;
    }

    public Equipment EquipmentUsed
    {
        set { _equipmentUsed = value; }
    }

    public override void Handle(Concert concert)
    {
        if (_choiceMade == EventChoice.A)
        {
            if (_equipmentUsed != null)
            {
                if (_equipmentUsed.EquipmentType == EquipmentType.LightShow)
                {
                    concert.CrowdEnergy += 30;
                    ExpEffect = 500;
                }
                else if (_equipmentUsed.EquipmentType == EquipmentType.Firework)
                {
                    concert.CrowdEnergy += 40;
                    ExpEffect = 800;
                }
                else
                {
                    concert.CrowdEnergy += 10;
                    ExpEffect = 200;
                }
            }
            else
            {
                concert.CrowdEnergy += 5;
                ExpEffect = 50;
            }

            concert.CrowdEnergy = Mathf.Clamp(concert.CrowdEnergy, 0f, 100f);
        }
        else
        {
            concert.CrowdEnergy = Mathf.Clamp(concert.CrowdEnergy - 15f, 0f, 100f);
        }
    }
}