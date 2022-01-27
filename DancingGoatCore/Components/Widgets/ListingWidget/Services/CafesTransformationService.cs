using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Microsoft.Extensions.Localization;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get cafes transformation model and custom parametrization for page retriever.
    /// </summary>
    public class CafesTransformationService : BaseTransformationService
    {
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly ICountryRepository countryRepository;


        /// <summary>
        /// Creates an instance of <see cref="CafesTransformationService"/> class.
        /// </summary>
        /// <param name="attachmentUrlRetriever">Attachment URL retriever.</param>
        /// <param name="localizer">Represents an <see cref="IStringLocalizer"/> that provides localized strings.</param>
        /// <param name="countryRepository">Country repository.</param>
        public CafesTransformationService(IPageAttachmentUrlRetriever attachmentUrlRetriever, IStringLocalizer<SharedResources> localizer, ICountryRepository countryRepository)
        {
            this.attachmentUrlRetriever = attachmentUrlRetriever;
            this.localizer = localizer;
            this.countryRepository = countryRepository;
        }


        /// <summary>
        /// Returns Action for applying custom query parameters for selecting only company cafes.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        public override Action<DocumentQuery> GetCustomQueryParametrization(string transformationView)
        {
            return (query) => query.WhereTrue("CafeIsCompanyCafe");
        }


        /// <summary>
        /// Returns hydrated <see cref="CafesTransformationViewModel"/> for pages.
        /// </summary>
        /// <param name="pages">Pages to be listed in view model.</param>
        public override ITransformationViewModel GetModel(IEnumerable<TreeNode> pages)
        {
            if (pages == null)
            {
                return new CafesTransformationViewModel { Cafes = Enumerable.Empty<CafeViewModel>() };
            }
            var cafesList = pages.Select(cafe => CafeViewModel.GetViewModel(cafe as Cafe, countryRepository, localizer, attachmentUrlRetriever));
            return new CafesTransformationViewModel { Cafes = cafesList };
        }
    }
}
