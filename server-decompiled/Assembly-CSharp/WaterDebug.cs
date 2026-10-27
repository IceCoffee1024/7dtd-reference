using System;
using System.Diagnostics;

// Token: 0x02000CF5 RID: 3317
public static class WaterDebug
{
	// Token: 0x17000AA5 RID: 2725
	// (get) Token: 0x060064FD RID: 25853 RVA: 0x0027BD97 File Offset: 0x00279F97
	// (set) Token: 0x060064FE RID: 25854 RVA: 0x0027BD9E File Offset: 0x00279F9E
	public static WaterDebugManager Manager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000AA6 RID: 2726
	// (get) Token: 0x060064FF RID: 25855 RVA: 0x0027BDA6 File Offset: 0x00279FA6
	public static bool IsAvailable
	{
		get
		{
			return WaterDebug.Manager != null;
		}
	}

	// Token: 0x17000AA7 RID: 2727
	// (get) Token: 0x06006500 RID: 25856 RVA: 0x0027BDB0 File Offset: 0x00279FB0
	// (set) Token: 0x06006501 RID: 25857 RVA: 0x0027BDC2 File Offset: 0x00279FC2
	public static bool RenderingEnabled
	{
		get
		{
			WaterDebugManager manager = WaterDebug.Manager;
			return manager != null && manager.RenderingEnabled;
		}
		set
		{
			if (WaterDebug.Manager != null)
			{
				WaterDebug.Manager.RenderingEnabled = value;
			}
		}
	}

	// Token: 0x06006502 RID: 25858 RVA: 0x0027BDD6 File Offset: 0x00279FD6
	[Conditional("UNITY_EDITOR")]
	public static void Init()
	{
		if (!WaterSimulationNative.Instance.ShouldEnable)
		{
			return;
		}
		WaterDebugPools.CreatePools();
		WaterDebug.Manager = new WaterDebugManager();
		WaterDebug.RenderingEnabled = false;
	}

	// Token: 0x06006503 RID: 25859 RVA: 0x0027BDFA File Offset: 0x00279FFA
	[Conditional("UNITY_EDITOR")]
	public static void InitializeForChunk(Chunk _chunk)
	{
		WaterDebugManager manager = WaterDebug.Manager;
		if (manager == null)
		{
			return;
		}
		manager.InitializeDebugRender(_chunk);
	}

	// Token: 0x06006504 RID: 25860 RVA: 0x0027BE0C File Offset: 0x0027A00C
	[Conditional("UNITY_EDITOR")]
	public static void Draw()
	{
		WaterDebugManager manager = WaterDebug.Manager;
		if (manager == null)
		{
			return;
		}
		manager.DebugDraw();
	}

	// Token: 0x06006505 RID: 25861 RVA: 0x0027BE1D File Offset: 0x0027A01D
	[Conditional("UNITY_EDITOR")]
	public static void Cleanup()
	{
		WaterDebugManager manager = WaterDebug.Manager;
		if (manager != null)
		{
			manager.Cleanup();
		}
		WaterDebugPools.Cleanup();
		WaterDebug.Manager = null;
	}
}
