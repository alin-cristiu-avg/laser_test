using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LaserGameElement : GameElement
{
    public static Action UpdateLasers = delegate { };

    private const float MIN_LASER_DISTANCE = 25;

    [Header("Laser Vars")]
    [SerializeField] private LineRenderer laserPrefab;
    [SerializeField] private Transform laserContainer;

    private List<LineRenderer> _spawnedLasers = new List<LineRenderer>();
    private RaycastHit _laserHit;
    private List<NormalPos> _laserHits = new List<NormalPos>();

    private GameElement _gameElementComponent;
    private ReflectiveMirror _reflectiveComponent;

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        UpdateLasers += SetUpdateLaser;
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        UpdateLasers -= SetUpdateLaser;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void SetUpdateLaser()
    {
        ClearLaser();
        _laserHits.Add(new NormalPos() { pos = laserContainer.position, normal = laserContainer.right , hitElement = null});
        LaserCast(laserContainer.position, laserContainer.right);

        CreateLasers();
    }

    private void LaserCast(Vector3 origin, Vector3 dir)
    {
        Ray laserRay = new Ray(origin, dir);

        Physics.Raycast(laserRay, out _laserHit, MIN_LASER_DISTANCE);
        if (_laserHit.transform != null)
        {
            _gameElementComponent = _laserHit.transform.GetComponent<GameElement>();
            _reflectiveComponent = _laserHit.transform.GetComponent<ReflectiveMirror>();
            if (_gameElementComponent != null)
            {
                if(_gameElementComponent is FilterGameElement)
                {
                    Vector3 reflectedDir = _laserHits[_laserHits.Count - 1].normal;
                    _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = reflectedDir, hitElement = _gameElementComponent });
                    LaserCast(_laserHit.point, reflectedDir);
                }
                else if(_gameElementComponent is DoorSensorGameElement)
                {
                    _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = Vector3.zero });
                    DoorSensorGameElement doorSensor = _gameElementComponent as DoorSensorGameElement;
                    Debug.LogError(doorSensor.IsCorrectColor(Color.white));
                }
                else
                {
                    _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = Vector3.zero });
                }
            }
            else if(_reflectiveComponent != null)
            {
                Vector3 reflectedDir = Vector3.Reflect(dir, _laserHit.normal);
                _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = reflectedDir, hitElement = _gameElementComponent });
                LaserCast(_laserHit.point, reflectedDir);
            }
            else
            {
                _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = Vector3.zero });
            }
        }
    }

    private void CreateLasers()
    {
        Color currColor;
        Color lastColorUsed = gameElementsData.GetColor(GameElementsData.ColorType.White).color;

        for (int i = 0;i < _laserHits.Count-1;i++)
        {
            currColor = gameElementsData.GetColor(GetColorType(_laserHits[i].hitElement)).color;
            AddLaser(_laserHits[i].pos, _laserHits[i].normal);
            _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * (_laserHits[i+1].pos - _laserHits[i].pos).magnitude);
            SetLaserColor(_spawnedLasers[_spawnedLasers.Count - 1], lastColorUsed, currColor);
            lastColorUsed = currColor;
        }

        if (_laserHits[_laserHits.Count - 1].hitElement != null)
        {
            if (_laserHits[_laserHits.Count - 1].hitElement is FilterGameElement)
            {
                AddLaser(_laserHits[_laserHits.Count - 1].pos, _laserHits[_laserHits.Count - 1].normal);
                _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * MIN_LASER_DISTANCE);
            }
            else if (_laserHits[_laserHits.Count - 1].hitElement.GameElementType == GameElementsData.GameElementType.Mirror)
            {
                AddLaser(_laserHits[_laserHits.Count - 1].pos, _laserHits[_laserHits.Count - 1].normal);
                _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * MIN_LASER_DISTANCE);
            }
        }
        else
        {
            AddLaser(_laserHits[_laserHits.Count - 1].pos, _laserHits[_laserHits.Count - 1].normal);
            _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * MIN_LASER_DISTANCE);
        }
    }

    private GameElementsData.ColorType GetColorType(GameElement hitElement)
    {
        GameElementsData.ColorType color = GameElementsData.ColorType.White;

        if(hitElement != null)
        {
            Debug.LogError(hitElement.gameObject.name);

            if (hitElement is FilterGameElement)
            {
                FilterGameElement filter = hitElement as FilterGameElement;
                color = filter.FilterColor;
            }
        }

        return color;
    }

    private void AddLaser(Vector3 offsetPosition, Vector3 dir)
    {
        _spawnedLasers.Add(Instantiate(laserPrefab, offsetPosition, Quaternion.LookRotation(dir)));
        _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(0, Vector3.zero);
    }

    private void SetLaserColor(LineRenderer laser, Color prevColor, Color currColor)
    {
        laser.startColor = currColor;
        laser.endColor = currColor;
    }

    private void ClearLaser()
    {
        if (_spawnedLasers.Count > 0)
        {
            for (int i = 0; i < _spawnedLasers.Count; i++)
            {
                Destroy(_spawnedLasers[i].gameObject);
            }
            _spawnedLasers.Clear();
        }

        if (_laserHits.Count > 0)
        {
            _laserHits.Clear();
        }
    }
}

public struct NormalPos
{
    public Vector3 pos;
    public Vector3 normal;
    public GameElement hitElement;
}
