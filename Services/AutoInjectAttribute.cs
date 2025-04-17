using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoInjectAttribute : Attribute { }
}
