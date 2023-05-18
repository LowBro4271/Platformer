using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    public bool isGrounded;
    Animator anim;
    public Vector3 vel;
    public int curHP;
    public int maxHP = 3;
    public bool isHit = false;
    public Main main;
    public bool key = false;
    public bool canTp = true;
    public bool inWater = false;
    public bool onLadder = false;
    public int coins = 0;
    public bool canHit = true;
    public GameObject blue, red;
    public int gemCount = 0;
    public float hitTimer = 0f;
    public Image PlayerCountdown;
    public Inventory inventory;
    public SoundEffector soundEffector;
    public Joystick Joystick;
    public Touch touch;
    public GameObject secondClue, thirdClue, fourthClue;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (inWater && !onLadder)
        {
            anim.SetInteger("State", 4);
            isGrounded = true;
            if (Joystick.Horizontal != 0)
                Flip();
        }
        else
        {
            vel = rb.velocity;
            checkGround();
            if (Joystick.Horizontal == 0 && (isGrounded) && !onLadder)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !onLadder)
                    anim.SetInteger("State", 2);
            }
        }
        rb.velocity = new Vector2(Joystick.Horizontal * speed, rb.velocity.y);

        /*        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
                    soundEffector.PlayJumpSound();
                }
        */
/*        if (Input.touchCount <= 0)
        {
            return;
        }
        foreach (Touch touch in Input.touches)
        {
            if (touch.tapCount == 2 && isGrounded)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
                soundEffector.PlayJumpSound();
            }
        }*/
    }

    void FixedUpdate()
    {

    }
    void Flip()
    {
        if (Joystick.Horizontal > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Joystick.Horizontal < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
    void checkGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !onLadder)
        {
            anim.SetInteger("State", 3);
        }
    }

    public void Jumper()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
        }
    public void RecountHP(int deltaHP)
    {
        if (deltaHP < 0)
        {
            if (canHit)
            {
                curHP = curHP + deltaHP;
                StopCoroutine(OnHit());
                StartCoroutine(Damage());
                /*canHit = false;*/
                isHit = true;
                StartCoroutine(OnHit());
            }
        }
        else if (curHP > maxHP)
        {
            curHP += deltaHP;
            curHP = maxHP;
        }
        else
        {
            curHP = curHP + deltaHP;
        }
        if (curHP <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }
   public IEnumerator OnHit()
    {
        if (isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
        if (GetComponent<SpriteRenderer>().color.g == 1)
            StopCoroutine(OnHit());
            /*yield return new WaitForSeconds(5f);*/
            /*canHit = true;*/
        if (GetComponent<SpriteRenderer>().color.g <= 0)
            isHit = false;
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
        
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_key();
        }
        if (collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTp)
            {
                collision.gameObject.GetComponent<Door>().teleport(gameObject);
                canTp = false;
                StartCoroutine(tpWait());
            }
            else if (key)
                collision.gameObject.GetComponent<Door>().unlock();
        }
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coins++;
            soundEffector.PlayCoinSound();
        }
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            /*RecountHP(1);*/
            inventory.Add_hp();
        }
        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            RecountHP(-1);
        }
        if (collision.gameObject.tag == "Blue")
        {
            Destroy(collision.gameObject);
            /*StartCoroutine(Invulnerability());*/
            inventory.Add_bg();
        }
        if (collision.gameObject.tag == "Red")
        {
            Destroy(collision.gameObject);
            /*StartCoroutine(Superspeed());*/
            inventory.Add_rg();
        }
        if (collision.gameObject.tag == "Death")
        {
            Invoke("Lose", 0.5f);
        }
        if (collision.gameObject.tag == "Second")
        {
            secondClue.SetActive(true);
        }
        if (collision.gameObject.tag == "Third")
        {
            thirdClue.SetActive(true);
        }
        if (collision.gameObject.tag == "Fourth")
        {
            fourthClue.SetActive(true);
        }
    }
    IEnumerator tpWait()
    {
        yield return new WaitForSeconds(1f);
        canTp = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            onLadder = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (Joystick.Vertical == 0)
            {
                anim.SetInteger("State", 6);
            }
            else
            {
                anim.SetInteger("State", 5);
                transform.Translate(Vector3.up * Joystick.Vertical * 0.5f * speed * Time.deltaTime);
            }
        }
        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f;
            }
        }
        if (collision.gameObject.tag == "Lava")
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 3f)
            {
                hitTimer = 0f;
                PlayerCountdown.fillAmount = 1f;
                RecountHP(-1);
            }
            else
                PlayerCountdown.fillAmount = 1 - (hitTimer / 3f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            onLadder = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 7f)
            {
                rb.gravityScale = 1f;
                speed /= 0.25f;
            }
        }
        if (collision.gameObject.tag == "Lava")
        {
            hitTimer = 0f;
            PlayerCountdown.fillAmount = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        if (collision.gameObject.tag == "Quicksand")
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }
    }
    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);
    }
    IEnumerator Invulnerability()
    {
            Debug.Log("Cock");
            gemCount++;
            CheckGems(blue);
            canHit = false;
            blue.SetActive(true);
            blue.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(4f);
            StartCoroutine(Invis(blue.GetComponent<SpriteRenderer>(), 0.02f));
            yield return new WaitForSeconds(1f);
            canHit = true;
            blue.SetActive(false);
            gemCount--;
            CheckGems(red);
    }
    IEnumerator Superspeed()
    {
        gemCount++;
        red.SetActive(true);
        CheckGems(red);
        speed = speed * 2f;
        red.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(red.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2f;
        gemCount--;
        blue.SetActive(false);
        CheckGems(blue);
    }
    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            blue.transform.localPosition = new Vector3(-0.5f, 0.5f, blue.transform.localPosition.z);
            red.transform.localPosition = new Vector3(0.5f, 0.5f, red.transform.localPosition.z);
        }
    }
    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invis(spr, time));
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Quicksand")
        {
            speed /= 0.25f;
            rb.mass /= 100f;
        }
    }
    public int GetCoins()
    {
        return coins;
    }
    public int GetHP()
    {
        return curHP;
    }
    public void BlueGem()
    {
        StartCoroutine(Invulnerability());
    }
    public void RedGem()
    {
        StartCoroutine(Superspeed());
    }
    IEnumerator Damage()
    {
        canHit = false;
        yield return new WaitForSeconds(1f);
        canHit = true;
    }
}
