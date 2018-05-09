using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class ReversiSettingsData
{
    [BoxGroup("Board")]
    [ValidateInput("EvenValue", "The value has to be even.", InfoMessageType.Warning)]
    public int columns = 8;
    [BoxGroup("Board")]
    [ValidateInput("EvenValue", "The value has to be even.", InfoMessageType.Warning)]
    public int rows = 8;
    private bool EvenValue(int value)
    {
        return value % 2 == 0;
    }

    [HideInInspector]
    public SerializableColor darkTileColor = new SerializableColor(new Color(0.159f, 0.689f, 0.176f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _darkTileColor
    {
        get { return darkTileColor != null ? darkTileColor.GetColor() : new Color(0.159f, 0.689f, 0.176f, 1f); }
        set { darkTileColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor lightTileColor = new SerializableColor(new Color(0.262f, 0.764f, 0.105f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _lightileColor
    {
        get { return lightTileColor != null ? lightTileColor.GetColor() : new Color(0.262f, 0.764f, 0.105f, 1f); }
        set { lightTileColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor topPieceColor = new SerializableColor(new Color(0.274f, 0.274f, 0.274f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _topPieceColor
    {
        get { return topPieceColor != null ? topPieceColor.GetColor() : new Color(0.274f, 0.274f, 0.274f, 1f); }
        set { topPieceColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor bottomPieceColor = new SerializableColor(Colors.GhostWhite);
    [ShowInInspector, BoxGroup("Colors")]
    private Color _bottomPieceColor
    {
        get { return bottomPieceColor != null ? bottomPieceColor.GetColor() : Colors.GhostWhite; }
        set { bottomPieceColor = new SerializableColor(value); }
    }


    public ReversiSettingsData()
    {
        columns = 8;
        rows = 8;


    }
    public ReversiSettingsData(ReversiSettingsData other)
    {
        columns = other.columns;
        rows = other.rows;

        darkTileColor = other.darkTileColor;
        lightTileColor = other.lightTileColor;
        topPieceColor = other.topPieceColor;
        bottomPieceColor = other.bottomPieceColor;

    }
}
public class ReversiSettings : MonoBehaviour
{


    public static ReversiSettings instance;
    public ReversiSettingsData settings = new ReversiSettingsData();
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void SaveSettings()
    {
        SaveLoad.SaveFile("/reversi_settings.dat", settings);
    }
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void LoadSettings()
    {
        ReversiSettingsData load = SaveLoad.LoadFile<ReversiSettingsData>("/reversi_settings.dat");
        if (load != null)
        {
            settings = load;
            return;
        }
    }
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void ResetSettings()
    {
        settings = new ReversiSettingsData();

    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        LoadSettings();
    }
}
