using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{

    public GameObject player;
    public float timeToExplode = 1.0f;
    public bool isInvulnerable = true;
    CapsuleCollider collider;

    void Start()
    {

      // disable collider while this Enemy is invulnerable
      collider = gameObject.GetComponent<CapsuleCollider>();
      collider.enabled = false;
      // TODO: make this object transparent
      // TODO: display blast radius of this object
      StartCoroutine(explode(timeToExplode));



    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator explode(float delay)
    {

        yield return new WaitForSeconds(delay);

        ParticleSystem exp = GetComponent<ParticleSystem>();
        exp.Play();

        // re enable collider
        collider.enabled = true;

        this.isInvulnerable = false;

        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 thisPosition = this.transform.position;

        float distanceFromPlayer = Vector3.Distance(thisPosition, playerPos);

        PlayerScript playerInstance = player.GetComponent<PlayerScript>();

        // checking if player was in blast radius
        if(distanceFromPlayer < 0.75){
          // remove 1 health from player and remove powerup
          playerInstance.RemoveHealth(1);
          playerInstance.RemovePowerups();

        }
        else if(distanceFromPlayer < 1.5){
          // only remove power up
          playerInstance.RemovePowerups();
        }

        // Destroy(gameObject);

    }
}
