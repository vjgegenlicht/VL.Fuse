using System;
using System.Collections;
using System.Collections.Generic;
using VL.Core;
using VL.Lib.Collections;

namespace Fuse.VL_API_Sketches.InterfaceApproach
{
    public struct BorderControlPointDescription
    {
        public string Name;
        public IVLTypeInfo TypeInfo;
        public int Index;
    }

    /// <summary>
    /// This represents the user patch inside the region
    /// You may create and manage several patch states by calling CreateRegionPatch
    /// </summary>
    public interface ICustomRegionPatch
    {
        /// <summary>
        /// Updates the patch state by calling what the user patched
        /// If the context is immutable it will return a new instance of the type when necessary
        /// </summary>
        /// <param name="inputs">The inputs from the inside perspective</param>
        /// <param name="outputs">The outputs from the inside perspective</param>
        /// <returns></returns>
        public ICustomRegionPatch Update(IEnumerable inputs, out Spread<object> outputs);
    }

    /// <summary>
    /// Represents the application of your region by the user, the values that flow into the region and outof. 
    /// It also allows you to instanciate what's inside: the patch of the user. 
    /// </summary>
    public interface ICustomRegion
    {
        public Spread<BorderControlPointDescription> Inputs { get; }
        /// <summary>
        /// The outputs from an outside perspective
        /// </summary>
        public Spread<BorderControlPointDescription> Outputs { get; }

        /// <summary>
        /// Retrieves the untouched inputs. 
        /// Your region might want to change some values before feeding it to the patch.
        /// </summary>
        public Spread<object> ReadInputs();

        /// <summary>
        /// After updating the custom region patch and altering the values you finally need to write the outputs.
        /// These may be different from the values that you got from the patch update call.
        /// </summary>
        /// <param name="outputs"></param>
        public void WriteOutputs(IEnumerable outputs);

        /// <summary>
        /// Create a patch state
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        public ICustomRegionPatch CreateRegionPatch(NodeContext Context, Spread<object> inputs, out Spread<object> outputs);
    }
}
