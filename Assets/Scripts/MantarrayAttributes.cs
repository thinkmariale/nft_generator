using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantarrayAttributes : RarityAttributes
{
    // FIRST
    public List<WeightedValue> backgrounds;
    public Color []backgroundsColor;

    // SECOND
    public List<WeightedValue> mantarrayColors;
    public List< ColorGroups >mantaColors;

    // THIRD
    public List<WeightedValue> mantarraySizes;
    public List<RangeGroupsFlt> mantaSizes;
    
    //FORTH
    public List<WeightedValue> mantarrayNum;
    public List<RangeGroupsInt> mantaRange;

    // feat gen:
    public RangeGroupsInt featRange;
    public RangeGroupsFlt featSizes;


    // Start is called before the first frame update
    void Start()
    {
        InitWeightArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void InitWeightArray() {
        weightedValues = new Dictionary<string, List<WeightedValue> >();
        // 1st attribute backgrounds
        weightedValues.Add("backgrounds", backgrounds);

        // 2nd attribute backgrounds
        weightedValues.Add("mantarrayColors", mantarrayColors);

        // 3rd attribute backgrounds
        weightedValues.Add("mantarraySizes", mantarraySizes);
    }
}
