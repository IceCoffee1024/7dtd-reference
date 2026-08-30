using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001626 RID: 5670
	[Preserve]
	public class ExplosionData : IXMLData
	{
		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x0600B1CB RID: 45515 RVA: 0x00425173 File Offset: 0x00423373
		// (set) Token: 0x0600B1CC RID: 45516 RVA: 0x0042517B File Offset: 0x0042337B
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

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x0600B1CD RID: 45517 RVA: 0x00425184 File Offset: 0x00423384
		// (set) Token: 0x0600B1CE RID: 45518 RVA: 0x0042518C File Offset: 0x0042338C
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

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x0600B1CF RID: 45519 RVA: 0x00425195 File Offset: 0x00423395
		// (set) Token: 0x0600B1D0 RID: 45520 RVA: 0x0042519D File Offset: 0x0042339D
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

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x0600B1D1 RID: 45521 RVA: 0x004251A6 File Offset: 0x004233A6
		// (set) Token: 0x0600B1D2 RID: 45522 RVA: 0x004251AE File Offset: 0x004233AE
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

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x0600B1D3 RID: 45523 RVA: 0x004251B7 File Offset: 0x004233B7
		// (set) Token: 0x0600B1D4 RID: 45524 RVA: 0x004251BF File Offset: 0x004233BF
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

		// Token: 0x0600B1D5 RID: 45525 RVA: 0x0042331B File Offset: 0x0042151B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x04008572 RID: 34162
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pBlockDamage;

		// Token: 0x04008573 RID: 34163
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pEntityDamage;

		// Token: 0x04008574 RID: 34164
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pParticleIndex;

		// Token: 0x04008575 RID: 34165
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRadiusBlocks;

		// Token: 0x04008576 RID: 34166
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRadiusEntities;

		// Token: 0x02001627 RID: 5671
		public static class Parser
		{
			// Token: 0x0600B1D7 RID: 45527 RVA: 0x004251C8 File Offset: 0x004233C8
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

			// Token: 0x04008577 RID: 34167
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
