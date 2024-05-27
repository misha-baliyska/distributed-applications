using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging
{
    public class IntegerRequestBase: ServiceRequestBase
    {
        public int Id { get; set; }
        public IntegerRequestBase(int id)
        {
            Id = id; 
        }
    }
}
