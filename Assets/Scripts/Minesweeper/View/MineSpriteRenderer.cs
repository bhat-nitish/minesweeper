using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper.View
{
    public interface IMineSpriteRenderer
    {
        Sprite GetSprite(MineSprite spriteIdentifier);
    }

    public class MineSpriteRenderer : IMineSpriteRenderer
    {
        private SpriteRenderer spriteRenderer;

        private Dictionary<MineSprite, Sprite> mineSprites = new Dictionary<MineSprite, Sprite>();

        public MineSpriteRenderer()
        {
            LoadAllSprites();
        }

        private void LoadAllSprites()
        {
            mineSprites.Add(MineSprite.OnePlay, Resources.Load<Sprite>("oneplay"));
            mineSprites.Add(MineSprite.TwoPlay, Resources.Load<Sprite>("twoplay"));
            mineSprites.Add(MineSprite.ThreePlay, Resources.Load<Sprite>("threeplay"));
            mineSprites.Add(MineSprite.FourPlay, Resources.Load<Sprite>("fourplay"));
            mineSprites.Add(MineSprite.FivePlay, Resources.Load<Sprite>("fiveplay"));
            mineSprites.Add(MineSprite.SixPlay, Resources.Load<Sprite>("sixplay"));
            mineSprites.Add(MineSprite.SevenPlay, Resources.Load<Sprite>("sevenplay"));
            mineSprites.Add(MineSprite.EightPlay, Resources.Load<Sprite>("rightplay"));

            mineSprites.Add(MineSprite.OneWon, Resources.Load<Sprite>("onewon"));
            mineSprites.Add(MineSprite.TwoWon, Resources.Load<Sprite>("twowon"));
            mineSprites.Add(MineSprite.ThreeWon, Resources.Load<Sprite>("threewon"));
            mineSprites.Add(MineSprite.FourWon, Resources.Load<Sprite>("fourwon"));
            mineSprites.Add(MineSprite.FiveWon, Resources.Load<Sprite>("fivewon"));
            mineSprites.Add(MineSprite.SixWon, Resources.Load<Sprite>("sixwon"));
            mineSprites.Add(MineSprite.SevenWon, Resources.Load<Sprite>("sevenwon"));
            mineSprites.Add(MineSprite.EightWon, Resources.Load<Sprite>("eightwon"));

            mineSprites.Add(MineSprite.OneLost, Resources.Load<Sprite>("onelost"));
            mineSprites.Add(MineSprite.TwoLost, Resources.Load<Sprite>("twolost"));
            mineSprites.Add(MineSprite.ThreeLost, Resources.Load<Sprite>("threelost"));
            mineSprites.Add(MineSprite.FourLost, Resources.Load<Sprite>("fourlost"));
            mineSprites.Add(MineSprite.FiveLost, Resources.Load<Sprite>("fivelost"));
            mineSprites.Add(MineSprite.SixLost, Resources.Load<Sprite>("sixlost"));
            mineSprites.Add(MineSprite.SevenLost, Resources.Load<Sprite>("sevenlost"));
            mineSprites.Add(MineSprite.EightLost, Resources.Load<Sprite>("eightlost"));

            mineSprites.Add(MineSprite.MineWon, Resources.Load<Sprite>("minewon"));
            mineSprites.Add(MineSprite.MineLost, Resources.Load<Sprite>("minelost"));

            mineSprites.Add(MineSprite.Hidden, Resources.Load<Sprite>("hidden"));
            mineSprites.Add(MineSprite.Revealed, Resources.Load<Sprite>("revealed"));
            mineSprites.Add(MineSprite.RevealedLost, Resources.Load<Sprite>("revealedlost"));
            mineSprites.Add(MineSprite.RevealedWon, Resources.Load<Sprite>("revealedwon"));
        }

        public Sprite GetSprite(MineSprite spriteIdentifier)
        {
            return mineSprites[spriteIdentifier];
        }
    }
}