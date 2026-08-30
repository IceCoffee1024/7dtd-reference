using System;
using System.Diagnostics;

// Token: 0x02000CCA RID: 3274
public static class WaterDebug
{
	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x0600645A RID: 25690 RVA: 0x002745CF File Offset: 0x002727CF
	// (set) Token: 0x0600645B RID: 25691 RVA: 0x002745D6 File Offset: 0x002727D6
	public static WaterDebugManager Manager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x0600645C RID: 25692 RVA: 0x002745DE File Offset: 0x002727DE
	public static bool IsAvailable
	{
		get
		{
			return WaterDebug.Manager != null;
		}
	}

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x0600645D RID: 25693 RVA: 0x002745E8 File Offset: 0x002727E8
	// (set) Token: 0x0600645E RID: 25694 RVA: 0x002745FA File Offset: 0x002727FA
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

	// Token: 0x0600645F RID: 25695 RVA: 0x0027460E File Offset: 0x0027280E
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

	// Token: 0x06006460 RID: 25696 RVA: 0x00274632 File Offset: 0x00272832
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

	// Token: 0x06006461 RID: 25697 RVA: 0x00274644 File Offset: 0x00272844
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

	// Token: 0x06006462 RID: 25698 RVA: 0x00274655 File Offset: 0x00272855
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
