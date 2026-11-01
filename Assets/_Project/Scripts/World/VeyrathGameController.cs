using System;
using System.IO;
using UnityEngine;
using Veyrath.Core;
using UnityEngine.Rendering;

namespace Veyrath.World
{
    /// <summary>Small, self-contained First Light gameplay loop; world visuals are disposable placeholders.</summary>
    public sealed class VeyrathGameController : MonoBehaviour
    {
        private const string SaveFileName = "veyrath-first-light.json";
        private readonly QuestState quest = new QuestState();
        private readonly InventoryState inventory = new InventoryState();
        private CharacterController player;
        private Transform npc;
        private Transform wraith;
        private Camera gameCamera;
        private float health = 100f;
        private float enemyHealth = 60f;
        private float attackCooldown;
        private float enemyAttackCooldown;
        private float messageUntil;
        private string message = "Find Edda near the ruined arch.";
        private bool inventoryOpen;
        private bool settingsOpen;
        private bool wraithDefeated;
        private bool lootCollected;
        private GameObject loot;
        private const string WeaponName = "Ashen shortblade";
        private const string LootName = "Wraith heartstone";
        private Vector3 spawnPoint = new Vector3(0f, 1f, -4f);

        private void Start()
        {
            VeyrathSettings.Initialize();
            BuildWorld();
            VeyrathLogger.Info("Game", "First Light gameplay scene started.");
            Show("Find Edda near the ruined arch.", 8f);
        }

        private void Update()
        {
            if (player == null) return;
            float dt = Time.deltaTime;
            attackCooldown -= dt;
            enemyAttackCooldown -= dt;
            MovePlayer(dt);
            FollowCamera(dt);
            UpdateWraith(dt);

            if (Input.GetKeyDown(KeyCode.E)) Interact();
            if (Input.GetKeyDown(KeyCode.E)) CollectLoot();
            if (Input.GetKeyDown(KeyCode.Space)) Attack();
            if (Input.GetKeyDown(KeyCode.I)) inventoryOpen = !inventoryOpen;
            if (Input.GetKeyDown(KeyCode.O)) settingsOpen = !settingsOpen;
            if (health <= 0f)
            {
                health = 100f;
                player.enabled = false;
                player.transform.position = spawnPoint;
                player.enabled = true;
                Show("You wake at the camp. The Wraith still waits.", 4f);
            }
        }

        private void BuildWorld()
        {
            Material ground = MakeMaterial(new Color(0.19f, 0.23f, 0.20f));
            Material stone = MakeMaterial(new Color(0.32f, 0.31f, 0.29f));
            Material playerMat = MakeMaterial(new Color(0.22f, 0.48f, 0.68f));
            Material npcMat = MakeMaterial(new Color(0.78f, 0.62f, 0.34f));
            Material enemyMat = MakeMaterial(new Color(0.48f, 0.16f, 0.22f));

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Placeholder | First Light clearing";
            floor.transform.position = new Vector3(0f, -0.55f, 3f);
            floor.transform.localScale = new Vector3(24f, 1f, 30f);
            floor.GetComponent<Renderer>().sharedMaterial = ground;

            CreatePrimitive("Old road stone", PrimitiveType.Cube, new Vector3(-4f, 0.2f, 5f), new Vector3(2.2f, 1.5f, 1f), stone);
            CreatePrimitive("Old road stone", PrimitiveType.Cube, new Vector3(4f, 0.2f, 5f), new Vector3(2.2f, 1.5f, 1f), stone);

            GameObject playerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerObject.name = "Player | Placeholder adventurer";
            playerObject.transform.position = spawnPoint;
            playerObject.GetComponent<Renderer>().sharedMaterial = playerMat;
            Destroy(playerObject.GetComponent<CapsuleCollider>());
            player = playerObject.AddComponent<CharacterController>();
            player.height = 2f;
            player.radius = 0.42f;
            player.center = Vector3.zero;

            npc = CreatePrimitive("Edda | NPC placeholder", PrimitiveType.Capsule, new Vector3(0f, 1f, 3f), Vector3.one, npcMat).transform;
            wraith = CreatePrimitive("Wraith | Enemy placeholder", PrimitiveType.Capsule, new Vector3(0f, 1f, 11f), Vector3.one, enemyMat).transform;
            loot = CreatePrimitive("Loot | Wraith heartstone", PrimitiveType.Sphere, new Vector3(0f, 0.55f, 11f), Vector3.one * 0.55f, MakeMaterial(new Color(0.65f, 0.25f, 0.75f)));
            loot.SetActive(false);

            GameObject cameraObject = new GameObject("First Light Camera");
            gameCamera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            gameCamera.fieldOfView = 58f;
            gameCamera.transform.position = player.transform.position + new Vector3(0f, 5.2f, -8f);
            gameCamera.transform.LookAt(player.transform.position + Vector3.up * 0.7f);

            GameObject sun = new GameObject("Placeholder daylight");
            Light directional = sun.AddComponent<Light>();
            directional.type = LightType.Directional;
            directional.intensity = 1.25f;
            sun.transform.rotation = Quaternion.Euler(48f, -32f, 0f);
            RenderSettings.ambientLight = new Color(0.48f, 0.47f, 0.43f);
        }

