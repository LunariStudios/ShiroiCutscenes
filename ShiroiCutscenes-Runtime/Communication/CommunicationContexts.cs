using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shiroi.Cutscenes.Communication {
    public interface ICommunicationDevice {
        string Name {
            get;
        }
    }

    public static class CommunicationDeviceUtility {
        public static Color32 GetColorFromName(this ICommunicationDevice device) {
            return GetColorFromName(device.Name);
        }

        public static Color32 GetColorFromName(string device) {
            var prop = new PropertyName(device).GetHashCode();
            var bytes = BitConverter.GetBytes(prop);
            return new Color32(bytes[0], bytes[1], bytes[2], bytes[3]);
        }
    }

    public interface IInputContext {
        IEnumerable<Input> GetInputs();
    }

    public interface IOutputContext {
        IEnumerable<Output> GetOutputs();
    }

    public static class CommunicationContextUtility {
        public static Input GetInput(this IInputContext context, string name) {
            return context.GetInputs().FirstOrDefault(input => input.Name == name);
        }

        public static Output GetOutput(this IOutputContext context, string name) {
            return context.GetOutputs().FirstOrDefault(input => input.Name == name);
        }
    }
}