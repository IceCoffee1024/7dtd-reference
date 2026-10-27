using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200166A RID: 5738
	[Preserve]
	public class ArmorData : IXMLData
	{
		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x0600B374 RID: 45940 RVA: 0x0042E6E6 File Offset: 0x0042C8E6
		// (set) Token: 0x0600B375 RID: 45941 RVA: 0x0042E6EE File Offset: 0x0042C8EE
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

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x0600B376 RID: 45942 RVA: 0x0042E6F7 File Offset: 0x0042C8F7
		// (set) Token: 0x0600B377 RID: 45943 RVA: 0x0042E6FF File Offset: 0x0042C8FF
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

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x0600B378 RID: 45944 RVA: 0x0042E708 File Offset: 0x0042C908
		// (set) Token: 0x0600B379 RID: 45945 RVA: 0x0042E710 File Offset: 0x0042C910
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

		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x0600B37A RID: 45946 RVA: 0x0042E719 File Offset: 0x0042C919
		// (set) Token: 0x0600B37B RID: 45947 RVA: 0x0042E721 File Offset: 0x0042C921
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

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x0600B37C RID: 45948 RVA: 0x0042E72A File Offset: 0x0042C92A
		// (set) Token: 0x0600B37D RID: 45949 RVA: 0x0042E732 File Offset: 0x0042C932
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

		// Token: 0x0600B37E RID: 45950 RVA: 0x0042E73B File Offset: 0x0042C93B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x040086BC RID: 34492
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMelee;

		// Token: 0x040086BD RID: 34493
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBullet;

		// Token: 0x040086BE RID: 34494
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pPuncture;

		// Token: 0x040086BF RID: 34495
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBlunt;

		// Token: 0x040086C0 RID: 34496
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pExplosive;

		// Token: 0x0200166B RID: 5739
		public static class Parser
		{
			// Token: 0x0600B380 RID: 45952 RVA: 0x0042E744 File Offset: 0x0042C944
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

			// Token: 0x040086C1 RID: 34497
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
