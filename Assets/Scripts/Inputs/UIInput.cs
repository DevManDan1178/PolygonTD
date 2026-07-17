using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class UIInput
{
    public static bool WasUIHit;
    private static PointerEventData eventData;
    public static GameInput.MouseOverType ProcessClick(Vector2 screenPos)
    {
        if (EventSystem.current == null){
            Debug.Log("No current eventsystem - ProcessClick ignored");
            return GameInput.MouseOverType.NULL;
        }
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        var uiResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, uiResults);

        foreach(RaycastResult result in uiResults) {
            ExecuteEvents.Execute(
                result.gameObject,
                eventData,
                ExecuteEvents.pointerClickHandler
            );
        }
        if (uiResults.Count > 0)
        {
            return GameInput.MouseOverType.UI;
        }
        
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(GameInput.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit != null) {
            hit.GetComponent<Node>()?.OnClick();
            return GameInput.MouseOverType.OBJECT;
        }

        return GameInput.MouseOverType.NULL;
    }
    
    public static void Process()
    {
        if (EventSystem.current == null){
            Debug.Log("No current eventsystem - Process ignored");
            return;
        }
        
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(GameInput.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        Enemy enemy = hit?.GetComponent<Enemy>();
        if (enemy)
        {
            Enemy.hoveredEnemy = enemy;
        }
    }
}