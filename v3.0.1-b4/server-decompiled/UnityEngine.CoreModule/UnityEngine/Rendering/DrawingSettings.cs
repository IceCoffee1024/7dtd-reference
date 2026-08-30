using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Rendering
{
	// Token: 0x02000458 RID: 1112
	public struct DrawingSettings : IEquatable<DrawingSettings>
	{
		// Token: 0x0600254C RID: 9548 RVA: 0x0003F714 File Offset: 0x0003D914
		public unsafe DrawingSettings(ShaderTagId shaderPassName, SortingSettings sortingSettings)
		{
			this.m_SortingSettings = sortingSettings;
			this.m_PerObjectData = PerObjectData.None;
			this.m_Flags = DrawRendererFlags.EnableInstancing;
			this.m_OverrideShaderID = 0;
			this.m_OverrideShaderPassIndex = 0;
			this.m_OverrideMaterialInstanceId = 0;
			this.m_OverrideMaterialPassIndex = 0;
			this.m_fallbackMaterialInstanceId = 0;
			this.m_MainLightIndex = -1;
			fixed (int* ptr = &this.shaderPassNames.FixedElementField)
			{
				int* ptr2 = ptr;
				*ptr2 = shaderPassName.id;
				for (int i = 1; i < DrawingSettings.maxShaderPasses; i++)
				{
					ptr2[i] = -1;
				}
			}
			this.m_UseSrpBatcher = 0;
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x0003F7A4 File Offset: 0x0003D9A4
		// (set) Token: 0x0600254E RID: 9550 RVA: 0x0003F7BC File Offset: 0x0003D9BC
		public SortingSettings sortingSettings
		{
			get
			{
				return this.m_SortingSettings;
			}
			set
			{
				this.m_SortingSettings = value;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x0003F7C8 File Offset: 0x0003D9C8
		// (set) Token: 0x06002550 RID: 9552 RVA: 0x0003F7E0 File Offset: 0x0003D9E0
		public PerObjectData perObjectData
		{
			get
			{
				return this.m_PerObjectData;
			}
			set
			{
				this.m_PerObjectData = value;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x0003F7EC File Offset: 0x0003D9EC
		// (set) Token: 0x06002552 RID: 9554 RVA: 0x0003F80C File Offset: 0x0003DA0C
		public bool enableDynamicBatching
		{
			get
			{
				return (this.m_Flags & DrawRendererFlags.EnableDynamicBatching) > DrawRendererFlags.None;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= DrawRendererFlags.EnableDynamicBatching;
				}
				else
				{
					this.m_Flags &= ~DrawRendererFlags.EnableDynamicBatching;
				}
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x0003F840 File Offset: 0x0003DA40
		// (set) Token: 0x06002554 RID: 9556 RVA: 0x0003F860 File Offset: 0x0003DA60
		public bool enableInstancing
		{
			get
			{
				return (this.m_Flags & DrawRendererFlags.EnableInstancing) > DrawRendererFlags.None;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= DrawRendererFlags.EnableInstancing;
				}
				else
				{
					this.m_Flags &= ~DrawRendererFlags.EnableInstancing;
				}
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x0003F894 File Offset: 0x0003DA94
		// (set) Token: 0x06002556 RID: 9558 RVA: 0x0003F8C1 File Offset: 0x0003DAC1
		public Material overrideMaterial
		{
			get
			{
				return (this.m_OverrideMaterialInstanceId != 0) ? (Object.FindObjectFromInstanceID(this.m_OverrideMaterialInstanceId) as Material) : null;
			}
			set
			{
				this.m_OverrideMaterialInstanceId = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x0003F8D8 File Offset: 0x0003DAD8
		// (set) Token: 0x06002558 RID: 9560 RVA: 0x0003F905 File Offset: 0x0003DB05
		public Shader overrideShader
		{
			get
			{
				return (this.m_OverrideShaderID != 0) ? (Object.FindObjectFromInstanceID(this.m_OverrideShaderID) as Shader) : null;
			}
			set
			{
				this.m_OverrideShaderID = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002559 RID: 9561 RVA: 0x0003F91C File Offset: 0x0003DB1C
		// (set) Token: 0x0600255A RID: 9562 RVA: 0x0003F934 File Offset: 0x0003DB34
		public int overrideMaterialPassIndex
		{
			get
			{
				return this.m_OverrideMaterialPassIndex;
			}
			set
			{
				this.m_OverrideMaterialPassIndex = value;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600255B RID: 9563 RVA: 0x0003F940 File Offset: 0x0003DB40
		// (set) Token: 0x0600255C RID: 9564 RVA: 0x0003F958 File Offset: 0x0003DB58
		public int overrideShaderPassIndex
		{
			get
			{
				return this.m_OverrideShaderPassIndex;
			}
			set
			{
				this.m_OverrideShaderPassIndex = value;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x0003F964 File Offset: 0x0003DB64
		// (set) Token: 0x0600255E RID: 9566 RVA: 0x0003F991 File Offset: 0x0003DB91
		public Material fallbackMaterial
		{
			get
			{
				return (this.m_fallbackMaterialInstanceId != 0) ? (Object.FindObjectFromInstanceID(this.m_fallbackMaterialInstanceId) as Material) : null;
			}
			set
			{
				this.m_fallbackMaterialInstanceId = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x0003F9A8 File Offset: 0x0003DBA8
		// (set) Token: 0x06002560 RID: 9568 RVA: 0x0003F9C0 File Offset: 0x0003DBC0
		public int mainLightIndex
		{
			get
			{
				return this.m_MainLightIndex;
			}
			set
			{
				this.m_MainLightIndex = value;
			}
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x0003F9CC File Offset: 0x0003DBCC
		public unsafe ShaderTagId GetShaderPassName(int index)
		{
			bool flag = index >= DrawingSettings.maxShaderPasses || index < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Index should range from 0 to DrawSettings.maxShaderPasses ({0}), was {1}", DrawingSettings.maxShaderPasses, index));
			}
			fixed (int* ptr = &this.shaderPassNames.FixedElementField)
			{
				int* ptr2 = ptr;
				return new ShaderTagId
				{
					id = ptr2[index]
				};
			}
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x0003FA40 File Offset: 0x0003DC40
		public unsafe void SetShaderPassName(int index, ShaderTagId shaderPassName)
		{
			bool flag = index >= DrawingSettings.maxShaderPasses || index < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Index should range from 0 to DrawSettings.maxShaderPasses ({0}), was {1}", DrawingSettings.maxShaderPasses, index));
			}
			fixed (int* ptr = &this.shaderPassNames.FixedElementField)
			{
				int* ptr2 = ptr;
				ptr2[index] = shaderPassName.id;
			}
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x0003FAA8 File Offset: 0x0003DCA8
		public bool Equals(DrawingSettings other)
		{
			for (int i = 0; i < DrawingSettings.maxShaderPasses; i++)
			{
				bool flag = !this.GetShaderPassName(i).Equals(other.GetShaderPassName(i));
				if (flag)
				{
					return false;
				}
			}
			return this.m_SortingSettings.Equals(other.m_SortingSettings) && this.m_PerObjectData == other.m_PerObjectData && this.m_Flags == other.m_Flags && this.m_OverrideMaterialInstanceId == other.m_OverrideMaterialInstanceId && this.m_OverrideMaterialPassIndex == other.m_OverrideMaterialPassIndex && this.m_fallbackMaterialInstanceId == other.m_fallbackMaterialInstanceId && this.m_UseSrpBatcher == other.m_UseSrpBatcher;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x0003FB64 File Offset: 0x0003DD64
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is DrawingSettings && this.Equals((DrawingSettings)obj);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x0003FB9C File Offset: 0x0003DD9C
		public override int GetHashCode()
		{
			int num = this.m_SortingSettings.GetHashCode();
			num = (num * 397) ^ (int)this.m_PerObjectData;
			num = (num * 397) ^ (int)this.m_Flags;
			num = (num * 397) ^ this.m_OverrideMaterialInstanceId;
			num = (num * 397) ^ this.m_OverrideMaterialPassIndex;
			num = (num * 397) ^ this.m_fallbackMaterialInstanceId;
			return (num * 397) ^ this.m_UseSrpBatcher;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x0003FC1C File Offset: 0x0003DE1C
		public static bool operator ==(DrawingSettings left, DrawingSettings right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x0003FC38 File Offset: 0x0003DE38
		public static bool operator !=(DrawingSettings left, DrawingSettings right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E13 RID: 3603
		private const int kMaxShaderPasses = 16;

		// Token: 0x04000E14 RID: 3604
		public static readonly int maxShaderPasses = 16;

		// Token: 0x04000E15 RID: 3605
		private SortingSettings m_SortingSettings;

		// Token: 0x04000E16 RID: 3606
		[FixedBuffer(typeof(int), 16)]
		internal DrawingSettings.<shaderPassNames>e__FixedBuffer shaderPassNames;

		// Token: 0x04000E17 RID: 3607
		private PerObjectData m_PerObjectData;

		// Token: 0x04000E18 RID: 3608
		private DrawRendererFlags m_Flags;

		// Token: 0x04000E19 RID: 3609
		private int m_OverrideShaderID;

		// Token: 0x04000E1A RID: 3610
		private int m_OverrideShaderPassIndex;

		// Token: 0x04000E1B RID: 3611
		private int m_OverrideMaterialInstanceId;

		// Token: 0x04000E1C RID: 3612
		private int m_OverrideMaterialPassIndex;

		// Token: 0x04000E1D RID: 3613
		private int m_fallbackMaterialInstanceId;

		// Token: 0x04000E1E RID: 3614
		private int m_MainLightIndex;

		// Token: 0x04000E1F RID: 3615
		private int m_UseSrpBatcher;

		// Token: 0x02000459 RID: 1113
		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 64)]
		public struct <shaderPassNames>e__FixedBuffer
		{
			// Token: 0x04000E20 RID: 3616
			public int FixedElementField;
		}
	}
}
