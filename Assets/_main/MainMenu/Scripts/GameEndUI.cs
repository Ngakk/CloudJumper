using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    public Text score, height;

    public void SetTexts(string _score, string _height)
    {
        score.text = _score;
        height.text = _height;
    }
}
