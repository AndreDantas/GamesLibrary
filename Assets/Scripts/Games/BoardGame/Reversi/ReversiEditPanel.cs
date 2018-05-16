using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ReversiEditPanel : BoardgameEditPanel
{
    protected override void SaveSettings()
    {
        BoardGameSettings.instance.settings = new ReversiSettingsData((ReversiSettingsData)settings);
        BoardGameSettings.instance.SaveSettings();

    }

    protected override void GetSettings()
    {
        boardSize = new Dictionary<string, object>()
    {
        { "6x6", 6},
        { "8x8", 8 },
        { "10x10", 10 },
        { "12x12", 12 }

    };
        settings = new ReversiSettingsData((ReversiSettingsData)BoardGameSettings.instance.settings);

    }

    public override void ResetSettings()
    {
        BoardGameSettings.instance.settings = new ReversiSettingsData();
        Init();
        BoardGameSettings.instance.SaveSettings();

    }
}
