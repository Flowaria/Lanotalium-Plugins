using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace GuideLines
{
    public class LineLayoutManager : MonoBehaviour
    {
        public static readonly int MAXLINECOUNT = 64;
        public static readonly string DUMMYLINENAME = "Dummy Line ";

        public GameObject StartLine, EndLine;
        public Image StartLineImage, EndLineImage;
        public RectTransform Layout;

        public GameObject[] LineObjects = new GameObject[MAXLINECOUNT];
        public Image[] LineImages = new Image[MAXLINECOUNT];

        public string LinePrefix = "VerticalLine";

        public int Count { get; private set; }

        public void Init()
        {
            Layout = GetComponent<RectTransform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var name = child.name;
                if (name.StartsWith(DUMMYLINENAME))
                {
                    if(name.EndsWith("(Start)"))
                    {
                        StartLine = child.gameObject;
                        StartLineImage = child.GetComponent<Image>();
                    }
                    else if(name.EndsWith("(End)"))
                    {
                        EndLine = child.gameObject;
                        EndLineImage = child.GetComponent<Image>();
                    }
                }
                else if(name.StartsWith(LinePrefix))
                {
                    var numberString = name.Replace(LinePrefix, "");
                    numberString = numberString.Substring(1, numberString.Length - 2);

                    if(int.TryParse(numberString, out int number))
                    {
                        if(0 < number && number <= MAXLINECOUNT)
                        {
                            LineObjects[number - 1] = child.gameObject;
                            LineImages[number - 1] = child.GetComponent<Image>();
                        }
                    }
                }
            }
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < MAXLINECOUNT; i++)
            {
                LineImages[i].color = color;
            }
        }

        public void SetCount(int count)
        {
            if(Count == count)
            {
                return;
            }

            Count = count;

            for(int i = 0;i<MAXLINECOUNT;i++)
            {
                //active when it's within count
                LineObjects[i].SetActive(i < count);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(Layout);
        }

        public void Hide()
        {
            ChangeAllLineDisplay(false);

            LayoutRebuilder.ForceRebuildLayoutImmediate(Layout);
        }

        public void Show()
        {
            ChangeAllLineDisplay(true);
        }

        private void ChangeAllLineDisplay(bool display)
        {
            for (int i = 0; i < Count; i++)
            {
                LineObjects[i].SetActive(display);
            }

            StartLine.SetActive(display);
            EndLine.SetActive(display);
        }
    }
}