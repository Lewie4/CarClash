using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.EndRace();

        other.transform.parent.GetComponentInChildren<GameUI>().resultText.text = "YOU WIN!";
    }
}
