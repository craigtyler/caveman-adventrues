using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public Sprite newSprite;

    public void SpriteChange()
    {
        StartCoroutine("ChangeSprite");
    }
    public IEnumerator ChangeSprite()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
