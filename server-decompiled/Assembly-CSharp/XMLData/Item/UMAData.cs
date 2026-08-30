using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200164D RID: 5709
	public class UMAData : IXMLData
	{
		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x0600B3C0 RID: 46016 RVA: 0x00433603 File Offset: 0x00431803
		// (set) Token: 0x0600B3C1 RID: 46017 RVA: 0x0043360B File Offset: 0x0043180B
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

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x0600B3C2 RID: 46018 RVA: 0x00433614 File Offset: 0x00431814
		// (set) Token: 0x0600B3C3 RID: 46019 RVA: 0x0043361C File Offset: 0x0043181C
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

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x0600B3C4 RID: 46020 RVA: 0x00433625 File Offset: 0x00431825
		// (set) Token: 0x0600B3C5 RID: 46021 RVA: 0x0043362D File Offset: 0x0043182D
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

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x0600B3C6 RID: 46022 RVA: 0x00433636 File Offset: 0x00431836
		// (set) Token: 0x0600B3C7 RID: 46023 RVA: 0x0043363E File Offset: 0x0043183E
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

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x0600B3C8 RID: 46024 RVA: 0x00433647 File Offset: 0x00431847
		// (set) Token: 0x0600B3C9 RID: 46025 RVA: 0x0043364F File Offset: 0x0043184F
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

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x0600B3CA RID: 46026 RVA: 0x00433658 File Offset: 0x00431858
		// (set) Token: 0x0600B3CB RID: 46027 RVA: 0x00433660 File Offset: 0x00431860
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

		// Token: 0x0600B3CC RID: 46028 RVA: 0x0042865B File Offset: 0x0042685B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x040086E4 RID: 34532
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMesh;

		// Token: 0x040086E5 RID: 34533
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pOverlayTints;

		// Token: 0x040086E6 RID: 34534
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pOverlay;

		// Token: 0x040086E7 RID: 34535
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pLayer;

		// Token: 0x040086E8 RID: 34536
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUISlot;

		// Token: 0x040086E9 RID: 34537
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pShowHair;

		// Token: 0x0200164E RID: 5710
		public static class Parser
		{
			// Token: 0x0600B3CE RID: 46030 RVA: 0x0043366C File Offset: 0x0043186C
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

			// Token: 0x040086EA RID: 34538
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
