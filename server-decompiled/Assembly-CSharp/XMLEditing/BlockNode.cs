using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace XMLEditing
{
	// Token: 0x02001682 RID: 5762
	public class BlockNode
	{
		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x0600B48F RID: 46223 RVA: 0x00437F8A File Offset: 0x0043618A
		// (set) Token: 0x0600B490 RID: 46224 RVA: 0x00437F92 File Offset: 0x00436192
		public string Name { get; set; }

		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x0600B491 RID: 46225 RVA: 0x00437F9B File Offset: 0x0043619B
		// (set) Token: 0x0600B492 RID: 46226 RVA: 0x00437FA3 File Offset: 0x004361A3
		public XElement Element { get; set; }

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x0600B493 RID: 46227 RVA: 0x00437FAC File Offset: 0x004361AC
		// (set) Token: 0x0600B494 RID: 46228 RVA: 0x00437FB4 File Offset: 0x004361B4
		public BlockNode Parent { get; set; }

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x0600B495 RID: 46229 RVA: 0x00437FBD File Offset: 0x004361BD
		public List<BlockNode> Children { get; } = new List<BlockNode>();

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x0600B496 RID: 46230 RVA: 0x00437FC5 File Offset: 0x004361C5
		// (set) Token: 0x0600B497 RID: 46231 RVA: 0x00437FCD File Offset: 0x004361CD
		public Dictionary<string, BlockNode.ElementInfo> ElementInfos
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Dictionary<string, BlockNode.ElementInfo>();

		// Token: 0x0600B498 RID: 46232 RVA: 0x00437FD6 File Offset: 0x004361D6
		public void AddChild(BlockNode child)
		{
			child.Parent = this;
			this.Children.Add(child);
		}

		// Token: 0x0600B499 RID: 46233 RVA: 0x00437FEC File Offset: 0x004361EC
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

		// Token: 0x0600B49A RID: 46234 RVA: 0x00438060 File Offset: 0x00436260
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

		// Token: 0x0600B49B RID: 46235 RVA: 0x004381C0 File Offset: 0x004363C0
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

		// Token: 0x02001683 RID: 5763
		public enum ModelOffsetType
		{
			// Token: 0x04008752 RID: 34642
			None,
			// Token: 0x04008753 RID: 34643
			Explicit,
			// Token: 0x04008754 RID: 34644
			ShapeNew,
			// Token: 0x04008755 RID: 34645
			ShapeModelEntity,
			// Token: 0x04008756 RID: 34646
			ShapeExt3dModel,
			// Token: 0x04008757 RID: 34647
			ShapeOther,
			// Token: 0x04008758 RID: 34648
			DefaultShapeNew
		}

		// Token: 0x02001684 RID: 5764
		public class ElementInfo
		{
			// Token: 0x170015DB RID: 5595
			// (get) Token: 0x0600B49D RID: 46237 RVA: 0x00438271 File Offset: 0x00436471
			// (set) Token: 0x0600B49E RID: 46238 RVA: 0x00438279 File Offset: 0x00436479
			public bool CanInherit { get; set; } = true;

			// Token: 0x170015DC RID: 5596
			// (get) Token: 0x0600B49F RID: 46239 RVA: 0x00438282 File Offset: 0x00436482
			// (set) Token: 0x0600B4A0 RID: 46240 RVA: 0x0043828A File Offset: 0x0043648A
			public bool IsClass { get; set; } = true;

			// Token: 0x170015DD RID: 5597
			// (get) Token: 0x0600B4A1 RID: 46241 RVA: 0x00438293 File Offset: 0x00436493
			// (set) Token: 0x0600B4A2 RID: 46242 RVA: 0x0043829B File Offset: 0x0043649B
			public XElement Element { get; set; }
		}
	}
}
