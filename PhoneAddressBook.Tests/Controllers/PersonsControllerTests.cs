using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneAddressBook.API.DTOs;
using PhoneAddressBook.API.Exceptions;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Domain.Entities;
using PhoneAddressBook.Domain.Enums;
using PhoneAddressBook.Controllers;

namespace PhoneAddressBook.Tests.Repositories;

[TestFixture]
public class PersonsControllerTests
{
    private Mock<IPersonService> _mockPersonService;
    private Mock<IMapper> _mockMapper;
    private PersonsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockPersonService = new Mock<IPersonService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new PersonsController(_mockPersonService.Object, _mockMapper.Object);
    }

    #region GetAll Tests

    [Test]
    public async Task GetAll_ShouldReturnOkWithPaginatedResponse_WhenDataExists()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var filter = "John";

        var persons = new List<Person>
        {
            new Person { Id = 1, FullName = "John Doe" },
            new Person { Id = 2, FullName = "Jane Smith" }
        };

        var totalCount = 2;
        var totalPages = 1;

        var personDtos = new List<PersonDto>
        {
            new PersonDto { Id = 1, Name = "John Doe", Addresses = new List<AddressDto>() },
            new PersonDto { Id = 2, Name = "Jane Smith", Addresses = new List<AddressDto>() }
        };

        _mockPersonService.Setup(s => s.GetAllAsync(pageNumber, pageSize, filter))
            .ReturnsAsync((persons, totalCount));

        _mockMapper.Setup(m => m.Map<List<PersonDto>>(persons))
            .Returns(personDtos);

        // Act
        var result = await _controller.GetAll(pageNumber, pageSize, filter);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;

        var response = okResult?.Value as PaginatedResponseDto<PersonDto>;
        response.Should().NotBeNull();
        response?.PageNumber.Should().Be(pageNumber);
        response?.PageSize.Should().Be(pageSize);
        response?.TotalCount.Should().Be(totalCount);
        response?.TotalPages.Should().Be(totalPages);
        response?.People.Should().BeEquivalentTo(personDtos);
    }

    [Test]
    public async Task GetAll_ShouldReturnOkWithEmptyList_WhenNoDataExists()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        string filter = null;

        var persons = new List<Person>();
        var totalCount = 0;
        var totalPages = 0;
        var personDtos = new List<PersonDto>();

        _mockPersonService.Setup(s => s.GetAllAsync(pageNumber, pageSize, filter))
            .ReturnsAsync((persons, totalCount));

        _mockMapper.Setup(m => m.Map<List<PersonDto>>(persons))
            .Returns(personDtos);

        // Act
        var result = await _controller.GetAll(pageNumber, pageSize, filter);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;

        var response = okResult?.Value as PaginatedResponseDto<PersonDto>;
        response.Should().NotBeNull();
        response?.PageNumber.Should().Be(pageNumber);
        response?.PageSize.Should().Be(pageSize);
        response?.TotalCount.Should().Be(totalCount);
        response?.TotalPages.Should().Be(totalPages);
        response?.People.Should().BeEmpty();
    }

    #endregion

    #region GetById Tests

    [Test]
    public async Task GetById_ShouldReturnOkWithPersonDto_WhenPersonExists()
    {
        // Arrange
        var personId = 1;
        var person = new Person { Id = personId, FullName = "John Doe", Addresses = new List<Address>() };
        var personDto = new PersonDto { Id = personId, Name = "John Doe", Addresses = new List<AddressDto>() };

        _mockPersonService.Setup(s => s.GetByIdAsync(personId))
            .ReturnsAsync(person);

        _mockMapper.Setup(m => m.Map<PersonDto>(person))
            .Returns(personDto);

        // Act
        var result = await _controller.GetById(personId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;

        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(personDto);
    }

    [Test]
    public async Task GetById_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var personId = 99;

        _mockPersonService.Setup(s => s.GetByIdAsync(personId))
            .ThrowsAsync(new NotFoundException($"Person with ID {personId} not found."));

        // Act
        var act = async () => { await _controller.GetById(personId); };

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Person with ID {personId} not found.");
    }

    #endregion

    #region Create Tests

    [Test]
    public async Task Create_ShouldReturnCreatedAtAction_WithCreatedPersonDto_WhenCreationIsSuccessful()
    {
        // Arrange
        var createPersonDto = new CreatePersonDto
        {
            FullName = "Alice Johnson",
            Addresses = new List<CreateAddressDto>
            {
                new CreateAddressDto
                {
                    Type = 1,
                    AddressDetail = "789 Maple Ave",
                    PhoneNumbers = new List<CreatePhoneNumberDto>
                    {
                        new CreatePhoneNumberDto { Number = "555-4321" }
                    }
                }
            }
        };

        var person = new Person
        {
            Id = 1,
            FullName = "Alice Johnson",
            Addresses = new List<Address>
            {
                new Address
                {
                    Id = 1,
                    Type = AddressType.Home,
                    AddressDetail = "789 Maple Ave",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Id = 1, Number = "555-4321" }
                    }
                }
            }
        };

        var personDto = new PersonDto
        {
            Id = 1,
            Name = "Alice Johnson",
            Addresses = new List<AddressDto>
            {
                new AddressDto
                {
                    Type = 1,
                    Address = "789 Maple Ave",
                    PhoneNumbers = new List<string>
                    {
                        "555-4321"
                    }
                }
            }
        };

        _mockMapper.Setup(m => m.Map<Person>(createPersonDto))
            .Returns(person);

        _mockPersonService.Setup(s => s.AddAsync(person))
            .ReturnsAsync(person);

        _mockMapper.Setup(m => m.Map<PersonDto>(person))
            .Returns(personDto);

        // Act
        var result = await _controller.Create(createPersonDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var okResult = result.Result as CreatedAtActionResult;
        okResult?.StatusCode.Should().Be(201);
        okResult?.Value.Should().BeEquivalentTo(personDto);
    }

    [Test]
    public void Create_ShouldThrowBadRequestException_WhenModelStateIsInvalid()
    {
        // Arrange
        var createPersonDto = new CreatePersonDto(); 
        _controller.ModelState.AddModelError("FullName", "The FullName field is required.");

        // Act
        var act = async () => { await _controller.Create(createPersonDto); };

        // Assert
        act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("The FullName field is required.");
    }

    [Test]
    public async Task Create_ShouldThrowException_WhenServiceThrowsException()
    {
        // Arrange
        var createPersonDto = new CreatePersonDto
        {
            FullName = "Bob Williams",
            Addresses = new List<CreateAddressDto>()
        };

        var person = new Person { FullName = "Bob Williams", Addresses = new List<Address>() };

        _mockMapper.Setup(m => m.Map<Person>(createPersonDto))
            .Returns(person);

        _mockPersonService.Setup(s => s.AddAsync(person))
            .ThrowsAsync(new Exception("Database error."));

        // Act
        var act = async () => { await _controller.Create(createPersonDto); };

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error.");
    }

    #endregion

    #region UpdateAddresses Tests

    [Test]
    public async Task UpdateAddresses_ShouldReturnOkWithUpdatedPersonDto_WhenUpdateIsSuccessful()
    {
        // Arrange
        var personId = 1;
        var updatePersonDto = new UpdatePersonDto
        {
            Addresses = new List<UpdateAddressDto>
            {
                new UpdateAddressDto
                {
                    Type = 1,
                    AddressDetail = "321 Oak St",
                    PhoneNumbers = new List<UpdatePhoneNumberDto>
                    {
                        new UpdatePhoneNumberDto { Number = "555-1111" }
                    }
                }
            }
        };

        var updatedPerson = new Person
        {
            Id = personId,
            FullName = "John Doe",
            Addresses = new List<Address>
            {
                new Address
                {
                    Id = 1,
                    Type = AddressType.Home,
                    AddressDetail = "321 Oak St",
                    PhoneNumbers = new List<Domain.Entities.PhoneNumber>
                    {
                        new PhoneNumber { Id = 1, Number = "555-1111" },
                        new PhoneNumber { Id = 2, Number = "555-2222" } 
                    }
                },
                new Address
                {
                    Id = 2,
                    Type = AddressType.Business,
                    AddressDetail = "654 Pine St",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Id = 3, Number = "555-3333" }
                    }
                }
            }
        };

        var personDto = new PersonDto
        {
            Id = personId,
            Name = "John Doe",
            Addresses = new List<AddressDto>
            {
                new AddressDto
                {
                    Type = 1,
                    Address = "321 Oak St",
                    PhoneNumbers = new List<string>
                    {
                        "555-1111",
                        "555-2222" 
                    }
                },
                new AddressDto
                {
                    Type = 2,
                    Address = "654 Pine St",
                    PhoneNumbers = new List<string>
                    {
                        "555-3333"
                    }
                }
            }
        };

        _mockPersonService.Setup(s => s.UpdateAddressesAsync(personId, updatePersonDto.Addresses))
            .Returns(Task.CompletedTask);

        _mockPersonService.Setup(s => s.GetByIdAsync(personId))
            .ReturnsAsync(updatedPerson);

        _mockMapper.Setup(m => m.Map<PersonDto>(updatedPerson))
            .Returns(personDto);

        // Act
        var result = await _controller.UpdateAddresses(personId, updatePersonDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;

        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(personDto);
    }

    [Test]
    public void UpdateAddresses_ShouldThrowBadRequestException_WhenModelStateIsInvalid()
    {
        // Arrange
        var personId = 1;
        var updatePersonDto = new UpdatePersonDto(); 
        _controller.ModelState.AddModelError("Addresses", "At least one address is required.");

        // Act
        var act = async () => { await _controller.UpdateAddresses(personId, updatePersonDto); };

        // Assert
        act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("At least one address is required.");
    }

    [Test]
    public void UpdateAddresses_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
    {
        // Arrange
        var personId = 99;
        var updatePersonDto = new UpdatePersonDto
        {
            Addresses = new List<UpdateAddressDto>()
        };

        _mockPersonService.Setup(s => s.UpdateAddressesAsync(personId, updatePersonDto.Addresses))
            .ThrowsAsync(new NotFoundException($"Person with id: {personId} not found."));

        // Act
        var act = async () => { await _controller.UpdateAddresses(personId, updatePersonDto); };

        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Person with id: {personId} not found.");
    }

    [Test]
    public void UpdateAddresses_ShouldThrowException_WhenServiceThrowsException()
    {
        // Arrange
        var personId = 1;
        var updatePersonDto = new UpdatePersonDto
        {
            Addresses = new List<UpdateAddressDto>()
        };

        _mockPersonService.Setup(s => s.UpdateAddressesAsync(personId, updatePersonDto.Addresses))
            .ThrowsAsync(new Exception("Database error."));

        // Act
        var act = async () => { await _controller.UpdateAddresses(personId, updatePersonDto); };

        // Assert
        act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error.");
    }

    #endregion

    #region Delete Tests

    [Test]
    public async Task Delete_ShouldReturnOkWithMessage_WhenDeletionIsSuccessful()
    {
        // Arrange
        var personId = 1;
        _mockPersonService.Setup(s => s.DeleteAsync(personId))
            .Returns(Task.CompletedTask);

        var expectedMessage = $"Deleted person with id:{personId}";

        // Act
        var result = await _controller.Delete(personId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().Be(expectedMessage);
    }

    [Test]
    public void Delete_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
    {
        // Arrange
        var personId = 99;

        _mockPersonService.Setup(s => s.DeleteAsync(personId))
            .ThrowsAsync(new NotFoundException($"Person with id: {personId} not found."));

        // Act
        var act = async () => { await _controller.Delete(personId); };

        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Person with id: {personId} not found.");
    }

    [Test]
    public void Delete_ShouldThrowException_WhenServiceThrowsException()
    {
        // Arrange
        var personId = 1;

        _mockPersonService.Setup(s => s.DeleteAsync(personId))
            .ThrowsAsync(new Exception("Database error."));

        // Act
        var act = async () => { await _controller.Delete(personId); };

        // Assert
        act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error.");
    }

    #endregion
}