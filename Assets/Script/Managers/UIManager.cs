using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] Hearts;
    public GameObject[] Potions;

    public static List<GameObject> heartList;
    public static List<GameObject> potionList;
    public static List<Sprite> itemList;

    public static List<GameObject> heartListBattle;
    public static List<GameObject> potionListBattle;

    public GameObject heartHolder;
    public GameObject potionHolder;
    public GameObject weaponHolder;

    public Canvas canvas;
    public Canvas PauseMenu;

    public Sprite Heart;
    public Sprite Potion;
    public Sprite Coin;
    public Sprite BlankSprite;

    private StatController PSC;

    private static bool btnClicked = false;
    public bool transitionFinished = false;


    void Awake()  //need fix; need to do the weapon swap on the UI too
    {
        Invoke("GetPSC", 0.1f);
        TogglePauseMenu();
        StartCoroutine("GetLists");
    }

    #region pauseMenu controls

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

    void GetPSC()
    {
        PSC = GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>(); //get the PSC after the player should of spawned
        canvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //piggybacking off this Invoke, we'll get the camera from the player too; so the UI isn't buggy.
    }

    #region battleUI

    private IEnumerator GetListForBatttleUI()
    {
        if (heartListBattle == null)
        {
            heartListBattle = new List<GameObject>();
            for (int i = 0; i < Hearts.Length; i++)
            {
                heartListBattle.Add(Hearts[i]);
            }
        }
        if (potionList == null)
        {
            potionListBattle = new List<GameObject>();
            for (int i = 0; i < Potions.Length; i++)
            {
                potionListBattle.Add(Potions[i]);
            }
        }
        yield break;
    }

    #endregion

    private IEnumerator GetLists()
    {
        if (heartList == null)
        {
            heartList = new List<GameObject>();
            for (int i = 0; i < Hearts.Length; i++)
            {
                heartList.Add(Hearts[i]);
            }
        }
        if (potionList == null)
        {
            potionList = new List<GameObject>();
            for (int i = 0; i < Potions.Length; i++)
            {
                potionList.Add(Potions[i]);
            }
        }
        if (itemList == null)
        {
            itemList = new List<Sprite>
            {
                Heart,
                Potion,
                Coin,
                BlankSprite
            };
            yield return new WaitUntil(() => GameManager.haveLists == true);
            foreach (var weapon in GameManager.WeaponList)
            {
                itemList.Add(weapon.GetComponent<SpriteRenderer>().sprite);
            }
        }
    }

    public void UpdateHp(int currentHP, int howMuchChanged, bool gainHp)
    {
        int activeHearts = 0;

        Sprite heart = null;
        Sprite blank = null;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].name == "Heart")
            {
                heart = itemList[i];
                break;
            }
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].name == "UIMask")
            {
                blank = itemList[i];
                break;
            }
        }

        int count = 0;
        while (count < heartList.Count )
        {
            if (heartList[count].GetComponent<Image>().sprite == heart)
            {
                activeHearts++;
            }
            count++;
        }

        int newHp = activeHearts + howMuchChanged;
        while (activeHearts < newHp && gainHp == true && activeHearts != newHp && activeHearts <= PSC.maxHealth && newHp <= PSC.maxHealth) // if hp is less then old hp then add a heart sprite till activeHearts == currentHp
        {
            heartList[activeHearts].GetComponent<Image>().sprite = heart; //-1 for array
            activeHearts++;
        }
        while (activeHearts > newHp && gainHp == false && activeHearts != newHp && activeHearts <= PSC.maxHealth && newHp <= PSC.maxHealth) //if hp is more than old hp remove heart by making heart sprite null till activehearts == currentHp
        {
            heartList[activeHearts - 1].GetComponent<Image>().sprite = blank; //-1 for array
            activeHearts--;
        }
    }
    public void UpdateItem(int currentItems, int newItems, bool addPots)
    {
        int activePotions = 0; //how many we have currently

        Sprite potion = null;
        Sprite blank = null;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].name == "HealthPotion")
            {
                potion = itemList[i];
                break;
            }
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].name == "UIMask")
            {
                blank = itemList[i];
                break;
            }
        }

        int count = 0;
        while (count < potionList.Count)
        {
            if (potionList[count].GetComponent<Image>().sprite == potion)
            {
                activePotions++;
            }
            count++;
        }
        int temp = activePotions + newItems;
        while (activePotions < 5 && activePotions != temp && addPots == true) // if we have less than our current pots, but not more than the max of 5 and our active pots are not == our current pots; add one
        {
            potionList[currentItems].GetComponent<Image>().sprite = potion;
            PSC.potionCount++;
            activePotions++;
        }
        while (activePotions > 0 && activePotions <= 5 && activePotions != temp && addPots == false && PSC.currentHealth < 10) // if we have more than our current pots, but not more than the max of 5 and our active pots are not == our current pots; subtract one
        {
            potionList[temp].GetComponent<Image>().sprite = blank;
            PSC.potionCount--;
            activePotions--; //subtract 1
            PSC.currentHealth = PSC.currentHealth + PSC.potionHeal; // add to health
        }
    }
    public void UpdateWeapon(string weaponName)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (weaponName == itemList[i].name)
            {
                weaponHolder.GetComponent<Image>().sprite = itemList[i];
            }
        }
    }

    public void UpdateCoin(int howManyChanged)
    {
        PSC.coinCount += howManyChanged;
        GameObject coinUI = GameObject.FindGameObjectWithTag("CoinUI");
        foreach (var text in coinUI.GetComponentsInChildren<Text>())
        {
            if (text.name == "CoinCounter")
            {
                text.text = "x " + PSC.coinCount;
            }
        }
    }

    #region  UI Control for addition weapon UI

    public void ShowMore()
    {
        GameObject moreUI = GameObject.FindGameObjectWithTag("MoreUI");
        if (btnClicked) // if false then have the UI disappeared
        {
            moreUI.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            foreach (var item in moreUI.GetComponentsInChildren<Text>())
            {
                item.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        else // if true, then show moreUI, and have stats updated
        {
            moreUI.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            foreach (var item in moreUI.GetComponentsInChildren<Text>())
            {
                item.color = new Color(.353f, .353f, .353f, 1f);
                if (item.name == "SlashDmg")
                {
                    item.text = "Slash: " + PSC.slashDmg.ToString();
                }
                if (item.name == "StabDmg")
                {
                    item.text = "Stab: " + PSC.stabDmg.ToString();
                }
                if (item.name == "SmashDmg")
                {
                    item.text = "Smash: " + PSC.smashDmg.ToString();
                }
            }
        }
        btnClicked = !btnClicked; // change the bool from false to true, then if clicked again revert repeat
    }



#endregion

    #region UI control for above player

    public void PlayerUI(int newItems, bool isHp, bool isPotion, bool isCoin)
    {
        GameObject moreUI = GameObject.FindGameObjectWithTag("PlayerUI");
        Image image = moreUI.GetComponentInChildren<Image>();
        Text text = moreUI.GetComponentInChildren<Text>();

        Sprite potion = null;
        Sprite heart = null;
        Sprite coin = null;
        Sprite none = null;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].name == "HealthPotion")
            {
                potion = itemList[i];
            }
            if (itemList[i].name == "Heart")
            {
                heart = itemList[i];
            }
            if (itemList[i].name == "Coin")
            {
                coin = itemList[i];
            }
            if (itemList[i].name == "UIMask")
            {
                none = itemList[i];
            }
        }

        if (isHp == true)
        {
            image.color = new Color(1f, 1f, 1f, 1f);
            text.color = new Color(1f, 1f, 1f, 1f);
            image.sprite = heart;
            text.text = "+" + newItems.ToString();
            StartCoroutine(ResetImageAndText(none));
        }
        if (isPotion == true)
        {
            image.color = new Color(1f, 1f, 1f, 1f);
            text.color = new Color(1f, 1f, 1f, 1f);
            image.sprite = potion;
            text.text = "+" + newItems.ToString();
            StartCoroutine(ResetImageAndText(none));
        }
        if (isCoin == true)
        {
            image.color = new Color(1f, 1f, 1f, 1f);
            text.color = new Color(1f, 1f, 1f, 1f);
            image.sprite = coin;
            text.text = "+" + newItems.ToString();
            StartCoroutine(ResetImageAndText(none));
        }
    }

    private IEnumerator ResetImageAndText(Sprite blank)
    {
        GameObject moreUI = GameObject.FindGameObjectWithTag("PlayerUI");
        Image image = moreUI.GetComponentInChildren<Image>();
        Text text = moreUI.GetComponentInChildren<Text>();

        yield return new WaitForSeconds(1f);
        image.sprite = blank;
        image.color = new Color(1f, 1f, 1f, 0f);
        text.text = string.Empty;
        text.color = new Color(1f, 1f, 1f, 0f);
    }

#endregion

    public int GetCount()
    {
        int tick = 0;
        int total = 0;

        Sprite potion = null;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].name == "HealthPotion")
            {
                potion = itemList[i];
                break;
            }
        }

        while (tick < potionList.Count)
        {
            if (potionList[tick].GetComponent<Image>().sprite == potion)
            {
                total++;
            }
            tick++;
        }
        return total;
    }
    public void SetImage()
    {
        foreach (var weapon in GameManager.WeaponList)
        {
            if (weapon.name == PSC.currentWeapon)
            {
                weaponHolder.GetComponent<Image>().sprite = weapon.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}