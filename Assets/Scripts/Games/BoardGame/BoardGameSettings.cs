using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class BoardGameSettingsData
{
    [BoxGroup("Board")]
    public int columns = 8;
    [BoxGroup("Board")]
    public int rows = 8;

    [HideInInspector]
    public SerializableColor darkTileColor = new SerializableColor(new Color(0.749f, 0.502f, 0.31f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _darkTileColor
    {
        get { return darkTileColor != null ? darkTileColor.GetColor() : new Color(0.749f, 0.502f, 0.31f, 1f); }
        set { darkTileColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor lightTileColor = new SerializableColor(new Color(1f, 0.82f, 0.682f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _lightileColor
    {
        get { return lightTileColor != null ? lightTileColor.GetColor() : new Color(1f, 0.82f, 0.682f, 1f); }
        set { lightTileColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor topPieceColor = new SerializableColor(Color.red);
    [ShowInInspector, BoxGroup("Colors")]
    private Color _topPieceColor
    {
        get { return topPieceColor != null ? topPieceColor.GetColor() : Color.red; }
        set { topPieceColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor bottomPieceColor = new SerializableColor(Colors.BlackLeatherJacket);
    [ShowInInspector, BoxGroup("Colors")]
    private Color _bottomPieceColor
    {
        get { return bottomPieceColor != null ? bottomPieceColor.GetColor() : Colors.BlackLeatherJacket; }
        set { bottomPieceColor = new SerializableColor(value); }
    }

    public BoardGameSettingsData()
    {
        columns = 8;
        rows = 8;
        darkTileColor = new SerializableColor(new Color(0.749f, 0.502f, 0.31f, 1f));
        lightTileColor = new SerializableColor(new Color(1f, 0.82f, 0.682f, 1f));
        topPieceColor = new SerializableColor(Color.red);
        bottomPieceColor = new SerializableColor(Colors.BlackLeatherJacket);
    }
    public BoardGameSettingsData(BoardGameSettingsData other)
    {
        columns = other.columns;
        rows = other.rows;
        darkTileColor = other.darkTileColor;
        lightTileColor = other.lightTileColor;
        topPieceColor = other.topPieceColor;
        bottomPieceColor = other.bottomPieceColor;
    }
}
public class BoardGameSettings : MonoBehaviour
{

    public static BoardGameSettings instance;
    public BoardGameSettingsData settings = new BoardGameSettingsData();
    public string saveName = "default";
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public virtual void SaveSettings()
    {
        SaveLoad.SaveFile("/" + saveName + "_settings.dat", settings);
    }
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public virtual void LoadSettings()
    {
        BoardGameSettingsData load = SaveLoad.LoadFile<BoardGameSettingsData>("/" + saveName + "_settings.dat");
        if (load != null)
        {
            settings = load;
        }
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
