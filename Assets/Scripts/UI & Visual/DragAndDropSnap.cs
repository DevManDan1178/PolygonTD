using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragAndDropSnap : MonoBehaviour
{
    [HideInInspector]
    public bool isDragging = false;
    public bool isTurret = false;
    private Collider2D[] colliders;
    Vector2 previousPosition;
    [HideInInspector]
    public Shop shop;
    private int amountOfColliders;
    public static float TileLength = 2;
    void Update(){
        if(!isDragging) {
            return;
        }
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(GameInput.getMousePositionVec3()); 
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x / TileLength ) * TileLength,Mathf.RoundToInt(transform.position.y / TileLength) * TileLength);
        if(previousPosition != (Vector2)transform.position){
            CheckColliders();
        }
        if (GameInput.IsMouseNonUIClick())
        {
            OnClick();
        }

    }
    
    public void OnClick()
    {
        if(!isTurret) {
            CheckIfBuildable();
        } else {
            shop.CancelBuild();
            Destroy(gameObject);
        }  
    }

    
    public void GetShop(Shop _shop){
        shop = _shop;
    }
    void CheckColliders()
    {
         colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
         previousPosition = transform.position;
         amountOfColliders = colliders.Length;
    }
    void CheckIfBuildable(){
        if(amountOfColliders == 0){ //start build
            if(Shop.tileToBuild == shop.path){
                isDragging = false;
                CheckPathExtention();  
                Destroy(gameObject);
            }else{
                isDragging = false;
                shop.BuildTile(transform.position);
                Destroy(gameObject);
            }
        }
        if(amountOfColliders > 0){ //cant build
            CancelDragAndDrop();
        }
    }
    void CheckPathExtention(){
        bool nextToEnd = false;
        Collider2D[] checkColliders = Physics2D.OverlapCircleAll(transform.position, TileLength/2);
        for (int i = 0; i < checkColliders.Length; i++){   
        if(checkColliders[i].tag == "EndPath"){ 
            nextToEnd = true;
            SpriteRenderer pathEnd = checkColliders[i].gameObject.GetComponent<SpriteRenderer>();
            pathEnd.color = Color.white;
            checkColliders[i].tag = "Path";
            //change color back to white
        }
        }
        if(nextToEnd == true){
            shop.BuildTile(transform.position);

        } else CancelDragAndDrop();
    }

    void CancelDragAndDrop(){
        Destroy(gameObject);
        shop.CancelBuild();
    }
    public void DestroyGameObject(){ Destroy(gameObject); }
}
