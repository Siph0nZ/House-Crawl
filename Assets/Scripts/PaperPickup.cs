using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PaperPickup : MonoBehaviour
{
    public int value = 1; // the value of the paper pickup    
    FadeInOut fade;

    // Start is called before the first frame update
    void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        InterfaceInventory inventory = other.GetComponent<InterfaceInventory>(); // gets the InterfaceInventory script from the player

        if (other.tag == "Player")
        {
            if (inventory != null)
            {
                inventory.Paper = inventory.Paper + value; // adds the value to the player's paper count
                Destroy(gameObject);
            }
            ScoreManager.instance.AddScore(); // calls the AddScore method from the Score script 

            if (ScoreManager.instance.score == 10)
            {
                EndGame(); 
            }
        }
    }

    public void EndGame()
    {
        StartCoroutine(EndScene());
    }

    public IEnumerator EndScene()
    {
        fade.FadeIn();
        Debug.Log("Fading in.");
        yield return new WaitForSeconds(1);
        Debug.Log("Loading End Screen scene.");
        SceneManager.LoadScene("End Screen");
    }
}
