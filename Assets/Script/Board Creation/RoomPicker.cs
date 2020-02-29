using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPicker : MonoBehaviour
{ 
    //width = 1.25 height = 1

    public float roomX = 1.25f; // X coord, width
    public float roomY = 1.25f; // Y coord, height

    public Vector3 newYPos;
    public Vector3 newXPos;

    public Dir direction;
    //up = 0, left = 1, down = 2, right = 3

    // +x is left; -x is right; +y is up; -y is down

    public void placeRoom(GameObject[] room, int roomNumber, int totalRooms) 
    {
        direction = (Dir)Random.Range(0, 4); // pick a random direction
        if (roomNumber > 0)
        {
            switch (direction)
            {
                case Dir.Up:
                    // place a door in the position (up)  in the room and spawn a room above last  
                    newYPos = new Vector3(room[roomNumber - 1].transform.position.x, (room[roomNumber - 1].transform.position.y + roomY));
                    room[roomNumber].transform.position = newYPos;
                    break;
                case Dir.Down:
                    // place a door in the position (down)  in the room and spawn a room below last  
                    newYPos = new Vector3(room[roomNumber - 1].transform.position.x, (room[roomNumber - 1].transform.position.y - roomY));
                    room[roomNumber].transform.position = newYPos;
                    break;
                case Dir.Left:
                    // place a door in the position (left)  in the room and spawn a room to the left of the last  
                    newXPos = new Vector3((room[roomNumber - 1].transform.position.x + roomX), room[roomNumber - 1].transform.position.y);
                    room[roomNumber].transform.position = newXPos;
                    break;
                case Dir.Right:
                    // place a door in the position (right) in the room and spawn a room to the right of the last  
                    newXPos = new Vector3((room[roomNumber - 1].transform.position.x - roomX), room[roomNumber - 1].transform.position.y);
                    room[roomNumber].transform.position = newXPos;
                    break;
            }
        }
    }
    
    public void checkRoomPos(GameObject[] room, int maxRooms) 
    {
        bool isFinished = false;
        int i = 0;

        while (isFinished == false)
        {
            int k = i + 1;
            int p = i + 2;
            int a = i + 3;


            int o = (maxRooms - i);
            int w = (maxRooms - 1) - i;
            int c = (maxRooms - 2) - i;
            int l = (maxRooms - 3) - i;

            #region positionChecks

            #region Checks down
            if (i < (maxRooms) && k < (maxRooms))
            {
                if (room[i].transform.position == room[k].transform.position)
                {
                    placeRoom(room, k, maxRooms,true);
                    i = 0;
                }
            }
            if (i < (maxRooms) && p < (maxRooms))
            {
                if (room[i].transform.position == room[p].transform.position && i != p)
                {
                    placeRoom(room, p, maxRooms,true);
                    i = 0;
                }
            }
            if (i < (maxRooms) && a < (maxRooms))
            {
                if (room[i].transform.position == room[a].transform.position && i != a)
                {
                    placeRoom(room, a, maxRooms,true);
                    i = 0;
                }
            }
            #endregion

            #region check up
            if (i < (maxRooms) && o < (maxRooms) && o > 0)
            {
                if (room[i].transform.position == room[o].transform.position && i != o)
                {
                    placeRoom(room, o, maxRooms,true);
                    i = 0;
                }
            }
            if (i < (maxRooms) && w < (maxRooms))
            {
                if (room[i].transform.position == room[w].transform.position && i != w)
                {
                    placeRoom(room, w, maxRooms,true);
                    i = 0;
                }
            }
            if (a < (maxRooms) && w < (maxRooms) && w > 0)
            {
                if (room[a].transform.position == room[w].transform.position && a != w)
                {
                    placeRoom(room, w, maxRooms,true);
                    i = 0;
                }
            }
            if (i < (maxRooms) && c > 0)
            {
                if (room[i].transform.position == room[c].transform.position && i != c)
                {
                    placeRoom(room, c, maxRooms,true);
                    i = 0;
                }
            }
            if (p < (maxRooms) && a < (maxRooms) && i != a)
            {
                if (room[p].transform.position == room[a].transform.position && i != a)
                {
                    placeRoom(room, a, maxRooms,true);
                    i = 0;
                }
            }
            if (i < (maxRooms) && l > 0 && i != l)
            {
                if (room[i].transform.position == room[l].transform.position && i != a)
                {
                    placeRoom(room, l, maxRooms,true);
                    i = 0;
                }
            }
            #endregion
            if (i < maxRooms)
            {
                i++;
            }
            if (i == maxRooms)
            {
                isFinished = true;
            }
        }

        #endregion
    }
    void placeRoom(GameObject[] room, int currentRoom, int maxRoom, bool push)
    {
        direction = (Dir)Random.Range(0, 4); // pick a random direction    

        switch (direction)
        {
            case Dir.Up:
                newYPos = new Vector3(room[currentRoom].transform.position.x, (room[currentRoom].transform.position.y + roomY));
                room[currentRoom].transform.position = newYPos;
                checkRoomPos(room, maxRoom);
                break;
            case Dir.Down:
                newYPos = new Vector3(room[currentRoom].transform.position.x, (room[currentRoom].transform.position.y - roomY));
                room[currentRoom].transform.position = newYPos;
                checkRoomPos(room, maxRoom);
                break;
            case Dir.Left:
                newXPos = new Vector3((room[currentRoom].transform.position.x + roomX), (room[currentRoom].transform.position.y));
                room[currentRoom].transform.position = newYPos;
                checkRoomPos(room, maxRoom);
                break;
            case Dir.Right:
                newXPos = new Vector3((room[currentRoom].transform.position.x - roomX), room[currentRoom].transform.position.y);
                room[currentRoom].transform.position = newYPos;
                checkRoomPos(room, maxRoom);
                break;
        }
    }
}


