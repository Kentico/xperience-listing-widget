using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Activities;
using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.WebAnalytics;
using CMS.WebAnalytics.Internal;

namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Helper methods for campaign generation.
    /// </summary>
    internal static class CampaignDataGeneratorHelpers
    {
        /// <summary>
        /// Removes pre-generated activities.
        /// </summary>
        /// <param name="campaignUTMCode">UTM code of the campaign.</param>
        public static void DeleteOldActivities(string campaignUTMCode)
        {
            var oldActivities = ActivityInfo.Provider.Get()
                                                    .WhereStartsWith("ActivityTitle", "GeneratedActivity_")
                                                    .WhereEquals("ActivityCampaign", campaignUTMCode)
                                                    .ToList();

            oldActivities.ForEach(ActivityInfo.Provider.Delete);
        }


        /// <summary>
        /// Generates activities for campaign.
        /// </summary>
        /// <param name="hashService">Provides hash computations for activity URLs.</param>
        /// <param name="activityDataParameters">Contains count to specify how many activities should be generated with given utm content and utm source</param>
        /// <param name="campaign">Campaign for which are activities generated.</param>
        /// <param name="type">Activity type (<see cref="PredefinedActivityType"/>).</param>
        /// <param name="contactsIDs">Represents IDs of all contacts for generating activities.</param>
        /// <param name="conversionItemID">Conversion item ID.</param>
        public static void GenerateActivities(IActivityUrlHashService hashService, IEnumerable<ActivityDataParameters> activityDataParameters, CampaignInfo campaign, string type, ContactsIDData contactsIDs, int conversionItemID = 0, string activityUrl = null)
        {
            var nodeID = 0;
            var itemID = 0;

            switch (type)
            {
                case PredefinedActivityType.PAGE_VISIT:
                    nodeID = conversionItemID;
                    break;

                default:
                    itemID = conversionItemID;
                    break;
            }

            foreach (var activityDataParameter in activityDataParameters)
            {
                for (var i = 0; i < activityDataParameter.Count; i++)
                {
                    GenerateFakeActivity(hashService, campaign.CampaignUTMCode, type, activityDataParameter, nodeID, itemID, campaign.CampaignSiteID, contactsIDs.GetNextContactID(), activityUrl);
                }
            }
        }


        private static void GenerateFakeActivity(IActivityUrlHashService hashService, string campaignUTMcode, string type, ActivityDataParameters activityDataParameter, int nodeID, int itemID, int siteID, int contactID, string activityUrl = null)
        {
            var activity = new ActivityInfo
            {
                ActivitySiteID = siteID,
                ActivityContactID = contactID,
                ActivityCampaign = campaignUTMcode,
                ActivityType = type,
                ActivityNodeID = nodeID,
                ActivityItemID = itemID,
                ActivityURL = activityUrl,
                ActivityURLHash = activityUrl != null ? hashService.GetActivityUrlHash(activityUrl) : 0,
                ActivityUTMSource = activityDataParameter.UtmSource,
                ActivityUTMContent = activityDataParameter.UtmContent,
                ActivityTitle = "GeneratedActivity_" + type + "_" + contactID
            };

            ActivityInfo.Provider.Set(activity);

        }


        /// <summary>
        /// Creates conversion for the campaign.
        /// </summary>
        /// <param name="campaignId">ID of the campaign.</param>
        /// <param name="conversionData">Campaign conversion data for generating.</param>
        public static void CreateConversion(int campaignId, CampaignConversionData conversionData)
        {
            var conversion = CampaignConversionInfo.Provider.Get()
                                                           .WhereEquals("CampaignConversionCampaignID", campaignId)
                                                           .WhereEquals("CampaignConversionActivityType", conversionData.ConversionActivityType)
                                                           .WhereEquals("CampaignConversionItemID", conversionData.ConversionItemID)
                                                           .WhereEquals("CampaignConversionURL", conversionData.ConversionUrl)
                                                           .WhereEquals("CampaignConversionIsFunnelStep", conversionData.ConversionIsFunnelStep)
                                                           .ToList().FirstOrDefault();

            if (conversion != null)
            {
                return;
            }

            conversion = new CampaignConversionInfo
            {
                CampaignConversionName = conversionData.ConversionName,
                CampaignConversionDisplayName = conversionData.ConversionDisplayName,
                CampaignConversionCampaignID = campaignId,
                CampaignConversionActivityType = conversionData.ConversionActivityType,
                CampaignConversionItemID = conversionData.ConversionItemID.GetValueOrDefault(),
                CampaignConversionIsFunnelStep = conversionData.ConversionIsFunnelStep,
                CampaignConversionOrder = conversionData.ConversionOrder,
                CampaignConversionURL = conversionData.ConversionUrl

            };

            CampaignConversionInfo.Provider.Set(conversion);
        }


        /// <summary>
        /// Adds the specific newsletter to campaign. If the newsletter does not exists it is created.
        /// </summary>
        /// <param name="campaign">Campaign where the newsletter is added.</param>
        /// <param name="issueGuid">Guid of the newsletter issue.</param>
        public static void AddNewsletterAsset(CampaignInfo campaign, Guid issueGuid)
        {
            var issue = ProviderHelper.GetInfoByGuid(PredefinedObjectType.NEWSLETTERISSUE, issueGuid, campaign.CampaignSiteID);
            if (issue == null)
            {
                return;
            }

            var newsletter = ProviderHelper.GetInfoById(PredefinedObjectType.NEWSLETTER, issue.GetValue("IssueNewsletterID").ToInteger(0));
            if (newsletter == null)
            {
                return;
            }

            var source = newsletter.GetValue("NewsletterDisplayName").ToString().Replace(' ', '_').ToLowerInvariant();

            issue.SetValue("IssueUseUTM", true);
            issue.SetValue("IssueUTMCampaign", campaign.CampaignUTMCode);
            issue.SetValue("IssueUTMSource", source);
            issue.Update();

            CreateNewsletterAsset(campaign.CampaignID, issueGuid);
        }


        /// <summary>
        /// Creates newsletter if it does not exist.
        /// </summary>
        /// <param name="campaignId">ID of the campaign.</param>
        /// <param name="nodeGuid">Newsletter node guid.</param>
        private static void CreateNewsletterAsset(int campaignId, Guid nodeGuid)
        {
            var campaignAsset = CampaignAssetInfo.Provider.Get()
                                                         .WhereEquals("CampaignAssetCampaignID", campaignId)
                                                         .WhereEquals("CampaignAssetAssetGuid", nodeGuid)
                                                         .ToList().FirstOrDefault();

            if (campaignAsset != null)
            {
                return;
            }

            campaignAsset = new CampaignAssetInfo
            {
                CampaignAssetType = PredefinedObjectType.NEWSLETTERISSUE,
                CampaignAssetCampaignID = campaignId,
                CampaignAssetAssetGuid = nodeGuid,
            };

            CampaignAssetInfo.Provider.Set(campaignAsset);
        }


        /// <summary>
        /// Adds the specific asset to campaign. If the asset does not exist it is created.
        /// </summary>
        /// <param name="campaignId">ID of the campaign.</param>
        /// <param name="assetUrlGuid">Asset URL GUID.</param>
        public static void AddPageAsset(int campaignId, Guid assetUrlGuid)
        {
            var campaignAsset = CampaignAssetInfo.Provider.Get()
                                                         .WhereEquals("CampaignAssetCampaignID", campaignId)
                                                         .WhereEquals("CampaignAssetAssetGuid", assetUrlGuid)
                                                         .ToList().FirstOrDefault();

            /* If the assest already exists, then do not create it. */
            if (campaignAsset != null)
            {
                return;
            }

            campaignAsset = new CampaignAssetInfo
            {
                CampaignAssetType = CampaignAssetUrlInfo.OBJECT_TYPE,
                CampaignAssetCampaignID = campaignId,
                CampaignAssetAssetGuid = assetUrlGuid,
            };

            CampaignAssetInfo.Provider.Set(campaignAsset);

            var campaignAssetUrl = CampaignAssetUrlInfo.Provider.Get()
                .WhereEquals("CampaignAssetUrlGuid", assetUrlGuid)
                .ToList().FirstOrDefault();

            if (campaignAssetUrl != null)
            {
                return;
            }

            var page = GetDocument(ContentItemIdentifiers.LANDINGPAGE_COFFEESAMPLES);
            campaignAssetUrl = new CampaignAssetUrlInfo
            {
                CampaignAssetUrlCampaignAssetID = campaignAsset.CampaignAssetID,
                CampaignAssetUrlGuid = assetUrlGuid,
                CampaignAssetUrlTarget = DocumentURLProvider.GetUrl(page).Replace("~", string.Empty),
                CampaignAssetUrlPageTitle = "Coffee samples"
            };

            CampaignAssetUrlInfo.Provider.Set(campaignAssetUrl);

        }


        /// <summary>
        /// Gets the document according to path.
        /// </summary>
        /// <param name="path">Path of the document.</param>
        /// <returns>Document.</returns>
        public static TreeNode GetDocument(string path)
        {
            return DocumentHelper.GetDocuments()
                                 .All()
                                 .Culture("en-US")
                                 .Path(path)
                                 .OnCurrentSite()
                                 .ToList().First();
        }
    }
}