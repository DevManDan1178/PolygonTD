using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurretBlueprint : PlaceableBlueprint
{
    [HideInInspector]
    public float upgradeCost;
    [HideInInspector]
    public float sellValue;
    [Header("Cost Taxing Value Start(after what value to start taxing)")] //the tax is multiplication of the regular price (upgrade cost scaling) multiplied by the upgrade cost taxing
    public float damageCostTaxingStart;
    public float fireRateCostTaxingStart;
    public float rangeCostTaxingStart;
    public float pierceCostTaxingStart;
    public float blastRadiusTaxingStart;
    [Header("Upgrade Cost Scaling(scaling * stat bonus) before tax")]
    public float damageUpgradeScaling;
    public float fireRateUpgradeScaling;
    public float rangeUpgradeScaling;
    public float pierceUpgradeScaling;
    public float blastRadiusUpgradeScaling;
    private float allUpgradeCosts = 0f;
    [Header("Upgrade Cost Tax: scaling * (1 + tax/100)^(value - tax start)")]
    public float damageTax;
    public float fireRateTax;
    public float rangeTax;    
    public float pierceTax;
    public float blastRadiusTax;
    public void scaleUpgradeCost(float _damage, float _fireRate, float _range, int _pierce, float _blastRadius, Node node){
        //scale cost with upgrades
    Turret nodeTurret = node.turret.GetComponent<Turret>();
        if( nodeTurret!= null){
            float turretDamage = nodeTurret.damage;
            float turretFireRate = nodeTurret.fireRate;
            float turretRange = nodeTurret.range;
            float turretPierce = nodeTurret.pierce;
            float TurretBlastRadius = nodeTurret.blastRadius;
            //upgrade cost = upgradeScaling * upgradeAmount * tax | tax = extra stats * tax value clamped between 1 and 5 (capped at 5x price)
        float damageUpgradeCost = calculateUpgradeCost(turretDamage, _damage, damageUpgradeScaling, damageCostTaxingStart, damageTax);
        float fireRateUpgradeCost = calculateUpgradeCost(turretFireRate, _fireRate, fireRateUpgradeScaling, fireRateCostTaxingStart, fireRateTax) ;
        float rangeUpgradeCost = calculateUpgradeCost(turretRange, _range, rangeUpgradeScaling, rangeCostTaxingStart, rangeTax);
        float pierceUpgradeCost = calculateUpgradeCost(turretPierce, _pierce, pierceUpgradeScaling, pierceCostTaxingStart, pierceTax);
        float blastRadiusUpgradeCost = calculateUpgradeCost(TurretBlastRadius, _blastRadius, blastRadiusUpgradeScaling, blastRadiusTaxingStart, blastRadiusTax);
        upgradeCost = damageUpgradeCost + fireRateUpgradeCost + rangeUpgradeCost + pierceUpgradeCost + blastRadiusUpgradeCost;
        }    
    }
    private float GetTotalUpgradeCost(float value, float scaling, float taxingStart, float tax)
    {
        if (value == 0 || scaling == 0)
        {
            return 0;
        }
        if (value <= taxingStart){
            return scaling * value;
        }
        float preTax = scaling * taxingStart;

        float excess = value - taxingStart;
        float growth = Mathf.Pow(1 + tax * 0.01f, excess);

        return preTax + scaling * (growth - 1) / (tax * 0.01f);
    }    
    private float calculateUpgradeCost(float currentValue, float upgradeByValue, float scaling, float taxingStart, float tax)
    {
        return GetTotalUpgradeCost(currentValue + upgradeByValue, scaling, taxingStart, tax) - GetTotalUpgradeCost(currentValue, scaling, taxingStart, tax);
    }
    public void UpdateSellValue(){
        allUpgradeCosts += upgradeCost;
        Debug.Log(allUpgradeCosts);
        sellValue = (cost + allUpgradeCosts) * 85 / 100;
    }
}
