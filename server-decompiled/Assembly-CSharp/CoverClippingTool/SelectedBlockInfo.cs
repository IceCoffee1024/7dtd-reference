using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x02001674 RID: 5748
	[Serializable]
	public struct SelectedBlockInfo
	{
		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x0600B453 RID: 46163 RVA: 0x004360BA File Offset: 0x004342BA
		// (set) Token: 0x0600B454 RID: 46164 RVA: 0x004360C4 File Offset: 0x004342C4
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

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x0600B455 RID: 46165 RVA: 0x00436116 File Offset: 0x00434316
		public ShapeReference savedShapeReference
		{
			get
			{
				return this._savedShapeReference;
			}
		}

		// Token: 0x0600B456 RID: 46166 RVA: 0x0043611E File Offset: 0x0043431E
		public SelectedBlockInfo(int thisIsDumb)
		{
			this._shapeInfo = null;
			this._savedShapeReference = default(ShapeReference);
			this.renderables = new List<Renderable>();
		}

		// Token: 0x04008729 RID: 34601
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BlockShapeInfo _shapeInfo;

		// Token: 0x0400872A RID: 34602
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public ShapeReference _savedShapeReference;

		// Token: 0x0400872B RID: 34603
		public readonly List<Renderable> renderables;
	}
}
