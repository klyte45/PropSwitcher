﻿using ColossalFramework.UI;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using Klyte.PropSwitcher.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Klyte.PropSwitcher.Data
{
    public class PSPropData : DataExtensionBase<PSPropData>
    {
        [XmlElement("Entries")]
        public XmlDictionary<PrefabChildEntryKey, SwitchInfo> Entries { get; set; } = new XmlDictionary<PrefabChildEntryKey, SwitchInfo>();
        [XmlElement("PrefabChildEntries")]
        public SimpleXmlDictionary<string, XmlDictionary<PrefabChildEntryKey, SwitchInfo>> PrefabChildEntries { get; set; } = new SimpleXmlDictionary<string, XmlDictionary<PrefabChildEntryKey, SwitchInfo>>();



        public override string SaveId { get; } = "K45_PS_BasicData";

        public override void AfterDeserialize(PSPropData loadedData)
        {
            if (loadedData != null)
            {
                var keysToRemove = new List<PrefabChildEntryKey>();
                loadedData.Entries.ForEach(x =>
                {
                    if (x.Value == null || x.Value.SwitchItems.Length == 0)
                    {
                        if (x.Value?.LegacyLoaded ?? false)
                        {
                            x.Value.SwitchItems = new SwitchInfo.Item[] {
                                new SwitchInfo.Item()
                            };
                        }
                        else
                        {
                            keysToRemove.Add(x.Key);
                        }
                    }

                });
                keysToRemove.ForEach(x => loadedData.Entries.Remove(x));
                var keyEntryToRemove = new List<Tuple<string, PrefabChildEntryKey>>();
                loadedData.PrefabChildEntries.ForEach(x => x.Value.ForEach(y =>
                 {
                     if (y.Value == null || y.Value.SwitchItems.Length == 0)
                     {
                         if (y.Value?.LegacyLoaded ?? false)
                         {
                             y.Value.SwitchItems = new SwitchInfo.Item[] {
                                new SwitchInfo.Item()
                             };
                         }
                         else
                         {
                             keyEntryToRemove.Add(Tuple.New(x.Key, y.Key));
                         }
                     }

                 })
                );
                keyEntryToRemove.ForEach(x => loadedData.PrefabChildEntries[x.First].Remove(x.Second));
            }
        }
    }
}
