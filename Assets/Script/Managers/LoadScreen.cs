using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    public Text loadingText;

    private bool isLoading = false;

    private GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        StartCoroutine("loadSequence");
    }

    IEnumerator loadSequence()
    {
        int count = 0;
        IntRange range = new IntRange(0,2);
        int rand = range.Random;
        if (isLoading == false)
        {
            //isLoading = true;
            while (isLoading == false)
            {
                loadingText.text = "Loading.";

                yield return new WaitForSeconds(1f);

                loadingText.text = "Loading..";

                yield return new WaitForSeconds(1f);

                loadingText.text = "Loading...";

                yield return new WaitForSeconds(1f);
                if (count == rand)
                {
                    isLoading = true;
                }
                else
                {
                    count++;
                }
            }
        }
        if (SceneManager.sceneCount == 2)
        {
            Debug.Log(SceneManager.sceneCount);
            gm.SwitchScenes(1, true, false);
        }
        SceneManager.UnloadSceneAsync(2);
    }
}
