using UnityEngine;

public class TowerPlacementZone : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private GameObject highlightObject;

    [SerializeField]
    private Transform towerSpawnPoint;


    [Header("Placement")]

    [SerializeField]
    private bool placementEnabled = true;


    [Header("Debug")]

    [SerializeField]
    private bool showDebugLogs = true;


    private bool isOccupied;


    private void Start()
    {
        HideHighlight();

        if (highlightObject == null)
        {
            Debug.LogError($"TowerPlacementZone : aucun Highlight Object sur {gameObject.name}.", this);
        }

        if (towerSpawnPoint == null)
        {
            DebugLog("Aucun Tower Spawn Point assigné. La position de la zone sera utilisée.");
        }
    }


    public bool CanPlaceTower()
    {
        return placementEnabled && !isOccupied;
    }


    public void ShowHighlight()
    {
        if (!CanPlaceTower())
        {
            DebugLog("Surlignage non affiché : zone occupée ou désactivée.");

            HideHighlight();
            return;
        }

        if (highlightObject == null)
        {
            Debug.LogError($"TowerPlacementZone : Highlight Object manquant sur {gameObject.name}.", this);

            return;
        }

        highlightObject.SetActive(true);

        DebugLog("Surlignage affiché.");
    }


    public void HideHighlight()
    {
        if (highlightObject == null)
        {
            return;
        }

        highlightObject.SetActive(false);
    }


    public bool PlaceTower(GameObject towerPrefab)
    {
        DebugLog("PlaceTower() appelée.");

        if (!placementEnabled)
        {
            DebugLog("Placement refusé : cette zone est désactivée.");

            return false;
        }

        if (isOccupied)
        {
            DebugLog("Placement refusé : cette zone est déjà occupée.");

            return false;
        }

        if (towerPrefab == null)
        {
            Debug.LogError($"TowerPlacementZone : prefab de tour null sur {gameObject.name}.", this);

            return false;
        }

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        if (towerSpawnPoint != null)
        {
            spawnPosition = towerSpawnPoint.position;
            spawnRotation = towerSpawnPoint.rotation;
        }

        GameObject createdTower = Instantiate(
            towerPrefab,
            spawnPosition,
            spawnRotation
        );

        if (createdTower == null)
        {
            Debug.LogError(
                $"TowerPlacementZone : échec de création de la tour sur {gameObject.name}.",
                this
            );

            return false;
        }

        isOccupied = true;

        HideHighlight();

        DebugLog(
            $"Tour créée : {createdTower.name}"
        );

        return true;
    }


    private void DebugLog(string message)
    {
        if (!showDebugLogs)
        {
            return;
        }

        Debug.Log(
            $"[TowerPlacementZone - {gameObject.name}] {message}",
            this
        );
    }
}