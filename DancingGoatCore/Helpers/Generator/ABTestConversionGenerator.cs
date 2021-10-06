using System;
using System.Text;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.OnlineMarketing;
using CMS.SiteProvider;
using CMS.WebAnalytics;

namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Generates data for the '/Home' page in 'en-us' culture on the current site.
    /// </summary>
    public class ABTestConversionGenerator
    {
        // Alias path and culture of the A/B tested page
        public const string ALIAS_PATH = ContentItemIdentifiers.HOME;
        public const string CULTURE_CODE = "en-us";

        // Code name of the conversion.
        private const string ABCONVERSION = "abconversion";

        // Code name of the recurring session conversion.
        private const string ABSESSIONCONVERSION_RECURRING = "absessionconversionrecurring";


        private readonly int[] mPurchaseFirstConversionsForOriginalPage = { 69, 43, 38, 27, 15, 39, 5, 4, 4, 2, 1, 1, 1, 3, 3 };
        private readonly int[] mPurchaseRecurringConversionsForOriginalPage = { 1, 2, 2, 1, 3, 3, 1, 2, 1, 3, 3, 3, 3, 1, 2 };
        private readonly int[] mPurchaseConversionsForOriginalPage = { 35, 45, 40, 30, 16, 9, 5, 5, 4, 2, 3, 1, 1, 4, 4 };
        private readonly int[] mPurchaseFirstVisitorsForOriginalPage = { 96, 113, 96, 69, 59, 45, 21, 14, 12, 8, 6, 6, 5, 3, 3 };
        private readonly int[] mPurchaseReturningVisitorsForOriginalPage = { 27, 33, 29, 33, 26, 26, 23, 29, 30, 39, 36, 39, 44, 47, 45 };

        private readonly int[] mPageVisitFirstConversionsForOriginalPage = { 65, 78, 90, 50, 37, 48, 17, 10, 7, 5, 4, 2, 1, 1, 5 };
        private readonly int[] mPageVisitRecurringConversionsForOriginalPage = { 5, 6, 6, 4, 1, 1, 4, 7, 6, 2, 1, 4, 4, 8, 10 };
        private readonly int[] mPageVisitConversionsForOriginalPage = { 44, 53, 48, 37, 24, 14, 11, 9, 7, 6, 8, 4, 4, 1, 1 };

        private readonly int[] mPurchaseFirstConversionsForBPage = { 57, 69, 60, 42, 28, 17, 11, 5, 3, 2, 2, 1, 1, 1, 1 };
        private readonly int[] mPurchaseRecurringConversionsForBPage = { 35, 70, 80, 27, 15, 9, 5, 4, 4, 2, 4, 1, 5, 2, 2 };
        private readonly int[] mPurchaseConversionsForBPage = { 58, 71, 62, 43, 28, 17, 12, 7, 4, 2, 2, 1, 1, 2, 3 };
        private readonly int[] mPurchaseFirstVisitorsForBPage = { 98, 114, 108, 78, 65, 57, 33, 18, 12, 8, 8, 6, 6, 5, 3 };
        private readonly int[] mPurchaseReturningVisitorsForBPage = { 26, 29, 35, 38, 29, 23, 23, 26, 27, 30, 39, 33, 50, 53, 50 };

        private readonly int[] mPageFirstConversionsForBPage = { 88, 64, 50, 33, 17, 45, 8, 6, 5, 3, 1, 1, 1, 1, 1 };
        private readonly int[] mPageRecurringConversionsForBPage = { 45, 82, 91, 35, 20, 13, 8, 6, 4, 2, 1, 1, 1, 1, 1 };
        private readonly int[] mPageConversionsForBPage = { 72, 84, 77, 62, 39, 26, 18, 11, 8, 6, 2, 1, 1, 2, 1 };


        private readonly IABTestManager mTestManager;
        private readonly IAnalyticsLogger mLogger;


        private ABTestInfo ABTest { get; set; }


        private IABTestVariant OriginalVariant { get; set; }


        private IABTestVariant BVariant { get; set; }


        public string PageVisitConversionName { get; private set; }


        /// <summary>
        /// Creates the instance of Data Generator and Initializes existing test definition
        /// </summary>
        /// <param name="testManager"></param>
        public ABTestConversionGenerator(IABTestManager testManager, IAnalyticsLogger logger)
        {
            mTestManager = testManager ?? throw new ArgumentNullException(nameof(testManager));
            mLogger = logger ?? throw new ArgumentNullException(nameof(logger));

            Init();
        }


        /// <summary>
        /// Initializes and class properties used in data generation
        /// </summary>
        private void Init()
        {
            var testedNode = GetTestedNode();
            AssignABTest(testedNode);
            AssignVariants(testedNode);
            AssignPageVisitConversions();
        }


        /// <summary>
        /// Get's existing tree node for A/B test
        /// </summary>
        private TreeNode GetTestedNode()
        {
            // Get the A/B tested 'Home' page
            var testedNode = DocumentHelper.GetDocument(new NodeSelectionParameters
            {
                SiteName = SiteContext.CurrentSiteName,
                AliasPath = ALIAS_PATH,
                CultureCode = CULTURE_CODE,
            }, new TreeProvider());

            if (testedNode == null)
            {
                throw new ABTestConversionGeneratorException($"Page identified by the alias path '{ALIAS_PATH}' in the '{CULTURE_CODE}' culture does not exist.");
            }

            return testedNode;
        }


        /// <summary>
        /// Gets existing not finished A/B test and assign it to class property
        /// </summary>
        /// <param name="testedNode">TreeNode for A/B Test</param>
        private void AssignABTest(TreeNode testedNode)
        {
            // Get prepared A/B test
            ABTest = mTestManager.GetABTestWithoutWinner(testedNode);

            if (ABTest == null)
            {
                throw new ABTestConversionGeneratorException($"A/B test for the '{testedNode.NodeAliasPath}' page does not exist.");
            }
        }


        /// <summary>
        /// Gets existing variants for A/B test and assign 2 of them for later data generation
        /// </summary>
        /// <param name="testedNode">TreeNode for A/B Test</param>
        private void AssignVariants(TreeNode testedNode)
        {
            var variants = mTestManager.GetVariants(testedNode);

            foreach (var variant in variants)
            {
                if (variant.IsOriginal)
                {
                    OriginalVariant = variant;
                }
                else
                {
                    BVariant = variant;
                }
            }

            if (OriginalVariant == null || BVariant == null)
            {
                throw new ABTestConversionGeneratorException($"Page '{testedNode.NodeAliasPath}' does not contain A/B variants for which the data are to be generated.");
            }
        }


        /// <summary>
        /// Checks existing test for required Conversions.
        /// If either of 2 required Conversions are not defined, created then and save.
        /// </summary>
        private void AssignPageVisitConversions()
        {
            var modified = false;

            // Page visit conversion
            if (!ABTest.ABTestConversionConfiguration.TryGetConversionByOriginalName(ABTestConversionNames.PAGE_VISIT, out ABTestConversion pageVisitConversion))
            {
                pageVisitConversion = new ABTestConversion(ABTestConversionNames.PAGE_VISIT, "/page-visit/uri");

                modified = true;
                ABTest.ABTestConversionConfiguration.AddConversion(pageVisitConversion);
            }

            PageVisitConversionName = pageVisitConversion.ConversionName;

            // Purchase conversion
            if (!ABTest.ABTestConversionConfiguration.TryGetConversion(ABTestConversionNames.PURCHASE, out _))
            {
                var purchaseConversion = new ABTestConversion(ABTestConversionNames.PURCHASE);

                modified = true;
                ABTest.ABTestConversionConfiguration.AddConversion(purchaseConversion);
            }

            // If any of the conversions were created, then save resulting test to DB
            if (modified)
            {
                ABTestInfo.Provider.Set(ABTest);
            }
        }


        /// <summary>
        /// Starts A/B test and generates conversions and visit logs for A/B test.
        /// </summary>
        public void StartTestAndGenerateData()
        {
            DateTime from = DateTime.Now.AddMonths(-1);
            DateTime to = DateTime.Now;

            // Starts A/B test
            ABTest.ABTestOpenFrom = from;
            ABTestInfo.Provider.Set(ABTest);

            // Clears already logged conversions and visits
            ClearStatisticsData();

            for (DateTime startTime = from; startTime < to.AddDays(1); startTime = startTime.AddDays(1))
            {
                int daysFromStart = (startTime - from).Days;

                LogABConversionHits(daysFromStart, startTime);
                LogABVisitHits(daysFromStart, startTime);
            }
        }


        /// <summary>
        /// Removes all previously logged data related to the Dancing Goat A/B test.
        /// </summary>
        private void ClearStatisticsData()
        {
            var where = new WhereCondition().WhereContains("StatisticsCode", ABTest.ABTestName);

            StatisticsInfoProvider.RemoveAnalyticsData(DateTimeHelper.ZERO_TIME, DateTimeHelper.ZERO_TIME, SiteContext.CurrentSiteID, where.ToString(true));
        }


        /// <summary>
        /// Performs logging of A/B conversion hits.
        /// </summary>
        /// <param name="daysFromStart">Determines for which day should logging be performed (selects values from the data arrays).</param>
        private void LogABConversionHits(int daysFromStart, DateTime logDate)
        {
            LogConversions(OriginalVariant, PageVisitConversionName, daysFromStart, mPageVisitFirstConversionsForOriginalPage, mPageVisitRecurringConversionsForOriginalPage, mPageVisitConversionsForOriginalPage, logDate);
            LogConversions(BVariant, PageVisitConversionName, daysFromStart, mPageFirstConversionsForBPage, mPageRecurringConversionsForBPage, mPageConversionsForBPage, logDate);

            LogConversions(OriginalVariant, ABTestConversionNames.PURCHASE, daysFromStart, mPurchaseFirstConversionsForOriginalPage, mPurchaseRecurringConversionsForOriginalPage, mPurchaseConversionsForOriginalPage, logDate);
            LogConversions(BVariant, ABTestConversionNames.PURCHASE, daysFromStart, mPurchaseFirstConversionsForBPage, mPurchaseRecurringConversionsForBPage, mPurchaseConversionsForBPage, logDate);
        }


        /// <summary>
        /// Performs logging of A/B visit hits.
        /// </summary>
        /// <param name="daysFromStart">Determines for which day should logging be performed (selects values from the data arrays).</param>
        private void LogABVisitHits(int daysFromStart, DateTime logDate)
        {
            LogHit("abvisitfirst", OriginalVariant, ALIAS_PATH, mPurchaseFirstVisitorsForOriginalPage, daysFromStart, logDate);
            LogHit("abvisitreturn", OriginalVariant, ALIAS_PATH, mPurchaseReturningVisitorsForOriginalPage, daysFromStart, logDate);

            LogHit("abvisitfirst", BVariant, ALIAS_PATH, mPurchaseFirstVisitorsForBPage, daysFromStart, logDate);
            LogHit("abvisitreturn", BVariant, ALIAS_PATH, mPurchaseReturningVisitorsForBPage, daysFromStart, logDate);
        }


        /// <summary>
        /// Performs logging of A/B conversions.
        /// </summary>
        private void LogConversions(IABTestVariant ABTestVariant, string conversionName, int daysFromStart, int[] firstConversions, int[] recurringConversions, int[] conversions, DateTime logDate)
        {
            LogHit(ABTestConstants.ABSESSIONCONVERSION_FIRST, ABTestVariant, conversionName, firstConversions, daysFromStart, logDate);
            LogHit(ABSESSIONCONVERSION_RECURRING, ABTestVariant, conversionName, recurringConversions, daysFromStart, logDate);
            LogHit(ABCONVERSION, ABTestVariant, conversionName, conversions, daysFromStart, logDate);
        }


        /// <summary>
        /// Performs logging of the hit.
        /// </summary>
        private void LogHit(string type, IABTestVariant variant, string conversionName, int[] variantData, int daysFromStart, DateTime logDate)
        {
            var conversionValueMultiplier = GetConversionValueMultiplier(variant, conversionName);

            var name = $"{type};{ABTest.ABTestName};{variant.Guid}";
            var hits = variantData[daysFromStart % variantData.Length];
            var data = new AnalyticsData(SiteContext.CurrentSiteID, conversionName, hits, hits * conversionValueMultiplier, CULTURE_CODE, uri: new Uri("http://domain.com"));

            mLogger.LogCustomAnalytics(name, data, logDate);
        }


        /// <summary>
        /// Return a value to be used for multiplying conversion value.
        /// </summary>
        /// <remarks>
        /// Only the value for purchase is multiplied as page visit has a value that equals to 1.
        /// Current date time is used to add more variance to the conversion value, so that in the report when
        /// 'Average conversion value' is used we have different values per day.
        /// Variant B always gets bigger multiplier so that it wins in every statistic.
        /// </remarks>
        private decimal GetConversionValueMultiplier(IABTestVariant variant, string conversionName)
        {
            decimal conversionValueMultiplier = 1m;
            if (conversionName.Equals(ABTestConversionNames.PURCHASE, StringComparison.OrdinalIgnoreCase))
            {
                if (variant.IsOriginal)
                {
                    conversionValueMultiplier = DateTime.Now.Millisecond / 2;
                }
                else
                {
                    conversionValueMultiplier = DateTime.Now.Millisecond * 2;
                }
            }

            return conversionValueMultiplier;
        }
    }


    /// <summary>
    /// Special exception to throw when no suitable existing A/B test is present
    /// </summary>
    public class ABTestConversionGeneratorException : Exception
    {
        public ABTestConversionGeneratorException()
            : base()
        {
        }

        public ABTestConversionGeneratorException(string message)
            : base(message)
        {
        }

        public ABTestConversionGeneratorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public string DisplayMessage
        {
            get
            {
                var builder = new StringBuilder();

                builder.AppendLine(Message);
                builder.AppendFormat("Please make sure there exists '{0}' node in '{1}' culture currently under A/B test with at least one alternative variant.",
                    ABTestConversionGenerator.ALIAS_PATH,
                    ABTestConversionGenerator.CULTURE_CODE);

                return builder.ToString();
            }
        }
    }
}