using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private TowerPlacementManager placementManager;

    [SerializeField]
    private GameObject towerPrefab;


    [Header("Debug")]

    [SerializeField]
    private bool showDebugLogs = true;


    public void SelectTower()
    {
        DebugLog("Le bouton a reçu le clic.");

        if (placementManager == null)
        {
            Debug.LogError(
                "TowerSelectionButton : Placement Manager n'est pas assigné.",
                this
            );

            return;
        }

        if (towerPrefab == null)
        {
            Debug.LogError(
                "TowerSelectionButton : Tower Prefab n'est pas assigné.",
                this
            );

            return;
        }

        DebugLog(
            $"Envoi du prefab {towerPrefab.name} au Placement Manager."
        );

        placementManager.SelectTower(towerPrefab);
    }


    private void DebugLog(string message)
    {
        if (!showDebugLogs)
        {
            return;
        }

        Debug.Log(
            $"[TowerSelectionButton - {gameObject.name}] {message}",
            this
        );
    }
}