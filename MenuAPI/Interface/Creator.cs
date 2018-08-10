using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShortCutAPI.Interface
{
    public enum AlignType
    {
        Descending,
        Ascending,
        NotCare
    }

    public interface ICreatorButton
    {
        string DefaultName(Language language);
        IEnumerator OnClick(LanotaliumContext context, GameObject Buttonobj);
    }

    public interface ICreatorButtonHotKey
    {
        string DefaultName(Language language);
        string HotKeyName(Language language);
        IEnumerator OnClick(LanotaliumContext context, GameObject Buttonobj);
    }

    public interface ICreatorButtonNumber
    {
        string DefaultName(Language language);
        string PlaceHolderName(Language language);
        IEnumerator OnClick(LanotaliumContext context, GameObject Buttonobj, GameObject NumberObj);
    }

    public class InterfaceCreatorManager
    {
        public AlignType NameAlign { get; set; }
        public AlignType TypeAlign { get; set; }

        public void AddButton(ICreatorButton button)
        {

        }

        public void AddButton(ICreatorButtonHotKey button)
        {

        }

        public void AddButton(ICreatorButtonNumber button)
        {

        }

        public void UpdateUI()
        {

        }
    }
}
