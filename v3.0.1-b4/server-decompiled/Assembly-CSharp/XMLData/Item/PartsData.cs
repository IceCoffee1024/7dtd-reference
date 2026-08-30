using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;

namespace XMLData.Item
{
	// Token: 0x0200162E RID: 5678
	[Preserve]
	public class PartsData : IXMLData
	{
		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x0600B2AD RID: 45741 RVA: 0x0042D641 File Offset: 0x0042B841
		// (set) Token: 0x0600B2AE RID: 45742 RVA: 0x0042D649 File Offset: 0x0042B849
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

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x0600B2AF RID: 45743 RVA: 0x0042D652 File Offset: 0x0042B852
		// (set) Token: 0x0600B2B0 RID: 45744 RVA: 0x0042D65A File Offset: 0x0042B85A
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

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x0600B2B1 RID: 45745 RVA: 0x0042D663 File Offset: 0x0042B863
		// (set) Token: 0x0600B2B2 RID: 45746 RVA: 0x0042D66B File Offset: 0x0042B86B
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

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x0600B2B3 RID: 45747 RVA: 0x0042D674 File Offset: 0x0042B874
		// (set) Token: 0x0600B2B4 RID: 45748 RVA: 0x0042D67C File Offset: 0x0042B87C
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

		// Token: 0x0600B2B5 RID: 45749 RVA: 0x0042331B File Offset: 0x0042151B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x04008602 RID: 34306
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pStock;

		// Token: 0x04008603 RID: 34307
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pReceiver;

		// Token: 0x04008604 RID: 34308
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pPump;

		// Token: 0x04008605 RID: 34309
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pBarrel;

		// Token: 0x0200162F RID: 5679
		public static class Parser
		{
			// Token: 0x0600B2B7 RID: 45751 RVA: 0x0042D688 File Offset: 0x0042B888
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

			// Token: 0x04008606 RID: 34310
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
