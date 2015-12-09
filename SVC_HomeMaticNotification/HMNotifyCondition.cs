using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The HMNotifyCondition can be used to prevent some events of raising a notification 
    /// by setting up one of this conditions. As a condition you can choose a Homematic Device 
    /// or Homematic Variable. By setting the match value the the state you'd like not to 
    /// receive a message the condition will be evaluated first before sending out notifications.
    /// </summary>
    public class HMNotifyCondition
    {
        public enum ConditionType
        {
            Variable,
            Device
        }

        public ConditionType CType;
        public int IseId;
        public string DeviceAddress;
        public string ValueKey;
        public string ConditionMatchValue;

        public HMNotifyCondition(int varIseId, string varMatchValue)
        {
            CType = ConditionType.Variable;
            IseId = varIseId;
            ConditionMatchValue = varMatchValue;
        }

        public HMNotifyCondition(string devAddress, string devValueKey, string devMatchValue)
        {
            CType = ConditionType.Device;
            DeviceAddress = devAddress;
            ValueKey = devValueKey;
            ConditionMatchValue = devMatchValue;
        }

        public override string ToString()
        {
            return String.Format("Condition for {0} at {1} when matches '{2}'", 
                CType.ToString(), (CType == ConditionType.Device ? DeviceAddress + " (" + ValueKey + ")" : "IseId " + IseId.ToString()), ConditionMatchValue);
        }
    }
}
