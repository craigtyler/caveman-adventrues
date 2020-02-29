using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textcontrol : MonoBehaviour
{
    void Start()
    {
        if (gameObject.GetComponent<TextMesh>() != null)
        {
            gameObject.GetComponent<Renderer>().sortingOrder = 10;
        }
    }

    void Update()
    {
        if (gameObject.name == "enemyText")
        {
            GetComponent<Text>().text = "Enemy\nHP : " + Battle.enemyHp;
        }
        if (gameObject.name == "playerText")
        {
            GetComponent<Text>().text = "Player\nHP : " + Battle.heroHp;
        }
        if (gameObject.name == "DmgTextToEnemy(Clone)")
        {
            GetComponent<TextMesh>().text = Battle.heroAtt.ToString();
        }
        if (gameObject.name == "DmgTextToPlayer(Clone)")
        {
            GetComponent<TextMesh>().text = Battle.enemyAtt.ToString();
        }
        if (gameObject.name == "HealText(Clone)")
        {
            GetComponent<TextMesh>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().potionHeal.ToString();
        }
    }
}
