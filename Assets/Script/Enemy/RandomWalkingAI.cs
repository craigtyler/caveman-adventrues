using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalkingAI : MonoBehaviour
{
    public IntRange idleTime = new IntRange(1, 3);

    public int stateTO;

    public float moveForce;

    private Animator anim;
    private Rigidbody2D rb2d;
    public Dir dir;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    void Start()
    { 
        StartCoroutine("WalkLoop");
    }

    private IEnumerator WalkLoop()
    {
        while (true)
        {
            stateTO = idleTime.Random;
            yield return Walking();
            yield return Idle();
            yield return new WaitForSeconds(2);
        }
    }
    private IEnumerator Walking()
    {
        dir = (Dir)Random.Range(0, 4); //random direction
        switch (dir)
        {
            // +x is left; -x is right; +y is up; -y is down
            case Dir.Up:
                rb2d.AddForce(Vector2.up * moveForce);
                break;
            case Dir.Left:
                rb2d.AddForce(Vector2.left * moveForce);
                break;
            case Dir.Down:
                rb2d.AddForce(Vector2.down * moveForce);
                break;
            case Dir.Right:
                rb2d.AddForce(Vector2.right * moveForce);
                break;
        }
        yield return new WaitForSeconds(2);
    }
    private IEnumerator Idle()
    {
        rb2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(stateTO);
    }
}
