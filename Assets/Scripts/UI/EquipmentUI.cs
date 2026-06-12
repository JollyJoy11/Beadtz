using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private Transform togglePanel;
    [SerializeField] private List<Equipment> equipmentList;

    void Start()
    {
        Toggle[] toggles = togglePanel.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < Mathf.Min(equipmentList.Count, toggles.Length); i++)
        {
            int index = i;

            var label = toggles[index].transform.Find("Label").GetComponent<Text>();
            label.text = $"{equipmentList[index].EquipmentType} {equipmentList[index].Cost}";

            toggles[index].onValueChanged.AddListener(isOn =>
            {
                bool wasSelected = DataCarrier.Instance.SelectedEquipment.Contains(equipmentList[index]);

                if (wasSelected)
                {
                    DataCarrier.Instance.SelectedEquipment.Remove(equipmentList[index]);
                }
                else
                {
                    DataCarrier.Instance.SelectedEquipment.Add(equipmentList[index]);
                }
            });
        }
    }
}