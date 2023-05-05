using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LaserGameElement : GameElement
{
    public static Action UpdateLasers = delegate { };

    private const float MIN_LASER_DISTANCE = 25;

    [Header("Laser Vars")]
    [SerializeField] private LaserComponent laserPrefab;
    [SerializeField] private Transform laserContainer;

    private List<LaserComponent> _spawnedLasers = new List<LaserComponent>();
    private RaycastHit _laserHit;
    private List<NormalPos> _laserHits = new List<NormalPos>();
    private GameElementsData.ColorType _rayHitsColor;

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
        _rayHitsColor = GameElementsData.ColorType.White;
        LaserCast(laserContainer.position, laserContainer.right);

        if (GameManager.Instance.IsInEditor == ScreensData.ScreenType.PlayMenu)
        {
            CreateLasers();

            GameManager.Instance.CheckGoalsReached();
        }
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
                    Vector3 correctHitPoint = _laserHit.point + reflectedDir * 0.1f;
                    _laserHits.Add(new NormalPos() { pos = correctHitPoint, normal = reflectedDir, hitElement = _gameElementComponent });
                    FilterGameElement filter = _gameElementComponent as FilterGameElement;
                    _rayHitsColor = gameElementsData.GetCombinedColor(_rayHitsColor, filter.FilterColor).type;
                    LaserCast(correctHitPoint, reflectedDir);
                }
                else if(_gameElementComponent is DoorSensorGameElement)
                {
                    DoorSensorGameElement doorSensor = _gameElementComponent as DoorSensorGameElement;
                    if (_laserHits[_laserHits.Count - 1] != null)
                    {
                        doorSensor.SetReceivedColor(_rayHitsColor);
                    }
                    _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = Vector3.zero });
                }
                else
                {
                    _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = Vector3.zero, hitElement = _gameElementComponent });
                }
            }
            else if(_reflectiveComponent != null)
            {
                Vector3 reflectedDir = Vector3.Reflect(dir, _laserHit.normal);
                _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = reflectedDir, hitElement = _gameElementComponent });
                LaserCast(_laserHit.point, reflectedDir);
            }
            /*else
            {
                _laserHits.Add(new NormalPos() { pos = _laserHit.point, normal = Vector3.zero });
            }*/
        }
    }

    private void CreateLasers()
    {
        for (int i = 0;i < _laserHits.Count-1;i++)
        {
            AddLaser(_laserHits[i].pos, _laserHits[i].normal);
            _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * (_laserHits[i+1].pos - _laserHits[i].pos).magnitude);
            if (i > 0)
            {
                _spawnedLasers[_spawnedLasers.Count - 1].SetColor(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, GetColorType(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, _laserHits[i].hitElement));
            }
            else
            {
                _spawnedLasers[_spawnedLasers.Count - 1].SetColor(GameElementsData.ColorType.White, GetColorType(GameElementsData.ColorType.White, _laserHits[i].hitElement));
            }
        }

        if (_laserHits[_laserHits.Count - 1].hitElement != null)
        {
            if (_laserHits[_laserHits.Count - 1].hitElement is FilterGameElement)
            {
                AddLaser(_laserHits[_laserHits.Count - 1].pos, _laserHits[_laserHits.Count - 1].normal);
                _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * MIN_LASER_DISTANCE);
                if (_spawnedLasers.Count > 1)
                {
                    _spawnedLasers[_spawnedLasers.Count - 1].SetColor(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, GetColorType(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, _laserHits[_spawnedLasers.Count - 1].hitElement));
                }
            }
            else if (_laserHits[_laserHits.Count - 1].hitElement.GameElementType == GameElementsData.GameElementType.Mirror)
            {
                AddLaser(_laserHits[_laserHits.Count - 1].pos, _laserHits[_laserHits.Count - 1].normal);
                _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * MIN_LASER_DISTANCE);
                if (_spawnedLasers.Count > 1)
                {
                    _spawnedLasers[_spawnedLasers.Count - 1].SetColor(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, GetColorType(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, _laserHits[_spawnedLasers.Count - 1].hitElement));
                }
            }
        }
        else
        {
            AddLaser(_laserHits[_laserHits.Count - 1].pos, _laserHits[_laserHits.Count - 1].normal);
            _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(1, Vector3.forward * MIN_LASER_DISTANCE);
            if (_spawnedLasers.Count > 1)
            {
                _spawnedLasers[_spawnedLasers.Count - 1].SetColor(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, GetColorType(_spawnedLasers[_spawnedLasers.Count - 2].GetCurrColor().type, _laserHits[_spawnedLasers.Count - 1].hitElement));
            }
        }
    }

    private GameElementsData.ColorType GetColorType(GameElementsData.ColorType lastUsedColor, GameElement hitElement)
    {
        GameElementsData.ColorType color = lastUsedColor;

        if(hitElement != null)
        {
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
        _spawnedLasers.Add(Instantiate(laserPrefab, offsetPosition, Quaternion.LookRotation(dir), transform));
        _spawnedLasers[_spawnedLasers.Count - 1].SetPosition(0, Vector3.zero);
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

public class NormalPos
{
    public Vector3 pos;
    public Vector3 normal;
    public GameElement hitElement;
}
