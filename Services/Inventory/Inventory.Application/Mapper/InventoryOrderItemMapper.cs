using Inventory.Application.Model.DTO.InventoryOrderItem;
using Inventory.Application.Model.Request;
using Inventory.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Mapper
{
    public static class InventoryOrderItemMapper
    {
        public static InventoryOrderItem CreateInventoryOrderItem(CreateInventoryOrderItemForOrder obj, long inventoryOrderId)
        {
            InventoryOrderItem item = new InventoryOrderItem();

            item.InventoryOrderId = inventoryOrderId;
            item.ProductId = obj.ProductId;
            item.Quantity = obj.Quantity;
            item.UnitPrice = obj.UnitPrice;

            return item;
        }

        public static InventoryOrderItem CreateInventoryOrderItem(UpdateInventoryOrderItemForOrder obj, long inventoryOrderId)
        {
            InventoryOrderItem item = new InventoryOrderItem();

            item.InventoryOrderId = inventoryOrderId;
            item.ProductId = obj.ProductId;
            item.Quantity = obj.Quantity;
            item.UnitPrice = obj.UnitPrice;

            return item;
        }

        public static InventoryOrderItem UpdateInventoryOrderItem(InventoryOrderItem item, UpdateInventoryOrderItemForOrder obj, long inventoryOrderId)
        {

            item.InventoryOrderId = inventoryOrderId;
            item.ProductId = obj.ProductId;
            item.Quantity = obj.Quantity;
            item.UnitPrice = obj.UnitPrice;

            return item;
        }

        public static GetInventoryOrderItemForOrderDTO GetInventoryOrderItemForOrderDTO(InventoryOrderItem orderItem)
        {
            GetInventoryOrderItemForOrderDTO orderItemDTO = new();
            orderItemDTO.Id = orderItem.Id;
            orderItemDTO.ProductId = orderItem.ProductId;
            orderItemDTO.UnitPrice = orderItem.UnitPrice;
            orderItemDTO.Quantity = orderItem.Quantity;

            return orderItemDTO;
        }
    }
}
