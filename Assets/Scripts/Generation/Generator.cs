using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Generator : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private int chunckSize = 128;
    [SerializeField] private int noiseMapSeed = 6969;

    [Header("Stars Variables")]
    [SerializeField] Material starsMat;
    [SerializeField] Mesh starsMesh;
    [SerializeField] GameObject _universeCenter;
    #endregion
    #region Class-Only Variables
    static private Vector3 __universeCenter;
    static private float _expansionRate = 0.0001f;
    public static List<CelestialObject> spawnedObjects = new List<CelestialObject>();
    #endregion
    #region Getters & Setters
    public static float GetExpansionRate { get { return _expansionRate; }}
    public static Vector3 GetCenter { get { return __universeCenter; } }

    public int NoiseMapChunckSize
    {
        get { return chunckSize; }
        set { chunckSize = value; }
    }
    public int NoiseMapSeed
    {
        get { return noiseMapSeed; }
        set { noiseMapSeed = value; }
    }

    public static List<CelestialObject> CelestialObjects { get { return spawnedObjects; } }

    public static void AddCelestialObject(CelestialObject c) { spawnedObjects.Add(c); }
    #endregion

    public void CleanUp() 
    {
        // Clean Up
        foreach (CelestialObject s in spawnedObjects)
        {
            s.Die();
        }

        spawnedObjects.Clear();
    }

    public void GenerateStars(float[,] noiseMap) 
    {
        // Determine the Center of the universe
        __universeCenter = new Vector3(chunckSize / 2, chunckSize / 2, chunckSize / 2);
        _universeCenter.transform.position = __universeCenter;

        // Delete any previously generated stars (will not be useful outside the editor)
        CleanUp();

        Random.InitState(noiseMapSeed);
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        int _currentId = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (noiseMap[x, y] == 1) 
                {
                    // GameObject _star = Instantiate(StarPrefab, new Vector3(x, 0, y), Quaternion.identity);
                    uint nSeed = (uint)((y) << 16 | (x)) * (uint)(noiseMapSeed);
                    // _star.GetComponent<Star>().SetupObj(/*this,*/ randSizeGen.GetNextInt(nSeed, 1, 4), (float)randHeightGen.GetNextDouble(0.0d, 1.0d) * thickness, (float)randMassGen.GetNextDouble(Mathf.Pow(1.61999999919f, 29f), 2 * Mathf.Pow(10, 31)));
                    StarObject _star =  new StarObject(_currentId,
                        new Vector3(x, 0, y),
                        Random.Range(0.635517899f, 8.90056178f),
                        Random.Range(18.1934054f, 264.575131f),
                        Random.Range(0.0f, 1.0f) * chunckSize,
                        Random.Range(10f, 60f),
                        starsMat, starsMesh);

                    spawnedObjects.Add(_star);
                    _currentId++;
                }
            }
        }

        GetComponent<AstroPhysics>().Setup();
    }

    private void Awake()
    {
        float[,] map = GetNoiseMap();
        GenerateStars(map);
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    public static T SafeDestroy<T>(T obj) where T : Object
    {
        if (Application.isEditor)
            Object.DestroyImmediate(obj);
        else
            Object.Destroy(obj);

        return null;
    }
    public static T SafeDestroyGameObject<T>(T component) where T : Component
    {
        if (component != null)
            SafeDestroy(component.gameObject);
        return null;
    }

    public float[,] GetNoiseMap() 
    {
        return NoiseMap.GenerateNoiseMap((uint)chunckSize, (uint)noiseMapSeed);
    }
}