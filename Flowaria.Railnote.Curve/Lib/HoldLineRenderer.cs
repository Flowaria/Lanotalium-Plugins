using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class HoldLineRenderer : MonoBehaviour
    {
        public const float TUNER_CORE_PERCENT = 0.125f;

        public float Duration
        {
            get => Duration;
            set
            {
                if (Duration < 0.0f) Duration = 0.0f;
                else Duration = value;
            }
        }

        public bool DoneInitRenderer { get; private set; } = false;
        public bool DoneInitMesh { get; private set; } = false;

        public bool IsHidden { get; private set; } = false;

        public int Length { get { return _Points.Length; } }

        public float UVEnd { get { return _UVStart + (_UVScale * Length); } }

        public Material Material
        {
            get => _MeshRenderer.material;
            set => _MeshRenderer.material = value;
        }

        public int SortingLayerID;
        public int SortingOrder;

        private MeshPoint[] _Points = new MeshPoint[0];
        private float[] _Percents;
        private bool[] _IsReverse;
        private bool[] _IsDirty;
        private float _UVStart = 0.0f;
        private float _UVScale = 0.0f;

        private MeshFilter _MeshFilter;
        private MeshRenderer _MeshRenderer;
        private Mesh _Mesh;

        private void Update()
        {
            if (!DoneInitRenderer)
            {
                AwakeFixed();
                DoneInitRenderer = true;
            }
        }

        private void AwakeFixed()
        {
            SetCount(2);

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

            DoneInitMesh = false;
        }

        public void SetCount(int count)
        {
            if(count <= 0)
            {
                throw new ArgumentOutOfRangeException("Count for Rail Mesh Renderer Can't be zero or negative!");
            }
            else if(count <= 1)
            {
                throw new ArgumentOutOfRangeException("Too less points! Please assign more than 1!");
            }

            _Points = new MeshPoint[count];
            _Percents = Enumerable.Repeat(0.0f, count).ToArray();
            _IsReverse = Enumerable.Repeat(false, count).ToArray();
            _IsDirty = Enumerable.Repeat(true, count).ToArray();

            DoneInitMesh = false;
        }

        public void SetPointByDegree(int index, float degree)
        {
            if (0 <= index && index < _Points.Length)
            {
                var rad = degree * Mathf.Deg2Rad;
                var point = new MeshPoint
                {
                    BasePoint = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad))
                };
                _Points[index] = point;
            } 
        }

        public void SetPercent(int index, float percent)
        {
            if (0 <= index && index < _Points.Length)
            {
                percent = Mathf.Clamp01(percent);

                if (!(_Percents[index] <= TUNER_CORE_PERCENT && percent <= TUNER_CORE_PERCENT))
                {
                    if(_Percents[index] == percent)
                    {
                        return;
                    }

                    _Percents[index] = percent;
                    _IsDirty[index] = true;
                }   
            }
        }

        public void SetReverseScrollSpeed(int index, bool onReverse)
        {
            if (0 <= index && index < _Points.Length)
            {
                if (_IsReverse[index] != onReverse)
                {
                    _IsReverse[index] = onReverse;
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

            var triangles_count = points.Length - 2;
            var triangles = new int[triangles_count * 3];

            Parallel.For(0, triangles_count, i =>
            {
                int trioffset = i * 3;
                int offset = i;
                int p1 = offset;
                int p2 = offset + 1;
                int p3 = offset + 2;

                if (i % 2 == 0)
                {
                    triangles[trioffset + 0] = p1; //0
                    triangles[trioffset + 1] = p3; //2
                    triangles[trioffset + 2] = p2; //1
                }
                else
                {
                    triangles[trioffset + 0] = p3; //3
                    triangles[trioffset + 1] = p1; //1
                    triangles[trioffset + 2] = p2; //2
                }
            });

            _Mesh.triangles = triangles;

            var uvs = new Vector2[points.Length];
            float uv_value = _UVStart;
            for (int i = 0; i < uvs.Length; i++)
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

            DoneInitMesh = true;
        }

        public void UpdateMeshImmediate()
        {
            if (!DoneInitRenderer)
                return;

            if (!DoneInitMesh)
                return;

            bool isChanged = false;

            var points = _Mesh.vertices;
            var trigs = _Mesh.triangles;

            var lastInvisible = 0;
            float lastInvisiblePercent = -1.0f;

            Parallel.For(0, _Points.Length, i =>
            {
                if (!_IsDirty[i])
                {
                    return;
                }

                isChanged = true;

                //able to flip mesh direction
                if (_Percents[i] >= 0.99f)
                {
                    if (i > lastInvisible)
                    {
                        lastInvisible = i;
                        lastInvisiblePercent = _Percents[i];
                    }

                    _Percents[i] = _IsReverse[i] ? 1.0f : 0.0f;
                    
                    //Reversed => false
                    FlipTrig(ref trigs, i, !_IsReverse[i]);
                }
                else
                {
                    //Reversed => true
                    FlipTrig(ref trigs, i, _IsReverse[i]);
                }

                points[i * 2] = _Points[i].GetLeft(_Percents[i]);
                points[(i * 2) + 1] = _Points[i].GetRight(_Percents[i]);
            });

            if (isChanged)
            {
                if (lastInvisiblePercent >= 0.0f)
                {
                    //Head Flickering bug prevent
                    FlipTrig(ref trigs, lastInvisible, _IsReverse[lastInvisible]);
                    points[lastInvisible * 2] = _Points[lastInvisible].GetLeft(0.99f);
                    points[(lastInvisible * 2) + 1] = _Points[lastInvisible].GetRight(0.99f);
                }

                _Mesh.vertices = points;
                _Mesh.triangles = trigs;
                _Mesh.RecalculateBounds();
            }
        }

        public void ClearMesh()
        {
            if (DoneInitRenderer && _Mesh.vertexCount > 0)
            {
                _Mesh.Clear();
            }

            DoneInitMesh = false;
        }

        public void Hide()
        {
            if (DoneInitRenderer && _MeshRenderer.enabled)
            {
                _MeshRenderer.enabled = false;
                IsHidden = true;
            }
        }

        public void Show()
        {
            if (DoneInitRenderer && !_MeshRenderer.enabled)
            {
                _MeshRenderer.enabled = true;
                IsHidden = false;
            }
        }

        private void FlipTrig(ref int[] trigs, int index, bool backward)
        {
            int trigoffset = index * 6;

            if (index <= 0)
            {
                return;
            }

            int swap_a1 = trigoffset - 1; //5
            int swap_a2 = trigoffset - 2; //4
            int swap_b1 = trigoffset - 4; //2
            int swap_b2 = trigoffset - 5; //1

            //Forward to Backward
            if (backward)
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
            //Backward to Forward
            else
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
        }

        private void Swap(ref int[] trigs, int idx1, int idx2)
        {
            int temp = trigs[idx1];
            trigs[idx1] = trigs[idx2];
            trigs[idx2] = temp;
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