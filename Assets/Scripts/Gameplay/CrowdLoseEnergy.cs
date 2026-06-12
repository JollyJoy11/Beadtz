using UnityEngine;
using System;

/// <summary>
/// 
/// </summary>
public class CrowdLoseEnergy : Event
{
    private Equipment _equipmentUsed;

    /// <summary>
    /// 
    /// </summary>
    public CrowdLoseEnergy() : base()
    {
        ExpEffect = -100;
    }

    public Equipment EquipmentUsed
    {
        set { _equipmentUsed = value; }
    }

    public override void Handle(Concert concert)
    {
        if (_handled)
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

            concert.CrowdEnergy = Mathf.Clamp(concert.CrowdEnergy, 0f, 100f);

        }
    }
}