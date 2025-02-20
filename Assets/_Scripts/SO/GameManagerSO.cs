using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "SO/GameManagerSO")]
public class GameManagerSO : SingletonScriptableObject<GameManagerSO>
{
    private Player player;
    private InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;
    public Vector3 NewPosition => newPosition;
    public Vector3 NewOrientation => newOrientation;
    public bool SceneLoaded => sceneLoaded;

    [NonSerialized] private Vector3 newPosition = new Vector3(2.5f, 2.5f); // NonSerialized, para que se resetee entre sesiones (partidas)
    [NonSerialized] private Vector3 newOrientation;
    [NonSerialized] private bool sceneLoaded = false;

    [NonSerialized] private int collectedCoins;
    public int CollectedCoins => collectedCoins;

    public Dictionary<int, bool> NonPersistentItems { get => nonPersistentItems; set => nonPersistentItems = value; }

    [NonSerialized] private Dictionary<int, bool> nonPersistentItems = new Dictionary<int, bool>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadedScene;

        // Hago la llamada 2 veces porque nada más cargar en la escena no se carga ni el player ni el inventory
        OnLoadedScene(SceneManager.GetActiveScene(), LoadSceneMode.Single); 
    }

    private void OnLoadedScene(Scene arg0, LoadSceneMode arg1)
    {
        if(!player)
            player = FindObjectOfType<Player>();
        if(!inventorySystem)
            inventorySystem = FindObjectOfType<InventorySystem>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadedScene;
    }

    public void ChangePlayerState(bool state)
    {
        player.PlayerCollisions.IsInteracting = !state;
    }

    public void LoadNewScene(Vector3 newPosition, Vector2 newOrientation, int newSceneIndex)
    {
        sceneLoaded = true;
        this.newPosition = newPosition;
        this.newOrientation = newOrientation;
        SceneManager.LoadScene(newSceneIndex);
    }

    public void ResetStatus()
    {
        sceneLoaded = false;
    }
}
