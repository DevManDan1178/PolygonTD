using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Use Laser")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public ParticleSystem[] laserImpactEffects;

    [Header("General")]
    public float range = 2f;
    public float fireRate;
    public float damage;
    public float slowPercent;
    public float slowDuration = 0.25f;
    private float fireCountdown = 0f;
    public int pierce;
    public float blastRadius;
    public float bulletSpeed;
    public bool homingBullets;
    public LayerMask enemyLayer;
    
    public GameObject bulletPrefab;
    private Enemy targetEnemy;
    [Header("Unity Setup Fields")]
    public float targetUpdateTime = 0.3f;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    
    public Transform firePoint;
    [HideInInspector]
    public string[] targetModes;
    private int targetModeNumber = 0;
    [HideInInspector] public string targetingMode;  //-First/Last- -Weak/Strong- -Close/Far- -random-

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, targetUpdateTime);
        targetModes = new string[7]; 
        targetModes[0] = "FIRST"; targetModes[1] = "LAST"; targetModes[2] = "WEAK"; targetModes[3] = "STRONG"; targetModes[4] = "CLOSE"; targetModes[5] = "FAR"; targetModes[6] ="RANDOM";
        targetingMode = targetModes[targetModeNumber];
    }
    //setting target
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float enemyDistance = Mathf.Infinity;
        GameObject EnemyTarget = null;

        if (targetingMode == "RANDOM")
        {

            Dictionary<GameObject, float> enemiesInRange = new();
            foreach(GameObject enemy in enemies){
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy > range || !(enemy.GetComponent<Enemy>()?.hypotheticalHealth > 0)) {
                    continue;
                }
                enemiesInRange.Add(enemy, distanceToEnemy);
            }
            var entries = enemiesInRange.ToList();

            if (entries.Count > 0)
            {
                var randomPair = entries[UnityEngine.Random.Range(0, entries.Count)];

                EnemyTarget = randomPair.Key;
                enemyDistance = randomPair.Value;
            }

        } else
        if (targetingMode == "CLOSE"){
            float shortestDistance = Mathf.Infinity;
            foreach(GameObject enemy in enemies)
            {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > range) continue;
                if (distanceToEnemy < shortestDistance && enemy.GetComponent<Enemy>()?.hypotheticalHealth > 0)
                {  
                shortestDistance = distanceToEnemy;
                EnemyTarget = enemy;
                enemyDistance= distanceToEnemy;
                }
            }
        }else
        if (targetingMode == "FAR"){
            float furthestDistance = 0;
            foreach(GameObject enemy in enemies)
            {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > range) continue;
            if (distanceToEnemy > furthestDistance && enemy.GetComponent<Enemy>().hypotheticalHealth > 0)
                {  
                furthestDistance = distanceToEnemy;
                EnemyTarget = enemy;
                enemyDistance= distanceToEnemy;
                }
            }
        }else
        if (targetingMode == "WEAK"){
            float weakestHP = Mathf.Infinity;
            foreach(GameObject enemy in enemies)
            {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > range) continue;
            float HP = enemy.GetComponent<Enemy>().hypotheticalHealth;
            
                if (HP < weakestHP)
                {  
                weakestHP = HP;
                EnemyTarget = enemy;
                enemyDistance= distanceToEnemy;
                }
            }
        }else
        if (targetingMode == "STRONG"){
            float strongestHP = 0f;
            foreach(GameObject enemy in enemies)
            {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > range) continue;
            float HP = enemy.GetComponent<Enemy>().hypotheticalHealth;
            
                if (HP > strongestHP)
                {  
                strongestHP = HP;
                EnemyTarget = enemy;
                enemyDistance= distanceToEnemy;
                }
            }
        }else
        if (targetingMode == "LAST"){
            float LeastWaypoints = Mathf.Infinity;
            float MostDistance = 0; //most distance to waypoint means it reached less far
            foreach(GameObject enemy in enemies)    //sort ones with least waypoints, then sort ones with least distance
            {   
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy > range) continue;
                float Waypoints = enemy.GetComponent<EnemyMovement>().waypointIndex;
                float Distance = Vector3.Distance(enemy.transform.position, enemy.GetComponent<EnemyMovement>().target.position);
                
                if(Waypoints <= LeastWaypoints){
                    if(Waypoints < LeastWaypoints) MostDistance = 0;
                    LeastWaypoints = Waypoints;                 
                    if(Distance > MostDistance){
                    MostDistance = Distance;
                    EnemyTarget = enemy;
                    enemyDistance = distanceToEnemy;
                    }
                }

            }
        }else //First is the final one. In order to avoid error, default to targeting first
        {
            float MostWaypoints = 0;
            float LeastDistance = Mathf.Infinity; //least distance to waypoint means it reached further
            foreach(GameObject enemy in enemies)    //sort ones with least waypoints, then sort ones with least distance
            {   
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy > range) continue;
                float Waypoints = enemy.GetComponent<EnemyMovement>().waypointIndex;
                float Distance = Vector3.Distance(enemy.transform.position, enemy.GetComponent<EnemyMovement>().target.position);

                if(Waypoints >= MostWaypoints){
                   if(Waypoints > MostWaypoints) LeastDistance = Mathf.Infinity; 
                    MostWaypoints = Waypoints;
                  
                    if(Distance < LeastDistance){
                    LeastDistance = Distance;
                    EnemyTarget = enemy;
                    enemyDistance = distanceToEnemy;
                    }
                }

            }
        }

        

        if(EnemyTarget != null && enemyDistance <= range)
        {
            target = EnemyTarget.transform;
            targetEnemy= EnemyTarget.GetComponent<Enemy>();
        } else
        {
            target = null;
            return;
        }
    }

    public void ChangeTargetingMode()
    {   
        targetModeNumber = (targetModeNumber + 1) % targetModes.Length;
        targetingMode = targetModes[targetModeNumber];
    }
    // Update is called once per frame
    void Update()
    {   //refresh cooldown
        fireCountdown -= Time.deltaTime;
        //if not target
        if(target == null)
        {   if(useLaser)
            {
                foreach (ParticleSystem laserImpactEffect_ in laserImpactEffects){
                    laserImpactEffect_.gameObject.SetActive(false);
                }
                if(lineRenderer.enabled){ 
                    lineRenderer.enabled = false;
                }
            }
            return;
        }
        //shooting if target
        LockOnTarget();
        if (useLaser) //using Laser
        {
           if(fireCountdown <= 0f)
            {
                Laser();
                fireCountdown = 1f/fireRate;
            }
            fireCountdown -= Time.deltaTime;
        } else //using bullets
        {
           if(fireCountdown <= 0f)
            {
            Shoot();
            fireCountdown = 1f/fireRate;
                if(homingBullets) targetEnemy.hypotheticalHealth -= damage;
            }
        } 
    }
    //Laser
    void Laser() 
    {   
        AudioManager.instance.Play("Laser Shot");
        Enemy[] hitEnemies = {};
       
         Vector2 direction = (target.position - transform.position).normalized; 
         float distance = (target.position - transform.position).magnitude;
         Ray ray = new Ray(target.position, direction);      
         RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance, enemyLayer);
         hitEnemies = new Enemy[pierce + 1];
         int countedEnemies = 0;
         foreach (RaycastHit2D hit in hits){
            if(countedEnemies > pierce) break;
             hitEnemies[countedEnemies] = hit.transform.GetComponent<Enemy>();   
             countedEnemies += 1;    
         } 
         if (countedEnemies == 0) return;
        //will break if array is longer than elements inside it so recreate the array and erase empty space
        
        Enemy[] damageEnemies = new Enemy[countedEnemies];
        for (int i = 0; i < countedEnemies; i++) 
        {   
            //int index = Mathf.Clamp(i, 0, countedEnemies);
            damageEnemies[i] = hitEnemies[i];
            continue;   
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        //disable old particle effects and create new,  deal damage 
                      
        foreach (ParticleSystem laserImpactEffect_ in laserImpactEffects){
            laserImpactEffect_.gameObject.SetActive(false);
        }
        
        laserImpactEffects =  new ParticleSystem[countedEnemies];
        int impactEffectCount = 0;
        foreach (Enemy  enemy in damageEnemies){
            GameObject impactEffectGO = ObjectPooler.Instance.SpawnFromPool("Laser Impact Effect", enemy.transform.position, Quaternion.identity);
            ParticleSystem impactEffect = impactEffectGO.GetComponent<ParticleSystem>() ;
            impactEffect.Play();
            laserImpactEffects[impactEffectCount] = impactEffect;
            
            enemy.TakeDamage(damage);
            enemy.Slow(slowPercent, slowDuration);
            impactEffectCount += 1;
            continue;
        }
    }
    void LockOnTarget()
    {
        Vector2 direction = new Vector2(
        target.position.x - transform.position.x,
        target.position.y - transform.position.y
    );
    transform.up = -direction;
    }
    void Shoot()
    {
    AudioManager.instance.Play("Bullet Shot");
    GameObject bulletGO = ObjectPooler.Instance.SpawnFromPool(bulletPrefab.name, firePoint.position, Quaternion.identity);
    Bullet bullet = bulletGO.GetComponent<Bullet>();
    bullet.speed = bulletSpeed;
    if (bullet!= null){
        if(homingBullets){ 
            bullet.Seek(target, damage, slowPercent, slowDuration, pierce, blastRadius);
        }
        else{
            bullet.Cast(target.position, damage, slowPercent, slowDuration, pierce, blastRadius, range);
        }
    } 
    }   
}
