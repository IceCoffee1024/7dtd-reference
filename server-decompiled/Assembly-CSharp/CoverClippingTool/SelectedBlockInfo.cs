using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x02001693 RID: 5779
	[Serializable]
	public struct SelectedBlockInfo
	{
		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x0600B4E5 RID: 46309 RVA: 0x00439D96 File Offset: 0x00437F96
		// (set) Token: 0x0600B4E6 RID: 46310 RVA: 0x00439DA0 File Offset: 0x00437FA0
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

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x0600B4E7 RID: 46311 RVA: 0x00439DF2 File Offset: 0x00437FF2
		public ShapeReference savedShapeReference
		{
			get
			{
				return this._savedShapeReference;
			}
		}

		// Token: 0x0600B4E8 RID: 46312 RVA: 0x00439DFA File Offset: 0x00437FFA
		public SelectedBlockInfo(int thisIsDumb)
		{
			this._shapeInfo = null;
			this._savedShapeReference = default(ShapeReference);
			this.renderables = new List<Renderable>();
		}

		// Token: 0x0400877F RID: 34687
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BlockShapeInfo _shapeInfo;

		// Token: 0x04008780 RID: 34688
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public ShapeReference _savedShapeReference;

		// Token: 0x04008781 RID: 34689
		public readonly List<Renderable> renderables;
	}
}
