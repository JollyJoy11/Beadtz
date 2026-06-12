using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void ClosePopup()
    {
        popup.SetActive(false); 
    }

}