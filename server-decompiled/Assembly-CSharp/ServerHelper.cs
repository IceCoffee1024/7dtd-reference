using System;
using UnityEngine;

// Token: 0x020014D2 RID: 5330
public static class ServerHelper
{
	// Token: 0x0600A6AB RID: 42667 RVA: 0x003E0F70 File Offset: 0x003DF170
	public static void SetupForServer(GameObject obj)
	{
		Component[] array = obj.GetComponentsInChildren<Renderer>();
		array = array;
		for (int i = 0; i < array.Length; i++)
		{
			((Renderer)array[i]).enabled = false;
		}
	}
}
