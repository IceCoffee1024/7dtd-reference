using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001D8 RID: 472
	[NativeHeader("Runtime/Graphics/CustomRenderTexture.h")]
	[UsedByNativeCode]
	public sealed class CustomRenderTexture : RenderTexture
	{
		// Token: 0x0600140B RID: 5131
		[FreeFunction(Name = "CustomRenderTextureScripting::Create")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateCustomRenderTexture([Writable] CustomRenderTexture rt);

		// Token: 0x0600140C RID: 5132
		[NativeName("TriggerUpdate")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TriggerUpdate(int count);

		// Token: 0x0600140D RID: 5133 RVA: 0x0001C6AB File Offset: 0x0001A8AB
		public void Update(int count)
		{
			CustomRenderTextureManager.InvokeTriggerUpdate(this, count);
			this.TriggerUpdate(count);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0001C6BE File Offset: 0x0001A8BE
		public void Update()
		{
			this.Update(1);
		}

		// Token: 0x0600140F RID: 5135
		[NativeName("TriggerInitialization")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TriggerInitialization();

		// Token: 0x06001410 RID: 5136 RVA: 0x0001C6C9 File Offset: 0x0001A8C9
		public void Initialize()
		{
			this.TriggerInitialization();
			CustomRenderTextureManager.InvokeTriggerInitialize(this);
		}

		// Token: 0x06001411 RID: 5137
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearUpdateZones();

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001412 RID: 5138
		// (set) Token: 0x06001413 RID: 5139
		public extern Material material
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001414 RID: 5140
		// (set) Token: 0x06001415 RID: 5141
		public extern Material initializationMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001416 RID: 5142
		// (set) Token: 0x06001417 RID: 5143
		public extern Texture initializationTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001418 RID: 5144
		[FreeFunction(Name = "CustomRenderTextureScripting::GetUpdateZonesInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetUpdateZonesInternal([NotNull("ArgumentNullException")] object updateZones);

		// Token: 0x06001419 RID: 5145 RVA: 0x0001C6DA File Offset: 0x0001A8DA
		public void GetUpdateZones(List<CustomRenderTextureUpdateZone> updateZones)
		{
			this.GetUpdateZonesInternal(updateZones);
		}

		// Token: 0x0600141A RID: 5146
		[FreeFunction(Name = "CustomRenderTextureScripting::SetUpdateZonesInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetUpdateZonesInternal([Unmarshalled] CustomRenderTextureUpdateZone[] updateZones);

		// Token: 0x0600141B RID: 5147
		[FreeFunction(Name = "CustomRenderTextureScripting::GetDoubleBufferRenderTexture", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern RenderTexture GetDoubleBufferRenderTexture();

		// Token: 0x0600141C RID: 5148
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void EnsureDoubleBufferConsistency();

		// Token: 0x0600141D RID: 5149 RVA: 0x0001C6E8 File Offset: 0x0001A8E8
		public void SetUpdateZones(CustomRenderTextureUpdateZone[] updateZones)
		{
			bool flag = updateZones == null;
			if (flag)
			{
				throw new ArgumentNullException("updateZones");
			}
			this.SetUpdateZonesInternal(updateZones);
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x0600141E RID: 5150
		// (set) Token: 0x0600141F RID: 5151
		public extern CustomRenderTextureInitializationSource initializationSource
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0001C714 File Offset: 0x0001A914
		// (set) Token: 0x06001421 RID: 5153 RVA: 0x0001C72A File Offset: 0x0001A92A
		public Color initializationColor
		{
			get
			{
				Color color;
				this.get_initializationColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_initializationColor_Injected(ref value);
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001422 RID: 5154
		// (set) Token: 0x06001423 RID: 5155
		public extern CustomRenderTextureUpdateMode updateMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001424 RID: 5156
		// (set) Token: 0x06001425 RID: 5157
		public extern CustomRenderTextureUpdateMode initializationMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001426 RID: 5158
		// (set) Token: 0x06001427 RID: 5159
		public extern CustomRenderTextureUpdateZoneSpace updateZoneSpace
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001428 RID: 5160
		// (set) Token: 0x06001429 RID: 5161
		public extern int shaderPass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600142A RID: 5162
		// (set) Token: 0x0600142B RID: 5163
		public extern uint cubemapFaceMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600142C RID: 5164
		// (set) Token: 0x0600142D RID: 5165
		public extern bool doubleBuffered
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600142E RID: 5166
		// (set) Token: 0x0600142F RID: 5167
		public extern bool wrapUpdateZones
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001430 RID: 5168
		// (set) Token: 0x06001431 RID: 5169
		public extern float updatePeriod
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0001C734 File Offset: 0x0001A934
		public CustomRenderTexture(int width, int height, RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite)
			: this(width, height, RenderTexture.GetCompatibleFormat(format, readWrite))
		{
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0001C748 File Offset: 0x0001A948
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height, RenderTextureFormat format)
			: this(width, height, format, RenderTextureReadWrite.Default)
		{
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0001C756 File Offset: 0x0001A956
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height)
			: this(width, height, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR))
		{
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0001C768 File Offset: 0x0001A968
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height, [DefaultValue("DefaultFormat.LDR")] DefaultFormat defaultFormat)
			: this(width, height, RenderTexture.GetDefaultColorFormat(defaultFormat))
		{
			bool flag = defaultFormat == DefaultFormat.DepthStencil || defaultFormat == DefaultFormat.Shadow;
			if (flag)
			{
				base.depthStencilFormat = SystemInfo.GetGraphicsFormat(defaultFormat);
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0001C7A4 File Offset: 0x0001A9A4
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height, GraphicsFormat format)
		{
			bool flag = format != GraphicsFormat.None && !base.ValidateFormat(format, FormatUsage.Render);
			if (!flag)
			{
				CustomRenderTexture.Internal_CreateCustomRenderTexture(this);
				this.width = width;
				this.height = height;
				base.graphicsFormat = format;
				base.SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(format));
			}
		}

		// Token: 0x06001437 RID: 5175
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_initializationColor_Injected(out Color ret);

		// Token: 0x06001438 RID: 5176
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_initializationColor_Injected(ref Color value);
	}
}
