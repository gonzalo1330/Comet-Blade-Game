using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persist : MonoBehaviour
{
    static Persist ToPersist = null;
    bool TheOne = false;

    int coinsCollected = 0;
    int deaths = 0;

    int coinBest = 0;
    int deathsBest = 999;

    // Start is called before the first frame update
    void Start()
    {
        if (!TheOne)
        {
            if (ToPersist == null)
            {
                ToPersist = this;
                TheOne = true;
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }
    }

    public void IncrementCoins() { coinsCollected++; }
    public void IncrementDeaths() { deaths++; }
    public void Reset() { coinsCollected = 0; deaths = 0; }
    public void Finish()
    {
        if (coinsCollected > coinBest) coinBest = coinsCollected;
        if (deathsBest > deaths) deathsBest = deaths;
    }

    public int GetBestDeaths() { return deathsBest; }
    public int GetBestCoins() { return coinBest; }
    public int GetDeaths() { return deaths; }
    public int GetCoins() { return coinsCollected; }
}
