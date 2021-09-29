using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float leastXPos;
    public Camera cam;
    private float halfCameraWidth;
  
    private float xDifference, yDifference, zDifference;

    public GameObject cameraEdgeBlocker;
    public GameObject collisionDestroy; //for collision detection of platforms

    private PlayerScript playerScript;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        xDifference = 0;
        zDifference = -8;
        yDifference = 1;
        cam = this.GetComponent<Camera>();
        halfCameraWidth = cam.orthographicSize * cam.aspect;

        //On start, set the leastXPos to the left side of the screen right now
        leastXPos = player.transform.position.x - halfCameraWidth;

        //The edge blocker is an invisible wall that nothing can pass. So turn off the renderer
        cameraEdgeBlocker.GetComponent<Renderer>().enabled = false;

        leastXPos = player.transform.position.x - 1;

        cam.transform.position = new Vector3(leastXPos + halfCameraWidth, cam.transform.position.y, cam.transform.position.z);

        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Only try to move the camera if the player is alive
        if (!playerScript.isDead) {
            //This is going to be where the camera would go to if the camera just followed the player unconditionally
            float attemptedXPos = player.transform.position.x - halfCameraWidth;

            //The current lowest position is smaller than the new position (ie the player moved right), so update the variable and camera
            if (leastXPos < attemptedXPos)
            {
                leastXPos = attemptedXPos;
                transform.position = player.transform.position + new Vector3(xDifference, yDifference, zDifference);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y + yDifference, zDifference);
            }
            cameraEdgeBlocker.transform.position = new Vector3(leastXPos, 0, 0);
            //setting the collision detector to move with the player but right behind the edge blocker so only platforms out of
            //frame are deleted
            collisionDestroy.transform.position = new Vector3(leastXPos - 50, 0, 0);
        }
    }
}
