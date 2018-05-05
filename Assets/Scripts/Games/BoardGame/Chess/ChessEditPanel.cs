using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;
public class ChessEditPanel : GamePanel
{
    public ScrollRect scrollRect;
    [SerializeField]
    private ChessSettingsData settings;
    public ChessBoardImage boardPreview;

    [ShowInInspector, HorizontalGroup("ColorGroup")]
    public List<Color> lightTileColors
    {
        get
        {
            return new List<Color> { new Color(0.691f, 0.691f, 0.691f, 1f),
                                new Color32(177, 228, 185, 255),
                                new Color32(204,183,174, 255),
                                new Color32(157,172,255, 255),
                                new Color32(234,240,206, 255),
                                new Color32(173,189,143, 255),
                                new Color32(227,193,111, 255)};
        }
    }
    [ShowInInspector, HorizontalGroup("ColorGroup")]
    public List<Color> darkTileColors
    {
        get
        {
            return new List<Color> { new Color(0.404f, 0.404f, 0.404f, 1f),
                                    new Color32(112, 162, 163, 255),
                                    new Color32(112,102,119, 255),
                                    new Color32(111,115,210, 255),
                                    new Color32(187,190,100, 255),
                                    new Color32(111,143,114, 255),
                                    new Color32(184,139,74, 255)};
        }
    }

    public TMP_Dropdown gameModeDropdown;
    private readonly Dictionary<string, ChessGameMode> gameMode = new Dictionary<string, ChessGameMode>()
    {
        { "Mini", ChessGameMode.Mini},
        { "Normal", ChessGameMode.Normal},
        { "Omega", ChessGameMode.Omega }
    };
    [SceneObjectsOnly]
    public OptionSelectUI removedPiece;
    [SceneObjectsOnly]
    public OptionSelectUI addedPiece;
    [SceneObjectsOnly]
    public ColorSelectUI lightTileColorSelect;
    [SceneObjectsOnly]
    public ColorSelectUI darkTileColorSelect;

    private readonly List<ChessGameMode> gameModeIndex = new List<ChessGameMode>();
    private List<string> DisplayNameBR { get { return new List<string>() { "Torre", "Cavalo", "Bispo" }; } }
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        settings = new ChessSettingsData(ChessSettings.instance.settings);

