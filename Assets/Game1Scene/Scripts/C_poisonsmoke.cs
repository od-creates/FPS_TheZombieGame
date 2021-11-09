using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_poisonsmoke : MonoBehaviour
{
    public GameObject poisonsmoke, player;
    GameObject[,] smoke_array;
    GameObject smoke_copy;
    public int smokecount;
    public float smokegapdist, smokeaffectinterval;
    float heightatpoint, smokeaffectstart,initialgapx;
    public Vector3 smokestartpos;
    GameObject uimanager;
    public bool dovanish;

    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("UI_Manager");
        smoke_array = new GameObject[smokecount, smokecount];
        m_placesmoke();
        smokeaffectstart = Time.time;
        dovanish = false;
        initialgapx = smokestartpos.x;
    }

    // Update is called once per frame
    void Update()
    {
        m_smokepasscheck();
        m_checkvanish();
    }

    void m_placesmoke()
    {
        for (int i = 0; i < smokecount; i++)
        {
            for (int j = 0; j < smokecount; j++)
            {
                heightatpoint = Terrain.activeTerrain.SampleHeight(smokestartpos);
                smokestartpos.y = heightatpoint + 1;
                smoke_copy = Instantiate(poisonsmoke);
                smoke_array[i, j] = smoke_copy;
                smoke_copy.transform.position = smokestartpos;
                smokestartpos.x += smokegapdist;

            }

            smokestartpos.z += smokegapdist;
            smokestartpos.x = initialgapx;

        }
    }

    void m_smokepasscheck()
    {
        for (int i = 0; i < smokecount; i++)
        {
            for (int j = 0; j < smokecount; j++)
            {
                if (Vector3.Distance(smoke_array[i, j].transform.position, player.transform.position) <= 8)
                {
                    if (Time.time - smokeaffectstart > smokeaffectinterval)
                    {
                        uimanager.GetComponent<C_playerhealth>().m_damagehealth();
                        smokeaffectstart = Time.time;
                    }

                }

            }

        }
    }

    void m_checkvanish()
    {
        if (dovanish)
        {
            for (int i = 0; i < smokecount; i++)
            {
                for (int j = 0; j < smokecount; j++)
                {
                    smoke_array[i, j].SetActive(false);

                }


            }
        }
    }
}
