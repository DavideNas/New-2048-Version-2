using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Undo : MonoBehaviour
{
    public void UndoMove()
    {
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
            GameControlManager.Instance.moveCount--;
            GameControlManager.Instance.NewMove--;

            GameControlManager.Instance.LoadPrevStage();
        }
        System.GC.Collect();
    }
}