using BepInEx.Logging;
using BepInEx.Unity.Mono;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using Game.Prefabs;
using System.Collections.Generic;

namespace Wayz.CS2.EducationBalancer;

[BepInPlugin("Wayz.CS2.EducationBalancerMod", "EducationBalancer", "0.0.1")]
public class EducationBalancerMod : BaseUnityPlugin
{
    public static ManualLogSource GameLogger = null!;

    private void Awake()
    {
        GameLogger = Logger;
        var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID + "_Cities2Harmony");

        var patchedMethods = harmony.GetPatchedMethods();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION} is loaded! Patched methods: " + patchedMethods.Count());

        foreach (var patchedMethod in patchedMethods)
        {
            Logger.LogInfo($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
        }
    }
}

[HarmonyPatch("Game.Prefabs.PrefabSystem", "AddPrefab")]
public static class SchoolPatch_Costs
{
    private static readonly Dictionary<string, (int, int)> _costAndCapacity = new()
    {
        { "ElementarySchool01", (50000, 2000) },
        { "ElementarySchool01 Extension Wing", (20000, 1000) },
        { "University01", (375000, 2500) },
        { "University01 Extension Wing", (125000, 1000) },
        { "College01 Extension Wing", (110000, 1000) }
    };

    [HarmonyPrefix]
    public static bool Prefix(object __instance, PrefabBase prefab)
    {
        if (_costAndCapacity.TryGetValue(prefab.name, out var pair))
        {
            (var cost, var capacity) = pair;

            var costComponent = prefab.GetComponent<ServiceConsumption>();
            costComponent.m_Upkeep = cost;

            var schoolComponent = prefab.GetComponent<School>();
            schoolComponent.m_StudentCapacity = capacity;
        }
        return true;
    }
}