        gameModeIndex.Clear();
        foreach (var item in gameMode.ToList())
        {
            gameModeIndex.Add(item.Value);
        }
        if (gameModeDropdown != null)
        {
            gameModeDropdown.ClearOptions();
            gameModeDropdown.AddOptions(gameMode.Keys.ToList());
            gameModeDropdown.value = gameModeIndex.IndexOf(settings.gameMode);
            gameModeDropdown.onValueChanged.RemoveAndAddListener(DropdownValueChanged);
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
            switch (settings.gameMode)
            {
                case ChessGameMode.Mini:
                    boardPreview.PlacePiecesMiniChess(settings.removedPiece);
                    break;
                case ChessGameMode.Normal:
                    boardPreview.PlacePiecesNormal();
                    break;
                case ChessGameMode.Omega:
                    boardPreview.PlacePiecesOmegaChess(settings.addedPiece);
                    break;

            }


        }
        if (addedPiece)
        {
            if (settings.gameMode == ChessGameMode.Omega)
            {
                addedPiece.gameObject.SetActive(true);
                List<StringObjectPair> options = new List<StringObjectPair>();
                List<string> piecesNames = new List<string>() { "rook", "knight", "bishop" };
                for (int i = 0; i < DisplayNameBR.Count; i++)
                {
                    options.Add(new StringObjectPair(DisplayNameBR[i], piecesNames[i]));
                }
                addedPiece.options = options;
                addedPiece.SetCurrentOption(piecesNames.IndexOf(settings.addedPiece));
                addedPiece.OnOptionChanged.RemoveAndAddListener(AddedPieceChanged);
                addedPiece.UpdateUI();
            }
            else
            {
                addedPiece.gameObject.SetActive(false);
            }

        }
        if (removedPiece)
        {
            if (settings.gameMode == ChessGameMode.Mini)
            {
                removedPiece.gameObject.SetActive(true);
                List<StringObjectPair> options = new List<StringObjectPair>();
                List<string> piecesNames = new List<string>() { "rook", "knight", "bishop" };
                for (int i = 0; i < DisplayNameBR.Count; i++)
                {
                    options.Add(new StringObjectPair(DisplayNameBR[i], piecesNames[i]));
                }
                removedPiece.options = options;
                removedPiece.SetCurrentOption(piecesNames.IndexOf(settings.removedPiece));
                removedPiece.OnOptionChanged.RemoveAndAddListener(RemovedPieceChanged);
                removedPiece.UpdateUI();
            }
            else
            {
                removedPiece.gameObject.SetActive(false);
            }

        }
        if (lightTileColorSelect)
        {
            lightTileColorSelect.selectColors.Clear();
            lightTileColorSelect.selectColors = lightTileColors;
            lightTileColorSelect.SetCurrentColor(settings.lightTileColor.GetColor());
            lightTileColorSelect.UpdateUI();
            lightTileColorSelect.OnColorSelect.RemoveAndAddListener(LightTileColorChanged);

        }
        if (darkTileColorSelect)
        {
            darkTileColorSelect.selectColors.Clear();
            darkTileColorSelect.selectColors = darkTileColors;
            darkTileColorSelect.SetCurrentColor(settings.darkTileColor.GetColor());
            darkTileColorSelect.UpdateUI();
            darkTileColorSelect.OnColorSelect.RemoveAndAddListener(DarkTileColorChanged);

        }
    }
    private void DropdownValueChanged(int newPosition)
    {
        ChessGameMode mode = gameMode.Values.ElementAt(newPosition);
        settings.gameMode = mode;
        switch (mode)
        {
            case ChessGameMode.Mini:
                boardPreview.columns = boardPreview.rows = settings.columns = settings.rows = 6;
                boardPreview.CreateGrid();
                boardPreview.PlacePiecesMiniChess(settings.removedPiece);
                break;
            case ChessGameMode.Normal:
                boardPreview.columns = boardPreview.rows = settings.columns = settings.rows = 8;
                boardPreview.CreateGrid();
                boardPreview.PlacePiecesNormal();
                break;
            case ChessGameMode.Omega:
                boardPreview.columns = boardPreview.rows = settings.columns = settings.rows = 10;
                boardPreview.CreateGrid();
                boardPreview.PlacePiecesOmegaChess(settings.addedPiece);
                break;
            default:
                break;
        }
        if (addedPiece)
        {
            if (settings.gameMode == ChessGameMode.Omega)
            {
                addedPiece.gameObject.SetActive(true);
                List<StringObjectPair> options = new List<StringObjectPair>();
                List<string> piecesNames = new List<string>() { "rook", "knight", "bishop" };
                for (int i = 0; i < DisplayNameBR.Count; i++)
                {
                    options.Add(new StringObjectPair(DisplayNameBR[i], piecesNames[i]));
                }
                addedPiece.options = options;
                addedPiece.SetCurrentOption(piecesNames.IndexOf(settings.addedPiece));
                addedPiece.OnOptionChanged.RemoveAndAddListener(AddedPieceChanged);
                addedPiece.UpdateUI();
            }
            else
            {
                addedPiece.gameObject.SetActive(false);
            }

        }
        if (removedPiece)
        {
            if (settings.gameMode == ChessGameMode.Mini)
            {
                removedPiece.gameObject.SetActive(true);
                List<StringObjectPair> options = new List<StringObjectPair>();
                List<string> piecesNames = new List<string>() { "rook", "knight", "bishop" };
                for (int i = 0; i < DisplayNameBR.Count; i++)
                {
                    options.Add(new StringObjectPair(DisplayNameBR[i], piecesNames[i]));
                }
                removedPiece.options = options;
                removedPiece.SetCurrentOption(piecesNames.IndexOf(settings.removedPiece));
                removedPiece.OnOptionChanged.RemoveAndAddListener(RemovedPieceChanged);
                removedPiece.UpdateUI();
            }
            else
            {
                removedPiece.gameObject.SetActive(false);
            }

        }
    }

    private void AddedPieceChanged(object obj)
    {
        string piece = obj as string;
        settings.addedPiece = piece;
        if (boardPreview)
            boardPreview.PlacePiecesOmegaChess(settings.addedPiece);
    }
    private void RemovedPieceChanged(object obj)
    {
        string piece = obj as string;
        settings.removedPiece = piece;
        if (boardPreview)
            boardPreview.PlacePiecesMiniChess(settings.removedPiece);
    }
    private void LightTileColorChanged(Color c)
    {
        if (boardPreview)
        {
            boardPreview.lightTile = c;
            boardPreview.UpdateGrid();

        }
        settings.lightTileColor = c;
    }
    private void DarkTileColorChanged(Color c)
    {
        if (boardPreview)
        {
            boardPreview.darkTile = c;
            boardPreview.UpdateGrid();

        }
        settings.darkTileColor = c;
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
        ChessSettings.instance.settings = new ChessSettingsData(settings);
        ChessSettings.instance.SaveSettings();
        return base.Exit();
    }

    public void ResetSettings()
    {

        ChessSettings.instance.settings = new ChessSettingsData();
        Init();
        ChessSettings.instance.SaveSettings();
    }
}
