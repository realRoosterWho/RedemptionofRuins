using UnityEngine;
using UnityEngine.UI;

public class GasBarUI : MonoBehaviour
{
    public Image gasBar;
    

    public void UpdateGasBar(float currentGas, float maxGas)
    {
        gasBar.fillAmount = currentGas / maxGas;
    }
}