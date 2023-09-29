using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.config
{
    internal class caster_reader
    {
        public string token { get; set; }
        public string prefix { get; set; }

        public async Task readCaster()
        {
            // Access the Prefix and Token in the json File
            using (StreamReader Read_ME = new StreamReader("caster.json"))
            {
                // Read shits in th file
                string read_json = await Read_ME.ReadToEndAsync();  
                caster_structure caster_json = JsonConvert.DeserializeObject<caster_structure>(read_json);

                // Will push the token to access anywhere in the class files
                this.token = caster_json.token;
                this.prefix = caster_json.prefix;
            }
        }
    }

    // Push from line 25 & 26 to token & prefix
    internal sealed class caster_structure
    {
        public string token { get; set; }
        public string prefix { get; set; }

    }
}
