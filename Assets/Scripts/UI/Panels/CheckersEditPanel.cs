﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class CheckersEditPanel : GamePanel
{
    public ScrollRect scrollRect;
    [SerializeField]
    private CheckersSettingsData settings = new CheckersSettingsData();
    public TMP_Dropdown boardSizeDropdown;
    public BuildGridUI boardPreview;
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
    private void Start()
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
            boardSizeDropdown.onValueChanged.AddListener(DropdownValueChanged);
        }
        if (boardPreview)
        {
            boardPreview.columns = settings.columns;
            boardPreview.rows = settings.rows;
            boardPreview.CreateGrid();
        }
        if (piecesByRow)
        {
            piecesByRow.maxValue = settings.columns / 2 - 1;
            piecesByRow.minValue = 1;
            piecesByRow.value = settings.piecesByRow;
            piecesByRow.OnValueChanged.AddListener(PiecesByRowChanged);
        }
        if (multiDirectionCaptureToggle)
        {
            multiDirectionCaptureToggle.isOn = settings.multiDirectionalCapture;
            multiDirectionCaptureToggle.onValueChanged.AddListener(MultiDirValueChanged);
        }
        if (piecesMovement)
        {
            piecesMovement.value = settings.pieceMoveDistance;
            piecesMovement.OnValueChanged.AddListener(PiecesMovementDistanceChanged);
        }
        if (kingInfiniteMovementDistance)
        {
            kingInfiniteMovementDistance.isOn = settings.kingInfiniteMoveDistance;
            kingInfiniteMovementDistance.onValueChanged.AddListener(KingInfiniteMovement);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CheckersSettings.instance.settings = new CheckersSettingsData(settings);
        CheckersSettings.instance.SaveSettings();
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
        }
    }

    private void PiecesByRowChanged(int value)
    {
        settings.piecesByRow = value;
        settings.piecesByRow = MathOperations.ClampMin(settings.piecesByRow, 1);
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
        settings.pieceMoveDistance = MathOperations.ClampMin(settings.pieceMoveDistance, 1);
    }

    public void ResetSettings()
    {
        settings = new CheckersSettingsData();
        if (boardSizeDropdown != null)
        {
            boardSizeDropdown.ClearOptions();
            boardSizeDropdown.AddOptions(boardSize.Keys.ToList());
            boardSizeDropdown.value = boardSizeIndex.IndexOf(settings.columns);

        }
        if (piecesByRow)
        {
            piecesByRow.value = settings.piecesByRow;

        }
        if (multiDirectionCaptureToggle)
        {
            multiDirectionCaptureToggle.isOn = settings.multiDirectionalCapture;

        }
        if (piecesMovement)
        {
            piecesMovement.value = settings.pieceMoveDistance;

        }
        if (kingInfiniteMovementDistance)
        {
            kingInfiniteMovementDistance.isOn = settings.kingInfiniteMoveDistance;

        }

        CheckersSettings.instance.settings = new CheckersSettingsData();
        CheckersSettings.instance.SaveSettings();
    }
}

