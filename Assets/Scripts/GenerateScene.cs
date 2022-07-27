using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AnglePos
{
   public Vector3 position;
   public float angle; 
    public AnglePos(Vector3 pos, float ang) {
        position = pos;
        angle = ang;
    }
}

public class GenerateScene : MonoBehaviour
{
    public GameObject testbALL;
    public Camera mainCamera;
    public GameObject prefabObj;
    public GameObject backgrounds;
    public GameObject []prefabFeatues;

    public Color []colors;
    public Texture []texturesFeatures;
    public Color [] colorFeatures;
    public MantarrayAttributes rAttrb; // mantarray
    public JellyAttributes jAttrb;   /// jelly fish

    public RecordingController recorder;
    public bool isRecording = false;
    private int _numRecorded = 0;
    // scene positioning variables
    [SerializeField] private int _maxWidth = 10;
    [SerializeField] private int _maxHeight = 10;
    [SerializeField] private int _maxObjGen = 10;
    [SerializeField] private int _minFeatGen;
    [SerializeField] private int _maxFeatGen;

    [SerializeField] private float _maxObjWidth = 1;
    [SerializeField] private float _minObjWidth = .1f;

    [SerializeField] private float _minSpeed = 0.1f;
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _maxSmallSpeed = 0.3f;
    
    // Start is called before the first frame update
    private List<MainObj> _listObjs;
    private List<GameObject> _listFeatures;
    private int collectionSize = 0;
    void Start()
    {
        _listObjs = new List<MainObj>();
        _listFeatures = new List<GameObject>();

        if(rAttrb != null)
            GenerateSceneWithRarityMantarray();
        if(jAttrb != null)
            GenerateSceneWithRarityJellyFish();
        //GenerateObjs();
    }

    void Reset() {
        foreach(MainObj o in _listObjs) {
            Destroy(o.gameObject);
        }
         foreach(GameObject o in _listFeatures) {
            Destroy(o);
        }

        _listObjs.Clear();
        _listFeatures.Clear();
    }

    void GenerateSceneWithRarityMantarray(){
        collectionSize = rAttrb.collectionSize;
        // background layer
        WeightedValue bg = rAttrb.GetRandomValue(rAttrb.backgrounds);
        backgrounds.GetComponent<Renderer>().material.SetColor("_color", rAttrb.backgroundsColor[bg.index]);

        // obj layer
        WeightedValue amount = rAttrb.GetRandomValue(rAttrb.mantarrayNum);
        WeightedValue size = rAttrb.GetRandomValue(rAttrb.mantarraySizes);
        WeightedValue mColors = rAttrb.GetRandomValue(rAttrb.mantarrayColors);
       
        int num = UnityEngine.Random.Range(rAttrb.mantaRange[amount.index].min, rAttrb.mantaRange[amount.index].max);

        GenerateObjsHelper(num, rAttrb.mantaSizes[size.index].min, rAttrb.mantaSizes[size.index].max, rAttrb.mantaColors[mColors.index].colors, rAttrb.featRange, rAttrb.featSizes);

        if(isRecording) {
            StartCoroutine(RecordScene());
        }
    }

     void GenerateSceneWithRarityJellyFish(){
          collectionSize = jAttrb.collectionSize;
        // background layer
        WeightedValue bg = jAttrb.GetRandomValue(jAttrb.backgrounds);
       // backgrounds.GetComponent<Renderer>().material.SetTexture("_texture2D", jAttrb.backgroundsTextures[bg.index]);
        backgrounds.GetComponent<Renderer>().material.color = jAttrb.backgroundsColor[bg.index];

        // obj layer
        WeightedValue amount = jAttrb.GetRandomValue(jAttrb.jellyNum);
        WeightedValue size = jAttrb.GetRandomValue(jAttrb.jellyFeatSizes);
        WeightedValue mColors = jAttrb.GetRandomValue(jAttrb.jellyFeatColors);
       
        int num = UnityEngine.Random.Range(jAttrb.jellyRange[amount.index].min, jAttrb.jellyRange[amount.index].max);

        GenerateObjsHelper(num, jAttrb.jellySizes[size.index].min, jAttrb.jellySizes[size.index].max, jAttrb.jellyColors[mColors.index].colors, jAttrb.featRange, jAttrb.featSizes);

        if(isRecording) {
            StartCoroutine(RecordScene());
        }
    }

