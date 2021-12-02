using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector3 _startPosition;
    [SerializeField] Vector3 _direction = Vector3.forward;
    [SerializeField] float _maxDistance = 2f;
    [SerializeField] float _speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction.normalized * Time.deltaTime * _speed);
        var distance = Vector3.Distance(_startPosition, transform.position);
        if (distance >= _maxDistance)
        {
            transform.position = _startPosition + (_direction.normalized * _maxDistance);
            _direction *= -1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null)
            return;
        player.transform.SetParent(transform);
    }

    void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null)
            return;
        player.transform.SetParent(null);
    }
}
