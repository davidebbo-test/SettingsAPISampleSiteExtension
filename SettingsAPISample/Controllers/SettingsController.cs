﻿using System;
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
        public ARMListEntry<SettingEntry> Get()
        {
            return ARMListEntry<SettingEntry>.Create(SettingsStore.Instance.Load(), Request);
        }

        // GET settings/foo
        public ARMEntry<SettingEntry> Get(string name)
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

            return ARMEntry<SettingEntry>.Create(entry, Request);
        }

        // PUT settings/foo
        public ARMEntry<SettingEntry> Put(string name, [FromBody]ARMEntry<SettingEntry> armEntry)
        {
            var settings = SettingsStore.Instance.Load();
            SettingEntry existingEntry = settings.FirstOrDefault(e => e.Name == name);
            if (existingEntry != null)
            {
                settings.Remove(existingEntry);
            }

            armEntry.Properties.Name = name;
            settings.Add(armEntry.Properties);
            settings.Sort((s1, s2) => s1.Name.CompareTo(s2.Name));

            SettingsStore.Instance.Save(settings);

            // Return the newly created entry
            return ARMEntry<SettingEntry>.Create(armEntry.Properties, Request);
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
