using System;
using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers {

  //api/commands
  [Route ("api/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase {
    private readonly ICommanderRepo _repository;
    private readonly IMapper _mapper;

    public CommandsController (
      ICommanderRepo repository,
      IMapper mapper
    ) {
      _repository = repository;
      _mapper = mapper;
    }
    // Get api/commads
    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllCommends () {
      var commendItems = _repository.GetAllCommands ();
      return Ok (_mapper.Map<IEnumerable<CommandReadDto>> (commendItems));
    }

    // Get api/commands/{id}
    [HttpGet ("{id}", Name = "GetCommandById")]
    public ActionResult<CommandReadDto> GetCommandById (int id) {
      var commendItem = _repository.GetCommandById (id);
      if (commendItem != null) {
        return Ok (_mapper.Map<CommandReadDto> (commendItem));
      } else {
        return NotFound ();
      }
    }

    // Post api/commands
    public ActionResult<CommandReadDto> CreateCommand (CommandCreateDto commandCreateDto) {
      var commandModel = _mapper.Map<Command> (commandCreateDto);
      _repository.CreateCommand (commandModel);
      _repository.SaveChanges ();
      var commandReadDto = _mapper.Map<CommandReadDto> (commandModel);

      // can see microsoft document about CreatedAtRoute
      // Postman會回傳這Command的取得路由而且是201 status code.
      return CreatedAtRoute (nameof (GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
      // return Ok (commandReadDto);
    }
  }
}