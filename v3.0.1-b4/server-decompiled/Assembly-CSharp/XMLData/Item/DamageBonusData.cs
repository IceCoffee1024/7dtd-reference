using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001622 RID: 5666
	[Preserve]
	public class DamageBonusData : IXMLData
	{
		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x0600B1AF RID: 45487 RVA: 0x004243D7 File Offset: 0x004225D7
		// (set) Token: 0x0600B1B0 RID: 45488 RVA: 0x004243DF File Offset: 0x004225DF
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

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x0600B1B1 RID: 45489 RVA: 0x004243E8 File Offset: 0x004225E8
		// (set) Token: 0x0600B1B2 RID: 45490 RVA: 0x004243F0 File Offset: 0x004225F0
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

		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x0600B1B3 RID: 45491 RVA: 0x004243F9 File Offset: 0x004225F9
		// (set) Token: 0x0600B1B4 RID: 45492 RVA: 0x00424401 File Offset: 0x00422601
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

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x0600B1B5 RID: 45493 RVA: 0x0042440A File Offset: 0x0042260A
		// (set) Token: 0x0600B1B6 RID: 45494 RVA: 0x00424412 File Offset: 0x00422612
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

		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x0600B1B7 RID: 45495 RVA: 0x0042441B File Offset: 0x0042261B
		// (set) Token: 0x0600B1B8 RID: 45496 RVA: 0x00424423 File Offset: 0x00422623
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

		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x0600B1B9 RID: 45497 RVA: 0x0042442C File Offset: 0x0042262C
		// (set) Token: 0x0600B1BA RID: 45498 RVA: 0x00424434 File Offset: 0x00422634
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

		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x0600B1BB RID: 45499 RVA: 0x0042443D File Offset: 0x0042263D
		// (set) Token: 0x0600B1BC RID: 45500 RVA: 0x00424445 File Offset: 0x00422645
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

		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x0600B1BD RID: 45501 RVA: 0x0042444E File Offset: 0x0042264E
		// (set) Token: 0x0600B1BE RID: 45502 RVA: 0x00424456 File Offset: 0x00422656
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

		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x0600B1BF RID: 45503 RVA: 0x0042445F File Offset: 0x0042265F
		// (set) Token: 0x0600B1C0 RID: 45504 RVA: 0x00424467 File Offset: 0x00422667
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

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x0600B1C1 RID: 45505 RVA: 0x00424470 File Offset: 0x00422670
		// (set) Token: 0x0600B1C2 RID: 45506 RVA: 0x00424478 File Offset: 0x00422678
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

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x0600B1C3 RID: 45507 RVA: 0x00424481 File Offset: 0x00422681
		// (set) Token: 0x0600B1C4 RID: 45508 RVA: 0x00424489 File Offset: 0x00422689
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

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x0600B1C5 RID: 45509 RVA: 0x00424492 File Offset: 0x00422692
		// (set) Token: 0x0600B1C6 RID: 45510 RVA: 0x0042449A File Offset: 0x0042269A
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

		// Token: 0x0600B1C7 RID: 45511 RVA: 0x0042331B File Offset: 0x0042151B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x0400855B RID: 34139
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHead;

		// Token: 0x0400855C RID: 34140
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGlass;

		// Token: 0x0400855D RID: 34141
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pStone;

		// Token: 0x0400855E RID: 34142
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCloth;

		// Token: 0x0400855F RID: 34143
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pConcrete;

		// Token: 0x04008560 RID: 34144
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBoulder;

		// Token: 0x04008561 RID: 34145
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMetal;

		// Token: 0x04008562 RID: 34146
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pWood;

		// Token: 0x04008563 RID: 34147
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pEarth;

		// Token: 0x04008564 RID: 34148
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSnow;

		// Token: 0x04008565 RID: 34149
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pPlants;

		// Token: 0x04008566 RID: 34150
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLeaves;

		// Token: 0x02001623 RID: 5667
		public static class Parser
		{
			// Token: 0x0600B1C9 RID: 45513 RVA: 0x004244A4 File Offset: 0x004226A4
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

			// Token: 0x04008567 RID: 34151
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
