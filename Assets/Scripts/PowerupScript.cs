using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    float xAngle, yAngle, zAngle, startY;
    float range = 0.2f;

    bool movingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        xAngle = 30;
        yAngle = 45;
        zAngle = 60;

        //Hold the starting y position
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate the powerup on every frame update
        transform.Rotate(xAngle * Time.deltaTime, yAngle * Time.deltaTime, zAngle * Time.deltaTime);

        //Also bob up and down
        Bob();
    }

    public void Bob()
    {
        if (movingUp)
        {
            transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(0, -0.5f, 0) * Time.deltaTime;
        }
        //Moved above the top of the movement line
        if (transform.position.y > startY + range)
        {
            movingUp = false;
        }
        //Moved below the bottom of the movement line
        else if (transform.position.y < startY - range)
        {
            movingUp = true;
        }
    }
}
