using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Undo : MonoBehaviour
{
    public static void UndoMove()
    {
        AdManager.Instance.ShowInterstitial();
        if (GameControlManager.Instance.UndoReady && (GameControlManager.Instance.NewMove > 1))
        {
            SoundFX.Instance.SpecialUndoFX();

            /*Utils.adCount++;

            if (5 == Utils.adCount)
            {
                Utils.adCount = 0;
                AdsManager.Instance.ShowAd(null);
            }*/

            GameControlManager.Instance.UndoReady = false;

            //Game.ScoreText.text = "";//(string) Utils.allScoreStack[allScoreStack.Count -1];
            GameControlManager.Instance.MoveCount--;
            GameControlManager.Instance.NewMove--;

            GameControlManager.Instance.LoadPrevStage();
        }
        System.GC.Collect();
    }
}