using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    class PoolObject{
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t){
            transform = t;
        }
        public void Use(){
            inUse = true;
        }
        public void Dispose(){
            inUse = false;
        }

    }

    [System.Serializable]
    public struct YSpawnRange{
        public float min;
        public float max;
    }

    public GameObject prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;
    public Vector2 defaultSpwanPos;
    public bool spawnImmediate;
    public Vector2 immediateSpawnPos;
    public Vector2 targetAspectRatio;
    public YSpawnRange ySpawnRange;
    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    GameManager gameManager;

    private void Awake() {
        Configure();
    }

    private void Start() {
        gameManager = GameManager.Instance;
    }

    private void OnEnable() {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable() {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;

    }

    void OnGameOverConfirmed(){
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        Configure();
    }

    private void Update() {
        if (gameManager.GameOver){
            return;
        }
        Shift(); 
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnRate){
            Spwan();
            spawnTimer = 0;
        }
    }

    void Configure(){
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for (int i = 0; i < poolObjects.Length; i++)
        {
            Transform t = Instantiate(prefab).transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t); 
        }
        if (spawnImmediate){
            SpawnImmediate();
        }
    }

    void Spwan(){
        Transform t = GetPoolObject();
        if (t == null){
            return;
        }
        Vector3 pos = Vector3.zero;
        pos.x = (defaultSpwanPos.x * Camera.main.aspect) / targetAspect;
        pos.y = Random.Range(ySpawnRange.min,ySpawnRange.max);
        t.position = pos;
    }

    void SpawnImmediate(){
        Transform t = GetPoolObject();
        if (t == null){
            return;
        }
        Vector3 pos = Vector3.zero;
        pos.x = (immediateSpawnPos.x * Camera.main.aspect) / targetAspect;
        pos.y = Random.Range(ySpawnRange.min,ySpawnRange.max);
        t.position = pos;
        Spwan();
    }

    void Shift(){
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.localPosition += -Vector3.right * shiftSpeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }
    }

    void CheckDisposeObject(PoolObject obj){
        if (obj.transform.position.x < (-defaultSpwanPos.x * Camera.main.aspect) / targetAspect){
            obj.Dispose();
            obj.transform.position = Vector3.one * 1000;
        }
    }

    Transform GetPoolObject(){
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse){
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
