using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200165A RID: 5722
	[Preserve]
	public class AttributesData : IXMLData
	{
		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x0600B32A RID: 45866 RVA: 0x0042C9DF File Offset: 0x0042ABDF
		// (set) Token: 0x0600B32B RID: 45867 RVA: 0x0042C9E7 File Offset: 0x0042ABE7
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

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x0600B32C RID: 45868 RVA: 0x0042C9F0 File Offset: 0x0042ABF0
		// (set) Token: 0x0600B32D RID: 45869 RVA: 0x0042C9F8 File Offset: 0x0042ABF8
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

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x0600B32E RID: 45870 RVA: 0x0042CA01 File Offset: 0x0042AC01
		// (set) Token: 0x0600B32F RID: 45871 RVA: 0x0042CA09 File Offset: 0x0042AC09
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

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x0600B330 RID: 45872 RVA: 0x0042CA12 File Offset: 0x0042AC12
		// (set) Token: 0x0600B331 RID: 45873 RVA: 0x0042CA1A File Offset: 0x0042AC1A
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

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x0600B332 RID: 45874 RVA: 0x0042CA23 File Offset: 0x0042AC23
		// (set) Token: 0x0600B333 RID: 45875 RVA: 0x0042CA2B File Offset: 0x0042AC2B
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

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x0600B334 RID: 45876 RVA: 0x0042CA34 File Offset: 0x0042AC34
		// (set) Token: 0x0600B335 RID: 45877 RVA: 0x0042CA3C File Offset: 0x0042AC3C
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

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x0600B336 RID: 45878 RVA: 0x0042CA45 File Offset: 0x0042AC45
		// (set) Token: 0x0600B337 RID: 45879 RVA: 0x0042CA4D File Offset: 0x0042AC4D
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

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x0600B338 RID: 45880 RVA: 0x0042CA56 File Offset: 0x0042AC56
		// (set) Token: 0x0600B339 RID: 45881 RVA: 0x0042CA5E File Offset: 0x0042AC5E
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

		// Token: 0x0600B33A RID: 45882 RVA: 0x0042C337 File Offset: 0x0042A537
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x04008681 RID: 34433
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pEntityDamage;

		// Token: 0x04008682 RID: 34434
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pBlockDamage;

		// Token: 0x04008683 RID: 34435
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAccuracy;

		// Token: 0x04008684 RID: 34436
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pFalloffRange;

		// Token: 0x04008685 RID: 34437
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainHealth;

		// Token: 0x04008686 RID: 34438
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainFood;

		// Token: 0x04008687 RID: 34439
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pGainWater;

		// Token: 0x04008688 RID: 34440
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDegradationRate;

		// Token: 0x0200165B RID: 5723
		public static class Parser
		{
			// Token: 0x0600B33C RID: 45884 RVA: 0x0042CA68 File Offset: 0x0042AC68
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

			// Token: 0x04008689 RID: 34441
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
