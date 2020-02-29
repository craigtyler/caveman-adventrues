using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    public void doorCheck(GameObject[] room, GameObject[] LeftDoor, GameObject[] RightDoor, GameObject[] UpDoor, GameObject[] DownDoor)
    {
        RaycastHit2D[] hitUp = new RaycastHit2D[room.Length];
        RaycastHit2D[] hitDown = new RaycastHit2D[room.Length];
        RaycastHit2D[] hitLeft = new RaycastHit2D[room.Length];
        RaycastHit2D[] hitRight = new RaycastHit2D[room.Length];

        for (int i = 0; i < room.Length; i++)
        {
            hitUp[i] = Physics2D.Raycast(UpDoor[i].transform.position, Vector2.up, 1);
            hitDown[i] = Physics2D.Raycast(DownDoor[i].transform.position, Vector2.down, 1);
            hitLeft[i] = Physics2D.Raycast(LeftDoor[i].transform.position, Vector2.left, 1);
            hitRight[i] = Physics2D.Raycast(RightDoor[i].transform.position, Vector2.right, 1);
        }
        for (int i = 0; i < room.Length; i++)
        {
            if (hitUp[i].collider == null)
            {
                UpDoor[i].SetActive(false);
                //Debug.Log("Deactivated Up Door");
            }
            if (hitDown[i].collider == null)
            {
                DownDoor[i].SetActive(false);
               // Debug.Log("Deactivated Down Door");
            }
            if (hitLeft[i].collider == null)
            {
                LeftDoor[i].SetActive(false);
              //  Debug.Log("Deactivated Left Door");
            }
            if (hitRight[i].collider == null)
            {
                RightDoor[i].SetActive(false);
               // Debug.Log("Deactivated Right Door");
            }
        }
    }
}
