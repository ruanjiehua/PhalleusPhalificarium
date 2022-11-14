using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dong : MonoBehaviour
{
    GameObject gochu;
    /* player object for enemy movement */
    [SerializeField] int health;
    /* health of dong enemy */
    Rigidbody2D rb;
    /* rigidbody for collision damage */

    void Start()
    {
        gochu = FindObjectOfType<Gochu>().gameObject;  
        /* declares gochu */
        rb = GetComponent<Rigidbody2D>();
        /* declares rb */
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {  
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Enemy")
        /* if the object being collided with does not have "Player" && "Enemy" tag  */
        {
            Destroy(collision.gameObject);
            /* destroy object being collided with */
            health--;
            /* health decreases from projectile hit */
        }

        if (health <= 0)
        /* if health is 0 or less than 0 */
        {
            Destroy(gameObject);
            /* destroy this Dong object */
        }
    }

    void Update()
    {
        if (gochu != null)
        /* if gochu is not null (destoryed/dead) */
        {
            Vector2 difference = gochu.transform.position - transform.position;
            /* Vector2 difference between player position and enemy position */
            float theta = Mathf.Atan2(difference.y, difference.x);
            /* angle theta between player and enemy */
            transform.rotation = Quaternion.Euler(0, 0, theta * Mathf.Rad2Deg);
            /* rotates the enemy by theta to face player */
            rb.velocity = transform.right * 0.4f;
            /* gives enemy velocity to move toward player */
        } 
        else 
        /* else if player is null (destroyed/dead) */
        {
            rb.velocity =  new Vector2(0,0);
            /* enemy movement speed set to 0 */
        }
    }
}