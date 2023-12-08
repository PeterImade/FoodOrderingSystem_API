using AutoMapper;
using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.DTOs.Customer;
using FoodOrderingSystem_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FoodOrderingSystem_API.Controllers;

[Route("api/CustomerAPI")]
[ApiController]
public class CustomerAPIController : ControllerBase
{ 
        private readonly ICustomerRepository _customerRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public CustomerAPIController(ICustomerRepository customerRepository, IMapper mapper)
        {
            this._customerRepo = customerRepository;
            this._mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllCustomers()
        {
            try
            {
                IEnumerable<Customer> customers = await _customerRepo.GetAllAsync(tracked: false);
                var customersDTO = _mapper.Map<List<CustomerDTO>>(customers);

                _response.Result = customersDTO;
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

        [HttpGet("{id:int}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetCustomer(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return BadRequest();
                }

                var customer = await _customerRepo.GetCustomer(customer => customer.Id == id, tracked: false);

                if (customer == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                var customerDTO = _mapper.Map<CustomerDTO>(customer);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = customerDTO;

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

        public async Task<ActionResult<APIResponse>> CreateCustomer([FromBody] CustomerCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest();
                }

                if (await _customerRepo.GetAsync(customer => customer.FirstName.ToLower() == createDTO.FirstName.ToLower() && customer.LastName.ToLower() == createDTO.LastName.ToLower()) != null)
                {
                    ModelState.AddModelError("Custom Error", "Customer already exists!");
                    return BadRequest(ModelState);
                }

                var customer = _mapper.Map<Customer>(createDTO);

                await _customerRepo.CreateAsync(customer);

                var customerDTO = _mapper.Map<CustomerCreateDTO>(customer);

                _response.Result = customerDTO;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCustomer", new { id = customer.Id }, _response);
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
        public async Task<ActionResult<APIResponse>> DeleteCustomer(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var customer = await _customerRepo.GetAsync(customer => customer.Id == id, tracked: false);

                if (customer == null)
                {
                    return NotFound();
                }

                await _customerRepo.RemoveAsync(customer);
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
        public async Task<ActionResult<APIResponse>> UpdateCustomer([FromBody] CustomerUpdateDTO updateDTO, int id)
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

                var customer = _mapper.Map<Customer>(updateDTO);
                await _customerRepo.UpdateAsync(customer);
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
        public async Task<ActionResult<APIResponse>> UpdatePartialCustomer(int id, JsonPatchDocument<CustomerUpdateDTO> patchDTO)
        {
            try
            {
                if (id == 0 || patchDTO == null)
                {
                    return BadRequest();
                }
                var customer = await _customerRepo.GetAsync(customer => customer.Id == id, tracked: false);

                if (customer == null)
                {
                    return NotFound();
                }

                var customerUpdateDTO = _mapper.Map<CustomerUpdateDTO>(customer);

                patchDTO.ApplyTo(customerUpdateDTO, ModelState);

                var model = _mapper.Map<Customer>(customerUpdateDTO);

                await _customerRepo.UpdateAsync(model);

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