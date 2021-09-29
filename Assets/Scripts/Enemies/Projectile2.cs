using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collisionInfo){

      GameObject collidedObject = collisionInfo.gameObject;

      //If collided with player
      if(collidedObject.tag.Equals("Player")){
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript playerInstance = player.GetComponent<PlayerScript>();
        playerInstance.RemoveHealth(1);

      }

      if(!collidedObject.tag.Equals("Enemy4")){
        Destroy(gameObject);
      }

    }

}
