using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSTracker.Helpers
{

    /// <summary>
    /// Class to control the state of the Arduino
    /// </summary>
    class StateManager
    {
        /// <summary>
        /// The LCD light state 
        /// </summary>
        public bool LightOn { get; set; }

        /// <summary>
        /// The LCD display state 
        /// </summary>
        public bool DisplayOn { get; set; }

        /// <summary>
        /// Initialize the state manager.
        /// </summary>
        public void Initialize()
        {
            LightOn = false;
            DisplayOn = false;
        }
    }
}
