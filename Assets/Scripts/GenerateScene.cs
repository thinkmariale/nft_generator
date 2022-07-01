 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateScene : MonoBehaviour
{
    public GameObject gidParent;
    public GameObject prefabObj;
    public GameObject []prefabFeatues;
    public GameObject []prefabBg;
    public float [] weightFeatures;
    public float [] weightBg;
    public Color []colors;

    public Texture []texturesFeatures;
    [SerializeField] private int _maxWidth = 10;
    [SerializeField] private int _maxHeight = 10;
    [SerializeField] private int _maxObjGen = 10;
    [SerializeField] private int _minFeatGen;
    [SerializeField] private int _maxFeatGen;

    [SerializeField] private float _maxObjWidth = 1;
    [SerializeField] private float _minObjWidth = .1f;

    [SerializeField] private float _maxFeatWidth = 1;
    [SerializeField] private float _minFeatWidth = .1f;


    [SerializeField] private float _minSpeed = 0.1f;
    [SerializeField] private float _maxSpeed = 1f;

     // [SerializeField] private int _maxObjGen;
    // Start is called before the first frame update
    private List<MainObj> _listObjs;
    private List<GameObject> _listFeatures;

    void Start()
    {
        _listObjs = new List<MainObj>();
        _listFeatures = new List<GameObject>();

        GenerateObjs();
    }

    void GenerateObjs() {

        int num = Random.Range(1, _maxObjGen);

        Vector3 dir = new Vector3(-1,1,0);
        for(int i=0;i< num;i++){
            float x = Random.Range(-1 *_maxWidth,_maxWidth);
            float y = Random.Range(-1 *_maxHeight,_maxHeight);
            float z = Random.Range(0, 1);
            float size = Random.Range(_minObjWidth,_maxObjWidth);

            GameObject obj = Instantiate(prefabObj,new Vector3(x,y,z), prefabObj.transform.rotation);
            obj.transform.localScale = new Vector3(size, size,size);
            float s = Random.Range(_minSpeed, _maxSpeed);
            int c =  Random.Range(0, colors.Length);
            MainObj mobj = obj.GetComponent<MainObj>();
            Vector3 posMax = obj.transform.position + (dir * 1.7f *_maxWidth);
            Vector3 posMin = obj.transform.position - (dir * 1.7f *_maxWidth);
            Debug.Log("speed " + s);
            mobj.Init(colors[c], s, posMax, posMin);
            _listObjs.Add(mobj);
        }

        // Feature
        num = Random.Range(_minFeatGen, _maxFeatGen);
        for(int f = 0; f < prefabFeatues.Length; f++) {
            for(int i=0;i< num;i++) {
                Vector2  pos = Random.insideUnitCircle * _maxHeight * 1.5f;
                float size = Random.Range(_minFeatWidth,_maxFeatWidth);

                GameObject obj = Instantiate(prefabFeatues[f],new Vector3(pos.x,pos.y,3), prefabFeatues[f].transform.rotation,gidParent.transform);
                obj.transform.localScale = new Vector3(size, size,size);
                 int c =  Random.Range(0, texturesFeatures.Length);
                 obj.GetComponent<Renderer>().material.SetTexture("_texture2D", texturesFeatures[c]);
               
                _listFeatures.Add(obj);
            }
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        // Vector3 dir = new Vector3(-1,1,0);
        // foreach(MainObj m in _listObjs) {
        //     Vector3 pos = m.transform.position + (dir * 1.7f *_maxWidth);
        //     Debug.DrawLine (pos, pos + dir * 10, Color.red, Mathf.Infinity);
        // }
    }
}
