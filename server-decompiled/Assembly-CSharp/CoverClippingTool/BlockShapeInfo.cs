using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using XMLEditing;

namespace CoverClippingTool
{
	// Token: 0x020016AB RID: 5803
	public class BlockShapeInfo
	{
		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x0600B54B RID: 46411 RVA: 0x0043C4A3 File Offset: 0x0043A6A3
		public bool HasPendingChanges
		{
			get
			{
				return this.modifiedCoverFaceMask != this.CoverFaceMask || !this.CoverFaceMaskMulti.ValuesEquals<Vector3i, BlockFaceFlag>(this.modifiedCoverFaceMaskMulti);
			}
		}

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x0600B54C RID: 46412 RVA: 0x0043C4CC File Offset: 0x0043A6CC
		public Vector3 RenderOffset
		{
			get
			{
				if (this.Source == DataSource.Block)
				{
					Vector3 vector = this.ModelOffset;
					Vector3 vector2 = Vector3.zero;
					if (this.IsOversized)
					{
						vector2 = -this.OversizeBounds.center;
					}
					else
					{
						vector2.y = -0.5f;
						if (this.MultiBlockDim != Vector3i.one)
						{
							if ((this.MultiBlockDim.x & 1) == 0)
							{
								vector2.x = -0.5f;
							}
							if ((this.MultiBlockDim.z & 1) == 0)
							{
								vector2.z = -0.5f;
							}
						}
					}
					vector += vector2;
					vector.y += 0.5f;
					return BlockShapeInfo.PlacerModelOffset + vector;
				}
				return this.ModelOffset;
			}
		}

		// Token: 0x0600B54D RID: 46413 RVA: 0x0043C58D File Offset: 0x0043A78D
		public void RevertChanges()
		{
			this.modifiedCoverFaceMask = this.CoverFaceMask;
		}

