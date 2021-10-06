namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Represents campaign conversion with all data.
    /// </summary>
    internal class CampaignConversionData
    {
        /// <summary>
        /// Campaign conversion name.
        /// </summary>
        public string ConversionName
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign conversion display name.
        /// </summary>
        public string ConversionDisplayName
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign conversion activity type code name.
        /// </summary>
        public string ConversionActivityType
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign conversion detail item ID. For example nodeID for <c>pagevisit</c> <see cref="ConversionActivityType"/>.
        /// </summary>
        public int? ConversionItemID
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign conversion order.
        /// </summary>
        public int ConversionOrder
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign conversion represents step in campaign journey, i.e. conversion required to reach desired conversion.
        /// </summary>
        public bool ConversionIsFunnelStep
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign conversion url.
        /// </summary>
        public string ConversionUrl
        {
            get;
            set;
        }
    }
}