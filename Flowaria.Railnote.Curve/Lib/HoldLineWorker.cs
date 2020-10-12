using Lanotalium.Chart;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Flowaria.Railnote.Curve.Lib
{
    public class HoldLineWorker : MonoBehaviour
    {
        public LanotaHoldNote Note;
        public HoldLineMeshRenderer[] MeshRenderers = new HoldLineMeshRenderer[0];

        public Material UnTouchMaterial;
        public Material TouchMaterial;

        private LimTunerManager _Tuner;
        private GameObject _TunerObject;

        private static Func<float, float> _CalcMovePercent = null;

        private int _NoteSortingID;
        

        private bool _initalized = false;

        private void Update()
        {
            if (!_initalized)
            {
                if (Note == null)
                {
                    Destroy(this);
                }
                
                _Tuner = LimTunerManager.Instance;
                _TunerObject = GameObject.Find("LimTunerManager/Tuner");
                _NoteSortingID = SortingLayer.NameToID("Note");

                MethodInfo method = typeof(LimHoldNoteManager).GetMethod("CalculateMovePercent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                _CalcMovePercent = (Func<float, float>)Delegate.CreateDelegate(typeof(Func<float, float>), _Tuner.HoldNoteManager, method);

                _initalized = true;
            }

            CheckRendererCount();
            UpdateRenderers();
        }

        private void OnEnable()
        {
            for (int i = 0; i < MeshRenderers.Length; i++)
            {
                var renderer = MeshRenderers[i];
                renderer.ClearMesh();
                renderer.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < MeshRenderers.Length; i++)
            {
                var renderer = MeshRenderers[i];
                renderer.ClearMesh();
                renderer.gameObject.SetActive(false);
            }
        }

        private int _lastRendererCount = 0;

        private void CheckRendererCount()
        {
            int count = 1;
            if (Note.Joints != null)
            {
                int jointCount = Note.Joints.Count;
                count = jointCount > 0 ? jointCount : 1;
            }

            //more than before
            if (_lastRendererCount < count)
            {
                var newRenderers = new HoldLineMeshRenderer[count];
                for (int i = 0; i < count; i++)
                {
                    if (i >= MeshRenderers.Length)
                    {
                        var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                        obj.layer = 9;

                        var renderer = obj.AddComponent<HoldLineMeshRenderer>();
                        renderer.SortingLayerID = _NoteSortingID;
                        renderer.SortingOrder = -32767;

                        var sort = obj.AddComponent<SortingGroup>();
                        sort.sortingLayerID = _NoteSortingID;
                        sort.sortingOrder = -32767;

                        var collider = obj.GetComponent<MeshCollider>();
                        Destroy(collider);

                        var mrenderer = obj.GetComponent<MeshRenderer>();
                        mrenderer.enabled = false;

                        obj.transform.SetParent(_TunerObject.transform, false);

                        float scale = 1.0f / _TunerObject.transform.localScale.x;
                        obj.transform.localScale = new Vector3(scale, scale, scale);

                        newRenderers[i] = renderer;
                    }
                    else
                    {
                        newRenderers[i] = MeshRenderers[i];
                    }
                }

                MeshRenderers = newRenderers;

                _lastRendererCount = count;
            }
            //less than before
            else if (_lastRendererCount > count)
            {
                //skip existing one
                for (int i = count; i < MeshRenderers.Count(); i++)
                {
                    Destroy(MeshRenderers[i].gameObject);
                }
                MeshRenderers = MeshRenderers.Take(count).ToArray();

                _lastRendererCount = count;
            }
        }

        private void UpdateRenderers()
        {
            float uvRepeatTime = 1.0f;
            float uvTileScale = TouchMaterial.mainTextureScale.x;
            float uvTileScaleInverse = 1.0f / uvTileScale;

            Parallel.For(0, MeshRenderers.Length,
            i =>
            {
                var renderer = MeshRenderers[i];

                if (!renderer._isAwakeCalled)
                {
                    return;
                }

                float startTime, deltaTime;
                float startDegree, endDegree, deltaDegree;
                int ease = 0;

            //No Joints Variant
            if (MeshRenderers.Length <= 1)
                {
                    startTime = Note.Time;
                    deltaTime = Note.Duration;

                    if (Note.Jcount == 0)
                    {
                        startDegree = Note.Degree;
                        endDegree = Note.Degree;
                    }
                    else
                    {
                        startDegree = Note.Degree;
                        endDegree = Note.Degree + Note.Joints[0].dDegree;

                        ease = Note.Joints[0].Cfmi;
                    }
                }
            //1+ Joints Variant
            else
                {
                    var joint = Note.Joints[i];
                    startTime = joint.aTime - joint.dTime;
                    deltaTime = joint.dTime;

                    startDegree = joint.aDegree - joint.dDegree;
                    endDegree = joint.aDegree;

                    ease = joint.Cfmi;
                }

                deltaDegree = endDegree - startDegree;

                float unscaledTotalPoints = Mathf.Max(
                    Mathf.Abs(deltaDegree) * 1.25f,
                    (150.0f * deltaTime * 1.5f),
                    100.0f);

                int totalPoints = (int)(unscaledTotalPoints * 1.5f);

                bool shouldInit = false;
                if (!renderer.Initialized || renderer.Length != totalPoints)
                {
                    renderer.SetPointCount(totalPoints);
                    shouldInit = true;
                }

                var secPoints = (int)((uvRepeatTime / deltaTime) * totalPoints);

                renderer.SetUVScale((1.0f / secPoints) * uvTileScaleInverse);

                Parallel.For(0, totalPoints,
                j =>
                {
                    //Last Index will be Start of the Rail
                    float percent = (j / (float)(totalPoints - 1));
                    percent = 1.0f - percent;

                    if (shouldInit)
                    {
                        float degree = startDegree + (deltaDegree * EasingLookupTable.EaseEvaluate(percent, ease));
                        renderer.SetPointByDegree(j, degree);
                    }

                    float time = startTime + (deltaTime * percent);

                    float movePercent = CalcEasedMovePercent(time);
                    renderer.SetPointPercent(j, movePercent * 0.01f);
                });
            });

            bool touched = false;
            if (Note.Time <= _Tuner.ChartTime)
            {
                touched = true;
            }
            else
            {
                touched = false;
            }

            for (int i = 0; i < MeshRenderers.Length; i++)
            {
                var renderer = MeshRenderers[i];

                if (!renderer._isAwakeCalled)
                {
                    break;
                }

                if (!renderer.Initialized)
                {
                    if (i >= 1)
                    {
                        renderer.SetUVStart(MeshRenderers[i - 1].UVEnd);
                    }
                    renderer.InitMesh();
                }

                renderer.material = touched ? TouchMaterial : UnTouchMaterial;

                renderer.UpdateMeshImmediate();
                renderer.gameObject.transform.eulerAngles = new Vector3(0.0f, _Tuner.CameraManager.CurrentRotation + 180.0f, 0.0f);
            }
        }

        public void ForceUpdate()
        {
            for(int i = 0;i<MeshRenderers.Length;i++)
            {
                MeshRenderers[i].ClearMesh();
            }
        }

        
        private float CalcEasedMovePercent(float time)
        {
            var percent = _CalcMovePercent(time);
            return EasingLookupTable.MoveEasePercentEvaluate(percent, LimNoteEase.Instance.DemoMode);
        }
    }
}