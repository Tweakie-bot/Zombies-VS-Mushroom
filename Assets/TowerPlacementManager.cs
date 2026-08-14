using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacementManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private TowerPlacementZone[] placementZones;


    [Header("Debug")]

    [SerializeField]
    private bool showDebugLogs = true;

    private bool canPlaceTowers;

    private GameObject selectedTowerPrefab;


    private void Start()
    {
        DebugReferences();
        HideAllZones();
    }


    private void Update()
    {
        if (selectedTowerPrefab == null)
        {
            return;
        }

        if (!canPlaceTowers)
        {
            DebugLog("Placement annulé parce que le placement n'est plus autorisé.");

            CancelPlacement();
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            DebugLog("Clic droit et annulation du placement.");

            CancelPlacement();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                DebugLog("Clic gauche ignoré : la souris est sur l'interface.");

                return;
            }

            TryPlaceTower();
        }
    }


    public void SelectTower(GameObject towerPrefab)
    {
        DebugLog("SelectTower() appelée.");

        if (!canPlaceTowers)
        {
            DebugLog("Sélection refusée : nous ne sommes pas en préparation.");

            return;
        }

        if (towerPrefab == null)
        {
            Debug.LogError("TowerPlacementManager : le prefab reçu est null.", this);

            return;
        }

        selectedTowerPrefab = towerPrefab;

        DebugLog($"Tour sélectionnée : {selectedTowerPrefab.name}");

        ShowAvailableZones();
    }


    public void SetPlacementAllowed(bool isAllowed)
    {
        canPlaceTowers = isAllowed;

        DebugLog($"Autorisation de placement : {canPlaceTowers}");

        if (!canPlaceTowers)
        {
            CancelPlacement();
        }
    }


    public void CancelPlacement()
    {
        if (selectedTowerPrefab != null)
        {
            DebugLog($"Sélection annulée : {selectedTowerPrefab.name}");
        }

        selectedTowerPrefab = null;

        HideAllZones();
    }


    private void TryPlaceTower()
    {
        DebugLog("Tentative de placement.");

        if (mainCamera == null)
        {
            Debug.LogError("TowerPlacementManager : Main Camera n'est pas assignée.", this);

            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            DebugLog("Le raycast n'a touché aucun collider.");

            return;
        }

        DebugLog($"Collider touché : {hit.collider.gameObject.name}");

        TowerPlacementZone zone = hit.collider.GetComponentInParent<TowerPlacementZone>();

        if (zone == null)
        {
            DebugLog("Le collider touché n'appartient pas à une PlacementZone.");

            return;
        }

        DebugLog($"Zone détectée : {zone.gameObject.name}");

        if (!zone.CanPlaceTower())
        {
            DebugLog($"Placement refusé : la zone {zone.gameObject.name} est occupée ou désactivée.");

            return;
        }

        bool towerWasPlaced = zone.PlaceTower(selectedTowerPrefab);

        if (!towerWasPlaced)
        {
            DebugLog("La zone a refusé de créer la tour.");

            return;
        }

        DebugLog($"Tour {selectedTowerPrefab.name} placée sur {zone.gameObject.name}.");

        CancelPlacement();
    }


    private void ShowAvailableZones()
    {
        DebugLog("Affichage des zones disponibles.");

        if (placementZones == null)
        {
            Debug.LogError("TowerPlacementManager : le tableau Placement Zones est null.", this);

            return;
        }

        for (int i = 0; i < placementZones.Length; i++)
        {
            TowerPlacementZone zone = placementZones[i];

            if (zone == null)
            {
                Debug.LogError($"TowerPlacementManager : l'élément {i} du tableau Placement Zones est vide.", this);

                continue;
            }

            zone.ShowHighlight();
        }
    }


    private void HideAllZones()
    {
        if (placementZones == null)
        {
            return;
        }

        for (int i = 0; i < placementZones.Length; i++)
        {
            TowerPlacementZone zone = placementZones[i];

            if (zone == null)
            {
                continue;
            }

            zone.HideHighlight();
        }
    }


    private void DebugReferences()
    {
        if (mainCamera == null)
        {
            Debug.LogError("TowerPlacementManager : Main Camera n'est pas assignée.", this);
        }
        else
        {
            DebugLog($"Caméra assignée : {mainCamera.gameObject.name}");
        }

        if (placementZones == null)
        {
            Debug.LogError("TowerPlacementManager : le tableau Placement Zones est null.", this);

            return;
        }

        DebugLog($"Nombre d'emplacements assignés : {placementZones.Length}");

        for (int i = 0; i < placementZones.Length; i++)
        {
            if (placementZones[i] == null)
            {
                Debug.LogError($"TowerPlacementManager : l'emplacement {i} du tableau est vide.", this);
            }
            else
            {
                DebugLog($"Zone {i} assignée : {placementZones[i].gameObject.name}");
            }
        }
    }


    private void DebugLog(string message)
    {
        if (!showDebugLogs)
        {
            return;
        }

        Debug.Log($"[TowerPlacementManager] {message}", this);
    }
}