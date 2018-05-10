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
        settings = new ReversiSettingsData((ReversiSettingsData)BoardGameSettings.instance.settings);

    }

    public override void ResetSettings()
    {
        BoardGameSettings.instance.settings = new ReversiSettingsData();
        Init();
        BoardGameSettings.instance.SaveSettings();

    }
}
