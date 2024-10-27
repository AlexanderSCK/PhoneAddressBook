using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneAddressBook.API.DTOs;
using PhoneAddressBook.API.Exceptions;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Domain.Entities;

namespace PhoneAddressBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public PersonsController(IPersonService personService, IMapper mapper)
    {
        _personService = personService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a paginated list of persons with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<PersonDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = null)
    {
        var (persons, totalCount) = await _personService.GetAllAsync(pageNumber, pageSize, filter);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var response = new PaginatedResponseDto<PersonDto>
        {
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalCount = totalCount,
            TotalPages = totalPages,
            People = _mapper.Map<List<PersonDto>>(persons)
        };

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a person by their unique identifier.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetById(int id)
    {
        var person = await _personService.GetByIdAsync(id);
        var personDto = _mapper.Map<PersonDto>(person);

        return Ok(personDto);
    }

    /// <summary>
    /// Creates a new person.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create(CreatePersonDto createPersonDto)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException(ModelState);
        }
        var person = _mapper.Map<Person>(createPersonDto);
        var addedPerson = await _personService.AddAsync(person);
        var personDto = _mapper.Map<PersonDto>(addedPerson);

        return CreatedAtAction(nameof(GetById), new { id = personDto.Id }, personDto);
    }

    /// <summary>
    /// Updates an existing person.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonDto>> UpdateAddresses(int id, UpdatePersonDto updatePersonDto)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException(ModelState);
        }

        await _personService.UpdateAddressesAsync(id, updatePersonDto.Addresses);

        var updatedPerson = await _personService.GetByIdAsync(id);
        var personDto = _mapper.Map<PersonDto>(updatedPerson);

        return Ok(personDto);
    }

    /// <summary>
    /// Deletes a person by their unique identifier.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _personService.DeleteAsync(id);
        return Ok($"Deleted person with id:{id}");
    }
}