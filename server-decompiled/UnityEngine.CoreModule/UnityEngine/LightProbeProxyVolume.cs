using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001AD RID: 429
	[NativeHeader("Runtime/Camera/LightProbeProxyVolume.h")]
	public sealed class LightProbeProxyVolume : Behaviour
	{
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000FEE RID: 4078
		public static extern bool isFeatureSupported
		{
			[NativeName("IsFeatureSupported")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00015D0C File Offset: 0x00013F0C
		[NativeName("GlobalAABB")]
		public Bounds boundsGlobal
		{
			get
			{
				Bounds bounds;
				this.get_boundsGlobal_Injected(out bounds);
				return bounds;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x00015D24 File Offset: 0x00013F24
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x00015D3A File Offset: 0x00013F3A
		[NativeName("BoundingBoxSizeCustom")]
		public Vector3 sizeCustom
		{
			get
			{
				Vector3 vector;
				this.get_sizeCustom_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_sizeCustom_Injected(ref value);
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00015D44 File Offset: 0x00013F44
		// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x00015D5A File Offset: 0x00013F5A
		[NativeName("BoundingBoxOriginCustom")]
		public Vector3 originCustom
		{
			get
			{
				Vector3 vector;
				this.get_originCustom_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_originCustom_Injected(ref value);
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000FF4 RID: 4084
		// (set) Token: 0x06000FF5 RID: 4085
		public extern float probeDensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000FF6 RID: 4086
		// (set) Token: 0x06000FF7 RID: 4087
		public extern int gridResolutionX
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000FF8 RID: 4088
		// (set) Token: 0x06000FF9 RID: 4089
		public extern int gridResolutionY
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000FFA RID: 4090
		// (set) Token: 0x06000FFB RID: 4091
		public extern int gridResolutionZ
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000FFC RID: 4092
		// (set) Token: 0x06000FFD RID: 4093
		public extern LightProbeProxyVolume.BoundingBoxMode boundingBoxMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000FFE RID: 4094
		// (set) Token: 0x06000FFF RID: 4095
		public extern LightProbeProxyVolume.ResolutionMode resolutionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001000 RID: 4096
		// (set) Token: 0x06001001 RID: 4097
		public extern LightProbeProxyVolume.ProbePositionMode probePositionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001002 RID: 4098
		// (set) Token: 0x06001003 RID: 4099
		public extern LightProbeProxyVolume.RefreshMode refreshMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001004 RID: 4100
		// (set) Token: 0x06001005 RID: 4101
		public extern LightProbeProxyVolume.QualityMode qualityMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001006 RID: 4102
		// (set) Token: 0x06001007 RID: 4103
		public extern LightProbeProxyVolume.DataFormat dataFormat
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00015D64 File Offset: 0x00013F64
		public void Update()
		{
			this.SetDirtyFlag(true);
		}

		// Token: 0x06001009 RID: 4105
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetDirtyFlag(bool flag);

		// Token: 0x0600100B RID: 4107
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_boundsGlobal_Injected(out Bounds ret);

		// Token: 0x0600100C RID: 4108
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_sizeCustom_Injected(out Vector3 ret);

		// Token: 0x0600100D RID: 4109
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_sizeCustom_Injected(ref Vector3 value);

		// Token: 0x0600100E RID: 4110
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_originCustom_Injected(out Vector3 ret);

		// Token: 0x0600100F RID: 4111
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_originCustom_Injected(ref Vector3 value);

		// Token: 0x020001AE RID: 430
		public enum ResolutionMode
		{
			// Token: 0x040005FC RID: 1532
			Automatic,
			// Token: 0x040005FD RID: 1533
			Custom
		}

		// Token: 0x020001AF RID: 431
		public enum BoundingBoxMode
		{
			// Token: 0x040005FF RID: 1535
			AutomaticLocal,
			// Token: 0x04000600 RID: 1536
			AutomaticWorld,
			// Token: 0x04000601 RID: 1537
			Custom
		}

		// Token: 0x020001B0 RID: 432
		public enum ProbePositionMode
		{
			// Token: 0x04000603 RID: 1539
			CellCorner,
			// Token: 0x04000604 RID: 1540
			CellCenter
		}

		// Token: 0x020001B1 RID: 433
		public enum RefreshMode
		{
			// Token: 0x04000606 RID: 1542
			Automatic,
			// Token: 0x04000607 RID: 1543
			EveryFrame,
			// Token: 0x04000608 RID: 1544
			ViaScripting
		}

		// Token: 0x020001B2 RID: 434
		public enum QualityMode
		{
			// Token: 0x0400060A RID: 1546
			Low,
			// Token: 0x0400060B RID: 1547
			Normal
		}

		// Token: 0x020001B3 RID: 435
		public enum DataFormat
		{
			// Token: 0x0400060D RID: 1549
			HalfFloat,
			// Token: 0x0400060E RID: 1550
			Float
		}
	}
}
