using System;
using System.Collections.Generic;
using System.Linq;
using Minesweeper.EventArgs;
using MineSweeper.Game.Minesweeper;
using MineSweeper.Models;
using MineSweeper.View;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MineGridView : MonoBehaviour
{
    #region Properties

    private MineCellView[,] mineElements;

    [SerializeField] public GameObject mineCell;

    private float scale = .4f;

    private float gap = .2f;

    private float leftOffset, topOffset;

    private IMineGridPresenter _presenter;

    #endregion

    #region Event Handler Subscription

    private void OnGameWin(object sender, EventArgs e) => ShowWin();

    private void OnMineClicked(object sender, EventArgs e) => ShowLost();

    private void OnCellClickHandled(object sender, CellClickedEventArgs e)
    {
        if (e.IsFirstClick)
        {
            HandleMineGenerated();
        }

        if (e.MineCount > 0)
        {
            mineElements[e.X, e.Y].ShowMineCount(e.MineCount);
        }
        else
        {
            RevealAdjacentCells(e.AdjacentCells);
        }
    }

    #endregion

    #region MonoBehvariour Lifecycle

    void Awake()
    {
        _presenter.GenerateMineGrid();
        _presenter.OnGameWin += OnGameWin;
        _presenter.OnCellClickHandled += OnCellClickHandled;
        _presenter.OnMineClicked += OnMineClicked;
        mineElements = new MineCellView[_presenter.GetRows(), _presenter.GetColumns()];
        DisplayMineGrid();
    }

    public void Update()
    {
        HandleLeftClick();
    }

    private void OnDestroy()
    {
        _presenter.OnGameWin -= OnGameWin;
        _presenter.OnCellClickHandled -= OnCellClickHandled;
        _presenter.OnMineClicked -= OnMineClicked;
    }

    #endregion

    #region Dependency Injection

    [Inject]
    private void Init(IMineGridPresenter presenter)
    {
        _presenter = presenter;
    }

    #endregion

    #region Game Play

    private void ShowWin()
    {
        foreach (var cell in mineElements)
        {
            cell.ShowWin();
        }
    }

    private void ShowLost()
    {
        foreach (var cell in mineElements)
        {
            cell.ShowLost();
        }
    }

    private void HandleMineGenerated()
    {
        UpdateMineCountForView();
        PlaceMineCells();
    }

    private void PlaceMineCells()
    {
        for (int i = 0; i < _presenter.GetRows(); i++)
        {
            for (int j = 0; j < _presenter.GetColumns(); j++)
            {
                MineCellView cell = mineElements[i, j];
                cell.IsMine = _presenter.GetMineCell(i, j).IsMine;
            }
        }
    }

    private void DisplayMineGrid()
    {
        float xpos, ypos, zpos = 89;

        leftOffset = _presenter.GetColumns() * (scale + gap) / 2f - scale;
        topOffset = _presenter.GetRows() * (scale + gap) / 2f;

        for (int i = 0; i < _presenter.GetRows(); i++)
        {
            for (int j = 0; j < _presenter.GetColumns(); j++)
            {
                GameObject mineGrid = new GameObject("mineGrid");
                GameObject go;
                xpos = -leftOffset + j * (scale + gap);
                ypos = -topOffset + i * (scale + gap);
                go = Instantiate(mineCell, new Vector3(xpos, ypos, zpos), Quaternion.identity) as GameObject;
                go.transform.parent = mineGrid.transform;
                go.transform.localScale = new Vector3(scale, scale, scale);
                MineCellView cell = go.GetComponent<MineCellView>();
                cell.X = i;
                cell.Y = j;
                cell.Id = _presenter.GetMineCell(i, j).Id;
                cell.IsMine = _presenter.GetMineCell(i, j).IsMine;

                mineElements[i, j] = cell;
            }
        }
    }

    private void HandleLeftClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                MineCellView mineCellCLicked = hit.collider.gameObject.GetComponent<MineCellView>();
                if (mineCellCLicked != null && !mineCellCLicked.IsCellPlayed && !_presenter.HasGameEnded())
                {
                    _presenter.CellClicked(mineCellCLicked.X, mineCellCLicked.Y);
                }
            }
        }
    }

    private void RevealAdjacentCells(List<MineCell> adjacentCells)
    {
        foreach (var cell in adjacentCells)
        {
            mineElements[cell.X, cell.Y].ShowMineCount(cell.MineCount);
        }
    }

    private void UpdateMineCountForView()
    {
        var mineCells = _presenter.GetMineCells();
        foreach (var cell in mineCells)
        {
            mineElements[cell.X, cell.Y].MineCount = mineCells[cell.X, cell.Y].MineCount;
        }
    }

    #endregion
}