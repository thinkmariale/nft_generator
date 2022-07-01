using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObj : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Renderer objRenderer;

    private Color _color;
    private float _speed;
    private Vector2 _maxPosition;
    private Vector2 _minPosition;
    private bool _init = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveObj();
    }

    void MoveObj() {
        if(!_init) {return;}
        transform.position = Vector3.MoveTowards(transform.position, _maxPosition, Time.deltaTime * _speed);
        if(transform.position.x <= _maxPosition.x && transform.position.y >= _maxPosition.y) {
            transform.position = _minPosition;
        }
    }

    public void Init(Color c, float s, Vector2 max, Vector2 min) {

        Debug.Log("color");
        _color = c;
        _speed = s;
        if(spriteRend != null)
            spriteRend.material.color = _color;
        if(objRenderer != null)
            objRenderer.material.SetColor("_color", _color);
        _maxPosition = max;
        _minPosition = min;
     
        _init = true;
    }

   
}
