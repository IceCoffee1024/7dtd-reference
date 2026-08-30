using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace XMLEditing
{
	// Token: 0x02001648 RID: 5704
	public class BlockNode
	{
		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x0600B300 RID: 45824 RVA: 0x0042EF6E File Offset: 0x0042D16E
		// (set) Token: 0x0600B301 RID: 45825 RVA: 0x0042EF76 File Offset: 0x0042D176
		public string Name { get; set; }

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x0600B302 RID: 45826 RVA: 0x0042EF7F File Offset: 0x0042D17F
		// (set) Token: 0x0600B303 RID: 45827 RVA: 0x0042EF87 File Offset: 0x0042D187
		public XElement Element { get; set; }

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x0600B304 RID: 45828 RVA: 0x0042EF90 File Offset: 0x0042D190
		// (set) Token: 0x0600B305 RID: 45829 RVA: 0x0042EF98 File Offset: 0x0042D198
		public BlockNode Parent { get; set; }

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x0600B306 RID: 45830 RVA: 0x0042EFA1 File Offset: 0x0042D1A1
		public List<BlockNode> Children { get; } = new List<BlockNode>();

		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x0600B307 RID: 45831 RVA: 0x0042EFA9 File Offset: 0x0042D1A9
		// (set) Token: 0x0600B308 RID: 45832 RVA: 0x0042EFB1 File Offset: 0x0042D1B1
		public Dictionary<string, BlockNode.ElementInfo> ElementInfos
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Dictionary<string, BlockNode.ElementInfo>();

		// Token: 0x0600B309 RID: 45833 RVA: 0x0042EFBA File Offset: 0x0042D1BA
		public void AddChild(BlockNode child)
		{
			child.Parent = this;
			this.Children.Add(child);
		}

		// Token: 0x0600B30A RID: 45834 RVA: 0x0042EFD0 File Offset: 0x0042D1D0
		public bool TryGetPropertyParent(string targetPropertyName, out BlockNode propertyParentBlockNode, out BlockNode.ElementInfo propertyElementInfo, out int depth)
		{
			depth = 0;
			BlockNode blockNode = this;
			while (blockNode != null)
			{
				if (depth >= 100)
				{
					Debug.LogError("Max recursion depth exceeded!");
					break;
				}
				BlockNode.ElementInfo elementInfo;
				if (blockNode.ElementInfos.TryGetValue(targetPropertyName, out elementInfo))
				{
					if (elementInfo.Element != null)
					{
						propertyParentBlockNode = blockNode;
						propertyElementInfo = elementInfo;
						return true;
					}
					if (!elementInfo.CanInherit)
					{
						propertyParentBlockNode = null;
						propertyElementInfo = null;
						return false;
					}
				}
				blockNode = blockNode.Parent;
				depth++;
			}
			propertyParentBlockNode = null;
			propertyElementInfo = null;
			return false;
		}

		// Token: 0x0600B30B RID: 45835 RVA: 0x0042F044 File Offset: 0x0042D244
		public bool TryGetModelOffset(out Vector3 modelOffset, out int depth, out BlockNode.ModelOffsetType modelOffsetType)
		{
			BlockNode blockNode;
			BlockNode.ElementInfo elementInfo;
			if (this.TryGetPropertyParent("ModelOffset", out blockNode, out elementInfo, out depth))
			{
				modelOffsetType = BlockNode.ModelOffsetType.Explicit;
				modelOffset = StringParsers.ParseVector3(elementInfo.Element.GetAttribute(XNames.value), 0, -1);
				return true;
			}
			BlockNode.ElementInfo elementInfo2;
			if (!this.TryGetPropertyParent("Shape", out blockNode, out elementInfo2, out depth))
			{
				depth = 0;
				BlockNode blockNode2 = this;
				while (blockNode2.Parent != null)
				{
					blockNode2 = blockNode2.Parent;
					depth++;
				}
				modelOffsetType = BlockNode.ModelOffsetType.DefaultShapeNew;
				modelOffset = new Vector3(1f, 0f, 1f);
				return true;
			}
			string text = elementInfo2.Element.GetAttribute(XNames.value).Trim();
			Type typeWithPrefix = ReflectionHelpers.GetTypeWithPrefix("BlockShape", text);
			if (typeWithPrefix == null)
			{
				modelOffsetType = BlockNode.ModelOffsetType.None;
				Debug.LogError("Failed to create shape type \"BlockShape" + text + "\" for block: " + this.Name);
				modelOffset = Vector3.zero;
				return false;
			}
			if (typeof(BlockShapeNew).IsAssignableFrom(typeWithPrefix))
			{
				modelOffsetType = BlockNode.ModelOffsetType.ShapeNew;
				modelOffset = new Vector3(1f, 0f, 1f);
				return true;
			}
			if (typeof(BlockShapeModelEntity).IsAssignableFrom(typeWithPrefix))
			{
				modelOffsetType = BlockNode.ModelOffsetType.ShapeModelEntity;
				modelOffset = new Vector3(0f, 0.5f, 0f);
				return true;
			}
			modelOffsetType = BlockNode.ModelOffsetType.None;
			modelOffset = Vector3.zero;
			return false;
		}

		// Token: 0x0600B30C RID: 45836 RVA: 0x0042F1A4 File Offset: 0x0042D3A4
		public bool ShapeSupportsModelOffset()
		{
			BlockNode blockNode;
			BlockNode.ElementInfo elementInfo;
			int num;
			if (this.TryGetPropertyParent("Shape", out blockNode, out elementInfo, out num))
			{
				string text = elementInfo.Element.GetAttribute(XNames.value).Trim();
				Type typeWithPrefix = ReflectionHelpers.GetTypeWithPrefix("BlockShape", text);
				if (typeWithPrefix == null)
				{
					Debug.LogError("Failed to create shape type \"BlockShape" + text + "\" for block: " + this.Name);
					return false;
				}
				if (!typeof(BlockShapeNew).IsAssignableFrom(typeWithPrefix) && !typeof(BlockShapeModelEntity).IsAssignableFrom(typeWithPrefix))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x02001649 RID: 5705
		public enum ModelOffsetType
		{
			// Token: 0x04008623 RID: 34339
			None,
			// Token: 0x04008624 RID: 34340
			Explicit,
			// Token: 0x04008625 RID: 34341
			ShapeNew,
			// Token: 0x04008626 RID: 34342
			ShapeModelEntity,
			// Token: 0x04008627 RID: 34343
			ShapeExt3dModel,
			// Token: 0x04008628 RID: 34344
			ShapeOther,
			// Token: 0x04008629 RID: 34345
			DefaultShapeNew
		}

		// Token: 0x0200164A RID: 5706
		public class ElementInfo
		{
			// Token: 0x1700155F RID: 5471
			// (get) Token: 0x0600B30E RID: 45838 RVA: 0x0042F255 File Offset: 0x0042D455
			// (set) Token: 0x0600B30F RID: 45839 RVA: 0x0042F25D File Offset: 0x0042D45D
			public bool CanInherit { get; set; } = true;

			// Token: 0x17001560 RID: 5472
			// (get) Token: 0x0600B310 RID: 45840 RVA: 0x0042F266 File Offset: 0x0042D466
			// (set) Token: 0x0600B311 RID: 45841 RVA: 0x0042F26E File Offset: 0x0042D46E
			public bool IsClass { get; set; } = true;

			// Token: 0x17001561 RID: 5473
			// (get) Token: 0x0600B312 RID: 45842 RVA: 0x0042F277 File Offset: 0x0042D477
			// (set) Token: 0x0600B313 RID: 45843 RVA: 0x0042F27F File Offset: 0x0042D47F
			public XElement Element { get; set; }
		}
	}
}
