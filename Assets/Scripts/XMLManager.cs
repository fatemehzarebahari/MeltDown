using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMLManager : MonoBehaviour
{
    public static XMLManager ins;

    void Awake()
    {
        ins = this;
    }
    
    public void SaveData(SaveDataType data)
    {
        string filePath = Application.dataPath + "/StreamingFiles/XML/data.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(SaveDataType));
        FileStream stream = new FileStream(filePath,FileMode.Create);
        serializer.Serialize(stream,data);
        stream.Close();
    }
    public SaveDataType LoadData()
    {
        string filePath = Application.dataPath + "/StreamingFiles/XML/data.xml";
        if (File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaveDataType));
            FileStream stream = new FileStream(filePath, FileMode.Open);
            SaveDataType data = (SaveDataType)serializer.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            SaveDataType data = new SaveDataType();
            data.coin = 0;
            data.currentCharacter = 0;
            data.receivedCharacters = new List<int>() { 0 };
            SaveData(data);
            return data;
        }
    }
    
}
[System.Serializable]
public class SaveDataType
{
    public int coin;
    public int currentCharacter;
    public List<int> receivedCharacters;
    
}
