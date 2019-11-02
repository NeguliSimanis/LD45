using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Slider hungerProgressBar;
    public Slider remainingTimeProgressBar;
    public Slider sanityProgressBar;
    
    public int hungerLevel = 0;
    public float remainingTime = 100;
    public int sanityLevel = 100;

    public void AddToHungerLevel(int amount)
    {
        hungerLevel += amount;
        if (hungerLevel >= 100)
        {
            //gameOver
        }

        hungerProgressBar.value = hungerLevel / 100;
    }

    public void AddToSanityLevel(int amount)
    {
        sanityLevel += amount;

        if (sanityLevel <= 0)
        {
            //gameOver
            return;
        }

        else if (sanityLevel > 100)
        {
            sanityLevel = 100;
        }

        sanityProgressBar.value = sanityLevel / 100;
    }

    public int HungerLevel
    {
        get
        {
           if (hungerLevel < 0)
            {
                return 0;
            }

           else
            {
                return hungerLevel;
            }
        }
    }

  /*  public float RemainingTime
    {
        get
        {
            if(remainingTime < 0 )
            {
                return 0;
            }

            else
            {
                return remainingTime;
            }
        }
    }*/

    public int SanityLevel
    {
        get
        {
            if(sanityLevel < 0)
            {
                return 0;
            }

            else
            {
                return sanityLevel;
            }
        }
    }

    /*private void Update()
    {
        if (GameManager.instance.isGamePaused)
            return;
        remainingTime -= 0.01F;
        remainingTimeProgressBar.value = remainingTime / 100;
    }*/

}
