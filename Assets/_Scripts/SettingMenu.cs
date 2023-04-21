using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[Serializable]
public class SaveData
{
    [Range(0, 1)] public float volume;
    public int BackGroundNo;
    [Range(1, 10)] public int MaxRound;
}

public class SettingMenu : MonoBehaviour
{

    public SaveData saveData;

    public Slider Vol_Slider;
    public Slider MaxRoundSlider;
    public Text MaxRoundText;

    [HideInInspector]public AudioSource audioSource;
    public Setting inputs;
    void Awake()
    {
        load();

    }
    private void Start()
    {
        audioSource=SoundManager.instance.GetComponent<AudioSource>();
    }

    public void save()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/Player.dat", FileMode.Create);
        SaveData data = new SaveData();
        data = saveData;
       /*
        saveData.volume = Vol_Slider.value;
        saveData.MaxRound= (int)MaxRoundSlider.value;
        MaxRoundText.text = ((int)MaxRoundSlider.value).ToString();
        */
        formatter.Serialize(file, data);
        file.Close();
        Debug.Log("Saved");

    }

    public void load()
    {
        if (File.Exists(Application.persistentDataPath + "/Player.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Player.dat", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Loaded");
            //data is in our -data- object but now assign that data to our UI.
            Vol_Slider.value = saveData.volume;
            audioSource.volume = saveData.volume;
            MaxRoundSlider.value = saveData.MaxRound;
            MaxRoundText.text = ((int)MaxRoundSlider.value).ToString();
            inputs.BackgroundNum = saveData.BackGroundNo;
        }
    }

    private void OnApplicationQuit()
    {
        save();
    }

    void OnDisable()
    {
        save();
    }

    public void changeVolume()
    {
        audioSource.volume = Vol_Slider.value;
        saveData.volume = Vol_Slider.value;
    }

    public void changeMaxNumber()
    {
        saveData.MaxRound = (int)MaxRoundSlider.value;
        MaxRoundText.text = ((int)MaxRoundSlider.value).ToString();
        inputs.maxRound = saveData.MaxRound;
    }

    public void changeBackGround(int num)
    {
        saveData.BackGroundNo = num;
        inputs.BackgroundNum = saveData.BackGroundNo;
    }

}
