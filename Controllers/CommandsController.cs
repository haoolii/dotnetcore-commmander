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
    [HttpGet ("{id}")]
    public ActionResult<CommandReadDto> GetCommandById (int id) {
      var commendItem = _repository.GetCommandById (id);
      if (commendItem != null) {
        return Ok (_mapper.Map<CommandReadDto> (commendItem));
      } else {
        return NotFound ();
      }
    }
  }
}