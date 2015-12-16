// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System;

[Serializable]
public struct MessageData
{
	public GameObject receiver;
	public string message;
	public bool sendString;
	public bool sendInt;
	public bool sendFloat;
	public bool sendBool;
	public string sValue;
	public int iValue;
	public bool bValue;
	public float fValue;
}