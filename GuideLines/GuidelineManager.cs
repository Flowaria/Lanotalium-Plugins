using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GuideLines
{
    public class GuidelineManager : MonoBehaviour
    {
        public GameObject TunerObject;
        public GameObject TunerDot_Prefab;

        private GameObject _TunerDot;
        private SpriteRenderer _TunerDotRenderer;
        private LineLayoutManager _VerticalLayout, _HorizontalLayout;

        private bool _isInitialized = false;

        public void Init()
        {
            if(_isInitialized)
            {
                return;
            }

            for(int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name.Equals("VerticalLayout"))
                {
                    _VerticalLayout = child.gameObject.AddComponent<LineLayoutManager>();
                    _VerticalLayout.LinePrefix = "VerticalLine ";
                    _VerticalLayout.Init();
                }
                else if (child.name.Equals("HorizontalLayout"))
                {
                    _HorizontalLayout = child.gameObject.AddComponent<LineLayoutManager>();
                    _HorizontalLayout.LinePrefix = "HorizontalLine ";
                    _HorizontalLayout.Init();
                }
            }

            if(_VerticalLayout == null || _HorizontalLayout == null)
            {
                this.enabled = false;
            }

            _TunerDot = GameObject.Instantiate(TunerDot_Prefab, TunerObject.transform);
            _TunerDotRenderer = _TunerDot.GetComponent<SpriteRenderer>();

            _isInitialized = true;
        }

        public void SetCounts(int vertical, int horizontal)
        {
            _VerticalLayout.SetCount(vertical);
            _HorizontalLayout.SetCount(horizontal);
        }

        public void SetColor(Color color)
        {
            _VerticalLayout.SetColor(color);
            _HorizontalLayout.SetColor(color);
            _TunerDotRenderer.color = color;
        }

        public void Hide()
        {
            _VerticalLayout.Hide();
            _HorizontalLayout.Hide();
            _TunerDot.SetActive(false);
        }

        public void Show()
        {
            _VerticalLayout.Show();
            _HorizontalLayout.Show();
            _TunerDot.SetActive(true);
        }
    }
}
