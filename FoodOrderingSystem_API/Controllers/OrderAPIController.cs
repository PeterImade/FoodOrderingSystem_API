using AutoMapper;
using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.DTOs.Customer;
using FoodOrderingSystem_API.DTOs.Order;
using FoodOrderingSystem_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FoodOrderingSystem_API.Controllers;

[Route("api/OrderAPI")]
[ApiController]
public class OrderAPIController : ControllerBase
{ 
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public OrderAPIController(IOrderRepository orderRepository, IMapper mapper)
        {
            this._orderRepo = orderRepository;
            this._mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllOrders()
        {
            try
            {
                IEnumerable<Order> orders = await _orderRepo.GetAllAsync(tracked: false);
                var ordersDTO = _mapper.Map<List<OrderDTO>>(orders);

                _response.Result = ordersDTO;
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

        [HttpGet("{id:int}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return BadRequest();
                }

                var order = await _orderRepo.GetAnOrder(order => order.Id == id, tracked: false);

                if (order == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                var orderDTO = _mapper.Map<OrderDTO>(order);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = orderDTO;

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

        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] OrderCreateDTO createDTO)
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

                var order = _mapper.Map<Order>(createDTO);

                await _orderRepo.CreateAsync(order);

                var orderDTO = _mapper.Map<OrderCreateDTO>(order);

                _response.Result = orderDTO;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetOrder", new { id = order.Id }, _response);
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
        public async Task<ActionResult<APIResponse>> DeleteOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var order = await _orderRepo.GetAsync(order => order.Id == id, tracked: false);

                if (order == null)
                {
                    return NotFound();
                }

                await _orderRepo.RemoveAsync(order);
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
        public async Task<ActionResult<APIResponse>> UpdateOrder([FromBody] OrderUpdateDTO updateDTO, int id)
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

                var order = _mapper.Map<Order>(updateDTO);
                await _orderRepo.UpdateAsync(order);
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
        public async Task<ActionResult<APIResponse>> UpdatePartialOrder(int id, JsonPatchDocument<OrderUpdateDTO> patchDTO)
        {
            try
            {
                if (id == 0 || patchDTO == null)
                {
                    return BadRequest();
                }
                var order = await _orderRepo.GetAsync(order => order.Id == id, tracked: false);

                if (order == null)
                {
                    return NotFound();
                }

                var orderUpdateDTO = _mapper.Map<OrderUpdateDTO>(order);

                patchDTO.ApplyTo(orderUpdateDTO, ModelState);

                var model = _mapper.Map<Order>(orderUpdateDTO);

                await _orderRepo.UpdateAsync(model);

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