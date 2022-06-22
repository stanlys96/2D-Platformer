using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
  private CameraController cameraController;
  public Transform cameraPosition;
  public float cameraSpeed;

  public int threshhold1, threshhold2;

  public float activeTime, fadeoutTime, inactiveTime;
  private float activeCounter, fadeCounter, inactiveCounter;

  public Transform[] spawnPoints;
  private Transform targetPoint;
  public float moveSpeed;

  public Animator animator;

  public Transform theBoss;

  public float timeBetweenShots1, timeBetweenShots2;
  private float shotCounter;
  public GameObject bullet;
  public Transform shotPoint;

  public GameObject winObjects;

  private bool battleEnded;

  public string bossRef;

  // Start is called before the first frame update
  void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        cameraController.enabled = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShots1;

        AudioManager.instance.PlayBossMusic();
    }

    // Update is called once per frame
    void Update()
    {
        cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, cameraPosition.position, cameraSpeed * Time.deltaTime);

    if (!battleEnded)
    {
      if (BossHealthController.instance.currentHealth > threshhold1)
      {
        if (activeCounter > 0)
        {
          activeCounter -= Time.deltaTime;
          if (activeCounter <= 0)
          {
            fadeCounter = fadeoutTime;
            animator.SetTrigger("vanish");
          }

          shotCounter -= Time.deltaTime;
          if (shotCounter <= 0)
          {
            shotCounter = timeBetweenShots1;

            Instantiate(bullet, shotPoint.position, Quaternion.identity);
          }
        }
        else if (fadeCounter > 0)
        {
          fadeCounter -= Time.deltaTime;
          if (fadeCounter <= 0)
          {
            theBoss.gameObject.SetActive(false);
            inactiveCounter = inactiveTime;
          }
        }
        else if (inactiveCounter > 0)
        {
          inactiveCounter -= Time.deltaTime;
          if (inactiveCounter <= 0)
          {
            theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            theBoss.gameObject.SetActive(true);

            activeCounter = activeTime;
            shotCounter = timeBetweenShots1;
          }
        }
      }
      else
      {
        if (targetPoint == null)
        {
          targetPoint = theBoss;
          fadeCounter = fadeoutTime;
          animator.SetTrigger("vanish");
        }
        else
        {
          if (Vector3.Distance(theBoss.position, targetPoint.position) > 0.2f)
          {
            theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(theBoss.position, targetPoint.position) <= 0.2f)
            {
              fadeCounter = fadeoutTime;
              animator.SetTrigger("vanish");
            }

            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
              if (PlayerHealthController.instance.currentHealth > threshhold2)
              {
                shotCounter = timeBetweenShots1;
              }
              else
              {
                shotCounter = timeBetweenShots2;
              }


              Instantiate(bullet, shotPoint.position, Quaternion.identity);
            }
          }
          else if (fadeCounter > 0)
          {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter <= 0)
            {
              theBoss.gameObject.SetActive(false);
              inactiveCounter = inactiveTime;
            }
          }
          else if (inactiveCounter > 0)
          {
            inactiveCounter -= Time.deltaTime;
            if (inactiveCounter <= 0)
            {
              theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

              targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
              int whileBreaker = 0;
              while (targetPoint.position == theBoss.position && whileBreaker < 100)
              {
                print("HEY");
                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                whileBreaker++;
              }

              theBoss.gameObject.SetActive(true);

              if (PlayerHealthController.instance.currentHealth > threshhold2)
              {
                shotCounter = timeBetweenShots1;
              }
              else
              {
                shotCounter = timeBetweenShots2;
              }
            }
          }
        }
      }
    }
    else 
    {
      fadeCounter -= Time.deltaTime;
      if (fadeCounter < 0) 
      {
        if (winObjects != null) 
        {
          winObjects.SetActive(true);
          winObjects.transform.SetParent(null);
        }

        cameraController.enabled = true;
        gameObject.SetActive(false);
        AudioManager.instance.PlayLevelMusic();

        PlayerPrefs.SetInt(bossRef, 1);
      }
    }
  }

  public void EndBattle() 
  {
    battleEnded = true;

    fadeCounter = fadeoutTime;
    animator.SetTrigger("vanish");
    theBoss.GetComponent<Collider2D>().enabled = false;

    BossBullet[] bullets = FindObjectsOfType<BossBullet>();
    if (bullets.Length > 0) 
    {
      foreach (BossBullet bossBullet in bullets)
      {
        Destroy(bossBullet.gameObject);
      }
    }
  }
}
