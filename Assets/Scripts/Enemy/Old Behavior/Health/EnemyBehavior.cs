/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float hitPoints;
    public float maxHitPoints = 5;
    public EnemyHealthBar healthbar;
    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHitPoints;
        healthbar.SetHealth(hitPoints, maxHitPoints);
    }

    public void TakenHit(float damage) {
        hitPoints -= damage;
        healthbar.SetHealth(hitPoints, maxHitPoints);

        if(hitPoints <= 0) {
            Destroy(gameObject);
        }
    }
}
*/