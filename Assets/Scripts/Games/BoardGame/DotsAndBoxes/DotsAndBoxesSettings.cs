using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class DotsAndBoxesSettingsData : BoardGameSettingsData
{
    public DotsAndBoxesSettingsData() : base()
    {
        columns = 4;
        rows = 4;
        topPieceColor = Colors.Blueberry;
        bottomPieceColor = Colors.PersianRed;
    }

    public DotsAndBoxesSettingsData(BoardGameSettingsData other) : base(other)
    {
    }
}
public class DotsAndBoxesSettings : BoardGameSettings
{

    public override void LoadSettings()
    {

        DotsAndBoxesSettingsData load = SaveLoad.LoadFile<DotsAndBoxesSettingsData>("/" + saveName + "_settings.dat");
        if (load != null)
        {
            settings = load;
        }
    }

    protected override void Awake()
    {
        saveName = "dots_and_boxes";
        base.Awake();
    }

    public override void ResetSettings()
    {
        settings = new DotsAndBoxesSettingsData();
        SaveSettings();
    }
}
