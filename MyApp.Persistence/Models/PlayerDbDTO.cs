using MyApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Persistence.Models;
public class PlayerDbDTO
{
    [Key] public int PlayerNameId { get; set; }
    public string Name { get; set; }

    public PlayerDbDTO()
    {
        //added for database-purposes
    }

    public PlayerDbDTO(string name)
        {
            this.Name = name;
        }
}
