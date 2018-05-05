using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class CheckersSettingsData
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
    [BoxGroup("Pieces")]
    public int piecesByRow = 3;
    [BoxGroup("Pieces")]
    public int pieceMoveDistance = 1;
    [BoxGroup("Pieces")]
    public bool kingInfiniteMoveDistance = true;
    [BoxGroup("Pieces")]
    public bool multiDirectionalCapture = true;

    public CheckersSettingsData()
    {
        columns = 8;
        rows = 8;
        piecesByRow = 3;
        pieceMoveDistance = 1;
        kingInfiniteMoveDistance = true;
        multiDirectionalCapture = true;
        darkTileColor = new SerializableColor(new Color(0.749f, 0.502f, 0.31f, 1f));
        lightTileColor = new SerializableColor(new Color(1f, 0.82f, 0.682f, 1f));
        topPieceColor = new SerializableColor(Color.red);
        bottomPieceColor = new SerializableColor(Colors.BlackLeatherJacket);
    }
    public CheckersSettingsData(CheckersSettingsData other)
    {
        columns = other.columns;
        rows = other.rows;
        pieceMoveDistance = other.pieceMoveDistance;
        piecesByRow = other.piecesByRow;
        kingInfiniteMoveDistance = other.kingInfiniteMoveDistance;
        multiDirectionalCapture = other.multiDirectionalCapture;
        darkTileColor = other.darkTileColor;
        lightTileColor = other.lightTileColor;
        topPieceColor = other.topPieceColor;
        bottomPieceColor = other.bottomPieceColor;
    }
}
public class CheckersSettings : MonoBehaviour
{
    public static CheckersSettings instance;
    public CheckersSettingsData settings = new CheckersSettingsData();
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void SaveSettings()
    {
        SaveLoad.SaveFile("/checkers_settings.dat", settings);
    }
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void LoadSettings()
    {
        CheckersSettingsData load = SaveLoad.LoadFile<CheckersSettingsData>("/checkers_settings.dat");
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
