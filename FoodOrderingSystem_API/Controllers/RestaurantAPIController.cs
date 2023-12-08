using AutoMapper;
using Azure;
using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.DTOs.RestaurantDTO;
using FoodOrderingSystem_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FoodOrderingSystem_API.Controllers;

[Route("api/RestaurantAPI")]
[ApiController]
public class RestaurantAPIController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepo;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public RestaurantAPIController(IRestaurantRepository restaurantRepository, IMapper mapper)
    {
        this._restaurantRepo = restaurantRepository;
        this._mapper = mapper;
        this._response = new APIResponse();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetAllRestaurants()
    {
        try
        {
            IEnumerable<Restaurant> restaurants = await _restaurantRepo.GetAllAsync(tracked: false);
            var restaurantsDTO = _mapper.Map<List<RestaurantDTO>>(restaurants);

            _response.Result = restaurantsDTO;
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

    [HttpGet("{id:int}", Name = "GetRestaurant")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public async Task<ActionResult<APIResponse>> GetRestaurant(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.NoContent;
                return BadRequest();
            }

           //var restaurant = await _restaurantRepo.GetAsync(restaurant => restaurant.Id == id, tracked: false);
           var restaurant = await _restaurantRepo.GetRestaurant(restaurant => restaurant.Id == id, tracked: false);

            if (restaurant == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }

            var restaurantDTO = _mapper.Map<RestaurantDTO>(restaurant);

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = restaurantDTO;

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

    public async Task<ActionResult<APIResponse>> CreateRestaurant([FromBody] RestaurantCreateDTO createDTO)
    {
        try
        {
            if (createDTO == null)
            {
                return BadRequest();
            }

            if (await _restaurantRepo.GetAsync(restaurant => restaurant.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error", "Restaurant already exists!"); 
                return BadRequest(ModelState);
            }

            var restaurant = _mapper.Map<Restaurant>(createDTO);

            await _restaurantRepo.CreateAsync(restaurant);
            
            var restaurantDTO = _mapper.Map<RestaurantCreateDTO>(restaurant);

            _response.Result = restaurantDTO;
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetRestaurant", new { id = restaurant.Id }, _response);
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
    public async Task<ActionResult<APIResponse>> DeleteRestaurant(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var restaurant = await _restaurantRepo.GetAsync(restaurant => restaurant.Id == id, tracked: false);

            if (restaurant == null)
            {
                return NotFound();
            }

            await _restaurantRepo.RemoveAsync(restaurant);
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
    public async Task<ActionResult<APIResponse>> UpdateRestaurant([FromBody] RestaurantUpdateDTO updateDTO, int id)
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

            var restaurant = _mapper.Map<Restaurant>(updateDTO);
            await _restaurantRepo.UpdateAsync(restaurant);
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
    public async Task<ActionResult<APIResponse>> UpdatePartialRestaurant(int id, JsonPatchDocument<RestaurantUpdateDTO> patchDTO)
    {
        try
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var restaurant = await _restaurantRepo.GetAsync(restaurant => restaurant.Id == id, tracked: false);

            if (restaurant == null)
            {
                return NotFound();
            }

            var restaurantUpdateDTO = _mapper.Map<RestaurantUpdateDTO>(restaurant);

            patchDTO.ApplyTo(restaurantUpdateDTO, ModelState);

            var model = _mapper.Map<Restaurant>(restaurantUpdateDTO);

            await _restaurantRepo.UpdateAsync(model);

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