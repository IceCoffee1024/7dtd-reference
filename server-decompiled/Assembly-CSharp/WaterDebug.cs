using System;
using System.Diagnostics;

// Token: 0x02000CEA RID: 3306
public static class WaterDebug
{
	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x060064D8 RID: 25816 RVA: 0x0027A86F File Offset: 0x00278A6F
	// (set) Token: 0x060064D9 RID: 25817 RVA: 0x0027A876 File Offset: 0x00278A76
	public static WaterDebugManager Manager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x060064DA RID: 25818 RVA: 0x0027A87E File Offset: 0x00278A7E
	public static bool IsAvailable
	{
		get
		{
			return WaterDebug.Manager != null;
		}
	}

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x060064DB RID: 25819 RVA: 0x0027A888 File Offset: 0x00278A88
	// (set) Token: 0x060064DC RID: 25820 RVA: 0x0027A89A File Offset: 0x00278A9A
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

	// Token: 0x060064DD RID: 25821 RVA: 0x0027A8AE File Offset: 0x00278AAE
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

	// Token: 0x060064DE RID: 25822 RVA: 0x0027A8D2 File Offset: 0x00278AD2
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

	// Token: 0x060064DF RID: 25823 RVA: 0x0027A8E4 File Offset: 0x00278AE4
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

	// Token: 0x060064E0 RID: 25824 RVA: 0x0027A8F5 File Offset: 0x00278AF5
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
