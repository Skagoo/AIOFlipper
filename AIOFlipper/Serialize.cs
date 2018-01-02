using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AIOFlipper
{
    public static class Serialize
    {
        public static string ToJson(this Account[] self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this Item[] self) => JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this Sale self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