    //  private void Update() {
    //         int disR = 10; 
    //         int disR1 = 10;
    //         Vector3 posOnScreenPos = mainCamera.WorldToScreenPoint(testbALL.transform.position);
    //         if (posOnScreenPos.x < 0 || posOnScreenPos.x > Screen.width || posOnScreenPos.y < 0 || posOnScreenPos.y > Screen.height) {
    //             Debug.Log("Outside!");
    //         } else {
    //             float disX = (Screen.width - posOnScreenPos.x)/Screen.width;
    //             float disY = (Screen.height - posOnScreenPos.y) / Screen.height;

    //             Debug.Log(disX + " " + disY);
    //             if(disX >0.7f|| disY > 0.7f || disX < 0.3f || disY < 0.3f) {
    //                 disR = 3;
    //                 disR1 = 10;
    //             }
    //         }
    // }
    void objectsCircle(float radius = 10f) {
          
            int amountToSpawn = 6;
            for (int i = 0; i < amountToSpawn; i++)
            {
                float angle = Mathf.PI*2f / amountToSpawn *i;
                Vector3 newPos = new Vector3(Mathf.Cos(angle)*radius, Mathf.Sin(angle)*radius,0);
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = newPos;
        }
    }

    AnglePos RndPosCircle(float z, float radius = 10f, int amountToSpawn=100) {
          
        int r = UnityEngine.Random.Range(0,amountToSpawn);
        float angle = Mathf.PI*2f / amountToSpawn * r;
        Vector3 newPos = new Vector3(Mathf.Cos(angle)*radius, Mathf.Sin(angle)*radius,z);
        // GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // go.transform.position = newPos;
        
        float angleDed = angle * (180f/Mathf.PI) + 50;
        return new AnglePos(newPos, angleDed);
    }

    void GenerateObjsHelper(int amount, float minSize, float maxSize, Color [] mColors, RangeGroupsInt featRange, RangeGroupsFlt featSize) {

        int swimType = UnityEngine.Random.Range(0,3);
        float z = 0;
        for(int i=0; i < amount; i++){
            float x = UnityEngine.Random.Range(-1 * _maxWidth, _maxWidth);
            float y = UnityEngine.Random.Range(-1 * _maxHeight, _maxHeight);
            float size = UnityEngine.Random.Range(minSize, maxSize);

            GameObject obj = Instantiate(prefabObj, new Vector3(x,y,z), prefabObj.transform.rotation);
            obj.transform.localScale = new Vector3(size, size,size);

            float s = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
            int c =  UnityEngine.Random.Range(0, mColors.Length);
           // Debug.Log("speed " + s);
            MainObj mobj = obj.GetComponent<MainObj>();
           
            var swimTo = obj.transform.position + obj.transform.right;
            var swimBack = obj.transform.position -obj.transform.right;
            // random dirrections
            if(swimType == 0) {
                AnglePos spawnDir = RndPosCircle(z);
                float angle = spawnDir.angle;//Mathf.Atan2(spawnPosMt.y, spawnPosMt.x) * Mathf.Rad2Deg;
                obj.transform.rotation = Quaternion.Euler(new Vector3(angle, 90,90));
                swimTo = obj.transform.position + obj.transform.right * 12;
                swimBack = obj.transform.position -obj.transform.right * 12;
            
            } else {
                // same direction
                Vector3 dir = new Vector3(-1, 1,z);
                swimTo = obj.transform.position + (dir * 1.7f* _maxWidth);
                swimBack = obj.transform.position - (dir * 1.7f* _maxWidth);
            }
       
            // GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // go.transform.position = swimTo;

            //  GameObject go1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // go1.transform.position = swimBack;

            mobj.Init(mColors[c], s, swimTo, swimBack, mainCamera);
            _listObjs.Add(mobj);
            z += 0.3f;
        }

        // Feature
        int featAmoount = UnityEngine.Random.Range(featRange.min, featRange.max);
        for(int f = 0; f < prefabFeatues.Length; f++) {
            for(int i=0;i < featAmoount; i++) {
                Vector2  pos = UnityEngine.Random.insideUnitCircle * _maxHeight * 1.5f;
                float size = UnityEngine.Random.Range(featSize.min, featSize.max);

                GameObject obj = Instantiate(prefabFeatues[f],new Vector3(pos.x,pos.y,3), prefabFeatues[f].transform.rotation);
                obj.transform.localScale = new Vector3(size, size,size);
              
                 int c =  UnityEngine.Random.Range(0, texturesFeatures.Length);
                 obj.GetComponent<Renderer>().material.SetTexture("_texture2D", texturesFeatures[c]);

                if(colorFeatures.Length > 0) {
                    int cl = UnityEngine.Random.Range(0, colorFeatures.Length);
                    obj.GetComponent<Renderer>().material.SetColor("_color", colorFeatures[cl]);
                }
                _listFeatures.Add(obj);
            }
        }
    }

