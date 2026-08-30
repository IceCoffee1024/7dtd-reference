using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200163D RID: 5693
	[Preserve]
	public class DamageBonusData : IXMLData
	{
		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x0600B2AC RID: 45740 RVA: 0x00429717 File Offset: 0x00427917
		// (set) Token: 0x0600B2AD RID: 45741 RVA: 0x0042971F File Offset: 0x0042791F
		public DataItem<float> Head
		{
			get
			{
				return this.pHead;
			}
			set
			{
				this.pHead = value;
			}
		}

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x0600B2AE RID: 45742 RVA: 0x00429728 File Offset: 0x00427928
		// (set) Token: 0x0600B2AF RID: 45743 RVA: 0x00429730 File Offset: 0x00427930
		public DataItem<float> Glass
		{
			get
			{
				return this.pGlass;
			}
			set
			{
				this.pGlass = value;
			}
		}

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x0600B2B0 RID: 45744 RVA: 0x00429739 File Offset: 0x00427939
		// (set) Token: 0x0600B2B1 RID: 45745 RVA: 0x00429741 File Offset: 0x00427941
		public DataItem<float> Stone
		{
			get
			{
				return this.pStone;
			}
			set
			{
				this.pStone = value;
			}
		}

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x0600B2B2 RID: 45746 RVA: 0x0042974A File Offset: 0x0042794A
		// (set) Token: 0x0600B2B3 RID: 45747 RVA: 0x00429752 File Offset: 0x00427952
		public DataItem<float> Cloth
		{
			get
			{
				return this.pCloth;
			}
			set
			{
				this.pCloth = value;
			}
		}

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x0600B2B4 RID: 45748 RVA: 0x0042975B File Offset: 0x0042795B
		// (set) Token: 0x0600B2B5 RID: 45749 RVA: 0x00429763 File Offset: 0x00427963
		public DataItem<float> Concrete
		{
			get
			{
				return this.pConcrete;
			}
			set
			{
				this.pConcrete = value;
			}
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x0600B2B6 RID: 45750 RVA: 0x0042976C File Offset: 0x0042796C
		// (set) Token: 0x0600B2B7 RID: 45751 RVA: 0x00429774 File Offset: 0x00427974
		public DataItem<float> Boulder
		{
			get
			{
				return this.pBoulder;
			}
			set
			{
				this.pBoulder = value;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x0600B2B8 RID: 45752 RVA: 0x0042977D File Offset: 0x0042797D
		// (set) Token: 0x0600B2B9 RID: 45753 RVA: 0x00429785 File Offset: 0x00427985
		public DataItem<float> Metal
		{
			get
			{
				return this.pMetal;
			}
			set
			{
				this.pMetal = value;
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x0600B2BA RID: 45754 RVA: 0x0042978E File Offset: 0x0042798E
		// (set) Token: 0x0600B2BB RID: 45755 RVA: 0x00429796 File Offset: 0x00427996
		public DataItem<float> Wood
		{
			get
			{
				return this.pWood;
			}
			set
			{
				this.pWood = value;
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x0600B2BC RID: 45756 RVA: 0x0042979F File Offset: 0x0042799F
		// (set) Token: 0x0600B2BD RID: 45757 RVA: 0x004297A7 File Offset: 0x004279A7
		public DataItem<float> Earth
		{
			get
			{
				return this.pEarth;
			}
			set
			{
				this.pEarth = value;
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x0600B2BE RID: 45758 RVA: 0x004297B0 File Offset: 0x004279B0
		// (set) Token: 0x0600B2BF RID: 45759 RVA: 0x004297B8 File Offset: 0x004279B8
		public DataItem<float> Snow
		{
			get
			{
				return this.pSnow;
			}
			set
			{
				this.pSnow = value;
			}
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x0600B2C0 RID: 45760 RVA: 0x004297C1 File Offset: 0x004279C1
		// (set) Token: 0x0600B2C1 RID: 45761 RVA: 0x004297C9 File Offset: 0x004279C9
		public DataItem<float> Plants
		{
			get
			{
				return this.pPlants;
			}
			set
			{
				this.pPlants = value;
			}
		}

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x0600B2C2 RID: 45762 RVA: 0x004297D2 File Offset: 0x004279D2
		// (set) Token: 0x0600B2C3 RID: 45763 RVA: 0x004297DA File Offset: 0x004279DA
		public DataItem<float> Leaves
		{
			get
			{
				return this.pLeaves;
			}
			set
			{
				this.pLeaves = value;
			}
		}

		// Token: 0x0600B2C4 RID: 45764 RVA: 0x0042865B File Offset: 0x0042685B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x04008634 RID: 34356
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHead;

		// Token: 0x04008635 RID: 34357
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGlass;

		// Token: 0x04008636 RID: 34358
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pStone;

		// Token: 0x04008637 RID: 34359
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCloth;

		// Token: 0x04008638 RID: 34360
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pConcrete;

		// Token: 0x04008639 RID: 34361
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBoulder;

		// Token: 0x0400863A RID: 34362
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMetal;

		// Token: 0x0400863B RID: 34363
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pWood;

		// Token: 0x0400863C RID: 34364
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pEarth;

		// Token: 0x0400863D RID: 34365
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSnow;

		// Token: 0x0400863E RID: 34366
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pPlants;

		// Token: 0x0400863F RID: 34367
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLeaves;

		// Token: 0x0200163E RID: 5694
		public static class Parser
		{
			// Token: 0x0600B2C6 RID: 45766 RVA: 0x004297E4 File Offset: 0x004279E4
			public static DamageBonusData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "DamageBonusData");
				Type type = Type.GetType(typeof(DamageBonusData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				DamageBonusData damageBonusData = (DamageBonusData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing DamageBonus", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!DamageBonusData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing DamageBonus", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 2545987019U)
						{
							if (num <= 1307099730U)
							{
								if (num != 78706450U)
								{
									if (num != 81868168U)
									{
										if (num == 1307099730U)
										{
											if (text2 == "Boulder")
											{
												float num2;
												try
												{
													num2 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
												}
												catch (Exception ex)
												{
													throw new InvalidValueException(string.Concat(new string[]
													{
														"Could not parse attribute \"",
														positionXmlElement.Name,
														"\" value \"",
														ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
														"\""
													}), positionXmlElement.LineNumber, ex);
												}
												DataItem<float> dataItem = new DataItem<float>("Boulder", num2);
												damageBonusData.pBoulder = dataItem;
											}
										}
									}
									else if (text2 == "Wood")
									{
										float num3;
										try
										{
											num3 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex2)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex2);
										}
										DataItem<float> dataItem2 = new DataItem<float>("Wood", num3);
										damageBonusData.pWood = dataItem2;
									}
								}
								else if (text2 == "Snow")
								{
									float num4;
									try
									{
										num4 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex3)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex3);
									}
									DataItem<float> dataItem3 = new DataItem<float>("Snow", num4);
									damageBonusData.pSnow = dataItem3;
								}
							}
							else if (num != 1842662042U)
							{
								if (num != 1858281043U)
								{
									if (num == 2545987019U)
									{
										if (text2 == "Glass")
										{
											float num5;
											try
											{
												num5 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex4)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex4);
											}
											DataItem<float> dataItem4 = new DataItem<float>("Glass", num5);
											damageBonusData.pGlass = dataItem4;
										}
									}
								}
								else if (text2 == "Plants")
								{
									float num6;
									try
									{
										num6 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex5)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex5);
									}
									DataItem<float> dataItem5 = new DataItem<float>("Plants", num6);
									damageBonusData.pPlants = dataItem5;
								}
							}
							else if (text2 == "Stone")
							{
								float num7;
								try
								{
									num7 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex6)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex6);
								}
								DataItem<float> dataItem6 = new DataItem<float>("Stone", num7);
								damageBonusData.pStone = dataItem6;
							}
						}
						else if (num <= 2995012523U)
						{
							if (num != 2553495518U)
							{
								if (num != 2840670588U)
								{
									if (num == 2995012523U)
									{
										if (text2 == "Cloth")
										{
											float num8;
											try
											{
												num8 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
											}
											catch (Exception ex7)
											{
												throw new InvalidValueException(string.Concat(new string[]
												{
													"Could not parse attribute \"",
													positionXmlElement.Name,
													"\" value \"",
													ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
													"\""
												}), positionXmlElement.LineNumber, ex7);
											}
											DataItem<float> dataItem7 = new DataItem<float>("Cloth", num8);
											damageBonusData.pCloth = dataItem7;
										}
									}
								}
								else if (text2 == "Metal")
								{
									float num9;
									try
									{
										num9 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
									}
									catch (Exception ex8)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex8);
									}
									DataItem<float> dataItem8 = new DataItem<float>("Metal", num9);
									damageBonusData.pMetal = dataItem8;
								}
							}
							else if (text2 == "Concrete")
							{
								float num10;
								try
								{
									num10 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex9)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex9);
								}
								DataItem<float> dataItem9 = new DataItem<float>("Concrete", num10);
								damageBonusData.pConcrete = dataItem9;
							}
						}
						else if (num != 2996251363U)
						{
							if (num != 3947615209U)
							{
								if (num == 4159608695U)
								{
									if (text2 == "Earth")
									{
										float num11;
										try
										{
											num11 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
										}
										catch (Exception ex10)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex10);
										}
										DataItem<float> dataItem10 = new DataItem<float>("Earth", num11);
										damageBonusData.pEarth = dataItem10;
									}
								}
							}
							else if (text2 == "Leaves")
							{
								float num12;
								try
								{
									num12 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
								}
								catch (Exception ex11)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex11);
								}
								DataItem<float> dataItem11 = new DataItem<float>("Leaves", num12);
								damageBonusData.pLeaves = dataItem11;
							}
						}
						else if (text2 == "Head")
						{
							float num13;
							try
							{
								num13 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
							}
							catch (Exception ex12)
							{
								throw new InvalidValueException(string.Concat(new string[]
								{
									"Could not parse attribute \"",
									positionXmlElement.Name,
									"\" value \"",
									ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
									"\""
								}), positionXmlElement.LineNumber, ex12);
							}
							DataItem<float> dataItem12 = new DataItem<float>("Head", num13);
							damageBonusData.pHead = dataItem12;
						}
						if (!dictionary.ContainsKey(positionXmlElement.Name))
						{
							dictionary[positionXmlElement.Name] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						text2 = positionXmlElement.Name;
						int num14 = dictionary2[text2];
						dictionary2[text2] = num14 + 1;
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in DamageBonusData.Parser.knownAttributesMultiplicity)
				{
					int num15 = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num15 < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num15 > keyValuePair.Value.max))
					{
						throw new IncorrectAttributeOccurrenceException(string.Concat(new string[]
						{
							"Element has incorrect number of \"",
							keyValuePair.Key,
							"\" attribute instances, found ",
							num15.ToString(),
							", expected ",
							keyValuePair.Value.ToString()
						}), _elem.LineNumber);
					}
				}
				return damageBonusData;
			}

			// Token: 0x04008640 RID: 34368
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Head",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Glass",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Stone",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Cloth",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Concrete",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Boulder",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Metal",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Wood",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Earth",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Snow",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Plants",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Leaves",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
