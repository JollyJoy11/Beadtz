using TMPro;
using UnityEngine;

public class ArtistBreakNotification : MonoBehaviour
{
    private Animator animator;
    private bool isNotificationActive = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowNotification(Artist artist)
    {
        if (isNotificationActive) return; 

        isNotificationActive = true; 
        animator.SetTrigger("ShowNotification");
        Invoke(nameof(HideNotification), 5f);

        transform.Find("NotiText").GetComponent<TMP_Text>().text = $"{artist.ArtistName} went on a holiday~";
    }

    void HideNotification()
    {
        animator.SetTrigger("HideNotification");
        Invoke(nameof(ResetFlag), 1f); 
    }

    void ResetFlag()
    {
        isNotificationActive = false; 
    }
}