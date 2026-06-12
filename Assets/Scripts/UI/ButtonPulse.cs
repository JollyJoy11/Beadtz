using UnityEngine;

public class ButtonPulse : MonoBehaviour
{
    [SerializeField]private float _speed = 6.0f;
    [SerializeField]private float _scaleAmount = 0.05f;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * _speed) * _scaleAmount;
        transform.localScale = initialScale * scale;
    }
}
