using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BattleFlow : MonoBehaviour {

    public GameObject attackDamage;
    public GameObject DOTDamage;

    public GameObject slashObj;
    public GameObject poisonObj;

    private StatController PSC;
    private StatController ESC;

    [HideInInspector]
    public GameObject playerPos;
    [HideInInspector]
    public GameObject enemyPos;

    private GameManger gm;

    private bool endFighting = false;

    public static GameObject tempEnemy;

    public GameObject player;
    public GameObject enemy;

    public static string whoseTurn = "player";
    public Text MessageText;

    public static float heroHP;
    public static float heroBaseAttPow;
    public static float heroAttPow;

    public static float DOTDur = 0;
    public static float DOTDam = 0;

    public static float enemyHP;
    public static float enemyAttPow;

    public Button btnOption1;
    public Button btnOption2;
    public Button btnOption3;


    public KeyCode skill1;
    public KeyCode skill2;
    public KeyCode skill3;
    public KeyCode skill4;
    public KeyCode skill5;
    public KeyCode skill6;

    void Awake()
    {
        gm = new GameManger();
        Button btn1 = btnOption1.GetComponent<Button>();
        Button btn2 = btnOption2.GetComponent<Button>();
        Button btn3 = btnOption3.GetComponent<Button>();
        StartCoroutine(FightLoop());
        //create code to find the player object, then set the player object with the sprite and the weapon he may have //done
        //do the same for the enemy object
    }

    private IEnumerator FightLoop()
    {
        yield return StartCoroutine(StartFighting());
        yield return StartCoroutine(Fighting());
        yield return StartCoroutine(EndFighting());
        gm.SwitchScenes(1, false, true);
    }

    private IEnumerator StartFighting()
    {
        Debug.Log("Started Fighting");
        //get the enemy/player positions
        enemyPos = GameObject.Find("EnemyPos").gameObject;
        playerPos = GameObject.Find("PlayerPos").gameObject;

        //set player to a new instance of the player prefab that was left in the exploring view
        player = Instantiate(GameObject.FindGameObjectWithTag("Player").gameObject, playerPos.transform.position, Quaternion.identity) as GameObject;
        player.transform.parent = playerPos.transform;

        enemy = Instantiate(tempEnemy, enemyPos.transform.position, Quaternion.identity) as GameObject;
        enemy.GetComponent<RandomWalkingAI>().enabled = false;
        enemy.transform.parent = enemyPos.transform;

        //gets Stats from script so they can be used
        PSC = player.GetComponent<StatController>();
        ESC = enemy.GetComponent<StatController>();
        //disable the player camera so it doesnt break
        player.GetComponentInChildren(typeof(Camera)).gameObject.SetActive(false);

        //sets all values in the beginning
        heroHP = PSC.health;
        heroBaseAttPow = PSC.damage;
        enemyHP = ESC.health;
        enemyAttPow = ESC.damage;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator Fighting()
    {
        Debug.Log("Am Fighting");

        heroAttPow = 0;

        if (btnOption1 && (whoseTurn == "player"))
        {
            //Debug.Log("Sucess!!");
            heroAttPow = heroBaseAttPow + 1;
            whoseTurn = "neither";
            Instantiate(slashObj, enemyPos.transform.position, Quaternion.identity);
        }
        if (btnOption2 && (whoseTurn == "player"))
        {
            //Debug.Log("Sucess!!");
            heroAttPow = heroBaseAttPow + 2;
            whoseTurn = "neither";
        }
        if (btnOption3 && (whoseTurn == "player"))
        {
            //Debug.Log("Sucess!!");
            DOTDur = 4;
            DOTDam = 30;
            whoseTurn = "neither";
            Instantiate(poisonObj, enemyPos.transform.position, transform.rotation);
        }
        if (whoseTurn == "neither")
        {
            enemyHP -= heroAttPow;

            if (DOTDur >= 1)
            {
                DOTDur -= 1;
                enemyHP -= DOTDam;
                Instantiate(DOTDamage, enemyPos.transform.position, DOTDamage.transform.rotation);
            }

            if (enemyHP <= 0)
            {
                MessageText.text = "Battle Won";
                Destroy(tempEnemy);
                endFighting = true;
            }

            //Debug.Log (BattleFlow.enemyHP);
            Instantiate(attackDamage, new Vector3(-8, 1, 0), attackDamage.transform.rotation);
            enemy.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            whoseTurn = "effectsdelay";
            StartCoroutine(flashdelay());
        }
        if (whoseTurn == "enemy")
        {
            heroHP -= enemyAttPow;

            if (heroHP <= 0)
            {
                MessageText.text = "You are Dead";
                endFighting = true;
            }
            whoseTurn = "player";
        }

        while (endFighting == false)
        {
            yield return null;
        }
    }

    private IEnumerator EndFighting()
    {
        PSC.UpdateHP(heroHP);
        MessageText.text = string.Empty;

        yield return new WaitForSeconds(2f);
    }

    IEnumerator flashdelay()
    {
        yield return new WaitForSeconds(.5f);
        enemy.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        whoseTurn = "enemy";
    }
}
