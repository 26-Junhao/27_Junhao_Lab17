using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public float moveSpeed;
    public float jumpForce;
    public int healthCount;
    public int coinCount;
    public GameObject Healthtxt;
    public GameObject Cointxt;
    private AudioSource audioSource;
    public AudioClip[] audioClipsArr;
    int jumpCount;
    bool isWalking = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float hVelocity = 0;
        float vVelocity = 0;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            hVelocity = -moveSpeed;
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));
            transform.localScale = new Vector3(-1, 1, 1);
            isWalking = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isWalking = false;
            animator.SetFloat("xVelocity", 0);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            isWalking = true;
            hVelocity = moveSpeed;
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isWalking = false;
            animator.SetFloat("xVelocity", 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0)
        {
            jumpCount++;
            audioSource.clip = audioClipsArr[0];
            audioSource.PlayOneShot(audioClipsArr[0]);
            animator.SetTrigger("JumpTrigger");
            animator.SetBool("isOnGround", false);
            vVelocity = jumpForce;
        }

        hVelocity = Mathf.Clamp(rb.velocity.x + hVelocity, -5, 5);

        rb.velocity = new Vector2(hVelocity, rb.velocity.y + vVelocity);

        if(isWalking)
        {
            audioSource.clip = audioClipsArr[1];
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Mace")
        {
            healthCount -= 10;
            Healthtxt.GetComponent<Text>().text = "Heath: " + healthCount;
        }
        if(collision.gameObject.tag == "Coin")
        {
            coinCount++;
            Destroy(collision.gameObject, 2);
            Cointxt.GetComponent<Text>().text = "Coin: " + coinCount;
        }
        if(collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
            animator.SetBool("isOnGround", true);
        }
    }
}
