using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200166C RID: 5740
	[Preserve]
	public class AttributesData : IXMLData
	{
		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x0600B382 RID: 45954 RVA: 0x0042EDE3 File Offset: 0x0042CFE3
		// (set) Token: 0x0600B383 RID: 45955 RVA: 0x0042EDEB File Offset: 0x0042CFEB
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

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x0600B384 RID: 45956 RVA: 0x0042EDF4 File Offset: 0x0042CFF4
		// (set) Token: 0x0600B385 RID: 45957 RVA: 0x0042EDFC File Offset: 0x0042CFFC
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

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x0600B386 RID: 45958 RVA: 0x0042EE05 File Offset: 0x0042D005
		// (set) Token: 0x0600B387 RID: 45959 RVA: 0x0042EE0D File Offset: 0x0042D00D
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

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x0600B388 RID: 45960 RVA: 0x0042EE16 File Offset: 0x0042D016
		// (set) Token: 0x0600B389 RID: 45961 RVA: 0x0042EE1E File Offset: 0x0042D01E
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

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x0600B38A RID: 45962 RVA: 0x0042EE27 File Offset: 0x0042D027
		// (set) Token: 0x0600B38B RID: 45963 RVA: 0x0042EE2F File Offset: 0x0042D02F
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

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x0600B38C RID: 45964 RVA: 0x0042EE38 File Offset: 0x0042D038
		// (set) Token: 0x0600B38D RID: 45965 RVA: 0x0042EE40 File Offset: 0x0042D040
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

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x0600B38E RID: 45966 RVA: 0x0042EE49 File Offset: 0x0042D049
		// (set) Token: 0x0600B38F RID: 45967 RVA: 0x0042EE51 File Offset: 0x0042D051
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

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x0600B390 RID: 45968 RVA: 0x0042EE5A File Offset: 0x0042D05A
		// (set) Token: 0x0600B391 RID: 45969 RVA: 0x0042EE62 File Offset: 0x0042D062
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

		// Token: 0x0600B392 RID: 45970 RVA: 0x0042E73B File Offset: 0x0042C93B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x040086C2 RID: 34498
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pEntityDamage;

		// Token: 0x040086C3 RID: 34499
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBlockDamage;

		// Token: 0x040086C4 RID: 34500
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAccuracy;

		// Token: 0x040086C5 RID: 34501
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFalloffRange;

		// Token: 0x040086C6 RID: 34502
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainHealth;

		// Token: 0x040086C7 RID: 34503
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainFood;

		// Token: 0x040086C8 RID: 34504
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainWater;

		// Token: 0x040086C9 RID: 34505
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDegradationRate;

		// Token: 0x0200166D RID: 5741
		public static class Parser
		{
			// Token: 0x0600B394 RID: 45972 RVA: 0x0042EE6C File Offset: 0x0042D06C
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

			// Token: 0x040086CA RID: 34506
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
