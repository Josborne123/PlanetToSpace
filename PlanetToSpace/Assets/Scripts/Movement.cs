using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float rotateThrust = 400f;
    [SerializeField] float mainThrust = 1500f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem thrusterParticle;


    Rigidbody rb;
    AudioSource myaudiosource;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myaudiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space) || (Input.GetKey(KeyCode.W)))
        {
            StartToThrust();
        }
        else
        {
            StopThrusting();
        }
    }

    void StopThrusting()
    {
        myaudiosource.Stop();
        thrusterParticle.Stop();
    }

    void StartToThrust()
    {
        if (!thrusterParticle.isPlaying)
        {
            thrusterParticle.Play();
        }

        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!myaudiosource.isPlaying)
        {
            myaudiosource.PlayOneShot(mainEngine);
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotateThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotateThrust);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // un-freezing so the physics system can take over
    }
}
