using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_playerhealth : MonoBehaviour
{
    public GameObject healthbackground, gameover,reticle,terrainscene,playagainbutton,gemmanager,poisonsmoke;
    Image healthstat;
    static float healthamount;
    public float damageamount, healthamountsend,timeout;
    GameObject zombie;
    int counter;
    

    // Start is called before the first frame update
    void Start()
    {
        //healthamount = 1f;
        healthstat = healthbackground.transform.Find("healthdetector").GetComponent<Image>();
        healthamount = 1f;
        healthstat.fillAmount = healthamount;
        healthstat.color = Color.green;
        gameover.SetActive(false);
        playagainbutton.SetActive(false);
        healthamountsend = healthamount;
        counter = 0;
        timeout = -1;
        
        


    }

    // Update is called once per frame
    void Update()
    {
        //print("health" + healthamount);
        m_gameover();
    }

    public void m_damagehealth()
    {
        healthamount -= damageamount;
        //print("entered damagehealth"+healthamount);
        healthstat.fillAmount = healthamount;
        if(healthamount <0.3f)
        {
            healthstat.color = Color.red;
        }

        
    }

    public void m_gameover()
    {
        if (healthamount <= 0 || timeout==0)
        {
            //print("you die");
            //this.GetComponent<AudioSource>().PlayOneShot(playeraudio);
            gameover.SetActive(true);
            if(healthamount <= 0)
                gameover.transform.GetChild(2).gameObject.SetActive(false);
            if(timeout == 0)
                gameover.transform.GetChild(1).gameObject.SetActive(false);
            playagainbutton.SetActive(true);
            reticle.SetActive(false);
            terrainscene.GetComponent<AudioSource>().enabled = false;
            healthamountsend = healthamount;
            gemmanager.GetComponent<C_gemmanager>().dovanish = true;
            poisonsmoke.GetComponent<C_poisonsmoke>().dovanish = true;


            if (counter==0 && timeout!=0)//to play player deadsound once on dying
            {
                this.GetComponent<AudioSource>().Play();
                counter = 1;
                Cursor.lockState = CursorLockMode.None;
            }
            
        }

        
    }

}
