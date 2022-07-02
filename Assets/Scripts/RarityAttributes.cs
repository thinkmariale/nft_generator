using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class WeightedValue
{
   public string value;
   public int index;
    public int weight;

    public WeightedValue(string _v, int _w, int i){
        weight = _w;
        value = _v;
        index = i;
    }
}

[Serializable]
public class ColorGroups
{
       public Color []colors;
}

[Serializable]
public class RangeGroupsFlt
{
       public float min;
       public float max;
}

[Serializable]
public class RangeGroupsInt
{
       public int min;
       public int max;
}

public class RarityAttributes : MonoBehaviour
{
    public int collectionSize = 100;
    public string collectionName;

    public int numberAttr = 10;


    protected Dictionary<string, List<WeightedValue> > weightedValues;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void InitWeightArray(){
        weightedValues = new Dictionary<string, List<WeightedValue> >();
    }

    public WeightedValue GetRandomValue(List<WeightedValue> weightedValueList)
    {
        WeightedValue output = null;
 
        //Getting a random weight value
        var totalWeight = 0;
        foreach (var entry in weightedValueList)
        {
            totalWeight += entry.weight;
        }
        var rndWeightValue = UnityEngine.Random.Range(1, totalWeight + 1);
 
        //Checking where random weight value falls
        var processedWeight = 0;
        foreach (var entry in weightedValueList)
        {
            processedWeight += entry.weight;
            if(rndWeightValue <= processedWeight)
            {
                output = entry;
                Debug.Log(output.value);
                break;
            }
        }
 
        return output;
    }
}
