using System.Collections.Generic;
using CucuTools.Blend.Impl;
using CucuTools.Colors;
using CucuTools.Common;
using CucuTools.Math;
using CucuTools.Tag;
using CucuTools.Timer;
using CucuTools.Trigger;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Examples.Scripts
{
    public class CucuExampleSceneController : MonoBehaviour
    {
        private enum ColorMap
        {
            Rainbow,
            Jet,
            Hot,
        }

        private readonly Dictionary<ColorMap, CucuColorPalette> _colorPaletteMap =
            new Dictionary<ColorMap, CucuColorPalette>()
            {
                {ColorMap.Rainbow, CucuColor.Palettes.Rainbow},
                {ColorMap.Jet, CucuColor.Palettes.Jet},
                {ColorMap.Hot, CucuColor.Palettes.Hot},
            };

        [SerializeField] private ColorMap _colorMap;

        [SerializeField] private GameObject _cubanationZone;

        [SerializeField] private GameObject _spheranationZone;

        [SerializeField] private GameObject _cucuMember;
        [SerializeField] private GameObject _cucuCube;
        [SerializeField] private GameObject _cucuSphere;
        [SerializeField] private Transform _cucuRoot;

        [SerializeField, Range(1, 2000)] private int _countMax = 500;
        [SerializeField, Range(0, 2000)] private int _countCurr = 0;

        private CucuBlendColorGradient cucuBlendColor =>
            _cucuBlendColorHash ?? (_cucuBlendColorHash = GetComponent<CucuBlendColorGradient>());

        private CucuBlendColorGradient _cucuBlendColorHash;
        private List<Transform> _cucus = new List<Transform>();
        private Color _color;
        private CucuTimer _timer;

        [SerializeField] private CucuRangeFloat rangeFloat;
        [SerializeField] private CucuRangeInt rangeInt;

        private void Awake()
        {
            Cucu.Log("Welcome to " + "CUCU WORLD".ToColoredString("3caa3c") + "!", "cucurbitacine");

            InitTriggers();

            cucuBlendColor.SetGradient(CucuColor.Palettes.Jet.ToGradient());
        }

        private void Start()
        {
            _timer = CucuTimerFactory.Create("colorValue");
            _timer
                .SetDuration(10f)
                .OnStop(() => _timer.StartTimer())
                .StartTimer();
        }

        private void Update()
        {
            CreateCucumber();
            UpdateColor();
        }

        private void OnValidate()
        {
            UpdateGradient();
            rangeFloat.Validate();
            rangeInt.Validate();
        }

        private void UpdateColor()
        {
            var t = Mathf.Clamp01(_timer.TimeLocal / _timer.Duration);

            t = 2 * (t < 0.5f ? t : (1 - t));

            cucuBlendColor.SetBlend(t);

            _color = cucuBlendColor.Color;
        }

        private void UpdateGradient()
        {
            cucuBlendColor.SetGradient(_colorPaletteMap[_colorMap].ToGradient());
        }

        private void InitTriggers()
        {
            var cubeTrigger = _cubanationZone.GetComponent<CucuTrigger>();

            cubeTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Enter)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;
                    if (!cucuTag.Key.Equals("cube")) return;
                    cucuTag.gameObject.GetComponentInChildren<Renderer>().material
                        .SetColor("_EmissionColor", _color);
                });

            cubeTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Exit)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;
                    if (!cucuTag.Key.Equals("cube")) return;
                    Transformation(cucuTag.transform, _cucuCube);
                });

            var sphereTrigger = _spheranationZone.GetComponent<CucuTrigger>();

            sphereTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Enter)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;
                    if (!cucuTag.Key.Equals("sphere")) return;
                    cucuTag.gameObject.GetComponentInChildren<Renderer>().material
                        .SetColor("_EmissionColor", _color);
                });

            sphereTrigger.RegisterComponent<CucuTag>(CucuTrigger.TriggerState.Exit)
                .AddListener(t =>
                {
                    var cucuTag = t as CucuTag;
                    if (!cucuTag.Key.Equals("sphere")) return;
                    Transformation(cucuTag.transform, _cucuSphere);
                });
        }

        private void CreateCucumber()
        {
            var obj = Instantiate(_cucuMember,
                _cucuRoot.position + Random.onUnitSphere * 2, Random.rotation,
                _cucuRoot);

            obj.transform.localScale *= (Random.value - 0.5f) * 0.333f + 2f;
            obj.name = $"{_cucus.Count}_{obj.name}";

            obj.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * 1000);
            obj.GetComponent<CucuTag>().SetKey(Random.value < 0.5f ? "cube" : "sphere");

            _cucus.Add(obj.transform);

            ClearCucumbers();

            _countCurr = _cucus.Count;
        }

        private void ClearCucumbers()
        {
            while (_cucus.Count > _countMax)
                DeleteCucumber();
        }

        private void DeleteCucumber()
        {
            if (_cucus.Count > 0)
            {
                var cucu = _cucus[0];
                _cucus.RemoveAt(0);
                if (cucu != null)
                    Destroy(cucu.gameObject);
            }
        }

        private void Transformation(Transform origin, GameObject target)
        {
            var index = _cucus.IndexOf(origin);

            if (!(0 <= index && index < _cucus.Count))
            {
                Destroy(origin.gameObject);
                return;
            }

            var obj = Instantiate(target, origin.position, origin.rotation);
            _cucus[index] = obj.transform;

            obj.transform.parent = origin.parent;

            var rigOrigin = origin.gameObject.GetComponent<Rigidbody>();
            var rigTarget = obj.GetComponent<Rigidbody>();

            rigTarget.velocity = rigOrigin.velocity;
            rigTarget.angularVelocity = rigOrigin.angularVelocity;

            Destroy(origin.gameObject);

            var timer = CucuTimerFactory.Create();

            var mat = obj.gameObject.GetComponent<Renderer>().material;

            timer
                .SetDuration(1f)
                .SetTick(5)
                .OnTick(() =>
                {
                    var t = Mathf.Clamp01(timer.TimeLocal / timer.Duration);
                    var colorInit = _color.LerpTo(Color.black, t);
                    mat?.SetColor("_EmissionColor", colorInit);
                })
                .OnStop(() => mat?.SetColor("_EmissionColor", Color.black))
                .DestroyAfterStop()
                .StartTimer();
        }
    }
}