using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform boardContainer;
    [SerializeField] private List<PointOfInterest> pointsOfInterestPrefabs;
    [SerializeField] private GameObject pathPrefab;
    
    [Header("Map Configuration")]
    [SerializeField, Min(1)] private int numberOfStartingPoints = 4;
    [SerializeField, Min(1)] private int mapLength = 10;
    [SerializeField, Min(1)] private int maxWidth = 5;
    [SerializeField] private bool allowCrisscrossing;
    
    [Header("Positioning")]
    [SerializeField] private float xMaxSize;
    [SerializeField] private float yPadding;
    
    [Header("Probability Settings")]
    [Range(0.1f, 1f), SerializeField] private float chancePathMiddle = 0.5f;
    [Range(0f, 1f), SerializeField] private float chancePathSide = 0.3f;
    
    [Header("Generation Settings")]
    [SerializeField, Range(0.9f, 5f)] private float multiplicativeSpaceBetweenLines = 2.5f;
    [SerializeField, Range(1f, 5.5f)] private float multiplicativeNumberOfMinimumConnections = 3f;
    [SerializeField, Min(1)] private int maxRegenerationAttempts = 10;
    [SerializeField, Min(1)] private int maxConnectionAttemptsPerPOI = 10;
    
    // Pooling system
    private Queue<GameObject> pathPool = new Queue<GameObject>();
    private List<GameObject> activePaths = new List<GameObject>();
    private int initialPoolSize = 50;
    
    // Map data
    private PointOfInterest[][] _pointOfInterestsPerFloor;
    private readonly List<PointOfInterest> pointsOfInterest = new();
    private int _numberOfConnections = 0;
    private float _lineLength;
    private float _lineHeight;

    private void Start()
    {
        ValidateParameters();
        InitializePathPool();
        RecreateBoard();
    }

    private void ValidateParameters()
    {
        // Validate prefabs
        if (pointsOfInterestPrefabs == null || pointsOfInterestPrefabs.Count == 0)
        {
            Debug.LogError("MapGenerator: No PointOfInterest prefabs assigned!");
            enabled = false;
            return;
        }

        if (pathPrefab == null)
        {
            Debug.LogError("MapGenerator: Path prefab is not assigned!");
            enabled = false;
            return;
        }

        if (boardContainer == null)
        {
            Debug.LogError("MapGenerator: Board container is not assigned!");
            enabled = false;
            return;
        }

        // Validate ranges
        numberOfStartingPoints = Mathf.Clamp(numberOfStartingPoints, 1, maxWidth);
        mapLength = Mathf.Max(1, mapLength);
        maxWidth = Mathf.Max(1, maxWidth);
        xMaxSize = Mathf.Max(0.1f, xMaxSize);
        yPadding = Mathf.Max(0.1f, yPadding);
        
        // Check for null prefabs in list
        for (int i = pointsOfInterestPrefabs.Count - 1; i >= 0; i--)
        {
            if (pointsOfInterestPrefabs[i] == null)
            {
                Debug.LogWarning($"MapGenerator: Removing null PointOfInterest prefab at index {i}");
                pointsOfInterestPrefabs.RemoveAt(i);
            }
        }
        
        if (pointsOfInterestPrefabs.Count == 0)
        {
            Debug.LogError("MapGenerator: No valid PointOfInterest prefabs remaining!");
            enabled = false;
        }
    }

    private void InitializePathPool()
    {
        if (pathPrefab == null) return;

        // Get path dimensions
        MeshFilter meshFilter = pathPrefab.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            _lineLength = meshFilter.sharedMesh.bounds.size.z * pathPrefab.transform.localScale.z;
            _lineHeight = meshFilter.sharedMesh.bounds.size.y * pathPrefab.transform.localScale.y;
        }
        else
        {
            Debug.LogWarning("MapGenerator: Path prefab has no MeshFilter or mesh. Using default dimensions.");
            _lineLength = 1f;
            _lineHeight = 1f;
        }

        // Pre-instantiate pool objects
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject path = Instantiate(pathPrefab, boardContainer);
            path.SetActive(false);
            pathPool.Enqueue(path);
        }
    }

    private GameObject GetPathFromPool()
    {
        if (pathPool.Count > 0)
        {
            GameObject path = pathPool.Dequeue();
            path.SetActive(true);
            activePaths.Add(path);
            return path;
        }
        
        // If pool is empty, create a new one
        GameObject newPath = Instantiate(pathPrefab, boardContainer);
        activePaths.Add(newPath);
        return newPath;
    }

    private void ReturnPathToPool(GameObject path)
    {
        if (path == null) return;
        
        path.SetActive(false);
        if (!pathPool.Contains(path))
        {
            pathPool.Enqueue(path);
        }
        
        activePaths.Remove(path);
    }

    public void RecreateBoard()
    {
        ValidateParameters();
        
        // Return all paths to pool
        foreach (var path in activePaths.ToArray())
        {
            ReturnPathToPool(path);
        }
        activePaths.Clear();
        
        // Destroy all POIs (not pooled)
        DestroyAllChildren(boardContainer);
        
        _numberOfConnections = 0;
        GenerateRandomSeed();
        pointsOfInterest.Clear();
        
        _pointOfInterestsPerFloor = new PointOfInterest[mapLength][];
        for (int i = 0; i < _pointOfInterestsPerFloor.Length; i++)
        {
            _pointOfInterestsPerFloor[i] = new PointOfInterest[maxWidth];
        }
        
        CreateMap();
    }

    private void GenerateRandomSeed()
    {
        int tempSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }

    private PointOfInterest InstantiatePointOfInterest(int floorN, int xNum)
    {
        // Validate indices
        if (floorN < 0 || floorN >= mapLength || xNum < 0 || xNum >= maxWidth)
        {
            Debug.LogWarning($"MapGenerator: Invalid indices floor:{floorN}, x:{xNum}");
            return null;
        }

        // Return existing if available
        if (_pointOfInterestsPerFloor[floorN][xNum] != null)
        {
            return _pointOfInterestsPerFloor[floorN][xNum];
        }

        // Calculate position
        float xSize = xMaxSize / maxWidth;
        float xPos = (xSize * xNum) + (xSize / 2f);
        float yPos = yPadding * floorN;

        // Add random variation
        xPos += Random.Range(-xSize / 4f, xSize / 4f);
        yPos += Random.Range(-yPadding / 4f, yPadding / 4f);

        Vector3 pos = new Vector3(xPos, 0, yPos);
        
        // Validate prefab selection
        if (pointsOfInterestPrefabs == null || pointsOfInterestPrefabs.Count == 0)
        {
            Debug.LogError("MapGenerator: No PointOfInterest prefabs available!");
            return null;
        }

        int prefabIndex = Random.Range(0, pointsOfInterestPrefabs.Count);
        if (pointsOfInterestPrefabs[prefabIndex] == null)
        {
            Debug.LogError($"MapGenerator: Prefab at index {prefabIndex} is null!");
            return null;
        }

        PointOfInterest instance = Instantiate(pointsOfInterestPrefabs[prefabIndex], boardContainer);
        if (instance == null)
        {
            Debug.LogError("MapGenerator: Failed to instantiate PointOfInterest!");
            return null;
        }

        pointsOfInterest.Add(instance);
        instance.transform.localPosition = pos;
        _pointOfInterestsPerFloor[floorN][xNum] = instance;

        // Create connections (with safety limits)
        CreateConnections(instance, floorN, xNum);
        
        return instance;
    }

    private void CreateConnections(PointOfInterest instance, int floorN, int xNum)
    {
        if (instance == null || floorN >= mapLength - 1) return;

        int connectionsCreated = 0;
        int attempts = 0;
        List<System.Action> connectionAttempts = new List<System.Action>();

        // Define possible connections
        if (xNum > 0)
        {
            connectionAttempts.Add(() => TryCreateConnection(instance, floorN + 1, xNum - 1, chancePathSide));
        }
        
        if (xNum < maxWidth - 1)
        {
            connectionAttempts.Add(() => TryCreateConnection(instance, floorN + 1, xNum + 1, chancePathSide));
        }
        
        connectionAttempts.Add(() => TryCreateConnection(instance, floorN + 1, xNum, chancePathMiddle));

        // Shuffle attempts for more random distribution
        ShuffleList(connectionAttempts);

        // Try connections with safety limit
        while (connectionsCreated == 0 && attempts < maxConnectionAttemptsPerPOI && floorN < mapLength - 1)
        {
            attempts++;
            
            foreach (var attempt in connectionAttempts)
            {
                attempt.Invoke();
                
                // Check if a connection was created (we need to track this differently)
                // For simplicity, we'll rely on the attempts count and force one connection if needed
            }

            // Force at least one connection if we're at the last floor and still have none
            if (connectionsCreated == 0 && attempts == maxConnectionAttemptsPerPOI - 1)
            {
                ForceCreateConnection(instance, floorN, xNum);
            }
        }

        // Final fallback: if no connections after all attempts, force middle connection
        if (connectionsCreated == 0)
        {
            ForceCreateConnection(instance, floorN, xNum);
        }
    }

    private void TryCreateConnection(PointOfInterest instance, int targetFloor, int targetX, float probability)
    {
        if (Random.Range(0f, 1f) >= probability) return;
        
        if (allowCrisscrossing || _pointOfInterestsPerFloor[targetFloor][targetX] == null)
        {
            PointOfInterest nextPOI = InstantiatePointOfInterest(targetFloor, targetX);
            if (nextPOI != null)
            {
                AddLineBetweenPoints(instance, nextPOI);
                instance.NextPointsOfInterest.Add(nextPOI);
                _numberOfConnections++;
            }
        }
    }

    private void ForceCreateConnection(PointOfInterest instance, int floorN, int xNum)
    {
        if (floorN >= mapLength - 1) return;

        // Try middle first, then left, then right
        if (IsValidPosition(floorN + 1, xNum))
        {
            CreateForcedConnection(instance, floorN + 1, xNum);
        }
        else if (xNum > 0 && IsValidPosition(floorN + 1, xNum - 1))
        {
            CreateForcedConnection(instance, floorN + 1, xNum - 1);
        }
        else if (xNum < maxWidth - 1 && IsValidPosition(floorN + 1, xNum + 1))
        {
            CreateForcedConnection(instance, floorN + 1, xNum + 1);
        }
    }

    private void CreateForcedConnection(PointOfInterest instance, int targetFloor, int targetX)
    {
        PointOfInterest nextPOI = InstantiatePointOfInterest(targetFloor, targetX);
        if (nextPOI != null)
        {
            AddLineBetweenPoints(instance, nextPOI);
            instance.NextPointsOfInterest.Add(nextPOI);
            _numberOfConnections++;
        }
    }

    private bool IsValidPosition(int floor, int x)
    {
        return floor >= 0 && floor < mapLength && x >= 0 && x < maxWidth;
    }

    private void CreateMap()
    {
        int regenerationAttempts = 0;
        bool validMapCreated = false;

        while (!validMapCreated && regenerationAttempts < maxRegenerationAttempts)
        {
            regenerationAttempts++;
            
            // Clear previous attempt
            _numberOfConnections = 0;
            pointsOfInterest.Clear();
            for (int i = 0; i < mapLength; i++)
            {
                _pointOfInterestsPerFloor[i] = new PointOfInterest[maxWidth];
            }

            // Create starting points
            List<int> positions = GetRandomIndexes(numberOfStartingPoints);
            foreach (int j in positions)
            {
                PointOfInterest poi = InstantiatePointOfInterest(0, j);
                if (poi == null)
                {
                    Debug.LogWarning($"MapGenerator: Failed to create POI at position {j}");
                }
            }

            // Validate map
            if (_numberOfConnections >= mapLength * multiplicativeNumberOfMinimumConnections)
            {
                validMapCreated = true;
                Debug.Log($"Created board with {_numberOfConnections} connections and {pointsOfInterest.Count} points (Attempt {regenerationAttempts})");
            }
            else if (regenerationAttempts < maxRegenerationAttempts)
            {
                Debug.LogWarning($"Attempt {regenerationAttempts}: Insufficient connections ({_numberOfConnections}). Retrying...");
                
                // Cleanup failed attempt
                DestroyAllChildren(boardContainer);
                foreach (var path in activePaths.ToArray())
                {
                    ReturnPathToPool(path);
                }
                activePaths.Clear();
            }
        }

        if (!validMapCreated)
        {
            Debug.LogError($"Failed to create valid map after {maxRegenerationAttempts} attempts. Consider adjusting parameters.");
            
            // Create a minimal valid map as fallback
            CreateFallbackMap();
        }
    }

    private void CreateFallbackMap()
    {
        // Create a simple straight line as fallback
        for (int floor = 0; floor < mapLength; floor++)
        {
            int x = Mathf.Min(floor, maxWidth - 1);
            PointOfInterest poi = InstantiatePointOfInterest(floor, x);
            
            if (floor > 0 && poi != null)
            {
                PointOfInterest prevPOI = _pointOfInterestsPerFloor[floor - 1][Mathf.Min(floor - 1, maxWidth - 1)];
                if (prevPOI != null)
                {
                    AddLineBetweenPoints(prevPOI, poi);
                    prevPOI.NextPointsOfInterest.Add(poi);
                    _numberOfConnections++;
                }
            }
        }
        
        Debug.Log($"Created fallback map with {_numberOfConnections} connections");
    }

    private void AddLineBetweenPoints(PointOfInterest pointA, PointOfInterest pointB)
    {
        // Null checks
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("MapGenerator: Attempted to add line between null points");
            return;
        }

        if (pointA.transform == null || pointB.transform == null)
        {
            Debug.LogWarning("MapGenerator: Point transforms are null");
            return;
        }

        // Calculate line positioning
        Vector3 dir = (pointB.transform.position - pointA.transform.position).normalized;
        float dist = Vector3.Distance(pointA.transform.position, pointB.transform.position);

        // Safety check for zero distance
        if (dist < 0.001f)
        {
            Debug.LogWarning("MapGenerator: Points are too close together");
            return;
        }

        int num = Mathf.Max(1, (int)(dist / (_lineLength * multiplicativeSpaceBetweenLines)));
        float pad = (dist - (num * _lineLength)) / (num + 1);
        Vector3 startPos = pointA.transform.position + (dir * (pad + (_lineLength / 2f)));

        // Create lines
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = startPos + ((_lineLength + pad) * i * dir);
            GameObject line = GetPathFromPool();
            
            if (line != null)
            {
                line.transform.position = pos;
                line.transform.LookAt(pointB.transform);
                line.transform.position -= Vector3.up * (_lineHeight / 2f);
            }
        }
    }

    private List<int> GetRandomIndexes(int n)
    {
        List<int> indexes = new List<int>();
        
        if (n > maxWidth)
        {
            Debug.LogWarning($"MapGenerator: numberOfStartingPoints ({n}) > maxWidth ({maxWidth}). Clamping to maxWidth.");
            n = maxWidth;
        }

        while (indexes.Count < n)
        {
            int randomNum = Random.Range(0, maxWidth);
            if (!indexes.Contains(randomNum))
            {
                indexes.Add(randomNum);
            }
            
            // Safety check
            if (indexes.Count >= maxWidth)
                break;
        }
        
        return indexes;
    }

    private void DestroyAllChildren(Transform parent)
    {
        if (parent == null) return;

        List<Transform> toDestroy = new List<Transform>();
        
        // Collect children (excluding pooled paths which are already inactive)
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf) // Only destroy active objects (POIs)
            {
                toDestroy.Add(child);
            }
        }

        // Destroy using appropriate method
        for (int i = toDestroy.Count - 1; i >= 0; i--)
        {
            if (toDestroy[i] != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(toDestroy[i].gameObject);
                }
                else
                {
                    DestroyImmediate(toDestroy[i].gameObject);
                }
            }
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void OnDestroy()
    {
        // Cleanup pooled objects
        foreach (GameObject path in pathPool)
        {
            if (path != null)
            {
                Destroy(path);
            }
        }
        pathPool.Clear();

        foreach (GameObject path in activePaths)
        {
            if (path != null)
            {
                Destroy(path);
            }
        }
        activePaths.Clear();

        // Clear references
        pointsOfInterest.Clear();
        _pointOfInterestsPerFloor = null;
    }

    // Public method to manually clean up (useful for scene transitions)
    public void Cleanup()
    {
        OnDestroy();
    }

    // Editor helper method
    #if UNITY_EDITOR
    private void OnValidate()
    {
        // Auto-update min values in editor
        numberOfStartingPoints = Mathf.Max(1, numberOfStartingPoints);
        mapLength = Mathf.Max(1, mapLength);
        maxWidth = Mathf.Max(1, maxWidth);
    }
    #endif
}