		// Token: 0x0600B54E RID: 46414 RVA: 0x0043C59C File Offset: 0x0043A79C
		public bool SaveChanges()
		{
			if (this.HasPendingChanges)
			{
				if (this.modifiedCoverFaceMask != BlockFaceFlag.None || (this.Extends != null && this.Extends.CoverFaceMask != this.modifiedCoverFaceMask))
				{
					this.CoverFaceMask = this.modifiedCoverFaceMask;
					XMLUtils.SetProperty(this.Element, CoverMaskBlockPlacer.PropCoverMask, XNames.value, BlockShapeInfo.SerializeCoverMaskValue(this.CoverFaceMask));
				}
				else if (this.CoverFaceMaskElement != null)
				{
					this.CoverFaceMask = BlockFaceFlag.None;
					if (this.CoverFaceMaskMultiElement.IsChildOf(this.Element))
					{
						if (this.CoverFaceMaskElement.Parent != null)
						{
							this.CoverFaceMaskElement.Remove();
						}
						this.CoverFaceMaskElement = null;
					}
					else
					{
						XMLUtils.SetProperty(this.Element, CoverMaskBlockPlacer.PropCoverMask, XNames.value, BlockShapeInfo.SerializeCoverMaskValue(this.CoverFaceMask));
					}
				}
				if (this.modifiedCoverFaceMaskMulti.Count > 0 || (this.Extends != null && !this.Extends.CoverFaceMaskMulti.ValuesEquals<Vector3i, BlockFaceFlag>(this.CoverFaceMaskMulti)))
				{
					this.CoverFaceMaskMulti.Clear();
					this.modifiedCoverFaceMaskMulti.CopyTo<Vector3i, BlockFaceFlag>(this.CoverFaceMaskMulti, false);
					this.CoverFaceMaskMultiElement = XMLUtils.SetArray(this.Element, CoverMaskBlockPlacer.PropCoverMaskMulti);
					this.CoverFaceMaskMultiElement.RemoveNodes();
					this.CoverFaceMaskMultiElement.Add("\r\n");
					foreach (KeyValuePair<Vector3i, BlockFaceFlag> keyValuePair in this.CoverFaceMaskMulti.OrderBy<KeyValuePair<Vector3i, BlockFaceFlag>, string>((KeyValuePair<Vector3i, BlockFaceFlag> x) => x.Key.ToString()))
					{
						XElement xelement = new XElement(XNames.item);
						this.CoverFaceMaskMultiElement.Add("\t");
						this.CoverFaceMaskMultiElement.Add("\t");
						this.CoverFaceMaskMultiElement.Add("\t");
						this.CoverFaceMaskMultiElement.Add(xelement);
						this.CoverFaceMaskMultiElement.Add("\r\n");
						xelement.SetAttributeValue(CoverMaskBlockPlacer.PropCoverOffset, keyValuePair.Key.ToString());
						xelement.SetAttributeValue(CoverMaskBlockPlacer.PropCoverMask, BlockShapeInfo.SerializeCoverMaskValue(keyValuePair.Value));
					}
					this.CoverFaceMaskMultiElement.Add("\t");
					this.CoverFaceMaskMultiElement.Add("\t");
				}
				else if (this.CoverFaceMaskMultiElement != null)
				{
					this.CoverFaceMaskMulti.Clear();
					if (this.CoverFaceMaskMultiElement.IsChildOf(this.Element))
					{
						if (this.CoverFaceMaskMultiElement.Parent != null)
						{
							this.CoverFaceMaskMultiElement.Remove();
						}
						this.CoverFaceMaskMultiElement = null;
					}
					else
					{
						this.CoverFaceMaskMultiElement = XMLUtils.SetArray(this.Element, CoverMaskBlockPlacer.PropCoverMaskMulti);
						this.CoverFaceMaskMultiElement.RemoveNodes();
						this.CoverFaceMaskMultiElement.Add("\r\n");
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600B54F RID: 46415 RVA: 0x0043C888 File Offset: 0x0043AA88
		public BlockShapeInfo(string name, BlockShapeInfo extends, ShapeDataSet dataSet, XElement element, ShapeData shapeData)
		{
			this.Name = name;
			this.Extends = extends;
			this.Source = dataSet.Source;
			this.Element = element;
			this.Doc = element.Document;
			this.OversizeBounds = default(Bounds);
			PropertyValue propertyValue;
			if (shapeData.Properties.TryGetValue("Model", out propertyValue))
			{
				this.ModelName = propertyValue.Value;
			}
			this.ModelOffset = ((this.Source == DataSource.Block) ? new Vector3(0f, 0.5f, 0f) : new Vector3(1f, 0f, 1f));
			PropertyValue propertyValue2;
			if (shapeData.Properties.TryGetValue("ModelOffset", out propertyValue2))
			{
				this.ModelOffset = StringParsers.ParseVector3(propertyValue2.Value, 0, -1);
			}
			PropertyValue propertyValue3;
			PropertyValue propertyValue4;
			if (shapeData.Properties.TryGetValue("MultiBlockDim", out propertyValue3))
			{
				this.MultiBlockDim = StringParsers.ParseVector3i(propertyValue3.Value, 0, -1, false);
				List<Vector3i> list = new List<Vector3i>();
				if (shapeData.Properties.ContainsKey(Block.PropMultiBlockLayer0))
				{
					int num = 0;
					while (shapeData.Properties.ContainsKey(Block.PropMultiBlockLayer + num.ToString()))
					{
						string[] array = shapeData.Properties[Block.PropMultiBlockLayer + num.ToString()].Value.Split(',', StringSplitOptions.None);
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = array[i].Trim();
							if (array[i].Length > this.MultiBlockDim.x)
							{
								throw new Exception("Multi block layer entry " + i.ToString() + " too long for block ");
							}
							for (int j = 0; j < array[i].Length; j++)
							{
								if (array[i][j] != ' ')
								{
									list.Add(new Vector3i(j, num, i));
								}
							}
						}
						num++;
					}
				}
				else
				{
					int num2 = this.MultiBlockDim.x / 2;
					int num3 = Mathf.RoundToInt((float)this.MultiBlockDim.x / 2f + 0.1f) - 1;
					int num4 = this.MultiBlockDim.z / 2;
					int num5 = Mathf.RoundToInt((float)this.MultiBlockDim.z / 2f + 0.1f) - 1;
					for (int k = -num2; k <= num3; k++)
					{
						for (int l = 0; l < this.MultiBlockDim.y; l++)
						{
							for (int m = -num4; m <= num5; m++)
							{
								list.Add(new Vector3i(k, l, m));
							}
						}
					}
				}
				this.MultiBlockArray = new Block.MultiBlockArray(this.MultiBlockDim, list);
			}
			else if (shapeData.Properties.TryGetValue(Block.PropOversizedBounds, out propertyValue4))
			{
				this.IsOversized = true;
				this.OversizeBounds = StringParsers.ParseBounds(propertyValue4.Value);
				this.MultiBlockDim = World.worldToBlockPos(this.OversizeBounds.size);
			}
			else
			{
				this.MultiBlockDim = Vector3i.one;
			}
			PropertyValue propertyValue5;
			if (shapeData.Properties.TryGetValue(CoverMaskBlockPlacer.PropCoverMask, out propertyValue5))
			{
				this.CoverFaceMaskElement = propertyValue5.Element;
				if (this.CoverFaceMaskElement != null && !this.CoverFaceMaskElement.IsChildOf(this.Element))
				{
					this.CoverFaceMaskElement = null;
				}
				this.CoverFaceMask = StringParsers.ParseCoverFaceMask(propertyValue5.Value);
			}
			else
			{
				this.CoverFaceMaskElement = null;
				this.CoverFaceMask = BlockFaceFlag.None;
			}
			this.modifiedCoverFaceMask = this.CoverFaceMask;
			this.CoverFaceMaskMulti.Clear();
			ArrayValue arrayValue;
			if (shapeData.Arrays.TryGetValue(CoverMaskBlockPlacer.PropCoverMaskMulti, out arrayValue))
			{
				this.CoverFaceMaskMultiElement = arrayValue.Element;
				if (this.CoverFaceMaskMultiElement != null && !this.CoverFaceMaskMultiElement.IsChildOf(this.Element))
				{
					this.CoverFaceMaskMultiElement = null;
				}
				foreach (Dictionary<string, PropertyValue> dictionary in arrayValue.Items)
				{
					PropertyValue propertyValue6;
					PropertyValue propertyValue7;
					Vector3i vector3i;
					if (dictionary != null && dictionary.TryGetValue(CoverMaskBlockPlacer.PropCoverOffset, out propertyValue6) && dictionary.TryGetValue(CoverMaskBlockPlacer.PropCoverMask, out propertyValue7) && StringParsers.TryParseVector3i(propertyValue6.Value, out vector3i))
					{
						this.CoverFaceMaskMulti[vector3i] = StringParsers.ParseCoverFaceMask(propertyValue7.Value);
					}
				}
			}
			this.CoverFaceMaskMulti.CopyTo<Vector3i, BlockFaceFlag>(this.modifiedCoverFaceMaskMulti, false);
			if (this.MultiBlockArray != null)
			{
				HashSet<Vector3i> hashSet = this.MultiBlockArray.pos.ToHashSet<Vector3i>();
				using (Dictionary<Vector3i, BlockFaceFlag>.Enumerator enumerator2 = this.modifiedCoverFaceMaskMulti.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<Vector3i, BlockFaceFlag> keyValuePair = enumerator2.Current;
						if (!hashSet.Contains(keyValuePair.Key))
						{
							this.modifiedCoverFaceMaskMulti.Remove(keyValuePair.Key);
						}
					}
					goto IL_04F8;
				}
			}
			this.modifiedCoverFaceMaskMulti.Clear();
			IL_04F8:
			if (this.MultiBlockArray != null && this.MultiBlockArray.Length > 0)
			{
				this.BlockBounds = this.MultiBlockArray.GetBlockBounds();
				return;
			}
			this.BlockBounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);
		}

		// Token: 0x0600B550 RID: 46416 RVA: 0x0043CDE8 File Offset: 0x0043AFE8
		public static string SerializeCoverMaskValue(BlockFaceFlag coverFaceMask)
		{
			string text = string.Empty;
			for (int i = 0; i < BlockShapeInfo.cubeSideGizmoFaceFlags.Length; i++)
			{
				if ((coverFaceMask & BlockShapeInfo.cubeSideGizmoFaceFlags[i]) != BlockFaceFlag.None)
				{
					text += string.Format("{0},", BlockShapeInfo.cubeSideGizmoFaceChars[i]);
				}
			}
			return text;
		}

		// Token: 0x040087D4 RID: 34772
		public static readonly Vector3 PlacerModelOffset = new Vector3(0.5f, 0f, 0.5f);

		// Token: 0x040087D5 RID: 34773
		public static readonly Vector3 DefaultModelOffset = new Vector3(0f, 0.5f, 0f);

		// Token: 0x040087D6 RID: 34774
		public readonly string Name;

		// Token: 0x040087D7 RID: 34775
		public readonly BlockShapeInfo Extends;

		// Token: 0x040087D8 RID: 34776
		public readonly DataSource Source;

		// Token: 0x040087D9 RID: 34777
		public readonly XElement Element;

		// Token: 0x040087DA RID: 34778
		public readonly XDocument Doc;

		// Token: 0x040087DB RID: 34779
		public readonly string ModelName;

		// Token: 0x040087DC RID: 34780
		public readonly Vector3 ModelOffset;

		// Token: 0x040087DD RID: 34781
		public readonly Vector3i MultiBlockDim;

		// Token: 0x040087DE RID: 34782
		public readonly bool IsOversized;

		// Token: 0x040087DF RID: 34783
		public readonly Bounds OversizeBounds;

		// Token: 0x040087E0 RID: 34784
		public readonly Block.MultiBlockArray MultiBlockArray;

		// Token: 0x040087E1 RID: 34785
		public BlockFaceFlag CoverFaceMask;

		// Token: 0x040087E2 RID: 34786
		public XElement CoverFaceMaskElement;

		// Token: 0x040087E3 RID: 34787
		public readonly Dictionary<Vector3i, BlockFaceFlag> CoverFaceMaskMulti = new Dictionary<Vector3i, BlockFaceFlag>();

		// Token: 0x040087E4 RID: 34788
		public XElement CoverFaceMaskMultiElement;

		// Token: 0x040087E5 RID: 34789
		public readonly BoundsInt BlockBounds;

		// Token: 0x040087E6 RID: 34790
		public BlockFaceFlag modifiedCoverFaceMask;

		// Token: 0x040087E7 RID: 34791
		public readonly Dictionary<Vector3i, BlockFaceFlag> modifiedCoverFaceMaskMulti = new Dictionary<Vector3i, BlockFaceFlag>();

		// Token: 0x040087E8 RID: 34792
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly BlockFaceFlag[] cubeSideGizmoFaceFlags = new BlockFaceFlag[]
		{
			BlockFaceFlag.Top,
			BlockFaceFlag.Bottom,
			BlockFaceFlag.North,
			BlockFaceFlag.West,
			BlockFaceFlag.South,
			BlockFaceFlag.East
		};

		// Token: 0x040087E9 RID: 34793
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly char[] cubeSideGizmoFaceChars = new char[] { 'T', 'B', 'N', 'W', 'S', 'E' };
	}
}
