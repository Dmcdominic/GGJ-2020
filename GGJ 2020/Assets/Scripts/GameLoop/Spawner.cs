using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private readonly int numPlayers = 4; // TODO - DYNAMIC. DROP IN W/ START
    private readonly float spawn_delay = 2f;

    // Public fields
    public List<GameObject> spawnPoints;

    public playerID carPrefab;
    public IntEvent playerDied;
    public LivesList playerLives;

    // Private vars
    private int spawnLoopIndex = 0;


    // Init
    private void Awake() {
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
        newCar.transform.position = spawnPoints[spawnLoopIndex].transform.position;
        spawnLoopIndex = (spawnLoopIndex + 1) % spawnPoints.Count;
    }
}
