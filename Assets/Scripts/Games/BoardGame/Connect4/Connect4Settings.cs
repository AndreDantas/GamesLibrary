using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class Connect4SettingsData : BoardGameSettingsData
{
    [BoxGroup("Board")]
    public int connectTarget = 4;

    public Connect4SettingsData()
    {
        columns = 7;
        rows = 6;
        darkTileColor = lightTileColor = new SerializableColor(Colors.BlueBolt);

        topPieceColor = new SerializableColor(Colors.YellowCrayola);
        bottomPieceColor = new SerializableColor(Colors.Red);
        connectTarget = 4;
    }

    public Connect4SettingsData(Connect4SettingsData other) : base(other)
    {
        connectTarget = other.connectTarget;
    }

}
public class Connect4Settings : BoardGameSettings
{
    protected override void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        saveName = "connect4";
        LoadSettings();
    }

    public override void LoadSettings()
    {
        Connect4SettingsData load = SaveLoad.LoadFile<Connect4SettingsData>("/" + saveName + "_settings.dat");
        if (load != null)
        {
            settings = load;
        }
        else
            settings = new Connect4SettingsData();
    }

    public override void ResetSettings()
    {
        settings = new Connect4SettingsData();
        SaveSettings();
    }

}
