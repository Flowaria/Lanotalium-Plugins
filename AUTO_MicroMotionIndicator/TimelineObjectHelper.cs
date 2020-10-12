using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AUTO_MicroMotionIndicator
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    public class TimelineObjectHelper : MonoBehaviour
    {
        public GameObject Marker;
        public Button MarkerButton;

        private RectTransform _Rect;
        private Button _Button;

        private bool _Enabled = true;

        void Start()
        {
            _Rect = GetComponent<RectTransform>();
            _Button = GetComponent<Button>();
            MarkerButton.onClick.AddListener(OnClickTimeline);

            StartCoroutine(UpdateActive());
        }

        void OnClickTimeline()
        {
            LimOperationManager.Instance.OnTimeLineClick(this.gameObject.GetInstanceID());
        }


        IEnumerator UpdateActive()
        {
            while(true)
            {
                if (_Rect.rect.size.x < 10.0f)
                {
                    if(!_Enabled)
                    {
                        Marker.SetActive(true);
                        _Enabled = true;
                    }
                }
                else
                {
                    if (_Enabled)
                    {
                        Marker.SetActive(false);
                        _Enabled = false;
                    }
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
}
