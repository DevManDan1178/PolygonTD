using UnityEngine.EventSystems;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    
    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]

    private Renderer rend;
    private Color startColor;
    public float upgradeLevel;
    [HideInInspector] public float sellValue = 0f;
    [HideInInspector] public float sellReturn = 0.85f;
    BuildManager buildManager;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
        if(transform.childCount > 0) turret = transform.GetChild(0).gameObject;
    }

    public void OnClick() {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {//if UI is above
            return;
        } 
        if(turret != null)
        {  
            buildManager.SelectNode(this);
            return;
        }
        if(!buildManager.CanBuild)
        {
            return;
        }
        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            return;
        }
        turretBlueprint = blueprint; //built blueprint is now node's blueprint
        PlayerStats.Money -= blueprint.cost;
        PlayerStats.instance.UpdateStats();
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, transform.position, Quaternion.identity);
        turret = _turret;
        //turretBlueprint.UpdateSellValue();
        sellValue += blueprint.cost * sellReturn;
        buildManager.SelectNode(this);
    }
    public bool UpgradeTurret(float Updamage, float UpfireRate, float Uprange, int Uppierce, float UpblastRadius)
    {
        if(PlayerStats.Money < turretBlueprint.upgradeCost) {
            return false;
        }
        PlayerStats.Money -= turretBlueprint.upgradeCost;
        PlayerStats.instance.UpdateStats();
        Turret upTurret =  this.turret.GetComponent<Turret>();
        //upgrade turret
        upTurret.damage += Updamage;
        upTurret.fireRate += UpfireRate;
        upTurret.range +=  Uprange;
        upTurret.pierce += Uppierce;
        upTurret.blastRadius += UpblastRadius;
        sellValue += turretBlueprint.upgradeCost * sellReturn;
        //turretBlueprint.UpdateSellValue();
        return true;
    }
    public void SellTurret(){
        if(this.turret != null){
            Destroy(this.turret);
            //PlayerStats.Money += turretBlueprint.sellValue;
            PlayerStats.Money += sellValue;
            PlayerStats.instance.UpdateStats();        
        }
    }

    public void ChangeTargetingMode()
    {
        if(turret == null) return;
        turret.GetComponent<Turret>().ChangeTargetingMode();
    }

    /*
    void OnMouseEnter() 
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return; //If UI is above 
        if(!buildManager.CanBuild) return; //If no turret selected
        if(buildManager.HasMoney){
            rend.material.color = hoverColor;
            //show sprite of turret to build
        
        } else{
            rend.material.color = notEnoughMoneyColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
    */
}
