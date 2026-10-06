using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200165C RID: 5724
	[Preserve]
	public class DamageBonusData : IXMLData
	{
		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x0600B33E RID: 45886 RVA: 0x0042D3F3 File Offset: 0x0042B5F3
		// (set) Token: 0x0600B33F RID: 45887 RVA: 0x0042D3FB File Offset: 0x0042B5FB
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

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x0600B340 RID: 45888 RVA: 0x0042D404 File Offset: 0x0042B604
		// (set) Token: 0x0600B341 RID: 45889 RVA: 0x0042D40C File Offset: 0x0042B60C
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

		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x0600B342 RID: 45890 RVA: 0x0042D415 File Offset: 0x0042B615
		// (set) Token: 0x0600B343 RID: 45891 RVA: 0x0042D41D File Offset: 0x0042B61D
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

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x0600B344 RID: 45892 RVA: 0x0042D426 File Offset: 0x0042B626
		// (set) Token: 0x0600B345 RID: 45893 RVA: 0x0042D42E File Offset: 0x0042B62E
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

		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x0600B346 RID: 45894 RVA: 0x0042D437 File Offset: 0x0042B637
		// (set) Token: 0x0600B347 RID: 45895 RVA: 0x0042D43F File Offset: 0x0042B63F
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

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x0600B348 RID: 45896 RVA: 0x0042D448 File Offset: 0x0042B648
		// (set) Token: 0x0600B349 RID: 45897 RVA: 0x0042D450 File Offset: 0x0042B650
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

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x0600B34A RID: 45898 RVA: 0x0042D459 File Offset: 0x0042B659
		// (set) Token: 0x0600B34B RID: 45899 RVA: 0x0042D461 File Offset: 0x0042B661
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

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x0600B34C RID: 45900 RVA: 0x0042D46A File Offset: 0x0042B66A
		// (set) Token: 0x0600B34D RID: 45901 RVA: 0x0042D472 File Offset: 0x0042B672
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

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x0600B34E RID: 45902 RVA: 0x0042D47B File Offset: 0x0042B67B
		// (set) Token: 0x0600B34F RID: 45903 RVA: 0x0042D483 File Offset: 0x0042B683
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

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x0600B350 RID: 45904 RVA: 0x0042D48C File Offset: 0x0042B68C
		// (set) Token: 0x0600B351 RID: 45905 RVA: 0x0042D494 File Offset: 0x0042B694
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

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x0600B352 RID: 45906 RVA: 0x0042D49D File Offset: 0x0042B69D
		// (set) Token: 0x0600B353 RID: 45907 RVA: 0x0042D4A5 File Offset: 0x0042B6A5
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

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x0600B354 RID: 45908 RVA: 0x0042D4AE File Offset: 0x0042B6AE
		// (set) Token: 0x0600B355 RID: 45909 RVA: 0x0042D4B6 File Offset: 0x0042B6B6
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

		// Token: 0x0600B356 RID: 45910 RVA: 0x0042C337 File Offset: 0x0042A537
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x0400868A RID: 34442
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHead;

		// Token: 0x0400868B RID: 34443
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGlass;

		// Token: 0x0400868C RID: 34444
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pStone;

		// Token: 0x0400868D RID: 34445
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCloth;

		// Token: 0x0400868E RID: 34446
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pConcrete;

		// Token: 0x0400868F RID: 34447
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBoulder;

		// Token: 0x04008690 RID: 34448
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMetal;

		// Token: 0x04008691 RID: 34449
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pWood;

		// Token: 0x04008692 RID: 34450
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pEarth;

		// Token: 0x04008693 RID: 34451
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSnow;

		// Token: 0x04008694 RID: 34452
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pPlants;

		// Token: 0x04008695 RID: 34453
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLeaves;

		// Token: 0x0200165D RID: 5725
		public static class Parser
		{
			// Token: 0x0600B358 RID: 45912 RVA: 0x0042D4C0 File Offset: 0x0042B6C0
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

			// Token: 0x04008696 RID: 34454
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
