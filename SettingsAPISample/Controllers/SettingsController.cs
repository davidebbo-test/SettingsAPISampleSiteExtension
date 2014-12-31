using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json;

namespace SettingsAPISample.Controllers
{
    public class SettingsController : ApiController
    {
        internal Dictionary<string, string> ReadSettings()
        {
            string file = GetSettingsFilePath();
            if (File.Exists(file))
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(file));
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        internal void SaveSettings(Dictionary<string, string> settings)
        {
            string file = GetSettingsFilePath();
            File.WriteAllText(file, JsonConvert.SerializeObject(settings));
        }

        string GetSettingsFilePath()
        {
            string folder = HostingEnvironment.MapPath("~/App_Data");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return Path.Combine(folder, "settings.json");
        }

        // GET settings
        public Dictionary<string, string> Get()
        {
            return ReadSettings();
        }

        // GET settings/foo
        public string Get(string id)
        {
            var settings = ReadSettings();

            string val;
            if (!settings.TryGetValue(id, out val))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No setting with ID = {0}", id)),
                    ReasonPhrase = "Settings ID Not Found"
                };

                throw new HttpResponseException(resp);
            }

            return val;
        }

        // PUT settings/foo
        public void Put(string id, [FromBody]string value)
        {
            var settings = ReadSettings();
            settings[id] = value;
            SaveSettings(settings);
        }

        // DELETE settings/foo
        public void Delete(string id)
        {
            var settings = ReadSettings();
            if (settings.Remove(id))
            {
                SaveSettings(settings);
            }
        }
    }
}
