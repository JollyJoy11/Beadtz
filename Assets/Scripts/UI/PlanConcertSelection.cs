using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlanConcertSelection : MonoBehaviour
{
    [SerializeField] private GameObject popup;

    public void SelectVenue()
    {
        if (DataCarrier.Instance.SelectedCity == null && DataCarrier.Instance.SelectedVenue == null)
        {
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Please select a venue!";
        }
        else if (!DataCarrier.Instance.SelectedVenue.IsBought)
        {
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = $"{DataCarrier.Instance.SelectedVenue.VenueName} is not unlocked yet.";
        }
        else if (GameManager.Instance.Player.Money - DataCarrier.Instance.SelectedVenue.Cost < 0)
        {
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Insufficient budget to rent venue.";
        }
        else
        {
            SceneManager.LoadScene("PlanConcertSong");
        }
    }

    public void SelectSong()
    {
        if (DataCarrier.Instance.SelectedSongs.Count == 6)
        {
            SceneManager.LoadScene("PlanConcertAttribute");
        }
        else
        {
            popup.gameObject.SetActive(true);
            popup.transform.Find("Text").GetComponent<TMP_Text>().text = "Please select 6 songs!";
        }
    }
}