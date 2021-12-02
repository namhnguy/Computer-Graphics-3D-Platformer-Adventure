using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] UnityEvent _onPressed;
    [SerializeField] UnityEvent _onReleased;

    //Vector3 _initialPosition;

    private void Awake()
    {
        //_initialPosition = transform.localPosition;
        BecomeReleased();
    }

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null)
            return;
        BecomePressed();
    }

    void BecomePressed()
    {
        //Debug.Log("inside press");
        //transform.localPosition = new Vector3(transform.localPosition.x, 0.40f, transform.localPosition.z);
        _onPressed?.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null)
            return;
        BecomeReleased();
    }

    void BecomeReleased()
    {
        //Debug.Log("inside release");
        //transform.localPosition = _initialPosition;
        _onReleased?.Invoke();
    }
}
