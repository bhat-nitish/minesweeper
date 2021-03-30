using System.Collections;
using System.Collections.Generic;
using MineSweeper.Game.Minesweeper;
using Minesweeper.Model;
using MineSweeper.Models;
using Minesweeper.View;
using UnityEngine;
using Zenject;

namespace MineSweeper.View
{
    public class MineCellView : MonoBehaviour, IMineCell
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsMine { get; set; }

        public bool IsCellPlayed { get; set; }

        public int MineCount { get; set; }

        private SpriteRenderer spriteRenderer = new SpriteRenderer();

        private IMineSpriteRenderer _mineSpriteRenderer;

        #region Dependency Injection

        [Inject]
        private void Init(IMineSpriteRenderer mineSpriteRenderer)
        {
            _mineSpriteRenderer = mineSpriteRenderer;
        }

        #endregion

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            var sprite = _mineSpriteRenderer.GetSprite(MineSprite.Hidden);
            if (sprite == null || spriteRenderer == null) return;
            spriteRenderer.sprite = sprite;
        }

        public void ShowWin()
        {
            if (IsMine)
            {
                spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.MineWon);
            }
            else
            {
                switch (MineCount)
                {
                    case 0:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.RevealedWon);
                        break;
                    case 1:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.OneWon);
                        break;
                    case 2:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.TwoWon);
                        break;
                    case 3:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.ThreeWon);
                        break;
                    case 4:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.FourWon);
                        break;
                    case 5:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.FiveWon);
                        break;
                    case 6:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.SixWon);
                        break;
                    case 7:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.SevenWon);
                        break;
                    case 8:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.EightWon);
                        break;
                }
            }
        }

        public void ShowLost()
        {
            if (IsMine)
            {
                spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.MineLost);
            }
            else
            {
                switch (MineCount)
                {
                    case 0:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.RevealedLost);
                        break;
                    case 1:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.OneLost);
                        break;
                    case 2:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.TwoLost);
                        break;
                    case 3:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.ThreeLost);
                        break;
                    case 4:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.FourLost);
                        break;
                    case 5:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.FiveLost);
                        break;
                    case 6:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.SixLost);
                        break;
                    case 7:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.SevenLost);
                        break;
                    case 8:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.EightLost);
                        break;
                }
            }
        }

        public void ShowMineCount(int count)
        {
            if (IsMine)
            {
                spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.MineLost);
            }
            else
            {
                switch (MineCount)
                {
                    case 0:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.Revealed);
                        break;
                    case 1:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.OnePlay);
                        break;
                    case 2:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.TwoPlay);
                        break;
                    case 3:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.ThreePlay);
                        break;
                    case 4:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.FourPlay);
                        break;
                    case 5:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.FivePlay);
                        break;
                    case 6:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.SixPlay);
                        break;
                    case 7:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.SevenPlay);
                        break;
                    case 8:
                        spriteRenderer.sprite = _mineSpriteRenderer.GetSprite(MineSprite.EightPlay);
                        break;
                }
            }
        }
    }
}