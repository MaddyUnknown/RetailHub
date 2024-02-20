using Inventory.Application.Model.DTO.InventoryOrder;
using Inventory.Application.Model.Request;
using Inventory.Core.Entities;
using Inventory.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Mapper
{
    public static class InventoryOrderMapper
    {
        public static InventoryOrder CreateInventoryOrder(CreateInventoryOrderWithItems obj)
        {
            InventoryOrder order = new InventoryOrder();

            order.ExternalOrderReferenceNumber = obj.ExternalOrderReferenceNumber;
            order.VendorId = obj.VendorId;
            order.SubTotal = obj.Items!.Sum(x => x.UnitPrice * x.Quantity);
            order.Discount = obj.Discount;
            order.ShippingFee = obj.ShippingFee;
            order.TotalCost = order.SubTotal - order.Discount + order.ShippingFee;

            return order;
        }

        public static InventoryOrder UpdateInventoryOrder(InventoryOrder order, UpdateInventoryOrderWithItems obj)
        {
            order.ExternalOrderReferenceNumber = obj.ExternalOrderReferenceNumber;
            order.VendorId = obj.VendorId;
            order.SubTotal = obj.Items!.Sum(x => x.UnitPrice * x.Quantity);
            order.Discount = obj.Discount;
            order.ShippingFee = obj.ShippingFee;
            order.TotalCost = order.SubTotal - order.Discount + order.ShippingFee;

            return order;
        }

        public static GetInventoryOrderWithItemsDTO GetInventoryOrderWithItems(InventoryOrder order, IEnumerable<InventoryOrderItem> orderItems)
        {
            GetInventoryOrderWithItemsDTO orderDTO = new GetInventoryOrderWithItemsDTO();
            orderDTO.Id = order.Id;
            orderDTO.InventoryOrderNumber = order.InventoryOrderNumber;
            orderDTO.ExternalOrderReferenceNumber = order.ExternalOrderReferenceNumber;
            orderDTO.VendorId = order.VendorId;
            orderDTO.SubTotal = order.SubTotal;
            orderDTO.Discount = order.Discount;
            orderDTO.ShippingFee = order.ShippingFee;
            orderDTO.TotalCost = order.TotalCost;
            orderDTO.InventoryOrderStatus = order.InventoryOrderStatus;
            orderDTO.CreatedBy = order.CreatedBy;
            orderDTO.LastUpdatedBy = order.UpdatedBy;
            orderDTO.CreatedOn = order.CreationDate;
            orderDTO.LastUpdatedOn = order.UpdateDate;

            orderDTO.Items = orderItems.Select(x => InventoryOrderItemMapper.GetInventoryOrderItemForOrderDTO(x)).ToList();

            return orderDTO;
        }
    }
}
