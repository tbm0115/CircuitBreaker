namespace CircuitBreakerApp.Models
{
    public class CircuitBreakerInstance
    {
        /// <summary>
        /// Label that the user can describe the location of the breaker panel
        /// </summary>
        public string Label { get; set; } = "Garage";

        /// <summary>
        /// Collection of registered breakers
        /// </summary>
        public List<Breaker> Breakers { get; set; } = new List<Breaker>(20);

        public class Breaker
        {
            /// <summary>
            /// Label that the user can describe what is controlled by this switch
            /// </summary>
            public string Label { get; set; } = "N/A";

            /// <summary>
            /// How many amps this breaker supports. Ie. 15, 20, 30, 40, etc.
            /// </summary>
            public int Amperage { get; set; } = 0;

            public BreakerTypes Type { get; set; } = BreakerTypes.Unused;

        }
        public enum BreakerTypes
        {
            /// <summary>
            /// Means there is no switch at the panel position
            /// </summary>
            Unused,
            /// <summary>
            /// The most common, this is a single breaker switch for an individual circuit
            /// </summary>
            SinglePole,
            /// <summary>
            /// This is a single breaker with two switches for multiple circuits
            /// </summary>
            DoublePole,
            /// <summary>
            /// This is a single breaker spanning two breaker slots
            /// </summary>
            DoubleCircuit
        }
    }
}
