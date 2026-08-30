using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001639 RID: 5689
	[Preserve]
	public class ArmorData : IXMLData
	{
		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x0600B28A RID: 45706 RVA: 0x00428606 File Offset: 0x00426806
		// (set) Token: 0x0600B28B RID: 45707 RVA: 0x0042860E File Offset: 0x0042680E
		public DataItem<float> Melee
		{
			get
			{
				return this.pMelee;
			}
			set
			{
				this.pMelee = value;
			}
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x0600B28C RID: 45708 RVA: 0x00428617 File Offset: 0x00426817
		// (set) Token: 0x0600B28D RID: 45709 RVA: 0x0042861F File Offset: 0x0042681F
		public DataItem<float> Bullet
		{
			get
			{
				return this.pBullet;
			}
			set
			{
				this.pBullet = value;
			}
		}

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x0600B28E RID: 45710 RVA: 0x00428628 File Offset: 0x00426828
		// (set) Token: 0x0600B28F RID: 45711 RVA: 0x00428630 File Offset: 0x00426830
		public DataItem<float> Puncture
		{
			get
			{
				return this.pPuncture;
			}
			set
			{
				this.pPuncture = value;
			}
		}

		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x0600B290 RID: 45712 RVA: 0x00428639 File Offset: 0x00426839
		// (set) Token: 0x0600B291 RID: 45713 RVA: 0x00428641 File Offset: 0x00426841
		public DataItem<float> Blunt
		{
			get
			{
				return this.pBlunt;
			}
			set
			{
				this.pBlunt = value;
			}
		}

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x0600B292 RID: 45714 RVA: 0x0042864A File Offset: 0x0042684A
		// (set) Token: 0x0600B293 RID: 45715 RVA: 0x00428652 File Offset: 0x00426852
		public DataItem<float> Explosive
		{
			get
			{
				return this.pExplosive;
			}
			set
			{
				this.pExplosive = value;
			}
		}

		// Token: 0x0600B294 RID: 45716 RVA: 0x0042865B File Offset: 0x0042685B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x04008625 RID: 34341
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMelee;

		// Token: 0x04008626 RID: 34342
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBullet;

		// Token: 0x04008627 RID: 34343
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pPuncture;

		// Token: 0x04008628 RID: 34344
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBlunt;

		// Token: 0x04008629 RID: 34345
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pExplosive;

		// Token: 0x0200163A RID: 5690
		public static class Parser
		{
			// Token: 0x0600B296 RID: 45718 RVA: 0x00428664 File Offset: 0x00426864
			public static ArmorData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "ArmorData");
				Type type = Type.GetType(typeof(ArmorData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				ArmorData armorData = (ArmorData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing Armor", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!ArmorData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing Armor", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						if (!(text2 == "Melee"))
						{
							if (!(text2 == "Bullet"))
							{
								if (!(text2 == "Puncture"))
								{
									if (!(text2 == "Blunt"))
									{
										if (text2 == "Explosive")
										{
											float num;
											try
											{
												num = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
											DataItem<float> dataItem = new DataItem<float>("Explosive", num);
											armorData.pExplosive = dataItem;
										}
									}
									else
									{
										float num2;
										try
										{
											num2 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
										DataItem<float> dataItem2 = new DataItem<float>("Blunt", num2);
										armorData.pBlunt = dataItem2;
									}
								}
								else
								{
									float num3;
									try
									{
										num3 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
									DataItem<float> dataItem3 = new DataItem<float>("Puncture", num3);
									armorData.pPuncture = dataItem3;
								}
							}
							else
							{
								float num4;
								try
								{
									num4 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
								DataItem<float> dataItem4 = new DataItem<float>("Bullet", num4);
								armorData.pBullet = dataItem4;
							}
						}
						else
						{
							float num5;
							try
							{
								num5 = floatParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
							DataItem<float> dataItem5 = new DataItem<float>("Melee", num5);
							armorData.pMelee = dataItem5;
						}
						if (!dictionary.ContainsKey(positionXmlElement.Name))
						{
							dictionary[positionXmlElement.Name] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						text2 = positionXmlElement.Name;
						int num6 = dictionary2[text2];
						dictionary2[text2] = num6 + 1;
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in ArmorData.Parser.knownAttributesMultiplicity)
				{
					int num7 = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num7 < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num7 > keyValuePair.Value.max))
					{
						throw new IncorrectAttributeOccurrenceException(string.Concat(new string[]
						{
							"Element has incorrect number of \"",
							keyValuePair.Key,
							"\" attribute instances, found ",
							num7.ToString(),
							", expected ",
							keyValuePair.Value.ToString()
						}), _elem.LineNumber);
					}
				}
				return armorData;
			}

			// Token: 0x0400862A RID: 34346
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Melee",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Bullet",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Puncture",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Blunt",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Explosive",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
