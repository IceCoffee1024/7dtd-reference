using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x02001659 RID: 5721
	[Serializable]
	public struct SelectedBlockInfo
	{
		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x0600B356 RID: 45910 RVA: 0x00430D7A File Offset: 0x0042EF7A
		// (set) Token: 0x0600B357 RID: 45911 RVA: 0x00430D84 File Offset: 0x0042EF84
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

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x0600B358 RID: 45912 RVA: 0x00430DD6 File Offset: 0x0042EFD6
		public ShapeReference savedShapeReference
		{
			get
			{
				return this._savedShapeReference;
			}
		}

		// Token: 0x0600B359 RID: 45913 RVA: 0x00430DDE File Offset: 0x0042EFDE
		public SelectedBlockInfo(int thisIsDumb)
		{
			this._shapeInfo = null;
			this._savedShapeReference = default(ShapeReference);
			this.renderables = new List<Renderable>();
		}

		// Token: 0x04008650 RID: 34384
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BlockShapeInfo _shapeInfo;

		// Token: 0x04008651 RID: 34385
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public ShapeReference _savedShapeReference;

		// Token: 0x04008652 RID: 34386
		public readonly List<Renderable> renderables;
	}
}
