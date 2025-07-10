using HarmonyLib;
using Vintagestory.API.Common;

namespace RopePhysicsFix
{
    public class RopePhysicsFixModSystem : ModSystem
    {
        private Harmony harmony;

        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            if (harmony == null)
            {
                harmony = new Harmony("ropephysicsfix.clothpoint");
                harmony.PatchAll();
                api.Logger.Notification("RopePhysicsFix: Harmony patches applied.");
            }
        }

        public override void Dispose()
        {
            if (harmony != null)
            {
                harmony.UnpatchAll("ropephysicsfix.clothpoint");
                harmony = null;
            }
        }
    }
}

