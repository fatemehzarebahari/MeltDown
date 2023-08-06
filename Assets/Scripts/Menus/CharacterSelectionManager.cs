using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField] private Transform cameraCenter;
    [SerializeField] private Transform playerPlatform;
    [SerializeField] private Transform[] playerPrefabs;
    [SerializeField] private int[] charactersValue;
    [SerializeField] private XMLManager xmlData;
    
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    [SerializeField] private Color chosenCharacterButtonColor;

    
    
    private Transform _currentPlayer;
    private int _coin;
    private int _playerCharacterIndex;
    private List<int> _receivedCharacters;
    private int _playerIndexInScene;

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    

    public void Next()
    {
        if (_playerIndexInScene < playerPrefabs.Length - 1)
        {
            _playerIndexInScene++;
            ChangePlayer();
            if (!previousButton.interactable)
                previousButton.interactable = true;
            if (_playerIndexInScene == playerPrefabs.Length - 1)
            {
                nextButton.interactable = false;
            }
        }

            
    }

    public void Previous()
    {
        if (_playerIndexInScene > 0)
        {
            _playerIndexInScene--;
            ChangePlayer();
            if (!nextButton.interactable)
                nextButton.interactable = true;
            if (_playerIndexInScene == 0)
            {
                previousButton.interactable = false;
            }
        }
    }

    public void Use_Or_Buy()
    {
        if (_receivedCharacters.Contains(_playerIndexInScene))
        {
            _playerCharacterIndex = _playerIndexInScene;
            SaveData();
            ChangePlayer();
        }
        else if (_coin >= charactersValue[_playerIndexInScene])
        {
            _receivedCharacters.Add(_playerIndexInScene);
            _coin -= charactersValue[_playerIndexInScene];
            _playerCharacterIndex = _playerIndexInScene;
            SaveData();
            ChangePlayer();
            SetCoinText();
        }
    }

    private void Start()
    {
        LoadData();
        _playerIndexInScene = _playerCharacterIndex;
        InstantiatePlayerInScene();
        SetCoinText();
    }
    
    private void LoadData()
    {
        SaveDataType data = xmlData.LoadData();
        _coin = data.coin;
        
        _playerCharacterIndex = data.currentCharacter;
        if (_playerCharacterIndex <= playerPrefabs.Length)
        {
            _currentPlayer = playerPrefabs[_playerCharacterIndex];
        }
        else
        {
            _currentPlayer = playerPrefabs[0];
            _playerCharacterIndex = 0;
        }

        _receivedCharacters = data.receivedCharacters;

    }

    private void SaveData()
    {
        SaveDataType data = new SaveDataType();
        data.coin = _coin;
        data.currentCharacter = _playerCharacterIndex;
        data.receivedCharacters = _receivedCharacters;
        xmlData.SaveData(data);
    }

    private void ChangePlayer()
    {
        Destroy(_currentPlayer.gameObject);
        InstantiatePlayerInScene();
    }
    private void InstantiatePlayerInScene()
    {
        Vector3 direction = cameraCenter.transform.position - playerPlatform.position;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _currentPlayer = Instantiate(playerPrefabs[_playerIndexInScene], playerPlatform.position, lookRotation);

        if (_receivedCharacters.Contains(_playerIndexInScene))
        {
            
            SetButtonText("Use");

        }
        else 
            SetButtonText(charactersValue[_playerIndexInScene].ToString());
    }

    private void SetButtonText(string text)
    {
        buttonText.SetText(text);
    }
    private void SetCoinText()
    {
        coinText.SetText(_coin.ToString());
    }
    private void ChangeButtonAppearance()
    {
        
    }
    
}
