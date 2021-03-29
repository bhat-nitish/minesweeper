using System;
using System.Collections.Generic;
using System.Linq;
using MineSweeper.Game.Minesweeper;
using MineSweeper.Models;
using UnityEngine;
using Zenject;

public class MineGridView : MonoBehaviour, IMineGridView
{
    private MineCellView[,] mineElements;

    [SerializeField] public GameObject mineCell;

    private float scale = .4f;

    private float gap = .2f;

    private float leftOffset, topOffset;

    IMineGridPresenter _presenter;

    [Inject]
    public void Init(IMineGridPresenter presenter)
    {
        _presenter = presenter;
    }

    void Awake()
    {
        _presenter.GenerateMineGrid();
        mineElements = new MineCellView[_presenter.GetRows(), _presenter.GetColumns()];
        DisplayMineGrid();
    }

    public void ShowWin()
    {
        foreach (var cell in mineElements)
        {
            cell.ShowWin();
        }
    }

    public void ShowLost()
    {
        foreach (var cell in mineElements)
        {
            cell.ShowLost();
        }
    }

    public void PlaceMineCells()
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

    public void DisplayMineGrid()
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
                //update block script data
                MineCellView cell = go.GetComponent<MineCellView>();

                //block.SpriteRenderer.sprite = sprites[SPRITE.HIDDEN];
                cell.X = i;
                cell.Y = j;
                cell.Id = _presenter.GetMineCell(i, j).Id;
                cell.IsMine = _presenter.GetMineCell(i, j).IsMine;

                mineElements[i, j] = cell;
            }
        }
    }

    void Update()
    {
        HandleLeftClick();
    }

    public void MineClicked()
    {
        foreach (var mineCellView in mineElements)
        {
            mineCellView.IsCellPlayed = true;
        }

        ShowLost();
    }

    public void HandleLeftClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                MineCellView mineCellCLicked = hit.collider.gameObject.GetComponent<MineCellView>();
                if (mineCellCLicked != null && !mineCellCLicked.IsCellPlayed && !_presenter.HasGameEnded())
                {
                    if (_presenter.GetPlayedCellsCount() == 0)
                    {
                        _presenter.GenerateMineLocations(mineCellCLicked.X, mineCellCLicked.Y);
                        _presenter.UpdateMineCount();
                        UpdateMineCountForView();
                        PlaceMineCells();
                        _presenter.IncrementPlayedCellsCount();
                        var adjacentCellsTraversed = _presenter.TraverseAdjacentCells(new List<MineCell>(),
                            mineCellCLicked.X, mineCellCLicked.Y);
                        RevealAdjacentCells(adjacentCellsTraversed);
                        _presenter.SetMineCellAsPlayed(mineCellCLicked.X, mineCellCLicked.Y);
                        mineElements[mineCellCLicked.X, mineCellCLicked.Y].IsCellPlayed = true;
                    }
                    else
                    {
                        if (mineCellCLicked.IsMine)
                        {
                            MineClicked();
                        }
                        else
                        {
                            if (mineCellCLicked.MineCount > 0)
                            {
                                _presenter.IncrementPlayedCellsCount();
                                mineCellCLicked.ShowMineCount(mineCellCLicked.MineCount);
                            }
                            else
                            {
                                _presenter.IncrementPlayedCellsCount();
                                var adjacentCellsTraversed = _presenter.TraverseAdjacentCells(new List<MineCell>(),
                                    mineCellCLicked.X, mineCellCLicked.Y);
                                RevealAdjacentCells(adjacentCellsTraversed);
                            }
                        }

                        mineElements[mineCellCLicked.X, mineCellCLicked.Y].IsCellPlayed = true;
                        _presenter.SetMineCellAsPlayed(mineCellCLicked.X, mineCellCLicked.Y);
                    }

                    bool isWin = _presenter.Checkwin();
                    if (isWin)
                    {
                        _presenter.EndGame();
                        ShowWin();
                    }
                }
            }
        }
    }

    public void RevealAdjacentCells(List<MineCell> adjacentCells)
    {
        foreach (var cell in adjacentCells)
        {
            mineElements[cell.X, cell.Y].ShowMineCount(cell.MineCount);
        }
    }

    private void RevealMineGrid()
    {
        var mineCells = _presenter.GetMineCells();
        foreach (var cell in mineCells)
        {
            mineElements[cell.X, cell.Y].ShowMineCount(cell.MineCount);
        }
    }

    public void UpdateMineCountForView()
    {
        var mineCells = _presenter.GetMineCells();
        foreach (var cell in mineCells)
        {
            mineElements[cell.X, cell.Y].MineCount = mineCells[cell.X, cell.Y].MineCount;
        }
    }
}