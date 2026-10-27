using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;

namespace XMLData.Item
{
	// Token: 0x0200167A RID: 5754
	[Preserve]
	public class PartsData : IXMLData
	{
		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x0600B494 RID: 46228 RVA: 0x00438A61 File Offset: 0x00436C61
		// (set) Token: 0x0600B495 RID: 46229 RVA: 0x00438A69 File Offset: 0x00436C69
		public DataItem<ItemClass> Stock
		{
			get
			{
				return this.pStock;
			}
			set
			{
				this.pStock = value;
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x0600B496 RID: 46230 RVA: 0x00438A72 File Offset: 0x00436C72
		// (set) Token: 0x0600B497 RID: 46231 RVA: 0x00438A7A File Offset: 0x00436C7A
		public DataItem<ItemClass> Receiver
		{
			get
			{
				return this.pReceiver;
			}
			set
			{
				this.pReceiver = value;
			}
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x0600B498 RID: 46232 RVA: 0x00438A83 File Offset: 0x00436C83
		// (set) Token: 0x0600B499 RID: 46233 RVA: 0x00438A8B File Offset: 0x00436C8B
		public DataItem<ItemClass> Pump
		{
			get
			{
				return this.pPump;
			}
			set
			{
				this.pPump = value;
			}
		}

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x0600B49A RID: 46234 RVA: 0x00438A94 File Offset: 0x00436C94
		// (set) Token: 0x0600B49B RID: 46235 RVA: 0x00438A9C File Offset: 0x00436C9C
		public DataItem<ItemClass> Barrel
		{
			get
			{
				return this.pBarrel;
			}
			set
			{
				this.pBarrel = value;
			}
		}

		// Token: 0x0600B49C RID: 46236 RVA: 0x0042E73B File Offset: 0x0042C93B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x04008772 RID: 34674
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pStock;

		// Token: 0x04008773 RID: 34675
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pReceiver;

		// Token: 0x04008774 RID: 34676
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pPump;

		// Token: 0x04008775 RID: 34677
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pBarrel;

		// Token: 0x0200167B RID: 5755
		public static class Parser
		{
			// Token: 0x0600B49E RID: 46238 RVA: 0x00438AA8 File Offset: 0x00436CA8
			public static PartsData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "PartsData");
				Type type = Type.GetType(typeof(PartsData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				PartsData partsData = (PartsData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing Parts", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!PartsData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing Parts", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						if (!(text2 == "Stock"))
						{
							if (!(text2 == "Receiver"))
							{
								if (!(text2 == "Pump"))
								{
									if (text2 == "Barrel")
									{
										ItemClass itemClass;
										try
										{
											itemClass = null;
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
										DataItem<ItemClass> dataItem = new DataItem<ItemClass>("Barrel", itemClass);
										_updateLater.Add(positionXmlElement, dataItem);
										partsData.pBarrel = dataItem;
									}
								}
								else
								{
									ItemClass itemClass2;
									try
									{
										itemClass2 = null;
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
									DataItem<ItemClass> dataItem2 = new DataItem<ItemClass>("Pump", itemClass2);
									_updateLater.Add(positionXmlElement, dataItem2);
									partsData.pPump = dataItem2;
								}
							}
							else
							{
								ItemClass itemClass3;
								try
								{
									itemClass3 = null;
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
								DataItem<ItemClass> dataItem3 = new DataItem<ItemClass>("Receiver", itemClass3);
								_updateLater.Add(positionXmlElement, dataItem3);
								partsData.pReceiver = dataItem3;
							}
						}
						else
						{
							ItemClass itemClass4;
							try
							{
								itemClass4 = null;
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
							DataItem<ItemClass> dataItem4 = new DataItem<ItemClass>("Stock", itemClass4);
							_updateLater.Add(positionXmlElement, dataItem4);
							partsData.pStock = dataItem4;
						}
						if (!dictionary.ContainsKey(positionXmlElement.Name))
						{
							dictionary[positionXmlElement.Name] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						text2 = positionXmlElement.Name;
						int num = dictionary2[text2];
						dictionary2[text2] = num + 1;
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in PartsData.Parser.knownAttributesMultiplicity)
				{
					int num2 = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num2 < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num2 > keyValuePair.Value.max))
					{
						throw new IncorrectAttributeOccurrenceException(string.Concat(new string[]
						{
							"Element has incorrect number of \"",
							keyValuePair.Key,
							"\" attribute instances, found ",
							num2.ToString(),
							", expected ",
							keyValuePair.Value.ToString()
						}), _elem.LineNumber);
					}
				}
				return partsData;
			}

			// Token: 0x04008776 RID: 34678
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Stock",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Receiver",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Pump",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Barrel",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
