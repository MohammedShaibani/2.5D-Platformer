using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : MonoBehaviour
{

    public GameObject player;
    private float xLeftLimit;
    private float xRightLimit;
    private bool collidedWithPlat = false;

    // the direction the Enemy5 is moving in (1 == right, 0 == left)
    private int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
      // transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MoveEnemy(){

      if(collidedWithPlat){  // direction is right

        if(direction == 1){
          if(transform.position.x < xRightLimit){
            transform.position += new Vector3(0.01f, 0.0f, 0.0f);
          }
          else{
            direction = 0; // switch directions
          }
        }
        else { // direction is left
          if(transform.position.x > xLeftLimit){
            transform.position -= new Vector3(0.01f, 0.0f, 0.0f);
          }
          else{
            direction = 1; // switch directions
          }
        }

      }

    }

    private void OnCollisionEnter(Collision collisionInfo){

      GameObject collidedObject = collisionInfo.gameObject;

      //If collided with platform
      if(collidedObject.tag.Equals("PlatformCollect") && !collidedWithPlat){
        collidedWithPlat = true;

        xLeftLimit = collidedObject.transform.Find("Start").position.x + 0.5f;
        xRightLimit = collidedObject.transform.Find("End").position.x - 0.5f;

        InvokeRepeating("MoveEnemy", .5f, 0.01f);

      }

      if(collidedObject.tag.Equals("Player")){
        Destroy(gameObject);
      }

    }
}
