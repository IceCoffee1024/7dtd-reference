using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace XMLData.Item
{
	// Token: 0x0200167E RID: 5758
	public class UMAData : IXMLData
	{
		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x0600B4AA RID: 46250 RVA: 0x004396E3 File Offset: 0x004378E3
		// (set) Token: 0x0600B4AB RID: 46251 RVA: 0x004396EB File Offset: 0x004378EB
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

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x0600B4AC RID: 46252 RVA: 0x004396F4 File Offset: 0x004378F4
		// (set) Token: 0x0600B4AD RID: 46253 RVA: 0x004396FC File Offset: 0x004378FC
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

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x0600B4AE RID: 46254 RVA: 0x00439705 File Offset: 0x00437905
		// (set) Token: 0x0600B4AF RID: 46255 RVA: 0x0043970D File Offset: 0x0043790D
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

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x0600B4B0 RID: 46256 RVA: 0x00439716 File Offset: 0x00437916
		// (set) Token: 0x0600B4B1 RID: 46257 RVA: 0x0043971E File Offset: 0x0043791E
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

		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x0600B4B2 RID: 46258 RVA: 0x00439727 File Offset: 0x00437927
		// (set) Token: 0x0600B4B3 RID: 46259 RVA: 0x0043972F File Offset: 0x0043792F
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

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x0600B4B4 RID: 46260 RVA: 0x00439738 File Offset: 0x00437938
		// (set) Token: 0x0600B4B5 RID: 46261 RVA: 0x00439740 File Offset: 0x00437940
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

		// Token: 0x0600B4B6 RID: 46262 RVA: 0x0042E73B File Offset: 0x0042C93B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x0400877B RID: 34683
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pMesh;

		// Token: 0x0400877C RID: 34684
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pOverlayTints;

		// Token: 0x0400877D RID: 34685
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pOverlay;

		// Token: 0x0400877E RID: 34686
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<int> pLayer;

		// Token: 0x0400877F RID: 34687
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pUISlot;

		// Token: 0x04008780 RID: 34688
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<bool> pShowHair;

		// Token: 0x0200167F RID: 5759
		public static class Parser
		{
			// Token: 0x0600B4B8 RID: 46264 RVA: 0x0043974C File Offset: 0x0043794C
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

			// Token: 0x04008781 RID: 34689
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
