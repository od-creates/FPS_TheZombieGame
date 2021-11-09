using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;




public class C_zombie : MonoBehaviour
{

    Animator g_playeranimator;
    public Transform g_target;
    public bool istargetzombie;
    public float attackdist,deadbodytime,attackinterval,healthdepletiondist;
    float elapsedtime,deadtime;
    public AudioClip zombiescream, zombieattack, zombiedead;
    AudioSource zombieaudio;
    public int deadflag;
    GameObject playerhealth,gemmanager;
    float attackstarttime;
    int count;



    // Start is called before the first frame update
    void Awake()
    {
        g_playeranimator = GetComponent<Animator>();
        g_playeranimator.SetBool("run", true);
        zombieaudio = GetComponent<AudioSource>();
        zombieaudio.PlayOneShot(zombiescream,0.6f);
        deadflag = -1;
        playerhealth = GameObject.Find("UI_Manager");
        gemmanager = GameObject.Find("Gem_Manager");
        attackstarttime = Time.time;
        count = 0;
        
        


    }

    // Update is called once per frame
    void Update()
    {
        
        GetComponent<NavMeshAgent>().destination = g_target.position;
        
        m_controlanimation();
        m_vanishdeadbody();
     
        

    }

    void m_controlanimation()
    {
        //print("attackdist" + Vector3.Distance(transform.position, g_target.position));

        if(Vector3.Distance(transform.position,g_target.position)<=attackdist)
        {
            g_playeranimator.SetBool("run", false);
            g_playeranimator.SetBool("attack", true);
            zombieaudio.PlayOneShot(zombieattack,0.6f);

            if (Vector3.Distance(transform.position, g_target.position) <= healthdepletiondist)
            {

                if (Time.time - attackstarttime > attackinterval && istargetzombie==false)//damagehealth() should not be called if target is zombie
                {
                    playerhealth.GetComponent<C_playerhealth>().m_damagehealth();
                    attackstarttime = Time.time;
                }
            }

            

        }
        else
        {
            g_playeranimator.SetBool("attack", false);
            g_playeranimator.SetBool("run", true);
            zombieaudio.PlayOneShot(zombiescream,0.6f);
        }

        
        
        if(playerhealth.GetComponent<C_playerhealth>().healthamountsend<=0 || playerhealth.GetComponent<C_playerhealth>().timeout==0 || gemmanager.GetComponent<C_gemmanager>().iswinner==true)//if gameover vanish zombies
        {
            this.gameObject.SetActive(false);
            return;
        }
       
    }

    public void m_deadanimation()
    {
        zombieaudio.PlayOneShot(zombiedead,1f);
        g_playeranimator.SetTrigger("dead");

        if(count==0)
        {
            deadtime = Time.time;
            count = 1;
        }
        
        deadflag = 0;

    }

    void m_vanishdeadbody()
    {
        elapsedtime = Time.time - deadtime;
        if(elapsedtime>=deadbodytime && deadflag==0)//deadtime!=0 checks if m_deadanimation has been run first
        {
            deadflag = 1;
            count = 0;
        }
    }

    
}
