using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MouseController : MonoBehaviour
{
    private GameObject _clickedObject;
    private GameObject _releasedObject;
    private LineRenderer _lineRenderer;
    private bool _isDrawing = false;
    
    public SignalConnectorBase signalRConnector;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2; // A single line requires 2 points
        _lineRenderer.startWidth = 0.5f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.enabled = false; // Initially disable the line
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var r = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (r.Length == 0) return;
            _clickedObject = r[0].gameObject;
            var planet1 = _clickedObject.GetComponent<Planet>();
            if (planet1.ownerPlayer.id != UserData.Id)
            {
                _clickedObject = null;
                return;
            }
            _isDrawing = true;
            
            var startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 0; // Set z to 0 since we're in 2D
            _lineRenderer.SetPosition(0, startPoint); // Set start point
            _lineRenderer.SetPosition(1, startPoint); // Initially set it to the same point
            
            if (planet1)
            {
                _lineRenderer.startWidth = planet1.size / 2;
            }
            _lineRenderer.startColor = _clickedObject.GetComponent<SpriteRenderer>().color;
            _lineRenderer.endColor = _clickedObject.GetComponent<SpriteRenderer>().color;
            _lineRenderer.enabled = true; // Enable the line
        }
        
        if (Input.GetMouseButton(0) && _isDrawing)
        {
            var currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 0; // Keep z at 0
            _lineRenderer.SetPosition(1, currentPoint); // Update the line's end point
            if (_releasedObject)
            {
                _lineRenderer.endColor = _releasedObject.GetComponent<SpriteRenderer>().color;
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _isDrawing = false;
            _lineRenderer.enabled = false; // Hide the line
            var r = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (r.Length == 0)
            {
                _releasedObject = null;
                _clickedObject = null;
                return;
            };
            _releasedObject = r[0].gameObject;
        }

        if (!_clickedObject || !_releasedObject)
        {
            return;
        }
        
        var planet = _clickedObject.GetComponent<Planet>();
        var planet2 = _releasedObject.GetComponent<Planet>();
        if (planet == planet2 || !planet || !planet2)
        {
            _clickedObject = null;
            _releasedObject = null;            
        }
        else 
        {
            var amount = planet.SpawnShips(_releasedObject.transform);
            signalRConnector.SendShips(planet.id, planet2.id, amount);
        }
        
        _clickedObject = null;
        _releasedObject = null;
    }
}
