using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering
{
	// Token: 0x02000465 RID: 1125
	public struct RendererListParams : IEquatable<RendererListParams>
	{
		// Token: 0x060025AC RID: 9644 RVA: 0x00040420 File Offset: 0x0003E620
		public RendererListParams(CullingResults cullingResults, DrawingSettings drawSettings, FilteringSettings filteringSettings)
		{
			this.cullingResults = cullingResults;
			this.drawSettings = drawSettings;
			this.filteringSettings = filteringSettings;
			this.tagName = ShaderTagId.none;
			this.isPassTagName = false;
			this.tagValues = null;
			this.stateBlocks = null;
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x00040470 File Offset: 0x0003E670
		internal int numStateBlocks
		{
			get
			{
				bool flag = this.tagValues != null;
				int num;
				if (flag)
				{
					num = this.tagValues.Value.Length;
				}
				else
				{
					num = 0;
				}
				return num;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x000404A8 File Offset: 0x0003E6A8
		internal IntPtr stateBlocksPtr
		{
			get
			{
				bool flag = this.stateBlocks == null;
				IntPtr intPtr;
				if (flag)
				{
					intPtr = IntPtr.Zero;
				}
				else
				{
					intPtr = (IntPtr)this.stateBlocks.Value.GetUnsafeReadOnlyPtr<RenderStateBlock>();
				}
				return intPtr;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x000404EC File Offset: 0x0003E6EC
		internal IntPtr tagsValuePtr
		{
			get
			{
				bool flag = this.tagValues == null;
				IntPtr intPtr;
				if (flag)
				{
					intPtr = IntPtr.Zero;
				}
				else
				{
					intPtr = (IntPtr)this.tagValues.Value.GetUnsafeReadOnlyPtr<ShaderTagId>();
				}
				return intPtr;
			}
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00040530 File Offset: 0x0003E730
		internal void Dispose()
		{
			bool flag = this.stateBlocks != null;
			if (flag)
			{
				this.stateBlocks.Value.Dispose();
				this.stateBlocks = null;
			}
			bool flag2 = this.tagValues != null;
			if (flag2)
			{
				this.tagValues.Value.Dispose();
				this.tagValues = null;
			}
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000405A0 File Offset: 0x0003E7A0
		internal void Validate()
		{
			bool flag = this.tagValues != null && this.stateBlocks != null;
			if (flag)
			{
				bool flag2 = this.tagValues.Value.Length != this.stateBlocks.Value.Length;
				if (flag2)
				{
					throw new ArgumentException(string.Format("Arrays {0} and {1} should have same length, but {2} had length {3} while {4} had length {5}.", new object[]
					{
						"tagValues",
						"stateBlocks",
						"tagValues",
						this.tagValues.Value.Length,
						"stateBlocks",
						this.stateBlocks.Value.Length
					}));
				}
			}
			else
			{
				bool flag3 = (this.tagValues != null && this.stateBlocks == null) || (this.tagValues == null && this.stateBlocks != null);
				if (flag3)
				{
					throw new ArgumentException(string.Format("Arrays {0} and {1} should have same length, but one of them is null ({2} : {3}, {4} : {5}).", new object[]
					{
						"tagValues",
						"stateBlocks",
						"tagValues",
						this.tagValues != null,
						"stateBlocks",
						this.stateBlocks != null
					}));
				}
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00040710 File Offset: 0x0003E910
		public bool Equals(RendererListParams other)
		{
			return this.cullingResults == other.cullingResults && this.drawSettings == other.drawSettings && this.filteringSettings == other.filteringSettings && this.tagName == other.tagName && this.isPassTagName == other.isPassTagName && this.tagValues == other.tagValues && this.stateBlocks == other.stateBlocks;
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x00040814 File Offset: 0x0003EA14
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RendererListParams && this.Equals((RendererListParams)obj);
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x0004084C File Offset: 0x0003EA4C
		public override int GetHashCode()
		{
			int num = this.cullingResults.GetHashCode();
			num = (num * 397) ^ this.drawSettings.GetHashCode();
			num = (num * 397) ^ this.filteringSettings.GetHashCode();
			num = (num * 397) ^ this.tagName.GetHashCode();
			num = (num * 397) ^ (this.isPassTagName ? 0 : 1);
			num = (num * 397) ^ this.tagValues.GetHashCode();
			return (num * 397) ^ this.stateBlocks.GetHashCode();
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x0004090C File Offset: 0x0003EB0C
		public static bool operator ==(RendererListParams left, RendererListParams right)
		{
			return left.Equals(right);
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x00040928 File Offset: 0x0003EB28
		public static bool operator !=(RendererListParams left, RendererListParams right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E5C RID: 3676
		public static readonly RendererListParams Invalid = default(RendererListParams);

		// Token: 0x04000E5D RID: 3677
		public CullingResults cullingResults;

		// Token: 0x04000E5E RID: 3678
		public DrawingSettings drawSettings;

		// Token: 0x04000E5F RID: 3679
		public FilteringSettings filteringSettings;

		// Token: 0x04000E60 RID: 3680
		public ShaderTagId tagName;

		// Token: 0x04000E61 RID: 3681
		public bool isPassTagName;

		// Token: 0x04000E62 RID: 3682
		public NativeArray<ShaderTagId>? tagValues;

		// Token: 0x04000E63 RID: 3683
		public NativeArray<RenderStateBlock>? stateBlocks;
	}
}
