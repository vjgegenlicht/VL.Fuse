﻿using System.Collections.Generic;
using Stride.Graphics;

namespace Fuse
{
    public class SetBufferNode<TIn> : ShaderNode<Void> where TIn : struct
    {
        private GpuValue<Buffer> _buffer;
        private GpuValue<int> _index;
        private GpuValue<TIn> _value;
        
        public SetBufferNode(GpuValue<Buffer> theBuffer, GpuValue<int> theIndex, GpuValue<TIn> theValue) : base( "setBuffer")
        {
            _buffer = theBuffer;
            _index = theIndex;
            _value = theValue;
            
            Setup(new List<AbstractGpuValue>(){theBuffer,theIndex,theValue});
        }
        
        protected override Dictionary<string, string> CreateTemplateMap()
        {
            return new Dictionary<string, string>();
        }

        protected override string SourceTemplate()
        {
            const string shaderCode = "${bufferName}[${index}] = ${value};";
            return ShaderTemplateEvaluator.Evaluate(shaderCode,new Dictionary<string, string>()
            {
                {"bufferName", _buffer.ID},
                {"index", _index.ID},
                {"value", _value.ID}
            });
        }
    }
}