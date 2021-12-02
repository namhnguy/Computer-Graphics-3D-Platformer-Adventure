using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoadLevelButton : MonoBehaviour
{
    [SerializeField] string _levelName;
    [SerializeField] AudioClip _startSound;

    float _transitionTime = 1.5f;
    AudioSource _audioSource;

    public string LevelName => _levelName;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(_levelName);
    }

    public void StartButton()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        if (_startSound != null)
            _audioSource.PlayOneShot(_startSound);
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(1);
    }
}
