using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json;
using SettingsAPISample.Models;

namespace SettingsAPISample.Controllers
{
    public class SettingsController : ApiController
    {
        internal List<SettingEntry> ReadSettings()
        {
            string file = GetSettingsFilePath();
            if (File.Exists(file))
            {
                return JsonConvert.DeserializeObject<List<SettingEntry>>(File.ReadAllText(file));
            }
            else
            {
                return new List<SettingEntry>();
            }
        }

        internal void SaveSettings(List<SettingEntry> settings)
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
        public List<SettingEntry> Get()
        {
            string qqq = "{ Name:'foo', Value:'Some value' }";
            var q = JsonConvert.DeserializeObject<SettingEntry>(qqq);
            Console.WriteLine(q);



            return ReadSettings();
        }

        // GET settings/foo
        public SettingEntry Get(string name)
        {
            var settings = ReadSettings();

            SettingEntry entry = settings.FirstOrDefault(e => e.Name == name);
            if (entry == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No setting with name = {0}", name)),
                    ReasonPhrase = "Settings name Not Found"
                };

                throw new HttpResponseException(resp);
            }

            return entry;
        }

        // PUT settings/foo
        public void Put(string name, [FromBody]SettingEntry entry)
        {
            var settings = ReadSettings();
            SettingEntry existingEntry = settings.FirstOrDefault(e => e.Name == name);
            if (existingEntry != null)
            {
                existingEntry.Value = entry.Value;
            }
            else
            {
                entry.Name = name;
                settings.Add(entry);
                settings.Sort((s1, s2) => s1.Name.CompareTo(s2.Name));
            }

            SaveSettings(settings);
        }

        // DELETE settings/foo
        public void Delete(string name)
        {
            var settings = ReadSettings();

            SettingEntry entry = settings.FirstOrDefault(e => e.Name == name);

            if (entry != null)
            {
                settings.Remove(entry);

                SaveSettings(settings);
            }
        }
    }
}
