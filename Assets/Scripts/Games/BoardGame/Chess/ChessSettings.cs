using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
[System.Serializable]
public class ChessSettingsData
{
    [SerializeField]
    public ChessGameMode gameMode = ChessGameMode.Normal;
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
    public bool random = false;
    [HideInInspector]
    public SerializableColor darkTileColor = new SerializableColor(new Color(0.404f, 0.404f, 0.404f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _darkTileColor
    {
        get { return darkTileColor != null ? darkTileColor.GetColor() : new Color(0.404f, 0.404f, 0.404f, 1f); }
        set { darkTileColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor lightTileColor = new SerializableColor(new Color(0.691f, 0.691f, 0.691f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _lightileColor
    {
        get { return lightTileColor != null ? lightTileColor.GetColor() : new Color(0.691f, 0.691f, 0.691f, 1f); }
        set { lightTileColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor topPieceColor = new SerializableColor(new Color(0.243f, 0.243f, 0.243f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _topPieceColor
    {
        get { return topPieceColor != null ? topPieceColor.GetColor() : new Color(0.243f, 0.243f, 0.243f, 1f); }
        set { topPieceColor = new SerializableColor(value); }
    }
    [HideInInspector]
    public SerializableColor bottomPieceColor = new SerializableColor(new Color(0.89f, 0.883f, 0.883f, 1f));
    [ShowInInspector, BoxGroup("Colors")]
    private Color _bottomPieceColor
    {
        get { return bottomPieceColor != null ? bottomPieceColor.GetColor() : new Color(0.89f, 0.883f, 0.883f, 1f); }
        set { bottomPieceColor = new SerializableColor(value); }
    }
    public List<string> PiecesNames { get { return new List<string> { "pawn", "rook", "knight", "bishop", "king", "queen" }; } }
    public static string GetPieceName(ChessPieceType type)
    {
        string result = "";
        switch (type)
        {
            case ChessPieceType.PAWN:
                result = "pawn";
                break;
            case ChessPieceType.KNIGHT:
                result = "knight";
                break;
            case ChessPieceType.BISHOP:
                result = "bishop";
                break;
            case ChessPieceType.ROOK:
                result = "rook";
                break;
            case ChessPieceType.KING:
                result = "king";
                break;
            case ChessPieceType.QUEEN:
                result = "queen";
                break;
            default:
                break;
        }
        return result;
    }
    [ValueDropdown("PiecesNames")]
    [ShowIf("gameMode", ChessGameMode.Mini)]
    public string removedPiece = "bishop";
    [ValueDropdown("PiecesNames")]
    [ShowIf("gameMode", ChessGameMode.Omega)]
    public string addedPiece = "knight";

    public ChessSettingsData()
    {
        columns = 8;
        rows = 8;
        random = false;
        darkTileColor = new SerializableColor(new Color(0.404f, 0.404f, 0.404f, 1f));
        lightTileColor = new SerializableColor(new Color(0.691f, 0.691f, 0.691f, 1f));
        topPieceColor = new SerializableColor(new Color(0.243f, 0.243f, 0.243f, 1f));
        bottomPieceColor = new SerializableColor(new Color(0.89f, 0.883f, 0.883f, 1f));
        removedPiece = "bishop";
        addedPiece = "knight";
        gameMode = ChessGameMode.Normal;
    }
    public ChessSettingsData(ChessSettingsData other)
    {
        columns = other.columns;
        rows = other.rows;
        random = other.random;
        darkTileColor = other.darkTileColor;
        lightTileColor = other.lightTileColor;
        topPieceColor = other.topPieceColor;
        bottomPieceColor = other.bottomPieceColor;
        removedPiece = other.removedPiece;
        addedPiece = other.addedPiece;
        gameMode = other.gameMode;
    }
}
public class ChessSettings : MonoBehaviour
{


    public static ChessSettings instance;
    public ChessSettingsData settings = new ChessSettingsData();
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void SaveSettings()
    {
        SaveLoad.SaveFile("/chess_settings.dat", settings);
    }
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void LoadSettings()
    {
        ChessSettingsData load = SaveLoad.LoadFile<ChessSettingsData>("/chess_settings.dat");
        if (load != null)
        {
            settings = load;

        }
        else
            settings = new ChessSettingsData();
    }
    [ButtonGroup("G1")]
    [Button(ButtonSizes.Medium)]
    public void ResetSettings()
    {
        settings = new ChessSettingsData();
        SaveSettings();
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
