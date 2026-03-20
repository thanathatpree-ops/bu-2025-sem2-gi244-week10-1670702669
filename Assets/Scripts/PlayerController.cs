using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;
    private int health = 3;

    private Rigidbody rb;
    private InputAction jumpAction;
    public InputAction speedAction;
    private bool isOnGround = true;
    public bool onGround = false;
    private int maxJumped = 2;
    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;
    private bool canDoubleJump = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");
        speedAction = InputSystem.actions.FindAction("Sprint");
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered && gameOver == false)
        {

            if (isOnGround == true)
            {
                rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
                isOnGround = false;
                canDoubleJump = true;

                playerAnim.SetTrigger("Jump_trig");
                dirtParticle.Stop();
                playerAudio.PlayOneShot(jumpSfx);
            }

            else if (canDoubleJump == true)
            {
                rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
                canDoubleJump = false;

                playerAnim.SetTrigger("Jump_trig");
                playerAudio.PlayOneShot(jumpSfx);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            maxJumped = 2;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            health = health - 1;
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSfx);

            Debug.Log("current Hp" + health);
            if (health == 0)
            {
                Debug.Log("Game Over!");
                gameOver = true;
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                dirtParticle.Stop();
            }
        }

    }
}