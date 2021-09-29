using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelGeneration : MonoBehaviour {

    private const float SpawnThreshold = 10f;
    private int ChunkCounter, ChunkAmount;
    private int PlatformCount = 5;
    private bool LoadEnd = false;
    private bool LoadNextChunk = true;


    [SerializeField] private Transform StartPlatform;
    [SerializeField] private List<Transform> platformList;
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> enemyList;
    [SerializeField] private List<Transform> specialEnemyList;
    [SerializeField] private List<Transform> pickupList;
    [SerializeField] private Transform Collectable;
    [SerializeField] private Transform EndLevelPrefab;


    private Vector3 lastPlatformPosition;

    //starts off with 4 platforms spawned in after the start platform so the current scene is populated with platforms
    private void Awake() {
        lastPlatformPosition = StartPlatform.Find("End").position;
        int initialPlatformCount = 4;
        for (int i = 0; i < initialPlatformCount; i++) {
            SpawnPlatform();
        }
        InvokeRepeating("SpawnEnemy0", 5.0f, 5.0f);
        InvokeRepeating("SpawnEnemy1", 12.0f, 12.0f);
    }

    //continuously checks whether the player is getting too close to the last platform and spawns in more so there isnt a delay
    private void Update() {
        if (lastPlatformPosition.x - player.transform.position.x < SpawnThreshold && ChunkAmount < 3 && LoadNextChunk == true) {
            for (int i = 0; i < PlatformCount; i++) {
                SpawnPlatform();
            }
            ChunkAmount += 1;

        }
        if (ChunkAmount == 3 && LoadEnd == false) {
            LoadNextChunk = false;
            ChunkCounter += 1;
            //ChunkAmount = 0;
            SpawnEndLevel();
        }

    }

    /*
     chooses a random platform from the list of platforms and then creates a vector based on its start point
     then spawns it depending on its start position and the position of the last platform + one more vector to
     randomize position a bit
     then it sets the last known platform position based off the end point of the last platform spawned
     */
    private void SpawnPlatform() {
        Transform chosenPlatformPrefab = platformList[Random.Range(0, platformList.Count)];
        Vector3 chosenPlatformStartPosition = chosenPlatformPrefab.Find("Start").position;
        Vector3 spawnPosition = lastPlatformPosition + chosenPlatformPrefab.position - chosenPlatformStartPosition + new Vector3(Random.Range(0, 2), Random.Range(-2, 2), 0);
        Transform lastPlatformTransform = SpawnPlatform(chosenPlatformPrefab, spawnPosition);
        lastPlatformPosition = lastPlatformTransform.Find("End").position;
    }

    private void SpawnEndLevel() {
        Transform EndPrefab = EndLevelPrefab;
        Vector3 startPos = EndPrefab.Find("Start").position;
        Vector3 spawnPos = lastPlatformPosition + EndPrefab.position - startPos + new Vector3(Random.Range(0, 2), Random.Range(-2, 2), 0);
        Transform EndLevelTransform = SpawnPlatform(EndPrefab, spawnPos);
        LoadEnd = true;
    }


    //Instantiates the prefab of the given platform  and defines its start and end positions
    private Transform SpawnPlatform(Transform platformPrefab, Vector3 spawnPosition) {
        Transform platformTransform = Instantiate(platformPrefab, spawnPosition, platformPrefab.transform.rotation);
        int collectCount = Random.Range(5, 10);
        if (platformTransform.childCount == 4) {
            Vector3 CollectableSpawn = platformTransform.Find("SpawnCollect").position;
            Vector3 EnemySpawn = platformTransform.Find("EnemySpawn").position;
            spawnEnemies(EnemySpawn, enemyList);
            spawnCollectables(CollectableSpawn, collectCount, Collectable, pickupList);
        }
        return platformTransform;
    }


    /*
    This function is in charge of spawning all collectables, so the actual collectables + the pickups
    For the collectables it will run through a for loop and spawn in a certain number of collectables based on the count variable
    For the pickup it will spawn in a random powerup from the list of powerups
    */
    private void spawnCollectables(Vector3 pos, int count, Transform collectPrefab, List<Transform> powerupPrefabList) { //position for first collectable and number of collectables to spawn
        int spawn = Random.Range(0, 5);
        if (spawn < 3) {
            for (int i = 0; i < count; i++) {
                Transform collect = Instantiate(collectPrefab, new Vector3(pos.x + i, pos.y + 0.5f, 0), Quaternion.identity);
            }
        }
        if (spawn > 3) {
            Transform powerupPrefab = powerupPrefabList[Random.Range(0, powerupPrefabList.Count)];
            Transform collect = Instantiate(powerupPrefab, new Vector3(pos.x + 4, pos.y + 0.5f, 0), Quaternion.identity);
        }
    }

    private void spawnEnemies(Vector3 pos, List<Transform> enemyPrefabList) {
        int enemyToSpawn = Random.Range(0, enemyPrefabList.Count);
        Transform enemyPrefab = enemyPrefabList[enemyToSpawn];
        Transform spawnEnemy;
        if (enemyToSpawn == 0) {
            spawnEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
        if (enemyToSpawn == 1) {    //enemy 4 spawns above the platform and shoots projectiles
            spawnEnemy = Instantiate(enemyPrefab, pos + new Vector3(0, 2, 0), Quaternion.identity);
        }
        if (enemyToSpawn == 2) {    //enemy 5 spawns on platform and patrols it
            spawnEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    private void SpawnEnemy0(){
      Transform enemyPrefab = specialEnemyList[0];
      Transform spawnEnemy;
      spawnEnemy = Instantiate(enemyPrefab, player.transform.position, Quaternion.identity);
    }

    private void SpawnEnemy1(){

      Transform enemyPrefab = specialEnemyList[1];
      Transform spawnEnemy;
      spawnEnemy = Instantiate(enemyPrefab, player.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
    }
}
