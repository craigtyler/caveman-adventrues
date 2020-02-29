using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptControl : MonoBehaviour 
{
	private HubScript hm;

	void Start()
	{
		hm = GameObject.Find ("HubManager").GetComponent<HubScript>();
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.name != "DownTrigger" && this.gameObject.name != "LeftTrigger" && this.gameObject.name != "RightTrigger" && this.gameObject.name != "TrapDoorTrigger" && this.gameObject.name != "Wizard")
        {
            if (other.tag == "Player")
            {
                if (this.gameObject.name == "CaveTrigger" && GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().currentHealth <= 0)
                {
                    hm.StartCoroutine(hm.ShowErrorPrompt("I should probably figure out what just happened..."));
                }
                else
                {
                    hm.StartCoroutine("ShowPrompt");
                    hm.StartCoroutine(hm.ChangePos(gameObject));
                }
            }
        }
        if (this.gameObject.name == "DownTrigger" | this.gameObject.name == "LeftTrigger" | this.gameObject.name == "RightTrigger")
        {
            
            if (other.tag == "Player")
            {
                hm.StartCoroutine(hm.ShowErrorPrompt("Area unavailable! Come back later!"));
            }
        }
        if (this.gameObject.name == "Wizard")
        {
            if (HubScript.talkedToWiz == false)
            {
                //need to make a coroutine for the cutscene in Hubscript.
                hm.StartCoroutine("CutSceneStartQuest");
                Debug.Log("Wizard talked to");
                HubScript.talkedToWiz = true;
            }
            else if (HubScript.finishedQuest == true)
            {
                hm.StartCoroutine(hm.ShowErrorPrompt("I have nothing more for you, thank you."));
            }
            else if (HubScript.talkedToWiz == true && GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().currentHealth > 0 && HubScript.finishedQuest == false)
            {
                hm.StartCoroutine(hm.ShowErrorPrompt("I gave you a weapon and you're super smart, \n what else could you want?"));
            }
            else if (HubScript.talkedToWiz == true && GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().currentHealth <= 0 && HubScript.finishedQuest == false)
            {
                hm.StartCoroutine("CutSceneAfterDeath");
            }
        }
        if (this.gameObject.name == "TrapDoorTrigger" && HubScript.finishedQuest == false)
        {
            if (HubScript.talkedToWiz == false)
            {
                if (other.tag == "Player")
                {
                    hm.StartCoroutine(hm.ShowErrorPrompt("*grunt* Ugggg?"));
                }
            }
            else if (HubScript.talkedToWiz == true && HubScript.finishedQuest == true)
            {
                hm.StartCoroutine(hm.ShowErrorPrompt("I don't think I ever want to go in there again!"));
            }
            else if (HubScript.talkedToWiz == true && HubScript.finishedQuest == false)
            {
                if (other.tag == "Player")
                {
                    hm.StartCoroutine("ShowPrompt");
                    hm.StartCoroutine(hm.ChangePos(gameObject));
                }
            }
        }
        if (this.gameObject.name == "WeaponSpawn")
        {
            if (HubScript.talkedToWiz == true)
            {
                if (other.tag == "Player")
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                }
            }
        }
    }
}