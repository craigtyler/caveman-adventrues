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

    //use counters to check if up < right || up == right 
    //up = 0, left = 1, down = 2, right = 3

    // +x is left; -x is right; +y is up; -y is down

    //create an overload method for the first room only, to instantiate the door. all other rooms will use the other "placeroom" method. 

    //create another overload method for placeRoom to pass details of the rooms that conflicted and (the bool that is attached to them)?. Use the case;switch to determine their position again and start the loop over(the checkPos for loop)

    //try to connect the rooms as they're moved
    //this place room method should include a spawn for the door prefab too.
    public void placeRoom(GameObject[] room, GameObject door, int roomNumber, int totalRooms) 
    {
        Debug.Log("Placed Room");

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
    // we'll have to use this function WHILE the generator moves rooms. 
    //probably call this method inside each of the cases. since we have an if statement keeping the first room from being checked, probably best way to do it
    

   //
    public void checkRoomPos(GameObject[] room, int maxRooms) 
    {
        for (int i = 1; i == maxRooms; i++)
        {
            int j = i - 1;
            int k = i + 1;
            int a = k + 1;

            int w = maxRooms - i;

            if (room[i].transform.position == room[j].transform.position)
            {
                placeRoom(room, i, maxRooms);
            }
            if (room[i].transform.position == room[k].transform.position)
            {
                placeRoom(room, i, maxRooms);
            }
            if (room[i].transform.position == room[w].transform.position && i!= w)
            {
                placeRoom(room, i, maxRooms);
            }
            if (room[k].transform.position == room[w].transform.position && i != w)
            {
                placeRoom(room, k, maxRooms);
            }
            if (room[a].transform.position == room[w].transform.position && a != w) 
            {
                placeRoom(room, a, maxRooms);
            }
        }
    }
    //finished, needs checked
    void placeRoom(GameObject[] room, int currentRoom, int maxRoom)
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
                newXPos = new Vector3(room[currentRoom].transform.position.x + roomX, room[currentRoom].transform.position.y);
                room[currentRoom].transform.position = newYPos;
                checkRoomPos(room, maxRoom);
                break;
            case Dir.Right:
                newXPos = new Vector3(room[currentRoom].transform.position.x - roomX, room[currentRoom].transform.position.y);
                room[currentRoom].transform.position = newYPos;
                checkRoomPos(room, maxRoom);
                break;
        }
    }
}


