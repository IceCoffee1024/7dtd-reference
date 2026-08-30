using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001641 RID: 5697
	[Preserve]
	public class ExplosionData : IXMLData
	{
		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x0600B2C8 RID: 45768 RVA: 0x0042A4B3 File Offset: 0x004286B3
		// (set) Token: 0x0600B2C9 RID: 45769 RVA: 0x0042A4BB File Offset: 0x004286BB
		public DataItem<int> BlockDamage
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

		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x0600B2CA RID: 45770 RVA: 0x0042A4C4 File Offset: 0x004286C4
		// (set) Token: 0x0600B2CB RID: 45771 RVA: 0x0042A4CC File Offset: 0x004286CC
		public DataItem<int> EntityDamage
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

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x0600B2CC RID: 45772 RVA: 0x0042A4D5 File Offset: 0x004286D5
		// (set) Token: 0x0600B2CD RID: 45773 RVA: 0x0042A4DD File Offset: 0x004286DD
		public DataItem<int> ParticleIndex
		{
			get
			{
				return this.pParticleIndex;
			}
			set
			{
				this.pParticleIndex = value;
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x0600B2CE RID: 45774 RVA: 0x0042A4E6 File Offset: 0x004286E6
		// (set) Token: 0x0600B2CF RID: 45775 RVA: 0x0042A4EE File Offset: 0x004286EE
		public DataItem<int> RadiusBlocks
		{
			get
			{
				return this.pRadiusBlocks;
			}
			set
			{
				this.pRadiusBlocks = value;
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x0600B2D0 RID: 45776 RVA: 0x0042A4F7 File Offset: 0x004286F7
		// (set) Token: 0x0600B2D1 RID: 45777 RVA: 0x0042A4FF File Offset: 0x004286FF
		public DataItem<int> RadiusEntities
		{
			get
			{
				return this.pRadiusEntities;
			}
			set
			{
				this.pRadiusEntities = value;
			}
		}

		// Token: 0x0600B2D2 RID: 45778 RVA: 0x0042865B File Offset: 0x0042685B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x0400864B RID: 34379
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pBlockDamage;

		// Token: 0x0400864C RID: 34380
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pEntityDamage;

		// Token: 0x0400864D RID: 34381
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pParticleIndex;

		// Token: 0x0400864E RID: 34382
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRadiusBlocks;

		// Token: 0x0400864F RID: 34383
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRadiusEntities;

		// Token: 0x02001642 RID: 5698
		public static class Parser
		{
			// Token: 0x0600B2D4 RID: 45780 RVA: 0x0042A508 File Offset: 0x00428708
			public static ExplosionData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "ExplosionData");
				Type type = Type.GetType(typeof(ExplosionData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				ExplosionData explosionData = (ExplosionData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing Explosion", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!ExplosionData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing Explosion", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						if (!(text2 == "BlockDamage"))
						{
							if (!(text2 == "EntityDamage"))
							{
								if (!(text2 == "ParticleIndex"))
								{
									if (!(text2 == "RadiusBlocks"))
									{
										if (text2 == "RadiusEntities")
										{
											int num;
											try
											{
												num = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
											DataItem<int> dataItem = new DataItem<int>("RadiusEntities", num);
											explosionData.pRadiusEntities = dataItem;
										}
									}
									else
									{
										int num2;
										try
										{
											num2 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
										DataItem<int> dataItem2 = new DataItem<int>("RadiusBlocks", num2);
										explosionData.pRadiusBlocks = dataItem2;
									}
								}
								else
								{
									int num3;
									try
									{
										num3 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
									DataItem<int> dataItem3 = new DataItem<int>("ParticleIndex", num3);
									explosionData.pParticleIndex = dataItem3;
								}
							}
							else
							{
								int num4;
								try
								{
									num4 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
								DataItem<int> dataItem4 = new DataItem<int>("EntityDamage", num4);
								explosionData.pEntityDamage = dataItem4;
							}
						}
						else
						{
							int num5;
							try
							{
								num5 = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
							DataItem<int> dataItem5 = new DataItem<int>("BlockDamage", num5);
							explosionData.pBlockDamage = dataItem5;
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
				foreach (KeyValuePair<string, Range<int>> keyValuePair in ExplosionData.Parser.knownAttributesMultiplicity)
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
				return explosionData;
			}

			// Token: 0x04008650 RID: 34384
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"BlockDamage",
					new Range<int>(true, 0, true, 1)
				},
				{
					"EntityDamage",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ParticleIndex",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RadiusBlocks",
					new Range<int>(true, 0, true, 1)
				},
				{
					"RadiusEntities",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
