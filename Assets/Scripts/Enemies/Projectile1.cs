using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : MonoBehaviour
{
    private float timeTillDestroy = 5;
    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine(destroy(timeTillDestroy));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collisionInfo){

      GameObject collidedObject = collisionInfo.gameObject;

      //If collided with player
      if(collidedObject.tag.Equals("Player")){
        // Destroy(gameObject);
      }
      else if(collidedObject.tag.Equals("Platform"))
      {
          GetComponent<Rigidbody>().AddForce(transform.right * 500);
      }

    }


    IEnumerator destroy(float timeTillDestroy){

      yield return new WaitForSeconds(timeTillDestroy);
      Destroy(gameObject);

    }
}
