using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private readonly int numPlayers = 4; // TODO - DYNAMIC. DROP IN W/ START
    private readonly float spawn_delay = 2f;

    // Public fields
    public List<GameObject> spawnPoints;
    public GameObject[] carObjects;
    public playerID carPrefab;
    public IntEvent playerDied;
    public LivesList playerLives;
    public bool[] inGame;
    // Private vars
    private int spawnLoopIndex = 0;


    // Init
    private void Awake() {
        inGame = new bool[4] { true, true, true, true };
        carObjects = new GameObject[4] { null, null, null, null };
        playerDied.AddListener(onPlayerDied);
        // TODO - whenever a player drops in, initialize their lives. For now, 4 players always
        for(int p = 0; p < numPlayers; p++) {
            // initialize lives (TODO)
        }
    }

    // Called whenever a player dies
    private void onPlayerDied(int player) {
        // TODO - check if lives > 0, then decrement
        StartCoroutine(spawnCarDelayed(player, spawn_delay));
    }

    IEnumerator spawnCarDelayed(int player, float delay) {
        yield return new WaitForSeconds(delay);
        spawnCar(player);
    }

    // Spawn a car for a certain player
    private void spawnCar(int player) {
        carPrefab.p = player;
        GameObject newCar = Instantiate(carPrefab.gameObject);
        carObjects[player] = newCar;
        newCar.transform.position = spawnPoints[spawnLoopIndex].transform.position;
        spawnLoopIndex = (spawnLoopIndex + 1) % spawnPoints.Count;
    }

    public void destroyCar(int player)
    {
        Destroy(carObjects[player]);
    }

    public void changeState(int player)
    {
        if (!inGame[player])
        {
            spawnCar(player);
        } else
        {
            destroyCar(player);
        }
        inGame[player] = !inGame[player];
    }
    
}
