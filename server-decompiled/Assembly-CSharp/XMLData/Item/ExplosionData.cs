using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x02001660 RID: 5728
	[Preserve]
	public class ExplosionData : IXMLData
	{
		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x0600B35A RID: 45914 RVA: 0x0042E18F File Offset: 0x0042C38F
		// (set) Token: 0x0600B35B RID: 45915 RVA: 0x0042E197 File Offset: 0x0042C397
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

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x0600B35C RID: 45916 RVA: 0x0042E1A0 File Offset: 0x0042C3A0
		// (set) Token: 0x0600B35D RID: 45917 RVA: 0x0042E1A8 File Offset: 0x0042C3A8
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

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x0600B35E RID: 45918 RVA: 0x0042E1B1 File Offset: 0x0042C3B1
		// (set) Token: 0x0600B35F RID: 45919 RVA: 0x0042E1B9 File Offset: 0x0042C3B9
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

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x0600B360 RID: 45920 RVA: 0x0042E1C2 File Offset: 0x0042C3C2
		// (set) Token: 0x0600B361 RID: 45921 RVA: 0x0042E1CA File Offset: 0x0042C3CA
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

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x0600B362 RID: 45922 RVA: 0x0042E1D3 File Offset: 0x0042C3D3
		// (set) Token: 0x0600B363 RID: 45923 RVA: 0x0042E1DB File Offset: 0x0042C3DB
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

		// Token: 0x0600B364 RID: 45924 RVA: 0x0042C337 File Offset: 0x0042A537
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x040086A1 RID: 34465
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pBlockDamage;

		// Token: 0x040086A2 RID: 34466
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pEntityDamage;

		// Token: 0x040086A3 RID: 34467
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pParticleIndex;

		// Token: 0x040086A4 RID: 34468
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRadiusBlocks;

		// Token: 0x040086A5 RID: 34469
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pRadiusEntities;

		// Token: 0x02001661 RID: 5729
		public static class Parser
		{
			// Token: 0x0600B366 RID: 45926 RVA: 0x0042E1E4 File Offset: 0x0042C3E4
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

			// Token: 0x040086A6 RID: 34470
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
