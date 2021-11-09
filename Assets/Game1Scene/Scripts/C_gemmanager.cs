using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_gemmanager : MonoBehaviour
{
    public GameObject gem_prefab,player,winnerpanel,playagainbutton;
    string minutetext, secondtext;
    GameObject uimanager;
    public Text score,timer;
    static int point;
    AudioSource gemaudio;
    GameObject gem_copy;
    GameObject[,] gem_array;
    public int gemcounteachrow,totalgemtocollect;
    public Vector3 gemstartpos;
    public float gapdistance,totaltimeinseconds;
    float heightatpoint,minutes, seconds,initialgapx;
    public bool dovanish,iswinner;
    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("UI_Manager");
        point = 0;
        totaltimeinseconds = 120;
        gemaudio = this.GetComponent<AudioSource>();
        gem_array = new GameObject[gemcounteachrow,gemcounteachrow];
        m_placegems();
        dovanish = false;
        iswinner = false;
        winnerpanel.SetActive(false);
        initialgapx = gemstartpos.x;
    }

    // Update is called once per frame
    void Update()
    {
        m_collectgems();
        m_showtimer();
        m_checkvanish();
        m_checkwinner();
    }

    void m_placegems()
    {
        for(int i=0;i<gemcounteachrow;i++)
        {
            for(int j=0;j<gemcounteachrow;j++)
            {
                heightatpoint = Terrain.activeTerrain.SampleHeight(gemstartpos);
                gemstartpos.y = heightatpoint + 3;
                gem_copy = Instantiate(gem_prefab);
                gem_array[i,j] = gem_copy;
                gem_copy.transform.position = gemstartpos;
                gemstartpos.x += gapdistance;
                
            }

            gemstartpos.z += gapdistance;
            gemstartpos.x = initialgapx; ;

        }
    }

    void m_collectgems()
    {
        for(int i=0;i<gemcounteachrow;i++)
        {
            for(int j=0;j<gemcounteachrow;j++)
            {
                if (Vector3.Distance(player.transform.position,gem_array[i,j].transform.position)<1)
                {
                    if(gem_array[i,j].activeSelf==true)
                    {
                        point++;
                        score.text = "" + point;
                        gemaudio.Play();
                    }
                    
                    gem_array[i, j].SetActive(false);
                }
            }
        }
        
        
    }

    void m_showtimer()
    {


        if (totaltimeinseconds > 0)
        {
            if(uimanager.GetComponent<C_playerhealth>().healthamountsend>0)
            {
                minutes = Mathf.FloorToInt(totaltimeinseconds / 60);
                seconds = Mathf.FloorToInt(totaltimeinseconds % 60);

                if (minutes < 10)
                    minutetext = "0" + minutes;
                else
                    minutetext = "" + minutes;

                if (seconds < 10)
                    secondtext = "0" + seconds;
                else
                    secondtext = "" + seconds;

                timer.text = minutetext + ":" + secondtext;
                if (totaltimeinseconds <= 30)
                {
                    timer.color = Color.red;
                }
                else
                {
                    timer.color = Color.green;
                }

                totaltimeinseconds -= Time.deltaTime;
            }
            else
            {
                timer.text = minutetext + ":" + secondtext;
                timer.color = Color.red;
            }
            
        }
        else
        {
            totaltimeinseconds = 0;
            uimanager.GetComponent<C_playerhealth>().timeout = totaltimeinseconds;
            uimanager.GetComponent<C_playerhealth>().m_gameover();
            timer.text = minutetext + ":" + secondtext;
            timer.color = Color.red;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void m_checkvanish()
    {
        if(dovanish)
        {
            for (int i = 0; i < gemcounteachrow; i++)
            {
                for (int j = 0; j < gemcounteachrow; j++)
                {
                    gem_array[i, j].SetActive(false);

                }


            }
        }
    }

    void m_checkwinner()
    {
        if(point == totalgemtocollect)
        {
            //Time.timeScale = 0f;
            winnerpanel.SetActive(true);
            playagainbutton.SetActive(true);
            iswinner = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }


}
