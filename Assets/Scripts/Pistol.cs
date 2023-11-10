using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    // Start is called before the first frame update
    public float fireRate;
    public float damage;
    public float range;
    public float speed;
    public GameObject bulletPrefab;
    float currentTime;
    Camera cam;
    void Start()
    {
        fireRate = 0.2f;
        damage = 2f;
        range = 100f;
        currentTime = 0f;
        speed = 60f;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && currentTime >= fireRate)
        {
            Shoot();
            currentTime = 0f;
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));

        // Ensure the mouseWorldPosition is in 2D space by setting z to object's z
        mouseWorldPosition.z = transform.position.z;

        // Calculate the direction from the object to the mouse position
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        // Set the bullet's velocity to the direction * speed
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        bullet.GetComponent<Bullet>().damage = damage;
    }
}
