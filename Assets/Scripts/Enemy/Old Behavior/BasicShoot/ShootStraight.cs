using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootStraight : MonoBehaviour
{
    public GameObject bullet;

	public float fireRate;
	private float nextFire;

	// Use this for initialization
	void Start () {
		fireRate = 2f;
		nextFire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		CheckIfTimeToFire ();
	}

	void CheckIfTimeToFire()
	{
		if (Time.time > nextFire) {
			Instantiate (bullet, transform.position, Quaternion.identity);
			nextFire = Time.time + fireRate;
		}
	}

}
/*

    // egg bullet behavior
    public GameObject bulletPrefab;
    public GameObject target;
    public Transform firePoint;
    private float bulletSpeed = 40f;
    private float cooldown = 0.2f;
    private float nextFire = 0f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    void Update() {
        ProcessBulletSpawn();
    }
    private void ProcessBulletSpawn() {
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) && Time.time > nextFire) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Vector3 bulletPos = bullet.transform.position;
            bulletPos += bullet.transform.up * (bulletSpeed * Time.smoothDeltaTime);
            bullet.transform.position = bulletPos;
            nextFire = Time.time + cooldown;
        }
        if (Time.time > nextFire) {
            rb = GetComponent<Rigidbody2D> ();
            target = GameObject.Find("Hero");
            moveDirection = (target.transform.position - transform.position).normalized * bulletSpeed;
            rb.velocity = new Vector2 (moveDirection.x, moveDirection.y);
            Destroy (gameObject, 3f);
        }
    }
    */

