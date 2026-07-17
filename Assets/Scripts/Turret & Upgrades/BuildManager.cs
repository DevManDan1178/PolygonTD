using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildManager : MonoBehaviour
{   
    public static BuildManager instance;
    
    void Awake() {
        if(instance != null)
        {
            Debug.LogError("More than one BuildManager is present in scene!");
            return;
        }
        instance = this;
    }
    private TurretBlueprint turretToBuild;
    [HideInInspector] public Node SelectedNode;
    public NodeUI nodeUI;
    public bool CanBuild { get {return turretToBuild != null; }}
    public bool HasMoney {get {return PlayerStats.Money >= turretToBuild.cost;}}
    
    public void SelectNode(Node node)
    {   if(SelectedNode == node)
        {
            DeselectNode();
        return;
        }
        SelectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }
    public void DeselectNode(){
        SelectedNode = null;
        nodeUI.Hide();
    }
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }
    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
}
