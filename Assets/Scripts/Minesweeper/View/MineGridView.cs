using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Base;
using Minesweeper.EventArgs;
using MineSweeper.Game.Minesweeper;
using MineSweeper.Models;
using MineSweeper.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MineGridView : MonoBehaviour
{
    #region Properties

    private MineCellView[,] mineElements;

    [SerializeField] public GameObject mineCell;

    public Button mainMenuBtn;

    public GameObject tickingBombAudio;

    public GameObject cellClickAudio;

    public GameObject mineClickAudio;

    public GameObject gameWonAudio;

    public TextMeshProUGUI timer;

    public TextMeshProUGUI mineCountDisplay;

    private TickingBombAudio _tickingBombAudio;

    private CellClickedAudio _cellClickedAudio;

    private MineClickedAudio _mineClickedAudio;

    private GameWonAudio _gameWonAudio;

    private TimeSpan _startTime;

    private float scale = .4f;

    private float gap = .2f;

    private float leftOffset, topOffset;

    private IMineGridPresenter _presenter;

    private DiContainer _container;

    private int minutes, seconds, milliseconds;

    private bool gameInProgress;

    #endregion

    #region Event Handler Subscription

    private void OnGameWin(object sender, EventArgs e) => ShowWin();

    private void OnMineClicked(object sender, EventArgs e) => ShowLost();

    private void OnCellClickHandled(object sender, CellClickedEventArgs e)
    {
        _cellClickedAudio.Play();
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

    private void OnGameStarted(object sender, EventArgs e)
    {
        gameInProgress = true;
        _startTime = DateTime.UtcNow.TimeOfDay;
        StartCoroutine(RunTimer());
    }

    #endregion

    #region MonoBehvariour Lifecycle

    void Awake()
    {
        _presenter.GenerateMineGrid();
        _presenter.OnGameWin += OnGameWin;
        _presenter.OnCellClickHandled += OnCellClickHandled;
        _presenter.OnMineClicked += OnMineClicked;
        _presenter.GameStarted += OnGameStarted;
        mainMenuBtn.onClick.AddListener(ReturnToMiainMenu);
        mineElements = new MineCellView[_presenter.GetRows(), _presenter.GetColumns()];
        DisplayMineGrid();
        DisplayMineCount();
        InitializeAudio();
    }

    private void UpdateTimer()
    {
        var timeElapsed = DateTime.Now - _startTime;
        timer.SetText($"{timeElapsed.Hour - 1:00}:{timeElapsed.Minute:00}:{timeElapsed.Second:00}");
    }

    private void DisplayMineCount()
    {
        mineCountDisplay.SetText("Mine Count : " + _presenter.GetMineCount());
    }

    private void InitializeAudio()
    {
        var tickingBombAudioClip = Instantiate(tickingBombAudio);
        _tickingBombAudio = tickingBombAudioClip.GetComponent<TickingBombAudio>();

        var cellClickedAudioClip = Instantiate(cellClickAudio);
        _cellClickedAudio = cellClickedAudioClip.GetComponent<CellClickedAudio>();

        var mineClickedAudioClip = Instantiate(mineClickAudio);
        _mineClickedAudio = mineClickedAudioClip.GetComponent<MineClickedAudio>();

        var gameWonAudioClip = Instantiate(gameWonAudio);
        _gameWonAudio = gameWonAudioClip.GetComponent<GameWonAudio>();
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
    private void Init(IMineGridPresenter presenter, DiContainer container)
    {
        _presenter = presenter;
        _container = container;
    }

    #endregion

    #region Game Play

    private void ShowWin()
    {
        _gameWonAudio.Play();
        gameInProgress = false;
        StartCoroutine(ShowWinWithDelay(0.2f));
        StopCoroutine(RunTimer());
    }

    IEnumerator ShowWinWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            foreach (var cell in mineElements)
            {
                cell.ShowWin();
            }
        }
    }

    IEnumerator RunTimer()
    {
        while (gameInProgress)
        {
            yield return new WaitForSeconds(0.2f);
            UpdateTimer();
        }
    }

    private void ShowLost()
    {
        _mineClickedAudio.Play();
        _tickingBombAudio.Play();
        gameInProgress = false;
        StartCoroutine(RevealAllCellsInLostModeWithDelay(2f));
        StopCoroutine(RunTimer());
    }

    public void ReturnToMiainMenu()
    {
        GameNavigator.NavigateToScene(GameScenes.Menu);
    }

    IEnumerator RevealAllCellsInLostModeWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            foreach (var cell in mineElements)
            {
                cell.ShowLost();
            }
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

                go = _container.InstantiatePrefab(mineCell, new Vector3(xpos, ypos, zpos), Quaternion.identity,
                    mineGrid.transform);
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