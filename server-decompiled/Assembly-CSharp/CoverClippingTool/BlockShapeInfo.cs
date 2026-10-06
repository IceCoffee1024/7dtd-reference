using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using XMLEditing;

namespace CoverClippingTool
{
	// Token: 0x02001699 RID: 5785
	public class BlockShapeInfo
	{
		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x0600B4F3 RID: 46323 RVA: 0x0043A09F File Offset: 0x0043829F
		public bool HasPendingChanges
		{
			get
			{
				return this.modifiedCoverFaceMask != this.CoverFaceMask || !this.CoverFaceMaskMulti.ValuesEquals<Vector3i, BlockFaceFlag>(this.modifiedCoverFaceMaskMulti);
			}
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x0600B4F4 RID: 46324 RVA: 0x0043A0C8 File Offset: 0x004382C8
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

		// Token: 0x0600B4F5 RID: 46325 RVA: 0x0043A189 File Offset: 0x00438389
		public void RevertChanges()
		{
			this.modifiedCoverFaceMask = this.CoverFaceMask;
		}

		// Token: 0x0600B4F6 RID: 46326 RVA: 0x0043A198 File Offset: 0x00438398
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

		// Token: 0x0600B4F7 RID: 46327 RVA: 0x0043A484 File Offset: 0x00438684
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

		// Token: 0x0600B4F8 RID: 46328 RVA: 0x0043A9E4 File Offset: 0x00438BE4
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

		// Token: 0x04008793 RID: 34707
		public static readonly Vector3 PlacerModelOffset = new Vector3(0.5f, 0f, 0.5f);

		// Token: 0x04008794 RID: 34708
		public static readonly Vector3 DefaultModelOffset = new Vector3(0f, 0.5f, 0f);

		// Token: 0x04008795 RID: 34709
		public readonly string Name;

		// Token: 0x04008796 RID: 34710
		public readonly BlockShapeInfo Extends;

		// Token: 0x04008797 RID: 34711
		public readonly DataSource Source;

		// Token: 0x04008798 RID: 34712
		public readonly XElement Element;

		// Token: 0x04008799 RID: 34713
		public readonly XDocument Doc;

		// Token: 0x0400879A RID: 34714
		public readonly string ModelName;

		// Token: 0x0400879B RID: 34715
		public readonly Vector3 ModelOffset;

		// Token: 0x0400879C RID: 34716
		public readonly Vector3i MultiBlockDim;

		// Token: 0x0400879D RID: 34717
		public readonly bool IsOversized;

		// Token: 0x0400879E RID: 34718
		public readonly Bounds OversizeBounds;

		// Token: 0x0400879F RID: 34719
		public readonly Block.MultiBlockArray MultiBlockArray;

		// Token: 0x040087A0 RID: 34720
		public BlockFaceFlag CoverFaceMask;

		// Token: 0x040087A1 RID: 34721
		public XElement CoverFaceMaskElement;

		// Token: 0x040087A2 RID: 34722
		public readonly Dictionary<Vector3i, BlockFaceFlag> CoverFaceMaskMulti = new Dictionary<Vector3i, BlockFaceFlag>();

		// Token: 0x040087A3 RID: 34723
		public XElement CoverFaceMaskMultiElement;

		// Token: 0x040087A4 RID: 34724
		public readonly BoundsInt BlockBounds;

		// Token: 0x040087A5 RID: 34725
		public BlockFaceFlag modifiedCoverFaceMask;

		// Token: 0x040087A6 RID: 34726
		public readonly Dictionary<Vector3i, BlockFaceFlag> modifiedCoverFaceMaskMulti = new Dictionary<Vector3i, BlockFaceFlag>();

		// Token: 0x040087A7 RID: 34727
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

		// Token: 0x040087A8 RID: 34728
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly char[] cubeSideGizmoFaceChars = new char[] { 'T', 'B', 'N', 'W', 'S', 'E' };
	}
}
