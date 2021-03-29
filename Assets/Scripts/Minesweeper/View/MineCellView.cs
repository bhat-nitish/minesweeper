using System.Collections;
using System.Collections.Generic;
using Minesweeper.Model;
using MineSweeper.Models;
using UnityEngine;

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

        private SpriteRenderer spriteRenderer;

        void SetProperties(int id, int x, int y, bool isMine, bool isCellPlayed)
        {
            Id = id;
            X = x;
            Y = y;
            IsMine = isMine;
            IsCellPlayed = isCellPlayed;
        }

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (IsMine)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("minelost");
            }
            else
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("hidden");
            }
        }

        public void ShowMine()
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("minelost");
        }

        public void ShowWin()
        {
            if (IsMine)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("minewon");
            }
            else
            {
                switch (MineCount)
                {
                    case 0:
                        spriteRenderer.sprite = Resources.Load<Sprite>("revealedwon");
                        break;
                    case 1:
                        spriteRenderer.sprite = Resources.Load<Sprite>("onewon");
                        break;
                    case 2:
                        spriteRenderer.sprite = Resources.Load<Sprite>("twowon");
                        break;
                    case 3:
                        spriteRenderer.sprite = Resources.Load<Sprite>("threewon");
                        break;
                    case 4:
                        spriteRenderer.sprite = Resources.Load<Sprite>("fourwon");
                        break;
                    case 5:
                        spriteRenderer.sprite = Resources.Load<Sprite>("fivewon");
                        break;
                    case 6:
                        spriteRenderer.sprite = Resources.Load<Sprite>("sixwon");
                        break;
                    case 7:
                        spriteRenderer.sprite = Resources.Load<Sprite>("sevenwon");
                        break;
                    case 8:
                        spriteRenderer.sprite = Resources.Load<Sprite>("eightwon");
                        break;
                }
            }
        }

        public void ShowLost()
        {
            if (IsMine)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("minelost");
            }
            else
            {
                switch (MineCount)
                {
                    case 0:
                        spriteRenderer.sprite = Resources.Load<Sprite>("revealedlost");
                        break;
                    case 1:
                        spriteRenderer.sprite = Resources.Load<Sprite>("onelost");
                        break;
                    case 2:
                        spriteRenderer.sprite = Resources.Load<Sprite>("twolost");
                        break;
                    case 3:
                        spriteRenderer.sprite = Resources.Load<Sprite>("threelost");
                        break;
                    case 4:
                        spriteRenderer.sprite = Resources.Load<Sprite>("fourlost");
                        break;
                    case 5:
                        spriteRenderer.sprite = Resources.Load<Sprite>("fivelost");
                        break;
                    case 6:
                        spriteRenderer.sprite = Resources.Load<Sprite>("sixlost");
                        break;
                    case 7:
                        spriteRenderer.sprite = Resources.Load<Sprite>("sevenlost");
                        break;
                    case 8:
                        spriteRenderer.sprite = Resources.Load<Sprite>("eightlost");
                        break;
                }
            }
        }

        public void ShowMineCount(int count)
        {
            if (IsMine)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("minelost");
            }
            else
            {
                switch (count)
                {
                    case 0:
                        spriteRenderer.sprite = Resources.Load<Sprite>("revealed");
                        break;
                    case 1:
                        spriteRenderer.sprite = Resources.Load<Sprite>("oneplay");
                        break;
                    case 2:
                        spriteRenderer.sprite = Resources.Load<Sprite>("twoplay");
                        break;
                    case 3:
                        spriteRenderer.sprite = Resources.Load<Sprite>("threeplay");
                        break;
                    case 4:
                        spriteRenderer.sprite = Resources.Load<Sprite>("fourplay");
                        break;
                    case 5:
                        spriteRenderer.sprite = Resources.Load<Sprite>("fiveplay");
                        break;
                    case 6:
                        spriteRenderer.sprite = Resources.Load<Sprite>("sixplay");
                        break;
                    case 7:
                        spriteRenderer.sprite = Resources.Load<Sprite>("sevenplay");
                        break;
                    case 8:
                        spriteRenderer.sprite = Resources.Load<Sprite>("eightplay");
                        break;
                }
            }
        }

        void InitialRender()
        {
            if (IsMine)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("mark");
            }
            else
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("minelost");
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}