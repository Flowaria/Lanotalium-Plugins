using Lanotalium.Chart;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

namespace Flowaria.Railnote.Curve.Lib
{
    public sealed class HoldLineWorker : MonoBehaviour
    {
        private const BindingFlags BINDING_PRIVATE = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static float QUALITY = 1.0f;
        public static float UV_DURATION = 1.0f;
        public static float UPDATE_INTERVAL = 0.05f;

        public LanotaHoldNote Note;
        public HoldLineRenderer[] MeshRenderers = new HoldLineRenderer[0];

        public Material UnTouchMaterial;
        public Material TouchMaterial;

        private LimTunerManager _Tuner;
        private GameObject _TunerObject;

        private static Func<float, float> _CalcMovePercent = null;
        private static LimHoldNoteManager _HoldManager = null;

        private int _NoteSortingID;

        private bool _initalized = false;

        private float[] _NoteData = null;

        private NativeArray<float> _ScrollTimes;
        private NativeArray<float> _ScrollSpeeds;

        private void Update()
        {
            if (!_initalized)
            {
                if (Note == null)
                    Destroy(this);

                WriteNoteData();

                _Tuner = LimTunerManager.Instance;
                _TunerObject = GameObject.Find("LimTunerManager/Tuner");
                _NoteSortingID = SortingLayer.NameToID("Note");


                //Check Update for MovePercent Delegate
                bool shouldUpdateDelegate = false;
                if(_HoldManager == null || _CalcMovePercent == null)
                {
                    _HoldManager = _Tuner.HoldNoteManager;
                    shouldUpdateDelegate = true;
                }
                else if(_HoldManager != _Tuner.HoldNoteManager)
                {
                    _HoldManager = _Tuner.HoldNoteManager;
                    shouldUpdateDelegate = true;
                }

                if(shouldUpdateDelegate)
                {
                    var managerType = typeof(LimHoldNoteManager);
                    var method = managerType.GetMethod("CalculateMovePercent", BINDING_PRIVATE);
                    var newDelegate = Delegate.CreateDelegate(typeof(Func<float, float>), _HoldManager, method);
                    _CalcMovePercent = (Func<float, float>)newDelegate;
                }

                var scrolls = _Tuner.ScrollManager.Scroll;

                _ScrollTimes = new NativeArray<float>(scrolls.Count, Allocator.Persistent);
                _ScrollSpeeds = new NativeArray<float>(scrolls.Count, Allocator.Persistent);
                for (int j = 0; j < scrolls.Count; j++)
                {
                    _ScrollSpeeds[j] = scrolls[j].Speed;
                    _ScrollTimes[j] = scrolls[j].Time;
                }

                _initalized = true;
            }

            

            CheckRendererCount();
            UpdateRendererMeshValue();

            UpdateRendererMesh();
        }

        private void OnEnable()
        {
            //Restart Coroutine
            StartCoroutine(NoteDataCheck());
        }

        void OnDisable()
        {
            for (int i = 0; i < MeshRenderers.Length; i++)
            {
                var renderer = MeshRenderers[i];
                renderer.ClearMesh();
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < MeshRenderers.Length; i++)
            {
                Destroy(MeshRenderers[i].gameObject);
            }
        }

        private IEnumerator NoteDataCheck()
        {
            while (true)
            {
                if (IsNoteDataChanged())
                {
                    //Force Refresh
                    LimOperationManager.Instance.RefreshJointAbsoluteValues(Note);
                    WriteNoteData();
                    ClearAllMesh();
                }
                yield return new WaitForSecondsRealtime(UPDATE_INTERVAL);
            }
        }

        private void WriteNoteData()
        {
            int dataCount = 2 + (Note.Jcount * 3);
            _NoteData = new float[dataCount];
            _NoteData[0] = Note.Time;
            _NoteData[1] = Note.Degree;
            for (int i = 0; i < Note.Jcount; i++)
            {
                int offset = (i * 3) + 2;
                _NoteData[offset + 0] = Note.Joints[i].dTime;
                _NoteData[offset + 1] = Note.Joints[i].dDegree;
                _NoteData[offset + 2] = Note.Joints[i].Cfmi;
            }
        }

        private bool IsNoteDataChanged()
        {
            if(_NoteData == null)
                return true;


            int dataCount = 2 + (Note.Jcount * 3);
            if (dataCount != _NoteData.Length)
                return true;

            if (_NoteData[0] != Note.Time)
                return true;

            if (_NoteData[1] != Note.Degree)
                return true;

            for (int i = 0; i < Note.Jcount; i++)
            {
                var joint = Note.Joints[i];
                int offset = (i * 3) + 2;

                if (_NoteData[offset + 0] != joint.dTime)
                    return true;

                if (_NoteData[offset + 1] != joint.dDegree)
                    return true;

                if (_NoteData[offset + 2] != joint.Cfmi)
                    return true;
            }

            return false;
        }

