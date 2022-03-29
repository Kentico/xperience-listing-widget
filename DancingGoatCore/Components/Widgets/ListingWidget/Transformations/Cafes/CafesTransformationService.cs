using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Kentico.Xperience.ListingWidget;

using Microsoft.Extensions.Localization;

namespace DancingGoat.Widgets
{
    /// <inheritdoc/>
    public class CafesTransformationService : BaseTransformationService
    {
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly ICountryRepository countryRepository;


        /// <inheritdoc/>
        public override string PageType { get; } = Cafe.CLASS_NAME;


        /// <inheritdoc/>
        public override IEnumerable<Transformation> Transformations { get; } = new List<Transformation>
        {
            new Transformation
            {
                Name = "Our cafes",
                View = "~/Components/Widgets/ListingWidget/Transformations/Cafes/_OurCafes.cshtml",
                Description = "Transformation displays our cafes in 2 column grid.",
            }
        };


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
                return new CafesTransformationViewModel();
            }
            var cafesList = pages.Select(cafe => CafeViewModel.GetViewModel(cafe as Cafe, countryRepository, localizer, attachmentUrlRetriever));
            return new CafesTransformationViewModel { Cafes = cafesList };
        }
    }
}
