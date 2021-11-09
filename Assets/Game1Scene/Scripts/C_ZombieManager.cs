using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class C_ZombieManager : MonoBehaviour
{
    public GameObject g_zombies, g_player;
    GameObject[] zombie_copy_array;
    GameObject zombie_copy;
    public int poolsize;
    public GameObject[] g_spawnpts;
    public float spawninterval;
    float starttime, elapsedtime;
    // Start is called before the first frame update
    void Start()
    {
        starttime = Time.time;
        m_zombiepool();
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnzombies();
    }


    void m_spawnzombies()
    {
        
        for (int i = 0; i < poolsize; i++)
        {
            //GameObject zombie_copy = zombie_copy_array[i];
            elapsedtime = Time.time - starttime;
            //elapsedtime >= spawninterval &&
            if (elapsedtime >= spawninterval && !(zombie_copy_array[i].activeSelf) )
            {
                //print("ifcalled");
                zombie_copy_array[i].SetActive(true);
                //print(zombie_copy_array[i].activeSelf + "Status1");
                zombie_copy_array[i].GetComponent<C_zombie>().deadflag = -1;
                zombie_copy_array[i].transform.position = g_spawnpts[Random.Range(0, 24)].transform.position;
                //print(zombie_copy_array[i].activeSelf + "Status2");
                zombie_copy_array[i].GetComponent<NavMeshAgent>().enabled = true;
                //print(zombie_copy_array[i].activeSelf + "Status3");
                zombie_copy_array[i].GetComponent<C_zombie>().g_target = g_player.transform;
                //print(zombie_copy_array[i].activeSelf + "Status4");
                zombie_copy_array[i].GetComponent<C_zombie>().istargetzombie = false;


                starttime = Time.time;
            }
            
            //print("object" + zombie_copy);

            if(zombie_copy_array[i].GetComponent<C_zombie>().deadflag == 0)
            {
                zombie_copy_array[i].GetComponent<C_zombie>().g_target = zombie_copy_array[i].transform;//on dead should not follow target(player)
                zombie_copy_array[i].GetComponent<C_zombie>().istargetzombie = true;
            }

            if(zombie_copy_array[i].GetComponent<C_zombie>().deadflag==1)
            {
                zombie_copy_array[i].SetActive(false);
            }

        }
        
    }

    void m_zombiepool()
    {
        zombie_copy_array = new GameObject[poolsize];
        for(int i=0;i<poolsize;i++)
        {
            zombie_copy= Instantiate(g_zombies);
            zombie_copy.SetActive(false);
            zombie_copy_array[i] = zombie_copy;
        }
    }
}
