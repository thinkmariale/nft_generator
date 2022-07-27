using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyAttributes : RarityAttributes
{
    // FIRST
    public List<WeightedValue> backgrounds;
    public Color []backgroundsColor;
    public Texture2D []backgroundsTextures;
    // SECOND
    public List<WeightedValue> jellyFeatColors;
    public List< ColorGroups >jellyColors;

    // THIRD
    public List<WeightedValue> jellyFeatSizes;
    public List<RangeGroupsFlt> jellySizes;
    
    //FORTH
    public List<WeightedValue> jellyNum;
    public List<RangeGroupsInt> jellyRange;

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
        weightedValues.Add("jellyFeatColors", jellyFeatColors);

        // 3rd attribute backgrounds
        weightedValues.Add("jellyFeatSizes", jellyFeatSizes);
    }
}
