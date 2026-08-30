using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x0200044A RID: 1098
	public struct AttachmentDescriptor : IEquatable<AttachmentDescriptor>
	{
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x0003D634 File Offset: 0x0003B834
		// (set) Token: 0x060024A4 RID: 9380 RVA: 0x0003D64C File Offset: 0x0003B84C
		public RenderBufferLoadAction loadAction
		{
			get
			{
				return this.m_LoadAction;
			}
			set
			{
				this.m_LoadAction = value;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x0003D658 File Offset: 0x0003B858
		// (set) Token: 0x060024A6 RID: 9382 RVA: 0x0003D670 File Offset: 0x0003B870
		public RenderBufferStoreAction storeAction
		{
			get
			{
				return this.m_StoreAction;
			}
			set
			{
				this.m_StoreAction = value;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x0003D67C File Offset: 0x0003B87C
		// (set) Token: 0x060024A8 RID: 9384 RVA: 0x0003D694 File Offset: 0x0003B894
		public GraphicsFormat graphicsFormat
		{
			get
			{
				return this.m_Format;
			}
			set
			{
				this.m_Format = value;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x0003D6A0 File Offset: 0x0003B8A0
		// (set) Token: 0x060024AA RID: 9386 RVA: 0x0003D6E6 File Offset: 0x0003B8E6
		public RenderTextureFormat format
		{
			get
			{
				bool flag = GraphicsFormatUtility.IsDepthStencilFormat(this.m_Format) && this.m_Format != GraphicsFormat.ShadowAuto;
				RenderTextureFormat renderTextureFormat;
				if (flag)
				{
					renderTextureFormat = RenderTextureFormat.Depth;
				}
				else
				{
					renderTextureFormat = GraphicsFormatUtility.GetRenderTextureFormat(this.m_Format);
				}
				return renderTextureFormat;
			}
			set
			{
				this.m_Format = GraphicsFormatUtility.GetGraphicsFormat(value, RenderTextureReadWrite.Default);
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060024AB RID: 9387 RVA: 0x0003D6F8 File Offset: 0x0003B8F8
		// (set) Token: 0x060024AC RID: 9388 RVA: 0x0003D710 File Offset: 0x0003B910
		public RenderTargetIdentifier loadStoreTarget
		{
			get
			{
				return this.m_LoadStoreTarget;
			}
			set
			{
				this.m_LoadStoreTarget = value;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x0003D71C File Offset: 0x0003B91C
		// (set) Token: 0x060024AE RID: 9390 RVA: 0x0003D734 File Offset: 0x0003B934
		public RenderTargetIdentifier resolveTarget
		{
			get
			{
				return this.m_ResolveTarget;
			}
			set
			{
				this.m_ResolveTarget = value;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x0003D740 File Offset: 0x0003B940
		// (set) Token: 0x060024B0 RID: 9392 RVA: 0x0003D758 File Offset: 0x0003B958
		public Color clearColor
		{
			get
			{
				return this.m_ClearColor;
			}
			set
			{
				this.m_ClearColor = value;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060024B1 RID: 9393 RVA: 0x0003D764 File Offset: 0x0003B964
		// (set) Token: 0x060024B2 RID: 9394 RVA: 0x0003D77C File Offset: 0x0003B97C
		public float clearDepth
		{
			get
			{
				return this.m_ClearDepth;
			}
			set
			{
				this.m_ClearDepth = value;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060024B3 RID: 9395 RVA: 0x0003D788 File Offset: 0x0003B988
		// (set) Token: 0x060024B4 RID: 9396 RVA: 0x0003D7A0 File Offset: 0x0003B9A0
		public uint clearStencil
		{
			get
			{
				return this.m_ClearStencil;
			}
			set
			{
				this.m_ClearStencil = value;
			}
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x0003D7AC File Offset: 0x0003B9AC
		public void ConfigureTarget(RenderTargetIdentifier target, bool loadExistingContents, bool storeResults)
		{
			this.m_LoadStoreTarget = target;
			bool flag = loadExistingContents && this.m_LoadAction != RenderBufferLoadAction.Clear;
			if (flag)
			{
				this.m_LoadAction = RenderBufferLoadAction.Load;
			}
			if (storeResults)
			{
				bool flag2 = this.m_StoreAction == RenderBufferStoreAction.StoreAndResolve || this.m_StoreAction == RenderBufferStoreAction.Resolve;
				if (flag2)
				{
					this.m_StoreAction = RenderBufferStoreAction.StoreAndResolve;
				}
				else
				{
					this.m_StoreAction = RenderBufferStoreAction.Store;
				}
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x0003D810 File Offset: 0x0003BA10
		public void ConfigureResolveTarget(RenderTargetIdentifier target)
		{
			this.m_ResolveTarget = target;
			bool flag = this.m_StoreAction == RenderBufferStoreAction.StoreAndResolve || this.m_StoreAction == RenderBufferStoreAction.Store;
			if (flag)
			{
				this.m_StoreAction = RenderBufferStoreAction.StoreAndResolve;
			}
			else
			{
				this.m_StoreAction = RenderBufferStoreAction.Resolve;
			}
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x0003D84E File Offset: 0x0003BA4E
		public void ConfigureClear(Color clearColor, float clearDepth = 1f, uint clearStencil = 0U)
		{
			this.m_ClearColor = clearColor;
			this.m_ClearDepth = clearDepth;
			this.m_ClearStencil = clearStencil;
			this.m_LoadAction = RenderBufferLoadAction.Clear;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0003D870 File Offset: 0x0003BA70
		public AttachmentDescriptor(GraphicsFormat format)
		{
			this = default(AttachmentDescriptor);
			this.m_LoadAction = RenderBufferLoadAction.DontCare;
			this.m_StoreAction = RenderBufferStoreAction.DontCare;
			this.m_Format = format;
			this.m_LoadStoreTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.None);
			this.m_ResolveTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.None);
			this.m_ClearColor = new Color(0f, 0f, 0f, 0f);
			this.m_ClearDepth = 1f;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x0003D8DC File Offset: 0x0003BADC
		public AttachmentDescriptor(RenderTextureFormat format)
		{
			this = new AttachmentDescriptor(GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Default));
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x0003D8DC File Offset: 0x0003BADC
		public AttachmentDescriptor(RenderTextureFormat format, RenderTargetIdentifier target, bool loadExistingContents = false, bool storeResults = false, bool resolve = false)
		{
			this = new AttachmentDescriptor(GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Default));
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x0003D8F0 File Offset: 0x0003BAF0
		public bool Equals(AttachmentDescriptor other)
		{
			return this.m_LoadAction == other.m_LoadAction && this.m_StoreAction == other.m_StoreAction && this.m_Format == other.m_Format && this.m_LoadStoreTarget.Equals(other.m_LoadStoreTarget) && this.m_ResolveTarget.Equals(other.m_ResolveTarget) && this.m_ClearColor.Equals(other.m_ClearColor) && this.m_ClearDepth.Equals(other.m_ClearDepth) && this.m_ClearStencil == other.m_ClearStencil;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x0003D98C File Offset: 0x0003BB8C
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is AttachmentDescriptor && this.Equals((AttachmentDescriptor)obj);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0003D9C4 File Offset: 0x0003BBC4
		public override int GetHashCode()
		{
			int num = (int)this.m_LoadAction;
			num = (num * 397) ^ (int)this.m_StoreAction;
			num = (num * 397) ^ (int)this.m_Format;
			num = (num * 397) ^ this.m_LoadStoreTarget.GetHashCode();
			num = (num * 397) ^ this.m_ResolveTarget.GetHashCode();
			num = (num * 397) ^ this.m_ClearColor.GetHashCode();
			num = (num * 397) ^ this.m_ClearDepth.GetHashCode();
			return (num * 397) ^ (int)this.m_ClearStencil;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0003DA70 File Offset: 0x0003BC70
		public static bool operator ==(AttachmentDescriptor left, AttachmentDescriptor right)
		{
			return left.Equals(right);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0003DA8C File Offset: 0x0003BC8C
		public static bool operator !=(AttachmentDescriptor left, AttachmentDescriptor right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000DA5 RID: 3493
		private RenderBufferLoadAction m_LoadAction;

		// Token: 0x04000DA6 RID: 3494
		private RenderBufferStoreAction m_StoreAction;

		// Token: 0x04000DA7 RID: 3495
		private GraphicsFormat m_Format;

		// Token: 0x04000DA8 RID: 3496
		private RenderTargetIdentifier m_LoadStoreTarget;

		// Token: 0x04000DA9 RID: 3497
		private RenderTargetIdentifier m_ResolveTarget;

		// Token: 0x04000DAA RID: 3498
		private Color m_ClearColor;

		// Token: 0x04000DAB RID: 3499
		private float m_ClearDepth;

		// Token: 0x04000DAC RID: 3500
		private uint m_ClearStencil;
	}
}
