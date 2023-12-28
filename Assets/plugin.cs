using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;        // <-- Not required if using monomod instead
using LethalLib.Modules; // <-- I'm using this because of NetworkPrefabs
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Netcode.Components; // <-- Only required for NetworkPrefab (not shown)
using UnityEngine;

namespace Rethunk_Company
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "tech.rethunk.src.lc_rethunk_company";
        private const string PLUGIN_NAME = "RTX 3080 Scrap";
        private const string PLUGIN_VERSION = "1.0";

        public static BepInEx.Logging.ManualLogSource LogSource { get; private set; }

        public static ConfigFile config;

        internal static Plugin __instance;

        // I haven't switched to monomod yet..... also, this isn't required for a scrap-only mod
        private Harmony harmony;

        #region Unity Methods
        private void Awake()
        {
            LogSource = Logger;
            config = Config;

            harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            TryLoadAssets();

            LogSource.LogInfo($"Plugin {PLUGIN_GUID} was loaded!");
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
            MainAssets.Unload(true);

            LogSource.LogInfo($"Plugin {PLUGIN_GUID} was unloaded!");
        }
        #endregion

        public static AssetBundle MainAssets;

        // Actual AssetBundle loading code here
        public void TryLoadAssets()
        {
            if (MainAssets == null)
            {
                MainAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location), "your_asset_bundle_name_and_extension"));
                LogSource.LogInfo("Loaded asset bundle");
            }
        }
    }
}