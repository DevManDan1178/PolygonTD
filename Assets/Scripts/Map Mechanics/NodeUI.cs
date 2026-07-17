using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NodeUI : MonoBehaviour
{
    private Node target; //The node being hovered
    public static NodeUI instance;
    public GameObject uI;
    [HideInInspector]
    public float upgradeDamage = 0f;
    [HideInInspector]
    public float upgradeFireRate = 0f;
    [HideInInspector]
    public float upgradeRange = 0f;
    [HideInInspector]
    public int upgradePierce = 0;
    [HideInInspector]
    public float upgradeBlastRadius = 0f;
    [Header ("Special Stats UI")]
    public GameObject Piercepanel;
    public GameObject BlastRadiusPanel;
    [Header ("UI Elements - Functions")]
    public Text upgradeCost;
    public Text sellValue;
    [Header ("UI Elements - Current Stats")]
    public Text currentDamageTXT;
    public Text currentFireRateTXT;
    public Text currentRangeTXT;
    public Text currentPierceTXT;
    public Text currentBlastRadiusTXT;
    [Header ("UI Elements - Upgraded Stats")]
    public Text upgradedDamageTXT;
    public Text upgradedFireRateTXT;
    public Text upgradedRangeTXT;
    public Text upgradedPierceTXT;
    public Text upgradedBlastRadiusTXT;
    [Header ("Other")]
    public GameObject rangeCircle;
    public Text targetMode;
    private GameObject currentRangeCircle;
    private Turret nodeTurret;
    void Start(){
        instance = this;
    }
     
    void Update(){
        if(!target) {
            return;
        }
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }
    public void SetTarget(Node _target){
        AudioManager.instance.Play("UI Click Heavy");
        target = _target;
        nodeTurret = target.turret.GetComponent<Turret>();
        uI.SetActive(true);
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        if(target.turretBlueprint.prefab == null){
            upgradeCost.text = "N/A"; 
        }
        else{ 
            upgradeCost.text = Mathf.Round(target.turretBlueprint.upgradeCost).ToString();
        } 
        sellValue.text = Mathf.Round(target.sellValue).ToString();
        //set upgrade values to 0
        upgradeDamage = 0f;
        upgradeFireRate = 0f;
        upgradeRange = 0f;
        upgradePierce = 0;
        upgradeBlastRadius = 0f;
        LoadStats();
        string turretTargetMode = nodeTurret.targetingMode;
        targetMode.text = "TARGET: " + turretTargetMode;
    }
    public void Hide(){
        if(currentRangeCircle) {
            AudioManager.instance.Play("UI Click Heavy");
        }
        Untoggle();
    }

    private void Untoggle()
    {
        uI.SetActive(false);
        if(currentRangeCircle) {
            Destroy(currentRangeCircle);
        }
        target = null; 
    }
    public void Upgrade(){
        bool upgradeSuccess = target.UpgradeTurret(upgradeDamage, upgradeFireRate, upgradeRange, upgradePierce, upgradeBlastRadius);
         //reset upgrade values
        upgradeDamage = 0f;
        upgradeFireRate = 0f;
        upgradeRange = 0f;
        if (upgradeSuccess == false) {
            AudioManager.instance.Play("Error");
            LoadStats();
            return;
        }
        BuildManager.instance.DeselectNode();

    }
    public void Sell(){
        target.SellTurret();
        BuildManager.instance.DeselectNode();
        AudioManager.instance.Play("UI Click Heavy");
    }

    public void ChangeTargeting()
    {
        AudioManager.instance.Play("UI Click Heavy");
        target.ChangeTargetingMode();
        string turretTargetMode = target.turret.GetComponent<Turret>().targetingMode;
        targetMode.text = "TARGET: " + turretTargetMode;
    }

    void LoadStats(){
        if(nodeTurret == null)
        {
            return;
        }
        
        //show current stats
        float TurretDmg = nodeTurret.damage;
        float TurretFireRate = nodeTurret.fireRate;
        float TurretRange = nodeTurret.range;

        currentDamageTXT.text = TurretDmg.ToString();
        currentFireRateTXT.text = TurretFireRate.ToString();
        currentRangeTXT.text = TurretRange.ToString();

        //show  upgrade stats
        float upgradeDMG= TurretDmg + upgradeDamage;
        float upgradeFR= TurretFireRate + upgradeFireRate;
        float upgradeR= TurretRange + upgradeRange;
        upgradedDamageTXT.text = upgradeDMG.ToString();
        upgradedFireRateTXT.text = upgradeFR.ToString();
        upgradedRangeTXT.text = upgradeR.ToString();
        
        //special stats
        if (nodeTurret.pierce > 0){ Piercepanel.SetActive(true);
            float TurretPierce = nodeTurret.pierce;
            currentPierceTXT.text = TurretPierce.ToString();
            float upgradeP = TurretPierce + upgradePierce; 
            upgradedPierceTXT.text = upgradeP.ToString();
        }else Piercepanel.SetActive(false);
        if (nodeTurret.blastRadius >0f){  BlastRadiusPanel.SetActive(true);
            float TurretBlastRadius = nodeTurret.blastRadius;
            currentBlastRadiusTXT.text = TurretBlastRadius.ToString();
            float upgradeBR = TurretBlastRadius + upgradeBlastRadius;
            upgradedBlastRadiusTXT.text = upgradeBR.ToString();
        }else BlastRadiusPanel.SetActive(false);
        
        //Scale upgrade Cost
        target.turretBlueprint.scaleUpgradeCost(upgradeDamage, upgradeFireRate, upgradeRange, upgradePierce, upgradeBlastRadius, target);
        if(target.turretBlueprint.prefab == null){upgradeCost.text = "N/A"; }
        else{upgradeCost.text = Mathf.Round(target.turretBlueprint.upgradeCost).ToString();} 
        sellValue.text = Mathf.Round(target.sellValue).ToString();
        //Display range 
        DisplayRange(upgradeR);
    }

    void DisplayRange(float TurretRange){
    if(currentRangeCircle) Destroy(currentRangeCircle);
        GameObject range = GameObject.Instantiate(rangeCircle, target.transform.position, Quaternion.identity, target.transform);
        range.transform.localScale = new Vector3(TurretRange, TurretRange, 0);
        currentRangeCircle = range;
    }
    public void AddDamageUpgrade(float _damage){
        upgradeDamage += _damage;
        LoadStats();
        AudioManager.instance.Play("UI Click");
    }
    public void AddFireRateUpgrade(float _fireRate){
        upgradeFireRate += _fireRate;
        LoadStats();  
        AudioManager.instance.Play("UI Click");     
    }
    public void AddRangeUpgrade(float _range){
        upgradeRange += _range;
        LoadStats();      
        AudioManager.instance.Play("UI Click");
    }
    public void AddPierceUpgrade(int _pierce){ //only if turret already has pierce
        if(nodeTurret.pierce < 1) return;
        upgradePierce += _pierce;
        LoadStats();
        AudioManager.instance.Play("UI Click");
    }
    public void AddBlastRadiusUpgrade(float _blastRadius){
        if(nodeTurret.blastRadius == 0f) return;
        upgradeBlastRadius += _blastRadius;
        LoadStats();
        AudioManager.instance.Play("UI Click");
    }
}
