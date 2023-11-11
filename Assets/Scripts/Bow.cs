using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    // Start is called before the first frame update
    public float fireRate;
    public float damage;
    public float range;
    public float speed;
    public GameObject bulletPrefab;
    float currentTime;
    Camera cam;
    private Vector3 originalScale;
    public AudioClip shootSound;
    void Start()
    {
        fireRate = 1f;
        damage = 12f;
        range = 100f;
        currentTime = 0f;
        speed = 150f;
        cam = Camera.main;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.GetButton("Fire1") && currentTime >= fireRate && transform.parent.name == "Player")
        {
            Shoot();
            currentTime = 0f;
        }
        Vector3 parentScale = transform.parent.lossyScale;
        transform.localScale = new Vector3(originalScale.x / parentScale.x, originalScale.y / parentScale.y, originalScale.z / parentScale.z);

        // Set the rotation to the mouse position
        if (transform.parent.name == "Player")
        {
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg - 45);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
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

        // Set the bullet's rotation to the direction
        bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180);

        //play the shoot sound
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
    }
}
