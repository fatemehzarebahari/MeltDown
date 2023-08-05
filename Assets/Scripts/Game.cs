using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Offsets")]
    [SerializeField] private float destroyersSpeed = 30;
    [SerializeField] private float destroyersMaxSpeed = 500f;
    [SerializeField] private float destroyersUpgradeOffset = 10;
    [SerializeField] private int coinIncreaseAmountAtStart = 2;
    [SerializeField] private int playerPlatformIndex = 0;
    [SerializeField] private float increasingSpeedWaitingTime = 10;
    
    [Space] [Header("GameMode")] 
    [SerializeField] private bool inGame;
    
    [Space][Header("GameObjects")]
    [SerializeField] private Transform destroyersCenter;
    [SerializeField] private Destroyer rotatingThing;
    [SerializeField] private TextMeshProUGUI coinTextUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private XMLManager xmlData;
    [SerializeField] private Transform ground;

    [Space][Header("Prefabs")]
    [SerializeField] private Transform[] playerPrefabs;
    [SerializeField] private Transform[] automatePlayerPrefabs;
    [SerializeField] private Transform[] playerPlatforms;

    [Space][Header("SetByTheGame")]
    [SerializeField] private List<automatePlayer> automatePlayersInGame;
    
    private int _coin;
    private Transform _playerCharacterPrefab;

    private bool _getCoin = true;
    private float _minimumSpeed;
    
    private List<int> _receivedCharacters;
    private int _playerCharacterIndex;

    private bool _increaseSpeed = true;
    
    
    private void Awake()
    {
        LoadData();
        rotatingThing.SetSpeed(destroyersSpeed);
        _minimumSpeed = destroyersSpeed;

    }

    void Update()
    {
        if (_getCoin)
        {
            _getCoin = false;
            StartCoroutine(TimeToGetCoin());
        }

        if (_increaseSpeed)
        {
            _increaseSpeed = false;
            StartCoroutine(IncreaseDestroyersSpeed());
        }
        
    }
    private IEnumerator IncreaseDestroyersSpeed()
    {
        yield return new WaitForSeconds(increasingSpeedWaitingTime);
        
        if(destroyersSpeed < destroyersMaxSpeed)
        {
            destroyersSpeed += destroyersUpgradeOffset;
            
            
            rotatingThing.SetSpeed(destroyersSpeed);
            foreach (automatePlayer ap in automatePlayersInGame)
            {
                ap.SetDestroyersSpeed(destroyersSpeed);
            }
        }
        // if(increasingSpeedWaitingTime > 5) increasingSpeedWaitingTime -=0.1; 
        // by uncommenting the above line the time to increase speed will get shorter until it becomes 5
        _increaseSpeed = true;

    }

    public void Died()
    {
        destroyersCenter.GetComponent<Destroyer>().enabled = false;
        SaveData();
        gameOverMenu.SetActive(true);
    }
    private IEnumerator TimeToGetCoin()
    {
        Debug.Log(90 / destroyersSpeed);
        yield return new WaitForSeconds(90 / destroyersSpeed);
        GetCoin();
        _getCoin = true;
    }

    public void GetCoin()
    {
        int coinIncreaseAmount = coinIncreaseAmountAtStart * (int)(destroyersSpeed / _minimumSpeed);
        _coin += coinIncreaseAmount;
        SetCoinInGameScene();
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
        Transform player = Instantiate(playerPrefab, playerPlatforms[platformIndex].position, lookRotation);
        player.GetComponent<PlayersOrigin>().SetInGame(inGame);

        if(player.GetComponent<Player>() != null)
            player.GetComponent<Player>().SetGame(this);
        if (player.GetComponent<automatePlayer>() != null)
        {
            player.GetComponent<automatePlayer>().SetDestroyersCenter(destroyersCenter);
            player.GetComponent<automatePlayer>().SetDestroyersSpeed(destroyersSpeed);
            
            automatePlayersInGame.Add(player.GetComponent<automatePlayer>());
        }

    }



}

