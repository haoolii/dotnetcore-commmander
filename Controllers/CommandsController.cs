using System.Collections.Generic;
using Commander.Data;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{

  //api/commands
  [Route("api/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase
  {
    private readonly ICommanderRepo _repository;

    public CommandsController(ICommanderRepo repository)
    {
      _repository = repository;
    }
    // private readonly MockCommanderRepo _repository = new MockCommanderRepo();
    // Get api/commads
    [HttpGet]
    public ActionResult<IEnumerable<Command>> GetAllCommends()
    {
      var commendItems = _repository.GetAllCommands();
      return Ok(commendItems);
    }

    // Get api/commands/{id}
    [HttpGet("{id}")]
    public ActionResult<Command> GetCommandById(int id)
    {
      var commendItem = _repository.GetCommandById(id);
      return Ok(commendItem);
    }
  }
}