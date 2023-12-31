﻿using AutoMapper;
using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.DTOs.Menu;
using FoodOrderingSystem_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FoodOrderingSystem_API.Controllers
{
    [Route("api/MenuAPI")]
    [ApiController]
    public class MenuAPIController: ControllerBase
    {
        private readonly IMenuRepository _menuRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public MenuAPIController(IMenuRepository menuRepository, IMapper mapper)
        {
            this._menuRepo = menuRepository;
            this._mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllMenus()
        {
            try
            {
                IEnumerable<Menu> menus = await _menuRepo.GetAllAsync(tracked: false);
                var menusDTO = _mapper.Map<List<MenuDTO>>(menus);

                _response.Result = menusDTO;
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

        [HttpGet("{id:int}", Name = "GetMenu")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetMenu(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return BadRequest();
                }

                var menu = await _menuRepo.GetAsync(menu => menu.Id == id, tracked: false);

                if (menu == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                var menuDTO = _mapper.Map<MenuDTO>(menu);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = menuDTO;

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

        public async Task<ActionResult<APIResponse>> CreateMenu([FromBody] MenuCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest();
                }

                if (await _menuRepo.GetAsync(menu => menu.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Custom Error", "Menu already exists!");
                    return BadRequest(ModelState);
                }

                var menu = _mapper.Map<Menu>(createDTO);

                await _menuRepo.CreateAsync(menu);

                var menuDTO = _mapper.Map<MenuCreateDTO>(menu);

                _response.Result = menuDTO;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetMenu", new { id = menu.Id }, _response);
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
        public async Task<ActionResult<APIResponse>> DeleteMenu(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var menu = await _menuRepo.GetAsync(menu => menu.Id == id, tracked: false);

                if (menu == null)
                {
                    return NotFound();
                }

                await _menuRepo.RemoveAsync(menu);
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
        public async Task<ActionResult<APIResponse>> UpdateMenu([FromBody] MenuUpdateDTO updateDTO, int id)
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

                var menu = _mapper.Map<Menu>(updateDTO);
                await _menuRepo.UpdateAsync(menu);
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
        public async Task<ActionResult<APIResponse>> UpdatePartialMenu(int id, JsonPatchDocument<MenuUpdateDTO> patchDTO)
        {
            try
            {
                if (id == 0 || patchDTO == null)
                {
                    return BadRequest();
                }
                var menu = await _menuRepo.GetAsync(menu => menu.Id == id, tracked: false);

                if (menu == null)
                {
                    return NotFound();
                }

                var menuUpdateDTO = _mapper.Map<MenuUpdateDTO>(menu);

                patchDTO.ApplyTo(menuUpdateDTO, ModelState);

                var model = _mapper.Map<Menu>(menuUpdateDTO);

                await _menuRepo.UpdateAsync(model);

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
}