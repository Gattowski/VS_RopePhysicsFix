using HarmonyLib;
using System.Linq;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace RopePhysicsFix
{
    [HarmonyPatch(typeof(ClothPoint), "update")]
    public static class ClothPoint_Update_Patch
    {
        static bool Prefix(ClothPoint __instance, float dt)
        {
            var csField = typeof(ClothPoint).GetField("cs", BindingFlags.NonPublic | BindingFlags.Instance);
            var cs = csField?.GetValue(__instance) as ClothSystem;

            if (cs == null || cs.api?.World == null)
            {
                return true;
            }

            var world = cs.api.World;

            // Create our proxy and copy values
            ClothPointNew proxy = new ClothPointNew(cs);
            CopyFields(__instance, proxy);

            proxy.update(dt, world);

            

            return false;
        }

        private static void CopyFields(ClothPoint from, ClothPointNew to)
        {
            var fields = typeof(ClothPoint).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var toFields = typeof(ClothPointNew).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var toField = toFields.FirstOrDefault(f => f.Name == field.Name && f.FieldType == field.FieldType);
                if (toField != null)
                {
                    toField.SetValue(to, field.GetValue(from));
                }
            }
        }
    }
}

