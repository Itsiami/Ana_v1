using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int coinsCount;
    public TextMeshProUGUI coinsCountText;

    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène");
        }

        instance = this; // Permet d'accéder à l'instance de n'importe où
    }

    public void AddCoins(int count)
    {
        if ((coinsCount + count) < 1000)
        {
            coinsCount += count;
        } else
        {
            coinsCount = 999;
        }

        if(coinsCount < 10)
        {
            coinsCountText.text = "00" + coinsCount.ToString();
        } else if(coinsCount >= 10 && coinsCount < 100)
        {
            coinsCountText.text = "0" + coinsCount.ToString();
        } else
        {
            coinsCountText.text = coinsCount.ToString();
        }
    }
}
