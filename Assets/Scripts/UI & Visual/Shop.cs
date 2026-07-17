using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviour
{
    [Header("Shop Elements")]
    public TurretBlueprint[] turretBlueprints;
    public bool[] turretBlueprintsPlaceable;
    /*    
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;
    public TurretBlueprint machineTurret;
    */
    public PlaceableBlueprint node;
    public bool nodePlaceable = true;
    public PlaceableBlueprint path;
    public bool pathPlaceable = true;

    [Header("Tile Shop Configurations")] 
    public GameObject RangeCirclePerfab;
    public GameObject NodePreview;  
    public GameObject PathPreview;
    public GameObject nodeParentObject;
    public GameObject pathParentObject;
    [HideInInspector]
    public static PlaceableBlueprint tileToBuild;
    private GameObject currentBuildingTile;
    private GameObject currentBuildingTurret;
    BuildManager buildManager;

    [Header("Shop Prices Texts")]
    public Text[] turretCostTexts;
    public Text nodeCost;
    public Text pathCost;
    [Header("Placeable Buttons")]
    public Button[] placeableTurretButtons;
    public Button nodePlaceableButton;
    public Button pathPlaceableButton;
    [Header ("Other")]
    public Button CancelButton;
    public Text CancelButtonText;
    private Color disabledCostTextColor = new Color(0.65f,0.65f,0.65f);
    private Color disabledPlaceableButtonColor = new Color(0.75f,0.75f,0.75f,0.8f);
    [HideInInspector]
    public static Shop instance;
    void Start(){
        instance = this;
        buildManager = BuildManager.instance;
        int idx = 0;
        foreach(Text turretCostText in turretCostTexts)
        {
            turretCostText.text = turretBlueprints[idx++].cost.ToString();
        }
        for (int i = 0; i < placeableTurretButtons.Length; i++)
        {
            Button turretButton = placeableTurretButtons[i];
            if (!(i < turretBlueprints.Length && i < turretBlueprintsPlaceable.Length && turretBlueprintsPlaceable[i]))
            {
                setButtonDisabled(turretButton);
                turretCostTexts[i].color = disabledCostTextColor;
            }
            
        }
        if (!nodePlaceable)
        {
            setButtonDisabled(nodePlaceableButton);
            nodeCost.color = disabledCostTextColor;
        }
        if (!pathPlaceable)
        {
            setButtonDisabled(pathPlaceableButton);
            pathCost.color = disabledCostTextColor;
        }
   
        /*
        standardTurretCost.text =  standardTurret.cost.ToString();
        missileLauncherCost.text = missileLauncher.cost.ToString();
        laserBeamerCost.text = laserBeamer.cost.ToString();
        machineTurretCost.text = machineTurret.cost.ToString();
        */
        nodeCost.text = node.cost.ToString();
        pathCost.text = path.cost.ToString();
    }

    private void setButtonDisabled(Button button)
    {
        button.interactable = enabled;
        var img = button.GetComponent<UnityEngine.UI.Image>();
        img.color = disabledCostTextColor;
    }

    void SetDragAndDrop(GameObject gameObject_){
        //create turret object with range circle, add drag and drop snap
        GameObject turretPreview = (GameObject)Instantiate(gameObject_, Camera.main.ScreenToWorldPoint(GameInput.getMousePositionVec3()), Quaternion.identity);
        float turretRange =  turretPreview.GetComponent<Turret>().range;
        GameObject rangeCircle = GameObject.Instantiate(RangeCirclePerfab, turretPreview.transform.position, Quaternion.identity, turretPreview.transform);
        rangeCircle.transform.localScale = new Vector3(turretRange, turretRange, 0f);
        turretPreview.GetComponent<Turret>().enabled = false;
        DragAndDropSnap Tdrag = turretPreview.AddComponent<DragAndDropSnap>();
        Tdrag.isDragging = true;
        Tdrag.GetShop(this);
        Tdrag.isTurret = true;
        currentBuildingTurret = turretPreview;
        //do the same for the turret preview tile
        GameObject turretTilePreview = (GameObject)Instantiate(PathPreview, turretPreview.transform.position, Quaternion.identity);
        turretTilePreview.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 40);   
        DragAndDropSnap Ndrag = turretTilePreview.GetComponent<DragAndDropSnap>();
        Ndrag.isDragging = true;
        Ndrag.GetShop(this);
        Ndrag.isTurret = true;
        currentBuildingTile = turretTilePreview;
    }

    public void SelectTurretBlueprint(int index)
    {      
        CancelBuild();
        if (!(index < turretBlueprints.Length && index < turretBlueprintsPlaceable.Length && turretBlueprintsPlaceable[index])){
            return;
        }
        if (PlayerStats.Money < turretBlueprints[index].cost)
        {
            AudioManager.instance.Play("Error");
            return;
        }

        buildManager.SelectTurretToBuild(turretBlueprints[index]);
        ToggleCancelButton(true);
        SetDragAndDrop(turretBlueprints[index].prefab);
    }

    //create a function for each turret as button cannot send a TurretBlueprint value
    public void SelectStandardTurret(){
        SelectTurretBlueprint(0);
    }
    public void SelectMissileLauncher(){
        SelectTurretBlueprint(1);
    }
    public void SelectLaserBeamer(){
        SelectTurretBlueprint(2);
    }
    public void SelectMachineTurret(){
        SelectTurretBlueprint(3);
    }
    public void PrepareNode(){
        CancelBuild();
        if(!nodePlaceable || tileToBuild != null) {
            return;
        }
        if (PlayerStats.Money < node.cost)
        {
            AudioManager.instance.Play("Error");
            return;
        }
        ToggleCancelButton(true);   

        tileToBuild = node;
        GameObject nodePosition = (GameObject)Instantiate(NodePreview, Camera.main.ScreenToWorldPoint(GameInput.getMousePositionVec3()), Quaternion.identity);
        DragAndDropSnap drag = nodePosition.GetComponent<DragAndDropSnap>();
        drag.isDragging = true;
        drag.GetShop(this);
        currentBuildingTile = nodePosition;
    }
    public void BuildTile(Vector3 position){
        
        if(tileToBuild == path){
            GameObject tile = (GameObject)Instantiate(tileToBuild.prefab, position, Quaternion.identity, pathParentObject.transform); 
            FindAnyObjectByType<Waypoints>().AddWaypoint(tile.transform);
            PlayerStats.Money -= tileToBuild.cost;
            tileToBuild = null;
        }
        if(tileToBuild == node){
            Instantiate(tileToBuild.prefab, position, Quaternion.identity, nodeParentObject.transform);
            PlayerStats.Money -= tileToBuild.cost;
            tileToBuild = null;
        }
        PlayerStats.instance.UpdateStats();

        ToggleCancelButton(false);
    }
    public void PreparePathTile(){
        CancelBuild();
        if(!pathPlaceable || tileToBuild != null) {
            return;
        } 
        if (PlayerStats.Money < path.cost)
        {
            AudioManager.instance.Play("Error");
            return;
        }
        ToggleCancelButton(true);
        tileToBuild = path;
        GameObject pathPosition = (GameObject)Instantiate(PathPreview, Camera.main.ScreenToWorldPoint(GameInput.getMousePositionVec3()), Quaternion.identity);
        DragAndDropSnap drag = pathPosition.GetComponent<DragAndDropSnap>();
        drag.isDragging = true;
        drag.GetShop(this);
        currentBuildingTile = pathPosition;
    }
    public void CancelBuild(){
        if(currentBuildingTile != null && currentBuildingTile.GetComponent<DragAndDropSnap>() != null) currentBuildingTile.GetComponent<DragAndDropSnap>().DestroyGameObject();
        if(currentBuildingTurret != null && currentBuildingTurret.GetComponent<DragAndDropSnap>() != null) currentBuildingTurret.GetComponent<DragAndDropSnap>().DestroyGameObject();
        currentBuildingTile = null;
        tileToBuild = null;
        buildManager.SelectTurretToBuild(null);
        ToggleCancelButton(false);
    }

    private void SelectBlueprint(int blueprintIndex)
    {   
        
        if (blueprintIndex >= turretBlueprints.Length + 2)
        {
            return;
        }
        if (blueprintIndex < turretBlueprints.Length)
        {
            SelectTurretBlueprint(blueprintIndex);
            return;
        }
        if (turretBlueprints.Length - blueprintIndex == 0)
        {
            PrepareNode();
            return;
        } 
        PreparePathTile();
    }
     void Update() {
        if((currentBuildingTile != null || currentBuildingTurret != null) && !InputBridge.keyboardInputDisabled && Input.GetKeyDown(KeyCode.Q) && !GameManager.paused) {
            CancelBuild();   
            return;
        }
        for (int i = 1; i <= turretBlueprints.Length + 2; i++) {
            KeyCode key = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Alpha" + i);

            if (!InputBridge.keyboardInputDisabled && Input.GetKeyDown(key) && !GameManager.paused){
                SelectBlueprint(i - 1);
            }
        }
    }


    public void ToggleCancelButton(bool active){//false = inactive | true = active
        if (CancelButton.gameObject.activeSelf == active)
        {
            return;
        }

        CancelButton.gameObject.SetActive(active);
        /*
        if (active == false){
            CancelButton.GetComponent<Image>().color = new Color32(170,255,215,255);
            CancelButtonText.text = "SELECT";
        }*/
        if (active == true){
            CancelButton.GetComponent<Image>().color = new Color32(255,190,170,255);
            CancelButtonText.text = "CANCEL (Q)";
        }
        AudioManager.instance.Play("UI Click Heavy");
    }

}

