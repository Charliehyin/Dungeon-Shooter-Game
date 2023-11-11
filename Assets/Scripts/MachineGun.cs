using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
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
        fireRate = 0.05f;
        damage = 1f;
        range = 100f;
        currentTime = 0f;
        speed = 50f;
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
            float angle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;
            // If the angle is greater than 90 or less than -90, flip the sprite
            if (angle > 90 || angle < -90)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
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

        // Create a random angle between -10 and 10 degrees
        float angle = Random.Range(-10f, 10f);

        // Set the bullet's velocity to the direction * speed offset by the angle
        bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, angle) * direction * speed;
        bullet.GetComponent<Bullet>().damage = damage;

        // Play the shoot sound
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
    }
}
