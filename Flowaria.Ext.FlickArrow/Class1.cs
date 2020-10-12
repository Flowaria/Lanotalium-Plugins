using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Ext.FlickArrow
{
    public class FlickArrowEffect : MonoBehaviour
    {
        public float scrollSpeed = 1.5F;
        private Renderer rend;
        private float randOffset = 0;
        void Start()
        {
            rend = GetComponent<Renderer>();
            randOffset = UnityEngine.Random.Range(-1.0f, 1.0f);
        }
        void Update()
        {
            float offset = Time.time * scrollSpeed * 1.3f + randOffset;
            rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }
    }
}