        private void CheckRendererCount()
        {
            int count = 1;
            if (Note.Joints != null)
            {
                int jointCount = Note.Joints.Count;
                count = jointCount > 0 ? jointCount : 1;
            }

            //more than before
            if (MeshRenderers.Length < count)
            {
                var newRenderers = new HoldLineRenderer[count];
                for (int i = 0; i < count; i++)
                {
                    if (i >= MeshRenderers.Length)
                    {
                        newRenderers[i] = CreateRendererObject().GetComponent<HoldLineRenderer>();
                    }
                    else
                    {
                        newRenderers[i] = MeshRenderers[i];
                    }
                }

                MeshRenderers = newRenderers;
            }
            //less than before
            else if (MeshRenderers.Length > count)
            {
                //skip existing one
                for (int i = count; i < MeshRenderers.Count(); i++)
                {
                    Destroy(MeshRenderers[i].gameObject);
                }
                MeshRenderers = MeshRenderers.Take(count).ToArray();
            }
        }

        private void UpdateRendererMeshValue()
        {
            float chartTime = _Tuner.ChartTime;
            float uvTileScale = TouchMaterial.mainTextureScale.x;
            float uvTileScaleInverse = 1.0f / UV_DURATION;

            Parallel.For(0, MeshRenderers.Length,
            i =>
            {
                var renderer = MeshRenderers[i];

                if (!renderer.DoneInitRenderer)
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

                if(startTime + deltaTime <= chartTime)
                {
                    //Skip when joint is fully hidden
                    renderer.Hide();
                    return;
                }
                else
                {
                    renderer.Show();
                }

                deltaDegree = endDegree - startDegree;


                //Get Total Points count inside Joint
                int totalPoints = 0;
                if (!renderer.DoneInitMesh)
                {
                    float unscaledTotalPoints = DetermineIdealCount(deltaTime, deltaDegree);
                    totalPoints = (int)(unscaledTotalPoints * QUALITY);

                    renderer.SetCount(totalPoints);

                    //Set UV Scale
                    var secPoints = (int)((UV_DURATION / deltaTime) * totalPoints);
                    renderer.SetUVScale((1.0f / secPoints) * uvTileScaleInverse);
                }
                else
                {
                    totalPoints = renderer.Length;
                }

                float percentStep = 1.0f / (float)(totalPoints - 1);
                Parallel.For(0, totalPoints,
                j =>
                {
                    //First Index will be Start of the Rail
                    float percent = j * percentStep;
                    float time = startTime + (deltaTime * percent);

                    //Initialize Basic Info for Renderer
                    if (!renderer.DoneInitMesh)
                    {
                        float degree = startDegree + (deltaDegree * EasingLookupTable.EaseEvaluate(percent, ease));
                        renderer.SetPointByDegree(j, degree);

                        var isReverse = CalculateScrollSpeed(time) < 0.0f;
                        renderer.SetReverseScrollSpeed(j, isReverse);
                    }

                    //Skip the move calculation for obvious one
                    if (time <= chartTime)
                    {
                        renderer.SetPercent(j, 1.0f);
                        return;
                    }

                    //Put Move percent to Renderer
                    float movePercent = CalcEasedMovePercent(time);
                    renderer.SetPercent(j, movePercent * 0.01f);
                });
            });
        }

        private void UpdateRendererMesh()
        {
            float chartTime = _Tuner.ChartTime;
            var rotation = new Vector3(0.0f, _Tuner.CameraManager.CurrentRotation + 180.0f, 0.0f);
            //Finalize
            for (int i = 0; i < MeshRenderers.Length; i++)
            {
                var renderer = MeshRenderers[i];

                if (!renderer.DoneInitRenderer || renderer.IsHidden)
                {
                    continue;
                }

                if (!renderer.DoneInitMesh)
                {
                    if (i >= 1)
                    {
                        renderer.SetUVStart(MeshRenderers[i - 1].UVEnd);
                    }
                    renderer.InitMesh();
                }

                renderer.Material = (Note.Time <= chartTime) ? TouchMaterial : UnTouchMaterial;
                renderer.gameObject.transform.eulerAngles = rotation;

                renderer.UpdateMeshImmediate();
            }
        }

        public void ClearAllMesh()
        {
            for (int i = 0; i < MeshRenderers.Length; i++)
            {
                MeshRenderers[i].ClearMesh();
            }
        }

        private float DetermineIdealCount(float deltaTime, float deltaDegree)
        {
            float timeSegment = deltaTime * 100.0f;
            float degreeSegment = Mathf.Abs(deltaDegree) * 2.0f;

            return Mathf.Max(degreeSegment + timeSegment, 100.0f);
        }

        private float CalcEasedMovePercent(float time)
        {
            var percent = _CalcMovePercent(time);
            return EasingLookupTable.MoveEasePercentEvaluate(percent, LimNoteEase.Instance.DemoMode);
        }

        private float CalculateScrollSpeed(float timing)
        {
            var scrolls = _Tuner.ScrollManager.Scroll;
            if (scrolls != null)
            {
                for (int i = 0; i < scrolls.Count; i++)
                {
                    var scrTime = scrolls[i].Time;
                    if (scrTime > timing)
                    {
                        if (i > 0)
                        {
                            return scrolls[i - 1].Speed;
                        }
                    }
                    else if (timing == scrTime)
                    {
                        return scrolls[i].Speed;
                    }
                }
            }

            return 1.0f;
        }

        private GameObject CreateRendererObject()
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.layer = 9;

            var renderer = obj.AddComponent<HoldLineRenderer>();
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

            return obj;
        }
    }
}