using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gochu : MonoBehaviour
{
    [SerializeField] private GameObject piss;
    /* Stores the prefab piss so player can instantiate piss object using prefab in order to fire piss */
    [SerializeField] float horizontal;
    /* player horizontal input */
    [SerializeField] float vertical;
    /* player vertical input */
    float moveLimiter = 1.415f;
    /* float to insure when holding down both horizontal and vertical movement keys, player speed is not unintentionally accelerated */
    bool canTakeDamage = true;
    /* bool that checks if player can take damage so invisibility frames can be activated*/
    bool canShoot = true;
    /* bool for whether player can fire a projectile */
    bool canReload = true;
   
    float invincibilityTimer = 1f;
    /* float for seconds which player is invincible */
    float fireRateCooldown = 0.5f;
    /* float for seconds which player is unable to shoot after shooting */
    float reloadTimer = 4f;
    /* float for seconds which player is reloading */
    Rigidbody2D rb;
    /* RigidBody2D for physics */
    [SerializeField] float peeRecoil;
    /* float to measure recoil by pee weapon */
    [SerializeField] float radius;
    /* distance to which pee is fired in regards to player */
    [SerializeField] int health;
    /* int to gauge player health */
    [SerializeField] float movementMultiplier;
    /* float for movement speed */
    int maxAmmo = 6;
    /* int for ammo capacity */
    [SerializeField] int currentAmmo;
    /* int for current ammo */
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();  
      /* declare RigidBody2d rb */
      currentAmmo = maxAmmo;
      /* sets currentAmmo to max */
    }
    private float GetAngleToCursor(Vector3 pos) 
    /* gets the angle from player to the cursor */
    { 
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos;
        /* takes coordinate of cursor and subtracts coordinate of player to find x & y distance between the two */
        return Mathf.Atan2(lookDirection.y, lookDirection.x);
        /* returns angle between vector coordinates */
    }


    private IEnumerator ReloadTimer()
    {      
        canReload = false;
        /* canReload set to false so reloading cannot occur while reloading */
        canShoot = false;
        /* canShoot set to false so player cannot shoot while realoding*/
        yield return new WaitForSeconds(reloadTimer);
        /* reload timer activates */
        canShoot = true;
        /* canShoot set to true when reloading is finish so player can shoot */
        canReload = true;
        /* canReload set to true so player can reload after reloading is finished */
        currentAmmo = maxAmmo;
        /* ammo is reloaded */
    }
    private IEnumerator InvincibilityTimer()
    /* invincibility frames to prevent player from instant dying to collision */
    {
        yield return new WaitForSeconds(invincibilityTimer);
        /* invincibility timer */
        canTakeDamage = true;
        /* set canTakeDamage to true -> invincibility frames ends */
    }
    
    private IEnumerator ShootCooldownTimer()
    /* limits the attack speed of player */
    {
        yield return new WaitForSeconds(fireRateCooldown);
        /* attack speed timer */
        canShoot = true;
        /* allows to player to shoot again once timer is finished */
    }
    private void FixedUpdate() 
    {
        if (horizontal != 0 && vertical != 0) 
        /* if both horizontal & vertical movement keys are being pressed */
        {
            horizontal /= moveLimiter;
            /* horizontal movement limited to prevent unintentional extra movement */
            vertical /= moveLimiter;
            /* vertical  movement limited to prevent unintentional extra movement */
        }
        transform.position = new Vector2
        /* transform position to move player */
        (
            transform.position.x + horizontal * movementMultiplier,
            /* takes current x coordinate and shifts it by horizontal input times movementMultipler */ 
            transform.position.y + vertical * movementMultiplier
            /* takes current y coordinate and shifts it by horizontal input times movementMultipler */ 
        );
        rb.velocity = new Vector2(0f,0f);
        /* velocity set to 0 to ensure that player is not unintentionally boosted by high velocity obkect */
    }
    
    private void FirePiss(float theta)
    /* fires piss object from player and takes float theta as angle */
    {
        if (canShoot == true && currentAmmo > 0 && canReload)
        /* if canShoot is true and current ammo is greater than 0 and canReload is also true */
        {
            StartCoroutine(ShootCooldownTimer());
            /* Shoot timer activated */
            GameObject currentPiss = Instantiate(piss, new Vector3(transform.position.x + (Mathf.Cos(theta) * radius), transform.position.y + (Mathf.Sin(theta) * radius), transform.position.z), Quaternion.identity);
            /* creates a piss object using piss prefab at offset  */
            currentPiss.transform.rotation = Quaternion.Euler(0, 0, theta * Mathf.Rad2Deg);
            /* rotates instantiated piss object to face same angle as player is facing using angle theta */
            currentPiss.GetComponent<Rigidbody2D>().velocity = currentPiss.transform.right * 10;
            /* gets the RigidBody2d component and changes velocity to move projectile */
            transform.position = new Vector3(transform.position.x + (Mathf.Cos(theta + 3.14f) * peeRecoil), transform.position.y + (Mathf.Sin(theta + 3.14f) * peeRecoil), transform.position.z);
            /* adds recoil to piss projectile fire; moves player opposite to direction they are facing */
            currentAmmo--;
            /* reduces ammo in weapon after shooting */
            canShoot = false;
            /* set to false to limit fire rate */
        }
        else
        if (currentAmmo == 0 && canReload)
        /* if ammo is empty and canReload is true */
        {
            StartCoroutine(ReloadTimer());
            /* calls reload function if player attempts to fire weapon when ammo is empty */
        }
    }
    void Update()
    {
        float theta = GetAngleToCursor(transform.position);
        /* uses player position vector to get angle from player to cursor */
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, theta * Mathf.Rad2Deg)); 
        /* changes rotation of player sprite when cursor position changes */
        if (Input.GetMouseButton(0))
        /* if left mouse button is pressed */
        {
           FirePiss(theta);
           /* calls FirePiss to fire piss projectile */
        }
        
        if (Input.GetKey("r") && currentAmmo < maxAmmo && canReload)
        /* if r key is pressed */
        {
            StartCoroutine(ReloadTimer());
            /* reload weapon */
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        /* updates horizontal input */
        vertical = Input.GetAxisRaw("Vertical");
        /* updates vertical input */
    }  
    private void OnCollisionStay2D(Collision2D collision) 
    {
        if (canTakeDamage)
        /* if canTakeDamage = true */
        {
            StartCoroutine(InvincibilityTimer());
            /* invincibility frames are started */
            health--;
            /* damage is taken, health is decreased */
            canTakeDamage = false;
            /* set to false so player can have invincibility frames */
        }

        if (health <= 0)
        /* if health is 0 or less than 0 */
        {
            Destroy(gameObject);
            /* player is destroyed */
        }
    }
}