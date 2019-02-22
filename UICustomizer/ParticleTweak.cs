using EasyRequest;
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
using UnityEngine.UI;

namespace Flowaria.Lanotalium.Plugin.Tweak
{

   

    public class AskForParticleSetting
    {
        [Name("Use Particle effect")]
        public bool UseClickParticle = true;

        [Name("Show Combo count when judge text shows")]
        public bool UseComboCount = true;
    }

    public class ParticleTweak : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "UiTweak - Particle Effects";
        }

        public string Description(Language language)
        {
            return "Add Particle effect when note hit";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (context.IsProjectLoaded)
            {
                Request<AskForParticleSetting> request = new Request<AskForParticleSetting>();
                yield return context.UserRequest.Request(request, "Particle Setting");
                if (request.Succeed)
                {
                    GameObject i = null;
                    ParticleManager mgr = null;
                    if ((i = GameObject.Find("ParticleManagerObject")) == null)
                    {
                        var r = new Resources.ParticleResources();

                        yield return ResourceBundle.LoadFromBundle<Resources.ParticleResources>(Application.streamingAssetsPath + "/Assets/uitweak/uitweak.particle", x => r = x);
                        ComboTextControl.kazo = r.Font_Kawoszeh;

                        i = new GameObject();
                        i.name = "ParticleManagerObject";
                        i.AddComponent<ParticleManager>();
                        mgr = i.GetComponent<ParticleManager>();
                        mgr.r = r;
                        mgr.c = context;
                        mgr.Tuner = GameObject.Find("LimTunerManager/Tuner");

                        var combo = GameObject.Instantiate(r.Prefab_ComboTextCluster, mgr.Tuner.transform, false);
                        mgr.ComboText_0 = combo.transform.Find("0").gameObject.AddComponent<ComboTextControl>();
                        mgr.ComboText_45 = combo.transform.Find("45").gameObject.AddComponent<ComboTextControl>();
                        mgr.ComboText_90 = combo.transform.Find("90").gameObject.AddComponent<ComboTextControl>();
                        mgr.ComboText_135 = combo.transform.Find("135").gameObject.AddComponent<ComboTextControl>();
                        mgr.ComboText_180 = combo.transform.Find("180").gameObject.AddComponent<ComboTextControl>();
                        mgr.ComboText_225 = combo.transform.Find("225").gameObject.AddComponent<ComboTextControl>();
                        mgr.ComboText_270 = combo.transform.Find("270").gameObject.AddComponent<ComboTextControl>();
                        mgr.ComboText_315 = combo.transform.Find("315").gameObject.AddComponent<ComboTextControl>();
                        
                    }
                    else
                    {
                        mgr = i.GetComponent<ParticleManager>();
                    }

                    mgr.UseNoteParticle = request.Object.UseClickParticle;
                    mgr.UseComboCounter = request.Object.UseComboCount;
                    
                }
            }
            yield return null;
        }
    }

    public class ParticleManager : MonoBehaviour
    {
        public Resources.ParticleResources r;
        public LanotaliumContext c;
        public GameObject Tuner;
        public bool UseNoteParticle;
        public bool UseComboCounter;

        void Update()
        {
            if (UseNoteParticle || UseComboCounter)
            {
                UpdateEffect();
            }
        }

        #region Particle Effect
        int PreviousNoteCount = 0;
        Dictionary<LanotaTapNote, bool> ShouldTabNoteEffect = new Dictionary<LanotaTapNote, bool>();
        Dictionary<LanotaHoldNote, bool> ShouldHoldNoteEffect = new Dictionary<LanotaHoldNote, bool>();
        Dictionary<LanotaHoldNote, bool> ShouldHoldNoteEffectOnce = new Dictionary<LanotaHoldNote, bool>();
        List<LanotaTapNote> EffectTapNote = new List<LanotaTapNote>();
        List<LanotaHoldNote> EffectHoldNote = new List<LanotaHoldNote>();
        void UpdateEffect()
        {
            float ChartTime = c.TunerManager.ChartTime;

            EffectTapNote.Clear();
            EffectHoldNote.Clear();

            int NoteCount = 0;
            foreach (var note in c.TunerManager.TapNoteManager.TapNote)
            {
                if (!ShouldTabNoteEffect.ContainsKey(note))
                    ShouldTabNoteEffect.Add(note, true);

                if (note.Time < ChartTime)
                    NoteCount++;

                if (ShouldTabNoteEffect[note])
                {
                    if (ChartTime - note.Time >= 0.0f)
                    {
                        if(UseNoteParticle) EmitTapNoteEffect(note);
                        if (UseComboCounter) EffectTapNote.Add(note);
                        ShouldTabNoteEffect[note] = false;
                    }
                }
                else if (note.Time > ChartTime)
                {
                    ShouldTabNoteEffect[note] = true;
                }
            }

            foreach (var note in c.TunerManager.HoldNoteManager.HoldNote)
            {
                if (!ShouldHoldNoteEffect.ContainsKey(note))
                    ShouldHoldNoteEffect.Add(note, true);

                if (!ShouldHoldNoteEffectOnce.ContainsKey(note))
                    ShouldHoldNoteEffectOnce.Add(note, true);

                if (note.Time < ChartTime)
                    NoteCount += CalculateHoldNoteCount(note, ChartTime);

                if (ShouldHoldNoteEffect[note])
                {
                    if (ChartTime - note.Time >= 0.0f)
                    {
                        if(UseNoteParticle && ShouldHoldNoteEffectOnce[note])
                        {
                            CreateParticle(r.Prefab_ShockwaveHoldStart, note.HoldNoteGameObject, Tuner.transform);
                            SetParticleDelay(CreateParticle(r.Prefab_ShockwaveHoldEnd, note.HoldNoteGameObject, Tuner.transform), note.Duration);
                            SetParticleDuration(CreateParticle(r.Prefab_ShockwaveHoldMiddle, note.HoldNoteGameObject, Tuner.transform), note.Duration, true);
                            SetParticleDuration(CreateParticle(r.Prefab_SparkleHold, note.HoldNoteGameObject, Tuner.transform), note.Duration);
                            ShouldHoldNoteEffectOnce[note] = false;
                        }
                        if(UseComboCounter) EffectHoldNote.Add(note);

                        if (ChartTime - (note.Time + note.Duration) >= 0.0f)
                            ShouldHoldNoteEffect[note] = false;
                    }
                }
                else if (note.Time > ChartTime)
                {
                    ShouldHoldNoteEffect[note] = true;
                    ShouldHoldNoteEffectOnce[note] = true;
                }
            }

            if (UseComboCounter && NoteCount != PreviousNoteCount)
            {
                foreach (var note in EffectTapNote)
                {
                    ReportCombo(note.Degree, ++PreviousNoteCount);
                }
                foreach (var note in EffectHoldNote)
                {
                    ReportCombo(note.HoldNoteGameObject.transform.localEulerAngles.y - c.TunerManager.CameraManager.CurrentRotation, ++PreviousNoteCount);
                }
                PreviousNoteCount = NoteCount;
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
        #endregion

        #region Combo Effect
        bool Approximately(float a, float b)
        {
            if (Mathf.Abs(a - b) < 0.0003f) return true;
            return false;
        }

        int CalculateHoldNoteCount(LanotaHoldNote Hold, float Time)
        {
            int NoteCount = 0;
            float Duration = (Hold.Time + Hold.Duration) < Time ? Hold.Duration : (Time - Hold.Time);
            float JudgeDelta = 30f / c.TunerManager.BpmManager.CalculateBpm(Hold.Time);
            while (NoteCount * JudgeDelta < Duration && !Approximately(NoteCount * JudgeDelta, Duration))
                NoteCount++;
            if (Time > Hold.Time + Hold.Duration) NoteCount += 1;
            return NoteCount;
        }

        public ComboTextControl ComboText_0, ComboText_45, ComboText_90, ComboText_135, ComboText_180, ComboText_225, ComboText_270, ComboText_315;
        void ReportCombo(float angle, int combo)
        {
            float nrmAngle = angle % 360;
            float absAngle = (nrmAngle < 0) ? 360 + nrmAngle : nrmAngle;

            ComboTextControl ComboText = null;
            if (absAngle > 337.5 || absAngle <= 22.5)
            {
                ComboText = ComboText_0;
            }
            else if (22.5 < absAngle && absAngle <= 67.5)
            {
                ComboText = ComboText_45;
            }
            else if (67.5 < absAngle && absAngle <= 112.5)
            {
                ComboText = ComboText_90;
            }
            else if (112.5 < absAngle && absAngle <= 157.5)
            {
                ComboText = ComboText_135;
            }
            else if (157.5 < absAngle && absAngle <= 202.5)
            {
                ComboText = ComboText_180;
            }
            else if (202.5 < absAngle && absAngle <= 247.5)
            {
                ComboText = ComboText_225;
            }
            else if (247.5 < absAngle && absAngle <= 292.5)
            {
                ComboText = ComboText_270;
            }
            else if (292.5 < absAngle && absAngle <= 337.5)
            {
                ComboText = ComboText_315;
            }

            if (ComboText != null)
            {
                ComboText.Show(combo);
            }
        }
        #endregion
    }

    public class ComboTextControl : MonoBehaviour
    {
        public TextMesh Txt;
        public Animator Anime;
        public static Font kazo; 
        public void Start()
        {
            Anime = GetComponent<Animator>();
            Txt = transform.Find("Text").gameObject.GetComponent<TextMesh>();
            Txt.font = kazo;
        }

        public void Show(int combo)
        {
            Txt.text = combo.ToString();
            Anime.Play(0);
        }
    }

    public class FollowGameObject : MonoBehaviour
    {
        public GameObject Parent;
        void Update()
        {
            if (Parent != null)
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
