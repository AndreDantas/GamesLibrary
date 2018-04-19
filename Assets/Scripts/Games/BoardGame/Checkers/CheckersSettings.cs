using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CheckersSettingsData
{
    public int columns = 8;
    public int rows = 8;
    public int piecesByRow = 3;
    public int pieceMoveDistance = 1;
    public bool kingInfiniteMoveDistance = true;
    public bool multiDirectionalCapture = true;

    public CheckersSettingsData()
    {
        columns = 8;
        rows = 8;
        piecesByRow = 3;
        pieceMoveDistance = 1;
        kingInfiniteMoveDistance = true;
        multiDirectionalCapture = true;
    }
    public CheckersSettingsData(CheckersSettingsData other)
    {
        columns = other.columns;
        rows = other.rows;
        pieceMoveDistance = other.pieceMoveDistance;
        piecesByRow = other.piecesByRow;
        kingInfiniteMoveDistance = other.kingInfiniteMoveDistance;
        multiDirectionalCapture = other.multiDirectionalCapture;
    }
}
public class CheckersSettings : MonoBehaviour
{
    public static CheckersSettings instance;
    public CheckersSettingsData settings = new CheckersSettingsData();
    public void SaveSettings()
    {
        SaveLoad.SaveFile("/checkers_settings.dat", settings);
    }

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
