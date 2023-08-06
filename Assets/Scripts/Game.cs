using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    
    [Header("Offsets")]
    [SerializeField] private int playerPlatformIndex = 0;
    [SerializeField] private int timeToGetCoin = 1;
    [SerializeField] private int coinIncreaseAmount = 1;
    
    [Space] [Header("GameMode")] 
    [SerializeField] private bool inGame;
    
    [Space][Header("GameObjects")]
    [SerializeField] private Transform destroyersCenter;
    [SerializeField] private TextMeshProUGUI coinTextUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private XMLManager xmlData;

    [Space][Header("Prefabs")]
    [SerializeField] private Transform[] playerPrefabs;
    [SerializeField] private Transform[] automatePlayerPrefabs;
    [SerializeField] private Transform[] playerPlatforms;

    [Space][Header("SetByTheGame")]
    [SerializeField] private List<PlayersOrigin> players;

    private bool alive = true;
    private int _coin;
    private Transform _playerCharacterPrefab;
    private bool _getCoin = true;
    private List<int> _receivedCharacters;
    private int _playerCharacterIndex;

    private void Awake()
    {
        instance = this;
        
        LoadData();

    }

    void Update()
    {
        if (alive &&_getCoin)
        {
            _getCoin = false;
            StartCoroutine(TimeToGetCoin());
        }
            
    }

    private void GameOver()
    {
        SaveData();
        gameOverMenu.SetActive(true);
    }
    private IEnumerator TimeToGetCoin()
    {
        yield return new WaitForSeconds((float)90 / timeToGetCoin);
        GetCoin();
        _getCoin = true;
    }

    public void GetCoin()
    {
        _coin += coinIncreaseAmount;
        SetCoinInGameScene();
    }

    public void AddPlayer(PlayersOrigin newPlayer)
    {
        newPlayer.SetInGame(inGame);
        players.Add(newPlayer);
    }

    public void RemovePlayer(PlayersOrigin newPlayer)
    {
        players.Remove(newPlayer);
        if (players.Count <= 0)
        {
            GameOver();
        }
    }
    
    private void SetCoinInGameScene()
    {
        if(inGame)
            coinTextUI.SetText(_coin.ToString());
    }

    private void LoadData()
    {
        SaveDataType data = xmlData.LoadData();
        _coin = data.coin;
        SetCoinInGameScene();
        
        _playerCharacterIndex = data.currentCharacter;
        if (_playerCharacterIndex <= playerPrefabs.Length)
        {
            _playerCharacterPrefab = playerPrefabs[_playerCharacterIndex];
        }
        else
        {
            _playerCharacterPrefab = playerPrefabs[0];
            _playerCharacterIndex = 0;
        }

        _receivedCharacters = data.receivedCharacters;
        
        CreatePlayers();
        
    }

    private void SaveData()
    {
        SaveDataType data = new SaveDataType();
        data.coin = _coin;
        data.currentCharacter = _playerCharacterIndex;
        data.receivedCharacters = _receivedCharacters;
        xmlData.SaveData(data);
    }

    private void CreatePlayers()
    {
        if (inGame)
        {
            for (int i = 0; i < playerPlatforms.Length; i++)
            {
                if (i != playerPlatformIndex)
                {
                    int randomIndex = Random.Range(0, automatePlayerPrefabs.Length);
                    InstantiatePlayer(i, automatePlayerPrefabs[randomIndex]);
                }
            }

            InstantiatePlayer(playerPlatformIndex, _playerCharacterPrefab);
        }
        
        else
        {
            for (int i = 0; i < playerPlatforms.Length; i++)
            {
                int randomIndex = Random.Range(0, automatePlayerPrefabs.Length);
                InstantiatePlayer(i, automatePlayerPrefabs[randomIndex]);
            }
        }

    }
    private void InstantiatePlayer(int platformIndex,Transform playerPrefab)
    {
        Vector3 direction = destroyersCenter.transform.position - playerPlatforms[platformIndex].position;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Instantiate(playerPrefab, playerPlatforms[platformIndex].position, lookRotation);
    }
    
}

