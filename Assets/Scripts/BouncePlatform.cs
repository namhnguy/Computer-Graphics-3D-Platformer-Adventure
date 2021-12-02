using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    [SerializeField] float _bounceForce = 5f;

    public float BounceForce { get { return _bounceForce; }  }
}
