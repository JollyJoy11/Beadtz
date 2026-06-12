using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayNotification()
    {
        animator.SetTrigger("ShakeBell");
    }

    public void StopNotification()
    {
        animator.ResetTrigger("ShakeBell"); 
        animator.Play("Idle");
    }
}