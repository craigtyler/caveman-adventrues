using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Battle : MonoBehaviour {

    public GameObject dmgTxtToEnemy;
    public GameObject dmgTxtToPlayer;
    public GameObject healText;
    public GameObject slashObj;
    public GameObject stabObj;
    public GameObject smashObj;

    private StatController PSC; //player SC
    private StatController ESC; //enemy SC
    [HideInInspector]
    public GameObject playerPos;
    [HideInInspector]
    public GameObject enemyPos;
    
    private string Player = "Player";
    private string Enemy = "Enemy";
    private string enemyTag;

    private GameManager gm;

    private bool endFight = false;
    private string dmgType = "";

    [HideInInspector]
    public static GameObject tempEnemy;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public GameObject enemy;
    public GameObject ExitDoor;

    public static string turn = "player";
    public Text MessageText;
    public Text PotionText;

    //stats for player
    public static int heroHp;
    public static int heroBaseAtt;
    public static int heroAtt;
    public static int heroPotions;

    //dmg modifiers from weapon
    public static int slashDmgMod;
    public static int stabDmgMod;
    public static int smashDmgMod;

    //stats for enemy
    public static int enemyHp;
    public static int enemyBaseAtt;
    public static int enemyAtt;

    public Button btnOption1;
    public Button btnOption2;
    public Button btnOption3;
    public Button btnUsePotion;

    private bool btnClicked = false;
    private bool btn1Click = false;
    private bool btn2Click = false;
    private bool btn3Click = false;
    private bool btnUsePotionClick = false;

    private bool usedPotion = false;
    private bool usedMove = false;

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        StartCoroutine(FightLoop());
    }

    private IEnumerator FightLoop()
    {
        yield return StartCoroutine(StartFight()); // done, shouldn't need test
        yield return StartCoroutine(Fighting()); // should be done, need test; 
        yield return StartCoroutine(EndFight()); // need to finish this
    }

    private IEnumerator StartFight()
    {
        /// Find positions for player and enemy
        enemyPos = GameObject.Find("EnemyPos").gameObject;
        playerPos = GameObject.Find("PlayerPos").gameObject;

        //gets stats from original player stat controller
        //had to move refrence to here, because if we find with tag, it'll get confused as to which we want
        PSC = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<StatController>();

        //sets player as a new instance of the player that was left in the exploring view;
        player = Instantiate(GameObject.FindGameObjectWithTag("Player").gameObject, playerPos.transform.position, Quaternion.identity) as GameObject;
        player.transform.parent = playerPos.transform;

        PotionText.text = "Health Potions:\n " + PSC.potionCount;

        //sets enemy as a copy of the enemy we bumped into
        enemy = Instantiate(tempEnemy, enemyPos.transform.position, Quaternion.identity) as GameObject;
        enemy.GetComponent<RandomWalkingAI>().enabled = false;
        enemy.transform.parent = enemyPos.transform;
        enemy.name = tempEnemy.name;

        //gets stats from the enemy
        ESC = enemy.GetComponent<StatController>();

        //turn off the camera of the new player
        player.GetComponentInChildren(typeof(Camera)).gameObject.SetActive(false);
        enemyTag = enemy.gameObject.tag;

        heroHp = PSC.currentHealth;
        heroBaseAtt = PSC.currentDmg;
        heroPotions = PSC.potionCount;
        slashDmgMod = PSC.slashDmg;
        stabDmgMod = PSC.stabDmg;
        smashDmgMod = PSC.smashDmg;

        enemyHp = ESC.currentHealth;
        enemyBaseAtt = ESC.currentDmg;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator Fighting()
    {
        while (endFight == false)
        {
            //do player turn
            yield return StartCoroutine(PlayerTurn());
            //calculate how much dmg the player did
            yield return StartCoroutine(DamageCalculate(Player));
            //check if the enemy is dead.. 
            if (enemyHp > 0)
            {
                //..if we didnt kill the enemy, do his turn
                yield return StartCoroutine(EnemyTurn());
                // then figure out his damage, and check if player ded
                yield return StartCoroutine(DamageCalculate(Enemy));
            }
        }
    }

    private IEnumerator PlayerTurn()
    {
        //wait until a button is clicked

        //lambda expression
        //method without a declaration, i.e., access modifier, return value declaration, and name
        yield return new WaitUntil(() => btnClicked == true);

        btnClicked = false;
        
        if (btn1Click == true)
        {
            dmgType = "slash";
            heroAtt = heroBaseAtt + slashDmgMod;
        }
        else if (btn2Click == true)
        {
            dmgType = "stab";
            heroAtt = heroBaseAtt + stabDmgMod;
        }
        else if (btn3Click == true)
        {
            dmgType = "smash";
            heroAtt = heroBaseAtt + smashDmgMod;
        }
        else if (btnUsePotionClick == true)
        {
            usedPotion = true;
        }
        btn1Click = false;
        btn2Click = false;
        btn3Click = false;
        btnUsePotionClick = false;
    }

    private IEnumerator DamageCalculate(string turn)
    {
        if (turn == Player)
        {
            while (usedMove == false)
            {
                if (usedPotion != true)
                {
                    usedMove = true;
                    enemyHp = enemyHp - heroAtt;
                    Instantiate(dmgTxtToEnemy, enemyPos.transform.position, Quaternion.identity);
                    enemy.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                    if (dmgType == "slash")
                    {
                        Instantiate(slashObj, enemyPos.transform.position, Quaternion.identity);
                    }
                    else if (dmgType == "stab")
                    {
                        Instantiate(stabObj, enemyPos.transform.position, Quaternion.identity);
                    }
                    else if (dmgType == "smash")
                    {
                        Instantiate(smashObj, enemyPos.transform.position, Quaternion.identity);
                    }
                    dmgType = string.Empty;
                    if (enemyHp <= 0)
                    {
                        endFight = true;
                    }
                    yield return new WaitForSeconds(.5f);
                    enemy.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                }
                else if (usedPotion == true && heroPotions > 0 && heroHp < PSC.maxHealth)
                {
                    usedMove = true;
                    Instantiate(healText, player.transform.position, Quaternion.identity);
                    player.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
                    heroHp++;
                    gm.UsePotion(gm.SC);
                    heroPotions--;
                    PotionText.text = "Health Potions:\n " + heroPotions;
                    yield return new WaitForSeconds(.5f);
                    player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                    yield return new WaitForSeconds(.5f);
                }
                if (usedPotion == true && endFight == false && usedMove == false) // add more to 
                {
                    if (heroHp >= PSC.maxHealth)
                    {
                        MessageText.text = "Already max health! Select an attack!";
                    }
                    else if (heroPotions <= PSC.maxPotionCount)
                    {
                        MessageText.text = "No Potions! Select an attack!";
                    }
                    yield return PlayerTurn();
                }
                MessageText.text = string.Empty;
                usedPotion = false;
            }
            usedMove = false;
        }
        if (turn == Enemy)
        {
            heroHp = heroHp - enemyAtt;
            Instantiate(dmgTxtToPlayer, playerPos.transform.position, Quaternion.identity);
            player.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            Instantiate(slashObj, playerPos.transform.position, Quaternion.identity);
            if (heroHp <= 0)
            {
                endFight = true;
            }
            yield return new WaitForSeconds(.5f);
            player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        IntRange randRange = new IntRange(0, 2);
        int rand = randRange.Random;

        if (rand == 0)
        {
            enemyAtt = enemyBaseAtt + 0;
        }
        if (rand == 1)
        {
            enemyAtt = enemyBaseAtt + 1;
        }
        yield return new WaitForSeconds(.1f);
    }

    private IEnumerator EndFight()
    {
        MessageText.text = string.Empty;
        if (heroHp <= 0)
        {
            Debug.Log("Player Dead");
            MessageText.text = "You've been defeated by the " + enemy.name;
            yield return new WaitForSeconds(1f);
            //add a cut scene for the player being dead
            PSC.UpdateHP(0);
            yield return new WaitForSeconds(.05f);
            gm.SwitchScenes(2, false, true);
            StartCoroutine(gm.UnloadGame());
        }
        else if (enemyHp <= 0)
        {
            MessageText.text = "You've defeated the " + enemy.name;
            if (enemyTag == "Boss")
            {
                Instantiate(ExitDoor, tempEnemy.transform.position, Quaternion.identity);
            }
            if (enemyTag != "Boss")
            {
                ChanceToSpawnItem();
            }
            Destroy(tempEnemy);
            if (PSC.currentHealth > heroHp) //if the health we started with is less than the health we ended with, then we do this
            {
                int hpDifference = heroHp - PSC.currentHealth;
                PSC.UpdateItemAndUI(true, hpDifference, false, 0, false, PSC.name, false);
            }
            PSC.UpdateHP(heroHp);
            if (PSC.potionCount > heroPotions)
            {
                int potionDif = heroPotions - PSC.potionCount;
                PSC.UpdateItemAndUI(false, 0, true, potionDif, false, PSC.name, false);
            }
            yield return new WaitForSeconds(2f);
            gm.SwitchScenes(2, false, true);
            //switch back to overview 
        }
    }

    private void ChanceToSpawnItem()
    {
        IntRange ChanceToSpawn = new IntRange(1, 20);
        int randChance = ChanceToSpawn.Random;
        IntRange randItem = new IntRange(0, GameManager.ItemList.Count);
        int moreRand = randItem.Random;
        if (randChance < 18)
        {
            GameObject obj = Instantiate(GameManager.ItemList[moreRand], tempEnemy.transform.position, Quaternion.identity) as GameObject;
            obj.name = GameManager.ItemList[moreRand].name;
        }
    }

    public void CheckButtonClick(Button btn)
    {
        if (btn == btnOption1)
        {
            btnClicked = true;
            btn1Click = true;
        }
        if (btn == btnOption2)
        {
            btnClicked = true;
            btn2Click = true;
        }
        if (btn == btnOption3)
        {
            btnClicked = true;
            btn3Click = true;
        }
        if (btn == btnUsePotion)
        {
            btnClicked = true;
            btnUsePotionClick = true;
        }
    }
}
