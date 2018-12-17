using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcAngular;
namespace Mooc.Models
{
    [AngularType]
    public class TestResponse
    {
        public string Name { get; set; }
        public TestModel2 model2 { get; set; }
    }
}
