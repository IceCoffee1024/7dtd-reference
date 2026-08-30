using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace XMLEditing
{
	// Token: 0x02001663 RID: 5731
	public class BlockNode
	{
		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x0600B3FD RID: 46077 RVA: 0x004342AE File Offset: 0x004324AE
		// (set) Token: 0x0600B3FE RID: 46078 RVA: 0x004342B6 File Offset: 0x004324B6
		public string Name { get; set; }

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x0600B3FF RID: 46079 RVA: 0x004342BF File Offset: 0x004324BF
		// (set) Token: 0x0600B400 RID: 46080 RVA: 0x004342C7 File Offset: 0x004324C7
		public XElement Element { get; set; }

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x0600B401 RID: 46081 RVA: 0x004342D0 File Offset: 0x004324D0
		// (set) Token: 0x0600B402 RID: 46082 RVA: 0x004342D8 File Offset: 0x004324D8
		public BlockNode Parent { get; set; }

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x0600B403 RID: 46083 RVA: 0x004342E1 File Offset: 0x004324E1
		public List<BlockNode> Children { get; } = new List<BlockNode>();

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x0600B404 RID: 46084 RVA: 0x004342E9 File Offset: 0x004324E9
		// (set) Token: 0x0600B405 RID: 46085 RVA: 0x004342F1 File Offset: 0x004324F1
		public Dictionary<string, BlockNode.ElementInfo> ElementInfos
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Dictionary<string, BlockNode.ElementInfo>();

		// Token: 0x0600B406 RID: 46086 RVA: 0x004342FA File Offset: 0x004324FA
		public void AddChild(BlockNode child)
		{
			child.Parent = this;
			this.Children.Add(child);
		}

		// Token: 0x0600B407 RID: 46087 RVA: 0x00434310 File Offset: 0x00432510
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

		// Token: 0x0600B408 RID: 46088 RVA: 0x00434384 File Offset: 0x00432584
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

		// Token: 0x0600B409 RID: 46089 RVA: 0x004344E4 File Offset: 0x004326E4
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

		// Token: 0x02001664 RID: 5732
		public enum ModelOffsetType
		{
			// Token: 0x040086FC RID: 34556
			None,
			// Token: 0x040086FD RID: 34557
			Explicit,
			// Token: 0x040086FE RID: 34558
			ShapeNew,
			// Token: 0x040086FF RID: 34559
			ShapeModelEntity,
			// Token: 0x04008700 RID: 34560
			ShapeExt3dModel,
			// Token: 0x04008701 RID: 34561
			ShapeOther,
			// Token: 0x04008702 RID: 34562
			DefaultShapeNew
		}

		// Token: 0x02001665 RID: 5733
		public class ElementInfo
		{
			// Token: 0x17001598 RID: 5528
			// (get) Token: 0x0600B40B RID: 46091 RVA: 0x00434595 File Offset: 0x00432795
			// (set) Token: 0x0600B40C RID: 46092 RVA: 0x0043459D File Offset: 0x0043279D
			public bool CanInherit { get; set; } = true;

			// Token: 0x17001599 RID: 5529
			// (get) Token: 0x0600B40D RID: 46093 RVA: 0x004345A6 File Offset: 0x004327A6
			// (set) Token: 0x0600B40E RID: 46094 RVA: 0x004345AE File Offset: 0x004327AE
			public bool IsClass { get; set; } = true;

			// Token: 0x1700159A RID: 5530
			// (get) Token: 0x0600B40F RID: 46095 RVA: 0x004345B7 File Offset: 0x004327B7
			// (set) Token: 0x0600B410 RID: 46096 RVA: 0x004345BF File Offset: 0x004327BF
			public XElement Element { get; set; }
		}
	}
}
