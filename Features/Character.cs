using System;
using System.Reflection;
using UnityEngine;

namespace Obscura.TRDS.Features
{
    public static class Character
    {
        public static float newStrength = 200f;
        public static bool climbWallEnabled = false;
        public static bool invincibilityEnabled = false;

        public static void DrawCharacterTab(GameObject playerObject)
        {
            GUI.Label(new Rect(10, 70, 100, 20), "Strength:");
            newStrength = GUI.HorizontalSlider(new Rect(110, 75, 180, 20), newStrength, 0f, 1000f);

            if (GUI.Button(new Rect(10, 100, 100, 20), "Reset"))
            {
                newStrength = 200f;
            }

            climbWallEnabled = GUI.Toggle(new Rect(120, 100, 100, 20), climbWallEnabled, "Climb Wall");

            // Invincibility Toggle
            invincibilityEnabled = GUI.Toggle(new Rect(10, 125, 150, 20), invincibilityEnabled, "Invincibility");
        }

        public static void UpdateCharacterFeatures(GameObject playerObject)
        {
            var bodyForces = UnityEngine.Object.FindObjectsOfType<bodyForce>();
            foreach (var bodyForce in bodyForces)
            {
                bodyForce.strength = newStrength;

                if (climbWallEnabled)
                {
                    var chestRb = bodyForce.chest.GetComponent<Rigidbody>();
                    if (chestRb != null)
                    {
                        chestRb.AddForce(Vector3.up * 50f, ForceMode.Force);
                    }
                }
            }

            if (playerObject != null)
            {
                var charStrength = playerObject.GetComponent<characterStrength>();
                if (charStrength != null)
                {
                    // Removed: noKnockout { charStrength.ApplyGetUp(); } don't work correctly
                }
            }

            // Invincibility using Reflection
            var cheatMgr = UnityEngine.Object.FindObjectOfType<cheatManager>();
            if (cheatMgr != null)
            {
                MethodInfo activateCheatMethod = typeof(cheatManager).GetMethod("activateCheat", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo cheatsField = typeof(cheatManager).GetField("cheats", BindingFlags.Public | BindingFlags.Instance);

                if (activateCheatMethod != null && cheatsField != null)
                {
                    var cheatsArray = (Array)cheatsField.GetValue(cheatMgr);
                    if (cheatsArray != null && cheatsArray.Length > 6)
                    {
                        object cheatObject = cheatsArray.GetValue(6);
                        if (cheatObject != null)
                        {
                            FieldInfo activeField = cheatObject.GetType().GetField("active", BindingFlags.NonPublic | BindingFlags.Instance);
                            if (activeField != null)
                            {
                                bool currentActiveState = (bool)activeField.GetValue(cheatObject);

                                if (currentActiveState != invincibilityEnabled)
                                {
                                    activateCheatMethod.Invoke(cheatMgr, new object[] { 6 });
                                    MelonLoader.MelonLogger.Msg($"Invincibility toggled to: {invincibilityEnabled}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
