using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Util
{
    public class Parameter
    {
        public ParameterDirection ParameterDirectioin = ParameterDirection.Input;
        public string ParameterName;
        public DbType ParameterType = DbType.String;
        public object ParameterValue;
        public int Scale;
        public int Size;
    }
}
