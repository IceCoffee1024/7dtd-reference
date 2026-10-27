using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200166E RID: 5742
	[Preserve]
	public class DamageBonusData : IXMLData
	{
		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x0600B396 RID: 45974 RVA: 0x0042F7F7 File Offset: 0x0042D9F7
		// (set) Token: 0x0600B397 RID: 45975 RVA: 0x0042F7FF File Offset: 0x0042D9FF
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

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x0600B398 RID: 45976 RVA: 0x0042F808 File Offset: 0x0042DA08
		// (set) Token: 0x0600B399 RID: 45977 RVA: 0x0042F810 File Offset: 0x0042DA10
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

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x0600B39A RID: 45978 RVA: 0x0042F819 File Offset: 0x0042DA19
		// (set) Token: 0x0600B39B RID: 45979 RVA: 0x0042F821 File Offset: 0x0042DA21
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

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x0600B39C RID: 45980 RVA: 0x0042F82A File Offset: 0x0042DA2A
		// (set) Token: 0x0600B39D RID: 45981 RVA: 0x0042F832 File Offset: 0x0042DA32
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

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x0600B39E RID: 45982 RVA: 0x0042F83B File Offset: 0x0042DA3B
		// (set) Token: 0x0600B39F RID: 45983 RVA: 0x0042F843 File Offset: 0x0042DA43
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

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x0600B3A0 RID: 45984 RVA: 0x0042F84C File Offset: 0x0042DA4C
		// (set) Token: 0x0600B3A1 RID: 45985 RVA: 0x0042F854 File Offset: 0x0042DA54
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

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x0600B3A2 RID: 45986 RVA: 0x0042F85D File Offset: 0x0042DA5D
		// (set) Token: 0x0600B3A3 RID: 45987 RVA: 0x0042F865 File Offset: 0x0042DA65
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

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x0600B3A4 RID: 45988 RVA: 0x0042F86E File Offset: 0x0042DA6E
		// (set) Token: 0x0600B3A5 RID: 45989 RVA: 0x0042F876 File Offset: 0x0042DA76
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

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x0600B3A6 RID: 45990 RVA: 0x0042F87F File Offset: 0x0042DA7F
		// (set) Token: 0x0600B3A7 RID: 45991 RVA: 0x0042F887 File Offset: 0x0042DA87
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

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x0600B3A8 RID: 45992 RVA: 0x0042F890 File Offset: 0x0042DA90
		// (set) Token: 0x0600B3A9 RID: 45993 RVA: 0x0042F898 File Offset: 0x0042DA98
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

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x0600B3AA RID: 45994 RVA: 0x0042F8A1 File Offset: 0x0042DAA1
		// (set) Token: 0x0600B3AB RID: 45995 RVA: 0x0042F8A9 File Offset: 0x0042DAA9
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

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x0600B3AC RID: 45996 RVA: 0x0042F8B2 File Offset: 0x0042DAB2
		// (set) Token: 0x0600B3AD RID: 45997 RVA: 0x0042F8BA File Offset: 0x0042DABA
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

		// Token: 0x0600B3AE RID: 45998 RVA: 0x0042E73B File Offset: 0x0042C93B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x040086CB RID: 34507
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pHead;

		// Token: 0x040086CC RID: 34508
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pGlass;

		// Token: 0x040086CD RID: 34509
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pStone;

		// Token: 0x040086CE RID: 34510
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pCloth;

		// Token: 0x040086CF RID: 34511
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pConcrete;

		// Token: 0x040086D0 RID: 34512
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBoulder;

		// Token: 0x040086D1 RID: 34513
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMetal;

		// Token: 0x040086D2 RID: 34514
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pWood;

		// Token: 0x040086D3 RID: 34515
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pEarth;

		// Token: 0x040086D4 RID: 34516
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pSnow;

		// Token: 0x040086D5 RID: 34517
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pPlants;

		// Token: 0x040086D6 RID: 34518
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pLeaves;

		// Token: 0x0200166F RID: 5743
		public static class Parser
		{
			// Token: 0x0600B3B0 RID: 46000 RVA: 0x0042F8C4 File Offset: 0x0042DAC4
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

			// Token: 0x040086D7 RID: 34519
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
