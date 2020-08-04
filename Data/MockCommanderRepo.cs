using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{

  public class MockCommanderRepo : ICommanderRepo
  {
    public IEnumerable<Command> GetAllCommands()
    {
      var commands = new List<Command> {
        new Command { Id = 0, HowTo = "Boild an egg", Line = "Boil water", Platform = "Pan" },
        new Command { Id = 1, HowTo = "Cut Breat", Line = "Get a knife", Platform = "knife & chopping board" },
        new Command { Id = 2, HowTo = "Make cup of tea", Line = "Place teabag in cup", Platform = "cup" }
      };

      return commands;
    }

    public Command GetCommandById(int id)
    {
      return new Command { Id = 0, HowTo = "Boild an egg", Line = "Boil water", Platform = "Kettle" };
    }
  }
}