using System;
using System.Collections.Generic;
using VL.Core;
using VL.Lib.Collections;

namespace Fuse.VL_API_Sketches.DelegateApproach
{
    public struct BorderControlPointDescription
    {
        public string Name;
        public IVLTypeInfo TypeInfo;
        public int Index;
    }

    public delegate void RegionPatchCreator(in Spread<object> inputs, out object stateOutput, out Spread<object> outputs);
    public delegate void RegionPatchUpdator(in object stateInput, in Spread<object> inputs, out object stateOutput, out Spread<object> outputs);
    //public delegate void RegionPatchDisposer(in object stateInput);

    /// <summary>
    /// Represents the application of your region by the user, the values that flow into the region and outof. 
    /// It also allows you to instanciate what's inside: the patch of the user. 
    /// </summary>
    public class CustomRegion
    {
        public Spread<BorderControlPointDescription> Inputs { get; }
        public Spread<BorderControlPointDescription> Outputs { get; }
        public RegionPatchCreator PatchCreator;
        public RegionPatchUpdator PatchUpdator;
        //public RegionPatchDisposer PatchDisposer;

        public Spread<object> ReadInputs()
        {
            return null;
        }

        public void WriteOutputs(IEnumerable<object> outputs)
        {
        }

        public void CreateRegionPatch(NodeContext nodeContext, in Spread<object> inputs, out CustomRegionPatch regionPatch, out Spread<object> outputs)
        {
            regionPatch = new CustomRegionPatch(nodeContext, this);
            outputs = null;
        }
    }

    /// <summary>
    /// Represents the body inside the region as patched by the user. 
    /// </summary>
    public struct CustomRegionPatch : IDisposable
    {
        object State;
        CustomRegion Region;
        NodeContext NodeContext;

        public CustomRegionPatch(object State) : this()
        {
            this.State = State;
        }

        public CustomRegionPatch(NodeContext NodeContext, CustomRegion CustomRegion) : this()
        {
            this.NodeContext = NodeContext;
            this.Region = CustomRegion;
        }

        public CustomRegionPatch Update(in Spread<object> inputs, out Spread<object> outputs)
        {
            Region.PatchUpdator(in State, in inputs, out var newState, out outputs);
            if (NodeContext.IsImmutable)
            {
                if (newState != State)
                    return new CustomRegionPatch(newState);
                else
                    return this;
            }
            else
            {
                State = newState;
                return this;
            }
        }

        public void Dispose()
        {
            (State as IDisposable)?.Dispose();
        }
    }
}
