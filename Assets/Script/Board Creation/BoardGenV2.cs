using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    // +x is left; -x is right; +y is up; -y is down
public enum Dir
{
    Up, Left, Down, Right
}

public class BoardGenV2 : MonoBehaviour
{
    public GameObject[] roomTemplate;

    public GameObject[] bossRoomTemplate;

    public GameObject[] TreasureRoomTemplate;

    [HideInInspector] 
    public GameObject[] LeftDoor;
    [HideInInspector]
    public GameObject[] RightDoor;
    [HideInInspector]
    public GameObject[] UpDoor;
    [HideInInspector]
    public GameObject[] DownDoor;
    [HideInInspector]
    public GameObject ExitDoor;
    [HideInInspector]
    public GameObject[] enemySpawns;

    public IntRange numRooms = new IntRange(0, 10);

    public GameObject player;
    public GameObject[] enemies;
    public GameObject[] bosses;
    public GameObject[] weapons;
    public GameObject[] items;

    private GameObject boardHolder;
    [HideInInspector]
    public GameObject enemyHolder;
    private int maxRooms;

    private RoomPicker roomPickerHolder;
    private RoomCheck roomCheckHolder;
    private GameManager gm;

    [HideInInspector]
    public GameObject[] rooms;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        gm.GetEnemyList(enemies, bosses);
        gm.GetItemList(weapons, items);
        SetUpLevel(gm.CurrentFloor);
    }
    public void SetUpLevel(int levelNum)
    {
        if (boardHolder == null)
        {
            boardHolder = new GameObject("BoardHolder");
        }
        Invoke("SpawnCharacter", .01f);
        Invoke("SpawnEnemies", 1);
        Invoke("SpawnItems", 1);
        temp = 0; // reset temp counter
        SpawnRooms();
    }
    private void SpawnCharacter()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length != 1)
        {
            Instantiate(player, Vector3.zero, Quaternion.identity);
        }
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().UpdateItemAndUI(false, 0, false, 0, true, GameObject.FindGameObjectWithTag("Player").GetComponent<StatController>().currentWeapon, false);
    }

    private void SpawnRooms()
    {
        int a = 0;
        int roomsInArray = 0;
        int totalRoomsSpawned = 0;

        maxRooms = numRooms.Random;

        int newMaxRoom = maxRooms + 2;

        IntRange randReg = new IntRange(0, roomTemplate.Length);
        IntRange randBoss = new IntRange(0, bossRoomTemplate.Length);
        IntRange randGold = new IntRange(0, TreasureRoomTemplate.Length);

        rooms = new GameObject[newMaxRoom];

        LeftDoor = new GameObject[rooms.Length];
        RightDoor = new GameObject[rooms.Length];
        UpDoor = new GameObject[rooms.Length];
        DownDoor = new GameObject[rooms.Length];
        enemySpawns = new GameObject[rooms.Length - 1];

        while (totalRoomsSpawned < maxRooms)
        {
            int randomRoomTemp = randReg.Random;
            
            GameObject Room = Instantiate(roomTemplate[randomRoomTemp], Vector3.zero, Quaternion.identity) as GameObject;

            LeftDoor[a] = Room.transform.Find("Door_Left").gameObject;
            RightDoor[a] = Room.transform.Find("Door_Right").gameObject;
            UpDoor[a] = Room.transform.Find("Door_Up").gameObject;
            DownDoor[a] = Room.transform.Find("Door_Down").gameObject;
            enemySpawns[a] = Room.transform.Find("DoorCheck").gameObject;
            a++;

            Room.transform.parent = boardHolder.transform;
            Room.name = roomTemplate[randomRoomTemp].name;
            totalRoomsSpawned++;
            while (roomsInArray < totalRoomsSpawned)
            {
                rooms[roomsInArray] = Room;
                roomsInArray++;
            }
            if (totalRoomsSpawned == maxRooms)
            {
                int randomBossRoom = randBoss.Random;

                GameObject bossRoom = Instantiate(bossRoomTemplate[randomBossRoom], Vector3.zero, Quaternion.identity) as GameObject;

                LeftDoor[a] = bossRoom.transform.Find("Door_Left").gameObject;
                RightDoor[a] = bossRoom.transform.Find("Door_Right").gameObject;
                UpDoor[a] = bossRoom.transform.Find("Door_Up").gameObject;
                DownDoor[a] = bossRoom.transform.Find("Door_Down").gameObject;
                enemySpawns[a] = bossRoom.transform.Find("DoorCheck").gameObject;
                a++;

                bossRoom.transform.parent = boardHolder.transform;
                bossRoom.name = bossRoomTemplate[randomBossRoom].name;
                totalRoomsSpawned++;

                rooms[roomsInArray] = bossRoom;
                roomsInArray++;
            }
            if (totalRoomsSpawned == newMaxRoom - 1)
            {
                int randomGoldRoom = randGold.Random;

                GameObject goldRoom = Instantiate(TreasureRoomTemplate[randomGoldRoom], Vector3.zero, Quaternion.identity);
                //find the door prefabs
                LeftDoor[a] = goldRoom.transform.Find("Door_Left").gameObject;
                RightDoor[a] = goldRoom.transform.Find("Door_Right").gameObject;
                UpDoor[a] = goldRoom.transform.Find("Door_Up").gameObject;
                DownDoor[a] = goldRoom.transform.Find("Door_Down").gameObject;

                goldRoom.transform.parent = boardHolder.transform;
                goldRoom.name = TreasureRoomTemplate[randomGoldRoom].name;
                totalRoomsSpawned++;

                rooms[roomsInArray] = goldRoom;
            }
        }
        int count = 0;//temp counter
        while (count < rooms.Length)
        {
            moveRoom(rooms, rooms.Length);
            count++;
            while (count == rooms.Length)
            {
                roomPickerHolder.checkRoomPos(rooms, rooms.Length);
                count = count - 1;
                if (count != rooms.Length)
                {
                    roomCheckHolder = new RoomCheck();
                    roomCheckHolder.doorCheck(rooms,LeftDoor,RightDoor,UpDoor,DownDoor);
                }
            }
        }
    }
     int temp = 0;

    void moveRoom(GameObject[] room, int maxRooms)
    {
        roomPickerHolder = new RoomPicker(); // just to call the "RoompickerClass"

        roomPickerHolder.placeRoom(room, temp, maxRooms);
        temp++;
    }

    
    public void ResetLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void SpawnEnemies()
    {
        if (enemyHolder == null)
        {
            enemyHolder = new GameObject("EnemyHolder");
        }
        for (int i = 1; i < enemySpawns.Length; i++)
        {
            int temp = 0;
            IntRange randEnemyRange = new IntRange(0, enemies.Length);
            IntRange randBossRange = new IntRange(0, bosses.Length);
            IntRange randAmount = new IntRange(1, 3); // get a rand amount to spawn
            int randEnemyAmount = randAmount.Random;
            //not include boss room, and the room before as it'll be a treasure room
            if (i != enemySpawns.Length)
            {
                while (temp < randEnemyAmount) // if were not at the end of the rand amount of enemies
                {
                    temp++;
                    int randEnemy = randEnemyRange.Random;
                    if (rooms[i].name != "TreasureRoom" | rooms[i].name != "BossRoom")
                    {
                        if (i < maxRooms)
                        {
                            GameObject Enemy = Instantiate(GameManager.EnemyList[randEnemy], enemySpawns[i].transform.position, Quaternion.identity);
                            Enemy.name = GameManager.EnemyList[randEnemy].name;
                            Enemy.transform.parent = enemyHolder.transform;
                        }
                    }
                }
                if (rooms[i].name == "BossRoom")
                {
                    int randBoss = randBossRange.Random;
                    GameObject boss = Instantiate(GameManager.BossList[randBoss], enemySpawns[i].transform.position, Quaternion.identity);
                    boss.name = GameManager.BossList[randBoss].name;
                }
            }
        }
    }
    public void SpawnItems()
    {
        int temp = 0;
        GameObject[] itemSpawns = new GameObject[GameObject.FindGameObjectsWithTag("ItemSpawn").Length]; //create an array to hold the item spawns

        IntRange randItem = new IntRange(0, items.Length);
        IntRange randWeapon = new IntRange(0, weapons.Length);

        GameObject treasureRoom = GameObject.FindGameObjectWithTag("TreasureRoom");

        foreach (var item in GameObject.FindGameObjectsWithTag("ItemSpawn"))
        {
            itemSpawns[temp] = item;
            temp++;
        }
        for (int i = 0; i < itemSpawns.Length; i++)
        {
            if (itemSpawns[i].name == "WeaponHolder")
            {
                int rand = randWeapon.Random;
                GameObject Weapon = Instantiate(GameManager.WeaponList[rand], itemSpawns[i].transform.position, Quaternion.identity);
                Weapon.name = GameManager.WeaponList[rand].name;
            }
            if (itemSpawns[i].name != "WeaponHolder")
            {
                int rand = randItem.Random;
                GameObject item = Instantiate(GameManager.ItemList[rand], itemSpawns[i].transform.position, Quaternion.identity);
                item.name = GameManager.ItemList[rand].name;
            }
        }
    }
}