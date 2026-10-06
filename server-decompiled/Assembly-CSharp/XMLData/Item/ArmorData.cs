using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001658 RID: 5720
	[Preserve]
	public class ArmorData : IXMLData
	{
		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x0600B31C RID: 45852 RVA: 0x0042C2E2 File Offset: 0x0042A4E2
		// (set) Token: 0x0600B31D RID: 45853 RVA: 0x0042C2EA File Offset: 0x0042A4EA
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

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x0600B31E RID: 45854 RVA: 0x0042C2F3 File Offset: 0x0042A4F3
		// (set) Token: 0x0600B31F RID: 45855 RVA: 0x0042C2FB File Offset: 0x0042A4FB
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

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x0600B320 RID: 45856 RVA: 0x0042C304 File Offset: 0x0042A504
		// (set) Token: 0x0600B321 RID: 45857 RVA: 0x0042C30C File Offset: 0x0042A50C
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

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x0600B322 RID: 45858 RVA: 0x0042C315 File Offset: 0x0042A515
		// (set) Token: 0x0600B323 RID: 45859 RVA: 0x0042C31D File Offset: 0x0042A51D
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

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x0600B324 RID: 45860 RVA: 0x0042C326 File Offset: 0x0042A526
		// (set) Token: 0x0600B325 RID: 45861 RVA: 0x0042C32E File Offset: 0x0042A52E
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

		// Token: 0x0600B326 RID: 45862 RVA: 0x0042C337 File Offset: 0x0042A537
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x0400867B RID: 34427
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pMelee;

		// Token: 0x0400867C RID: 34428
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBullet;

		// Token: 0x0400867D RID: 34429
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pPuncture;

		// Token: 0x0400867E RID: 34430
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pBlunt;

		// Token: 0x0400867F RID: 34431
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<float> pExplosive;

		// Token: 0x02001659 RID: 5721
		public static class Parser
		{
			// Token: 0x0600B328 RID: 45864 RVA: 0x0042C340 File Offset: 0x0042A540
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

			// Token: 0x04008680 RID: 34432
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
