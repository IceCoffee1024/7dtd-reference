using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200166C RID: 5740
	public class UMAData : IXMLData
	{
		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x0600B452 RID: 46162 RVA: 0x004372DF File Offset: 0x004354DF
		// (set) Token: 0x0600B453 RID: 46163 RVA: 0x004372E7 File Offset: 0x004354E7
		public DataItem<string> Mesh
		{
			get
			{
				return this.pMesh;
			}
			set
			{
				this.pMesh = value;
			}
		}

		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x0600B454 RID: 46164 RVA: 0x004372F0 File Offset: 0x004354F0
		// (set) Token: 0x0600B455 RID: 46165 RVA: 0x004372F8 File Offset: 0x004354F8
		public DataItem<string> OverlayTints
		{
			get
			{
				return this.pOverlayTints;
			}
			set
			{
				this.pOverlayTints = value;
			}
		}

		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x0600B456 RID: 46166 RVA: 0x00437301 File Offset: 0x00435501
		// (set) Token: 0x0600B457 RID: 46167 RVA: 0x00437309 File Offset: 0x00435509
		public DataItem<string> Overlay
		{
			get
			{
				return this.pOverlay;
			}
			set
			{
				this.pOverlay = value;
			}
		}

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x0600B458 RID: 46168 RVA: 0x00437312 File Offset: 0x00435512
		// (set) Token: 0x0600B459 RID: 46169 RVA: 0x0043731A File Offset: 0x0043551A
		public DataItem<int> Layer
		{
			get
			{
				return this.pLayer;
			}
			set
			{
				this.pLayer = value;
			}
		}

		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x0600B45A RID: 46170 RVA: 0x00437323 File Offset: 0x00435523
		// (set) Token: 0x0600B45B RID: 46171 RVA: 0x0043732B File Offset: 0x0043552B
		public DataItem<string> UISlot
		{
			get
			{
				return this.pUISlot;
			}
			set
			{
				this.pUISlot = value;
			}
		}

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x0600B45C RID: 46172 RVA: 0x00437334 File Offset: 0x00435534
		// (set) Token: 0x0600B45D RID: 46173 RVA: 0x0043733C File Offset: 0x0043553C
		public DataItem<bool> ShowHair
		{
			get
			{
				return this.pShowHair;
			}
			set
			{
				this.pShowHair = value;
			}
		}

		// Token: 0x0600B45E RID: 46174 RVA: 0x0042C337 File Offset: 0x0042A537
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x0400873A RID: 34618
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMesh;

		// Token: 0x0400873B RID: 34619
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pOverlayTints;

		// Token: 0x0400873C RID: 34620
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pOverlay;

		// Token: 0x0400873D RID: 34621
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pLayer;

		// Token: 0x0400873E RID: 34622
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUISlot;

		// Token: 0x0400873F RID: 34623
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pShowHair;

		// Token: 0x0200166D RID: 5741
		public static class Parser
		{
			// Token: 0x0600B460 RID: 46176 RVA: 0x00437348 File Offset: 0x00435548
			public static UMAData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "UMAData");
				Type type = Type.GetType(typeof(UMAData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				UMAData umadata = (UMAData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing UMA", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!UMAData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing UMA", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						if (!(text2 == "Mesh"))
						{
							if (!(text2 == "OverlayTints"))
							{
								if (!(text2 == "Overlay"))
								{
									if (!(text2 == "Layer"))
									{
										if (!(text2 == "UISlot"))
										{
											if (text2 == "ShowHair")
											{
												bool flag;
												try
												{
													flag = boolParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
												DataItem<bool> dataItem = new DataItem<bool>("ShowHair", flag);
												umadata.pShowHair = dataItem;
											}
										}
										else
										{
											string text3;
											try
											{
												text3 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
											DataItem<string> dataItem2 = new DataItem<string>("UISlot", text3);
											umadata.pUISlot = dataItem2;
										}
									}
									else
									{
										int num;
										try
										{
											num = intParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
										DataItem<int> dataItem3 = new DataItem<int>("Layer", num);
										umadata.pLayer = dataItem3;
									}
								}
								else
								{
									string text4;
									try
									{
										text4 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
									DataItem<string> dataItem4 = new DataItem<string>("Overlay", text4);
									umadata.pOverlay = dataItem4;
								}
							}
							else
							{
								string text5;
								try
								{
									text5 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
								DataItem<string> dataItem5 = new DataItem<string>("OverlayTints", text5);
								umadata.pOverlayTints = dataItem5;
							}
						}
						else
						{
							string text6;
							try
							{
								text6 = stringParser.Parse(ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null));
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
							DataItem<string> dataItem6 = new DataItem<string>("Mesh", text6);
							umadata.pMesh = dataItem6;
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
				foreach (KeyValuePair<string, Range<int>> keyValuePair in UMAData.Parser.knownAttributesMultiplicity)
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
				return umadata;
			}

			// Token: 0x04008740 RID: 34624
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Mesh",
					new Range<int>(true, 0, true, 1)
				},
				{
					"OverlayTints",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Overlay",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Layer",
					new Range<int>(true, 0, true, 1)
				},
				{
					"UISlot",
					new Range<int>(true, 0, true, 1)
				},
				{
					"ShowHair",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
