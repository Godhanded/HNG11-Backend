using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage1.Models
{
    public record ResponseDto(string client_ip,string location,string greeting);
}