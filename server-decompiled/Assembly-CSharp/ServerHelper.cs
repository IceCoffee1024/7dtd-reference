using System;
using UnityEngine;

// Token: 0x02001485 RID: 5253
public static class ServerHelper
{
	// Token: 0x0600A4A8 RID: 42152 RVA: 0x003D4B9C File Offset: 0x003D2D9C
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
