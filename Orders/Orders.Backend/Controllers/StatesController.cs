using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class StatesController : GenericController<State>
    {
        private readonly IStatesUnifOfWork _statesUnitOfWork;
        public StatesController(IGenericUnitOfWork<State> unitOfWork, IStatesUnifOfWork statesUnitOfWork) : base(unitOfWork)
        {
            _statesUnitOfWork = statesUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _statesUnitOfWork.GetAsync();
            if (response.WasSuccesse)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _statesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccesse)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _statesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccesse)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }


        [HttpGet("{id:int}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _statesUnitOfWork.GetAsync(id);
            if (response.WasSuccesse)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}
