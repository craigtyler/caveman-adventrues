using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HubScript : MonoBehaviour {

    public Canvas PauseMenu;

	public Canvas canvas;
	public Text text;
	public Image image;
    public Image transtionImg;
    public Button btnYes;
    public Button btnNo;

    public GameObject playerSpawn;
    public GameObject weaponSpawn;

    //gameobject triggers
    public GameObject UpTrigger;
    public GameObject DownTrigger;
    public GameObject RightTrigger;
    public GameObject LeftTrigger;

    public GameObject HubTrigger;
    public GameObject DoorTrigger;
    public GameObject CaveTrigger;
    public GameObject TrapDoor;

    //gameobject spawns
    public GameObject OWUpSpawn;
    public GameObject OWDownSpawn;
    public GameObject OWLeftSpawn;
    public GameObject OWRightSpawn;

    public GameObject HubSpawn;
    public GameObject DoorSpawn;
    public GameObject CaveSpawn;
    public GameObject TrapDoorSpawn;

	public static bool isShown = true;
    public static bool isItChecked = false;
    public static bool changePos = false;
    public static bool talkedToWiz = false;
    public static bool finishedQuest = false;

    public GameObject[] Weapons;

    private void Start()
    {
        TogglePauseMenu();
        if (GameObject.FindGameObjectsWithTag("Player").Length != 1)
        {
            GameObject player = Instantiate(Resources.Load("Player/Player")) as GameObject;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().currentHealth <= 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = CaveSpawn.transform.position;
            if (GameObject.FindGameObjectsWithTag("GM").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("GM"));
            }
        }
        else if (GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().currentHealth > 0 && finishedQuest == false)
        {
            if (GameObject.FindGameObjectsWithTag("GM").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("GM"));
            }
            GameObject.FindGameObjectWithTag("Player").transform.position = playerSpawn.transform.position;
        }
        else if(GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().currentHealth > 0 && finishedQuest == true)
            {
            if (GameObject.FindGameObjectsWithTag("GM").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("GM"));
            }
            GameObject.FindGameObjectWithTag("Player").transform.position = TrapDoorSpawn.transform.position;
        }
        EnableControl();
    }

    #region PauseMenu

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (PauseMenu.GetComponentInChildren<Canvas>().enabled == true)
        {
            PauseMenu.GetComponentInChildren<Canvas>().enabled = false;
            Time.timeScale = 1;
        }
        else if (PauseMenu.GetComponentInChildren<Canvas>().enabled == false)
        {
            PauseMenu.GetComponentInChildren<Canvas>().enabled = true;
            Time.timeScale = 0;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    public IEnumerator ShowPrompt()
	{
        DisableControl();
        isShown = true;
        isItChecked = false;
        changePos = false;
        if (isShown == true)
        {
            text.color = new Color(.2f, .2f, .2f, 1f);
            image.color = new Color(1f, 1f, 1f, 1f);
            btnYes.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            btnYes.GetComponentInChildren<Text>().color = new Color(.2f, .2f, .2f, 1f);
            btnNo.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            btnNo.GetComponentInChildren<Text>().color = new Color(.2f, .2f, .2f, 1f);
        }
        yield return new WaitUntil(() => isItChecked == true);
        isShown = false;
        if (isShown == false)
        {
            text.color = new Color(1f, 1f, 1f, 0f);
            image.color = new Color(1f, 1f, 1f, 0f);
            btnYes.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            btnYes.GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f, 0f);
            btnNo.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            btnNo.GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f, 0f);
        }
        EnableControl();
        yield return null;
	}
    public IEnumerator ShowErrorPrompt(string errorString)
    {
        DisableControl();
        isShown = true;
        if (isShown == true)
        {
            text.text = errorString;
            text.color = new Color(.2f, .2f, .2f, 1f);
            image.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(2f);
        }
        isShown = false;
        if (isShown == false)
        {
            text.color = new Color(1f, 1f, 1f, 0f);
            image.color = new Color(1f, 1f, 1f, 0f);
            text.text = "Would you like to move to the next area?";//set back to the original message
        }
        EnableControl();
        yield return null;
    }

    public IEnumerator ShowTransition()
    {
        transtionImg.color = new Color(0f, 0f, 0f, .353f);
        yield return new WaitForSeconds(0.1f);
        transtionImg.color = new Color(0f, 0f, 0f, .686f);
        yield return new WaitForSeconds(0.1f);
        transtionImg.color = new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.5f);
        transtionImg.color = new Color(0f, 0f, 0f, .686f);
        yield return new WaitForSeconds(0.1f);
        transtionImg.color = new Color(0f, 0f, 0f, .353f);
        yield return new WaitForSeconds(0.1f);
        transtionImg.color = new Color(0f, 0f, 0f, 0f);
        yield return null;
    }

	//if we hit the OWUpSpawn then we go to Hub Spawn, If we hit the Hub Spawn, then we go to OWSpawn
	public IEnumerator ChangePos(GameObject newPos)
	{
        yield return new WaitUntil(() => isShown == false);
        if (changePos == true)
        {
            if (newPos.name == UpTrigger.name)//go to the hub
            {
                StartCoroutine(ShowTransition());
                yield return new WaitForSeconds(.8f);
                GameObject.FindGameObjectWithTag("Player").transform.position = HubSpawn.transform.position;
            }
            if (newPos.name == HubTrigger.name) //go back to the OW
            {
                StartCoroutine(ShowTransition());
                yield return new WaitForSeconds(.8f);
                GameObject.FindGameObjectWithTag("Player").transform.position = OWUpSpawn.transform.position;
            }
            if (newPos.name == DoorTrigger.name)// go to the cave 
            {
                StartCoroutine(ShowTransition());
                yield return new WaitForSeconds(.8f);
                GameObject.FindGameObjectWithTag("Player").transform.position = CaveSpawn.transform.position;
            }
            if (newPos.name == CaveTrigger.name) // go to the cave entrance
            {
                StartCoroutine(ShowTransition());
                yield return new WaitForSeconds(.8f);
                GameObject.FindGameObjectWithTag("Player").transform.position = DoorSpawn.transform.position;
            }
            if (newPos.name == TrapDoor.name)
            {
                StartCoroutine(ShowTransition());
                yield return new WaitForSeconds(.8f);
                SceneManager.LoadSceneAsync(1);
            }
        }
		yield return null;
	}
    public IEnumerator CutSceneStartQuest()
    {
        DisableControl();
        text.color = new Color(.2f, .2f, .2f, 1f);
        image.color = new Color(1f, 1f, 1f, 1f);
        text.text = "Wizard:  If you're gonna come into my abode make yourself..";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  ... Oooffff";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Another simple minded beast stumbling in here.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  ...";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  I'll take care of this, hold still..";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  Ugguh!";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Sanguinem vicissim, I bind you to me..";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Now you will be as intelligent as me.. ";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard: ..Almost.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Now go venture down that hatch outside, \nand retrieve my possesion!";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  I'll Procure what you seek, just don't turn me back.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Take this weapon behind you, it'll help protect you.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        SpawnWeapon();
        text.text = "CaveBoy:  (This is insane.. I can't believe I've been this stupid..)";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  (And now I'm forced to be a minion or lose all I know.)";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.color = new Color(.2f, .2f, .2f, 0f);
        image.color = new Color(1f, 1f, 1f, 0f);
        text.text = "Would you like to move to the next area?";//set back to the original message
        EnableControl();
        yield return null;
    }

    public IEnumerator CutSceneAfterDeath()
    {
        DisableControl();
        text.color = new Color(.2f, .2f, .2f, 1f);
        image.color = new Color(1f, 1f, 1f, 1f);
        text.text = "CaveBoy:  ...How did I get here?";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Well.. You had died..";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  I was dead?!";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Yes, WAS. But I resurrected you just then.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  Seriously? This is really weird..";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  You'll have to go back down into the caves again, you \n are still  indebted to me, remember?";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "CaveBoy:  Alright, alright. I'll go back down.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.text = "Wizard:  Try and not die again, I don't want to have\n to do that again.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(.3f);
        text.color = new Color(.2f, .2f, .2f, 0f);
        image.color = new Color(1f, 1f, 1f, 0f);
        text.text = "Would you like to move to the next area?";//set back to the original message
        GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().UpdateHP(10);
        EnableControl();
        yield return null;
    }

    void SpawnWeapon()
    {
        Weapons = new GameObject[10];
        int temp = 0;
        foreach (var weapons in Resources.LoadAll("Weapon"))
        {
            Weapons[temp] = weapons as GameObject;
            temp++;
        }
        IntRange rand = new IntRange(0, Weapons.Length);
        int randWeapon = rand.Random;
        GameObject weapon = Instantiate(Weapons[randWeapon], weaponSpawn.transform.position, Quaternion.identity);
        weapon.name = Weapons[randWeapon].name;
    }

    public void GetButtonPrompt(Button btn)
    {
        if (btn == btnYes)
        {
            changePos = true;
        }
        if (btn == btnNo)
        {
            changePos = false;
        }
        isItChecked = true;
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
}