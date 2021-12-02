using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class Collectable : MonoBehaviour
{
    public Text newText;
    public int count = 0;
    public int total = 5;
    public string collectable;
    public Animator transition;

    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] float transitionTime = 1f;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        newText.text = "Collect all " + total + " " + collectable + "!\nYou have " + count + "!\n";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(LoadLevel("MainMenu"));
    }

    void OnTriggerEnter(Collider collision) 
    {  
        if(collision.gameObject.tag == "Collect")
        {
            count++;
            ChangeText();
            Destroy(collision.gameObject);
        }

        if (count == total)
        {
            if (collision.gameObject.tag == "Portal_Forest")
                StartCoroutine(LoadLevel("Forest"));
            else if (collision.gameObject.tag == "Portal_Desert")
                StartCoroutine(LoadLevel("Desert"));
            else if (collision.gameObject.tag == "Portal_Mountain")
                StartCoroutine(LoadLevel("Mountains"));
            else if (collision.gameObject.tag == "Portal_Coast")
                StartCoroutine(LoadLevel("Coast"));
            else if (collision.gameObject.tag == "Portal_Cabin")
                StartCoroutine(LoadLevel("Cabin"));
            else if (collision.gameObject.tag == "Portal_Cabin_End")
                StartCoroutine(LoadLevel("Cabin End"));
            else if (collision.gameObject.tag == "Portal_Coast_End")
                StartCoroutine(LoadLevel("Coast End"));
            else if (collision.gameObject.tag == "Portal_Mountains_End")
                StartCoroutine(LoadLevel("Mountains End"));
        }
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }

    void ChangeText(){
        if(count < total)
        {
            newText.text = "Collect all " + total + " " + collectable + "!\nYou have " + count + "!\n";
            audioSource.PlayOneShot(audioClips[0]);
        } 
        if(count == total) 
        {
            newText.text = "Head to the portal!";
            audioSource.PlayOneShot(audioClips[1]);
        }
    }
}
