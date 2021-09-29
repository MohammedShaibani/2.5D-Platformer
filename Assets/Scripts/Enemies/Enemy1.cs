using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{

    int projectilesShotCount = 0;
    public GameObject projectile;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

      InvokeRepeating("shootProjectile", 1f, 1f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void shootProjectile(){

      if(projectilesShotCount == 5){
        Destroy(gameObject);
      }
      else{

        projectile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;

        player = GameObject.FindGameObjectWithTag("Player");
        float playerZpos = player.transform.position.z;

        if(playerZpos > transform.position.z){
          transform.Rotate(0.0f, 0.0f, 0.0f, Space.World);
        } else {
          transform.Rotate(180.0f, 0.0f, 0.0f, Space.World);
        }

        projectile.GetComponent<Rigidbody>().AddForce(transform.right * 1000);
        projectilesShotCount += 1;
      }

    }
}
