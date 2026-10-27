using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace XMLEditing
{
	// Token: 0x02001697 RID: 5783
	public class BlockNodeMap : IEnumerable<KeyValuePair<string, BlockNode>>, IEnumerable
	{
		// Token: 0x0600B4FC RID: 46332 RVA: 0x0043A6BE File Offset: 0x004388BE
		public IEnumerator<KeyValuePair<string, BlockNode>> GetEnumerator()
		{
			return this.blockNodes.GetEnumerator();
		}

		// Token: 0x0600B4FD RID: 46333 RVA: 0x0043A6BE File Offset: 0x004388BE
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.blockNodes.GetEnumerator();
		}

		// Token: 0x0600B4FE RID: 46334 RVA: 0x0043A6D0 File Offset: 0x004388D0
		public bool TryGetValue(string targetName, out BlockNode blockNode)
		{
			return this.blockNodes.TryGetValue(targetName, out blockNode);
		}

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x0600B4FF RID: 46335 RVA: 0x0043A6DF File Offset: 0x004388DF
		public int Count
		{
			get
			{
				return this.blockNodes.Count;
			}
		}

		// Token: 0x0600B500 RID: 46336 RVA: 0x0043A6EC File Offset: 0x004388EC
		public void PopulateFromFile(string blocksFilePath)
		{
			XDocument xdocument = XMLUtils.LoadXDocument(blocksFilePath);
			this.root = xdocument.Root;
			this.Refresh();
		}

		// Token: 0x0600B501 RID: 46337 RVA: 0x0043A712 File Offset: 0x00438912
		public void PopulateFromRoot(XElement root)
		{
			this.root = root;
			this.Refresh();
		}

		// Token: 0x0600B502 RID: 46338 RVA: 0x0043A724 File Offset: 0x00438924
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

		// Token: 0x0400879D RID: 34717
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, BlockNode> blockNodes = new Dictionary<string, BlockNode>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400879E RID: 34718
		[PublicizedFrom(EAccessModifier.Private)]
		public XElement root;
	}
}
