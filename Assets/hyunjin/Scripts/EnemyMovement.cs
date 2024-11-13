using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public float stopDuration = 2f;
    public float searchDuration = 1f;

    private Animator animator;
    int currentWaypointIndex = 0;
    float waitTimer = 0f;
    bool isWaiting = false;
    bool isSearching = false;
    bool isReverse = false;

    void Start() {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true);
    }

    void Update() {
        if(!isWaiting)
            MoveToWayPoint();
        else
            HandleWaiting();
    }

    void MoveToWayPoint() {
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 회전
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        // waypoint에 도착했을 때
        if (Vector3.Distance(transform.position, target.position) < 0.1f) {
            if (target.CompareTag("searchPoint"))
                startSearching();
            else
                stopAtWaypoint();
        }
    }

    void startSearching() {
        waitTimer = searchDuration;
        isWaiting = true;
        isSearching = true;
        animator.SetBool("isWalking", false);
        // animator.SetBool("isSearching", true);
        transform.rotation = Quaternion.LookRotation(Vector3.left);
    }

    void stopAtWaypoint() {
        waitTimer = stopDuration;
        isWaiting = true;
        animator.SetBool("isWalking", false);
    }

    void HandleWaiting() {
        waitTimer -= Time.deltaTime;
        if(waitTimer <= 0) {
            isWaiting = false;
            animator.SetBool("isWalking", true);
            if (isSearching) {
                // animator.SetBool("isSearching", false);
                isSearching = false;
            }
            GoToNextPoint();
        }
    }

    void GoToNextPoint() {
        if (isReverse) {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0) {
                currentWaypointIndex = 1;
                isReverse = false;
            }
        } else {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) {
                currentWaypointIndex = waypoints.Length - 2;
                isReverse = true;
            }
        }
    }

    void OnFootstep() {
        // 발자국 소리
    }
}
