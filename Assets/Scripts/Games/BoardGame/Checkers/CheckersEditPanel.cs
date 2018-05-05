using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Sirenix.OdinInspector;
public class CheckersEditPanel : GamePanel
{
    public ScrollRect scrollRect;
    [SerializeField]
    private CheckersSettingsData settings = new CheckersSettingsData();
    public TMP_Dropdown boardSizeDropdown;
    public CheckerBoardImage boardPreview;
    [ShowInInspector, HorizontalGroup("ColorGroup")]
    public List<Color> topPlayerColors { get { return new List<Color> { Color.red, Colors.PersianRed, Colors.BlizzardBlue, Colors.OrangeCrayola, Colors.GreenLizard }; } }
    [ShowInInspector, HorizontalGroup("ColorGroup")]
    public List<Color> bottomplayerColors { get { return new List<Color> { Colors.BlackLeatherJacket, Colors.YellowMunsell, Colors.PersianPink, Colors.PurpleHeart, Colors.DutchWhite }; } }

    private readonly Dictionary<string, int> boardSize = new Dictionary<string, int>()
    {
        { "6x6", 6},
        { "8x8", 8 },
        { "10x10", 10 }
    };
    private readonly List<int> boardSizeIndex = new List<int>();
    public ValueSelectUI piecesByRow;
    public Toggle multiDirectionCaptureToggle;
    public ValueSelectUI piecesMovement;
    public Toggle kingInfiniteMovementDistance;
    [SceneObjectsOnly]
    public ColorSelectUI topPlayerColorSelect;
    [SceneObjectsOnly]
    public ColorSelectUI bottomPlayerColorSelect;
    private void Start()
    {
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


    public override IEnumerator Exit()
    {
        CheckersSettings.instance.settings = new CheckersSettingsData(settings);
        CheckersSettings.instance.SaveSettings();
        return base.Exit();
    }
    void Init()
    {
        settings = new CheckersSettingsData(CheckersSettings.instance.settings);
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
            topPlayerColorSelect.selectColors = topPlayerColors;
            topPlayerColorSelect.SetCurrentColor(settings.topPieceColor.GetColor());
            topPlayerColorSelect.UpdateUI();
            topPlayerColorSelect.OnColorSelect.RemoveAndAddListener(TopPlayerColorChanged);

        }
        if (bottomPlayerColorSelect)
        {
            bottomPlayerColorSelect.selectColors.Clear();
            bottomPlayerColorSelect.selectColors = bottomplayerColors;
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
            boardPreview.CreateGrid();
            boardPreview.rowsWithPieces = settings.piecesByRow;
            boardPreview.PlacePiecesNormal();

        }
        if (piecesByRow)
        {
            piecesByRow.maxValue = settings.columns / 2 - 1;
            piecesByRow.minValue = 1;
            piecesByRow.value = settings.piecesByRow;
            piecesByRow.OnValueChanged.RemoveAndAddListener(PiecesByRowChanged);
        }
        if (multiDirectionCaptureToggle)
        {
            multiDirectionCaptureToggle.isOn = settings.multiDirectionalCapture;
            multiDirectionCaptureToggle.onValueChanged.RemoveAndAddListener(MultiDirValueChanged);
        }
        if (piecesMovement)
        {
            piecesMovement.value = settings.pieceMoveDistance;
            piecesMovement.OnValueChanged.RemoveAndAddListener(PiecesMovementDistanceChanged);
        }
        if (kingInfiniteMovementDistance)
        {
            kingInfiniteMovementDistance.isOn = settings.kingInfiniteMoveDistance;
            kingInfiniteMovementDistance.onValueChanged.RemoveAndAddListener(KingInfiniteMovement);
        }
    }

    private void DropdownValueChanged(int newPosition)
    {
        int realValue = boardSize.Values.ElementAt(newPosition);
        settings.columns = settings.rows = realValue;
        if (piecesByRow)
        {
            piecesByRow.maxValue = settings.columns / 2 - 1;
            piecesByRow.minValue = 1;
        }
        if (boardPreview)
        {
            boardPreview.columns = boardPreview.rows = realValue;
            boardPreview.CreateGrid();
            boardPreview.PlacePiecesNormal();
        }
    }

    private void PiecesByRowChanged(int value)
    {
        settings.piecesByRow = value;
        settings.piecesByRow = UtilityFunctions.ClampMin(settings.piecesByRow, 1);
        if (boardPreview)
        {
            boardPreview.rowsWithPieces = value;
            boardPreview.PlacePiecesNormal();
        }
    }

    private void TopPlayerColorChanged(Color c)
    {
        if (boardPreview)
        {
            boardPreview.topPieceColor = c;
            boardPreview.UpdateGrid();

        }
        settings.topPieceColor = c;
    }
    private void BottomPlayerColorChanged(Color c)
    {
        if (boardPreview)
        {
            boardPreview.bottomPieceColor = c;
            boardPreview.UpdateGrid();

        }
        settings.bottomPieceColor = c;
    }
    private void MultiDirValueChanged(bool toggle)
    {
        settings.multiDirectionalCapture = toggle;
    }
    private void KingInfiniteMovement(bool toggle)
    {
        settings.kingInfiniteMoveDistance = toggle;
    }
    private void PiecesMovementDistanceChanged(int value)
    {
        settings.pieceMoveDistance = value;
        settings.pieceMoveDistance = UtilityFunctions.ClampMin(settings.pieceMoveDistance, 1);
    }

    public void ResetSettings()
    {

        CheckersSettings.instance.settings = new CheckersSettingsData();
        Init();
        CheckersSettings.instance.SaveSettings();
    }
}

