using UnityEngine;
using System;

[System.Serializable]
public class Equipment
{
    [SerializeField] private EquipmentType _equipmentType;
    [SerializeField] private int _cost;
    [SerializeField] private int _expEarned;
    private bool _used;

    public Equipment(EquipmentType equipmentType, int cost, int expEarned)
    {
        _equipmentType = equipmentType;
        _cost = cost;
        _expEarned = expEarned;
        _used = false;
    }

    public Equipment(Equipment equipment)
    {
        _equipmentType = equipment._equipmentType;
        _cost = equipment._cost;
        _expEarned = equipment._expEarned;
        _used = false;
    }

    public bool Used
    {
        get { return _used; }
        set { _used = value; }
    }

    public EquipmentType EquipmentType
    {
        get { return _equipmentType; }
    }

    public int Cost
    {
        get { return _cost; }
    }

    public int ExpEarned
    {
        get { return _expEarned; }
    }
}