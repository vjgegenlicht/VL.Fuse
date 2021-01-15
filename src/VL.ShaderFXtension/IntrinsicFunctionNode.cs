﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stride.Core.Extensions;
using VL.Stride.Shaders.ShaderFX;

namespace VL.ShaderFXtension
{
    public class IntrinsicFunctionNode<T> : ShaderNode<T>
    {
        
        public IntrinsicFunctionNode(OrderedDictionary<string,AbstractGpuValue> inputs, string theFunction, ConstantValue<T> theDefault) : base(theFunction, theDefault)
        {
            const string shaderCode = "${resultType} ${resultName} = ${function}(${arguments});";
            var valueMap = new Dictionary<string, string>
            {
                {"function", theFunction},
                {"arguments", ShaderNodesUtil.BuildArguments(inputs)}
            };
            Setup(shaderCode, inputs, valueMap);
        }
        
        
    }
}