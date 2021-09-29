using UnityEngine;

public class DestroyLevelObjects : MonoBehaviour
{
    //GameObject platform, platform2, collect, enemy;
    Transform Player;

    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (Player.position.x - transform.position.x > 40f) {
            Destroy(gameObject);
        }
    }
/*
    private void DestroyCollectable(GameObject collect) {
        collect = GameObject.FindGameObjectWithTag("Collectable");
        Destroy(collect);
    }

        //finding the platform, and then destroying it
    private void DestroyPlatforms(GameObject platform, GameObject platform2) {
        platform = GameObject.FindGameObjectWithTag("Platform");
        platform2 = GameObject.FindGameObjectWithTag("PlatformCollect");
        Destroy(platform);
        Destroy(platform2);
        Debug.Log("Destroyed");
        
    }*/
}
