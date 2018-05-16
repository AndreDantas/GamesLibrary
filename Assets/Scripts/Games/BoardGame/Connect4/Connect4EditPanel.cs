using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;
public class Connect4EditPanel : BoardgameEditPanel
{
    public ValueSelectUI connectTargetSelect;
    protected override void SaveSettings()
    {
        BoardGameSettings.instance.settings = new Connect4SettingsData((Connect4SettingsData)settings);
        BoardGameSettings.instance.SaveSettings();
    }
    protected override void GetSettings()
    {
        boardSize = new Dictionary<string, object>()
    {
        { "6x5", new ObjectPair<int>(6,5)},
        { "7x6",  new ObjectPair<int>(7,6)},
        { "9x8", new ObjectPair<int>(9,8) },


    };
        topPlayerColors = new List<Color> { new Connect4SettingsData().topPieceColor, Colors.PurpleHeart, Colors.OrangePantone, Colors.BistreBrown, Colors.BlackLeatherJacket };
        bottomplayerColors = new List<Color> { new Connect4SettingsData().bottomPieceColor, Colors.PinkLavender, Colors.GreenBlue, Colors.VenetianRed, Colors.GhostWhite };

        settings = new Connect4SettingsData((Connect4SettingsData)BoardGameSettings.instance.settings);

    }

    protected override void Init()
    {
        GetSettings();
        boardSizeIndex.Clear();
        foreach (var item in boardSize.ToList())
        {
            boardSizeIndex.Add(item.Value);
        }
        if (boardSizeDropdown != null)
        {
            boardSizeDropdown.ClearOptions();
            boardSizeDropdown.AddOptions(boardSize.Keys.ToList());
            ObjectPair<int> temp = new ObjectPair<int>(settings.columns, settings.rows);

            boardSizeDropdown.value = boardSizeIndex.FindIndex(x =>
                                                    ((ObjectPair<int>)x).first == temp.first && ((ObjectPair<int>)x).second == temp.second);
            boardSizeDropdown.onValueChanged.RemoveAndAddListener(DropdownValueChanged);
        }
        if (topPlayerColorSelect)
        {
            topPlayerColorSelect.selectColors.Clear();
            topPlayerColorSelect.SetColors(topPlayerColors);
            topPlayerColorSelect.SetCurrentColor(settings.topPieceColor.GetColor());
            topPlayerColorSelect.UpdateUI();
            topPlayerColorSelect.OnColorSelect.RemoveAndAddListener(TopPlayerColorChanged);

        }
        if (bottomPlayerColorSelect)
        {
            bottomPlayerColorSelect.selectColors.Clear();
            bottomPlayerColorSelect.SetColors(bottomplayerColors);
            bottomPlayerColorSelect.SetCurrentColor(settings.bottomPieceColor.GetColor());
            bottomPlayerColorSelect.UpdateUI();
            bottomPlayerColorSelect.OnColorSelect.RemoveAndAddListener(BottomPlayerColorChanged);

        }
        if (boardPreview)
        {
            boardPreview.columns = settings.columns;
            boardPreview.rows = settings.rows;
            boardPreview.darkTile = settings.darkTileColor;
            boardPreview.lightTile = settings.lightTileColor;
            boardPreview.topPieceColor = settings.topPieceColor;
            boardPreview.bottomPieceColor = settings.bottomPieceColor;
            boardPreview.BuildBoard();
            boardPreview.PlacePiecesNormal();

        }
        ExtraSettings();
    }
    protected override void ExtraSettings()
    {

        if (connectTargetSelect)
        {
            connectTargetSelect.cycle = false;
            connectTargetSelect.minValue = 3;
            connectTargetSelect.maxValue = 5;
            connectTargetSelect.value = ((Connect4SettingsData)settings).connectTarget;
            connectTargetSelect.OnValueChanged.RemoveAndAddListener(OnConnectTargetChanged);
        }

    }
    public override void ResetSettings()
    {
        BoardGameSettings.instance.settings = new Connect4SettingsData();
        Init();
        BoardGameSettings.instance.SaveSettings();

    }

    protected void OnConnectTargetChanged(int value)
    {
        ((Connect4SettingsData)settings).connectTarget = value;
    }

    protected override void DropdownValueChanged(int newPosition)
    {
        ObjectPair<int> realValue = (ObjectPair<int>)boardSize.Values.ElementAt(newPosition);
        settings.columns = realValue.first;
        settings.rows = realValue.second;

        if (boardPreview)
        {
            boardPreview.columns = settings.columns;
            boardPreview.rows = settings.rows;
            boardPreview.BuildBoard();
            boardPreview.PlacePiecesNormal();
        }
    }
}
