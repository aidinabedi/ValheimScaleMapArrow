using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

[BepInPlugin(Manifest.ProjectName, Manifest.Product, Manifest.Version)]
[BepInProcess("valheim.exe")]
public class ScaleMapArrowPlugin : BaseUnityPlugin
{
	private readonly Harmony harmony = new Harmony(Manifest.ProjectName);

	private static ConfigEntry<float> minimapScale;

	private static ConfigEntry<float> largemapScale;

	private void Awake()
	{
		minimapScale = Config.Bind("General", "Minimap Local Player Scale", 1.5f);
		largemapScale = Config.Bind("General", "Largemap Local Player Scale", 1.5f);
		minimapScale.SettingChanged += Scale_SettingChanged;
		largemapScale.SettingChanged += Scale_SettingChanged;
		harmony.PatchAll(typeof(ScaleMapArrowPlugin));
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Minimap), "Start")]
	private static void MinimapStartPostfix(Minimap __instance)
	{
		__instance.m_smallMarker.localScale = new Vector3(minimapScale.Value, minimapScale.Value, minimapScale.Value);
		__instance.m_largeMarker.localScale = new Vector3(largemapScale.Value, largemapScale.Value, largemapScale.Value);
	}

	private void Scale_SettingChanged(object sender, EventArgs e)
	{
		if ((bool)Minimap.instance)
		{
			Minimap.instance.m_smallMarker.localScale = new Vector3(minimapScale.Value, minimapScale.Value, minimapScale.Value);
			Minimap.instance.m_largeMarker.localScale = new Vector3(largemapScale.Value, largemapScale.Value, largemapScale.Value);
		}
	}
}
