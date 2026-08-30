using System;
using System.Diagnostics;

// Token: 0x02000CDC RID: 3292
public static class WaterDebug
{
	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x060064E6 RID: 25830 RVA: 0x002774CB File Offset: 0x002756CB
	// (set) Token: 0x060064E7 RID: 25831 RVA: 0x002774D2 File Offset: 0x002756D2
	public static WaterDebugManager Manager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000A8A RID: 2698
	// (get) Token: 0x060064E8 RID: 25832 RVA: 0x002774DA File Offset: 0x002756DA
	public static bool IsAvailable
	{
		get
		{
			return WaterDebug.Manager != null;
		}
	}

	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x060064E9 RID: 25833 RVA: 0x002774E4 File Offset: 0x002756E4
	// (set) Token: 0x060064EA RID: 25834 RVA: 0x002774F6 File Offset: 0x002756F6
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

	// Token: 0x060064EB RID: 25835 RVA: 0x0027750A File Offset: 0x0027570A
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

	// Token: 0x060064EC RID: 25836 RVA: 0x0027752E File Offset: 0x0027572E
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

	// Token: 0x060064ED RID: 25837 RVA: 0x00277540 File Offset: 0x00275740
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

	// Token: 0x060064EE RID: 25838 RVA: 0x00277551 File Offset: 0x00275751
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
