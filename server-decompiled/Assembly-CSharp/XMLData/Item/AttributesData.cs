using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200163B RID: 5691
	[Preserve]
	public class AttributesData : IXMLData
	{
		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x0600B298 RID: 45720 RVA: 0x00428D03 File Offset: 0x00426F03
		// (set) Token: 0x0600B299 RID: 45721 RVA: 0x00428D0B File Offset: 0x00426F0B
		public DataItem<string> EntityDamage
		{
			get
			{
				return this.pEntityDamage;
			}
			set
			{
				this.pEntityDamage = value;
			}
		}

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x0600B29A RID: 45722 RVA: 0x00428D14 File Offset: 0x00426F14
		// (set) Token: 0x0600B29B RID: 45723 RVA: 0x00428D1C File Offset: 0x00426F1C
		public DataItem<string> BlockDamage
		{
			get
			{
				return this.pBlockDamage;
			}
			set
			{
				this.pBlockDamage = value;
			}
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x0600B29C RID: 45724 RVA: 0x00428D25 File Offset: 0x00426F25
		// (set) Token: 0x0600B29D RID: 45725 RVA: 0x00428D2D File Offset: 0x00426F2D
		public DataItem<string> Accuracy
		{
			get
			{
				return this.pAccuracy;
			}
			set
			{
				this.pAccuracy = value;
			}
		}

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x0600B29E RID: 45726 RVA: 0x00428D36 File Offset: 0x00426F36
		// (set) Token: 0x0600B29F RID: 45727 RVA: 0x00428D3E File Offset: 0x00426F3E
		public DataItem<string> FalloffRange
		{
			get
			{
				return this.pFalloffRange;
			}
			set
			{
				this.pFalloffRange = value;
			}
		}

		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x0600B2A0 RID: 45728 RVA: 0x00428D47 File Offset: 0x00426F47
		// (set) Token: 0x0600B2A1 RID: 45729 RVA: 0x00428D4F File Offset: 0x00426F4F
		public DataItem<string> GainHealth
		{
			get
			{
				return this.pGainHealth;
			}
			set
			{
				this.pGainHealth = value;
			}
		}

		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x0600B2A2 RID: 45730 RVA: 0x00428D58 File Offset: 0x00426F58
		// (set) Token: 0x0600B2A3 RID: 45731 RVA: 0x00428D60 File Offset: 0x00426F60
		public DataItem<string> GainFood
		{
			get
			{
				return this.pGainFood;
			}
			set
			{
				this.pGainFood = value;
			}
		}

		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x0600B2A4 RID: 45732 RVA: 0x00428D69 File Offset: 0x00426F69
		// (set) Token: 0x0600B2A5 RID: 45733 RVA: 0x00428D71 File Offset: 0x00426F71
		public DataItem<string> GainWater
		{
			get
			{
				return this.pGainWater;
			}
			set
			{
				this.pGainWater = value;
			}
		}

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x0600B2A6 RID: 45734 RVA: 0x00428D7A File Offset: 0x00426F7A
		// (set) Token: 0x0600B2A7 RID: 45735 RVA: 0x00428D82 File Offset: 0x00426F82
		public DataItem<string> DegradationRate
		{
			get
			{
				return this.pDegradationRate;
			}
			set
			{
				this.pDegradationRate = value;
			}
		}

		// Token: 0x0600B2A8 RID: 45736 RVA: 0x0042865B File Offset: 0x0042685B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x0400862B RID: 34347
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pEntityDamage;

		// Token: 0x0400862C RID: 34348
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBlockDamage;

		// Token: 0x0400862D RID: 34349
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAccuracy;

		// Token: 0x0400862E RID: 34350
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFalloffRange;

		// Token: 0x0400862F RID: 34351
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainHealth;

		// Token: 0x04008630 RID: 34352
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainFood;

		// Token: 0x04008631 RID: 34353
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainWater;

		// Token: 0x04008632 RID: 34354
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDegradationRate;

		// Token: 0x0200163C RID: 5692
		public static class Parser
		{
			// Token: 0x0600B2AA RID: 45738 RVA: 0x00428D8C File Offset: 0x00426F8C
			public static AttributesData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "AttributesData");
				Type type = Type.GetType(typeof(AttributesData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				AttributesData attributesData = (AttributesData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing Attributes", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!AttributesData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing Attributes", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 2055669289U)
						{
							if (num <= 711561591U)
							{
								if (num != 535561810U)
								{
									if (num == 711561591U)
									{
										if (text2 == "EntityDamage")
										{
											string text3;
											try
											{
												text3 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
											DataItem<string> dataItem = new DataItem<string>("EntityDamage", text3);
											attributesData.pEntityDamage = dataItem;
										}
									}
								}
								else if (text2 == "FalloffRange")
								{
									string text4;
									try
									{
										text4 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
									DataItem<string> dataItem2 = new DataItem<string>("FalloffRange", text4);
									attributesData.pFalloffRange = dataItem2;
								}
							}
							else if (num != 1827041476U)
							{
								if (num == 2055669289U)
								{
									if (text2 == "DegradationRate")
									{
										string text5;
										try
										{
											text5 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
										DataItem<string> dataItem3 = new DataItem<string>("DegradationRate", text5);
										attributesData.pDegradationRate = dataItem3;
									}
								}
							}
							else if (text2 == "Accuracy")
							{
								string text6;
								try
								{
									text6 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
								DataItem<string> dataItem4 = new DataItem<string>("Accuracy", text6);
								attributesData.pAccuracy = dataItem4;
							}
						}
						else if (num <= 2614118511U)
						{
							if (num != 2391273097U)
							{
								if (num == 2614118511U)
								{
									if (text2 == "BlockDamage")
									{
										string text7;
										try
										{
											text7 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
										DataItem<string> dataItem5 = new DataItem<string>("BlockDamage", text7);
										attributesData.pBlockDamage = dataItem5;
									}
								}
							}
							else if (text2 == "GainWater")
							{
								string text8;
								try
								{
									text8 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
								DataItem<string> dataItem6 = new DataItem<string>("GainWater", text8);
								attributesData.pGainWater = dataItem6;
							}
						}
						else if (num != 3036802414U)
						{
							if (num == 3448204316U)
							{
								if (text2 == "GainHealth")
								{
									string text9;
									try
									{
										text9 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
									DataItem<string> dataItem7 = new DataItem<string>("GainHealth", text9);
									attributesData.pGainHealth = dataItem7;
								}
							}
						}
						else if (text2 == "GainFood")
						{
							string text10;
							try
							{
								text10 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
							DataItem<string> dataItem8 = new DataItem<string>("GainFood", text10);
							attributesData.pGainFood = dataItem8;
						}
						if (!dictionary.ContainsKey(positionXmlElement.Name))
						{
							dictionary[positionXmlElement.Name] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						text2 = positionXmlElement.Name;
						int num2 = dictionary2[text2];
						dictionary2[text2] = num2 + 1;
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in AttributesData.Parser.knownAttributesMultiplicity)
				{
					int num3 = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num3 < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num3 > keyValuePair.Value.max))
					{
						throw new IncorrectAttributeOccurrenceException(string.Concat(new string[]
						{
							"Element has incorrect number of \"",
							keyValuePair.Key,
							"\" attribute instances, found ",
							num3.ToString(),
							", expected ",
							keyValuePair.Value.ToString()
						}), _elem.LineNumber);
					}
				}
				return attributesData;
			}

			// Token: 0x04008633 RID: 34355
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"EntityDamage",
					new Range<int>(true, 0, true, 1)
				},
				{
					"BlockDamage",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Accuracy",
					new Range<int>(true, 0, true, 1)
				},
				{
					"FalloffRange",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainHealth",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainFood",
					new Range<int>(true, 0, true, 1)
				},
				{
					"GainWater",
					new Range<int>(true, 0, true, 1)
				},
				{
					"DegradationRate",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
