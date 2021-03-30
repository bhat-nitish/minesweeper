using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Minesweeper.View
{
    public interface IMineSpriteRenderer
    {
        Sprite GetSprite(MineSprite spriteIdentifier);
    }

    public class MineSpriteRenderer : IMineSpriteRenderer
    {
        private Dictionary<MineSprite, Sprite> mineSprites = new Dictionary<MineSprite, Sprite>();

        public MineSpriteRenderer()
        {
            LoadAllSpritesFromSpriteAtlas();
        }

        private void LoadAllSpritesFromSpriteAtlas()
        {
            var mineSpriteAtlas = Resources.Load<SpriteAtlas>("minespriteatlas");
            
            mineSprites.Add(MineSprite.OnePlay, mineSpriteAtlas.GetSprite("oneplay"));
            mineSprites.Add(MineSprite.TwoPlay, mineSpriteAtlas.GetSprite("twoplay"));
            mineSprites.Add(MineSprite.ThreePlay, mineSpriteAtlas.GetSprite("threeplay"));
            mineSprites.Add(MineSprite.FourPlay, mineSpriteAtlas.GetSprite("fourplay"));
            mineSprites.Add(MineSprite.FivePlay, mineSpriteAtlas.GetSprite("fiveplay"));
            mineSprites.Add(MineSprite.SixPlay, mineSpriteAtlas.GetSprite("sixplay"));
            mineSprites.Add(MineSprite.SevenPlay, mineSpriteAtlas.GetSprite("sevenplay"));
            mineSprites.Add(MineSprite.EightPlay, mineSpriteAtlas.GetSprite("rightplay"));

            mineSprites.Add(MineSprite.OneWon, mineSpriteAtlas.GetSprite("onewon"));
            mineSprites.Add(MineSprite.TwoWon, mineSpriteAtlas.GetSprite("twowon"));
            mineSprites.Add(MineSprite.ThreeWon, mineSpriteAtlas.GetSprite("threewon"));
            mineSprites.Add(MineSprite.FourWon, mineSpriteAtlas.GetSprite("fourwon"));
            mineSprites.Add(MineSprite.FiveWon, mineSpriteAtlas.GetSprite("fivewon"));
            mineSprites.Add(MineSprite.SixWon, mineSpriteAtlas.GetSprite("sixwon"));
            mineSprites.Add(MineSprite.SevenWon, mineSpriteAtlas.GetSprite("sevenwon"));
            mineSprites.Add(MineSprite.EightWon, mineSpriteAtlas.GetSprite("eightwon"));

            mineSprites.Add(MineSprite.OneLost, mineSpriteAtlas.GetSprite("onelost"));
            mineSprites.Add(MineSprite.TwoLost, mineSpriteAtlas.GetSprite("twolost"));
            mineSprites.Add(MineSprite.ThreeLost, mineSpriteAtlas.GetSprite("threelost"));
            mineSprites.Add(MineSprite.FourLost, mineSpriteAtlas.GetSprite("fourlost"));
            mineSprites.Add(MineSprite.FiveLost, mineSpriteAtlas.GetSprite("fivelost"));
            mineSprites.Add(MineSprite.SixLost, mineSpriteAtlas.GetSprite("sixlost"));
            mineSprites.Add(MineSprite.SevenLost, mineSpriteAtlas.GetSprite("sevenlost"));
            mineSprites.Add(MineSprite.EightLost, mineSpriteAtlas.GetSprite("eightlost"));

            mineSprites.Add(MineSprite.MineWon, mineSpriteAtlas.GetSprite("minewon"));
            mineSprites.Add(MineSprite.MineLost, mineSpriteAtlas.GetSprite("minelost"));

            mineSprites.Add(MineSprite.Hidden, mineSpriteAtlas.GetSprite("hidden"));
            mineSprites.Add(MineSprite.Revealed, mineSpriteAtlas.GetSprite("revealed"));
            mineSprites.Add(MineSprite.RevealedLost, mineSpriteAtlas.GetSprite("revealedlost"));
            mineSprites.Add(MineSprite.RevealedWon, mineSpriteAtlas.GetSprite("revealedwon"));
        }

        public Sprite GetSprite(MineSprite spriteIdentifier)
        {
            return mineSprites[spriteIdentifier];
        }
    }
}