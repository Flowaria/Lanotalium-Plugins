using Lanotalium.Chart;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TempParticle
{
    public class ParticleResources
    {
        [ResourceName("SparkleFlickOut")]
        public GameObject Prefab_SparkleFlickOut;

        [ResourceName("SparkleFlickIn")]
        public GameObject Prefab_SparkleFlickIn;

        [ResourceName("SparkleHold")]
        public GameObject Prefab_SparkleHold;

        [ResourceName("ShockwaveClick")]
        public GameObject Prefab_ShockwaveClick;

        [ResourceName("ShockwaveHoldStart")]
        public GameObject Prefab_ShockwaveHoldStart;

        [ResourceName("ShockwaveHoldMiddle")]
        public GameObject Prefab_ShockwaveHoldMiddle;

        [ResourceName("ShockwaveHoldEnd")]
        public GameObject Prefab_ShockwaveHoldEnd;

        [ResourceName("harmonyeffect")]
        public GameObject Prefab_harmonyeffect;
    }


    public class OrnamentResources
    {
        [ResourceName("JudgeLineGlow")]
        public GameObject Prefab_JudgeGlow;

        [ResourceName("JudgeLineOrnament")]
        public GameObject Prefab_JudgeOrnament;
    }

    public class TempParticle3 : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Temp";
        }

        public string Name(Language language)
        {
            return "Tuner Ornament";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (context.IsProjectLoaded)
            {
                var judgeline = GameObject.Find("Tuner/JudgeLine");
                if (judgeline != null)
                {
                    OrnamentResources r = new OrnamentResources();
                    yield return ResourceBundle.LoadFromBundle<OrnamentResources>(Application.streamingAssetsPath + "/Assets/uitweak/uitweak.screen", x => r = x, context);

                    GameObject.Instantiate(r.Prefab_JudgeGlow, judgeline.transform).AddComponent<GlowManager>().c = context;
                    GameObject.Instantiate(r.Prefab_JudgeOrnament, judgeline.transform);
                }
                else
                    context.MessageBox.ShowMessage("no judgeline");
            }
            
            yield return null;
        }
    }

    public class TempParticle : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Temp";
        }

        public string Name(Language language)
        {
            return "No time for particle (only click once please)";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if(context.IsProjectLoaded)
            {
                ParticleResources r = new ParticleResources();
                yield return ResourceBundle.LoadFromBundle<ParticleResources>(Application.streamingAssetsPath + "/Assets/uitweak/uitweak.particle", x => r = x, context);

                GameObject i = null;
                if ((i = GameObject.Find("ParticleManagerObject")) != null)
                {
                    GameObject.Destroy(i);
                }
                GameObject mpObject = new GameObject();
                mpObject.name = "ParticleManagerObject";
                mpObject.AddComponent<ParticleManager>();
                var mgr = mpObject.GetComponent<ParticleManager>();
                mgr.r = r;
                mgr.c = context;
                mgr.Tuner = GameObject.Find("LimTunerManager/Tuner");
            }
            yield return null;
        }
    }

    public class GlowManager : MonoBehaviour
    {
        public LanotaliumContext c;
        void Update()
        {
            gameObject.GetComponent<Animator>().speed = c.TunerManager.BpmManager.CurrentBpm / 60;
        }
    }

    public class ParticleManager : MonoBehaviour
    {
        public ParticleResources r;
        public LanotaliumContext c;
        public GameObject Tuner;

        void Start()
        {

        }

        void Update()
        {
            if(c.TunerManager.ScrollManager.CurrentScrollSpeed <= 100)
            {
                UpdateTapNote();
                UpdateHoldNote();
            }
        }

        Dictionary<GameObject, bool> ShouldTabNoteEffect = new Dictionary<GameObject, bool>();
        void UpdateTapNote()
        {
            foreach (var note in c.TunerManager.TapNoteManager.TapNote)
            {
                if(note.shouldUpdate && c.TunerManager.ScrollManager.CurrentScrollSpeed > 0.0f && c.TunerManager.MediaPlayerManager.IsPlaying)
                {
                    if (!ShouldTabNoteEffect.ContainsKey(note.TapNoteGameObject))
                    {
                        ShouldTabNoteEffect.Add(note.TapNoteGameObject, true);
                    }
                    else
                    {
                        if (note.Percent >= 100.0f && ShouldTabNoteEffect[note.TapNoteGameObject])
                        {
                            EmitTapNoteEffect(note);
                            ShouldTabNoteEffect[note.TapNoteGameObject] = false;
                        }

                    }
                }
                else if (ShouldTabNoteEffect.ContainsKey(note.TapNoteGameObject))
                {
                    ShouldTabNoteEffect.Remove(note.TapNoteGameObject);
                }
            }
        }

        void EmitTapNoteEffect(LanotaTapNote note)
        {
            //harmony particle
            Instantiate(r.Prefab_harmonyeffect, note.TapNoteGameObject.transform.position, note.TapNoteGameObject.transform.rotation, Tuner.transform).AddComponent<AnimationAutoDestroy>();

            if (note.Type != 4)
                Instantiate(r.Prefab_ShockwaveClick, note.TapNoteGameObject.transform.position, note.TapNoteGameObject.transform.rotation, Tuner.transform);
            if (note.Type == 2)
                Instantiate(r.Prefab_SparkleFlickIn, note.TapNoteGameObject.transform.position, note.TapNoteGameObject.transform.rotation, Tuner.transform);
            else if (note.Type == 3)
                Instantiate(r.Prefab_SparkleFlickOut, note.TapNoteGameObject.transform.position, note.TapNoteGameObject.transform.rotation, Tuner.transform);
        }

        Dictionary<GameObject, bool> ShouldHoldNoteEndEffect = new Dictionary<GameObject, bool>();
        Dictionary<GameObject, bool> ShouldHoldNoteStartEffect = new Dictionary<GameObject, bool>();
        void UpdateHoldNote()
        {
            foreach (var note in c.TunerManager.HoldNoteManager.HoldNote)
            {
                if(note.shouldUpdate && c.TunerManager.ScrollManager.CurrentScrollSpeed > 0.0f && c.TunerManager.MediaPlayerManager.IsPlaying)
                {
                    if (!ShouldHoldNoteStartEffect.ContainsKey(note.HoldNoteGameObject))
                    {
                        ShouldHoldNoteStartEffect.Add(note.HoldNoteGameObject, true);
                        ShouldHoldNoteEndEffect.Add(note.HoldNoteGameObject, true);
                    }
                    else
                    {
                        if(note.Percent >= 100.0f)
                        {
                            if (ShouldHoldNoteStartEffect[note.HoldNoteGameObject])
                            {
                                CreateParticle(r.Prefab_ShockwaveHoldStart, note.HoldNoteGameObject, Tuner.transform);
                                SetParticleDelay(CreateParticle(r.Prefab_ShockwaveHoldEnd, note.HoldNoteGameObject, Tuner.transform), note.Duration);
                                SetParticleDuration(CreateParticle(r.Prefab_ShockwaveHoldMiddle, note.HoldNoteGameObject, Tuner.transform), note.Duration, true);
                                SetParticleDuration(CreateParticle(r.Prefab_SparkleHold, note.HoldNoteGameObject, Tuner.transform), note.Duration);
                                ShouldHoldNoteStartEffect[note.HoldNoteGameObject] = false;
                            }
                        }
                        
                    }
                }
                else if(ShouldHoldNoteStartEffect.ContainsKey(note.HoldNoteGameObject))
                {
                    ShouldHoldNoteStartEffect.Remove(note.HoldNoteGameObject);
                    ShouldHoldNoteEndEffect.Remove(note.HoldNoteGameObject);
                }
            }
        }

        GameObject CreateParticle(GameObject particle, GameObject tracker, Transform parent)
        {
            var obj = Instantiate(particle, tracker.transform.position, tracker.transform.rotation, parent).gameObject;
            obj.AddComponent<FollowGameObject>().Parent = tracker;
            obj.GetComponent<ParticleSystem>().Play();
            return obj;
        }

        void SetParticleDuration(GameObject obj, float duration, bool force_off = false)
        {
            var ps = obj.GetComponent<ParticleSystem>();
            ps.Stop();

            var main = ps.main;
            main.duration = duration;
            main.loop = false;

            ps.Play();

            if (force_off)
                obj.AddComponent<ParticleSystemAutoDestroy>().delay = duration;
        }

        void SetParticleDelay(GameObject obj, float delay)
        {
            var ps = obj.GetComponent<ParticleSystem>();
            ps.Stop();

            var main = ps.main;
            main.startDelay = delay;
            main.loop = false;

            ps.Play();
        }
    }

    public class FollowGameObject : MonoBehaviour
    {
        public GameObject Parent;
        void Update()
        {
            if(Parent != null)
            {
                gameObject.transform.position = Parent.transform.position;
                gameObject.transform.rotation = Parent.transform.rotation;
            }
        }
    }

    public class AnimationAutoDestroy : MonoBehaviour
    {
        
        public void Start()
        {
            Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public class ParticleSystemAutoDestroy : MonoBehaviour
    {
        private ParticleSystem ps;
        public float delay = 0f;

        public IEnumerator Start()
        {
            ps = GetComponent<ParticleSystem>();
            yield return new WaitForSecondsRealtime(delay);
            GameObject.Destroy(gameObject);
        }

        public void Update()
        {
            if (ps)
            {
                if (!ps.IsAlive())
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
