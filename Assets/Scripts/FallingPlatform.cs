using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    HashSet<PlayerController> _playersInTrigger = new HashSet<PlayerController>();
    Coroutine _coroutine;
    Vector3 _initialPosition;
    bool _falling;
    float _wiggleTimer;

    [Tooltip("Reset the wiggle timer when no players are on the platform")]
    [SerializeField] bool _resetOnEmpty;
    [SerializeField] float _fallSpeed = 9;
    [Range(0.1f, 5)] [SerializeField] float _fallAfterSeconds = 3;
    [Range(0.005f, 0.1f)] [SerializeField] float _shakeX = 0.005f;
    [Range(0.005f, 0.1f)] [SerializeField] float _shakeY = 0.005f;
    [Range(0.005f, 0.1f)] [SerializeField] float _shakeZ = 0.005f;

    void Start()
    {
        _initialPosition = transform.position;
    }

    void OnTriggerEnter(Collider collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player == null)
            return;

        _playersInTrigger.Add(player);

        if (_playersInTrigger.Count == 1)
            _coroutine = StartCoroutine(WiggleAndFall());
    }

    IEnumerator WiggleAndFall()
    {
        yield return new WaitForSeconds(0.25f);

        while (_wiggleTimer < _fallAfterSeconds)
        {
            float randomX = UnityEngine.Random.Range(-_shakeX, _shakeX);
            float randomY = UnityEngine.Random.Range(-_shakeY, _shakeY);
            float randomZ = UnityEngine.Random.Range(-_shakeZ, _shakeZ);
            transform.position = _initialPosition + new Vector3(randomX, randomY, randomZ);
            float randomDelay = UnityEngine.Random.Range(0.005f, 0.01f);
            yield return new WaitForSeconds(randomDelay);
            _wiggleTimer += randomDelay;
        }

        _falling = true;
        foreach (var collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }

        float fallTimer = 0;

        while (fallTimer < 3f)
        {
            transform.position += Vector3.down * Time.deltaTime * _fallSpeed;
            fallTimer += Time.deltaTime;
            yield return null;
        }

        transform.position = _initialPosition;
        _wiggleTimer = 0;
        _falling = false;
        foreach (var collider in GetComponents<Collider>())
        {
            collider.enabled = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (_falling)
            return;

        var player = collision.GetComponent<PlayerController>();
        if (player == null)
            return;

        _playersInTrigger.Remove(player);

        if (_playersInTrigger.Count == 0)
        {
            StopCoroutine(_coroutine);

            if (_resetOnEmpty)
                _wiggleTimer = 0;
        }
    }
}