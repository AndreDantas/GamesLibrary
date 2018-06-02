using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class DotsAndBoxesEditPanel : BoardgameEditPanel
{


    protected override void GetSettings()
    {
        boardSize = new Dictionary<string, object>()
    {
        { "3x3", 3},
        { "4x4", 4},
        { "5x5", 5},
        { "6x6", 6}
    };
        topPlayerColors = new List<Color> { new DotsAndBoxesSettingsData().topPieceColor, Colors.PurpleHeart, Colors.SpanishYellow, Colors.CyanProcess, Colors.BlackBean };
        bottomplayerColors = new List<Color> { new DotsAndBoxesSettingsData().bottomPieceColor, Colors.PinkLavender, Colors.GreenLizard, Colors.VenetianRed, Colors.BrownTraditional };

        settings = new DotsAndBoxesSettingsData((DotsAndBoxesSettingsData)BoardGameSettings.instance.settings);
    }

    protected override void SaveSettings()
    {
        BoardGameSettings.instance.settings = new DotsAndBoxesSettingsData((DotsAndBoxesSettingsData)settings);
        BoardGameSettings.instance.SaveSettings();
    }

    protected override void ExtraSettings()
    {

    }

    protected override void BottomPlayerColorChanged(Color c)
    {
        if (boardPreview)
        {
            boardPreview.bottomPieceColor = c;
            boardPreview.UpdateGrid();

        }
        settings.bottomPieceColor = c;
    }

    public override void ResetSettings()
    {
        BoardGameSettings.instance.settings = new DotsAndBoxesSettingsData();
        Init();
        if (topPlayerColorSelect)
        {

            topPlayerColorSelect.SetCurrentColor(0);
            topPlayerColorSelect.UpdateUI();

        }
        if (bottomPlayerColorSelect)
        {
            bottomPlayerColorSelect.SetCurrentColor(0);
            bottomPlayerColorSelect.UpdateUI();
        }
        BoardGameSettings.instance.SaveSettings();
    }

}
