using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Util
{
    public class StoredProcParameter
    {
        private List<string> parameterList = new List<string>();
        private Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();
        private List<DbType> typeList = new List<DbType>();
        private List<string> valueList = new List<string>();

        public void AddParameter()
        {
            for (int i = 0; i < this.Values.Count; i++)
            {
                if (!this.parameters.ContainsKey(this.Names[i]))
                {
                    this.AddParameter(this.Names[i], this.Values[i], this.Types[i]);
                }
            }
        }

        public void AddParameter(string parameterName, object parameterValue)
        {
            Parameter parameter = new Parameter();
            parameter.ParameterName = parameterName;
            parameter.ParameterValue = parameterValue;
            this.parameters.Add(parameterName, parameter);
        }

        public void AddParameter(string parameterName, object parameterValue, DbType parameterType)
        {
            Parameter parameter = new Parameter();
            parameter.ParameterName = parameterName;
            parameter.ParameterValue = parameterValue;
            parameter.ParameterType = parameterType;
            this.parameters.Add(parameterName, parameter);
        }

        public void AddParameter(string parameterName, object parameterValue, DbType parameterType, ParameterDirection direction)
        {
            Parameter parameter = new Parameter();
            parameter.ParameterName = parameterName;
            parameter.ParameterValue = parameterValue;
            parameter.ParameterType = parameterType;
            parameter.ParameterDirectioin = direction;
            this.parameters.Add(parameterName, parameter);
        }

        public void AddParameter(string parameterName, object parameterValue, DbType parameterType, int parameterSize, ParameterDirection direction)
        {
            Parameter parameter = new Parameter();
            parameter.ParameterName = parameterName;
            parameter.ParameterValue = parameterValue;
            parameter.ParameterType = parameterType;
            parameter.Size = parameterSize;
            parameter.ParameterDirectioin = direction;
            this.parameters.Add(parameterName, parameter);
        }

        public void AddParameter(string parameterName, object parameterValue, DbType parameterType, int parameterSize, int parameterScale, ParameterDirection direction)
        {
            Parameter parameter = new Parameter();
            parameter.ParameterName = parameterName;
            parameter.ParameterValue = parameterValue;
            parameter.ParameterType = parameterType;
            parameter.Size = parameterSize;
            parameter.Scale = parameterScale;
            parameter.ParameterDirectioin = direction;
            this.parameters.Add(parameterName, parameter);
        }

        public object this[string parameterName]
        {
            get
            {
                return this.parameters[parameterName].ParameterValue;
            }
        }

        public List<string> Names
        {
            get
            {
                return this.parameterList;
            }
        }

        internal Dictionary<string, Parameter> Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public List<DbType> Types
        {
            get
            {
                return this.typeList;
            }
        }

        public List<string> Values
        {
            get
            {
                return this.valueList;
            }
        }
    }
}
