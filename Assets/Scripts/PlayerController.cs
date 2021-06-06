using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnimator;
    public ParticleSystem explosion;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource _playerAudio;
    public float jumpForce = 10;
    public float gravityModifier = 1;
    public float speed = 5;

    public bool isOnGround = true;
    private int jumpCount = 0;

    public bool gameOver = false;
    private static readonly int JumpTrig = Animator.StringToHash("Jump_trig");
    private static readonly int DeathB = Animator.StringToHash("Death_b");
    private static readonly int DeathTypeINT = Animator.StringToHash("DeathType_int");
    private static readonly int DashSpeed = Animator.StringToHash("DashSpeed");

    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    // Movement speed in units per second.
    public float lerpSpeed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private static readonly int SpeedF = Animator.StringToHash("Speed_f");

    // Start is called before the first frame update
    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        // playerRb.AddForce(Vector3.up * 350);
        Physics.gravity *= gravityModifier;
        dirtParticle.Play();
        Debug.Log("Motion?", playerAnimator.GetComponent<Motion>());
        playerAnimator.SetFloat(SpeedF, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 0)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * lerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            // transform.Translate(vector);
            // transform.position.Set(transform.position.x, 0, transform.position.z);
        }
        else
        {
            playerAnimator.SetFloat(SpeedF, 1f);
        }

        // Jump!
        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || jumpCount < 2) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            jumpCount += 1;
            playerAnimator.SetTrigger(JumpTrig);
            dirtParticle.Stop();
            _playerAudio.PlayOneShot(jumpSound, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnimator.SetFloat(DashSpeed, 2);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnimator.SetFloat(DashSpeed, 1);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jumpCount = 0;
            if (!gameOver)
            {
                dirtParticle.Play();
            }
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            playerAnimator.SetBool(DeathB, true);
            playerAnimator.SetInteger(DeathTypeINT, 1);
            explosion.Play();
            _playerAudio.PlayOneShot(crashSound);
            dirtParticle.Stop();
        }
    }
}