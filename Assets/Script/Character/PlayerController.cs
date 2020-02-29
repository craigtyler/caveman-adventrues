using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;

    public float moveForce = 300f;
    public float maxSpeed = 3f;

    private GameObject player;

    private Animator anim;

    private Rigidbody2D rb2d;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(player);
    }
    #region playermovement


    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetTrigger("playerWalkLeft");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            anim.SetTrigger("playerWalkUp");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetTrigger("playerWalkDown");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetTrigger("playerWalkRight");
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h * rb2d.velocity.x < maxSpeed)
        {
            rb2d.AddForce(Vector2.right * h * moveForce);
        }
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, 0f);
        }
        else
        {
            rb2d.velocity = new Vector2(0f, 0f);
        }

        if (v * (rb2d.velocity.y) < maxSpeed)
        {
            rb2d.AddForce(Vector2.up * v * moveForce);
        }
        if (Mathf.Abs(rb2d.velocity.y) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.y) * maxSpeed, 0f);
        }
        else
        {
            rb2d.velocity = new Vector2(0f, 0f);
        }

        if (h > 0 && !facingRight)
        {
            FlipRight();
        }
        else if (h < 0 && !facingRight)
        {
            FlipRight();
        }
    }
    void FlipRight()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    #endregion


    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.name == "Door_Up")
        {
            player.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + .3f);
        }
        else if (other.gameObject.name == "Door_Down")
        {
            player.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y - .3f);
        }
        else if (other.gameObject.name == "Door_Right")
        {
            player.transform.position = new Vector3((other.gameObject.transform.position.x + .3f), other.gameObject.transform.position.y);
        }
        else if (other.gameObject.name == "Door_Left")
        {
            player.transform.position = new Vector3((other.gameObject.transform.position.x - .3f), other.gameObject.transform.position.y);
        }
        if (other.gameObject.tag == "ExitDoor")
        {
            GameManager.shouldFinishRound = true;
        }
        if (other.gameObject.tag == "Enemy")
        {
            Battle.tempEnemy = other.gameObject;
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().SwitchScenes(2, true, false);
        }
        if (other.gameObject.tag == "Boss")
        {
            Battle.tempEnemy = other.gameObject;
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().SwitchScenes(2, true, false);
            
        }
        if (other.gameObject.tag == "MeleeWeapon")
        {
            Debug.Log("Hit Weapon");
            if (SceneManager.GetActiveScene().name != "HubScene")
            {
                player.GetComponent<StatController>().UpdateItemAndUI(false, 0, false, 0, true, other.gameObject.name, false);
            }
            player.GetComponent<StatController>().UpdateWeaponStats(other.gameObject.GetComponent<StatController>());
            player.GetComponent<StatController>().currentWeapon = other.name;
          	Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "RangeWeapon")
        {
            player.GetComponent<StatController>().UpdateItemAndUI(false, 0, false, 0, true, other.gameObject.name, false);
			player.GetComponent<StatController>().UpdateWeaponStats(other.gameObject.GetComponent<StatController>());
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Consumable")
        {
            if (other.name == "HealthPotion")
            {
                if (player.GetComponent<StatController>().potionCount < 5)
                {
                    Destroy(other.gameObject);
                    player.GetComponent<StatController>().UpdateItemAndUI(false, 0, true, 1, false, other.name, false);
                }
            }
            if (other.name == "Coin")
            {
                Destroy(other.gameObject);
                player.GetComponent<StatController>().UpdateItemAndUI(false, 0, false, 1, false, other.name, true);
            }
            if (other.name == "Chest")
            {
                if (other.GetComponent<SpriteRenderer>().sprite.name == "ChestClosed")
                {
                    other.GetComponent<SpriteController>().SpriteChange();
                    IntRange randGold = new IntRange(3, 25);
                    int gold = randGold.Random;
                    /* IntRange shouldBeMimic = new IntRange(0, 20);
                    if (shouldBeMimic.Random < 4)
                    {

                    } */
                    player.GetComponent<StatController>().UpdateItemAndUI(false, 0, false, gold, false, other.name, true);
                }
            }
        }
    }
}
