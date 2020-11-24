using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Infrastructure.Exceptions
{
    public class DbException:Exception
    {
        public DbException(string msg) : base(msg)
        {

        }
    }
}