    void GenerateObjs() {

        int num = UnityEngine.Random.Range(1, _maxObjGen);

        Vector3 dir = new Vector3(-1,1,0);
        for(int i=0;i< num;i++){
            float x = UnityEngine.Random.Range(-1 *_maxWidth,_maxWidth);
            float y = UnityEngine.Random.Range(-1 *_maxHeight,_maxHeight);
            float z = UnityEngine.Random.Range(0, 1);
            float size = UnityEngine.Random.Range(_minObjWidth,_maxObjWidth);

            GameObject obj = Instantiate(prefabObj,new Vector3(x,y,z), prefabObj.transform.rotation);
            obj.transform.localScale = new Vector3(size, size,size);
            
            float s = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
            if(size < 0.5)
                s =  UnityEngine.Random.Range(_minSpeed, _maxSmallSpeed);
     
            int c =  UnityEngine.Random.Range(0, colors.Length);
            MainObj mobj = obj.GetComponent<MainObj>();
            Vector3 posMax = obj.transform.position + (dir * 1.7f *_maxWidth);
            Vector3 posMin = obj.transform.position - (dir * 1.7f *_maxWidth);
           // Debug.Log("speed " + s);
            mobj.Init(colors[c], s, posMax, posMin,mainCamera);
            _listObjs.Add(mobj);
        }

        // Feature
        num = UnityEngine.Random.Range(rAttrb.featRange.min, rAttrb.featRange.max);
        for(int f = 0; f < prefabFeatues.Length; f++) {
            for(int i=0;i< num;i++) {
                Vector2  pos = UnityEngine.Random.insideUnitCircle * _maxHeight * 1.5f;
                float size = UnityEngine.Random.Range(rAttrb.featSizes.min, rAttrb.featSizes.max);

                GameObject obj = Instantiate(prefabFeatues[f],new Vector3(pos.x,pos.y,3), prefabFeatues[f].transform.rotation);
                obj.transform.localScale = new Vector3(size, size,size);
                int c =  UnityEngine.Random.Range(0, texturesFeatures.Length);
                obj.GetComponent<Renderer>().material.SetTexture("_texture2D", texturesFeatures[c]);
               
                _listFeatures.Add(obj);
            }
        }
    }
   
   IEnumerator RecordScene() {
     Debug.Log("Recording: " + _numRecorded);
       yield return new WaitForSeconds(0.2f);
        recorder.StartRecording();
        yield return new WaitForSeconds(30);
        recorder.StopRecording();
        yield return new WaitForSeconds(1);
        Reset();
        yield return new WaitForSeconds(1);
        _numRecorded++;
       
        if(_numRecorded < collectionSize) {
            if(rAttrb != null)
                GenerateSceneWithRarityMantarray();
            if(jAttrb != null)
                GenerateSceneWithRarityJellyFish();
            }
   }

}
