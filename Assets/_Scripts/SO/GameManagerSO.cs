using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "SO/GameManagerSO")]
public class GameManagerSO : SingletonScriptableObject<GameManagerSO>
{
    private Player player;
    private InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;

    public Vector3 NewPosition { get => newPosition; }
    public Vector3 NewOrientation { get => newOrientation; }

    [NonSerialized] private Vector3 newPosition = new Vector3(2.5f, 2.5f); // NonSerialized, para que se resetee entre sesiones (partidas)
    [NonSerialized] private Vector3 newOrientation;

    private int collectedCoins;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadedScene;
    }

    private void OnLoadedScene(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<Player>();
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
        this.newPosition = newPosition;
        this.newOrientation = newOrientation;
        SceneManager.LoadScene(newSceneIndex);
    }
}
