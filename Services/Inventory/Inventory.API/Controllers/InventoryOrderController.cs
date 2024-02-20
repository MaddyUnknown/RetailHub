using Inventory.API.Response;
using Inventory.Application.Exception;
using Inventory.Application.Model.DTO.InventoryOrder;
using Inventory.Application.Model.Request;
using Inventory.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("/api/inventory")]
    public class InventoryOrderController : ControllerBase
    {
        private IInventoryOrderService _inventoryOrderService;

        public InventoryOrderController(IInventoryOrderService inventoryOrderService)
        {
            _inventoryOrderService = inventoryOrderService ?? throw new ArgumentNullException(nameof(inventoryOrderService));
        }

        [HttpPut("/create")]
        public async Task<IActionResult> CreateOrderWithItems([FromBody] CreateInventoryOrderWithItems requestObj)
        {
            try
            {
                var response = await _inventoryOrderService.CreateInventoryOrder(requestObj);
                return Ok(ResponseWrapper<GetInventoryOrderWithItemsDTO>.CreateResponse(true, "Operation successful", response));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ResponseWrapper<List<string>>.CreateResponse(false, "Input validation failed", ex.ErrorList));
            }
            catch(ServiceException ex)
            {
                return StatusCode(500, ResponseWrapper<string>.CreateResponse(false, ex.Message, null));
            }
            catch(System.Exception)
            {
                return StatusCode(500, ResponseWrapper<string>.CreateResponse(false, "Error occured processing request", null));
            }
        }

        [HttpPost("/update")]
        public async Task<IActionResult> UpdateOrderWithItems([FromBody] UpdateInventoryOrderWithItems requestObj)
        {
            try
            {
                var response = await _inventoryOrderService.UpdateInventoryOrder(requestObj);
                return Ok(ResponseWrapper<GetInventoryOrderWithItemsDTO>.CreateResponse(true, "Operation successful", response));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ResponseWrapper<List<string>>.CreateResponse(false, "Input validation failed", ex.ErrorList));
            }
            catch (ServiceException ex)
            {
                return StatusCode(500, ResponseWrapper<string>.CreateResponse(false, ex.Message, null));
            }
            catch (System.Exception)
            {
                return StatusCode(500, ResponseWrapper<string>.CreateResponse(false, "Error occured processing request", null));
            }
        }

    }
}
