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

    public bool isOnGround = true;

    public bool gameOver = false;
    private static readonly int JumpTrig = Animator.StringToHash("Jump_trig");
    private static readonly int DeathB = Animator.StringToHash("Death_b");
    private static readonly int DeathTypeINT = Animator.StringToHash("DeathType_int");

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        // playerRb.AddForce(Vector3.up * 350);
        Physics.gravity *= gravityModifier;
        dirtParticle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Jump!
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnimator.SetTrigger(JumpTrig);
            dirtParticle.Stop();
            _playerAudio.PlayOneShot(jumpSound,0.5f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
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