﻿using ColossalFramework;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Klyte.PropSwitcher.Xml
{
    public class SwitchInfo
    {
        [XmlAttribute("TargetPrefab")]
        [Obsolete]
        public string Old_TargetPrefab
        {
            get => null;
            set {
                if (!value.IsNullOrWhiteSpace())
                {
                    SwitchItems = SwitchItems.Where(x => x.TargetPrefab != value).Union(new Item[] { new Item { TargetPrefab = value } }).ToArray();
                }
            }
        }

        [XmlElement("SwitchItem")]
        public Item[] SwitchItems { get; set; } = new Item[0];
        [XmlIgnore]
        internal string m_fileSource;

        public class Item
        {
            [XmlAttribute("targetPrefab")]
            public string TargetPrefab { get; set; }
            [XmlAttribute("weightDraw")]
            public ushort WeightInDraws { get; set; }
            [XmlAttribute("rotationOffset")]
            public float RotationOffset { get; set; }




            private PropInfo m_cachedPropInfo;
            private string m_lastTryTargetProp;

            public PropInfo CachedProp
            {
                get {
                    if (TargetPrefab != m_lastTryTargetProp)
                    {
                        m_cachedPropInfo = PrefabCollection<PropInfo>.FindLoaded(TargetPrefab ?? "");

                        m_lastTryTargetProp = TargetPrefab;
                    }
                    return m_cachedPropInfo;
                }
            }

            private TreeInfo m_cachedTreeInfo;
            private string m_lastTryTargetTree;

            public TreeInfo CachedTree
            {
                get {
                    if (TargetPrefab != m_lastTryTargetTree)
                    {
                        m_cachedTreeInfo = PrefabCollection<TreeInfo>.FindLoaded(TargetPrefab ?? "");

                        m_lastTryTargetTree = TargetPrefab;
                    }
                    return m_cachedTreeInfo;
                }
            }
        }

        internal void Add(string newPrefab, float rotationOffset) => SwitchItems = SwitchItems.Where(x => x.TargetPrefab != newPrefab).Union(new Item[] { new Item { TargetPrefab = newPrefab, RotationOffset = rotationOffset } }).ToArray();
        internal void Remove(string prefabName) => SwitchItems = SwitchItems.Where(x => x.TargetPrefab != prefabName).ToArray();
    }
}