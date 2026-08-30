using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine
{
	// Token: 0x02000139 RID: 313
	internal static class BeforeRenderHelper
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x0000DF38 File Offset: 0x0000C138
		private static int GetUpdateOrder(UnityAction callback)
		{
			object[] customAttributes = callback.Method.GetCustomAttributes(typeof(BeforeRenderOrderAttribute), true);
			BeforeRenderOrderAttribute beforeRenderOrderAttribute = ((customAttributes != null && customAttributes.Length != 0) ? (customAttributes[0] as BeforeRenderOrderAttribute) : null);
			return (beforeRenderOrderAttribute != null) ? beforeRenderOrderAttribute.order : 0;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0000DF80 File Offset: 0x0000C180
		public static void RegisterCallback(UnityAction callback)
		{
			int updateOrder = BeforeRenderHelper.GetUpdateOrder(callback);
			List<BeforeRenderHelper.OrderBlock> list = BeforeRenderHelper.s_OrderBlocks;
			lock (list)
			{
				int num = 0;
				while (num < BeforeRenderHelper.s_OrderBlocks.Count && BeforeRenderHelper.s_OrderBlocks[num].order <= updateOrder)
				{
					bool flag2 = BeforeRenderHelper.s_OrderBlocks[num].order == updateOrder;
					if (flag2)
					{
						BeforeRenderHelper.OrderBlock orderBlock = BeforeRenderHelper.s_OrderBlocks[num];
						orderBlock.callback = (UnityAction)Delegate.Combine(orderBlock.callback, callback);
						BeforeRenderHelper.s_OrderBlocks[num] = orderBlock;
						return;
					}
					num++;
				}
				BeforeRenderHelper.OrderBlock orderBlock2 = default(BeforeRenderHelper.OrderBlock);
				orderBlock2.order = updateOrder;
				orderBlock2.callback = (UnityAction)Delegate.Combine(orderBlock2.callback, callback);
				BeforeRenderHelper.s_OrderBlocks.Insert(num, orderBlock2);
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0000E07C File Offset: 0x0000C27C
		public static void UnregisterCallback(UnityAction callback)
		{
			int updateOrder = BeforeRenderHelper.GetUpdateOrder(callback);
			List<BeforeRenderHelper.OrderBlock> list = BeforeRenderHelper.s_OrderBlocks;
			lock (list)
			{
				int num = 0;
				while (num < BeforeRenderHelper.s_OrderBlocks.Count && BeforeRenderHelper.s_OrderBlocks[num].order <= updateOrder)
				{
					bool flag2 = BeforeRenderHelper.s_OrderBlocks[num].order == updateOrder;
					if (flag2)
					{
						BeforeRenderHelper.OrderBlock orderBlock = BeforeRenderHelper.s_OrderBlocks[num];
						orderBlock.callback = (UnityAction)Delegate.Remove(orderBlock.callback, callback);
						BeforeRenderHelper.s_OrderBlocks[num] = orderBlock;
						bool flag3 = orderBlock.callback == null;
						if (flag3)
						{
							BeforeRenderHelper.s_OrderBlocks.RemoveAt(num);
						}
						break;
					}
					num++;
				}
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000E164 File Offset: 0x0000C364
		public static void Invoke()
		{
			List<BeforeRenderHelper.OrderBlock> list = BeforeRenderHelper.s_OrderBlocks;
			lock (list)
			{
				for (int i = 0; i < BeforeRenderHelper.s_OrderBlocks.Count; i++)
				{
					UnityAction callback = BeforeRenderHelper.s_OrderBlocks[i].callback;
					bool flag2 = callback != null;
					if (flag2)
					{
						callback();
					}
				}
			}
		}

		// Token: 0x040003FC RID: 1020
		private static List<BeforeRenderHelper.OrderBlock> s_OrderBlocks = new List<BeforeRenderHelper.OrderBlock>();

		// Token: 0x0200013A RID: 314
		private struct OrderBlock
		{
			// Token: 0x040003FD RID: 1021
			internal int order;

			// Token: 0x040003FE RID: 1022
			internal UnityAction callback;
		}
	}
}
