using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Serialization;
public class BoardgameEditPanel : GamePanel
{
    public ScrollRect scrollRect;
    [SerializeField]
    protected BoardGameSettingsData settings = new BoardGameSettingsData();
    public TMP_Dropdown boardSizeDropdown;
    public BoardImage boardPreview;
    [ShowInInspector, HorizontalGroup("ColorGroup"), OdinSerialize]
    public List<Color> topPlayerColors { get; set; } //= new List<Color> { Colors.GhostWhite, Colors.BrownChocolate, Colors.BlizzardBlue, Colors.OrangeCrayola, Colors.CyanCornflowerBlue };
    [ShowInInspector, HorizontalGroup("ColorGroup"), OdinSerialize]
    public List<Color> bottomplayerColors { get; set; } //= new List<Color> { Colors.BlackLeatherJacket, Colors.YellowMunsell, Colors.PinkFlamingo, Colors.PurpleHeart, Colors.FireEngineRed };

    protected Dictionary<string, int> boardSize = new Dictionary<string, int>()
    {
        { "6x6", 6},
        { "8x8", 8 },
        { "10x10", 10 }
    };
    protected List<int> boardSizeIndex = new List<int>();

    public ColorSelectUI topPlayerColorSelect;
    public ColorSelectUI bottomPlayerColorSelect;
    protected virtual void Start()
    {

        if (scrollRect == null)
            scrollRect = GetComponentInChildren<ScrollRect>();
        Init();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
    }

    protected virtual void SaveSettings()
    {
        BoardGameSettings.instance.settings = new BoardGameSettingsData(settings);
        BoardGameSettings.instance.SaveSettings();
    }

    public override IEnumerator Exit()
    {
        SaveSettings();
        return base.Exit();
    }

    protected virtual void GetSettings()
    {
        settings = new BoardGameSettingsData(BoardGameSettings.instance.settings);
    }

    protected virtual void Init()
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
            boardSizeDropdown.value = boardSizeIndex.IndexOf(settings.columns);
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

    }

    protected virtual void DropdownValueChanged(int newPosition)
    {
        int realValue = boardSize.Values.ElementAt(newPosition);
        settings.columns = settings.rows = realValue;

        if (boardPreview)
        {
            boardPreview.columns = boardPreview.rows = realValue;
            boardPreview.BuildBoard();
            boardPreview.PlacePiecesNormal();
        }
    }

    protected virtual void TopPlayerColorChanged(Color c)
    {
        if (boardPreview)
        {
            boardPreview.topPieceColor = c;
            boardPreview.UpdateGrid();

        }
        settings.topPieceColor = c;
    }
    protected virtual void BottomPlayerColorChanged(Color c)
    {
        if (boardPreview)
        {
            boardPreview.bottomPieceColor = c;
            boardPreview.UpdateGrid();

        }
        settings.bottomPieceColor = c;
    }

    public virtual void ResetSettings()
    {

        BoardGameSettings.instance.settings = new BoardGameSettingsData();
        Init();
        BoardGameSettings.instance.SaveSettings();
    }

}
