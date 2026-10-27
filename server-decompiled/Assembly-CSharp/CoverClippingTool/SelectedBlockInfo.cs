using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x020016A5 RID: 5797
	[Serializable]
	public struct SelectedBlockInfo
	{
		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x0600B53D RID: 46397 RVA: 0x0043C19A File Offset: 0x0043A39A
		// (set) Token: 0x0600B53E RID: 46398 RVA: 0x0043C1A4 File Offset: 0x0043A3A4
		public BlockShapeInfo shapeInfo
		{
			get
			{
				return this._shapeInfo;
			}
			set
			{
				this._shapeInfo = value;
				BlockShapeInfo shapeInfo = this._shapeInfo;
				this._savedShapeReference.Name = ((shapeInfo != null) ? shapeInfo.Name : null);
				this._savedShapeReference.Source = ((this._shapeInfo != null) ? this._shapeInfo.Source : DataSource.Block);
			}
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x0600B53F RID: 46399 RVA: 0x0043C1F6 File Offset: 0x0043A3F6
		public ShapeReference savedShapeReference
		{
			get
			{
				return this._savedShapeReference;
			}
		}

		// Token: 0x0600B540 RID: 46400 RVA: 0x0043C1FE File Offset: 0x0043A3FE
		public SelectedBlockInfo(int thisIsDumb)
		{
			this._shapeInfo = null;
			this._savedShapeReference = default(ShapeReference);
			this.renderables = new List<Renderable>();
		}

		// Token: 0x040087C0 RID: 34752
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BlockShapeInfo _shapeInfo;

		// Token: 0x040087C1 RID: 34753
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public ShapeReference _savedShapeReference;

		// Token: 0x040087C2 RID: 34754
		public readonly List<Renderable> renderables;
	}
}
