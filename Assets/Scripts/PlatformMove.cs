
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    float speed = 0.5f;
    float compX;
    void Start() {
        compX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
        if (transform.position.x >= compX + 3) {
            speed = -0.5f;
        }
        if (transform.position.x <= compX -1) {
            speed = 0.5f;
        }
        //Debug.Log(transform.localPosition);

    }

    private void OnTriggerEnter(Collider other) {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other) {
        other.transform.parent = null;
    }
}
