﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SettingsAPISample.Models
{
    public class ARMListEntry<T> where T : INamedObject
    {
        public static ARMListEntry<T> Create(IEnumerable<T> objects, HttpRequestMessage request)
        {
            return new ARMListEntry<T>
            {
                Value = objects.Select(entry => ARMEntry<T>.Create(entry, request, isChild: true))
            };
        }
        public IEnumerable<ARMEntry<T>> Value { get; set; }
    }
}
