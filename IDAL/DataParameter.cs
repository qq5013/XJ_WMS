using System;
using System.Collections.Generic;

using System.Text;

namespace IDAL
{
    [Serializable()]
    public class DataParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        public DataParameter() { }

        /// <summary>
        /// 用于与SQL语句匹配的参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        public DataParameter(string parameterName, object value)
        {
            ParameterName = parameterName;
            Value = value;
        }
    }
}
