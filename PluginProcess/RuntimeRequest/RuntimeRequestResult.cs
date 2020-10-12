using System;
using System.Collections.Generic;
using System.Text;

namespace Lanotalium.Plugin.Simple.RTRequest
{
    public class RuntimeRequestResult
    {
        public bool Success { get; private set; }
        private object _Context;

        internal RuntimeRequestResult(bool result, object obj)
        {
            Success = result;
            _Context = obj;
        }

        public object ReadField(string name)
        {
            var field = _Context.GetType().GetField(name);

            if (field != null)
            {
                return field.GetValue(_Context);
            }
            else
            {
                return null;
            } 
        }
    }
}
