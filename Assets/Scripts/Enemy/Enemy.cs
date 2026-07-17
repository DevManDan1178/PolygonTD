using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    [HideInInspector] public static Enemy hoveredEnemy;
    [Header("Attributes")]
    public float startSpeed = 3f;
    [HideInInspector] public float speed;
    public float startHealth = 3;
    [HideInInspector] public float health;
    public int enemyLeakDamage;
    public int CashValue;
    private Transform target;
    public float rotateSpeed;
    [HideInInspector]
    private float slowedSpeed;
    private float slowDuration;
    [Header("Unity components")]
    [SerializeField] private  Text healthText;
    [SerializeField] private  Image healthbar;
    [HideInInspector]
    public float hypotheticalHealth; //amount of health that turrets use to detect, so turrets dont unnecessarily shoot
    [SerializeField] private GameObject spriteHolder;
    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    [SerializeField] private Canvas HpCanvas;
    private bool isDead = false;

    void Start()
    { 
        health = startHealth;
        healthText.text = health + "/" + startHealth;
        speed = startSpeed;
        hypotheticalHealth = health;
        spriteRenderer = spriteHolder.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        HpCanvas.enabled = false;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthText.text = health + "/" + startHealth;
        healthbar.fillAmount = health / startHealth;
        StartCoroutine(FlashDamage());
        if (health <= 0)
        {
            Die();
        }
        //hypotheticalHealth = health;
    }

    IEnumerator FlashDamage(){
        if(!spriteRenderer || spriteRenderer.color != defaultColor) yield break;
        spriteRenderer.color = new Color (defaultColor.r - 0.15f, defaultColor.g - 0.15f, defaultColor.b - 0.15f, defaultColor.a);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = defaultColor;
    }
    void Update(){
        HpCanvas.enabled = this == hoveredEnemy;
        if (rotateSpeed > 0f)  {
            spriteHolder.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime, Space.World);
        }
        if(speed < startSpeed){
            slowDuration -= Time.deltaTime;
            if(slowDuration <= 0) {
                speed = startSpeed;
            }
        }
    }
    void Die(){
        if(isDead) return;
        isDead = true;
        PlayerStats.Money += CashValue; //update player stats
        PlayerStats.instance.UpdateStats();
        WaveSpawner.EnemiesAlive --;    
        AudioManager.instance.Play("Enemy Death");
        Destroy(gameObject);
    }
    public void EndPath()
    {
        PlayerStats.Lives -= (int)Mathf.Ceil((health/ startHealth) * enemyLeakDamage); //update player stats
        PlayerStats.instance.UpdateStats();
        AudioManager.instance.Play("Enemy Leak");
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
        
    }
    public void Slow(float percent, float duration)
    {
        if (speed > slowedSpeed){
        speed = speed * (100 - percent) / 100;
        slowDuration = duration;
        slowedSpeed = speed;
        }
    }
}
