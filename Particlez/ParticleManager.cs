using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Events.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Flowaria.Lanotalium.Plugin
{

    public class ParticleManagerPreferences
    {
        [Name("Use Particle effect")]
        public bool UseClickParticle = true;

        [Name("Use Sound Effect for key pressed event")]
        public bool UseRailParticle = true;

        [Name("Use Sound Effect for key pressed event")]
        public bool UseFlickParticle = true;

        [Name("Show Combo count when judge text shows")]
        public bool UseComboCount = true;
    }

    public class GameObjectBool
    {
        public GameObject obj = null;
        public bool actived = false;

        public GameObjectBool(GameObject obj, bool actived)
        {
            this.obj = obj;
            this.actived = actived;
        }
    }

    public class ParticleManager : MonoBehaviour
    {
        public LanotaliumContext context;
        public ParticleManagerPreferences pref;

        public GameObject PrefabHoldSparkle;

        

        void Start()
        {
            
        }

        void Update()
        {
            UpdateTapNote();
            UpdateHoldNote();
        }

        void UpdateTapNote()
        {
            foreach (var note in context.TunerManager.TapNoteManager.TapNote)
            {
                
            }
        }

        void OnTapNoteClicked(LanotaTapNote note)
        {

        }

        void OnHoldNoteClicked(LanotaHoldNote note)
        {

        }

        public Dictionary<LanotaHoldNote, GameObjectBool> pHoldSparkle = new Dictionary<LanotaHoldNote, GameObjectBool>();
        void UpdateHoldNote()
        {
            foreach (var note in context.TunerManager.HoldNoteManager.HoldNote)
            {
                if (!note.shouldUpdate)
                    continue;

                float time = context.TunerManager.ChartTime;
                bool playing = context.TunerManager.MediaPlayerManager.IsPlaying;


                if (note.shouldUpdate && note.StartEffectPlayed)
                {
                    if(!pHoldSparkle.ContainsKey(note))
                    {
                        var particle = GameObject.Instantiate(PrefabHoldSparkle, note.HoldNoteGameObject.transform.Find("Note"), false);
                        pHoldSparkle.Add(note, new GameObjectBool(particle, true));
                    }
                }
                else if (note.shouldUpdate && note.EndEffectPlayed)
                {
                    if(pHoldSparkle.ContainsKey(note))
                    {
                        GameObject.Destroy(pHoldSparkle[note].obj);
                        pHoldSparkle.Remove(note);
                    }
                }
                else if(!note.shouldUpdate)
                {
                    if (pHoldSparkle.ContainsKey(note))
                    {
                        GameObject.Destroy(pHoldSparkle[note].obj);
                        pHoldSparkle.Remove(note);
                    }
                }
            }
        }
    }
}
