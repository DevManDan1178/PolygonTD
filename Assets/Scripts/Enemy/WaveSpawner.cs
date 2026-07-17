using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{ 
    public static int EnemiesAlive = 0;
    public Transform spawnPoint;
    public float timeBetweenWaves;
    public Text waveCountdownText;
    public Text waveCountText;
    private float PrepCountdown = 8f;
    private float WaveTimer;
        private int waveIndex = 0;
    public EnemyBlueprint [] enemies;
    private float waveWeight;
    private bool spawning = false;
    public GameManager gameManager;
    [SerializeField] private int finalWave;
    [Header("Wave money = wave index * wave cash scaling")]
    public float waveCashScaling;
    public float baseWaveCash = 50;
    [Header("y = wave Weight, x = wave Index, y = a(x-h)^2 + k")]   
    public float a;
    public float h;
    public float k;
    [Header("t = WaveTimer, x = wave Index, t = b[c * x]  + d | d is minimum time")]
    public float b;
    public float c;
    public float d;
    [Header("t = spawn Interval, x = wave index, t = random(0.5, 1.5) / (e + x/f)")]
    public float e;
    public float f;
    void FixedUpdate()
    {   
        if(GameManager.gameEnded) return;
        
        if(EnemiesAlive > 0 || spawning == true){
        //Wave is in progress (currently spawning or alive)
            if (WaveTimer <= 0f) {
                SpawnWave();
                return;
            }
            WaveTimer -= Time.deltaTime;
            WaveTimer = Mathf.Clamp(WaveTimer, 0f, Mathf.Infinity);
            waveCountdownText.text = string.Format("{0:00.00}", WaveTimer);
            return;
        }
        if(PrepCountdown <= 0f || (EnemiesAlive == 0 && waveIndex > 0   )) //Wave should be starting
        {   
            SpawnWave();
            PrepCountdown = timeBetweenWaves;
            return;
        }
        
        //Wave is in intermission
        PrepCountdown -= Time.deltaTime;
        waveCountdownText.text = string.Format("{0:00.00}", Mathf.Max(0, PrepCountdown));
    }
    void SpawnWave()
    {   
        //if it is already final wave (cannot spawn wave after, so end game)
        if(waveIndex >= finalWave){
            gameManager.WinLevel();
            this.enabled = false;
            return;
        } 
        //Spawn, Increase wave index, Select enemies, Set WaveTimer
        PlayerStats.Money += baseWaveCash + waveIndex * waveCashScaling;
        spawning = true;
        waveIndex++;
        waveCountText.text = "wave " + waveIndex.ToString();
        PlayerStats.Rounds ++;
        PlayerStats.instance.UpdateStats();

        StartCoroutine(SelectEnemies());
        if(waveIndex == finalWave){WaveTimer = Mathf.Infinity;}
        else{WaveTimer = b * Mathf.Floor(c * waveIndex)  + d;} 
    }

    IEnumerator SelectEnemies(){
    waveWeight = a * ((waveIndex - h)*(waveIndex - h)) + k;
    float minWeightCost = Mathf.Infinity;
    List<EnemyBlueprint> spawnableEnemies = new List<EnemyBlueprint>(); 
    for (int i = 0; i < enemies.Length; i++)
    {
        if(enemies[i].minimumWave > waveIndex || enemies[i].maximumWave < waveIndex) continue;
        spawnableEnemies.Add(enemies[i]);
        if(enemies[i].weight < minWeightCost) minWeightCost = enemies[i].weight;
        
    }
        while(waveWeight >= minWeightCost){
            int i = Random.Range(0 , spawnableEnemies.Count);
            if (spawnableEnemies.Count == 0){
                break;
            }   
            if (spawnableEnemies[i].weight > waveWeight){ 
                spawnableEnemies.RemoveAt(i);
            continue;
            }
         waveWeight -= spawnableEnemies[i].weight;
         SpawnEnemy(spawnableEnemies[i]);
         //time to wait = random between 0.5 and 1.5 divided by (e + waveindex /f)
         yield return new WaitForSeconds(Random.Range(0.5f,1.5f)/(e + waveIndex/f));           
        }
        spawning = false; 
    }
    void SpawnEnemy(EnemyBlueprint enemy)
    {
        Instantiate(enemy.EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemiesAlive ++;
    }
}
    

