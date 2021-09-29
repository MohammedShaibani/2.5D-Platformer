
using UnityEngine;

public class PlatformMoveV : MonoBehaviour {
    float speed = 1f;
    float compY;
    void Start() {
        compY = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0);
        if (transform.position.y >= compY + 3) {
            speed = -1f;
        }
        if (transform.position.y <= compY - 1) {
            speed = 1f;
        }
        //Debug.Log(transform.localPosition);

    }
}
