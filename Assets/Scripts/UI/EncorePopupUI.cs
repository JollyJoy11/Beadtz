using UnityEngine;
using UnityEngine.UI;

public class EncorePopupUI : MonoBehaviour
{
    public static EncorePopupUI Instance;
    [SerializeField] private GameObject encorePopup;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    private System.Action<bool> _callback;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        encorePopup.SetActive(false);

        yesButton.onClick.AddListener(() => HandleSelection(true));
        noButton.onClick.AddListener(() => HandleSelection(false));
    }

    public void ShowPopup(System.Action<bool> callback)
    {
        encorePopup.SetActive(true);
        _callback = callback;
    }

    public void ClosePopup()
    {
        encorePopup.SetActive(false);
    }

    private void HandleSelection(bool isYes)
    {
        encorePopup.SetActive(false);
        _callback?.Invoke(isYes);
    }
}