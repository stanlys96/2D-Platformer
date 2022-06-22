using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private PlayerController playerController;
  public BoxCollider2D boxCollider2d;

  private float halfHeight, halfWidth;

  // Start is called before the first frame update
  void Start()
  {
    playerController = FindObjectOfType<PlayerController>();
    halfHeight = Camera.main.orthographicSize;
    halfWidth = halfHeight * Camera.main.aspect;

    AudioManager.instance.PlayLevelMusic();
  }

  // Update is called once per frame
  void Update()
  {
    if (playerController != null)
    {
      transform.position = new Vector3(
          Mathf.Clamp(playerController.transform.position.x, boxCollider2d.bounds.min.x + halfWidth, boxCollider2d.bounds.max.x - halfWidth),
          Mathf.Clamp(playerController.transform.position.y, boxCollider2d.bounds.min.y + halfHeight, boxCollider2d.bounds.max.y - halfHeight),
          transform.position.z
        );
    } else {
      playerController = FindObjectOfType<PlayerController>();
    }
  }
}
