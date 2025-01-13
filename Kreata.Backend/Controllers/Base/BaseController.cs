using Kreata.Backend.Repos.Base;
using Kreta.Shared.Assemblers;
using Kreta.Shared.Dtos;
using Kreta.Shared.Models;
using Kreta.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kreata.Backend.Controllers.Base
{
    public abstract class BaseController<TModel,TDto> : ControllerBase
        where TModel : class,IDbEntity<TModel>,new()
        where TDto : class, new()
    {
        protected Assambler<TModel,TDto> _assambler;
        protected IBaseRepo<TModel> _repo;

        public BaseController(Assambler<TModel, TDto>? assambler, IBaseRepo<TModel>? repo)
        {
            _assambler = assambler ?? throw new ArgumentNullException(nameof(assambler));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entity = (await _repo.FindByConditionAsync(s => s.Id == id)).FirstOrDefault();
            if (entity != null)
                return Ok(_assambler.ToDto(entity));
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentAsync()
        {
            var students = (await _repo.GetAllAsync()).ToList();
            return Ok(students.Select(student => _assambler.ToDto(student)));
        }

        [HttpPut()]
        public async Task<ActionResult> UpdateStudentAsync(TDto studentDto)
        {
            Response response = response = await _repo.UpdateAsync(_assambler.ToModel(studentDto));
            if (response.HasError)
            {
                Console.WriteLine(response.Error);
                response.ClearAndAddError("Az entitás adatainak módosítása nem sikerült!");
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudendAsync(Guid id)
        {
            Response response = await _repo.DeleteAsync(id);
            if (response.HasError)
            {
                Console.WriteLine(response.Error);
                response.ClearAndAddError("A entitás adatainak törlése nem sikerült!");
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateStudentAsync(TDto studentDto)
        {
            Response response = await _repo.CreateAsync(_assambler.ToModel(studentDto));
            if (response.HasError)
            {
                Console.WriteLine(response.Error);
                response.ClearAndAddError("Új entitáts adatának felvétele nem sikerült!");
                return BadRequest(response);
            }
            else
                return Ok(response);
        }
    }
}
