using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace XMLEditing
{
	// Token: 0x02001694 RID: 5780
	public class BlockNode
	{
		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x0600B4E7 RID: 46311 RVA: 0x0043A38E File Offset: 0x0043858E
		// (set) Token: 0x0600B4E8 RID: 46312 RVA: 0x0043A396 File Offset: 0x00438596
		public string Name { get; set; }

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x0600B4E9 RID: 46313 RVA: 0x0043A39F File Offset: 0x0043859F
		// (set) Token: 0x0600B4EA RID: 46314 RVA: 0x0043A3A7 File Offset: 0x004385A7
		public XElement Element { get; set; }

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x0600B4EB RID: 46315 RVA: 0x0043A3B0 File Offset: 0x004385B0
		// (set) Token: 0x0600B4EC RID: 46316 RVA: 0x0043A3B8 File Offset: 0x004385B8
		public BlockNode Parent { get; set; }

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x0600B4ED RID: 46317 RVA: 0x0043A3C1 File Offset: 0x004385C1
		public List<BlockNode> Children { get; } = new List<BlockNode>();

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x0600B4EE RID: 46318 RVA: 0x0043A3C9 File Offset: 0x004385C9
		// (set) Token: 0x0600B4EF RID: 46319 RVA: 0x0043A3D1 File Offset: 0x004385D1
		public Dictionary<string, BlockNode.ElementInfo> ElementInfos
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Dictionary<string, BlockNode.ElementInfo>();

		// Token: 0x0600B4F0 RID: 46320 RVA: 0x0043A3DA File Offset: 0x004385DA
		public void AddChild(BlockNode child)
		{
			child.Parent = this;
			this.Children.Add(child);
		}

		// Token: 0x0600B4F1 RID: 46321 RVA: 0x0043A3F0 File Offset: 0x004385F0
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

		// Token: 0x0600B4F2 RID: 46322 RVA: 0x0043A464 File Offset: 0x00438664
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

		// Token: 0x0600B4F3 RID: 46323 RVA: 0x0043A5C4 File Offset: 0x004387C4
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

		// Token: 0x02001695 RID: 5781
		public enum ModelOffsetType
		{
			// Token: 0x04008793 RID: 34707
			None,
			// Token: 0x04008794 RID: 34708
			Explicit,
			// Token: 0x04008795 RID: 34709
			ShapeNew,
			// Token: 0x04008796 RID: 34710
			ShapeModelEntity,
			// Token: 0x04008797 RID: 34711
			ShapeExt3dModel,
			// Token: 0x04008798 RID: 34712
			ShapeOther,
			// Token: 0x04008799 RID: 34713
			DefaultShapeNew
		}

		// Token: 0x02001696 RID: 5782
		public class ElementInfo
		{
			// Token: 0x170015E7 RID: 5607
			// (get) Token: 0x0600B4F5 RID: 46325 RVA: 0x0043A675 File Offset: 0x00438875
			// (set) Token: 0x0600B4F6 RID: 46326 RVA: 0x0043A67D File Offset: 0x0043887D
			public bool CanInherit { get; set; } = true;

			// Token: 0x170015E8 RID: 5608
			// (get) Token: 0x0600B4F7 RID: 46327 RVA: 0x0043A686 File Offset: 0x00438886
			// (set) Token: 0x0600B4F8 RID: 46328 RVA: 0x0043A68E File Offset: 0x0043888E
			public bool IsClass { get; set; } = true;

			// Token: 0x170015E9 RID: 5609
			// (get) Token: 0x0600B4F9 RID: 46329 RVA: 0x0043A697 File Offset: 0x00438897
			// (set) Token: 0x0600B4FA RID: 46330 RVA: 0x0043A69F File Offset: 0x0043889F
			public XElement Element { get; set; }
		}
	}
}
