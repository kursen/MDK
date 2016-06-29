using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloUIHelpers.Helpers
{
    public class JsonErrorObject
    {
        public static IEnumerable<System.Collections.Generic.KeyValuePair<string, string[]>> Convert(ModelStateDictionary model)
        {
            dynamic result = model.ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()).Where(k => k.Value.Count() > 0);
            return result;
        }
        public static IEnumerable<System.Collections.Generic.KeyValuePair<string, string[]>> Convert(Exception model)
        {
            Dictionary<string, string[]> rvalue = new Dictionary<string, string[]>();
            List<string> message = new List<string>();
            message.Add(model.Message);
            if (model.InnerException != null)
            {
                message.Add(model.InnerException.Message);
            }
            rvalue.Add("Error", message.ToArray());
            return rvalue.ToArray();
        }

        public int stat { get; set; }
        public IEnumerable<KeyValuePair<string, string[]>> message { get; set; }
    }
}