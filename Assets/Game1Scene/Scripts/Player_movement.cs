using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Player_movement : MonoBehaviour
{

    public float movespeed,rotatespeed,jumpforce,gravity;
    public GameObject playercam;
    public GameObject muzzleflash;
    ParticleSystem hitcopy;
    public GameObject hitanimation,hitanimation2;
    Ray raystart;
    RaycastHit rayhitinfo;
    AudioSource playeraudio;
    public AudioClip gunfire,footstep;
    float steptime, stepstart,firestart,firetime,clampx,clampz;
    public float stepintervaltime,stepdistance,fireintervaltime;
    GameObject playerhealth,gemmanager,uimanager;
   



    CharacterController char_cont;
    Vector3 movedirection,rotatedirection,rotateeulerangle;


    // Start is called before the first frame update
    void Awake()
    {
        char_cont = this.GetComponent<CharacterController>();
        muzzleflash.transform.Find("Light").gameObject.active = false;
        playeraudio = GetComponent<AudioSource>();
        stepstart = Time.time;
        firestart = Time.time;
        playerhealth = GameObject.Find("UI_Manager");
        gemmanager = GameObject.Find("Gem_Manager");
        Cursor.lockState = CursorLockMode.Locked;
        uimanager = GameObject.Find("UI_Canvas");
        uimanager.transform.GetChild(7).gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //m_playerjump();
        m_applygrav();
        m_playermove();
        m_playerrotate();
        m_muzzleflash();
        m_fire();
        m_playfootsteps();
        m_gameovercheck();
        steptime = Time.time - stepstart;
        firetime= Time.time - firestart;




    }

    void m_applygrav()
    {
        if (!char_cont.isGrounded)
        {
            movedirection.y += gravity * Time.deltaTime;
        }
   
    }

    void m_playermove()
    {
        movedirection.x = Input.GetAxis("Horizontal");
        movedirection.z = Input.GetAxis("Vertical");

        movedirection = transform.TransformDirection(movedirection);//wrt world axis

       
        char_cont.Move(movedirection * movespeed * Time.deltaTime);

        clampx = Mathf.Clamp(transform.position.x, 0, 1000);//clamp x
        clampz = Mathf.Clamp(transform.position.z, 0, 1000);//clamp z

        transform.position = new Vector3(clampx, transform.position.y, clampz);//apply clamp


    }

    void m_playerjump()
    {
        //print("char cont is grounded" + char_cont.isGrounded);
        
        if(char_cont.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            //print("jump called");
            movedirection.y =0;//since wrt local axis
            movedirection.y += jumpforce;
           // print("movedirection.y" + movedirection.y);
    
        }
    }


    void m_playerrotate()
    {
        rotatedirection.x = -Input.GetAxis("Mouse Y");
        

        playercam.transform.Rotate(rotatedirection * rotatespeed * Time.deltaTime);

        rotateeulerangle.x = playercam.transform.localEulerAngles.x;

        if(rotateeulerangle.x>200 && rotateeulerangle.x<300)
        {
            rotateeulerangle.x = 300;
        }

        if(rotateeulerangle.x>60 && rotateeulerangle.x<100)
        {
            rotateeulerangle.x = 60;
        }

        playercam.transform.localEulerAngles = rotateeulerangle;

        rotatedirection = Vector3.zero;//doubt

        rotatedirection.y = Input.GetAxis("Mouse X");
        this.transform.Rotate(rotatedirection * rotatespeed * Time.deltaTime, Space.Self);

    }


    //bool ismuzzleactive = false;

    void m_muzzleflash()
    {
        

        if(Input.GetMouseButtonDown(0) )
        {
            muzzleflash.GetComponent<ParticleSystem>().Play();
            muzzleflash.transform.Find("Light").gameObject.active = true;
            
            

            //ismuzzleactive = true;
        }

        if (Input.GetMouseButtonUp(0) )
        {

            muzzleflash.GetComponent<ParticleSystem>().Stop();
            muzzleflash.transform.Find("Light").gameObject.active = false;
           
            //ismuzzleactive = false;
        }

    }

    void m_fire()
    {
        if(Input.GetMouseButton(0))
        {
            if (firetime > fireintervaltime)
            {

                playeraudio.PlayOneShot(gunfire);
                firestart = Time.time;
            }
            
            


            raystart = playercam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(raystart, out rayhitinfo))
            {
               
                if(rayhitinfo.transform.tag=="zombie")
                {
                    Instantiate(hitanimation2, rayhitinfo.point, Quaternion.LookRotation(rayhitinfo.normal));
                    rayhitinfo.transform.GetComponent<C_zombie>().m_deadanimation();
                    
                    
                }
                else    
                {
                    Instantiate(hitanimation, rayhitinfo.point, Quaternion.LookRotation(rayhitinfo.normal));
                }

            }
            
        }
    }

    void m_playfootsteps()
    {
        if(char_cont.velocity.sqrMagnitude>stepdistance && steptime>stepintervaltime)
        {
            
            playeraudio.PlayOneShot(footstep);
            stepstart = Time.time;
        }
        
    }

    void m_gameovercheck()
    {
        if (playerhealth.GetComponent<C_playerhealth>().healthamountsend <= 0 || playerhealth.GetComponent<C_playerhealth>().timeout==0|| gemmanager.GetComponent<C_gemmanager>().iswinner==true)//if gameover vanish AKM
        {
            this.transform.GetChild(0).Find("AKM").gameObject.SetActive(false);
            this.GetComponent<Player_movement>().enabled = false;
            uimanager.transform.GetChild(7).gameObject.SetActive(true);
            return;
        }
    }

}
