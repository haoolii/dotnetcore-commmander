using System;
using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
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
    [HttpPost]
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

    //PUT api/commands/{id}
    [HttpPut ("{id}")]
    public ActionResult UpdateCommand (int id, CommandUpdateDto commandUpdateDto) {
      var commandModelFromRepo = _repository.GetCommandById (id);
      if (commandModelFromRepo == null) {
        return NotFound ();
      }
      _mapper.Map (commandUpdateDto, commandModelFromRepo);
      _repository.UpdateCommand (commandModelFromRepo);
      _repository.SaveChanges ();

      return NoContent ();
    }

    // PATCH api/commands/{id}
    [HttpPatch ("{id}")]
    public ActionResult PartialCommandUpdate (int id, JsonPatchDocument<CommandUpdateDto> patchDoc) {
      // 取得commandModel
      var commandModelFromRepo = _repository.GetCommandById (id);
      // 如果不存在要回NotFound
      if (commandModelFromRepo == null) {
        return NotFound ();
      }

      // 目標model轉成dto
      var commandToPatch = _mapper.Map<CommandUpdateDto> (commandModelFromRepo);

      // 傳進來的pathdoc蓋上去commandToPathch
      patchDoc.ApplyTo (commandToPatch, ModelState);

      // 驗證一下model
      if (!TryValidateModel (commandToPatch)) {
        return ValidationProblem (ModelState);
      }

      // 把path完成的Dto蓋上Model達到更新
      _mapper.Map (commandToPatch, commandModelFromRepo);

      // 沒效
      _repository.UpdateCommand (commandModelFromRepo);

      _repository.SaveChanges ();

      return NoContent ();

    }
  }
}