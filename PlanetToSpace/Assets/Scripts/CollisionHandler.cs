using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float delayTime = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;


    AudioSource myaudiosource;

    bool isTransitioning = false;
    bool collisionDisabled = false;


    // Start is called before the first frame update
    void Start()
    {
        myaudiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision

        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning == true || collisionDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        deathParticle.Play();
        isTransitioning = true;
        myaudiosource.Stop();
        myaudiosource.PlayOneShot(deathSound);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayTime);

    }

    void StartSuccessSequence()
    {
        successParticle.Play();
        isTransitioning = true;
        myaudiosource.Stop();
        myaudiosource.PlayOneShot(successSound);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayTime);
    }
 

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

}
