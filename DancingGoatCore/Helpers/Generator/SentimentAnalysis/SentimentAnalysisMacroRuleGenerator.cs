using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.DataEngine;
using CMS.FormEngine;
using CMS.MacroEngine;

namespace DancingGoat.Helpers.Generator
{
    public class SentimentAnalysisMacroRuleGenerator
    {
        private static readonly Guid MACRO_RULE_GUID = Guid.Parse("A8d3ba09-e3f0-4cf9-a995-ef85f4fa75b4");
        public const string MACRO_RULE_NAME = "LastFormSentimentAnalysisResult";

        public static void Generate()
        {
            var rule = MacroRuleInfo.Provider.Get(MACRO_RULE_NAME);
            if (rule == null)
            {
                rule = new MacroRuleInfo
                {
                    MacroRuleGUID = MACRO_RULE_GUID,
                    MacroRuleName = MACRO_RULE_NAME,
                    MacroRuleDisplayName = "Last form sentiment analysis result",
                    MacroRuleDescription = "Sample macro rule for sentiment analysis in marketing automation",
                    MacroRuleEnabled = true,
                    MacroRuleText = "Last form sentiment analysis result is {Sentiment}",
                    MacroRuleCondition = @"AutomationState.StateCustomData[""SENTIMENT_ANALYSIS_RESULT""] == {Sentiment}",
                    MacroRuleResourceName = "cms.onlinemarketing",
                    MacroRuleIsCustom = true,
                    MacroRuleParameters = GetMacroRuleParameters(),
                    MacroRuleAvailability = MacroRuleAvailabilityEnum.MainApplication,
                };

                rule.SetValue("MacroRuleRequiresContext", false);
                rule.Insert();
            }
        }


        private static string GetMacroRuleParameters()
        {
            var formInfo = new FormInfo();

            var fieldInfo = new FormFieldInfo
            {
                Name = "Sentiment",
                DataType = FieldDataType.Text,
                System = false,
                Visible = true,
                Size = 200,
                AllowEmpty = true,
                Guid = Guid.Parse("e0a7827e-2baa-4bca-9f97-1c571c4ae300")
            };

            fieldInfo.SetControlName(FormFieldControlName.DROPDOWNLIST);
            fieldInfo.SetPropertyValue(FormFieldPropertyEnum.FieldCaption, "Sentiment");
            fieldInfo.Settings["DisplayActualValueAsItem"] = false;
            fieldInfo.Settings["EditText"] = false;
            fieldInfo.Settings["Options"] = @"0;Positive
2;Negative
1;Neutral
3;Mixed";
            fieldInfo.Settings["SortItems"] = false;
            formInfo.AddFormItem(fieldInfo);

            return formInfo.GetXmlDefinition();
        }
    }
}
