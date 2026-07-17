using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Bullet : MonoBehaviour
{   [HideInInspector]
    private Transform target;
    public float speed;
    public GameObject impactEffect;
    public float explosionRadius;
    public string enemyTag = "Enemy";
    [HideInInspector]
    public float bulletDamage;
    [HideInInspector]
    public float slowPercent;
    [HideInInspector]
    public float slowDuration;
    [HideInInspector] public bool homingBullet;
    [HideInInspector] public int pierce;
    private int enemiesHit = 0;
    private float bulletRange;
    private Vector3 dir;
    private Vector3 targetPosition;
    Collider2D[] inExplosionRadius = null;
    public void Seek(Transform _target, float damage, float _slowPercent, float _slowduration, int _pierce, float _blastRadius)
    {
        target = _target;
        bulletDamage = damage;
        slowPercent = _slowPercent;
        slowDuration = _slowduration;
        homingBullet = true;
        pierce = _pierce;
        explosionRadius = _blastRadius;
        enemiesHit = 0;
    }
    private Vector3 destroyPosition;
    public void Cast(Vector3 _targetPosition, float damage, float _slowPercent, float _slowduration, int _pierce, float _blastRadius, float _range)
    {   
        dir = _targetPosition - transform.position; 
        bulletDamage = damage;
        slowPercent = _slowPercent;
        slowDuration = _slowduration;
        homingBullet = false;
        pierce = _pierce;
        explosionRadius = _blastRadius;
        bulletRange = _range;
        enemiesHit = 0;
        destroyPosition = transform.position + (dir.normalized * bulletRange * 1.25f);
    }
    // Update is called once per frame
    void Update()
    {
        if (homingBullet && target){
            dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.up = target.position - transform.position; 
            destroyPosition =  transform.position + (dir.normalized * bulletRange/3);
            targetPosition = target.position;
        }else{ //fling towards position
            float distanceThisFrame = speed * Time.deltaTime;  
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);  
            transform.up = dir; 
            if ((destroyPosition - transform.position).magnitude <= 0.05f) gameObject.SetActive(false);        
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider) 
    {  
        if(otherCollider.gameObject.tag != enemyTag) return;
        HitTarget(otherCollider.gameObject.transform);
    }

    void HitTarget(Transform target_)
    {
        
        GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 1f);
        
        if(explosionRadius > 0f)
        {   AudioManager.instance.Play("Explosion");
            Explode();
        } 
        else{
            AudioManager.instance.Play("Bullet Impact");
            Damage(target_);        
        }
        enemiesHit += 1;
        if (enemiesHit > pierce) gameObject.SetActive(false);
    }
    void Explode()
    { 
        inExplosionRadius = Physics2D.OverlapCircleAll(targetPosition, explosionRadius);
        foreach (Collider2D collider in inExplosionRadius)
        {  
            Damage(collider.transform);
        }
    }
    void Damage(Transform enemy)
    {
        if(enemy.GetComponent<Enemy>() != null)
        {
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(bulletDamage);
        e.Slow(slowPercent, slowDuration);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
