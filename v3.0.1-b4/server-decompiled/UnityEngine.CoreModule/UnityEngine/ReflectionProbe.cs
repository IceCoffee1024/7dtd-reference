using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000114 RID: 276
	[NativeHeader("Runtime/Camera/ReflectionProbes.h")]
	public sealed class ReflectionProbe : Behaviour
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000677 RID: 1655
		// (set) Token: 0x06000678 RID: 1656
		[NativeName("ProbeType")]
		[Obsolete("type property has been deprecated. Starting with Unity 5.4, the only supported reflection probe type is Cube.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public extern ReflectionProbeType type
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x000092BC File Offset: 0x000074BC
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x000092D2 File Offset: 0x000074D2
		[NativeName("BoxSize")]
		public Vector3 size
		{
			get
			{
				Vector3 vector;
				this.get_size_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x000092DC File Offset: 0x000074DC
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x000092F2 File Offset: 0x000074F2
		[NativeName("BoxOffset")]
		public Vector3 center
		{
			get
			{
				Vector3 vector;
				this.get_center_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_center_Injected(ref value);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600067D RID: 1661
		// (set) Token: 0x0600067E RID: 1662
		[NativeName("Near")]
		public extern float nearClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600067F RID: 1663
		// (set) Token: 0x06000680 RID: 1664
		[NativeName("Far")]
		public extern float farClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000681 RID: 1665
		// (set) Token: 0x06000682 RID: 1666
		[NativeName("IntensityMultiplier")]
		public extern float intensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x000092FC File Offset: 0x000074FC
		[NativeName("GlobalAABB")]
		public Bounds bounds
		{
			get
			{
				Bounds bounds;
				this.get_bounds_Injected(out bounds);
				return bounds;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000684 RID: 1668
		// (set) Token: 0x06000685 RID: 1669
		[NativeName("HDR")]
		public extern bool hdr
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000686 RID: 1670
		// (set) Token: 0x06000687 RID: 1671
		[NativeName("RenderDynamicObjects")]
		public extern bool renderDynamicObjects
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000688 RID: 1672
		// (set) Token: 0x06000689 RID: 1673
		public extern float shadowDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600068A RID: 1674
		// (set) Token: 0x0600068B RID: 1675
		public extern int resolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600068C RID: 1676
		// (set) Token: 0x0600068D RID: 1677
		public extern int cullingMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600068E RID: 1678
		// (set) Token: 0x0600068F RID: 1679
		public extern ReflectionProbeClearFlags clearFlags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00009314 File Offset: 0x00007514
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x0000932A File Offset: 0x0000752A
		public Color backgroundColor
		{
			get
			{
				Color color;
				this.get_backgroundColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_backgroundColor_Injected(ref value);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000692 RID: 1682
		// (set) Token: 0x06000693 RID: 1683
		public extern float blendDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000694 RID: 1684
		// (set) Token: 0x06000695 RID: 1685
		public extern bool boxProjection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000696 RID: 1686
		// (set) Token: 0x06000697 RID: 1687
		public extern ReflectionProbeMode mode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000698 RID: 1688
		// (set) Token: 0x06000699 RID: 1689
		public extern int importance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600069A RID: 1690
		// (set) Token: 0x0600069B RID: 1691
		public extern ReflectionProbeRefreshMode refreshMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600069C RID: 1692
		// (set) Token: 0x0600069D RID: 1693
		public extern ReflectionProbeTimeSlicingMode timeSlicingMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600069E RID: 1694
		// (set) Token: 0x0600069F RID: 1695
		public extern Texture bakedTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060006A0 RID: 1696
		// (set) Token: 0x060006A1 RID: 1697
		public extern Texture customBakedTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060006A2 RID: 1698
		// (set) Token: 0x060006A3 RID: 1699
		public extern RenderTexture realtimeTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060006A4 RID: 1700
		public extern Texture texture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00009334 File Offset: 0x00007534
		public Vector4 textureHDRDecodeValues
		{
			[NativeName("CalculateHDRDecodeValues")]
			get
			{
				Vector4 vector;
				this.get_textureHDRDecodeValues_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x060006A6 RID: 1702
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reset();

		// Token: 0x060006A7 RID: 1703 RVA: 0x0000934C File Offset: 0x0000754C
		public int RenderProbe()
		{
			return this.RenderProbe(null);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00009368 File Offset: 0x00007568
		public int RenderProbe([DefaultValue("null")] RenderTexture targetTexture)
		{
			return this.ScheduleRender(this.timeSlicingMode, targetTexture);
		}

		// Token: 0x060006A9 RID: 1705
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsFinishedRendering(int renderId);

		// Token: 0x060006AA RID: 1706
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int ScheduleRender(ReflectionProbeTimeSlicingMode timeSlicingMode, RenderTexture targetTexture);

		// Token: 0x060006AB RID: 1707
		[FreeFunction("CubemapGPUBlend")]
		[NativeHeader("Runtime/Camera/CubemapGPUUtility.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool BlendCubemap(Texture src, Texture dst, float blend, RenderTexture target);

		// Token: 0x060006AC RID: 1708
		[StaticAccessor("GetReflectionProbes()")]
		[NativeMethod("UpdateSampleData")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UpdateCachedState();

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060006AD RID: 1709
		[StaticAccessor("GetReflectionProbes()")]
		public static extern int minBakedCubemapResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060006AE RID: 1710
		[StaticAccessor("GetReflectionProbes()")]
		public static extern int maxBakedCubemapResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00009388 File Offset: 0x00007588
		[StaticAccessor("GetReflectionProbes()")]
		public static Vector4 defaultTextureHDRDecodeValues
		{
			get
			{
				Vector4 vector;
				ReflectionProbe.get_defaultTextureHDRDecodeValues_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060006B0 RID: 1712
		[StaticAccessor("GetReflectionProbes()")]
		public static extern Texture defaultTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060006B1 RID: 1713 RVA: 0x000093A0 File Offset: 0x000075A0
		// (remove) Token: 0x060006B2 RID: 1714 RVA: 0x000093D4 File Offset: 0x000075D4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ReflectionProbe, ReflectionProbe.ReflectionProbeEvent> reflectionProbeChanged;

		// Token: 0x060006B3 RID: 1715 RVA: 0x00009408 File Offset: 0x00007608
		[RequiredByNativeCode]
		private static void CallReflectionProbeEvent(ReflectionProbe probe, ReflectionProbe.ReflectionProbeEvent probeEvent)
		{
			Action<ReflectionProbe, ReflectionProbe.ReflectionProbeEvent> action = ReflectionProbe.reflectionProbeChanged;
			bool flag = action != null;
			if (flag)
			{
				action(probe, probeEvent);
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060006B4 RID: 1716 RVA: 0x00009430 File Offset: 0x00007630
		// (remove) Token: 0x060006B5 RID: 1717 RVA: 0x00009498 File Offset: 0x00007698
		[Obsolete("ReflectionProbe.defaultReflectionSet has been deprecated. Use ReflectionProbe.defaultReflectionTexture. (UnityUpgradable) -> UnityEngine.ReflectionProbe.defaultReflectionTexture", false)]
		public static event Action<Cubemap> defaultReflectionSet
		{
			add
			{
				bool flag = ReflectionProbe.registeredDefaultReflectionTextureActions.Any<Action<Texture>>((Action<Texture> h) => h.Method == value.Method);
				if (!flag)
				{
					Action<Texture> action = delegate(Texture b)
					{
						Cubemap cubemap = b as Cubemap;
						bool flag2 = cubemap != null;
						if (flag2)
						{
							value(cubemap);
						}
					};
					ReflectionProbe.defaultReflectionTexture += action;
					ReflectionProbe.registeredDefaultReflectionSetActions[value.Method.GetHashCode()] = action;
				}
			}
			remove
			{
				Action<Texture> action;
				bool flag = ReflectionProbe.registeredDefaultReflectionSetActions.TryGetValue(value.Method.GetHashCode(), out action);
				if (flag)
				{
					ReflectionProbe.defaultReflectionTexture -= action;
					ReflectionProbe.registeredDefaultReflectionSetActions.Remove(value.Method.GetHashCode());
				}
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060006B6 RID: 1718 RVA: 0x000094E0 File Offset: 0x000076E0
		// (remove) Token: 0x060006B7 RID: 1719 RVA: 0x00009548 File Offset: 0x00007748
		public static event Action<Texture> defaultReflectionTexture
		{
			add
			{
				bool flag = ReflectionProbe.registeredDefaultReflectionTextureActions.Any<Action<Texture>>((Action<Texture> h) => h.Method == value.Method) || ReflectionProbe.registeredDefaultReflectionSetActions.ContainsKey(value.Method.GetHashCode());
				if (!flag)
				{
					ReflectionProbe.registeredDefaultReflectionTextureActions.Add(value);
				}
			}
			remove
			{
				ReflectionProbe.registeredDefaultReflectionTextureActions.Remove(value);
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00009558 File Offset: 0x00007758
		[RequiredByNativeCode]
		private static void CallSetDefaultReflection(Texture defaultReflectionCubemap)
		{
			foreach (Action<Texture> action in ReflectionProbe.registeredDefaultReflectionTextureActions)
			{
				action(defaultReflectionCubemap);
			}
		}

		// Token: 0x060006BB RID: 1723
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector3 ret);

		// Token: 0x060006BC RID: 1724
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector3 value);

		// Token: 0x060006BD RID: 1725
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_center_Injected(out Vector3 ret);

		// Token: 0x060006BE RID: 1726
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_center_Injected(ref Vector3 value);

		// Token: 0x060006BF RID: 1727
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		// Token: 0x060006C0 RID: 1728
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_backgroundColor_Injected(out Color ret);

		// Token: 0x060006C1 RID: 1729
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_backgroundColor_Injected(ref Color value);

		// Token: 0x060006C2 RID: 1730
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_textureHDRDecodeValues_Injected(out Vector4 ret);

		// Token: 0x060006C3 RID: 1731
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_defaultTextureHDRDecodeValues_Injected(out Vector4 ret);

		// Token: 0x04000395 RID: 917
		private static Dictionary<int, Action<Texture>> registeredDefaultReflectionSetActions = new Dictionary<int, Action<Texture>>();

		// Token: 0x04000396 RID: 918
		private static List<Action<Texture>> registeredDefaultReflectionTextureActions = new List<Action<Texture>>();

		// Token: 0x02000115 RID: 277
		public enum ReflectionProbeEvent
		{
			// Token: 0x04000398 RID: 920
			ReflectionProbeAdded,
			// Token: 0x04000399 RID: 921
			ReflectionProbeRemoved
		}
	}
}
