using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{

    int projectilesShotCount = 0;
    public GameObject projectile;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

      InvokeRepeating("ShootProjectile", 3.5f, 0.2f);
      transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShootProjectile(){

      if(projectilesShotCount == 25){
        CancelInvoke("ShootProjectile");
        projectilesShotCount = 0;
        // wait 2 secs the start shooting again
        // InvokeRepeating("ShootProjectile", 2f, 0.1f);

      }
      else{

        projectile = Instantiate(projectile, transform.position, Quaternion.identity);

        projectile.GetComponent<Rigidbody>().AddForce(transform.up * 100);
        projectilesShotCount += 1;
      }

    }
}
