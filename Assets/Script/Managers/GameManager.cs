using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> EnemyList;
    public static List<GameObject> BossList;
    public static List<GameObject> WeaponList;
    public static List<GameObject> ItemList;

    public static bool haveLists = false;

    public int NumFloorsToWin;
    public int CurrentFloor;
    public float StartDelay = 3f;
    public float EndDelay = 2f;
    public Text MessageText;
    public static bool shouldFinishRound = false;

    public static bool loadedScene = false;

    private BoardGenV2 boardGen;
    private PlayerController PC;
    private UIManager UI;
    [HideInInspector]public StatController SC;  

    private WaitForSeconds StartWait;
    private WaitForSeconds EndWait;

    void Start()
    {
        boardGen = GameObject.Find("BoardManager").GetComponent<BoardGenV2>();
        UI = gameObject.GetComponent<UIManager>();
        Invoke("GetPSC", 0.5f);
        Invoke("StartGame", .01f);
    }
    void GetPSC()
    {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SC = GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>();
        SC.potionCount = UI.GetCount();
        UI.SetImage();
    }

    public void GetEnemyList(GameObject[] enemy, GameObject[] boss)
    {
        if (EnemyList == null)
        {
            EnemyList = new List<GameObject>();
            for (int i = 0; i < enemy.Length; i++)
            {
                EnemyList.Add(enemy[i]);
            }
        }
        if (BossList == null)
        {
            BossList = new List<GameObject>();
            for (int i = 0; i < boss.Length; i++)
            {
                BossList.Add(boss[i]);
            }
        }
    }
    public void GetItemList(GameObject[] weapon, GameObject[] items)
    {
        if (WeaponList == null)
        {
            WeaponList = new List<GameObject>();
            for (int i = 0; i < weapon.Length; i++)
            {
                WeaponList.Add(weapon[i]);
            }
        }
        if (ItemList == null)
        {
            ItemList = new List<GameObject>();
            for (int i = 0; i < items.Length; i++)
            {
                ItemList.Add(items[i]);
            }
        }
        if (WeaponList != null && ItemList != null)
        {
            haveLists = true;
        }
    }

    public void StartGame()
    {
        StartWait = new WaitForSeconds(StartDelay);
        EndWait = new WaitForSeconds(EndDelay);

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        shouldFinishRound = false;

        Debug.Log("running RoundStarting");
        yield return StartCoroutine(RoundStarting());
        Debug.Log("stopped RoundStarting, starting Roundplaying");
        yield return StartCoroutine(RoundPlaying());
        Debug.Log("stopped RoundPlaying, starting roundEnding");
        yield return StartCoroutine(RoundEnding());
        Debug.Log("stopped RoundEnding, checking if game over");
        if (CurrentFloor > NumFloorsToWin)
        {
            HubScript.finishedQuest = true;
            Debug.Log("Win");
            //add the final cut scene or something
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
        else 
        {
            Debug.Log("GameOver");
            Invoke("StartGame", 1);
            boardGen.ResetLevel();
        }
    }
    private IEnumerator RoundStarting()
    {
        MessageText.text = string.Empty;

        DisableControl();

        MessageText.text = "Floor " + CurrentFloor;

        yield return StartWait;
    }
    private IEnumerator RoundPlaying()
    {
        EnableControl();

        MessageText.text = string.Empty;

        while (shouldFinishRound == false)
        {
            yield return null;
        }
    }
    private IEnumerator RoundEnding()
    {
        DisableControl();

        MessageText.text = "Floor Complete!";

        CurrentFloor++;

        yield return EndWait;

        GameObject.FindGameObjectWithTag("ExitDoor").GetComponent<SpriteController>().SpriteChange();

        MessageText.text = string.Empty;

        yield return new WaitForSeconds(1.5f);
    }
    private void EnableControl()
    {
        GameObject player;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PlayerController>().enabled = true;
    }
    private void DisableControl()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
    }
    
    private bool ShouldFinishGame()
    {
        return CurrentFloor > NumFloorsToWin;
    }
    // scene index of 0 is hub; 1 is dungeon; 2 is battle
    public void SwitchScenes(int sceneIndex, bool loadScene, bool unloadScene)
    {
        //(what scene we're loading, if we're loading it, or unloading)
        DisableControl();
        if (loadScene == true && loadedScene == false)
        {
            loadedScene = true;
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        }
        if (unloadScene == true && loadedScene == true)
        {
            loadedScene = false;
            SceneManager.UnloadSceneAsync(sceneIndex);
            EnableControl();
        }
    }

    public IEnumerator UnloadGame()
    {
        //player died, so we need to add cut scene

        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        yield return null;
    }

    public void UsePotion(StatController PSC)
    {
        PSC = SC;
        if (PSC.potionCount > 0 && PSC.currentHealth < PSC.maxHealth)
        {
            PSC.UpdateItemAndUI(true, 1, true, -1, false, PSC.name, false);
        }
    }
}