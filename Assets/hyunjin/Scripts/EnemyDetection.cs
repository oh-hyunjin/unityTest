using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Transform player;
    bool isPlayerInRange = false;

    void OnTriggerEnter(Collider other) {
        if (other.transform == player)
            isPlayerInRange = true;
    }
    void OnTriggerExit(Collider other) {
        if (other.transform == player)
            isPlayerInRange = false;
    }

    void Update() {
        if (isPlayerInRange) {
            Vector3 direction = player.position  - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit)){
                if (raycastHit.collider.transform == player)
                    Debug.Log("Detected!!!!!");
            }
        }
    }
}
