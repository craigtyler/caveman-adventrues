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

    public static GameObject tempEnemy;

    public GameObject player;
    public GameObject enemy;

    public static string whoseTurn = "player";
    public Text MessageText;

    public static float heroHP;
    public static float heroAttPow;

    public static float DOTDur=0;
    public static float DOTDam=0;

    public static float enemyHP;
    public static float enemyAttPow;

    public KeyCode skill1;
    public KeyCode skill2;
    public KeyCode skill3;
    public KeyCode skill4;
    public KeyCode skill5;
    public KeyCode skill6;

    void Awake()
    {
        gm = new GameManger();
        GetInfo();
        //create code to find the player object, then set the player object with the sprite and the weapon he may have //done
        //do the same for the enemy object
    }

    public void GetInfo()
    {
        Debug.Log("Started");
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
        BattleFlow.heroHP = PSC.health;
        BattleFlow.heroAttPow = PSC.damage;
        BattleFlow.enemyHP = ESC.health;
        BattleFlow.enemyAttPow = ESC.damage;
    }

	// Update is called once per frame
	void Update () {
        //have to change to iEnumerator
        if ((Input.GetKeyUp(skill1)) && (BattleFlow.whoseTurn == "player"))
        {
            //Debug.Log("Sucess!!");
            BattleFlow.heroAttPow += 1;
            BattleFlow.whoseTurn = "neither";
            Instantiate(slashObj, enemyPos.transform.position, Quaternion.identity);
        }
        if ((Input.GetKeyUp(skill2)) && (BattleFlow.whoseTurn == "player"))
        {
            //Debug.Log("Sucess!!");
            BattleFlow.heroAttPow += 2;
            BattleFlow.whoseTurn = "neither";
        }
        if ((Input.GetKeyUp(skill3)) && (BattleFlow.whoseTurn == "player"))
        {
            //Debug.Log("Sucess!!");
            BattleFlow.DOTDur = 4;
            BattleFlow.DOTDam = 30;
            BattleFlow.whoseTurn = "neither";
            Instantiate(poisonObj, enemyPos.transform.position, transform.rotation);
        }
        if (BattleFlow.whoseTurn == "neither")
        {
            BattleFlow.enemyHP -=BattleFlow.heroAttPow;

            if (enemyHP <= 0)
            {
                MessageText.text = "You Win";
                new WaitForSeconds(2f);
                MessageText.text = string.Empty;
                PSC.UpdateHP(heroHP);
                Destroy(tempEnemy);
                gm.SwitchScenes(1, false, true);
            }

            if (BattleFlow.DOTDur>=1)
            {
                BattleFlow.DOTDur-=1;
                BattleFlow.enemyHP-=BattleFlow.DOTDam;
                Instantiate(DOTDamage, enemyPos.transform.position, DOTDamage.transform.rotation);
            }

            //Debug.Log (BattleFlow.enemyHP);
            Instantiate(attackDamage, new Vector3(-8, 1, 0),attackDamage.transform.rotation);
            enemy.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            BattleFlow.whoseTurn = "effectsdelay";
            StartCoroutine(flashdelay());
        }
        if (BattleFlow.whoseTurn == "enemy")
        {
            BattleFlow.heroHP -= BattleFlow.enemyAttPow;
            if (heroHP < 0)
            {

            }

            Debug.Log (BattleFlow.enemyHP+"    "+BattleFlow.heroHP);
            BattleFlow.whoseTurn = "player";
        }
    }

    IEnumerator flashdelay()
    {
        yield return new WaitForSeconds(.5f);
        enemy.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        BattleFlow.whoseTurn = "enemy";
    }
}
