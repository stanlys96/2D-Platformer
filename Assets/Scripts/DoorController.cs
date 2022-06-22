using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
  public Animator animator;

  public float distanceToOpen;

  private PlayerController playerController;

  private bool playerExiting;

  public Transform exitPoint;
  public float movePlayerSpeed;

  public string levelToLoad;

  // Start is called before the first frame update
  void Start()
  {
      playerController = PlayerHealthController.instance.GetComponent<PlayerController>();
  }

    // Update is called once per frame
  void Update()
  {
    if (Vector3.Distance(transform.position, playerController.transform.position) < distanceToOpen) 
    {
      animator.SetBool("doorOpen", true);
    }
    else 
    {
      animator.SetBool("doorOpen", false);
    }

    if (playerExiting) {
      // playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, exitPoint.position, movePlayerSpeed * Time.deltaTime);
      playerController.transform.position = exitPoint.position;
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      if (!playerExiting) {
        playerController.canMove = false;

        StartCoroutine(UseDoorCo());
      }
    }
  }

  IEnumerator UseDoorCo() {
    playerExiting = true;

    playerController.animator.enabled = false;

    UIController.instance.StartFadeToBlack();

    yield return new WaitForSeconds(1.5f);
    RespawnController.instance.SetSpawn(exitPoint.position);
    playerController.canMove = true;
    playerController.animator.enabled = true;

    UIController.instance.StartFadeFromBlack();

    PlayerPrefs.SetString("ContinueLevel", levelToLoad);
    PlayerPrefs.SetFloat("PosX", exitPoint.position.x);
    PlayerPrefs.SetFloat("PosY", exitPoint.position.y);
    PlayerPrefs.SetFloat("PosZ", exitPoint.position.z);

    SceneManager.LoadScene(levelToLoad);
  }
}
