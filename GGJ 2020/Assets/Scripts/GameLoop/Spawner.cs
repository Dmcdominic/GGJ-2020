using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // Public fields
    public playerID carPrefab;
    public List<GameObject> spawnPoints;

    // Private vars
    private int spawnLoopIndex = 0;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // Spawn a car for a certain player
    public void spawnCar(int player) {
        carPrefab.p = player;
        GameObject newCar = Instantiate(carPrefab.gameObject);
        newCar.transform.position = spawnPoints[spawnLoopIndex++].transform.position;
    }
}
