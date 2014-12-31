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
        // GET settings
        public List<SettingEntry> Get()
        {
            return SettingsStore.Instance.Load();
        }

        // GET settings/foo
        public SettingEntry Get(string name)
        {
            var settings = SettingsStore.Instance.Load();

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
            var settings = SettingsStore.Instance.Load();
            SettingEntry existingEntry = settings.FirstOrDefault(e => e.Name == name);
            if (existingEntry != null)
            {
                settings.Remove(existingEntry);
            }

            entry.Name = name;
            settings.Add(entry);
            settings.Sort((s1, s2) => s1.Name.CompareTo(s2.Name));

            SettingsStore.Instance.Save(settings);
        }

        // DELETE settings/foo
        public void Delete(string name)
        {
            var settings = SettingsStore.Instance.Load();

            SettingEntry entry = settings.FirstOrDefault(e => e.Name == name);

            if (entry != null)
            {
                settings.Remove(entry);

                SettingsStore.Instance.Save(settings);
            }
        }
    }
}
