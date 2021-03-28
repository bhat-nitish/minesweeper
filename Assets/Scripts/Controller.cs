using System;
using System.Collections.Generic;
using System.Linq;
using MineSweeper.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    private int rows = 15;
    private int columns = 15;
    private int mineCount = 2;

    private MineCell[,] mineCells;
    private int[,] mineLocations;

    private MineCellView[,] mineElements;

    //minecell prefab
    [SerializeField] public GameObject mineCell;

    //block scale
    private float scale = .4f;

    //gap between blocks
    private float gap = .2f;

    //offset from center
    private float leftOffset, topOffset;

    private int cellsPlayedCount = 0;

    private int totalCells = 0;

    private bool gameEnd;

    void Awake()
    {
        mineCells = new MineCell[rows, columns];
        mineLocations = new int[rows, columns];
        mineElements = new MineCellView[rows, columns];
        totalCells = rows * columns;
        GenerateMineGrid();
        DisplayMineGrid();
    }

    private bool Checkwin()
    {
        return cellsPlayedCount == (totalCells - mineCount);
    }

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

    private bool InGrid(int x, int y) => (x >= 0 && x < rows) && (y >= 0 && y < columns);

    public void GenerateMineLocations(int row, int column)
    {
        mineLocations = new int[rows, columns];
        int generatedMines = 0;
        while (generatedMines < mineCount)
        {
            int randomMineRow = Random.Range(0, rows);
            int randomMineColumn = Random.Range(0, columns);
            if (mineLocations[randomMineRow, randomMineColumn] == 1 ||
                (randomMineRow == row && randomMineColumn == column))
            {
                // mine aleady present
                continue;
            }
            else
            {
                mineLocations[randomMineRow, randomMineColumn] = 1;
                mineCells[randomMineRow, randomMineColumn].IsMine = true;
                generatedMines++;
            }
        }
    }

    public void PlaceMineCells()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                MineCellView cell = mineElements[i, j];
                cell.IsMine = mineCells[i, j].IsMine;
            }
        }
    }

    public void GenerateMineGrid()
    {
        int id = 1;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                mineCells[i, j] = new MineCell()
                {
                    Id = id++,
                    X = i,
                    Y = j,
                    IsMine = mineLocations[i, j] == 1
                };
            }
        }
    }

    public void DisplayMineGrid()
    {
        float xpos, ypos, zpos = 89;

        leftOffset = columns * (scale + gap) / 2f - scale;
        topOffset = rows * (scale + gap) / 2f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
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
                cell.Id = mineCells[i, j].Id;
                cell.IsMine = mineCells[i, j].IsMine;

                mineElements[i, j] = cell;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleLeftClick();
    }

    public void HandleLeftClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                MineCellView mineCellCLicked = hit.collider.gameObject.GetComponent<MineCellView>();
                if (mineCellCLicked != null && !mineCellCLicked.IsCellPlayed && !gameEnd)
                {
                    if (cellsPlayedCount == 0)
                    {
                        GenerateMineLocations(mineCellCLicked.X, mineCellCLicked.Y);
                        UpdateMineCount();
                        UpdateMineCountForView();
                        PlaceMineCells();
                        cellsPlayedCount++;
                        var adjacentCellsTraversed = TraverseAdjacentCells(new List<MineCell>(),
                            mineCellCLicked.X, mineCellCLicked.Y);
                        RevealAdjacentCells(adjacentCellsTraversed);
                        mineCells[mineCellCLicked.X, mineCellCLicked.Y].IsCellPlayed = true;
                        mineElements[mineCellCLicked.X, mineCellCLicked.Y].IsCellPlayed = true;
                    }
                    else
                    {
                        if (mineCellCLicked.IsMine)
                        {
                            foreach (var mineCellView in mineElements)
                            {
                                mineCellView.IsCellPlayed = true;
                            }

                            ShowLost();
                        }
                        else
                        {
                            if (mineCellCLicked.MineCount > 0)
                            {
                                cellsPlayedCount++;
                                mineCellCLicked.ShowMineCount(mineCellCLicked.MineCount);
                            }
                            else
                            {
                                cellsPlayedCount++;
                                var adjacentCellsTraversed = TraverseAdjacentCells(new List<MineCell>(),
                                    mineCellCLicked.X, mineCellCLicked.Y);
                                RevealAdjacentCells(adjacentCellsTraversed);
                            }
                        }

                        mineElements[mineCellCLicked.X, mineCellCLicked.Y].IsCellPlayed = true;
                        mineCells[mineCellCLicked.X, mineCellCLicked.Y].IsCellPlayed = true;
                    }

                    bool isWin = Checkwin();
                    if (isWin)
                    {
                        gameEnd = true;
                        ShowWin();
                    }
                }
            }
        }
    }

    private List<MineCell> TraverseAdjacentCells(List<MineCell> cells, int row, int column)
    {
        if (mineCells[row, column].IsMine || mineCells[row, column].IsCellPlayed)
        {
            return cells;
        }

        Stack<MineCell> traversedCells = new Stack<MineCell>();
        traversedCells.Push(mineCells[row, column]);
        cells.Add(mineCells[row, column]);
        List<MineCell> adjacentCells = new List<MineCell>();
        while (traversedCells.Any())
        {
            MineCell cell = traversedCells.Pop();
            mineCells[row, column].IsCellPlayed = true;
            if (cell.MineCount == 0)
            {
                adjacentCells = GetAdjacentMineCells(cell.X, cell.Y);
                adjacentCells = adjacentCells.Where(a => !a.IsMine).ToList();
                foreach (var adjacentCell in adjacentCells)
                {
                    if (!adjacentCell.IsCellPlayed)
                    {
                        cellsPlayedCount++;
                        TraverseAdjacentCells(cells, adjacentCell.X, adjacentCell.Y);
                    }
                }
            }
        }

        if (adjacentCells.Count > 0)
            cells.AddRange(adjacentCells);

        return cells;
    }

    private void RevealAdjacentCells(List<MineCell> adjacentCells)
    {
        foreach (var cell in adjacentCells)
        {
            mineElements[cell.X, cell.Y].ShowMineCount(cell.MineCount);
        }
    }

    private void RevealMineGrid()
    {
        foreach (var cell in mineCells)
        {
            mineElements[cell.X, cell.Y].ShowMineCount(cell.MineCount);
        }
    }

    private void UpdateMineCount()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var adjacentCells = GetAdjacentMineCells(i, j);
                mineCells[i, j].MineCount = CalculateMineCountForAdjacentCells(adjacentCells);
            }
        }
    }

    private void UpdateMineCountForView()
    {
        foreach (var cell in mineCells)
        {
            mineElements[cell.X, cell.Y].MineCount = mineCells[cell.X, cell.Y].MineCount;
        }
    }

    private List<MineCell> GetAdjacentMineCells(int row, int column)
    {
        List<MineCell> adjacentCells = new List<MineCell>();

        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = column - 1; j <= column + 1; j++)
            {
                if ((i != row || j != column) && (InGrid(i, j) && !mineCells[i, j].IsCellPlayed))
                {
                    adjacentCells.Add(mineCells[i, j]);
                }
            }
        }

        return adjacentCells;
    }

    private int CalculateMineCountForAdjacentCells(List<MineCell> adjacentCells) =>
        adjacentCells.Where(c => c.IsMine).Count();
}