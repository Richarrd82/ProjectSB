using UnityEngine;
using System.Collections.Generic;

public class SteakController : MonoBehaviour
{
    private GameObject end;
    private GameObject spawn;
    public GameObject salt;     //htf do i reference a child GO from script

    public float speed = 5f;
    public bool speedUp;
    public bool salted = false;
    public bool scoreAdded = false;
    public bool failCounted = false;

    public List<Bone> pulledBones = new List<Bone>();
    public List<Bone> bones = new List<Bone>();

    void Start()
    {
        end = GameObject.Find("End");

        if(this.gameObject.tag == "FirstSteak")
        {
            spawn = GameObject.Find("FirstSteakSpawn");
            gameObject.SetActive(true);
        }
        else
        {
            spawn = GameObject.Find("Spawn");
        }
    }

    void FixedUpdate()
    {
        Movement();

        if (pulledBones.Count == 3 && salted && !scoreAdded)
        {
            GameManager.instance.Score++;
            scoreAdded = true;
        }

        if (speedUp)
            speed += 0.0005f;

        if (salted)
            salt.SetActive(true);
    }

    void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, end.transform.position, speed * Time.deltaTime);

        if (transform.position.x <= -5.15f && !failCounted)
        {
            if (pulledBones.Count != 3 || !salted)
            {
                failCounted = true;
                GameManager.instance.NumberOfFails++;
            }

            else if (transform.position == end.transform.position)
            {
                if(this.gameObject.tag == "FirstSteak")
                {
                    gameObject.SetActive(false);
                }

                failCounted = false;
                salt.SetActive(false);
                ResetStats();
            }
        }

        else if (transform.position == end.transform.position)
        {
            if(this.gameObject.tag == "FirstSteak")
                gameObject.SetActive(false);
            
            ResetStats();
        }
    }

    public void RegisterBone(Bone bone)
    {
        pulledBones.Add(bone);
    }

    public void ResetStats()
    {
        transform.position = spawn.transform.position;

        //reseting all bones
        foreach(Bone bone in bones)
        {
            bone.gameObject.SetActive(true);
            bone.isInside = true;

            bone.rb.isKinematic = true;
            bone.boneCollider.enabled = true;

            bone.gameObject.transform.position = bone.StartPosition;
            bone.gameObject.transform.rotation = bone.StartRotation;
        }

        //reseting pulled bones only
        foreach (Bone bone in pulledBones)
        {
            bone.gameObject.SetActive(true);
            bone.CallOnce = false;
            bone.isInside = true;

            bone.rb.isKinematic = true;
            bone.boneCollider.enabled = true;

            bone.gameObject.transform.position = bone.StartPosition;
            bone.gameObject.transform.rotation = bone.StartRotation;
        }
        pulledBones.Clear();

        //reseting steak stats
        salted = false;
        failCounted = false;
        scoreAdded = false;
        salt.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Salt")
        {   
            Debug.Log("salted");
            salted = true;
            GameManager.instance.StopSalt();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Salt")
        {
            Debug.Log("salted");
            salted = true;
            GameManager.instance.StopSalt();
        }
    }
}
