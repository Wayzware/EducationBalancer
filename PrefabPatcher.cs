using Game.Prefabs;
using HarmonyLib;

namespace Wayz.CS2.SchoolCapacityBalancer;

[HarmonyPatch(typeof(PrefabSystem), "AddPrefab")]
public static class PrefabPatcher
{
    [HarmonyPrefix]
    public static bool Prefix(object __instance, PrefabBase prefab)
    {
        if (SchoolCapacityBalancer.SchoolOptions.TryGetValue(prefab.name, out var overrides))
        {
            if (overrides.UpkeepCost >= 0)
            {
                var costComponent = prefab.GetComponent<ServiceConsumption>();
                costComponent.m_Upkeep = overrides.UpkeepCost;
            }

            if (overrides.StudentCapacity >= 0)
            {
                var schoolComponent = prefab.GetComponent<School>();
                schoolComponent.m_StudentCapacity = overrides.StudentCapacity;
            }
        }
        return true;
    }
}