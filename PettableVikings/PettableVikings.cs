using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace PettableVikings
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class PettableVikingsPlugin : BaseUnityPlugin
    {
        private const string ModGUID = "com.valheim.pettablevikings";
        private const string ModName = "Pettable Vikings";
        private const string ModVersion = "1.0.2";

        public static ConfigEntry<bool> ModEnabled;
        public static ConfigEntry<string> HoverText;
        public static ConfigEntry<string> InteractionMessage;
        public static ConfigEntry<float> InteractionDistance;
        public static ConfigEntry<float> CooldownTimer;

        public static ConfigEntry<bool> ShowHearts;
        public static ConfigEntry<bool> PlaySound;

        /// Initializes the mod by loading configuration settings and setting up Harmony patches.
        private void Awake()
        {
            ModEnabled = Config.Bind("1 - General", "1_Enabled", true, "Enable the mod.");
            HoverText = Config.Bind("1 - General", "2_HoverText", "Pet @viking", "Text shown on the crosshair. Use @viking to insert the player's name.");
            InteractionMessage = Config.Bind("1 - General", "3_Message", "@viking loves you", "Text displayed after a successful pet. Use @viking to insert the player's name.");
            InteractionDistance = Config.Bind("1 - General", "4_Distance", 2.0f, "Maximum distance in meters to allow petting.");
            CooldownTimer = Config.Bind("1 - General", "5_Cooldown", 1.0f, "Seconds to wait before you can pet someone again.");

            ShowHearts = Config.Bind("2 - Visuals & Audio", "1_ShowHearts", true, "Show the pink hearts effect when petting.");
            PlaySound = Config.Bind("2 - Visuals & Audio", "2_PlaySound", true, "Play the default love sound when petting.");

            // Always patch: every handler checks ModEnabled at runtime, so toggling the
            // setting in-game (Configuration Manager) works without a restart.
            Harmony harmony = new Harmony(ModGUID);
            harmony.PatchAll();
        }
    }

    /// Harmony patch that adds the PettableComponent to all player instances.
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class Player_Awake_Patch
    {
        /// Postfix patch that attaches the petting functionality to a player on instantiation.
        public static void Postfix(Player __instance)
        {
            __instance.gameObject.AddComponent<PettableComponent>();
        }
    }

    /// Harmony patch that adds petting prompt text to the hover text displayed when looking at players.
    [HarmonyPatch(typeof(Player), "GetHoverText")]
    public static class Player_GetHoverText_Patch
    {
        /// Postfix patch that appends the petting interaction text to the hover display.
        public static void Postfix(Player __instance, ref string __result)
        {
            if (!PettableVikingsPlugin.ModEnabled.Value) return;
            Player localPlayer = Player.m_localPlayer;
            if (localPlayer == null || __instance == localPlayer || __instance.IsDead()) return;

            float distance = Vector3.Distance(localPlayer.transform.position, __instance.transform.position);
            if (distance > PettableVikingsPlugin.InteractionDistance.Value) return;

            string formattedHover = PettableVikingsPlugin.HoverText.Value.Replace("@viking", __instance.GetPlayerName());
            string petText = Localization.instance.Localize($"\n[<color=yellow><b>$KEY_Use</b></color>] {formattedHover}");
            
            __result += petText;
        }
    }

    public class PettableComponent : MonoBehaviour, Interactable
    {
        private Player _thisPlayer;
        private ZNetView _nview;
        private float _lastPetTime = -1000f;
        private float _lastVfxTime = -1000f;
        private const float MinVfxInterval = 0.25f;

        /// Initializes the petting component by caching player and network references and registering the VFX RPC.
        private void Awake()
        {
            _thisPlayer = GetComponent<Player>();
            _nview = GetComponent<ZNetView>();

            if (_nview != null && _nview.IsValid())
            {
                _nview.Register("PetVFX", RPC_PetVFX);
            }
        }

        /// Handles player interaction for petting. Validates distance, cooldown, and triggers VFX/SFX effects.
        /// <param name="user">The player attempting to pet.</param>
        /// <param name="hold">Whether the button is being held.</param>
        /// <param name="alt">Whether an alternate key is pressed.</param>
        /// <returns>True if interaction was successful, false otherwise.</returns>
        public bool Interact(Humanoid user, bool hold, bool alt)
        {
            if (!PettableVikingsPlugin.ModEnabled.Value) return false;

            Player localPlayer = Player.m_localPlayer;
            if (hold || alt || localPlayer == null || _thisPlayer == null || _thisPlayer == localPlayer || _thisPlayer.IsDead()) return false;

            float distance = Vector3.Distance(localPlayer.transform.position, _thisPlayer.transform.position);
            if (distance > PettableVikingsPlugin.InteractionDistance.Value) return false;

            if (Time.time - _lastPetTime < PettableVikingsPlugin.CooldownTimer.Value) return false;
            _lastPetTime = Time.time;

            Animator animator = user.GetComponent<Animator>();
            if (animator != null) animator.SetTrigger("interact");

            string finalMessage = PettableVikingsPlugin.InteractionMessage.Value.Replace("@viking", _thisPlayer.GetPlayerName());
            if (MessageHud.instance != null)
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, finalMessage);
            }

            if (_nview != null && _nview.IsValid())
            {
                _nview.InvokeRPC(ZNetView.Everybody, "PetVFX");
            }

            return true;
        }

        /// Handles item usage interaction (not implemented for petting).
        /// <param name="user">The player using the item.</param>
        /// <param name="item">The item being used.</param>
        /// <returns>Always returns false as items cannot be used for petting.</returns>
        public bool UseItem(Humanoid user, ItemDrop.ItemData item)
        {
            return false;
        }

        /// Network RPC that spawns visual and audio effects on all clients when a player is petted.
        /// Intelligently filters effects based on enabled settings to improve performance.
        /// <param name="sender">The network sender's ID (not used in this implementation).</param>
        private void RPC_PetVFX(long sender)
        {
            if (!PettableVikingsPlugin.ModEnabled.Value) return;
            if (!PettableVikingsPlugin.ShowHearts.Value && !PettableVikingsPlugin.PlaySound.Value) return;

            // Remote clients can invoke this RPC at will, so ignore bursts.
            if (Time.time - _lastVfxTime < MinVfxInterval) return;
            _lastVfxTime = Time.time;

            if (_thisPlayer == null || ZNetScene.instance == null) return;

            GameObject boarPrefab = ZNetScene.instance.GetPrefab("Boar");
            if (boarPrefab != null)
            {
                Tameable tame = boarPrefab.GetComponent<Tameable>();
                if (tame != null && tame.m_petEffect != null)
                {
                    foreach (var effect in tame.m_petEffect.m_effectPrefabs)
                    {
                        if (effect.m_prefab != null)
                        {
                            GameObject spawnedEffect = Instantiate(effect.m_prefab, _thisPlayer.GetTopPoint(), Quaternion.identity);

                            if (!PettableVikingsPlugin.PlaySound.Value)
                            {
                                var audioNodes = spawnedEffect.GetComponentsInChildren<ZSFX>();
                                foreach (var node in audioNodes) Destroy(node);
                                
                                var audioSources = spawnedEffect.GetComponentsInChildren<AudioSource>();
                                foreach (var source in audioSources) Destroy(source);
                            }

                            if (!PettableVikingsPlugin.ShowHearts.Value)
                            {
                                var particles = spawnedEffect.GetComponentsInChildren<ParticleSystem>();
                                foreach (var particle in particles) Destroy(particle);
                            }
                        }
                    }
                }
            }
        }
    }
}