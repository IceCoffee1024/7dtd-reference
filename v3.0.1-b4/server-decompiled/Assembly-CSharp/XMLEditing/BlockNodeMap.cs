using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace XMLEditing
{
	// Token: 0x0200164B RID: 5707
	public class BlockNodeMap : IEnumerable<KeyValuePair<string, BlockNode>>, IEnumerable
	{
		// Token: 0x0600B315 RID: 45845 RVA: 0x0042F29E File Offset: 0x0042D49E
		public IEnumerator<KeyValuePair<string, BlockNode>> GetEnumerator()
		{
			return this.blockNodes.GetEnumerator();
		}

		// Token: 0x0600B316 RID: 45846 RVA: 0x0042F29E File Offset: 0x0042D49E
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.blockNodes.GetEnumerator();
		}

		// Token: 0x0600B317 RID: 45847 RVA: 0x0042F2B0 File Offset: 0x0042D4B0
		public bool TryGetValue(string targetName, out BlockNode blockNode)
		{
			return this.blockNodes.TryGetValue(targetName, out blockNode);
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x0600B318 RID: 45848 RVA: 0x0042F2BF File Offset: 0x0042D4BF
		public int Count
		{
			get
			{
				return this.blockNodes.Count;
			}
		}

		// Token: 0x0600B319 RID: 45849 RVA: 0x0042F2CC File Offset: 0x0042D4CC
		public void PopulateFromFile(string blocksFilePath)
		{
			XDocument xdocument = XMLUtils.LoadXDocument(blocksFilePath);
			this.root = xdocument.Root;
			this.Refresh();
		}

		// Token: 0x0600B31A RID: 45850 RVA: 0x0042F2F2 File Offset: 0x0042D4F2
		public void PopulateFromRoot(XElement root)
		{
			this.root = root;
			this.Refresh();
		}

		// Token: 0x0600B31B RID: 45851 RVA: 0x0042F304 File Offset: 0x0042D504
		public void Refresh()
		{
			if (this.root == null)
			{
				Debug.LogError("Refresh failed: root element is null. This may occur if you have not called one of the PopulateFrom[...] methods prior to calling Refresh. Otherwise there may be an error in the source xml.");
				return;
			}
			if (!this.root.HasElements)
			{
				Debug.LogError("Refresh failed: root element has no child elements.");
				return;
			}
			this.blockNodes.Clear();
			foreach (XElement xelement in this.root.Elements(XNames.block))
			{
				string attribute = xelement.GetAttribute(XNames.name);
				BlockNode blockNode = new BlockNode
				{
					Name = attribute,
					Element = xelement
				};
				this.blockNodes[attribute] = blockNode;
			}
			foreach (BlockNode blockNode2 in this.blockNodes.Values)
			{
				foreach (XElement xelement2 in blockNode2.Element.Elements(XNames.property))
				{
					string text = xelement2.GetAttribute(XNames.name);
					if (text == "Extends")
					{
						string attribute2 = xelement2.GetAttribute(XNames.value);
						BlockNode blockNode3;
						if (this.blockNodes.TryGetValue(attribute2, out blockNode3))
						{
							blockNode3.AddChild(blockNode2);
							foreach (string text2 in xelement2.GetAttribute(XNames.param1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
							{
								BlockNode.ElementInfo elementInfo;
								if (!blockNode2.ElementInfos.TryGetValue(text2, out elementInfo))
								{
									elementInfo = new BlockNode.ElementInfo();
									blockNode2.ElementInfos[text2] = elementInfo;
								}
								elementInfo.CanInherit = false;
							}
							BlockNode.ElementInfo elementInfo2 = new BlockNode.ElementInfo();
							elementInfo2.CanInherit = false;
							elementInfo2.Element = xelement2;
							elementInfo2.IsClass = false;
							blockNode2.ElementInfos["Extends"] = elementInfo2;
						}
						else
						{
							Debug.LogError(string.Concat(new string[] { "Failed to find parent BlockNode \"", attribute2, "\" for block \"", blockNode2.Name, "\"" }));
						}
					}
					else
					{
						bool flag = false;
						if (string.IsNullOrWhiteSpace(text))
						{
							string attribute3 = xelement2.GetAttribute(XNames.class_);
							if (string.IsNullOrWhiteSpace(attribute3))
							{
								continue;
							}
							flag = true;
							text = attribute3;
						}
						BlockNode.ElementInfo elementInfo3;
						if (!blockNode2.ElementInfos.TryGetValue(text, out elementInfo3))
						{
							elementInfo3 = new BlockNode.ElementInfo();
							elementInfo3.CanInherit = text != "CreativeMode";
							blockNode2.ElementInfos[text] = elementInfo3;
						}
						elementInfo3.Element = xelement2;
						elementInfo3.IsClass = flag;
					}
				}
			}
		}

		// Token: 0x0400862D RID: 34349
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, BlockNode> blockNodes = new Dictionary<string, BlockNode>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400862E RID: 34350
		[PublicizedFrom(EAccessModifier.Private)]
		public XElement root;
	}
}
