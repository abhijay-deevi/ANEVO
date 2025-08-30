using UnityEngine;

public class Fireball : MonoBehaviour, IAttack
{
    public GameObject fireballPrefab;
    public Transform point; 

    public float speed = 10f;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private void Update()
    {
        timeBtwShots -= Time.deltaTime;
    }

    public void performAttack(Transform target)
    {
        if (timeBtwShots <= 0)
        {
            GameObject fireball = Instantiate(fireballPrefab, point.position, point.rotation);

            // Use the caster's facing direction
            Vector2 shootDir = point.up;
            fireball.GetComponent<Projectiles>().SetDirection(shootDir);

            timeBtwShots = startTimeBtwShots;
        }
    }
}
