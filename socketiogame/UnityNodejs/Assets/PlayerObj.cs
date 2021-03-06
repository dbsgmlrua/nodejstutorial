﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObj : MonoBehaviour
{
    [SerializeField]
    private Text NameSpace;
    public string Name;
    public bool PlayerCharacter = false;
    public void SetName(string t)
    {
        if(NameSpace!=null)
            NameSpace.text = t;
    }
    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
    //currentPlayer
    public float MoveSpeed = 1f;
    public Vector3 DestPos;
    private void Start()
    {
        DestPos = this.transform.position;
        SetName(Name);
    }
    private void Update()
    {
        if (!PlayerCharacter)
            return;
        float dist = Vector3.Distance(DestPos, transform.position);
        //if (!string.IsNullOrEmpty(Name))
        //    SetName(Name);
        if (dist > 0.2f)
        {
            this.transform.position = transform.position + (DestPos - transform.position).normalized * MoveSpeed * Time.deltaTime;
        }
    }
    public void SetDest(Vector3 pos)
    {
        DestPos = pos;
        float dist = Vector3.Distance(DestPos, transform.position);
    }
    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }
}
