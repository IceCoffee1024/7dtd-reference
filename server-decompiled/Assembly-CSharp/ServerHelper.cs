using System;
using UnityEngine;

// Token: 0x020014C0 RID: 5312
public static class ServerHelper
{
	// Token: 0x0600A653 RID: 42579 RVA: 0x003DEB6C File Offset: 0x003DCD6C
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
