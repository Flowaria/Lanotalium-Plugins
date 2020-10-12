using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HoldLineMeshRenderer : MonoBehaviour
    {
        public static readonly float TUNER_CORE_PERCENT = 0.125f;

        public float Duration
        {
            get => Duration;
            set
            {
                if (Duration < 0.0f) Duration = 0.0f;
                else Duration = value;
            }
        }

        public bool Initialized { get; private set; } = false;

        public int Length { get { return _Points.Length; } }

        public float UVEnd { get { return _UVStart + (_UVScale * Length); } }

        public Material material
        {
            get => _MeshRenderer.material;
            set => _MeshRenderer.material = value;
        }

        public int SortingLayerID;
        public int SortingOrder;

        private MeshPoint[] _Points = new MeshPoint[0];
        private float[] _Percents;
        private bool[] _ShouldDisplay;
        private bool[] _IsDirty;
        private float _UVStart = 0.0f;
        private float _UVScale = 0.0f;

        private MeshFilter _MeshFilter;
        private MeshRenderer _MeshRenderer;
        private Mesh _Mesh;

        public bool _isAwakeCalled = false;

        private void Update()
        {
            if (!_isAwakeCalled)
            {
                AwakeFixed();
                _isAwakeCalled = true;
            }
        }

        private void AwakeFixed()
        {
            SetPointCount(1);

            _MeshFilter = GetComponent<MeshFilter>();
            if (_MeshFilter == null)
            {
                _MeshFilter = gameObject.AddComponent<MeshFilter>();
            }

            _MeshRenderer = GetComponent<MeshRenderer>();
            if (_MeshRenderer == null)
            {
                _MeshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            _MeshRenderer.sortingLayerID = SortingLayerID;
            _MeshRenderer.sortingOrder = SortingOrder;

            _Mesh = _MeshFilter.mesh;
            _Mesh.Clear();

            _MeshRenderer.enabled = true;

            Initialized = false;
        }

        public void SetPointCount(int count)
        {
            _Points = new MeshPoint[count];
            _Percents = Enumerable.Repeat(0.0f, count).ToArray();
            _ShouldDisplay = Enumerable.Repeat(true, count).ToArray();
            _IsDirty = Enumerable.Repeat(true, count).ToArray();

            Initialized = false;
        }

        public void SetPointByDegree(int index, float degree)
        {
            var point = new MeshPoint();
            point.BasePoint = new Vector3(Mathf.Sin(degree * Mathf.Deg2Rad), 0, Mathf.Cos(degree * Mathf.Deg2Rad));
            _Points[index] = point;
        }

        public void SetPointPercent(int index, float percent)
        {
            if (0 <= index && index < _Points.Length)
            {
                if (!(_Percents[index] <= TUNER_CORE_PERCENT && percent <= TUNER_CORE_PERCENT))

                    _Percents[index] = percent;
                _IsDirty[index] = true;
            }
        }

        public void SetPointDisplay(int index, bool shouldDisplay)
        {
            if (0 <= index && index < _Points.Length)
            {
                if (_ShouldDisplay[index] != shouldDisplay)
                {
                    _ShouldDisplay[index] = shouldDisplay;
                    _IsDirty[index] = true;
                }
            }
        }

        public void SetUVStart(float start)
        {
            _UVStart = start;
        }

        public void SetUVScale(float scale)
        {
            _UVScale = scale;
        }

        public void InitMesh()
        {
            if (_Points.Length < 2)
            {
                return;
            }

            _Mesh.Clear();

            var points = PointsToArray();
            _Mesh.vertices = points;

            var triangles_length = points.Length - 2;
            var triangles = new int[triangles_length * 3];

            Parallel.For(0, triangles_length, i =>
            {
                int trioffset = i * 3;
                int offset = i;
                int p1 = offset;
                int p2 = offset + 1;
                int p3 = offset + 2;

                if (i % 2 == 0)
                {
                    triangles[trioffset + 0] = p1; //1
                    triangles[trioffset + 1] = p3; //3
                    triangles[trioffset + 2] = p2; //2
                }
                else
                {
                    triangles[trioffset + 0] = p1; //2
                    triangles[trioffset + 1] = p2; //3
                    triangles[trioffset + 2] = p3; //4
                }
            });

            _Mesh.triangles = triangles;

            var uvs = new Vector2[points.Length];
            float uv_value = _UVStart;
            for (int i = uvs.Length - 1; i >= 0; i--)
            {
                switch (i % 2)
                {
                    case 0:
                        uvs[i] = new Vector2(0, uv_value);
                        break;

                    case 1:
                        uvs[i] = new Vector2(1, uv_value);
                        uv_value += _UVScale;
                        break;
                }
            }

            _Mesh.uv = uvs;
            for (int i = 0; i < _Points.Length; i++)
            {
                _IsDirty[i] = true;
                _Percents[i] = 0.0f;
            }

            Initialized = true;
        }

        private bool _updateMeshWorking = false;

        public void UpdateMesh()
        {
            if (!_updateMeshWorking)
            {
                _updateMeshWorking = true;
                StartCoroutine(UpdateMeshWait());
            }
        }

        private IEnumerator UpdateMeshWait()
        {
            var task = UpdateMeshLazy();
            while (!task.IsCompleted)
            {
                yield return null;
            }
            _updateMeshWorking = false;
        }

        public void UpdateMeshImmediate()
        {
            if (!Initialized)
                return;

            bool isChanged = false;

            var points = _Mesh.vertices;
            var trigs = _Mesh.triangles;

            var lastInvisible = 999999;
            float lastInvisiblePercent = -1.0f;

            Parallel.For(0, _Points.Length, i =>
            {
                if (!_IsDirty[i])
                {
                    return;
                }

                isChanged = true;

                //able to flip mesh direction
                if (i > 1)
                {
                    if ((!_ShouldDisplay[i] || _Percents[i] >= 0.999f) && i > 1)
                    {
                        if (i < lastInvisible)
                        {
                            lastInvisible = i;
                            lastInvisiblePercent = _Percents[i];
                        }

                        //Send it to nowhere, I don't want to see thin lines!
                        _Percents[i] = 1.0f + (0.1f * i);
                        FlipTrig(ref trigs, i, true);
                    }
                    else if (i > 1)
                    {
                        if (_Percents[i] >= 0.985f)
                        {
                            _Percents[i] = 1.0f;
                        }

                        FlipTrig(ref trigs, i, false);
                    }
                }

                points[i * 2] = _Points[i].GetLeft(_Percents[i]) * (_Percents[i] * 10f);
                points[(i * 2) + 1] = _Points[i].GetRight(_Percents[i]) * (_Percents[i] * 10f);
            });

            if (isChanged)
            {
                if (lastInvisiblePercent >= 0.0f)
                {
                    //Head Flickering bug prevent
                    FlipTrig(ref trigs, lastInvisible, false);
                    points[lastInvisible * 2] = _Points[lastInvisible].GetLeft(0.99f) * (0.99f * 10f);
                    points[(lastInvisible * 2) + 1] = _Points[lastInvisible].GetRight(0.99f) * (0.99f * 10f);
                }

                _Mesh.vertices = points;
                _Mesh.triangles = trigs;
                _Mesh.RecalculateBounds();
            }

            _updateMeshWorking = false;
        }

        private async Task UpdateMeshLazy()
        {
            if (!Initialized)
                return;

            bool isChanged = false;

            var points = _Mesh.vertices;
            var trigs = _Mesh.triangles;

            var lastInvisible = 999999;
            float lastInvisiblePercent = -1.0f;

            await Task.Run(() => Parallel.For(0, _Points.Length, i =>
            {
                if (!_IsDirty[i])
                {
                    return;
                }

                isChanged = true;

                //able to flip mesh direction
                if (i > 1)
                {
                    if ((!_ShouldDisplay[i] || _Percents[i] >= 0.999f) && i > 1)
                    {
                        if (i < lastInvisible)
                        {
                            lastInvisible = i;
                            lastInvisiblePercent = _Percents[i];
                        }

                        //Send it to nowhere, I don't want to see thin lines!
                        _Percents[i] = 1.0f + (0.1f * i);
                        FlipTrig(ref trigs, i, true);
                    }
                    else if (i > 1)
                    {
                        if (_Percents[i] >= 0.985f)
                        {
                            _Percents[i] = 1.0f;
                        }

                        FlipTrig(ref trigs, i, false);
                    }
                }

                points[i * 2] = _Points[i].GetLeft(_Percents[i]) * (_Percents[i] * 10f);
                points[(i * 2) + 1] = _Points[i].GetRight(_Percents[i]) * (_Percents[i] * 10f);
            }));

            if (isChanged)
            {
                if (lastInvisiblePercent >= 0.0f)
                {
                    //Head Flickering bug prevent
                    FlipTrig(ref trigs, lastInvisible, false);
                    points[lastInvisible * 2] = _Points[lastInvisible].GetLeft(0.99f) * (0.99f * 10f);
                    points[(lastInvisible * 2) + 1] = _Points[lastInvisible].GetRight(0.99f) * (0.99f * 10f);
                }

                _Mesh.vertices = points;
                _Mesh.triangles = trigs;
                _Mesh.RecalculateBounds();
            }

            _updateMeshWorking = false;
        }

        private void FlipTrig(ref int[] trigs, int index, bool backward)
        {
            int trigoffset = index * 6;

            int swap_a1 = trigoffset - 5; //1
            int swap_a2 = trigoffset - 4; //2
            int swap_b1 = trigoffset - 3; //3
            int swap_b2 = trigoffset - 2; //4

            //Forward to Backward
            if (backward)
            {
                if (trigs[swap_a1] < trigs[swap_a2])
                {
                    Swap(ref trigs, swap_a1, swap_a2);
                }

                if (trigs[swap_b2] < trigs[swap_b1])
                {
                    Swap(ref trigs, swap_b1, swap_b2);
                }
            }
            //Backward to Forward
            else
            {
                if (trigs[swap_a1] > trigs[swap_a2])
                {
                    Swap(ref trigs, swap_a1, swap_a2);
                }

                if (trigs[swap_b2] > trigs[swap_b1])
                {
                    Swap(ref trigs, swap_b1, swap_b2);
                }
            }
        }

        private void Swap(ref int[] trigs, int idx1, int idx2)
        {
            int temp = trigs[idx1];
            trigs[idx1] = trigs[idx2];
            trigs[idx2] = temp;
        }

        public void ClearMesh()
        {
            if (_Mesh != null && _Mesh.vertexCount > 0)
            {
                _Mesh.Clear();
            }

            Initialized = false;
        }

        private Vector3[] PointsToArray()
        {
            var array = new Vector3[_Points.Length * 2];
            for (int i = 0; i < _Points.Length; i++)
            {
                array[i * 2] = _Points[i].GetLeft(TUNER_CORE_PERCENT);
                array[(i * 2) + 1] = _Points[i].GetRight(TUNER_CORE_PERCENT);
            }

            return array;
        }
    }
}