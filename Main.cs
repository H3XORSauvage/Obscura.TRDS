using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(Obscura.TRDS.Main), "Obscura.TRDS", "1.0.0", "Saucisson")]
[assembly: MelonGame("We're Five Games", "Totally Reliable Delivery Service")]

namespace Obscura.TRDS
{
    public class Main : MelonMod
    {
        private GameObject player;
        public static bool flyEnabled = true; // toggle Fly
        private bool showMenu = true;

        private int selectedTab = 0;
        private string[] tabs = { "Fly", "Other" }; // next: muscles

        private Rect windowRect = new Rect(20, 20, 300, 200);

        public override void OnGUI()
        {
            if (!showMenu)
                return;

            windowRect = GUI.Window(0, windowRect, DrawWindow, "Obscura.TRDS");
        }

        private void DrawWindow(int windowID)
        {
            selectedTab = GUI.Toolbar(new Rect(10, 30, 280, 30), selectedTab, tabs);

            switch (selectedTab)
            {
                case 0: // Fly
                    GUI.Label(new Rect(10, 70, 100, 20), "Fly Enabled:");
                    flyEnabled = GUI.Toggle(new Rect(110, 70, 50, 20), flyEnabled, "");

                    if (player != null)
                    {
                        var flyComp = player.GetComponent<Fly>();
                        if (flyComp != null)
                            flyComp.enabled = flyEnabled;

                        var rb = player.GetComponent<Rigidbody>();
                        if (rb != null)
                            rb.useGravity = !flyEnabled;
                    }
                    break;

                case 1: // Other
                    GUI.Label(new Rect(10, 70, 200, 20), "Future features here...");
                    break;
            }

            GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F5))
                showMenu = !showMenu;

            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    if (player.GetComponent<Fly>() == null)
                        player.AddComponent<Fly>();

                    var flyComp = player.GetComponent<Fly>();
                    if (flyComp != null)
                        flyComp.enabled = flyEnabled;

                    var rb = player.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.useGravity = !flyEnabled;

                    MelonLogger.Msg("Ragdoll Fly attaché au joueur.");
                }
            }
        }
    }
}
