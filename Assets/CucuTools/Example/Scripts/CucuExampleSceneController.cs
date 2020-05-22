using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CucuTools.Example
{
    public class CucuExampleSceneController : MonoBehaviour
    {
#pragma warning disable 0649

        [Space] [Header("Camera")] [SerializeField]
        Camera _camera;

        [SerializeField] private CucuRangeFloat _radiusRotation = new CucuRangeFloat(0f, 10f, 5f);
        [SerializeField] private CucuRangeFloat _periodRotation = new CucuRangeFloat(0f, 120f, 60f);

        [Space] [Header("Generation")] [SerializeField]
        private Transform _centerGeneration;

        [SerializeField] private GameObject _objectGeneration;
        [SerializeField] private CucuRangeFloat _radiusGeneration = new CucuRangeFloat(0f, 2f, 1f);

        [Space] [Header("Transformation")] [SerializeField]
        private GameObject _cubeTransformation;

        [SerializeField] private GameObject _sphereTransformation;
        [SerializeField, Range(0f, 5f)] private float _durationTransformation = 1f;
        [SerializeField] private CucuColor.ColorMap _colorMapTransformation;

        [Space] [Header("General")] [SerializeField, Range(1, 2000)]
        private int _countMaxObjects = 500;

        [SerializeField, Range(0f, 60f)] private float _periodChangeColor = 10f;

        [Space] [Header("Info")] [SerializeField, Range(0, 2000)]
        private int _countObjects = 0;



        private CucuBlendColorGradient blendColor
        {
            get
            {
                if (_blendColorHash != null) return _blendColorHash;

                _blendColorHash = gameObject.GetComponent<CucuBlendColorGradient>();

                if (_blendColorHash != null) return _blendColorHash;

                _blendColorHash = gameObject.AddComponent<CucuBlendColorGradient>();
                _blendColorHash.SetGradient(CucuColor.PaletteMaps[_colorMapTransformation].ToGradient());

                return _blendColorHash;
            }
        }

        private CucuBlendColorGradient _blendColorHash;

        private List<Transform> _objects = new List<Transform>();
        private Color _colorTransformation;
        private CucuTimer _timerColorChanger;
        private CucuTimer _timerCameraRotation;
#pragma warning disable 0414
        private readonly string _COLOR = "_Color";
#pragma warning restore 0414
        private readonly string _EMISSIONCOLOR = "_EmissionColor";
        private readonly string _OUTLINECOLOR = "_OutlineColor";

#pragma warning restore 0649
        
        private void Awake()
        {
            Cucu.Log("Welcome to " + "CUCU WORLD".ToColoredString("3caa3c") + "!", "cucurbitacine");
        }

        private void Start()
        {
            InitTriggers();

            InitTimers();
        }

        private void Update()
        {
            var t = _timerCameraRotation.TimeLocal / _timerCameraRotation.Duration;

            var phi = t * 2 * Mathf.PI;
            var radius = _radiusRotation.Value;
            var x = radius * Mathf.Cos(phi);
            var z = radius * Mathf.Sin(phi);

            _camera.transform.position = new Vector3(x, _camera.transform.position.y, z);
            _camera.transform.LookAt(new Vector3(0f, _camera.transform.position.y, 0f));

            CreateObject();
            UpdateColor();
        }

        private void OnValidate()
        {
            _timerColorChanger?.SetDuration(_periodChangeColor);
            _timerCameraRotation?.SetDuration(_periodRotation.Value);
            UpdateGradient();
            _radiusGeneration.UpdateValue();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = CucuColor.Palettes.Jet.Get(1f * _objects.Count / _countMaxObjects).SetColorAlpha(0.2f);
            Gizmos.DrawSphere(_centerGeneration.position, _radiusGeneration.Value);
        }

        private void InitTimers()
        {
            _timerColorChanger = CucuTimerFactory.Create("colorValue");
            _timerColorChanger
                .SetDuration(_periodChangeColor)
                .OnStop(() => _timerColorChanger.StartTimer())
                .StartTimer();

            _timerCameraRotation = CucuTimerFactory.Create("cameraRotation");
            _timerCameraRotation
                .SetDuration(60f)
                .OnStop(() => _timerCameraRotation.StartTimer())
                .StartTimer();
        }

        private void InitTriggers()
        {
            var tags = CucuTag.Tags;

            var tagsTransformation = tags.FindAll(t => t.Key == "transformation");

            var cubeTrigger = tagsTransformation.SelectWithArg("type", "cube").First().GetComponent<CucuTrigger>();

            cubeTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Enter)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;
                    cucuTag.GetComponentInChildren<Renderer>()
                        .SetColorUsePropertyBlock(cucuTag.Key.Equals("cube") ? _EMISSIONCOLOR : _OUTLINECOLOR,
                            _colorTransformation);
                });

            cubeTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Exit)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;
                    if (cucuTag.Key.Equals("cube"))
                    {
                        TransformationObject(cucuTag.transform, _cubeTransformation);
                    }
                    else
                    {
                        var rend = cucuTag.GetComponentInChildren<Renderer>();
                        var colorTarget = Color.black.SetColorAlpha(0f);
                        var timer = CucuTimerFactory.Create();
                        timer
                            .SetDuration(_durationTransformation)
                            .SetTick(1f / 24)
                            .OnTick(() =>
                            {
                                var colorInit = _colorTransformation.LerpTo(colorTarget,
                                    Mathf.Clamp01(timer.TimeLocal / timer.Duration));
                                rend.SetColorUsePropertyBlock(_OUTLINECOLOR, colorInit);
                            })
                            .OnStop(() => rend.SetColorUsePropertyBlock(_OUTLINECOLOR, colorTarget))
                            .DestroyAfterStop()
                            .StartTimer();
                    }
                });

            var sphereTrigger = tagsTransformation
                .SelectWithArg("type", "sphere")
                .FirstOrDefault()
                ?.GetComponent<CucuTrigger>();

            sphereTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Enter)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;
                    cucuTag.GetComponentInChildren<Renderer>()
                        .SetColorUsePropertyBlock(cucuTag.Key.Equals("sphere") ? _EMISSIONCOLOR : _OUTLINECOLOR,
                            _colorTransformation);
                });

            sphereTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Exit)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;

                    if (cucuTag.Key.Equals("sphere"))
                    {
                        TransformationObject(cucuTag.transform, _sphereTransformation);
                    }
                    else
                    {
                        var rend = cucuTag.GetComponentInChildren<Renderer>();
                        var colorTarget = Color.black.SetColorAlpha(0f);
                        var timer = CucuTimerFactory.Create();
                        timer
                            .SetDuration(_durationTransformation)
                            .SetTick(1f / 24)
                            .OnTick(() =>
                            {
                                var colorInit = _colorTransformation.LerpTo(colorTarget,
                                    Mathf.Clamp01(timer.TimeLocal / timer.Duration));
                                rend.SetColorUsePropertyBlock(_OUTLINECOLOR, colorInit);
                            })
                            .OnStop(() => rend.SetColorUsePropertyBlock(_OUTLINECOLOR, colorTarget))
                            .DestroyAfterStop()
                            .StartTimer();
                    }
                });
        }

        private void CreateObject()
        {
            var obj = Instantiate(_objectGeneration,
                _centerGeneration.position + Random.onUnitSphere * _radiusGeneration.Value, Random.rotation,
                _centerGeneration);

            obj.transform.localScale *= (Random.value - 0.5f) * 0.333f + 2f;
            obj.name = $"{_objects.Count}_{obj.name}";

            obj.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * 1000);
            obj.AddCucuTag(Random.value < 0.5f ? "cube" : "sphere");

            _objects.Add(obj.transform);

            ClearObjects();

            _countObjects = _objects.Count;
        }

        private void ClearObjects()
        {
            while (_objects.Count > _countMaxObjects)
                DeleteObject();
        }

        private void DeleteObject()
        {
            if (_objects.Count > 0)
            {
                var cucu = _objects[0];
                _objects.RemoveAt(0);
                if (cucu != null)
                    Destroy(cucu.gameObject);
            }
        }

        private void TransformationObject(Transform origin, GameObject target)
        {
            var index = _objects.IndexOf(origin);

            if (!(0 <= index && index < _objects.Count))
            {
                Destroy(origin.gameObject);
                return;
            }

            var obj = Instantiate(target, origin.position, origin.rotation);
            _objects[index] = obj.transform;

            obj.transform.parent = origin.parent;

            var rigOrigin = origin.gameObject.GetComponent<Rigidbody>();
            var rigTarget = obj.GetComponent<Rigidbody>();

            if (rigOrigin != null && rigTarget != null)
            {
                rigTarget.velocity = rigOrigin.velocity;
                rigTarget.angularVelocity = rigOrigin.angularVelocity;
            }

            Destroy(origin.gameObject);

            var timer = CucuTimerFactory.Create();

            var rend = obj.gameObject.GetComponent<Renderer>();

            timer
                .SetDuration(_durationTransformation)
                .SetTick(1f / 24)
                .OnStart(() => rend.SetColorUsePropertyBlock(_EMISSIONCOLOR, _colorTransformation))
                .OnTick(() =>
                {
                    var t = Mathf.Clamp01(timer.TimeLocal / timer.Duration);
                    var colorInit = _colorTransformation.LerpTo(Color.black, t);
                    rend.SetColorUsePropertyBlock(_EMISSIONCOLOR, colorInit);
                })
                .OnStop(() => rend.SetColorUsePropertyBlock(_EMISSIONCOLOR, Color.black))
                .DestroyAfterStop()
                .StartTimer();
        }

        private void UpdateColor()
        {
            var t = Mathf.Clamp01(_timerColorChanger.Duration > 0
                ? _timerColorChanger.TimeLocal / _timerColorChanger.Duration
                : 0f);

            t = 2 * (t < 0.5f ? t : (1 - t));

            blendColor.SetBlend(t);

            _colorTransformation = blendColor.Color;
        }

        private void UpdateGradient()
        {
            blendColor.SetGradient(CucuColor.PaletteMaps[_colorMapTransformation].ToGradient());
        }
    }
}