        private static Material MakeMaterial(Color color)
        {
            bool scriptablePipelineActive = GraphicsSettings.currentRenderPipeline != null;
            Shader shader = Shader.Find(scriptablePipelineActive ? "Universal Render Pipeline/Lit" : "Standard");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");
            Material material = new Material(shader) { color = color };
            return material;
        }

        private static GameObject CreatePrimitive(string objectName, PrimitiveType type, Vector3 position, Vector3 scale, Material material)
        {
            GameObject instance = GameObject.CreatePrimitive(type);
            instance.name = objectName;
            instance.transform.position = position;
            instance.transform.localScale = scale;
            instance.GetComponent<Renderer>().sharedMaterial = material;
            return instance;
        }

        private void MovePlayer(float dt)
        {
            float x = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f);
            float z = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);
            Vector3 input = new Vector3(x, 0f, z);
            if (input.sqrMagnitude > 1f) input.Normalize();
            Vector3 movement = input * 4.6f;
            movement.y = -2f;
            player.Move(movement * dt);
            if (input.sqrMagnitude > 0.01f) player.transform.forward = Vector3.Slerp(player.transform.forward, input.normalized, 12f * dt);
        }

        private void FollowCamera(float dt)
        {
            Vector3 target = player.transform.position + new Vector3(0f, 5.2f, -8f);
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, target, 5f * dt);
            gameCamera.transform.LookAt(player.transform.position + Vector3.up * 0.7f);
        }

        private void Interact()
        {
            if (Vector3.Distance(player.transform.position, npc.position) > 2.6f)
            {
                Show("Come closer to Edda or the Wraith.", 2f);
                return;
            }

            switch (quest.stage)
            {
                case FirstLightQuestStage.NotStarted:
                    quest.Accept();
                    inventory.AddItem(WeaponName);
                    Show("Edda: Take this " + WeaponName + ". End the Wraith's shadow and return to me.", 7f);
                    break;
                case FirstLightQuestStage.HuntTheWraith:
                    Show("Edda: The Wraith is somewhere up the old road. Be careful.", 5f);
                    break;
                case FirstLightQuestStage.ReturnToEdda:
                    if (!lootCollected)
                    {
                        Show("Collect the Wraith heartstone first.", 3f);
                        break;
                    }
                    if (quest.ClaimReward())
                    {
                        inventory.AddReward("Edda's iron charm", 25);
                        Show("Edda: You kept your word. Take this charm and 25 coins.", 6f);
                    }
                    break;
                default:
                    Show("Edda: First Light is quieter, thanks to you.", 4f);
                    break;
            }
        }

        private void Attack()
        {
            if (quest.stage != FirstLightQuestStage.HuntTheWraith || wraithDefeated || !inventory.Contains(WeaponName))
            {
                Show("There is no active target. Speak to Edda to begin the hunt.", 3f);
                return;
            }
            if (Vector3.Distance(player.transform.position, wraith.position) > 2.7f)
            {
                Show("The Wraith is too far away to strike.", 2f);
                return;
            }
            if (attackCooldown > 0f) return;
            attackCooldown = 0.55f;
            enemyHealth -= 25f;
            Show("You strike the Wraith. Its strength: " + Mathf.CeilToInt(Mathf.Max(0f, enemyHealth)) + ".", 2f);
            if (enemyHealth <= 0f)
            {
                wraithDefeated = true;
                wraith.gameObject.SetActive(false);
                loot.SetActive(true);
                quest.CompleteWraithHunt();
                Show("The Wraith dissolves. Collect the heartstone, then return to Edda.", 6f);
            }
        }

        private void CollectLoot()
        {
            if (lootCollected || !wraithDefeated || Vector3.Distance(player.transform.position, loot.transform.position) > 2.5f) return;
            lootCollected = true;
            loot.SetActive(false);
            inventory.AddItem(LootName);
            inventory.coins += 10;
            Show("Loot collected: " + LootName + " and 10 coins.", 4f);
        }

        private void UpdateWraith(float dt)
        {
            if (wraithDefeated || quest.stage != FirstLightQuestStage.HuntTheWraith) return;
            Vector3 delta = player.transform.position - wraith.position;
            delta.y = 0f;
            float distance = delta.magnitude;
            if (distance > 1.55f && distance < 14f)
                wraith.position += delta.normalized * (1.35f * dt);
            else if (distance <= 1.55f && enemyAttackCooldown <= 0f)
            {
                enemyAttackCooldown = 1.3f;
                health = Mathf.Max(0f, health - 12f);
                Show("The Wraith hits you. Health: " + Mathf.CeilToInt(health) + ".", 2f);
            }
        }

        private void Show(string text, float duration)
        {
            message = text;
            messageUntil = Time.time + duration;
        }

        private void Save()
        {
            SaveData data = new SaveData
            {
                playerX = player.transform.position.x,
                playerY = player.transform.position.y,
                playerZ = player.transform.position.z,
                health = health,
                questStage = quest.stage,
                rewardClaimed = quest.rewardClaimed,
                coins = inventory.coins,
                items = inventory.items.ToArray(),
                wraithDefeated = wraithDefeated,
                lootCollected = lootCollected
            };
            string path = Path.Combine(Application.persistentDataPath, SaveFileName);
            if (!SaveGameStore.TryWrite(path, data, out string saveError))
            {
                VeyrathLogger.Error("Save", "Could not save First Light: " + saveError);
                Show("Save could not be written.", 3f);
                return;
            }
            VeyrathLogger.Info("Save", "First Light progress saved to " + path);
            Show("Progress saved locally.", 3f);
        }

        private void Load()
        {
            string path = Path.Combine(Application.persistentDataPath, SaveFileName);
            if (!File.Exists(path))
            {
                Show("No First Light save exists yet.", 3f);
                return;
            }
            try
            {
                if (!SaveGameStore.TryRead(path, out SaveData data, out string loadError))
                    throw new InvalidDataException(loadError ?? "Save could not be read.");
                player.enabled = false;
                player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
                player.enabled = true;
                health = Mathf.Clamp(data.health, 1f, 100f);
                quest.stage = data.questStage;
                quest.rewardClaimed = data.rewardClaimed;
                inventory.coins = Mathf.Max(0, data.coins);
                inventory.items.Clear();
                if (data.items != null) inventory.items.AddRange(data.items);
                wraithDefeated = data.wraithDefeated;
                lootCollected = data.lootCollected;
                wraith.gameObject.SetActive(!wraithDefeated);
                loot.SetActive(wraithDefeated && !lootCollected);
                enemyHealth = wraithDefeated ? 0f : 60f;
                VeyrathLogger.Info("Save", "First Light progress loaded from " + path);
                Show("Progress loaded.", 3f);
            }
            catch (Exception exception)
            {
                VeyrathLogger.Error("Save", "Could not load First Light save: " + exception);
                Show("Save could not be loaded. See the Unity Console.", 4f);
            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(12, 12, 310, 130), "PROJECT VEYRATH | FIRST LIGHT");
            GUI.Label(new Rect(24, 42, 285, 24), "Health: " + Mathf.CeilToInt(health) + "     Coins: " + inventory.coins);
            GUI.Label(new Rect(24, 66, 285, 40), "Quest: " + QuestDescription());
            GUI.Label(new Rect(24, 108, 320, 24), "WASD move | E interact | Space attack | I inventory | O options");
            if (Time.time < messageUntil) GUI.Box(new Rect(12, Screen.height - 78, Mathf.Min(Screen.width - 24, 560), 62), message);
            if (GUI.Button(new Rect(Screen.width - 176, 12, 76, 30), "Save")) Save();
            if (GUI.Button(new Rect(Screen.width - 94, 12, 82, 30), "Load")) Load();
            if (GUI.Button(new Rect(Screen.width - 270, 12, 86, 30), "Options")) settingsOpen = !settingsOpen;
            if (inventoryOpen && !settingsOpen)
            {
                GUI.Box(new Rect(Screen.width - 290, 52, 278, 190), "INVENTORY");
                GUI.Label(new Rect(Screen.width - 276, 82, 250, 24), "Coins: " + inventory.coins);
                if (inventory.items.Count == 0) GUI.Label(new Rect(Screen.width - 276, 108, 250, 24), "No items yet.");
                for (int i = 0; i < inventory.items.Count; i++)
                    GUI.Label(new Rect(Screen.width - 276, 108 + i * 22, 250, 22), "• " + inventory.items[i]);
            }
            if (settingsOpen) DrawSettings();
        }

        private void DrawSettings()
        {
            int panelX = Screen.width - 300;
            GUI.Box(new Rect(panelX, 52, 288, 270), "SETTINGS");
            if (GUI.Button(new Rect(panelX + 12, 82, 264, 30), "Display: " + (VeyrathSettings.IsFullscreen ? "Fullscreen" : "Windowed")))
                VeyrathSettings.ToggleFullscreen();
            if (GUI.Button(new Rect(panelX + 12, 118, 264, 30), "Resolution: " + Screen.width + " × " + Screen.height))
                VeyrathSettings.CycleResolution();
            if (GUI.Button(new Rect(panelX + 12, 154, 264, 30), "V-Sync: " + (VeyrathSettings.IsVSync ? "On" : "Off")))
                VeyrathSettings.ToggleVSync();
            if (GUI.Button(new Rect(panelX + 12, 190, 264, 30), "Frame limit: " + VeyrathSettings.FrameRateLabel))
                VeyrathSettings.CycleFrameRate();
            if (GUI.Button(new Rect(panelX + 12, 226, 264, 30), "Quality: " + VeyrathSettings.QualityLabel))
                VeyrathSettings.CycleQuality();
            GUI.Label(new Rect(panelX + 12, 261, 84, 24), "Master volume");
            float volume = GUI.HorizontalSlider(new Rect(panelX + 112, 265, 138, 20), VeyrathSettings.MasterVolume, 0f, 1f);
            if (!Mathf.Approximately(volume, VeyrathSettings.MasterVolume)) VeyrathSettings.SetMasterVolume(volume);
            if (GUI.Button(new Rect(panelX + 12, 286, 264, 26), "Close")) settingsOpen = false;
        }

        private string QuestDescription()
        {
            switch (quest.stage)
            {
                case FirstLightQuestStage.NotStarted: return "Speak with Edda";
                case FirstLightQuestStage.HuntTheWraith: return "Defeat the Wraith";
                case FirstLightQuestStage.ReturnToEdda: return "Return to Edda";
                default: return "First Light complete";
            }
        }
    }
}
