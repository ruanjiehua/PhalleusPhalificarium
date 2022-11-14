using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DongMonkey : MonoBehaviour
{
    [SerializeField] private GameObject dong;
    /* GameObject dong for later replication */
    GameObject gochu;
    /* GameObject gochu for player position */
    [SerializeField] int health;
    /* health of monkey */
    Rigidbody2D rb;
    /* Rigidbody2d for physics */
    float playerRadius = 2f;
    /* radius away from player */
    float generationRadius = 2f;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        /* declaration of Rigidbody2d rb */
        gochu = FindObjectOfType<Gochu>().gameObject;
        /* declaration of player GameObject gochu */
        StartCoroutine(dongThrowTimer());
        
    }

    private IEnumerator dongThrowTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            GameObject currentDong = Instantiate
            (
                dong,
                new Vector3
                (
                    gochu.transform.position.x + Mathf.Cos(Random.Range(0, 2 * 3.14f)) * Random.Range(playerRadius, generationRadius),
                    gochu.transform.position.y + Mathf.Sin(Random.Range(0, 2 * 3.14f)) * Random.Range(playerRadius, generationRadius), 
                    transform.position.z
                ), 
                Quaternion.identity
            );
        }
    }

    void Update()
    {

    }
}
