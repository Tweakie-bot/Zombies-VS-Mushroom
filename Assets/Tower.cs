using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Combat")]

    [SerializeField]
    private float range = 5f;

    [SerializeField]
    private float attackCooldown = 1f;


    [Header("Rotation")]

    [SerializeField]
    private Transform rotatingPart;

    [SerializeField]
    private float rotationSpeed = 5f;


    [Header("Projectile")]

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform firePoint;


    [Header("Selection")]

    [SerializeField]
    private GameObject selectionHighlight;

    [SerializeField]
    private GameObject rangeVisual;

    private GameObject towerCanvas;

    private EnemyHealth currentTarget;
    private float attackTimer;

    private bool isSelected;

    // Une seule tourelle peut être sélectionnée à la fois.
    private static Tower selectedTower;


    private void Start()
    {
        Deselect();

        SetupRangeVisual();

        towerCanvas = GameObject.Find("TowerCanvas");
        towerCanvas.SetActive(false);
    }


    private void Update()
    {
        FindClosestTarget();

        if (currentTarget != null)
        {
            RotateTowardsTarget();

            if (attackTimer <= 0f)
            {
                Attack();
            }
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        if (isSelected && Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }


    private void OnMouseDown()
    {
        Select();
    }


    private void FindClosestTarget()
    {
        EnemyHealth[] enemies =
            FindObjectsByType<EnemyHealth>(
            );

        EnemyHealth closestEnemy = null;

        float closestDistance = Mathf.Infinity;


        foreach (EnemyHealth enemy in enemies)
        {
            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance > range)
            {
                continue;
            }

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }


        currentTarget = closestEnemy;
    }


    private void RotateTowardsTarget()
    {
        if (rotatingPart == null || currentTarget == null)
        {
            return;
        }

        Vector3 direction =
            currentTarget.transform.position
            - rotatingPart.position;

        // Rotation horizontale uniquement.
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        rotatingPart.rotation =
            Quaternion.Slerp(
                rotatingPart.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
    }


    private void Attack()
    {
        if (currentTarget == null)
        {
            return;
        }

        if (projectilePrefab == null || firePoint == null)
        {
            return;
        }

        // On enregistre uniquement la position actuelle
        // de l'ennemi au moment du tir.
        Vector3 targetPosition =
            currentTarget.transform.position;

        Vector3 direction =
            targetPosition - firePoint.position;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion projectileRotation =
            Quaternion.LookRotation(direction);

        Instantiate(
            projectilePrefab,
            firePoint.position,
            projectileRotation
        );

        attackTimer = attackCooldown;
    }


    private void Select()
    {
        if (selectedTower != null && selectedTower != this)
        {
            selectedTower.Deselect();
        }

        selectedTower = this;
        isSelected = true;

        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(true);
        }

        if (towerCanvas != null)
        {
            towerCanvas.SetActive(true);
        }

        if (rangeVisual != null)
        {
            rangeVisual.SetActive(true);
        }
    }


    public void Deselect()
    {
        isSelected = false;

        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(false);
        }

        if (towerCanvas != null)
        {
            towerCanvas.SetActive(false);
        }

        if (rangeVisual != null)
        {
            rangeVisual.SetActive(false);
        }

        if (selectedTower == this)
        {
            selectedTower = null;
        }
    }


    private void SetupRangeVisual()
    {
        if (rangeVisual == null)
        {
            return;
        }

        Vector3 scale =
            rangeVisual.transform.localScale;

        scale.x = range * 2f;
        scale.z = range * 2f;

        rangeVisual.transform.localScale = scale;
    }
}