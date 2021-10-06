using System;
using System.Collections.Generic;

namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Represents campaign with all data.
    /// </summary>
    internal class CampaignData
    {
        /// <summary>
        /// Campaign object name.
        /// </summary>
        public string CampaignName
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign object display name.
        /// </summary>
        public string CampaignDisplayName
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign object description.
        /// </summary>
        public string CampaignDescription
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign open from.
        /// </summary>
        public DateTime CampaignOpenFrom
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign open to.
        /// </summary>
        public DateTime CampaignOpenTo
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign UTM code.
        /// </summary>
        public string CampaignUTMCode
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign content inventory.
        /// </summary>
        public IEnumerable<Guid> CampaignContentInventory
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign email promotion.
        /// </summary>
        public Guid CampaignEmailPromotion
        {
            get;
            set;
        }


        /// <summary>
        /// Campaign conversions and campaign journey steps.
        /// </summary>
        public IEnumerable<CampaignConversionData> CampaignReportSetup
        {
            get;
            set;
        }
    }
}