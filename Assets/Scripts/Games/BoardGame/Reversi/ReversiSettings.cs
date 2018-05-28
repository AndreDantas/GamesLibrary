using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class ReversiSettingsData : BoardGameSettingsData
{

    public ReversiSettingsData()
    {
        columns = 8;
        rows = 8;
        darkTileColor = new SerializableColor(new Color(0.159f, 0.689f, 0.176f, 1f));
        lightTileColor = new SerializableColor(new Color(0.262f, 0.764f, 0.105f, 1f));
        topPieceColor = new SerializableColor(Colors.BlackLeatherJacket);
        bottomPieceColor = new SerializableColor(Colors.GhostWhite);

    }
    public ReversiSettingsData(ReversiSettingsData other) : base(other)
    {


    }
}
public class ReversiSettings : BoardGameSettings
{

    public override void LoadSettings()
    {
        ReversiSettingsData load = SaveLoad.LoadFile<ReversiSettingsData>("/" + saveName + "_settings.dat");
        if (load != null)
        {
            settings = load;
        }
        else
            settings = new ReversiSettingsData();
    }

    public override void ResetSettings()
    {
        settings = new ReversiSettingsData();
        SaveSettings();
    }

    protected override void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        saveName = "othello";
        LoadSettings();
    }
}
