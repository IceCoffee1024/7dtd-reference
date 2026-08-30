using System;
using UnityEngine;

// Token: 0x020014A0 RID: 5280
public static class ServerHelper
{
	// Token: 0x0600A5A5 RID: 42405 RVA: 0x003D9EE0 File Offset: 0x003D80E0
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
