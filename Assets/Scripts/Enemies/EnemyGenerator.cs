using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{


    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

      InvokeRepeating("SpawnEnemy2", 5.0f, 5.0f);
      // InvokeRepeating("SpawnOtherEnemies", 7.0f, 7.0f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy2(){

      player = GameObject.FindGameObjectWithTag("Player");
      // spawn in enemy2
      Instantiate(enemy2, player.transform.position, Quaternion.identity);

    }


    void SpawnOtherEnemies(){

      player = GameObject.FindGameObjectWithTag("Player");
      // spawn in either enemy1, 3, 4 or 5
      int enemyToSpawn = Random.Range(0, 4);

      Vector3 playerPos = player.transform.position;
      Vector3 posToSpawnEnemy;

      // posToSpawnEnemy = playerPos + new Vector3(0, 3, 0);
      // Instantiate(enemy4, posToSpawnEnemy, Quaternion.identity);
      // Instantiate(enemy5, playerPos, Quaternion.identity);


      if(enemyToSpawn == 0){    //doesnt matter
        posToSpawnEnemy = playerPos + new Vector3(1.5f, 0, 0);
        Instantiate(enemy1, posToSpawnEnemy, Quaternion.identity);
      }
      else if(enemyToSpawn == 1){   //keep same vector so they spawn above player
        posToSpawnEnemy = playerPos + new Vector3(0, 6, 0);
        Instantiate(enemy3, posToSpawnEnemy, Quaternion.identity);
      }
      else if(enemyToSpawn == 2){   //flying enemy so keep above platform
        posToSpawnEnemy = playerPos + new Vector3(0, 3, 0);
        Instantiate(enemy4, posToSpawnEnemy, Quaternion.identity);
      }
      else if(enemyToSpawn == 3){   //spawn on platform
        posToSpawnEnemy = playerPos + new Vector3(1.5f, 0, 0);
        Instantiate(enemy5, posToSpawnEnemy, Quaternion.identity);
      }

    }

}
