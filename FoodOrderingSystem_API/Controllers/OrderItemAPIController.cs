using AutoMapper;
using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.DTOs.Customer;
using FoodOrderingSystem_API.DTOs.Order;
using FoodOrderingSystem_API.DTOs.OrderItem;
using FoodOrderingSystem_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FoodOrderingSystem_API.Controllers;

[Route("api/OrderItemAPI")]
[ApiController]
public class OrderItemAPIController : ControllerBase
{ 
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public OrderItemAPIController(IOrderItemRepository orderItemRepo, IMapper mapper)
        {
            this._orderItemRepo = orderItemRepo;
            this._mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllOrderItems()
        {
            try
            {
                IEnumerable<OrderItem> orderItems = await _orderItemRepo.GetAllAsync(tracked: false);
                var orderItemsDTO = _mapper.Map<List<OrderItemDTO>>(orderItems);

                _response.Result = orderItemsDTO;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetOrderItem")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetOrderItem(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return BadRequest();
                }

                var orderItem = await _orderItemRepo.GetAsync(orderItem => orderItem.Id == id, tracked: false);

                if (orderItem == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                var orderItemDTO = _mapper.Map<OrderItemDTO>(orderItem);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = orderItemDTO;

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<ActionResult<APIResponse>> CreateOrderItem([FromBody] OrderItemCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest();
                }

                //if (await _orderRepo.GetAsync(order => customer.FirstName.ToLower() == createDTO.FirstName.ToLower() && customer.LastName.ToLower() == createDTO.LastName.ToLower()) != null)
                //{
                //    ModelState.AddModelError("Custom Error", "Customer already exists!");
                //    return BadRequest(ModelState);
                //}

                var orderItem = _mapper.Map<OrderItem>(createDTO);

                await _orderItemRepo.CreateAsync(orderItem);

                var orderItemDTO = _mapper.Map<OrderItemCreateDTO>(orderItem);

                _response.Result = orderItemDTO;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetOrderItem", new { id = orderItem.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> DeleteOrderItem(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var orderItem = await _orderItemRepo.GetAsync(orderItem => orderItem.Id == id, tracked: false);

                if (orderItem == null)
                {
                    return NotFound();
                }

                await _orderItemRepo.RemoveAsync(orderItem);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateOrderItem([FromBody] OrderItemUpdateDTO updateDTO, int id)
        {
            try
            {
                if (id == 0 || updateDTO == null)
                {
                    return BadRequest();
                }

                if (updateDTO.Id != id)
                {
                    return BadRequest();
                }

                var orderItem = _mapper.Map<OrderItem>(updateDTO);
                await _orderItemRepo.UpdateAsync(orderItem);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> UpdatePartialOrderItem(int id, JsonPatchDocument<OrderItemUpdateDTO> patchDTO)
        {
            try
            {
                if (id == 0 || patchDTO == null)
                {
                    return BadRequest();
                }
                var orderItem = await _orderItemRepo.GetAsync(orderItem => orderItem.Id == id, tracked: false);

                if (orderItem == null)
                {
                    return NotFound();
                }

                var orderItemUpdateDTO = _mapper.Map<OrderItemUpdateDTO>(orderItem);

                patchDTO.ApplyTo(orderItemUpdateDTO, ModelState);

                var model = _mapper.Map<OrderItem>(orderItemUpdateDTO);

                await _orderItemRepo.UpdateAsync(model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        } 
}