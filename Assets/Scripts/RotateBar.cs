using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBar : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
